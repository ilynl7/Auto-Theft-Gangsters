using Sproto;

namespace SprotoType;

public class notice_copy_scene_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 6;

		private string _id;

		private long _index;

		private long _time;

		private long _parm1;

		private long _parm2;

		private long _parm3;

		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(0);

		public long index
		{
			get
			{
				return _index;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_index = value;
			}
		}

		public bool HasIndex => has_field.has_field(1);

		public long time
		{
			get
			{
				return _time;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_time = value;
			}
		}

		public bool HasTime => has_field.has_field(2);

		public long parm1
		{
			get
			{
				return _parm1;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_parm1 = value;
			}
		}

		public bool HasParm1 => has_field.has_field(3);

		public long parm2
		{
			get
			{
				return _parm2;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_parm2 = value;
			}
		}

		public bool HasParm2 => has_field.has_field(4);

		public long parm3
		{
			get
			{
				return _parm3;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_parm3 = value;
			}
		}

		public bool HasParm3 => has_field.has_field(5);

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
					id = deserialize.read_string();
					break;
				case 1:
					index = deserialize.read_integer();
					break;
				case 2:
					time = deserialize.read_integer();
					break;
				case 3:
					parm1 = deserialize.read_integer();
					break;
				case 4:
					parm2 = deserialize.read_integer();
					break;
				case 5:
					parm3 = deserialize.read_integer();
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
				serialize.write_string(id, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(index, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(time, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(parm1, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(parm2, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(parm3, 5);
			}
			return serialize.close();
		}
	}
}
