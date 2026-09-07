using Sproto;

namespace SprotoType;

public class send_escort_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private long _npcid;

		private long _lineIndex;

		private string _mapInfoId;

		public long npcid
		{
			get
			{
				return _npcid;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_npcid = value;
			}
		}

		public bool HasNpcid => has_field.has_field(0);

		public long lineIndex
		{
			get
			{
				return _lineIndex;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_lineIndex = value;
			}
		}

		public bool HasLineIndex => has_field.has_field(1);

		public string mapInfoId
		{
			get
			{
				return _mapInfoId;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_mapInfoId = value;
			}
		}

		public bool HasMapInfoId => has_field.has_field(2);

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
					npcid = deserialize.read_integer();
					break;
				case 1:
					lineIndex = deserialize.read_integer();
					break;
				case 2:
					mapInfoId = deserialize.read_string();
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
				serialize.write_integer(npcid, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(lineIndex, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(mapInfoId, 2);
			}
			return serialize.close();
		}
	}
}
