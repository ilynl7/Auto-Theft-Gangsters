"""Byte-faithful port of the decompiled client's Sproto decode stack.

Mirrors analysis/Sproto/SprotoTypeReader.cs, SprotoTypeDeserialize.cs,
SprotoTypeBase.cs and the generated SprotoType/character.cs +
main_player_create.cs + Package.cs so that any desync the real client hits
reproduces here (the generic server-side Decoder is symmetric with the
server encoder and can therefore mask asymmetric bugs).
"""

import struct


class ClientError(Exception):
    pass


class Reader:
    """Port of SprotoTypeReader: begin/pos/size with relative Position."""

    def __init__(self, buffer: bytes, offset: int = 0, size: int = None):
        self.init(buffer, offset, size)

    def init(self, buffer: bytes, offset: int = 0, size: int = None):
        if size is None:
            size = len(buffer) - offset
        self.buffer = buffer
        self.begin = offset
        self.pos = offset
        self.size = offset + size
        self._check()

    def _check(self):
        if self.pos > self.size or self.begin > self.pos:
            raise ClientError("invalid pos")

    # Offset => absolute pos (client property `Offset`)
    @property
    def offset(self) -> int:
        return self.pos

    # Position => pos - begin (relative)
    @property
    def position(self) -> int:
        return self.pos - self.begin

    # Length => size - begin
    @property
    def length(self) -> int:
        return self.size - self.begin

    def read_byte(self) -> int:
        self._check()
        b = self.buffer[self.pos]
        self.pos += 1
        return b

    def seek(self, offset: int):
        self.pos = self.begin + offset
        self._check()


class Deserializer:
    """Port of SprotoTypeDeserialize."""

    def __init__(self, reader: Reader = None):
        self.reader = reader
        self.begin_data_pos = 0
        self.cur_field_pos = 0
        self.fn = 0
        self.tag = -1
        self.value = 0

    def init(self, src):
        """src: Reader (init(SprotoTypeReader)) — reader variant."""
        self._clear()
        self.reader = src
        self._init()

    def _clear(self):
        self.fn = 0
        self.tag = -1
        self.value = 0
        # client: if reader != null: reader.Seek(0)  (relative 0)

    def _init(self):
        self.fn = self.read_word()
        self.begin_data_pos = 2 + self.fn * 2  # sizeof_header + fn * sizeof_field
        self.cur_field_pos = self.reader.position
        if self.reader.length < self.begin_data_pos:
            raise ClientError("invalid decode header")
        self.reader.seek(self.begin_data_pos)

    def read_word(self) -> int:
        return self.reader.read_byte() | (self.reader.read_byte() << 8)

    def read_dword(self) -> int:
        return (self.reader.read_byte()
                | (self.reader.read_byte() << 8)
                | (self.reader.read_byte() << 16)
                | (self.reader.read_byte() << 24))

    def _expand64(self, v: int) -> int:
        v &= 0xFFFFFFFF
        if v & 0x80000000:
            v |= 0xFFFFFFFF00000000
        return v - (1 << 64) if v & 0x8000000000000000 else v

    def read_tag(self):
        position = self.reader.position
        self.reader.seek(self.cur_field_pos)
        while self.reader.position < self.begin_data_pos:
            self.tag += 1
            num = self.read_word()
            if (num & 1) == 0:
                self.cur_field_pos = self.reader.position
                self.reader.seek(position)
                self.value = num // 2 - 1
                return self.tag
            self.tag += num // 2
        self.reader.seek(position)
        return -1

    def read_integer(self) -> int:
        if self.value >= 0:
            return self.value
        num = self.read_dword()
        if num == 4:
            return self._expand64(self.read_dword())
        if num == 8:
            lo = self.read_dword()
            hi = self.read_dword()
            v = lo | (hi << 32)
            return v - (1 << 64) if v & (1 << 63) else v
        raise ClientError("read invalid integer size (%d)" % num)

    def read_string(self) -> str:
        if self.value >= 0:
            raise ClientError("read invalid string")
        num = self.read_dword()
        if num < 4:
            raise ClientError("error array size.")
        num -= 4
        raw = self.buffer_read(num)
        return raw.decode("utf-8", "replace")

    def buffer_read(self, n: int) -> bytes:
        # reader.Read into temp then... client read_string: reads dword, then
        # if num > remaining -> "error array object"; then reads raw.
        raw = self.reader.buffer[self.reader.pos:self.reader.pos + n]
        self.reader.pos += n
        self.reader._check()
        return bytes(raw)

    def read_obj(self, cls):
        num = self.read_dword()
        sub = Reader(self.reader.buffer, self.reader.offset, num)
        self.reader.seek(self.reader.position + num)
        return cls(sub)

    def read_array_size(self) -> int:
        if self.value >= 0:
            raise ClientError("invalid array value.")
        num = self.read_dword()
        return num

    def read_map(self, elem_cls, key_func):
        num = self.read_array_size()
        result = {}
        sub = Reader(b"", 0, 0)
        while num != 0:
            elem, read_size = self._read_element(elem_cls, sub, num)
            result[key_func(elem)] = elem
            num -= read_size
        return result

    def _read_element(self, cls, sub_reader: Reader, sz: int):
        read_size = 0
        if sz < 4:
            raise ClientError("error array size.")
        num = self.read_dword()
        sz -= 4
        read_size += 4
        if num > sz:
            raise ClientError("error array object.")
        sub_reader.init(self.reader.buffer, self.reader.offset, num)
        self.reader.seek(self.reader.position + num)
        result = cls(sub_reader)
        read_size += num
        return result, read_size

    def read_unknow_data(self):
        if self.value < 0:
            num = self.read_dword()
            self.reader.seek(num + self.reader.position)

    def size(self) -> int:
        return self.reader.position


