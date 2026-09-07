using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Sproto;

public class SprotoTypeDeserialize
{
	public delegate TK gen_key_func<TK, TV>(TV v);

	private SprotoTypeReader reader;

	private int begin_data_pos;

	private int cur_field_pos;

	private int fn;

	private int tag = -1;

	private int value;

	public SprotoTypeDeserialize()
	{
	}

	public SprotoTypeDeserialize(byte[] data)
	{
		init(data);
	}

	public SprotoTypeDeserialize(SprotoTypeReader reader)
	{
		init(reader);
	}

	public void init(byte[] data, int offset = 0, int len = 0)
	{
		clear();
		reader = new SprotoTypeReader(data, offset, len);
		init();
	}

	public void init(SprotoTypeReader reader)
	{
		clear();
		this.reader = reader;
		init();
	}

	private void init()
	{
		fn = read_word();
		int num = (begin_data_pos = SprotoTypeSize.sizeof_header + fn * SprotoTypeSize.sizeof_field);
		cur_field_pos = reader.Position;
		if (reader.Length < num)
		{
			SprotoTypeSize.error("invalid decode header.");
		}
		reader.Seek(begin_data_pos);
	}

	private ulong expand64(uint v)
	{
		ulong num = v;
		if ((num & 0x80000000u) != 0L)
		{
			num |= 0xFFFFFFFF00000000uL;
		}
		return num;
	}

	private int read_word()
	{
		return reader.ReadByte() | (reader.ReadByte() << 8);
	}

	private uint read_dword()
	{
		return (uint)(reader.ReadByte() | (reader.ReadByte() << 8) | (reader.ReadByte() << 16) | (reader.ReadByte() << 24));
	}

	private uint read_array_size()
	{
		if (value >= 0)
		{
			SprotoTypeSize.error("invalid array value.");
		}
		uint num = read_dword();
		if (num < 0)
		{
			SprotoTypeSize.error("error array size(" + num + ")");
		}
		return num;
	}

	public int read_tag()
	{
		int position = reader.Position;
		reader.Seek(cur_field_pos);
		while (reader.Position < begin_data_pos)
		{
			tag++;
			int num = read_word();
			if ((num & 1) == 0)
			{
				cur_field_pos = reader.Position;
				reader.Seek(position);
				value = num / 2 - 1;
				return tag;
			}
			tag += num / 2;
		}
		reader.Seek(position);
		return -1;
	}

	public long read_integer()
	{
		if (value >= 0)
		{
			return value;
		}
		uint num = read_dword();
		switch (num)
		{
		case 4u:
			return (long)expand64(read_dword());
		case 8u:
		{
			uint num2 = read_dword();
			uint num3 = read_dword();
			return (long)(num2 | ((ulong)num3 << 32));
		}
		default:
			SprotoTypeSize.error("read invalid integer size (" + num + ")");
			return 0L;
		}
	}

	public List<long> read_integer_list()
	{
		List<long> list = null;
		uint num = read_array_size();
		if (num == 0)
		{
			return new List<long>();
		}
		int num2 = reader.ReadByte();
		num--;
		switch (num2)
		{
		case 4:
		{
			if (num % 4 != 0)
			{
				SprotoTypeSize.error("error array size(" + num + ")@sizeof(Uint32)");
			}
			list = new List<long>();
			for (int j = 0; j < num / 4; j++)
			{
				ulong item2 = expand64(read_dword());
				list.Add((long)item2);
			}
			break;
		}
		case 8:
		{
			if (num % 8 != 0)
			{
				SprotoTypeSize.error("error array size(" + num + ")@sizeof(Uint64)");
			}
			list = new List<long>();
			for (int i = 0; i < num / 8; i++)
			{
				uint num3 = read_dword();
				uint num4 = read_dword();
				ulong item = num3 | ((ulong)num4 << 32);
				list.Add((long)item);
			}
			break;
		}
		default:
			SprotoTypeSize.error("error intlen(" + num2 + ")");
			break;
		}
		return list;
	}

