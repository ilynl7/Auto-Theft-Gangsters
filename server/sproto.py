"""Sproto binary codec — port of the client's SprotoPack / SprotoTypeSerialize /
SprotoTypeDeserialize from Assembly-CSharp.dll (Auto Theft Gangsters v1.19).

Wire layers (top to bottom):
    frame     : 2-byte BIG-endian length + sproto_pack(payload)
    payload   : sproto_object(Package{type?, session?}) || sproto_object(body)
    object    : sproto field stream (see encode_object / decode_object)

Object format (little-endian throughout):
    u16 fn | fn * u16 header records | body
  * header record (word & 1) == 1  -> tag skip: tag += word // 2
  * header record (word & 1) == 0  -> field present, value = word // 2 - 1
    * small int (fits in value): encoded in the header record itself
    * bool: same as small int (0/1)
    * large int: value record 0, body: u32 size (4|8) + int
    * string: value record 0, body: u32 byteLen + UTF-8 bytes
    * nested object: value record 0, body: u32 byteLen + nested object stream
    * arrays: value record 0, body: u32 totalByteLen + elements
      (int arrays carry a 1-byte element width of 4 or 8 first;
       each object/string element is individually length-prefixed)
"""

import struct

sizeof_header = 2
sizeof_field = 2

MAX_ONE_PACK_BYTE_SIZE = 16384


class SprotoError(Exception):
    pass


# ---------------------------------------------------------------------------
# SprotoPack — zero-compression applied to the raw sproto stream pre-framing
# ---------------------------------------------------------------------------

def sproto_pack(data: bytes) -> bytes:
    """Port of SprotoPack.pack: 8-byte-group zero-run compression.

    * all-zero group            -> single 0x00 byte
    * group with all 8 bytes set -> literal segment: 0xFF, size-byte, 8 bytes
      (consecutive literal segments merge into one block; size-byte = n-1
       where n is the number of 8-byte segments, max 256 per block)
    * 6-7 nonzero bytes with a pending literal run merge into that run
    * otherwise                 -> bitmask byte (LSB-first) + nonzero bytes
    """
    out = bytearray()
    n = len(data)
    ff_n = 0        # segments accumulated in the current literal run
    ff_pos = -1     # index of the 0xFF marker of the current run

    i = 0
    while i < n:
        seg = bytearray(data[i:i + 8])
        if len(seg) < 8:
            seg.extend(b"\x00" * (8 - len(seg)))

        nonzero = sum(1 for b in seg if b)
        if nonzero == 0:
            out.append(0x00)
            i += 8
            continue

        if nonzero == 8 or (nonzero >= 6 and ff_n > 0):
            # literal segment; merge into / start a 0xFF run
            if ff_n == 0:
                ff_pos = len(out)
                out.extend(b"\xff\x00")
            out.extend(seg)
            ff_n += 1
            if ff_n == 256:
                # close the block (max 256 segments) and start a new one
                out[ff_pos + 1] = 0xFF  # 256 - 1
                ff_n = 0
            i += 8
            continue

        if ff_n > 0:
            # close pending literal run before a bitmask group
            out[ff_pos + 1] = ff_n - 1
            ff_n = 0

        mask = 0
        chunk = bytearray()
        for bit in range(8):
            if seg[bit]:
                mask |= 1 << bit
                chunk.append(seg[bit])
        out.append(mask)
        out.extend(chunk)
        i += 8

    if ff_n > 0:
        out[ff_pos + 1] = ff_n - 1

    return bytes(out)


def sproto_unpack(data: bytes) -> bytes:
    """Port of SprotoPack.unpack."""
    out = bytearray()
    i = 0
    n = len(data)
    while i < n:
        b = data[i]
        i += 1
        if b == 0xFF:
            if i >= n:
                raise SprotoError("invalid unpack stream")
            num = (data[i] + 1) * 8
            i += 1
            if i + num > n:
                raise SprotoError("invalid unpack stream")
            out.extend(data[i:i + num])
            i += num
        else:
            for bit in range(8):
                if (b >> bit) & 1:
                    if i >= n:
                        raise SprotoError("invalid unpack stream")
                    out.append(data[i])
                    i += 1
                else:
                    out.append(0)
    return bytes(out)


# ---------------------------------------------------------------------------
# Frame layer
# ---------------------------------------------------------------------------

def frame_encode(payload: bytes) -> bytes:
    packed = sproto_pack(payload)
    return struct.pack(">H", len(packed)) + packed


class FrameDecoder:
    """Incremental TCP frame decoder (2-byte BE length + packed payload)."""

    def __init__(self) -> None:
        self._buf = bytearray()

    def feed(self, data: bytes):
        self._buf.extend(data)
        frames = []
        while len(self._buf) >= 2:
            (length,) = struct.unpack_from(">H", self._buf, 0)
            if length <= 0:
                raise SprotoError("zero-length frame")
            if len(self._buf) < 2 + length:
                break
            packed = bytes(self._buf[2:2 + length])
            del self._buf[:2 + length]
            frames.append(sproto_unpack(packed))
        return frames


