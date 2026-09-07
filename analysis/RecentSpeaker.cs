public class RecentSpeaker
{
	private long mServerId;

	private string mName;

	private PROFESSION_TYPE mProfession;

	public bool NewMessageFlag;

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

	public void Reset(long serverId, string name, PROFESSION_TYPE profession)
	{
		mServerId = serverId;
		mName = name;
		mProfession = profession;
		NewMessageFlag = false;
	}
}
