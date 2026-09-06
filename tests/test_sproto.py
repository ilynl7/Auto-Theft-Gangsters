"""Unit tests for the sproto codec (pack/unpack, object encode/decode)."""

import os
import sys

sys.path.insert(0, os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

from server import sproto


def pad8(data: bytes) -> bytes:
    """The client's pack zero-pads the final 8-byte group; unpack restores
    those zeros. The sproto decoder is header-driven, so the padding is
    harmless — but byte-exact round-trips must account for it."""
    rem = len(data) % 8
    return data if rem == 0 else data + b"\x00" * (8 - rem)


def test_pack_roundtrip_all_zero():
    data = b"\x00" * 64
    assert sproto.sproto_unpack(sproto.sproto_pack(data)) == pad8(data)


def test_pack_roundtrip_all_nonzero():
    data = bytes(range(1, 65))
    assert sproto.sproto_unpack(sproto.sproto_pack(data)) == pad8(data)


def test_pack_roundtrip_random_patterns():
    import random
    rng = random.Random(1234)
    for trial in range(200):
        length = rng.randint(0, 300)
        data = bytes(rng.randint(0, 255) for _ in range(length))
        packed = sproto.sproto_pack(data)
        assert sproto.sproto_unpack(packed) == pad8(data), "trial %d" % trial


def test_pack_never_grows_past_two_x():
    data = bytes(range(256)) * 4
    packed = sproto.sproto_pack(data)
    # worst case: literal blocks add ~2 bytes per 8 bytes
    assert len(packed) <= len(data) + (len(data) // 8) * 2 + 2


def test_frame_roundtrip():
    payload = sproto.encode_object({0: 218, 1: 42})
    wire = sproto.frame_encode(payload)
    dec = sproto.FrameDecoder()
    frames = dec.feed(wire)
    # unpacked payload may carry zero padding from the final 8-byte group
    assert frames[0] == pad8(payload)


def test_frame_split_across_feed():
    payload = sproto.encode_object({0: 7})
    wire = sproto.frame_encode(payload)
    dec = sproto.FrameDecoder()
    assert dec.feed(wire[:3]) == []
    frames = dec.feed(wire[3:])
    assert frames[0] == pad8(payload)


def test_encode_object_small_int():
    enc = sproto.encode_object({0: 5})
    assert sproto.decode_fields(enc) == {0: 5}


def test_encode_object_negative_small_int():
    # the client never header-encodes negatives (uint cast in write_integer);
    # they go into the body as 4-byte ints
    enc = sproto.encode_object({0: -1})
    assert sproto.decode_typed(enc, {0: "i"}) == {0: -1}


def test_encode_object_int32():
    enc = sproto.encode_object({0: 100000})
    assert sproto.decode_typed(enc, {0: "i"}) == {0: 100000}


def test_encode_object_int64():
    enc = sproto.encode_object({0: 5_000_000_000})
    assert sproto.decode_typed(enc, {0: "i"}) == {0: 5_000_000_000}


def test_encode_object_negative_int64():
    enc = sproto.encode_object({0: -5_000_000_000})
    assert sproto.decode_typed(enc, {0: "i"}) == {0: -5_000_000_000}


def test_encode_object_bool():
    assert sproto.decode_fields(sproto.encode_object({1: True})) == {1: 1}
    assert sproto.decode_fields(sproto.encode_object({1: False})) == {1: 0}


def test_encode_object_string():
    enc = sproto.encode_object({3: "hello gangster"})
    assert sproto.decode_fields(enc) == {3: "hello gangster"}


def test_encode_object_sparse_tags():
    enc = sproto.encode_object({0: 1, 100: 2, 101: "x"})
    assert sproto.decode_fields(enc) == {0: 1, 100: 2, 101: "x"}


def test_nested_object():
    pos = sproto.encode_object({0: 10, 1: 20, 2: 30, 3: 45})
    enc = sproto.encode_object({0: pos})
    d = sproto.decode_typed(enc, {0: "o"})
    inner = sproto.decode_typed(d[0], {0: "i", 1: "i", 2: "i", 3: "i"})
    assert inner == {0: 10, 1: 20, 2: 30, 3: 45}


def test_integer_array_roundtrip():
    blob = sproto.encode_integer_array([1, 2, 3, -4])
    assert blob[0] == 4  # element width byte
    enc = sproto.encode_object({5: blob})
    assert sproto.decode_typed(enc, {5: "ia"}) == {5: [1, 2, 3, -4]}

def test_object_array_roundtrip():
    objs = [{0: 1, 1: "x"}, {0: 2, 1: "yy"}]
    blob = sproto.encode_object_array(objs)
    enc = sproto.encode_object({5: blob})
    elems = sproto.decode_typed(enc, {5: "oa"})[5]
    decoded = [sproto.decode_fields(e) for e in elems]
    assert decoded == objs


def test_decoder_reader_stream():
    """Simulate the client's exact wire decode: Package + body in one payload."""
    pkg = sproto.encode_object({0: 4, 1: 77})   # type=4(login) session=77
    body = sproto.encode_object({0: 77, 1: "12345", 2: 0})
    payload = pkg + body

    dec = sproto.Decoder(payload)
    ptype = session = None
    while (tag := dec.next_tag()) is not None:
        if tag == 0:
            ptype = dec.read_integer()
        elif tag == 1:
            session = dec.read_integer()
    assert (ptype, session) == (4, 77)
    body_fields = sproto.decode_fields(payload, dec.pos)
    assert body_fields == {0: 77, 1: "12345", 2: 0}
