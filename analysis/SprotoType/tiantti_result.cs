using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class tiantti_result
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 6;

		private bool _win;

		private List<item> _items;

		private long _rankPos1;

		private long _rankPos2;

		private long _bestRankPos;

		private long _type;

		public bool win
		{
			get
			{
				return _win;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_win = value;
			}
		}

		public bool HasWin => has_field.has_field(0);

		public List<item> items
		{
			get
			{
				return _items;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_items = value;
			}
		}

		public bool HasItems => has_field.has_field(1);

		public long rankPos1
		{
			get
			{
				return _rankPos1;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_rankPos1 = value;
			}
		}

		public bool HasRankPos1 => has_field.has_field(2);

		public long rankPos2
		{
			get
			{
				return _rankPos2;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_rankPos2 = value;
			}
		}

		public bool HasRankPos2 => has_field.has_field(3);

		public long bestRankPos
		{
			get
			{
				return _bestRankPos;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_bestRankPos = value;
			}
		}

		public bool HasBestRankPos => has_field.has_field(4);

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(5);

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
					win = deserialize.read_boolean();
					break;
				case 1:
					items = deserialize.read_obj_list<item>();
					break;
				case 2:
					rankPos1 = deserialize.read_integer();
					break;
				case 3:
					rankPos2 = deserialize.read_integer();
					break;
				case 4:
					bestRankPos = deserialize.read_integer();
					break;
				case 5:
					type = deserialize.read_integer();
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
				serialize.write_boolean(win, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_obj(items, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(rankPos1, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(rankPos2, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(bestRankPos, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(type, 5);
			}
			return serialize.close();
		}
	}
}
