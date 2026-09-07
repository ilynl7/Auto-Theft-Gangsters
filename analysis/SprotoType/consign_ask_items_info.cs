using Sproto;

namespace SprotoType;

public class consign_ask_items_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 7;

		private long _type;

		private long _subType;

		private long _quality;

		private long _levelRange;

		private bool _use;

		private long _curPage;

		private long _profession;

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

		public long subType
		{
			get
			{
				return _subType;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_subType = value;
			}
		}

		public bool HasSubType => has_field.has_field(1);

		public long quality
		{
			get
			{
				return _quality;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_quality = value;
			}
		}

		public bool HasQuality => has_field.has_field(2);

		public long levelRange
		{
			get
			{
				return _levelRange;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_levelRange = value;
			}
		}

		public bool HasLevelRange => has_field.has_field(3);

		public bool use
		{
			get
			{
				return _use;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_use = value;
			}
		}

		public bool HasUse => has_field.has_field(4);

		public long curPage
		{
			get
			{
				return _curPage;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_curPage = value;
			}
		}

		public bool HasCurPage => has_field.has_field(5);

		public long profession
		{
			get
			{
				return _profession;
			}
			set
			{
				has_field.set_field(6, is_has: true);
				_profession = value;
			}
		}

		public bool HasProfession => has_field.has_field(6);

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
					subType = deserialize.read_integer();
					break;
				case 2:
					quality = deserialize.read_integer();
					break;
				case 3:
					levelRange = deserialize.read_integer();
					break;
				case 4:
					use = deserialize.read_boolean();
					break;
				case 5:
					curPage = deserialize.read_integer();
					break;
				case 6:
					profession = deserialize.read_integer();
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
				serialize.write_integer(subType, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(quality, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(levelRange, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_boolean(use, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(curPage, 5);
			}
			if (has_field.has_field(6))
			{
				serialize.write_integer(profession, 6);
			}
			return serialize.close();
		}
	}
}
