using System.Collections.Generic;

public class PlayerChatHistoryInfo
{
	private GameDefine.CHAT_CHANNEL_TYPE mChannelType;

	private long mSenderServerId;

	private string mSenderName;

	private long mTellId;

	private string mTellName;

	private string mChatInfo;

	private string mChatInfo2 = string.Empty;

	private GameDefine.CHAT_LINK_TYPE mLinkType;

	private PROFESSION_TYPE mSenderProfession;

	private int mLevel;

	private int mComboValue;

	private long mGuildId;

	private string mGuildName;

	private List<long> mLongData = new List<long>();

	private List<string> mStringData = new List<string>();

	public GameDefine.CHAT_CHANNEL_TYPE ChannelType
	{
		get
		{
			return mChannelType;
		}
		set
		{
			mChannelType = value;
		}
	}

	public long SenderServerId
	{
		get
		{
			return mSenderServerId;
		}
		set
		{
			mSenderServerId = value;
		}
	}

	public string SenderName
	{
		get
		{
			return mSenderName;
		}
		set
		{
			mSenderName = value;
		}
	}

	public long TellId
	{
		get
		{
			return mTellId;
		}
		set
		{
			mTellId = value;
		}
	}

	public string TellName
	{
		get
		{
			return mTellName;
		}
		set
		{
			mTellName = value;
		}
	}

	public string ChatInfo
	{
		get
		{
			return mChatInfo;
		}
		set
		{
			mChatInfo = value;
		}
	}

	public string ChatInfo2
	{
		get
		{
			return mChatInfo2;
		}
		set
		{
			mChatInfo2 = value;
		}
	}

	public GameDefine.CHAT_LINK_TYPE LinkType
	{
		get
		{
			return mLinkType;
		}
		set
		{
			mLinkType = value;
		}
	}

	public PROFESSION_TYPE SenderProfession
	{
		get
		{
			return mSenderProfession;
		}
		set
		{
			mSenderProfession = value;
		}
	}

	public int Level
	{
		get
		{
			return mLevel;
		}
		set
		{
			mLevel = value;
		}
	}

	public int ComboValue
	{
		get
		{
			return mComboValue;
		}
		set
		{
			mComboValue = value;
		}
	}

	public long GuildId
	{
		get
		{
			return mGuildId;
		}
		set
		{
			mGuildId = value;
		}
	}

	public string GuildName
	{
		get
		{
			return mGuildName;
		}
		set
		{
			mGuildName = value;
		}
	}

	public List<long> LongData => mLongData;

	public List<string> StringData => mStringData;
}
