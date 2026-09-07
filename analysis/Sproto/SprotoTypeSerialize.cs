using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sproto;

public class SprotoTypeSerialize
{
	private delegate void write_func();

	private int header_idx;

	private int header_sz;

	private int header_cap = SprotoTypeSize.sizeof_header;

	private SprotoStream data;

	private int data_idx;

	private int lasttag = -1;

	private int index;

	public SprotoTypeSerialize(int max_field_count)
	{
		header_sz = SprotoTypeSize.sizeof_header + max_field_count * SprotoTypeSize.sizeof_field;
	}

	private void set_header_fn(int fn)
	{
		data[header_idx - 2] = (byte)((uint)fn & 0xFFu);
		data[header_idx - 1] = (byte)((uint)(fn >> 8) & 0xFFu);
	}

	private void write_header_record(int record)
	{
		data[header_idx + header_cap - 2] = (byte)((uint)record & 0xFFu);
		data[header_idx + header_cap - 1] = (byte)((uint)(record >> 8) & 0xFFu);
		header_cap += 2;
		index++;
	}

	private void write_uint32_to_uint64_sign(bool is_negative)
	{
		byte v = (byte)(is_negative ? 255u : 0u);
		data.WriteByte(v);
		data.WriteByte(v);
		data.WriteByte(v);
		data.WriteByte(v);
	}

	private void write_tag(int tag, int value)
	{
		int num = tag - lasttag - 1;
		if (num > 0)
		{
			num = (num - 1) * 2 + 1;
			if (num > 65535)
			{
				SprotoTypeSize.error("tag is too big.");
			}
			write_header_record(num);
		}
		write_header_record(value);
		lasttag = tag;
	}

	private void write_uint32(uint v)
	{
		data.WriteByte((byte)(v & 0xFFu));
		data.WriteByte((byte)((v >> 8) & 0xFFu));
		data.WriteByte((byte)((v >> 16) & 0xFFu));
		data.WriteByte((byte)((v >> 24) & 0xFFu));
	}

	private void write_uint64(ulong v)
	{
		data.WriteByte((byte)(v & 0xFF));
		data.WriteByte((byte)((v >> 8) & 0xFF));
		data.WriteByte((byte)((v >> 16) & 0xFF));
		data.WriteByte((byte)((v >> 24) & 0xFF));
		data.WriteByte((byte)((v >> 32) & 0xFF));
		data.WriteByte((byte)((v >> 40) & 0xFF));
		data.WriteByte((byte)((v >> 48) & 0xFF));
		data.WriteByte((byte)((v >> 56) & 0xFF));
	}

	private void fill_size(int sz)
	{
		if (sz < 0)
		{
			SprotoTypeSize.error("fill invaild size.");
		}
		write_uint32((uint)sz);
	}

	private int encode_integer(uint v)
	{
		fill_size(4);
		write_uint32(v);
		return SprotoTypeSize.sizeof_length + 4;
	}

	private int encode_uint64(ulong v)
	{
		fill_size(8);
		write_uint64(v);
		return SprotoTypeSize.sizeof_length + 8;
	}