class SprotoTypeBase:
    def __init__(self, reader: Reader = None):
        self.deserialize = Deserializer()
        self.has_field = {}
        if reader is not None:
            self.init_reader(reader)

    def init_reader(self, reader: Reader):
        self.deserialize.init(reader)
        self.decode()
        return self.deserialize.size()

    def init_buf(self, buffer: bytes, offset: int = 0, length: int = 0):
        self.deserialize.init(Reader(buffer, offset, length))
        self.decode()
        return self.deserialize.size()

    def decode(self):
        raise NotImplementedError

    def set_field(self, idx):
        self.has_field[idx] = True


# ---------------------------------------------------------------------------
# Generated types (only the fields the decode switch touches)
# ---------------------------------------------------------------------------

class Package(SprotoTypeBase):
    def __init__(self):
        super().__init__()
        self.type = None
        self.session = None

    def clear(self):
        self.type = None
        self.session = None
        self.has_field = {}

    def decode(self):
        d = self.deserialize
        num = -1
        while (num := d.read_tag()) != -1:
            if num == 0:
                self.set_field(0)
                self.type = d.read_integer()
            elif num == 1:
                self.set_field(1)
                self.session = d.read_integer()
            else:
                d.read_unknow_data()


class character(SprotoTypeBase):
    def __init__(self, reader=None):
        super().__init__(reader)
        self.id = None
        self.general = None
        self.attribute_other = None
        self.property = None
        self.visual = None
        self.movement = None
        self.skills = None

    def decode(self):
        d = self.deserialize
        num = -1
        while (num := d.read_tag()) != -1:
            if num == 0:
                self.set_field(0)
                self.id = d.read_integer()
            elif num == 1:
                self.set_field(1)
                self.general = d.read_obj(RawObject)
            elif num == 2:
                self.set_field(2)
                self.attribute_other = d.read_obj(RawObject)
            elif num == 5:
                self.set_field(5)
                self.property = d.read_obj(RawObject)
            elif num == 6:
                self.set_field(6)
                self.visual = d.read_obj(RawObject)
            elif num == 7:
                self.set_field(7)
                self.movement = d.read_obj(RawObject)
            elif num == 8:
                self.set_field(8)
                self.skills = d.read_map(RawObject,
                                         lambda v: v.raw)  # key = skillId
            elif num == 13:
                self.set_field(13)
                self.runtime = d.read_obj(RawObject)
            elif num == 15:
                self.set_field(15)
                self.download = d.read_integer()
            elif num == 16:
                self.set_field(16)
                self.skill_index = d.read_integer()
            else:
                d.read_unknow_data()


class RawObject(SprotoTypeBase):
    """Stand-in for nested types: parse header tags without full schemas."""

    def __init__(self, reader=None):
        self.raw = b""
        super().__init__(reader)

    def init_reader(self, reader: Reader):
        # capture the raw bytes the client would hand to the nested type
        self.deserialize.init(reader)
        # record start/end of this object's view
        self.raw = reader.buffer[reader.begin:reader.size]
        self.decode()
        return self.deserialize.size()

    def decode(self):
        d = self.deserialize
        while d.read_tag() != -1:
            # unknown nested fields: emulate minimal consumption
            if d.value < 0:
                n = d.read_dword()
                d.reader.seek(n + d.reader.position)
            # header-encoded values consume nothing


class main_player_create_request(SprotoTypeBase):
    def __init__(self):
        super().__init__()
        self.character = None
        self.movement = None

    def decode(self):
        d = self.deserialize
        num = -1
        while (num := d.read_tag()) != -1:
            if num == 0:
                self.set_field(0)
                self.character = d.read_obj(character)
            elif num == 1:
                self.set_field(1)
                self.movement = d.read_obj(RawObject)
            else:
                d.read_unknow_data()


def decode_frame(unpacked: bytes):
    """Port of NetLogic.ProcessPack: pkg.init -> GenRequest -> request.init."""
    pkg = Package()
    offset = pkg.init_buf(unpacked, 0, len(unpacked))
    if pkg.type is None:
        raise ClientError("frame without type (push dispatch impossible)")
    req = main_player_create_request()
    req.init_buf(unpacked, offset, len(unpacked))
    return pkg, req
