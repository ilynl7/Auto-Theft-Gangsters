using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_consign_ask_items_info
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 5;

		private Dictionary<long, consign_item> _consign_items;

		private long _curPage;

		private long _maxPage;

		private long _success;

		private long _serverTime;

		public Dictionary<long, consign_item> consign_items
		{
			get
			{
				return _consign_items;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_consign_items = value;
			}
		}

		public bool HasConsign_items => has_field.has_field(0);

		public long curPage
		{
			get
			{
				return _curPage;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_curPage = value;
			}
		}

		public bool HasCurPage => has_field.has_field(1);

		public long maxPage
		{
			get
			{
				return _maxPage;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_maxPage = value;
			}
		}

		public bool HasMaxPage => has_field.has_field(2);

		public long success
		{
			get
			{
				return _success;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_success = value;
			}
		}

		public bool HasSuccess => has_field.has_field(3);

		public long serverTime
		{
			get
			{
				return _serverTime;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_serverTime = value;
			}
		}

		public bool HasServerTime => has_field.has_field(4);

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
					consign_items = deserialize.read_map((consign_item v) => v.id);
					break;
				case 1:
					curPage = deserialize.read_integer();
					break;
				case 2:
					maxPage = deserialize.read_integer();
					break;
				case 3:
					success = deserialize.read_integer();
					break;
				case 4:
					serverTime = deserialize.read_integer();
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
				serialize.write_obj(consign_items, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(curPage, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(maxPage, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(success, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(serverTime, 4);
			}
			return serialize.close();
		}
	}
}