	public bool read_boolean()
	{
		if (value < 0)
		{
			SprotoTypeSize.error("read invalid boolean.");
			return false;
		}
		return (value != 0) ? true : false;
	}

	public List<bool> read_boolean_list()
	{
		uint num = read_array_size();
		List<bool> list = new List<bool>();
		for (int i = 0; i < num; i++)
		{
			bool item = ((reader.ReadByte() != 0) ? true : false);
			list.Add(item);
		}
		return list;
	}

	public string read_string()
	{
		uint num = read_dword();
		byte[] array = new byte[num];
		reader.Read(array, 0, array.Length);
		return Encoding.UTF8.GetString(array);
	}

	public List<string> read_string_list()
	{
		uint num = read_array_size();
		List<string> list = new List<string>();
		uint num2 = 0u;
		while (num != 0)
		{
			if (num < SprotoTypeSize.sizeof_length)
			{
				SprotoTypeSize.error("error array size.");
			}
			uint num3 = read_dword();
			num -= (uint)SprotoTypeSize.sizeof_length;
			if (num3 > num)
			{
				SprotoTypeSize.error("error array object.");
			}
			byte[] array = new byte[num3];
			reader.Read(array, 0, array.Length);
			string @string = Encoding.UTF8.GetString(array);
			list.Add(@string);
			num -= num3;
			num2++;
		}
		return list;
	}

	public T read_obj<T>() where T : SprotoTypeBase, new()
	{
		int num = (int)read_dword();
		SprotoTypeReader sprotoTypeReader = new SprotoTypeReader(reader.Buffer, reader.Offset, num);
		reader.Seek(reader.Position + num);
		T result = new T();
		result.init(sprotoTypeReader);
		return result;
	}

	private T read_element<T>(SprotoTypeReader reader, uint sz, out uint read_size) where T : SprotoTypeBase, new()
	{
		read_size = 0u;
		if (sz < SprotoTypeSize.sizeof_length)
		{
			SprotoTypeSize.error("error array size.");
		}
		uint num = read_dword();
		sz -= (uint)SprotoTypeSize.sizeof_length;
		read_size += (uint)SprotoTypeSize.sizeof_length;
		if (num > sz)
		{
			SprotoTypeSize.error("error array object.");
		}
		reader.Init(this.reader.Buffer, this.reader.Offset, (int)num);
		this.reader.Seek(this.reader.Position + (int)num);
		T result = new T();
		result.init(reader);
		read_size += num;
		return result;
	}

	public List<T> read_obj_list<T>() where T : SprotoTypeBase, new()
	{
		uint num = read_array_size();
		List<T> list = new List<T>();
		SprotoTypeReader sprotoTypeReader = new SprotoTypeReader();
		uint num2 = 0u;
		while (num != 0)
		{
			list.Add(read_element<T>(sprotoTypeReader, num, out var read_size));
			num -= read_size;
			num2++;
		}
		return list;
	}

	public Dictionary<TK, TV> read_map<TK, TV>(gen_key_func<TK, TV> func) where TV : SprotoTypeBase, new()
	{
		uint num = read_array_size();
		Dictionary<TK, TV> dictionary = new Dictionary<TK, TV>();
		SprotoTypeReader sprotoTypeReader = new SprotoTypeReader();
		uint num2 = 0u;
		while (num != 0)
		{
			uint read_size;
			TV v = read_element<TV>(sprotoTypeReader, num, out read_size);
			TK key = func(v);
			if (!dictionary.ContainsKey(key))
			{
				dictionary.Add(key, v);
			}
			else
			{
				Debug.Log(key.ToString() + " :key error:" + v.ToString());
			}
			num -= read_size;
			num2++;
		}
		return dictionary;
	}

	public void read_unknow_data()
	{
		if (value < 0)
		{
			int num = (int)read_dword();
			reader.Seek(num + reader.Position);
		}
	}

	public int size()
	{
		return reader.Position;
	}

	public void clear()
	{
		fn = 0;
		tag = -1;
		value = 0;
		if (reader != null)
		{
			reader.Seek(0);
		}
	}
}