# ---------------------------------------------------------------------------
# Object layer
# ---------------------------------------------------------------------------

def encode_object(fields) -> bytes:
    """Encode {tag: value} where value is int | bool | str | bytes.

    `bytes` values are pre-encoded nested objects or arrays (see the
    encode_*_array helpers); they are written length-prefixed into the body.
    """
    header = bytearray()
    body = bytearray()
    last_tag = -1

    def write_record(word: int) -> None:
        header.extend(struct.pack("<H", word))

    def write_tag(tag: int, value: int) -> None:
        nonlocal last_tag
        skip = tag - last_tag - 1
        if skip > 0:
            word = (skip - 1) * 2 + 1
            if word > 0xFFFF:
                raise SprotoError("tag gap too big")
            write_record(word)
        write_record(value)
        last_tag = tag

    for tag in sorted(fields):
        value = fields[tag]
        if isinstance(value, bool):
            write_tag(tag, ((1 if value else 0) + 1) * 2)
        elif isinstance(value, int):
            # NOTE: the client's write_integer casts to uint before the
            # small-int check, so negatives are NEVER header-encoded.
            if 0 <= value < 32767:
                write_tag(tag, (value + 1) * 2)
            elif -2147483648 <= value <= 0x7FFFFFFF:
                body.extend(struct.pack("<Ii", 4, value))
                write_tag(tag, 0)
            else:
                body.extend(struct.pack("<Iq", 8, value))
                write_tag(tag, 0)
        elif isinstance(value, str):
            raw = value.encode("utf-8")
            body.extend(struct.pack("<I", len(raw)))
            body.extend(raw)
            write_tag(tag, 0)
        elif isinstance(value, (bytes, bytearray)):
            body.extend(struct.pack("<I", len(value)))
            body.extend(value)
            write_tag(tag, 0)
        else:
            raise SprotoError("unsupported field type %r" % type(value))

    out = bytearray()
    out.extend(struct.pack("<H", len(header) // sizeof_field))
    out.extend(header)
    out.extend(body)
    return bytes(out)


class Decoder:
    """Sequential field reader — port of SprotoTypeDeserialize."""

    def __init__(self, data: bytes, offset: int = 0, length: int = None) -> None:
        if length is None:
            length = len(data) - offset
        self.data = data
        self.end = offset + length
        self.offset = offset
        if length >= 2:
            self.fn = struct.unpack_from("<H", data, offset)[0]
        else:
            self.fn = 0
        self.begin_data = offset + sizeof_header + self.fn * sizeof_field
        if self.begin_data > self.end:
            raise SprotoError("invalid decode header")
        self.header_pos = offset + sizeof_header
        self.tag = -1
        self.value = 0
        self.pos = self.begin_data   # body read cursor

    # -- iteration -----------------------------------------------------
    def next_tag(self):
        """Advance to the next present field. Returns tag or None at end."""
        while self.header_pos < self.begin_data:
            self.tag += 1
            word = struct.unpack_from("<H", self.data, self.header_pos)[0]
            self.header_pos += sizeof_field
            if word & 1:
                self.tag += word // 2
            else:
                self.value = word // 2 - 1
                return self.tag
        return None

    def skip_field(self) -> None:
        """Skip the current field's body data (unknown tags)."""
        if self.value < 0:
            n = self._read_dword()
            self.pos += n

    # -- value readers ---------------------------------------------------
    def read_integer(self) -> int:
        if self.value >= 0:
            return self.value
        size = self._read_dword()
        if size == 4:
            v = struct.unpack_from("<i", self.data, self.pos)[0]
            self.pos += 4
            return v
        if size == 8:
            v = struct.unpack_from("<q", self.data, self.pos)[0]
            self.pos += 8
            return v
        raise SprotoError("read invalid integer size (%d)" % size)

    def read_boolean(self) -> bool:
        if self.value < 0:
            raise SprotoError("read invalid boolean")
        return self.value != 0

    def read_string(self) -> str:
        n = self._read_dword()
        raw = self.data[self.pos:self.pos + n]
        self.pos += n
        return raw.decode("utf-8")

    def read_raw(self) -> bytes:
        """Read a length-prefixed blob (nested object or array)."""
        n = self._read_dword()
        raw = self.data[self.pos:self.pos + n]
        self.pos += n
        return raw

    def read_integer_list(self):
        total = self._read_dword()
        if total == 0:
            return []
        width = self.data[self.pos]
        self.pos += 1
        total -= 1
        values = []
        if width == 4:
            if total % 4:
                raise SprotoError("bad int array size")
            for _ in range(total // 4):
                values.append(struct.unpack_from("<i", self.data, self.pos)[0])
                self.pos += 4
        elif width == 8:
            if total % 8:
                raise SprotoError("bad long array size")
            for _ in range(total // 8):
                values.append(struct.unpack_from("<q", self.data, self.pos)[0])
                self.pos += 8
        else:
            raise SprotoError("bad int array width %d" % width)
        return values

    def read_string_list(self):
        total = self._read_dword()
        values = []
        while total > 0:
            n = struct.unpack_from("<I", self.data, self.pos)[0]
            self.pos += 4
            total -= 4
            raw = self.data[self.pos:self.pos + n]
            self.pos += n
            total -= n
            values.append(raw.decode("utf-8"))
        return values

    def read_object_list(self):
        """Read an object array blob -> list of raw element bytes."""
        total = self._read_dword()
        elems = []
        while total > 0:
            n = struct.unpack_from("<I", self.data, self.pos)[0]
            self.pos += 4
            total -= 4
            elems.append(bytes(self.data[self.pos:self.pos + n]))
            self.pos += n
            total -= n
        return elems

    def _read_dword(self) -> int:
        d = struct.unpack_from("<I", self.data, self.pos)[0]
        self.pos += 4
        return d


def decode_fields(data: bytes, offset: int = 0, length: int = None) -> dict:
    """Decode an object stream into {tag: python value}.

    Header-encoded ints/bools decode natively; body fields are read as
    length-prefixed blobs and returned as `str` when they are valid UTF-8
    (strings), otherwise as raw `bytes` (nested objects / arrays — decode
    those with Decoder.read_object_list / decode_fields as appropriate).
    """
    dec = Decoder(data, offset, length)
    fields = {}
    while (tag := dec.next_tag()) is not None:
        if dec.value >= 0:
            fields[tag] = dec.value  # small int or bool encoded in header
        else:
            raw = dec.read_raw()
            try:
                fields[tag] = raw.decode("utf-8")
            except UnicodeDecodeError:
                fields[tag] = raw
    return fields


# ---------------------------------------------------------------------------
# Pre-encoded composite helpers (used as `bytes` values in encode_object)
# ---------------------------------------------------------------------------

def decode_typed(data: bytes, spec: dict = None, offset: int = 0,
                 length: int = None) -> dict:
    """Decode an object stream using per-tag type hints.

    spec maps sproto tag -> kind:
        'i'  integer            'b'  boolean
        's'  string             'o'  nested object (raw bytes)
        'ia' integer array      'sa' string array     'oa' object array
    Tags without a hint fall back to the untyped heuristic.
    """
    dec = Decoder(data, offset, length)
    spec = spec or {}
    fields = {}
    while (tag := dec.next_tag()) is not None:
        kind = spec.get(tag)
        if dec.value >= 0 and kind in (None, "i", "b"):
            if kind == "b":
                fields[tag] = dec.value != 0
            else:
                fields[tag] = dec.value
            continue
        if kind == "i":
            fields[tag] = dec.read_integer()
        elif kind == "b":
            fields[tag] = dec.read_boolean()
        elif kind == "s":
            fields[tag] = dec.read_string()
        elif kind == "o":
            fields[tag] = dec.read_raw()
        elif kind == "ia":
            fields[tag] = dec.read_integer_list()
        elif kind == "sa":
            fields[tag] = dec.read_string_list()
        elif kind == "oa":
            fields[tag] = dec.read_object_list()
        else:
            raw = dec.read_raw()
            try:
                fields[tag] = raw.decode("utf-8")
            except UnicodeDecodeError:
                fields[tag] = raw
    return fields


def as_bytes(v):
    """Normalize a decoded field value to bytes (strings round-trip via UTF-8)."""
    if isinstance(v, str):
        return v.encode("utf-8")
    if isinstance(v, (bytes, bytearray)):
        return bytes(v)
    raise TypeError("expected str/bytes, got %r" % type(v))


def as_str(v):
    """Normalize a decoded field value to str when it is text."""
    if isinstance(v, str):
        return v
    if isinstance(v, (bytes, bytearray)):
        try:
            return v.decode("utf-8")
        except UnicodeDecodeError:
            return v
    return v


def encode_integer_array(values, width: int = 4) -> bytes:
    """Elements blob for an integer array field (width byte + ints).

    The u32 total prefix is added by encode_object when this is placed in a
    body — do NOT add it here (the client reads exactly one length dword).
    """
    if width == 4:
        payload = b"".join(struct.pack("<i", v) for v in values)
    elif width == 8:
        payload = b"".join(struct.pack("<q", v) for v in values)
    else:
        raise SprotoError("bad array width")
    return struct.pack("<B", width) + payload


def encode_string_array(values) -> bytes:
    parts = []
    for v in values:
        raw = v.encode("utf-8")
        parts.append(struct.pack("<I", len(raw)) + raw)
    return b"".join(parts)


def encode_object_array(objs) -> bytes:
    """Elements blob for an object array field; each element is
    u32 len + object stream. The u32 total prefix is added by encode_object."""
    parts = []
    for obj in objs:
        enc = encode_object(obj) if not isinstance(obj, (bytes, bytearray)) else bytes(obj)
        parts.append(struct.pack("<I", len(enc)) + enc)
    return b"".join(parts)
