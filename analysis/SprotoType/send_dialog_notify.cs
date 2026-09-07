using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class send_dialog_notify
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _type;

		private string _key;

		private List<string> _parm;

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(0);

		public string key
		{
			get
			{
				return _key;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_key = value;
			}
		}

		public bool HasKey => has_field.has_field(1);

		public List<string> parm
		{
			get
			{
				return _parm;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_parm = value;
			}
		}

		public bool HasParm => has_field.has_field(2);

		public request()
			: base(max_field_count)
		{
		}

		public request(byte[] buffer)
			: base(max_field_count, buffer)
		{
			decode();
		}

		protected override void decode()
		{
			int num = -1;
			while ((num = deserialize.read_tag()) != -1)
			{
				switch (num)
				{
				case 0:
					type = deserialize.read_integer();
					break;
				case 1:
					key = deserialize.read_string();
					break;
				case 2:
					parm = deserialize.read_string_list();
					break;
				default:
					deserialize.read_unknow_data();
					break;
				}
			}
		}

		public override int encode(SprotoStream stream)
		{
			serialize.open(stream);
			if (has_field.has_field(0))
			{
				serialize.write_integer(type, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(key, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(parm, 2);
			}
			return serialize.close();
		}
	}
}
