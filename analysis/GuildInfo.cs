using System;
using SprotoType;

[Serializable]
public class GuildInfo
{
	private long mServerId = -1L;

	private string mGuildName;

	private int mGuildLevel;

	private string mGuildChiefName;

	private int mCurMemberNum;

	private int mMaxMemberNum;

	private int CurApplyNum;

	private int MaxApplyNum;

	private string mNotice;

	private bool mIsNeedAppro;

	private int mDisactiveState;

	private int mGuildIcon;

	private int mComboValue;

	private int mExp;

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

	public string GuilName
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

	public int GuilLevel
	{
		get
		{
			return mGuildLevel;
		}
		set
		{
			mGuildLevel = value;
		}
	}

	public string GuildChiefName
	{
		get
		{
			return mGuildChiefName;
		}
		set
		{
			mGuildChiefName = value;
		}
	}

	public int CurMemberNum
	{
		get
		{
			return mCurMemberNum;
		}
		set
		{
			mCurMemberNum = value;
		}
	}

	public int MaxMemberNum
	{
		get
		{
			return mMaxMemberNum;
		}
		set
		{
			mMaxMemberNum = value;
		}
	}

	public string Notice
	{
		get
		{
			return mNotice;
		}
		set
		{
			mNotice = value;
		}
	}

	public bool IsNeedAppro
	{
		get
		{
			return mIsNeedAppro;
		}
		set
		{
			mIsNeedAppro = value;
		}
	}

	public int DisactiveState
	{
		get
		{
			return mDisactiveState;
		}
		set
		{
			mDisactiveState = value;
		}
	}

	public int GuildIcon
	{
		get
		{
			return mGuildIcon;
		}
		set
		{
			mGuildIcon = value;
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

	public int Exp
	{
		get
		{
			return mExp;
		}
		set
		{
			mExp = value;
		}
	}

	public GuildInfo()
	{
		ResetGuildInfo();
	}

	public GuildInfo(guild_info info)
	{
		mServerId = ((!info.HasGuildId) ? (-1) : info.guildId);
		mGuildName = ((!info.HasGuildName) ? string.Empty : info.guildName);
		mGuildLevel = (int)((!info.HasGuildLevel) ? (-1) : info.guildLevel);
		mGuildChiefName = ((!info.HasGuildChiefName) ? string.Empty : info.guildChiefName);
		CurApplyNum = (int)((!info.HasGuildApplyNum) ? (-1) : info.guildApplyNum);
		MaxApplyNum = (int)((!info.HasGuildApplyMaxNum) ? (-1) : info.guildApplyMaxNum);
		mCurMemberNum = ((!info.HasGuildMemberNum) ? (-1) : ((int)info.guildMemberNum - CurApplyNum));
		mMaxMemberNum = (int)info.guildMaxPlayer;
		mNotice = ((!info.HasNotice) ? string.Empty : info.notice);
		mIsNeedAppro = info.isNeedAppro;
		mComboValue = (int)((!info.HasGuildCombo) ? (-1) : info.guildCombo);
		mExp = (int)((!info.HasGuildExp) ? (-1) : info.guildExp);
		mGuildIcon = (int)(info.HasIcon ? info.icon : 0);
		mDisactiveState = (int)(info.HasDisactiveState ? info.disactiveState : 0);
	}

	public bool IsCanApply()
	{
		return CurApplyNum < MaxApplyNum;
	}

	public void ResetGuildInfo()
	{
		mServerId = -1L;
		mGuildName = string.Empty;
		mGuildLevel = -1;
		mGuildChiefName = string.Empty;
		mCurMemberNum = -1;
		mMaxMemberNum = -1;
		CurApplyNum = -1;
		MaxApplyNum = -1;
		mGuildIcon = -1;
		mExp = 0;
		mComboValue = 0;
		mIsNeedAppro = false;
	}
}
