using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SearchTeamRootLogic : SingletonUnity<SearchTeamRootLogic>
{
	public TeamTargetTab TeamTargetTabRoot;

	public List<TeamLineRoot> TeamLineList;

	public UIGrid TeamLineGrid;

	private List<team> mCurTeamList;

	private List<team> mCurPageTeamList = new List<team>();

	private PlayerData mPlayerData;

	private string mCurKey;

	private string mResetGoalId = string.Empty;

	private float time;

	public GameObject NoTeamObj;

	public GameObject MatchBtnObj;

	public UILabel MatchBtnLabel;

	public TweenAlpha MatchAnima;

	public GameObject CreateBtn;

	private bool mIsMatching;

	private CopySceneData mCurCopyScene;

	public bool IsMatching
	{
		get
		{
			return mIsMatching;
		}
		set
		{
			mIsMatching = value;
		}
	}

	private void UpdateTeamList(string goalId)
	{
		get_team_list.request request = new get_team_list.request();
		if (!string.IsNullOrEmpty(goalId))
		{
			request.goalId = goalId;
		}
		NetLogic.GetInstance().Send<Protocol.get_team_list>(request);
	}

	public void Reset(string GoalId)
	{
		UpdateTeamList(GoalId);
		mResetGoalId = GoalId;
		UnityVersionUtil.SetActiveRecursive(TeamTargetTabRoot.gameObject, state: false);
		for (int i = 0; i < TeamLineList.Count; i++)
		{
			NGUITools.SetActive(TeamLineList[i].gameObject, state: false);
		}
	}

	public void Init()
	{
		UnityVersionUtil.SetActiveRecursive(TeamTargetTabRoot.gameObject, state: true);
		for (int i = 0; i < TeamLineList.Count; i++)
		{
			NGUITools.SetActive(TeamLineList[i].gameObject, state: true);
		}
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		List<TeamData> teamDataList = DataManager.GetTeamDataList();
		Dictionary<string, TeamTargetTabData> dictionary = new Dictionary<string, TeamTargetTabData>();
		CopyData copyInfoData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData;
		for (int j = 0; j < teamDataList.Count; j++)
		{
			if (!string.IsNullOrEmpty(teamDataList[j].CopyId) && !copyInfoData.DailyCopyInfoDic.ContainsKey(teamDataList[j].CopyId))
			{
				continue;
			}
			string text = string.Empty;
			if (string.IsNullOrEmpty(teamDataList[j].TitleName))
			{
				if (teamDataList[j].GoalType == 1)
				{
					CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(teamDataList[j].CopyId);
					text = copySceneDataById.MName;
				}
			}
			else
			{
				text = teamDataList[j].MTitleName;
			}
			if (string.IsNullOrEmpty(teamDataList[j].ParentId))
			{
				TeamTargetTabData teamTargetTabData = new TeamTargetTabData();
				teamTargetTabData.Reset(text, new List<string>(), teamDataList[j].ID, new List<string>());
				dictionary.Add(teamDataList[j].ID, teamTargetTabData);
			}
			else
			{
				TeamTargetTabData teamTargetTabData2 = dictionary[teamDataList[j].ParentId];
				teamTargetTabData2.SubTitle.Add(text);
				teamTargetTabData2.SubKey.Add(teamDataList[j].ID);
			}
		}
		TeamTargetTabRoot.Reset(new List<TeamTargetTabData>(dictionary.Values), OnClickLeftTab);
		if (string.IsNullOrEmpty(mResetGoalId))
		{
			TeamTargetTabRoot.TargetTabLineList[0].OnClickTab();
			return;
		}
		for (int k = 0; k < TeamTargetTabRoot.TargetTabLineList.Count; k++)
		{
			if (TeamTargetTabRoot.TargetTabLineList[k].Key.Equals(mResetGoalId))
			{
				TeamTargetTabRoot.TargetTabLineList[k].OnClickTab();
			}
		}
	}

	private void OnClickLeftTab(string key)
	{
		get_team_list.request request = new get_team_list.request();
		if (!string.IsNullOrEmpty(key))
		{
			request.goalId = key;
		}
		NetLogic.GetInstance().Send<Protocol.get_team_list>(request);
		ResetPage(key);
	}

	public void ResetPage(string goalId)
	{
		mCurKey = goalId;
		if (mCurKey.Equals("1"))
		{
			NGUITools.SetActive(MatchBtnObj, state: false);
		}
		else
		{
			NGUITools.SetActive(MatchBtnObj, state: true);
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveTeam())
		{
			NGUITools.SetActive(CreateBtn, state: false);
		}
		else
		{
			NGUITools.SetActive(CreateBtn, state: true);
		}
		UpdateMatchingLabel();
		if (mCurTeamList == null || mCurTeamList.Count == 0)
		{
			for (int i = 0; i < TeamLineList.Count; i++)
			{
				UnityVersionUtil.SetActiveRecursive(TeamLineList[i].gameObject, state: false);
			}
			NGUITools.SetActive(NoTeamObj, state: true);
			return;
		}
		NGUITools.SetActive(NoTeamObj, state: false);
		mCurPageTeamList.Clear();
		for (int j = 0; j < mCurTeamList.Count; j++)
		{
			if (mCurTeamList[j].goalId.Equals(mCurKey) && (!mPlayerData.IsHaveTeam() || mCurTeamList[j].id != mPlayerData.TeamInfo.TeamID))
			{
				mCurPageTeamList.Add(mCurTeamList[j]);
			}
		}
		if (mCurPageTeamList.Count == 0)
		{
			NGUITools.SetActive(NoTeamObj, state: true);
		}
		int num = mCurPageTeamList.Count - TeamLineList.Count;
		if (num > 0)
		{
			for (int k = 0; k < num; k++)
			{
				GameObject gameObject = Object.Instantiate(TeamLineList[0].gameObject) as GameObject;
				gameObject.transform.parent = TeamLineGrid.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				TeamLineRoot component = gameObject.GetComponent<TeamLineRoot>();
				TeamLineList.Add(component);
			}
		}
		for (int l = 0; l < TeamLineList.Count; l++)
		{
			if (l < mCurPageTeamList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(TeamLineList[l].gameObject, state: true);
				TeamLineList[l].Reset(mCurPageTeamList[l], mPlayerData.TeamInfo.HasAppliedTeam(mCurPageTeamList[l].id));
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(TeamLineList[l].gameObject, state: false);
			}
		}
		TeamLineGrid.Reposition();
	}

	public void UpdateTeamList(List<team> teamList)
	{
		mCurTeamList = teamList;
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			ResetPage(mCurKey);
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SearchTeamRoot);
	}

	public void OnClickFreshBtn()
	{
		if (Time.time < time)
		{
			NoticeLogic.AddNotifyData("#{100272}");
			return;
		}
		get_team_list.request request = new get_team_list.request();
		if (!string.IsNullOrEmpty(mCurKey))
		{
			request.goalId = mCurKey;
		}
		NetLogic.GetInstance().Send<Protocol.get_team_list>(request);
		time = Time.time + 2f;
	}

	public void OnClickCreateBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SearchTeamRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CreateTeamRoot, delegate
		{
			WaitResponseUIRootLogic.OpenWaitBox(145, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.ask_copyscenes_info>();
			SingletonUnity<CreateTeamRootLogic>.Instance.Reset(isCreate: true, mCurKey);
		});
	}

	public void OnClickMatchingBtn()
	{
		if (string.IsNullOrEmpty(mCurKey) || mCurKey.Equals("1"))
		{
			return;
		}
		mCurCopyScene = DataManager.GetCopySceneDataById(mCurKey);
		if (!CheckLevel())
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveTeam() && !playerData.IsTeamLeader())
		{
			NoticeLogic.AddNotifyData("#{102008}");
			return;
		}
		copyscene_info copyinfoByID = playerData.CopyInfoData.GetCopyinfoByID(mCurKey);
		int num = (int)copyinfoByID.CurNum;
		if (num <= 0)
		{
			if (!string.IsNullOrEmpty(mCurCopyScene.TimeInc))
			{
				PlayerData playerData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
				int itemStackNumById = playerData2.ItemBackPack.GetItemStackNumById(mCurCopyScene.TimeInc);
				if (itemStackNumById > 0)
				{
					ItemData itemDataByID = DataManager.GetItemDataByID(mCurCopyScene.TimeInc);
					MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{101598}", itemDataByID.MName), "#{100127}", OnClickYesUseitemMatch);
					return;
				}
			}
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101540}"));
		}
		else
		{
			MatchFun();
		}
	}

	public bool CheckLevel()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(mCurCopyScene.MinLevel);
	}

	public void OnClickYesUseitemMatch()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ItemContainer itemBackPack = playerData.ItemBackPack;
		List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(itemBackPack, IsAll: false, GameDefine.ITEM_TYPE.REMAIN);
		bool flag = false;
		GameItem gameItem = null;
		if (targetTypeItem == null || targetTypeItem.Count == 0)
		{
			flag = false;
		}
		else
		{
			for (int i = 0; i < targetTypeItem.Count; i++)
			{
				if (targetTypeItem[i].ItemId.Equals(mCurCopyScene.TimeInc))
				{
					flag = true;
					gameItem = targetTypeItem[i];
					break;
				}
			}
		}
		if (flag)
		{
			use_item.request request = new use_item.request();
			request.indexId = gameItem.IndexId;
			NetLogic.GetInstance().Send<Protocol.use_item>(request);
			WaitResponseUIRootLogic.OpenWaitBox(115, 10f, 0f);
			MatchFun();
		}
	}

	public void MatchFun()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		TeamData curCopyTeamData = DataManager.GetTeamDataDataByID(mCurCopyScene.ID);
		if (playerData.IsTeamLeader())
		{
			if (playerData.TeamInfo.IsVertify)
			{
				if (mCurCopyScene.ID.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
				{
					stop_random_select_team.request request = new stop_random_select_team.request();
					request.id = playerData.TeamInfo.TeamGoalData.ID;
					request.type1 = playerData.TeamInfo.TeamGoalData.GoalType;
					NetLogic.GetInstance().Send<Protocol.stop_random_select_team>(request);
				}
				else
				{
					MessageBoxLogic.OpenOKCancelBox("#{103204}", "#{100127}", delegate
					{
						req_change_team_goal.request rpcReq5 = new req_change_team_goal.request
						{
							goalId = mCurCopyScene.ID,
							minLevel = mCurCopyScene.MinLevel,
							maxLevel = mCurCopyScene.MaxLevel,
							isVerfiy = ((!playerData.TeamInfo.IsVertify) ? 1 : 0)
						};
						NetLogic.GetInstance().Send<Protocol.req_change_team_goal>(rpcReq5);
						random_select_team.request rpcReq6 = new random_select_team.request
						{
							id = mCurCopyScene.ID,
							type1 = curCopyTeamData.GoalType
						};
						NetLogic.GetInstance().Send<Protocol.random_select_team>(rpcReq6);
					});
				}
			}
			else if (mCurCopyScene.ID.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
			{
				random_select_team.request request2 = new random_select_team.request();
				request2.id = mCurCopyScene.ID;
				request2.type1 = curCopyTeamData.GoalType;
				NetLogic.GetInstance().Send<Protocol.random_select_team>(request2);
			}
			else
			{
				MessageBoxLogic.OpenOKCancelBox("#{103204}", "#{100127}", delegate
				{
					req_change_team_goal.request rpcReq3 = new req_change_team_goal.request
					{
						goalId = mCurCopyScene.ID,
						minLevel = mCurCopyScene.MinLevel,
						maxLevel = mCurCopyScene.MaxLevel,
						isVerfiy = ((!playerData.TeamInfo.IsVertify) ? 1 : 0)
					};
					NetLogic.GetInstance().Send<Protocol.req_change_team_goal>(rpcReq3);
					random_select_team.request rpcReq4 = new random_select_team.request
					{
						id = mCurCopyScene.ID,
						type1 = curCopyTeamData.GoalType
					};
					NetLogic.GetInstance().Send<Protocol.random_select_team>(rpcReq4);
				});
			}
		}
		else if (!playerData.IsHaveTeam())
		{
			if (playerData.TeamInfo.IsVertify)
			{
				if (mCurCopyScene.ID.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
				{
					stop_random_select_team.request request3 = new stop_random_select_team.request();
					request3.id = playerData.TeamInfo.TeamGoalData.ID;
					request3.type1 = playerData.TeamInfo.TeamGoalData.GoalType;
					NetLogic.GetInstance().Send<Protocol.stop_random_select_team>(request3);
				}
				else
				{
					MessageBoxLogic.OpenOKCancelBox("#{103204}", "#{100127}", delegate
					{
						stop_random_select_team.request rpcReq = new stop_random_select_team.request
						{
							id = playerData.TeamInfo.TeamGoalData.ID,
							type1 = playerData.TeamInfo.TeamGoalData.GoalType
						};
						NetLogic.GetInstance().Send<Protocol.stop_random_select_team>(rpcReq);
						random_select_team.request rpcReq2 = new random_select_team.request
						{
							id = mCurCopyScene.ID,
							type1 = curCopyTeamData.GoalType
						};
						NetLogic.GetInstance().Send<Protocol.random_select_team>(rpcReq2);
					});
				}
			}
			else
			{
				random_select_team.request request4 = new random_select_team.request();
				request4.id = mCurCopyScene.ID;
				request4.type1 = curCopyTeamData.GoalType;
				NetLogic.GetInstance().Send<Protocol.random_select_team>(request4);
			}
		}
		UpdateMatchingLabel();
	}

	public void UpdateMatchingLabel()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsTeamLeader())
		{
			if (playerData.TeamInfo.IsVertify)
			{
				if (mCurKey.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
				{
					MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{102007}");
					SetMatchAnima(isshow: true);
				}
				else
				{
					MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{100826}");
					SetMatchAnima(isshow: false);
				}
			}
			else
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{100826}");
				SetMatchAnima(isshow: false);
			}
		}
		else if (playerData.IsHaveTeam())
		{
			if (playerData.TeamInfo.IsVertify && mCurKey.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{102007}");
				SetMatchAnima(isshow: true);
			}
			else
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{100826}");
				SetMatchAnima(isshow: false);
			}
		}
		else if (playerData.TeamInfo.IsVertify)
		{
			if (mCurKey.Equals(playerData.TeamInfo.TeamGoalData.CopyId))
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{102007}");
				SetMatchAnima(isshow: true);
			}
			else
			{
				MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{100826}");
				SetMatchAnima(isshow: false);
			}
		}
		else
		{
			MatchBtnLabel.text = StrDictionary.GetDictionaryString("#{100826}");
			SetMatchAnima(isshow: false);
		}
	}

	public void SetMatchAnima(bool isshow)
	{
		if (isshow)
		{
			MatchAnima.enabled = true;
			MatchAnima.PlayForward();
			IsMatching = true;
		}
		else
		{
			MatchAnima.ResetToBeginning();
			MatchAnima.enabled = false;
			IsMatching = false;
		}
	}
}
