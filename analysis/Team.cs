using System.Collections.Generic;
using SprotoType;

public class Team
{
	private long mTeamID = -1L;

	private int mPreCount;

	private int mCount;

	private bool isVertify;

	private TeamMember[] mTeamMembers;

	private TeamMember mTeamLeader;

	private TeamData mTeamGoalData;

	private int mMinLimitLevel;

	private int mMaxLimitLevel;

	private bool mIsCheckingEnterCopy;

	private PlayerData mCachePlayerData;

	private List<teammember> mTempMemberList;

	private Dictionary<long, teammember> mApplyMemberDic = new Dictionary<long, teammember>();

	private Dictionary<long, team> mAppliedTeamDic = new Dictionary<long, team>();

	private List<long> mInvitedPlayer = new List<long>();

	public long TeamID => mTeamID;

	public int Count
	{
		get
		{
			return mCount;
		}
		set
		{
			mCount = value;
		}
	}

	public bool IsVertify
	{
		get
		{
			return isVertify;
		}
		set
		{
			isVertify = value;
		}
	}

	public TeamMember[] TeamMembers
	{
		get
		{
			return mTeamMembers;
		}
		set
		{
			mTeamMembers = value;
		}
	}

	public TeamMember TeamLeader
	{
		get
		{
			return mTeamLeader;
		}
		set
		{
			mTeamLeader = value;
		}
	}

	public TeamData TeamGoalData
	{
		get
		{
			return mTeamGoalData;
		}
		set
		{
			mTeamGoalData = value;
		}
	}

	public int MinLimitLevel
	{
		get
		{
			return mMinLimitLevel;
		}
		set
		{
			mMinLimitLevel = value;
		}
	}

	public int MaxLimitLevel
	{
		get
		{
			return mMaxLimitLevel;
		}
		set
		{
			mMaxLimitLevel = value;
		}
	}

	public bool IsCheckingEnterCopy
	{
		get
		{
			return mIsCheckingEnterCopy;
		}
		set
		{
			mIsCheckingEnterCopy = value;
		}
	}

	private PlayerData mPlayerData
	{
		get
		{
			if (mCachePlayerData == null)
			{
				mCachePlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			}
			return mCachePlayerData;
		}
	}

	public TeamMember SelfMember => GetTeamMemberByServerId(PlayerData.MainPlayerServerId);

	public Dictionary<long, teammember> ApplyMemberDic => mApplyMemberDic;

	public Dictionary<long, team> AppliedTeamDic => mAppliedTeamDic;

	public List<long> InvitedPlyaer
	{
		get
		{
			return mInvitedPlayer;
		}
		set
		{
			mInvitedPlayer = value;
		}
	}

	public Team()
	{
		mTeamID = -1L;
		mTeamMembers = new TeamMember[3];
		for (int i = 0; i < 3; i++)
		{
			mTeamMembers[i] = new TeamMember();
		}
		mTeamLeader = new TeamMember();
		Init();
	}

	public void Init()
	{
		mTeamID = -1L;
		for (int i = 0; i < 3; i++)
		{
			mTeamMembers[i].Init();
		}
		mTeamLeader.Init();
		mCount = 0;
	}

	public void ClearMemberEnterCopyState()
	{
		mTeamLeader.ClearEnterCopyState();
		for (int i = 0; i < mTeamMembers.Length; i++)
		{
			mTeamMembers[i].ClearEnterCopyState();
		}
	}

	public bool IsFull()
	{
		return mTeamMembers[2].IsValid();
	}

	public int GetEmptyNum()
	{
		return 4 - mCount;
	}

	public TeamMember GetTeamber(int index)
	{
		switch (index)
		{
		default:
			return null;
		case 0:
			return mTeamLeader;
		case 1:
		case 2:
		case 3:
		case 4:
			return mTeamMembers[index - 1];
		}
	}

	public int GetTeamberCount()
	{
		return mCount;
	}

