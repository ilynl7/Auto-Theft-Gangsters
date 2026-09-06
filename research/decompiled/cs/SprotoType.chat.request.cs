using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class chat
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 7;

		private long _tellId;

		private string _tellName;

		private string _chatInfo;

		private long _chattype;

		private long _linktype;

		private List<long> _intdata;

		private List<string> _stringdata;

		public long tellId
		{
			get
			{
				return _tellId;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_tellId = value;
			}
		}

		public bool HasTellId => has_field.has_field(0);

		public string tellName
		{
			get
			{
				return _tellName;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_tellName = value;
			}
		}

		public bool HasTellName => has_field.has_field(1);

		public string chatInfo
		{
			get
			{
				return _chatInfo;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_chatInfo = value;
			}
		}

		public bool HasChatInfo => has_field.has_field(2);

		public long chattype
		{
			get
			{
				return _chattype;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_chattype = value;
			}
		}

		public bool HasChattype => has_field.has_field(3);

		public long linktype
		{
			get
			{
				return _linktype;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_linktype = value;
			}
		}

		public bool HasLinktype => has_field.has_field(4);

		public List<long> intdata
		{
			get
			{
				return _intdata;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_intdata = value;
			}
		}

		public bool HasIntdata => has_field.has_field(5);

		public List<string> stringdata
		{
			get
			{
				return _stringdata;
			}
			set
			{
				has_field.set_field(6, is_has: true);
				_stringdata = value;
			}
		}

		public bool HasStringdata => has_field.has_field(6);

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
					tellId = deserialize.read_integer();
					break;
				case 1:
					tellName = deserialize.read_string();
					break;
				case 2:
					chatInfo = deserialize.read_string();
					break;
				case 3:
					chattype = deserialize.read_integer();
					break;
				case 4:
					linktype = deserialize.read_integer();
					break;
				case 5:
					intdata = deserialize.read_integer_list();
					break;
				case 6:
					stringdata = deserialize.read_string_list();
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
				serialize.write_integer(tellId, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(tellName, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(chatInfo, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(chattype, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(linktype, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(intdata, 5);
			}
			if (has_field.has_field(6))
			{
				serialize.write_string(stringdata, 6);
			}
			return serialize.close();
		}
	}
}
