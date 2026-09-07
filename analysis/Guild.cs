using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class Guild
{
	private Dictionary<long, guild_skill> mSkills;

	private Dictionary<string, donate_record> mRecords;

	private long mServerId = -1L;

	private string mGuildName;

	private int mGuildLevel;

	private long mGuildChiefId;

	private string mGuildChiefName;

	private int mGuildMaxPlayer;

	private int mGuildExp;

	private string mGuildNotice;

	private int mGuildIcon;

	private int mGuildAllContribute;

	private Dictionary<long, GuildMember> mGuildMemberList;

	private int mGuildMaxApplyNum;

	private int mGuildCurApplyNum;

	private int mGuildMemberNum;

	private int mGuildCombo;

	public string mNotice;

	public bool mIsNeedAppro;

	private int mDisactiveState;

	private Guild_JOB mPlayerJob;

	public int mViceNum;

	public int mElderNum;

	public Dictionary<long, guild_skill> GuildSkills
	{
		get
		{
			return mSkills;
		}
		set
		{
			mSkills = value;
		}
	}

	public Dictionary<string, donate_record> DonateRecord
	{
		get
		{
			return mRecords;
		}
		set
		{
			mRecords = value;
		}
	}

	public long ServerId
	{
		get
		{
			return mServerId;
		}
		set
		{
			if (mServerId != value)
			{
				mServerId = value;
				if (UIUpdateEvent.OnChangeGuild != null)
				{
					UIUpdateEvent.OnChangeGuild();
				}
			}
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

	public long GuildChiefId
	{
		get
		{
			return mGuildChiefId;
		}
		set
		{
			mGuildChiefId = value;
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

	public int GuildMaxPlayer
	{
		get
		{
			return mGuildMaxPlayer;
		}
		set
		{
			mGuildMaxPlayer = value;
		}
	}

	public int GuildExp
	{
		get
		{
			return mGuildExp;
		}
		set
		{
			mGuildExp = value;
		}
	}

	public string GuildNotice
	{
		get
		{
			return mGuildNotice;
		}
		set
		{
			mGuildNotice = value;
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

	public int GuildAllContribute
	{
		get
		{
			return mGuildAllContribute;
		}
		set
		{
			mGuildAllContribute = value;
		}
	}

	public Dictionary<long, GuildMember> GuildMemberList => mGuildMemberList;

	public int GuildMaxApplyNum
	{
		get
		{
			return mGuildMaxApplyNum;
		}
		set
		{
			mGuildMaxApplyNum = value;
		}
	}

	public int GuildCurApplyNum
	{
		get
		{
			return mGuildCurApplyNum;
		}
		set
		{
			mGuildCurApplyNum = value;
		}
	}

	public int GuildMemberNum
	{
		get
		{
			return mGuildMemberNum;
		}
		set
		{
			mGuildMemberNum = value;
		}
	}

	public int GuildCombo
	{
		get
		{
			return mGuildCombo;
		}
		set
		{
			mGuildCombo = value;
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

	public Guild_JOB PlayerJob
	{
		get
		{
			return mPlayerJob;
		}
		set
		{
			mPlayerJob = value;
		}
	}

	public int ViceNum
	{
		get
		{
			return mViceNum;
		}
		set
		{
			mViceNum = value;
		}
	}

	public int ElderNum
	{
		get
		{
			return mElderNum;
		}
		set
		{
			mElderNum = value;
		}
	}

	public Guild()
	{
		ResetGuild();
	}

	public void Init(guild_info info, Dictionary<string, donate_record> reords)
	{
		if (info.HasGuildId)
		{
			if (mServerId != info.guildId)
			{
				mServerId = info.guildId;
				if (UIUpdateEvent.OnChangeGuild != null)
				{
					UIUpdateEvent.OnChangeGuild();
				}
			}
		}
		else if (mServerId != -1)
		{
			mServerId = -1L;
			if (UIUpdateEvent.OnChangeGuild != null)
			{
				UIUpdateEvent.OnChangeGuild();
			}
		}
		mGuildName = ((!info.HasGuildName) ? string.Empty : info.guildName);
		mGuildLevel = (int)((!info.HasGuildLevel) ? (-1) : info.guildLevel);
		mGuildChiefId = ((!info.HasGuildChiefId) ? (-1) : info.guildChiefId);
		mGuildChiefName = ((!info.HasGuildChiefName) ? string.Empty : info.guildChiefName);
		mGuildExp = (int)((!info.HasGuildExp) ? (-1) : info.guildExp);
		mGuildMaxApplyNum = (int)((!info.HasGuildApplyMaxNum) ? (-1) : info.guildApplyMaxNum);
		mGuildCurApplyNum = (int)((!info.HasGuildApplyNum) ? (-1) : info.guildApplyNum);
		mGuildMaxPlayer = (int)info.guildMaxPlayer;
		mGuildMemberNum = ((!info.HasGuildMemberNum) ? (-1) : ((int)info.guildMemberNum - mGuildCurApplyNum));
		mGuildCombo = (int)((!info.HasGuildCombo) ? (-1) : info.guildCombo);
		mNotice = ((!info.HasNotice) ? string.Empty : info.notice);
		mIsNeedAppro = !info.HasIsNeedAppro || info.isNeedAppro;
		mViceNum = (int)(info.HasViceNum ? info.viceNum : 0);
		mElderNum = (int)(info.HasElderNum ? info.elderNum : 0);
		mPlayerJob = ((!info.HasPlayerJob) ? Guild_JOB.NO_JOB : ((Guild_JOB)info.playerJob));
		mGuildIcon = (int)(info.HasIcon ? info.icon : 0);
		mDisactiveState = (int)(info.HasDisactiveState ? info.disactiveState : 0);
		UpdateDonate(reords);
	}

	public void UpdateSKill(Dictionary<long, guild_skill> skills)
	{
		mSkills = skills;
	}

	public void UpdateSkillType(long type, long level)
	{
		if (mSkills != null && mSkills.ContainsKey(type))
		{
			mSkills[type].level = level;
		}
	}

	public void UpDataGuildMemberList(Dictionary<long, guild_member_info> memberlsit)
	{
		if (memberlsit.Count <= 0)
		{
			Debug.Log("No Member!!!!");
			return;
		}
		if (mGuildMemberList == null)
		{
			mGuildMemberList = new Dictionary<long, GuildMember>();
		}
		else
		{
			mGuildMemberList.Clear();
		}
		foreach (KeyValuePair<long, guild_member_info> item in memberlsit)
		{
			GuildMember guildMember = new GuildMember(item.Value);
			mGuildMemberList.Add(guildMember.ServerId, guildMember);
		}
		UpdateTips();
	}

	public bool CanEditNotice()
	{
		if (!IsGuildValid())
		{
			return false;
		}
		return mPlayerJob == Guild_JOB.JOB_Chief || mPlayerJob == Guild_JOB.JOB_VicePresident;
	}

	public bool CanApprove()
	{
		if (!IsGuildValid())
		{
			return false;
		}
		return mPlayerJob == Guild_JOB.JOB_Chief || mPlayerJob == Guild_JOB.JOB_VicePresident || mPlayerJob == Guild_JOB.JOB_Elder;
	}

	public bool CanSetApprove()
	{
		if (!IsGuildValid())
		{
			return false;
		}
		return mPlayerJob == Guild_JOB.JOB_Chief || mPlayerJob == Guild_JOB.JOB_VicePresident;
	}

	public bool CanChangeJob()
	{
		if (!IsGuildValid())
		{
			return false;
		}
		return mPlayerJob == Guild_JOB.JOB_Chief;
	}

	public bool CanChangMemmberJob(Guild_JOB job)
	{
		return true;
	}

	public bool CanChangeMemberJob(long id)
	{
		if (mGuildMemberList.ContainsKey(id) && CanChangeJob())
		{
			return mPlayerJob < mGuildMemberList[id].Job;
		}
		return false;
	}

	public bool CanKickedMember(long id)
	{
		if (!IsGuildValid())
		{
			return false;
		}
		int memberJobByID = (int)getMemberJobByID(id);
		int num = (int)mPlayerJob;
		if (num >= memberJobByID)
		{
			return false;
		}
		return mPlayerJob == Guild_JOB.JOB_Chief || mPlayerJob == Guild_JOB.JOB_VicePresident;
	}

	public bool CanLeaveGuild()
	{
		if (!IsGuildValid())
		{
			return true;
		}
		if (mPlayerJob == Guild_JOB.JOB_Chief)
		{
			return false;
		}
		return true;
	}

	public bool CanStartActivity()
	{
		return mPlayerJob == Guild_JOB.JOB_Chief || mPlayerJob == Guild_JOB.JOB_VicePresident;
	}

	public void UpdateDonate(Dictionary<string, donate_record> records)
	{
		mRecords = records;
	}

	public void UseDonate(string id)
	{
		if (mRecords != null && mRecords.ContainsKey(id))
		{
			mRecords[id].DonateCount--;
		}
	}

	public bool CheckGuildLevel(int minlevel, int maxlevel)
	{
		return GuilLevel >= minlevel && GuilLevel <= maxlevel;
	}

	public void UpdateAllContribute(int contribute)
	{
		mGuildAllContribute = contribute;
	}

	public GuildMember getChief()
	{
		return mGuildMemberList[mGuildChiefId];
	}

	public bool IsGuildValid()
	{
		return mServerId != -1;
	}

	public void ResetGuild()
	{
		if (mServerId != -1)
		{
			mServerId = -1L;
			if (UIUpdateEvent.OnChangeGuild != null)
			{
				UIUpdateEvent.OnChangeGuild();
			}
		}
		mGuildName = string.Empty;
		mGuildLevel = -1;
		mGuildChiefId = -1L;
		mGuildExp = -1;
		mGuildMaxApplyNum = -1;
		mGuildCurApplyNum = -1;
		mGuildMemberNum = -1;
		mGuildCombo = -1;
		mElderNum = 0;
		mViceNum = 0;
		if (mGuildMemberList != null)
		{
			mGuildMemberList.Clear();
		}
		else
		{
			mGuildMemberList = new Dictionary<long, GuildMember>();
			mGuildMemberList.Clear();
		}
		mDisactiveState = 0;
	}

	public void UpdateManageCount(Guild_JOB job1, Guild_JOB job2)
	{
		if (job1 == Guild_JOB.JOB_VicePresident)
		{
			mViceNum--;
		}
		if (job1 == Guild_JOB.JOB_Elder)
		{
			mElderNum--;
		}
		if (job2 == Guild_JOB.JOB_VicePresident)
		{
			mViceNum++;
		}
		if (job2 == Guild_JOB.JOB_Elder)
		{
			mElderNum++;
		}
	}

	public void setMemberJob(long id, Guild_JOB type)
	{
		if (mGuildMemberList.ContainsKey(id))
		{
			UpdateManageCount(mGuildMemberList[id].Job, type);
			mGuildMemberList[id].Job = type;
		}
		UpdateTips();
	}

	public GuildMember GetMemberInfoById(long ID)
	{
		if (mGuildMemberList.ContainsKey(ID))
		{
			return mGuildMemberList[ID];
		}
		return null;
	}

	public string getMemberName(long ID)
	{
		if (mGuildMemberList.ContainsKey(ID))
		{
			return mGuildMemberList[ID].MemberName;
		}
		return null;
	}

	public Guild_JOB getMemberJobByID(long ID)
	{
		if (mGuildMemberList.ContainsKey(ID))
		{
			return mGuildMemberList[ID].Job;
		}
		return Guild_JOB.NO_JOB;
	}

	public bool isHasMember(long ID)
	{
		return mGuildMemberList.ContainsKey(ID);
	}

	public void SetChiefInfoByJob(int job)
	{
		if (job == 0)
		{
			mGuildChiefId = Singleton<ObjManager>.Instance.MainPlayer.ServerId;
		}
	}

	public void RemoveByID(long ID)
	{
		if (isHasMember(ID))
		{
			mGuildMemberList.Remove(ID);
		}
		UpdateTips();
	}

	public void AddNewMember(guild_member_info info)
	{
		if (!isHasMember(info.characterId))
		{
			GuildMember guildMember = new GuildMember(info);
			GuildMemberList.Add(guildMember.ServerId, guildMember);
		}
		UpdateTips();
	}

	public bool isCanApply()
	{
		if (mGuildCurApplyNum >= mGuildMaxApplyNum)
		{
			return false;
		}
		return true;
	}

	public void UpdateTips()
	{
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.CheckTips(IsHaveGuildTips(), GameDefine.TIPS_TYPE.GUILD);
		}
		if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.RefershTips();
		}
	}

	public bool IsHaveGuildTips()
	{
		if (!IsGuildValid())
		{
			return false;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		return IsHaveNewApply() || playerData.ActivityData.IsHaveGuildActTips();
	}

	public bool IsHaveNewApply()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			return false;
		}
		if (!CanApprove())
		{
			return false;
		}
		List<GuildMember> list = new List<GuildMember>(mGuildMemberList.Values);
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Job == Guild_JOB.JOB_Candidate)
				{
					return true;
				}
			}
		}
		return false;
	}
}