	public void UpdateTeamInfo(update_team.request request)
	{
		bool flag = false;
		bool flag2 = false;
		if (request.HasTeam)
		{
			if (!mPlayerData.IsHaveTeam())
			{
				flag = true;
				NewMessageUIRootLogic.ClearAllteamMessage();
			}
		}
		else if (mPlayerData.IsHaveTeam())
		{
			flag2 = true;
			NewMessageUIRootLogic.ClearAllteamMessage();
		}
		long num = mTeamID;
		mPreCount = mCount;
		Init();
		if (request.HasTeam)
		{
			if (mTeamID != request.team.id)
			{
				mTeamID = request.team.id;
				if (UIUpdateEvent.OnChangeTeam != null)
				{
					UIUpdateEvent.OnChangeTeam();
				}
			}
			mCount = (int)request.team.count;
			isVertify = request.team.isVerfiy == 0;
			mTeamGoalData = DataManager.GetTeamDataDataByID(request.team.goalId);
			mMinLimitLevel = (int)request.team.minLevel;
			mMaxLimitLevel = (int)request.team.maxLevel;
			if (request.team.HasTeammembers)
			{
				mTempMemberList = new List<teammember>(request.team.teammembers.Values);
				for (int i = 0; i < mTempMemberList.Count; i++)
				{
					SetTeamMenmberInfo(mTempMemberList[i], mTeamMembers[i]);
					mTeamMembers[i].TeamJob = 1;
				}
			}
			SetTeamMenmberInfo(request.team.teamleader, mTeamLeader);
			mTeamLeader.TeamJob = 0;
		}
		else
		{
			mTeamID = -1L;
			if (num != -1 && UIUpdateEvent.OnChangeTeam != null)
			{
				UIUpdateEvent.OnChangeTeam();
			}
		}
		bool flag3 = false;
		bool flag4 = false;
		if (mPreCount < GetTeamberCount())
		{
			flag4 = CheckTeamIsFull();
			flag3 = flag4;
		}
		if (SingletonUnity<TeamPreparationRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamPreparationRootLogic>.Instance.gameObject) && mPreCount > GetTeamberCount())
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TeamPreparationRoot);
		}
		if (SingletonUnity<MissionTeamTipLogic>.Exists)
		{
			SingletonUnity<MissionTeamTipLogic>.Instance.TeamTipRoot.Reset();
			SingletonUnity<MissionTeamTipLogic>.Instance.CheckTeamTips();
		}
		if (SingletonUnity<CreateTeamRootLogic>.Exists)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonUnity<CreateTeamRootLogic>.Instance.OnClickCloseBtn();
			if (!UIManager.IsUnlockTutorialEnable())
			{
				flag3 = true;
				if (TutorialManager.CurStep == TUTORIAL_STEP.CREATE_TEAM_WAIT)
				{
					TutorialManager.MoveNext();
				}
			}
		}
		if (flag || flag2)
		{
			mPlayerData.TeamInfo.ClearTeamCacheData();
		}
		if (flag)
		{
			NoticeLogic.AddNotifyData("#{100832}");
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
				if ((!SingletonUnity<TeamUIRootNewLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<TeamUIRootNewLogic>.Instance.gameObject)) && !UIManager.IsUnlockTutorialEnable())
				{
					flag3 = true;
					if (mPlayerData.IsTeamLeader())
					{
						if (TutorialManager.CurStep == TUTORIAL_STEP.CREATE_TEAM_WAIT)
						{
							TutorialManager.MoveNext();
						}
					}
					else if (TutorialManager.CurStep == TUTORIAL_STEP.CREATE_TEAM_WAIT)
					{
						TutorialManager.CloseTutorial();
					}
				}
			}
			if (SingletonUnity<SearchTeamRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SearchTeamRootLogic>.Instance.gameObject))
			{
				SingletonUnity<SearchTeamRootLogic>.Instance.OnClickCloseBtn();
				if (!UIManager.IsUnlockTutorialEnable())
				{
					flag3 = true;
					if (mPlayerData.IsTeamLeader())
					{
						if (TutorialManager.CurStep == TUTORIAL_STEP.CREATE_TEAM_WAIT)
						{
							TutorialManager.MoveNext();
						}
					}
					else if (TutorialManager.CurStep == TUTORIAL_STEP.CREATE_TEAM_WAIT)
					{
						TutorialManager.CloseTutorial();
					}
				}
			}
		}
		if (flag3 && !UIManager.IsUnlockTutorialEnable() && (!SingletonUnity<TeamUIRootNewLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<TeamUIRootNewLogic>.Instance.gameObject)) && SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamRootNew);
		}
		if (flag2)
		{
			NoticeLogic.AddNotifyData("#{100833}");
			if (SingletonUnity<TeamUIRootNewLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamUIRootNewLogic>.Instance.gameObject))
			{
				SingletonUnity<TeamUIRootNewLogic>.Instance.OnClickCloseBtn();
			}
			mPlayerData.TeamInfo.IsVertify = false;
		}
		if (SingletonUnity<TeamUIRootNewLogic>.Exists)
		{
			SingletonUnity<TeamUIRootNewLogic>.Instance.Reset();
		}
		if (flag4 && mPlayerData.IsTeamLeader())
		{
			EnterMultiCopyAction();
		}
		if (SingletonUnity<MultiRankSmallRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MultiRankSmallRootLogic>.Instance.gameObject))
		{
			SingletonUnity<MultiRankSmallRootLogic>.Instance.ResetTeam();
		}
		if (SingletonUnity<ExpBattleInfoRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ExpBattleInfoRootLogic>.Instance.gameObject))
		{
			SingletonUnity<ExpBattleInfoRootLogic>.Instance.ResetTeam();
		}
		if (SingletonUnity<GuildBattleInfoRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildBattleInfoRoot>.Instance.gameObject))
		{
			SingletonUnity<GuildBattleInfoRoot>.Instance.ResetTeam();
		}
		if (SingletonUnity<TeamPreparationRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamPreparationRootLogic>.Instance.gameObject))
		{
			SingletonUnity<TeamPreparationRootLogic>.Instance.Reset();
		}
		if (SingletonUnity<NewDailyCopyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyCopyUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewDailyCopyUIRootLogic>.Instance.UpdateMatchingLabel();
		}
		if (SingletonUnity<MissionTeamTipLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MissionTeamTipLogic>.Instance.gameObject))
		{
			SingletonUnity<MissionTeamTipLogic>.Instance.UpdateMatchBtn();
		}
		if (SingletonUnity<SearchTeamRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SearchTeamRootLogic>.Instance.gameObject))
		{
			SingletonUnity<SearchTeamRootLogic>.Instance.UpdateMatchingLabel();
		}
		if (request.HasTeam && request.team.HasRecruit)
		{
			TeamBroadCastShow((int)request.team.recruit);
		}
	}

	public void UpdateMatchState(sync_random_team_state.request req)
	{
		if (req.HasState)
		{
			isVertify = req.state == 0;
		}
		if (req.HasId)
		{
			mTeamGoalData = DataManager.GetTeamDataDataByID(req.id);
		}
		if (SingletonUnity<TeamUIRootNewLogic>.Exists)
		{
			SingletonUnity<TeamUIRootNewLogic>.Instance.Reset();
		}
		if (SingletonUnity<NewDailyCopyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyCopyUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewDailyCopyUIRootLogic>.Instance.UpdateMatchingLabel();
		}
		if (SingletonUnity<MissionTeamTipLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MissionTeamTipLogic>.Instance.gameObject))
		{
			SingletonUnity<MissionTeamTipLogic>.Instance.UpdateMatchBtn();
		}
		if (SingletonUnity<SearchTeamRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SearchTeamRootLogic>.Instance.gameObject))
		{
			SingletonUnity<SearchTeamRootLogic>.Instance.UpdateMatchingLabel();
		}
	}

	public void EnterMultiCopyAction()
	{
		if (mPlayerData.TeamInfo.TeamLeader.CopyRestNum <= 0)
		{
			NoticeLogic.AddNotifyData("#{101020}");
			return;
		}
		CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(mPlayerData.TeamInfo.TeamGoalData.CopyId);
		if (mPlayerData.TeamInfo.GetTeamberCount() < copySceneDataById.MinMember)
		{
			NoticeLogic.AddNotifyData("#{102004}");
			return;
		}
		for (int i = 0; i < mPlayerData.TeamInfo.TeamMembers.Length; i++)
		{
			if (mPlayerData.TeamInfo.TeamMembers[i].IsValid() && mPlayerData.TeamInfo.TeamMembers[i].Level < copySceneDataById.MinLevel)
			{
				NoticeLogic.AddNotifyData2Client(false, "#{102005}", false, mPlayerData.TeamInfo.TeamMembers[i].Name);
				return;
			}
		}
		enter_multi_copy_scene_confirm.request request = new enter_multi_copy_scene_confirm.request();
		request.id = mPlayerData.TeamInfo.TeamGoalData.ID;
		request.type1 = mPlayerData.TeamInfo.TeamGoalData.GoalType;
		NetLogic.GetInstance().Send<Protocol.enter_multi_copy_scene_confirm>(request);
		mPlayerData.TeamInfo.ClearMemberEnterCopyState();
		mPlayerData.TeamInfo.IsCheckingEnterCopy = true;
		mPlayerData.TeamInfo.TeamLeader.IsReadyEnterCopy = false;
		if (SingletonUnity<TeamUIRootNewLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamUIRootNewLogic>.Instance.gameObject))
		{
			SingletonUnity<TeamUIRootNewLogic>.Instance.Reset();
		}
		NewMessageUIRootLogic.ClearAllteamMessage();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("DailyCopy", $"copy_{copySceneDataById.ID}", $"start_multi_{mPlayerData.TeamInfo.GetTeamberCount()}");
		if (mPlayerData.TeamInfo.GetTeamberCount() <= 1)
		{
			mPlayerData.TeamInfo.IsCheckingEnterCopy = false;
			mPlayerData.TeamInfo.TeamLeader.IsReadyEnterCopy = false;
		}
	}

	public bool CheckTeamIsFull()
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager == null || sceneManager.IsShowTeamScene() || sceneManager.IsCarScene())
		{
			return false;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveTeam() && playerData.IsTeamLeader())
		{
			CopySceneData copySceneData = null;
			if (playerData.TeamInfo.TeamGoalData.GoalType == 1)
			{
				copySceneData = DataManager.GetCopySceneDataById(playerData.TeamInfo.TeamGoalData.CopyId);
			}
			if (!UIManager.IsUnlockTutorialEnable() && copySceneData != null && playerData.TeamInfo.GetTeamberCount() >= copySceneData.MaxMember)
			{
				return true;
			}
		}
		return false;
	}

	public TeamMember GetTeamMemberByServerId(long serverId)
	{
		if (mTeamLeader.ServerId == serverId)
		{
			return mTeamLeader;
		}
		for (int i = 0; i < mTeamMembers.Length; i++)
		{
			if (mTeamMembers[i].ServerId == serverId)
			{
				return mTeamMembers[i];
			}
		}
		return null;
	}

	private void SetTeamMenmberInfo(teammember source, TeamMember target)
	{
		if (source.HasId)
		{
			target.ServerId = source.id;
		}
		if (source.HasTeamid)
		{
			target.TeamId = source.teamid;
		}
		if (source.HasName)
		{
			target.Name = source.name;
		}
		if (source.HasLevel)
		{
			target.Level = (int)source.level;
		}
		if (source.HasProfession)
		{
			target.Profession = (PROFESSION_TYPE)source.profession;
		}
		if (source.HasCombValue)
		{
			target.CombValue = (int)source.combValue;
		}
		if (source.HasMapInfoId)
		{
			target.MapInfoId = source.mapInfoId.ToString();
		}
		if (source.HasLineIndex)
		{
			target.ServerLineIndex = (int)source.lineIndex;
		}
		if (source.HasMemberType)
		{
			target.MemberType = (int)source.memberType;
		}
		if (source.HasHp)
		{
			target.HP = (int)source.hp;
		}
		if (source.HasMax_hp)
		{
			target.MaxHP = (int)source.max_hp;
		}
		if (source.HasVisual)
		{
			target.Visual = source.visual;
		}
		if (source.HasGuildId)
		{
			target.GuildName = source.guildName;
			target.GuildId = source.guildId;
		}
		else
		{
			target.GuildName = string.Empty;
			target.GuildId = 0L;
		}
		if (source.HasCurNum)
		{
			target.CopyRestNum = (int)source.curNum;
		}
		else
		{
			target.CopyRestNum = -1;
		}
		if (!IsCheckingEnterCopy)
		{
			target.ClearEnterCopyState();
		}
	}

	public void UpdateTeamMemberInfo(update_team_member.request request)
	{
		if (request.HasMember)
		{
			TeamMember teamMemberByServerId = GetTeamMemberByServerId(request.member.id);
			SetTeamMenmberInfo(request.member, teamMemberByServerId);
			if (SingletonUnity<MissionTeamTipLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MissionTeamTipLogic>.Instance.TeamTipRoot.gameObject))
			{
				SingletonUnity<MissionTeamTipLogic>.Instance.TeamTipRoot.UpdateMemberInfo(teamMemberByServerId);
			}
			if (SingletonUnity<TeamUIRootNewLogic>.Exists)
			{
				SingletonUnity<TeamUIRootNewLogic>.Instance.Reset();
			}
			if (SingletonUnity<MultiRankSmallRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MultiRankSmallRootLogic>.Instance.gameObject))
			{
				SingletonUnity<MultiRankSmallRootLogic>.Instance.UpdateMemberInfo(teamMemberByServerId);
			}
			if (SingletonUnity<ExpBattleInfoRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ExpBattleInfoRootLogic>.Instance.gameObject))
			{
				SingletonUnity<ExpBattleInfoRootLogic>.Instance.UpdateMemberInfo(teamMemberByServerId);
			}
			if (SingletonUnity<GuildBattleInfoRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildBattleInfoRoot>.Instance.gameObject))
			{
				SingletonUnity<GuildBattleInfoRoot>.Instance.UpdateMemberInfo(teamMemberByServerId);
			}
		}
	}

	public void UpdateSelfTeamInfo()
	{
	}

	public void AddApplyMember(teammember member)
	{
		if (!mApplyMemberDic.ContainsKey(member.id))
		{
			mApplyMemberDic.Add(member.id, member);
			if (SingletonUnity<TeamApplyListLogic>.Exists)
			{
				SingletonUnity<TeamApplyListLogic>.Instance.UpdataApplyList(new List<teammember>(mApplyMemberDic.Values));
			}
			if (SingletonUnity<TeamUIRootNewLogic>.Exists)
			{
				SingletonUnity<TeamUIRootNewLogic>.Instance.OnApplyTeam();
			}
			if (SingletonUnity<MissionTeamTipLogic>.Exists)
			{
				SingletonUnity<MissionTeamTipLogic>.Instance.SetTeamApplyTips(ishave: true);
			}
		}
	}

	public void RemoveApplyMember(long id)
	{
		if (!mApplyMemberDic.ContainsKey(id))
		{
			return;
		}
		mApplyMemberDic.Remove(id);
		if (SingletonUnity<TeamApplyListLogic>.Exists)
		{
			SingletonUnity<TeamApplyListLogic>.Instance.UpdataApplyList(new List<teammember>(mApplyMemberDic.Values));
		}
		if (SingletonUnity<UIManager>.Exists && mApplyMemberDic.Count == 0)
		{
			if (SingletonUnity<TeamUIRootNewLogic>.Exists)
			{
				SingletonUnity<TeamUIRootNewLogic>.Instance.OnClearApplyTeam();
			}
			if (SingletonUnity<MissionTeamTipLogic>.Exists)
			{
				SingletonUnity<MissionTeamTipLogic>.Instance.SetTeamApplyTips(ishave: false);
			}
		}
	}

	public void ClearApplyMember()
	{
		mApplyMemberDic.Clear();
	}

	public bool IsHaveApplyMember()
	{
		return mApplyMemberDic.Count != 0;
	}

	public bool HasAppliedTeam(long teamId)
	{
		return mAppliedTeamDic.ContainsKey(teamId);
	}

	public void AddApplyTeam(team team)
	{
		if (!mAppliedTeamDic.ContainsKey(team.id))
		{
			mAppliedTeamDic.Add(team.id, team);
		}
	}

	public void RemoveApplyTeam(long teamId)
	{
		if (mAppliedTeamDic.ContainsKey(teamId))
		{
			mAppliedTeamDic.Remove(teamId);
		}
	}

	public bool isTeamMemberById(long id)
	{
		if (mTeamLeader.ServerId == id)
		{
			return true;
		}
		for (int i = 0; i < mTeamMembers.Length; i++)
		{
			if (mTeamMembers[i].ServerId == id)
			{
				return true;
			}
		}
		return false;
	}

	public void ClearTeamApplyDic()
	{
		mAppliedTeamDic.Clear();
	}

	public bool HasInvited(long serverId)
	{
		return mInvitedPlayer.Contains(serverId);
	}

	public void RemoveInvitedPerson(long serverId)
	{
		if (mInvitedPlayer.Contains(serverId))
		{
			mInvitedPlayer.Remove(serverId);
		}
	}

	public void ClearTeamInviteList()
	{
		mInvitedPlayer.Clear();
	}

	public void ClearTeamCacheData()
	{
		ClearApplyMember();
		ClearTeamApplyDic();
		ClearTeamInviteList();
		mIsCheckingEnterCopy = false;
	}

	public void Reset()
	{
		Init();
		ClearTeamCacheData();
		isVertify = false;
	}

	public void TeamBroadCastShow(int RecruitNum)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveTeam() && playerData.IsTeamLeader() && RecruitNum > 0)
		{
			string empty = string.Empty;
			Team teamInfo = playerData.TeamInfo;
			GameDefine.CHAT_LINK_TYPE mCurLinkType = GameDefine.CHAT_LINK_TYPE.TEAM;
			List<long> list = new List<long>();
			list.Add(teamInfo.TeamID);
			list.Add(teamInfo.MinLimitLevel);
			list.Add(teamInfo.MaxLimitLevel);
			string empty2 = string.Empty;
			empty2 = ((teamInfo.TeamGoalData.GoalType != 0) ? DataManager.GetCopySceneDataById(teamInfo.TeamGoalData.CopyId).MName : teamInfo.TeamGoalData.MTitleName);
			string dictionaryString = StrDictionary.GetDictionaryString("#{100262}", empty2);
			empty = dictionaryString;
			if (((uint)RecruitNum & (true ? 1u : 0u)) != 0)
			{
				SendChatInfo(empty, GameDefine.CHAT_CHANNEL_TYPE.WORLD, mCurLinkType, list);
			}
			if (((uint)RecruitNum & 2u) != 0)
			{
				SendChatInfo(empty, GameDefine.CHAT_CHANNEL_TYPE.NORMAL, mCurLinkType, list);
			}
			if (((uint)RecruitNum & 4u) != 0 && playerData.IsHaveGuild())
			{
				SendChatInfo(empty, GameDefine.CHAT_CHANNEL_TYPE.GUILD, mCurLinkType, list);
			}
		}
	}

	public void SendChatInfo(string ChatInfo, GameDefine.CHAT_CHANNEL_TYPE mCurChannelType, GameDefine.CHAT_LINK_TYPE mCurLinkType, List<long> mLinkLongData)
	{
		string text = ChatInfo;
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (mCurLinkType == GameDefine.CHAT_LINK_TYPE.INVALID)
		{
			text = text.Replace("\r", " ");
			text = text.Replace("\n", " ");
		}
		if (mCurLinkType != GameDefine.CHAT_LINK_TYPE.INVALID && !text.Contains(ChatInfo))
		{
			return;
		}
		chat.request request = new chat.request();
		request.chattype = (long)mCurChannelType;
		request.linktype = (long)mCurLinkType;
		if (mCurLinkType != GameDefine.CHAT_LINK_TYPE.INVALID)
		{
			if (mLinkLongData.Count != 0)
			{
				request.intdata = mLinkLongData;
			}
			text = text.Replace(ChatInfo, $"[FF0000]{ChatInfo}[-]");
		}
		request.chatInfo = text;
		NetLogic.GetInstance().Send<Protocol.chat>(request);
	}
}