	private int encode_string(string str)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(str);
		fill_size(bytes.Length);
		data.Write(bytes, 0, bytes.Length);
		return SprotoTypeSize.sizeof_length + bytes.Length;
	}

	private int encode_struct(SprotoTypeBase obj)
	{
		int position = data.Position;
		data.Seek(SprotoTypeSize.sizeof_length, SeekOrigin.Current);
		int num = obj.encode(data);
		int position2 = data.Position;
		data.Seek(position, SeekOrigin.Begin);
		fill_size(num);
		data.Seek(position2, SeekOrigin.Begin);
		return SprotoTypeSize.sizeof_length + num;
	}

	private void clear()
	{
		index = 0;
		header_idx = 2;
		lasttag = -1;
		data = null;
		header_cap = SprotoTypeSize.sizeof_header;
	}

	public void write_integer(long integer, int tag)
	{
		long num = integer >> 31;
		int num2 = ((num != 0L && num != -1) ? 8 : 4);
		int value = 0;
		switch (num2)
		{
		case 4:
		{
			uint num3 = (uint)integer;
			if (num3 < 32767)
			{
				value = (int)((num3 + 1) * 2);
				num2 = 2;
			}
			else
			{
				num2 = encode_integer(num3);
			}
			break;
		}
		case 8:
			num2 = encode_uint64((ulong)integer);
			break;
		default:
			SprotoTypeSize.error("invaild integer size.");
			break;
		}
		write_tag(tag, value);
	}

	public void write_integer(List<long> integer_list, int tag)
	{
		if (integer_list == null || integer_list.Count <= 0)
		{
			return;
		}
		int position = data.Position;
		data.Seek(position + SprotoTypeSize.sizeof_length, SeekOrigin.Begin);
		int position2 = data.Position;
		int num = 4;
		data.Seek(position2 + 1, SeekOrigin.Begin);
		for (int i = 0; i < integer_list.Count; i++)
		{
			long num2 = integer_list[i];
			long num3 = num2 >> 31;
			int num4 = ((num3 != 0L && num3 != -1) ? 8 : 4);
			switch (num4)
			{
			case 4:
				write_uint32((uint)num2);
				if (num == 8)
				{
					bool is_negative = (num2 & 0x80000000u) != 0L;
					write_uint32_to_uint64_sign(is_negative);
				}
				break;
			case 8:
				if (num == 4)
				{
					data.Seek(position2 + 1, SeekOrigin.Begin);
					for (int j = 0; j < i; j++)
					{
						ulong v = (ulong)integer_list[j];
						write_uint64(v);
					}
					num = 8;
				}
				write_uint64((ulong)num2);
				break;
			default:
				SprotoTypeSize.error("invalid integer size(" + num4 + ")");
				break;
			}
		}
		int position3 = data.Position;
		data.Seek(position2, SeekOrigin.Begin);
		data.WriteByte((byte)num);
		int sz = position3 - position2;
		data.Seek(position, SeekOrigin.Begin);
		fill_size(sz);
		data.Seek(position3, SeekOrigin.Begin);
		write_tag(tag, 0);
	}

	public void write_boolean(bool b, int tag)
	{
		long integer = (b ? 1 : 0);
		write_integer(integer, tag);
	}

	public void write_boolean(List<bool> b_list, int tag)
	{
		if (b_list != null && b_list.Count > 0)
		{
			fill_size(b_list.Count);
			for (int i = 0; i < b_list.Count; i++)
			{
				byte v = (byte)(b_list[i] ? 1u : 0u);
				data.WriteByte(v);
			}
			write_tag(tag, 0);
		}
	}

	public void write_string(string str, int tag)
	{
		encode_string(str);
		write_tag(tag, 0);
	}

	public void write_string(List<string> str_list, int tag)
	{
		if (str_list == null || str_list.Count <= 0)
		{
			return;
		}
		int num = 0;
		foreach (string item in str_list)
		{
			num += SprotoTypeSize.sizeof_length + Encoding.UTF8.GetByteCount(item);
		}
		fill_size(num);
		foreach (string item2 in str_list)
		{
			encode_string(item2);
		}
		write_tag(tag, 0);
	}

	public void write_obj(SprotoTypeBase obj, int tag)
	{
		encode_struct(obj);
		write_tag(tag, 0);
	}

	private void write_set(write_func func, int tag)
	{
		int position = data.Position;
		data.Seek(SprotoTypeSize.sizeof_length, SeekOrigin.Current);
		func();
		int position2 = data.Position;
		int sz = position2 - position - SprotoTypeSize.sizeof_length;
		data.Seek(position, SeekOrigin.Begin);
		fill_size(sz);
		data.Seek(position2, SeekOrigin.Begin);
		write_tag(tag, 0);
	}

	public void write_obj<T>(List<T> obj_list, int tag) where T : SprotoTypeBase
	{
		if (obj_list == null || obj_list.Count <= 0)
		{
			return;
		}
		write_func func = delegate
		{
			foreach (T item in obj_list)
			{
				encode_struct(item);
			}
		};
		write_set(func, tag);
	}

	public void write_obj<TK, TV>(Dictionary<TK, TV> map, int tag) where TV : SprotoTypeBase
	{
		if (map == null || map.Count <= 0)
		{
			return;
		}
		write_func func = delegate
		{
			foreach (KeyValuePair<TK, TV> item in map)
			{
				encode_struct(item.Value);
			}
		};
		write_set(func, tag);
	}

	public void open(SprotoStream stream)
	{
		clear();
		data = stream;
		header_idx = stream.Position + header_cap;
		data_idx = data.Seek(header_sz, SeekOrigin.Current);
	}

	public int close()
	{
		set_header_fn(index);
		int up_count = header_sz - header_cap;
		data.MoveUp(data_idx, up_count);
		int result = data.Position - header_idx + SprotoTypeSize.sizeof_header;
		clear();
		return result;
	}
}
