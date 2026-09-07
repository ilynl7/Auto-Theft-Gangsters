using Sproto;

namespace SprotoType;

public class ret_guild_donate
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 6;

		private long _state;

		private string _id;

		private long _contribute;

		private long _all_contribute;

		private long _level;

		private long _exp;

		public long state
		{
			get
			{
				return _state;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_state = value;
			}
		}

		public bool HasState => has_field.has_field(0);

		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(1);

		public long contribute
		{
			get
			{
				return _contribute;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_contribute = value;
			}
		}

		public bool HasContribute => has_field.has_field(2);

		public long all_contribute
		{
			get
			{
				return _all_contribute;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_all_contribute = value;
			}
		}

		public bool HasAll_contribute => has_field.has_field(3);

		public long level
		{
			get
			{
				return _level;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_level = value;
			}
		}

		public bool HasLevel => has_field.has_field(4);

		public long exp
		{
			get
			{
				return _exp;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_exp = value;
			}
		}

		public bool HasExp => has_field.has_field(5);

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
					state = deserialize.read_integer();
					break;
				case 1:
					id = deserialize.read_string();
					break;
				case 2:
					contribute = deserialize.read_integer();
					break;
				case 3:
					all_contribute = deserialize.read_integer();
					break;
				case 4:
					level = deserialize.read_integer();
					break;
				case 5:
					exp = deserialize.read_integer();
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
				serialize.write_integer(state, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(id, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(contribute, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(all_contribute, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(level, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(exp, 5);
			}
			return serialize.close();
		}
	}
}
