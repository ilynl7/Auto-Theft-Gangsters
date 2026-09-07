using SprotoType;

public class TeamMember
{
	private long mServerId;

	private int mTeamJob;

	private long mTeamId;

	private string mName;

	private int mLevel;

	private PROFESSION_TYPE mProfession;

	private int mCombValue;

	private string mMapInfoId;

	private int mServerLineIndex;

	private int mMemberType;

	private int mHP;

	private int mMaxHP;

	private string mGuildName;

	private long mGuildId;

	private characterVisual visual;

	private bool mIsReadyEnterCopy;

	private bool mIsRefuseEnterCopy;

	private int mCopyRestNum = -1;

	public long ServerId
	{
		get
		{
			return mServerId;
		}
		set
		{
			mServerId = value;
		}
	}

	public int TeamJob
	{
		get
		{
			return mTeamJob;
		}
		set
		{
			mTeamJob = value;
		}
	}

	public long TeamId
	{
		get
		{
			return mTeamId;
		}
		set
		{
			mTeamId = value;
		}
	}

	public string Name
	{
		get
		{
			return mName;
		}
		set
		{
			mName = value;
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

	public PROFESSION_TYPE Profession
	{
		get
		{
			return mProfession;
		}
		set
		{
			mProfession = value;
		}
	}

	public int CombValue
	{
		get
		{
			return mCombValue;
		}
		set
		{
			mCombValue = value;
		}
	}

	public string MapInfoId
	{
		get
		{
			return mMapInfoId;
		}
		set
		{
			mMapInfoId = value;
		}
	}

	public int ServerLineIndex
	{
		get
		{
			return mServerLineIndex;
		}
		set
		{
			mServerLineIndex = value;
		}
	}

	public int MemberType
	{
		get
		{
			return mMemberType;
		}
		set
		{
			mMemberType = value;
		}
	}

	public int HP
	{
		get
		{
			return mHP;
		}
		set
		{
			mHP = value;
		}
	}

	public int MaxHP
	{
		get
		{
			return mMaxHP;
		}
		set
		{
			mMaxHP = value;
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

	public characterVisual Visual
	{
		get
		{
			return visual;
		}
		set
		{
			visual = value;
		}
	}

	public bool IsReadyEnterCopy
	{
		get
		{
			return mIsReadyEnterCopy;
		}
		set
		{
			mIsReadyEnterCopy = value;
		}
	}

	public bool IsRefuseEnterCopy
	{
		get
		{
			return mIsRefuseEnterCopy;
		}
		set
		{
			mIsRefuseEnterCopy = value;
		}
	}

	public int CopyRestNum
	{
		get
		{
			return mCopyRestNum;
		}
		set
		{
			mCopyRestNum = value;
		}
	}

	public void Init()
	{
		mServerId = -1L;
		mTeamJob = -1;
	}

	public bool IsValid()
	{
		return mServerId != -1;
	}

	public void ClearEnterCopyState()
	{
		mIsReadyEnterCopy = false;
		mIsRefuseEnterCopy = false;
	}
}
