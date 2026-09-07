using SprotoType;

public class GuildMember
{
	private long mServerId;

	private string mMemberName;

	private int mLevel;

	private int mVip;

	private PROFESSION_TYPE mProfession;

	private int mContribute;

	private int mAllContribute;

	private Guild_JOB mJob;

	private int mComboValue;

	public int mState;

	private long mLastLogout;

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

	public string MemberName
	{
		get
		{
			return mMemberName;
		}
		set
		{
			mMemberName = value;
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

	public int VIP
	{
		get
		{
			return mVip;
		}
		set
		{
			mVip = value;
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

	public int Contribute
	{
		get
		{
			return mContribute;
		}
		set
		{
			mContribute = value;
		}
	}

	public int AllContribute
	{
		get
		{
			return mAllContribute;
		}
		set
		{
			mAllContribute = value;
		}
	}

	public Guild_JOB Job
	{
		get
		{
			return mJob;
		}
		set
		{
			mJob = value;
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

	public int State
	{
		get
		{
			return mState;
		}
		set
		{
			mState = value;
		}
	}

	public long LastLogout
	{
		get
		{
			return mLastLogout;
		}
		set
		{
			mLastLogout = value;
		}
	}

	public GuildMember()
	{
		Init();
	}

	public GuildMember(guild_member_info info)
	{
		mServerId = info.characterId;
		mMemberName = info.name;
		mJob = ((!info.HasJob) ? Guild_JOB.NO_JOB : ((Guild_JOB)info.job));
		mLevel = (int)(info.HasLevel ? info.level : 0);
		mProfession = ((!info.HasProfession) ? PROFESSION_TYPE.INVALID : ((PROFESSION_TYPE)info.profession));
		mState = (int)(info.HasState ? info.state : 0);
		mVip = (int)((!info.HasVip) ? (-1) : info.vip);
		mContribute = (int)(info.HasContribute ? info.contribute : 0);
		mComboValue = (int)(info.HasCombValue ? info.combValue : 0);
		mLastLogout = ((!info.HasLastLogout) ? (-1) : info.lastLogout);
		mAllContribute = (int)(info.HasAll_contribute ? info.all_contribute : 0);
	}

	public void Init()
	{
	}
}
