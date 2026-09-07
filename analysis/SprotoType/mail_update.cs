using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class mail_update
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 12;

		private long _mailId;

		private long _sendertype;

		private string _title;

		private long _senderTime;

		private long _receiveId;

		private long _readTime;

		private string _context;

		private long _mailState;

		private long _sortTime;

		private Dictionary<string, item> _items;

		private long _expireday;

		public long mailId
		{
			get
			{
				return _mailId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_mailId = value;
			}
		}

		public bool HasMailId => has_field.has_field(0);

		public long sendertype
		{
			get
			{
				return _sendertype;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_sendertype = value;
			}
		}

		public bool HasSendertype => has_field.has_field(1);

		public string title
		{
			get
			{
				return _title;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_title = value;
			}
		}

		public bool HasTitle => has_field.has_field(2);

		public long senderTime
		{
			get
			{
				return _senderTime;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_senderTime = value;
			}
		}

		public bool HasSenderTime => has_field.has_field(3);

		public long receiveId
		{
			get
			{
				return _receiveId;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_receiveId = value;
			}
		}

		public bool HasReceiveId => has_field.has_field(4);

		public long readTime
		{
			get
			{
				return _readTime;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_readTime = value;
			}
		}

		public bool HasReadTime => has_field.has_field(5);

		public string context
		{
			get
			{
				return _context;
			}
			set
			{
				has_field.set_field(6, is_has: true);
				_context = value;
			}
		}

		public bool HasContext => has_field.has_field(6);

		public long mailState
		{
			get
			{
				return _mailState;
			}
			set
			{
				has_field.set_field(7, is_has: true);
				_mailState = value;
			}
		}

		public bool HasMailState => has_field.has_field(7);

		public long sortTime
		{
			get
			{
				return _sortTime;
			}
			set
			{
				has_field.set_field(8, is_has: true);
				_sortTime = value;
			}
		}

		public bool HasSortTime => has_field.has_field(8);

		public Dictionary<string, item> items
		{
			get
			{
				return _items;
			}
			set
			{
				has_field.set_field(9, is_has: true);
				_items = value;
			}
		}

		public bool HasItems => has_field.has_field(9);

		public long expireday
		{
			get
			{
				return _expireday;
			}
			set
			{
				has_field.set_field(10, is_has: true);
				_expireday = value;
			}
		}

		public bool HasExpireday => has_field.has_field(10);

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
					mailId = deserialize.read_integer();
					break;
				case 1:
					sendertype = deserialize.read_integer();
					break;
				case 3:
					title = deserialize.read_string();
					break;
				case 4:
					senderTime = deserialize.read_integer();
					break;
				case 5:
					receiveId = deserialize.read_integer();
					break;
				case 6:
					readTime = deserialize.read_integer();
					break;
				case 7:
					context = deserialize.read_string();
					break;
				case 8:
					mailState = deserialize.read_integer();
					break;
				case 9:
					sortTime = deserialize.read_integer();
					break;
				case 10:
					items = deserialize.read_map((item v) => v.id);
					break;
				case 11:
					expireday = deserialize.read_integer();
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
				serialize.write_integer(mailId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(sendertype, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(title, 3);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(senderTime, 4);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(receiveId, 5);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(readTime, 6);
			}
			if (has_field.has_field(6))
			{
				serialize.write_string(context, 7);
			}
			if (has_field.has_field(7))
			{
				serialize.write_integer(mailState, 8);
			}
			if (has_field.has_field(8))
			{
				serialize.write_integer(sortTime, 9);
			}
			if (has_field.has_field(9))
			{
				serialize.write_obj(items, 10);
			}
			if (has_field.has_field(10))
			{
				serialize.write_integer(expireday, 11);
			}
			return serialize.close();
		}
	}
}
