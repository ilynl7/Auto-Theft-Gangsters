using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class chat_item : SprotoTypeBase
{
	private static int max_field_count = 15;

	private long _senderId;

	private string _senderName;

	private long _tellId;

	private string _tellName;

	private string _chatInfo;

	private long _chattype;

	private long _linktype;

	private List<long> _intdata;

	private List<string> _stringdata;

	private long _senderProfession;

	private long _level;

	private long _combValue;

	private long _guildId;

	private string _guildName;

	private string _chatInfo2;

	public long senderId
	{
		get
		{
			return _senderId;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_senderId = value;
		}
	}

	public bool HasSenderId => has_field.has_field(0);

	public string senderName
	{
		get
		{
			return _senderName;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_senderName = value;
		}
	}

	public bool HasSenderName => has_field.has_field(1);

	public long tellId
	{
		get
		{
			return _tellId;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_tellId = value;
		}
	}

	public bool HasTellId => has_field.has_field(2);

	public string tellName
	{
		get
		{
			return _tellName;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_tellName = value;
		}
	}

	public bool HasTellName => has_field.has_field(3);

	public string chatInfo
	{
		get
		{
			return _chatInfo;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_chatInfo = value;
		}
	}

	public bool HasChatInfo => has_field.has_field(4);

	public long chattype
	{
		get
		{
			return _chattype;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_chattype = value;
		}
	}

	public bool HasChattype => has_field.has_field(5);

	public long linktype
	{
		get
		{
			return _linktype;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_linktype = value;
		}
	}

	public bool HasLinktype => has_field.has_field(6);

	public List<long> intdata
	{
		get
		{
			return _intdata;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_intdata = value;
		}
	}

	public bool HasIntdata => has_field.has_field(7);

	public List<string> stringdata
	{
		get
		{
			return _stringdata;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_stringdata = value;
		}
	}

	public bool HasStringdata => has_field.has_field(8);

	public long senderProfession
	{
		get
		{
			return _senderProfession;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_senderProfession = value;
		}
	}

	public bool HasSenderProfession => has_field.has_field(9);

	public long level
	{
		get
		{
			return _level;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_level = value;
		}
	}

	public bool HasLevel => has_field.has_field(10);

	public long combValue
	{
		get
		{
			return _combValue;
		}
		set
		{
			has_field.set_field(11, is_has: true);
			_combValue = value;
		}
	}

	public bool HasCombValue => has_field.has_field(11);

	public long guildId
	{
		get
		{
			return _guildId;
		}
		set
		{
			has_field.set_field(12, is_has: true);
			_guildId = value;
		}
	}

	public bool HasGuildId => has_field.has_field(12);

	public string guildName
	{
		get
		{
			return _guildName;
		}
		set
		{
			has_field.set_field(13, is_has: true);
			_guildName = value;
		}
	}

	public bool HasGuildName => has_field.has_field(13);

	public string chatInfo2
	{
		get
		{
			return _chatInfo2;
		}
		set
		{
			has_field.set_field(14, is_has: true);
			_chatInfo2 = value;
		}
	}

	public bool HasChatInfo2 => has_field.has_field(14);

	public chat_item()
		: base(max_field_count)
	{
	}

	public chat_item(byte[] buffer)
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
				senderId = deserialize.read_integer();
				break;
			case 1:
				senderName = deserialize.read_string();
				break;
			case 2:
				tellId = deserialize.read_integer();
				break;
			case 3:
				tellName = deserialize.read_string();
				break;
			case 4:
				chatInfo = deserialize.read_string();
				break;
			case 5:
				chattype = deserialize.read_integer();
				break;
			case 6:
				linktype = deserialize.read_integer();
				break;
			case 7:
				intdata = deserialize.read_integer_list();
				break;
			case 8:
				stringdata = deserialize.read_string_list();
				break;
			case 9:
				senderProfession = deserialize.read_integer();
				break;
			case 10:
				level = deserialize.read_integer();
				break;
			case 11:
				combValue = deserialize.read_integer();
				break;
			case 12:
				guildId = deserialize.read_integer();
				break;
			case 13:
				guildName = deserialize.read_string();
				break;
			case 14:
				chatInfo2 = deserialize.read_string();
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
			serialize.write_integer(senderId, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_string(senderName, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_integer(tellId, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_string(tellName, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_string(chatInfo, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(chattype, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(linktype, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(intdata, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_string(stringdata, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_integer(senderProfession, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_integer(level, 10);
		}
		if (has_field.has_field(11))
		{
			serialize.write_integer(combValue, 11);
		}
		if (has_field.has_field(12))
		{
			serialize.write_integer(guildId, 12);
		}
		if (has_field.has_field(13))
		{
			serialize.write_string(guildName, 13);
		}
		if (has_field.has_field(14))
		{
			serialize.write_string(chatInfo2, 14);
		}
		return serialize.close();
	}
}
