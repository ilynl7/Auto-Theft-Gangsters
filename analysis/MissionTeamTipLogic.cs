using System;
using System.IO;
using SprotoType;
using UnityEngine;

public class MissionTeamTipLogic : SingletonUnity<MissionTeamTipLogic>
{
	private TutorialManager.OnClickTutorialBtn OnClickTutorialBtn;

	public MissionTipLogic MissionTipRoot;

	public TeamTipLogic TeamTipRoot;

	public DanceTipLogic DanceTipRoot;

	public GameObject MenuObj;

	private bool mIsEnterDanceArea;

	public TweenAlpha TwHideBtnPic;

	public TweenPosition TwControllerRoot;

	public UISprite TeamBtn;

	public UISprite MissionBtn;

	public static int mCurPage = -1;

	private int mPrePage = -1;

	private bool IsHideFlag;

	public GameObject DanceBtn;

	public UIScrollView TipScrollView;

	public UISprite TipsFlag;

	public UISprite MissionTips;

	public GameObject MatchBtn;

	public UILabel TimeLabel;

	public UISprite NetStateSprite;

	public UISlider BatterySlider;

	private float startTime;

	private float startTime2;

	private float startTime3;

	private string[] wifiName = new string[4] { "CZ_WIFI_3", "CZ_WIFI_2", "CZ_WIFI_1", "CZ_WIFI_NOWIFI" };

	private string[] mobileName = new string[5] { "CZ_4G_4", "CZ_4G_3", "CZ_4G_2", "CZ_4G_1", "CZ_4G_NO4G" };

	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn
	{
		get
		{
			return OnClickTutorialBtn;
		}
		set
		{
			OnClickTutorialBtn = value;
		}
	}

	public bool IsEnterDanceArea
	{
		get
		{
			return mIsEnterDanceArea;
		}
		set
		{
			mIsEnterDanceArea = value;
		}
	}

	public int CurPage => mCurPage;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
		}
	}

	public void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	public void EnableReset()
	{
		NGUITools.SetActive(MissionTipRoot.gameObject, state: false);
		NGUITools.SetActive(TeamTipRoot.gameObject, state: false);
		NGUITools.SetActive(MenuObj.gameObject, state: false);
		NGUITools.SetActive(DanceTipRoot.gameObject, state: false);
		startTime = 300f;
		startTime2 = 300f;
		startTime3 = 10f;
		IsHideFlag = false;
		TwHideBtnPic.ResetToBeginning();
		if (mCurPage == 1)
		{
			mCurPage = -1;
			OnClickMissionBtn();
		}
		else
		{
			mCurPage = -1;
			OnClickMenuBtn();
		}
	}

	public void CheckTeamTips()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsTeamLeader())
		{
			if (playerData.TeamInfo.ApplyMemberDic.Count > 0)
			{
				TipsFlag.enabled = true;
			}
			else
			{
				TipsFlag.enabled = false;
			}
		}
		else
		{
			TipsFlag.enabled = false;
		}
	}

	public void UpdateMatchBtn()
	{
		if (mCurPage == 2)
		{
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveTeam())
			{
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.IsVertify)
				{
					NGUITools.SetActive(MatchBtn.gameObject, state: true);
				}
				else
				{
					NGUITools.SetActive(MatchBtn.gameObject, state: false);
				}
			}
		}
		else
		{
			NGUITools.SetActive(MatchBtn.gameObject, state: false);
		}
	}

	public void OnClickMatchBtn()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveTeam() && !playerData.IsTeamLeader())
		{
			NoticeLogic.AddNotifyData("#{102008}");
		}
		else if (playerData.TeamInfo.IsVertify)
		{
			MessageBoxLogic.OpenOKCancelBox("#{103205}", "#{100127}", delegate
			{
				stop_random_select_team.request rpcReq = new stop_random_select_team.request
				{
					id = playerData.TeamInfo.TeamGoalData.ID,
					type1 = playerData.TeamInfo.TeamGoalData.GoalType
				};
				NetLogic.GetInstance().Send<Protocol.stop_random_select_team>(rpcReq);
				NGUITools.SetActive(MatchBtn.gameObject, state: false);
			});
		}
	}

	public void UpdateUnlockTips()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData.MenuTabBtnTipIdList.Contains(3018.ToString()))
		{
			FunctionTipsRootLogic.AddFunctionTips(TeamBtn.gameObject, Vector3.zero, -1f);
		}
		else
		{
			FunctionTipsRootLogic.RemoveFunctionTips(TeamBtn.gameObject);
		}
	}

	public void SetTeamApplyTips(bool ishave)
	{
		TipsFlag.enabled = ishave;
	}

	public void OnClickHideBtn()
	{
		if (IsHideFlag)
		{
			IsHideFlag = false;
			if (TutorialManager.CurStep == TUTORIAL_STEP.MAIN_MISSION_PHONE_START)
			{
				TwHideBtnPic.ResetToBeginning();
				TwControllerRoot.ResetToBeginning();
				CheckTutorialEvent();
			}
			else
			{
				TwHideBtnPic.PlayReverse();
				TwControllerRoot.PlayReverse();
			}
		}
		else
		{
			IsHideFlag = true;
			TwHideBtnPic.PlayForward();
			TwControllerRoot.PlayForward();
		}
	}

	public void SetToCloseState()
	{
		IsHideFlag = true;
		TwHideBtnPic.PlayForward();
		TwControllerRoot.PlayForward();
		TwHideBtnPic.enabled = false;
		TwControllerRoot.enabled = false;
		TwHideBtnPic.value = TwHideBtnPic.to;
		TwControllerRoot.value = TwControllerRoot.to;
	}

	public void OnClickTeamBtn()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.TEAM))
		{
			if (mCurPage == 2)
			{
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveTeam())
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamRootNew);
				}
			}
			else
			{
				ShowTeamTip();
			}
		}
		else
		{
			int condition = DataManager.GetFunctionDataById(3018.ToString()).Condition;
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", condition));
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.MenuTabBtnTipIdList.Contains(3018.ToString()))
		{
			FunctionTipsRootLogic.RemoveFunctionTips(TeamBtn.gameObject);
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.MenuTabBtnTipIdList.Remove(3018.ToString());
			UpdateUnlockTips();
		}
	}

	public void OnClickMenuBtn()
	{
		if (mCurPage != 0)
		{
			mPrePage = mCurPage;
			mCurPage = 0;
			ShowMenuInfo();
		}
	}

	public void ShowMenuInfo()
	{
		NGUITools.SetActive(MissionTipRoot.gameObject, state: false);
		NGUITools.SetActive(TeamTipRoot.gameObject, state: false);
		NGUITools.SetActive(MenuObj.gameObject, state: true);
		NGUITools.SetActive(DanceTipRoot.gameObject, state: false);
		if (IsEnterDanceArea)
		{
			NGUITools.SetActive(DanceBtn.gameObject, state: true);
		}
		else
		{
			NGUITools.SetActive(DanceBtn.gameObject, state: false);
		}
		UpdateUnlockTips();
		CheckTeamTips();
		MissionTips.enabled = false;
	}

	public void OnClickBackBtn()
	{
		if (CurPage != 0)
		{
			OnClickMenuBtn();
		}
	}

	public void OnClickCloseBtn()
	{
		OnClickHideBtn();
	}

	public void OnClickMissionBtn()
	{
		if (mCurPage == 1)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
			{
				SingletonUnity<UIManager>.Instance.CloseAllPOPUI();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickMissionBtn();
				});
			}
		}
		else
		{
			ShowMissionTip();
			if (TutorialManager.CurStep == TUTORIAL_STEP.CREATE_TEAM_SWITCH_MISSION)
			{
				CheckTutorialEvent();
			}
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.MAIN_MISSION_CLICK_MISSION_TAB)
		{
			CheckTutorialEvent();
		}
	}

	public void OnClickDanceBtn()
	{
		if (CurPage != 3)
		{
			ShowDanceTip();
		}
	}

	public void ChangeToDanceMission(bool IsEnterDance)
	{
		if (IsEnterDanceArea == IsEnterDance)
		{
			return;
		}
		IsEnterDanceArea = IsEnterDance;
		if (IsEnterDanceArea)
		{
			NGUITools.SetActive(MissionTipRoot.gameObject, state: false);
			NGUITools.SetActive(TeamTipRoot.gameObject, state: false);
			NGUITools.SetActive(MenuObj.gameObject, state: false);
			NGUITools.SetActive(DanceTipRoot.gameObject, state: true);
			ShowDanceTip();
		}
		else if (mCurPage == 3)
		{
			if (mPrePage == 1)
			{
				OnClickMissionBtn();
			}
			else if (mPrePage == 2)
			{
				OnClickTeamBtn();
			}
			else
			{
				OnClickMenuBtn();
			}
		}
		else if (mCurPage == 0)
		{
			NGUITools.SetActive(DanceBtn.gameObject, state: false);
		}
	}

	public void ShowNewMissionFlag()
	{
		if (mCurPage != 1)
		{
			MissionTips.enabled = true;
		}
	}

	public void ShowMissionTip()
	{
		if (mCurPage != 1)
		{
			mPrePage = mCurPage;
			mCurPage = 1;
			NGUITools.SetActive(MissionTipRoot.gameObject, state: true);
			NGUITools.SetActive(TeamTipRoot.gameObject, state: false);
			NGUITools.SetActive(MenuObj.gameObject, state: false);
			NGUITools.SetActive(DanceTipRoot.gameObject, state: false);
			MissionTipRoot.Reset();
			TipScrollView.ResetPosition();
		}
	}

	public void ShowTeamTip()
	{
		if (mCurPage != 2)
		{
			mPrePage = mCurPage;
			mCurPage = 2;
			NGUITools.SetActive(MissionTipRoot.gameObject, state: false);
			NGUITools.SetActive(TeamTipRoot.gameObject, state: true);
			NGUITools.SetActive(MenuObj.gameObject, state: false);
			NGUITools.SetActive(DanceTipRoot.gameObject, state: false);
			TeamTipRoot.Reset();
			TipScrollView.ResetPosition();
			UpdateMatchBtn();
		}
	}

	public void ShowDanceTip()
	{
		request_dance_state_info.request rpcReq = new request_dance_state_info.request();
		NetLogic.GetInstance().Send<Protocol.request_dance_state_info>(rpcReq);
		if (mCurPage != 3)
		{
			mPrePage = mCurPage;
			mCurPage = 3;
			NGUITools.SetActive(MissionTipRoot.gameObject, state: false);
			NGUITools.SetActive(TeamTipRoot.gameObject, state: false);
			NGUITools.SetActive(MenuObj.gameObject, state: false);
			NGUITools.SetActive(DanceTipRoot.gameObject, state: true);
			DanceTipRoot.Reset();
			TipScrollView.ResetPosition();
		}
	}

	public void ResetMissionTip()
	{
		MissionTipRoot.Reset();
	}

	public void AddMission(string missionId)
	{
		if (mCurPage == 1)
		{
			MissionTipRoot.Reset();
		}
	}

	public void RemoveMission(string missionId)
	{
		if (mCurPage == 1)
		{
			MissionTipRoot.Reset();
		}
	}

	public void UpdateMission(string missionId)
	{
		if (mCurPage == 1)
		{
			MissionTipRoot.UpdateMission(missionId);
		}
	}

	public void UpdateDanceInfo()
	{
		if (CurPage == 3)
		{
			DanceTipRoot.Reset();
		}
	}

	private void Update()
	{
		startTime += Time.deltaTime;
		startTime2 += Time.deltaTime;
		startTime3 += Time.deltaTime;
		if (startTime >= 180f)
		{
			UpdataBattery();
			startTime = 0f;
		}
		if (startTime2 >= 60f)
		{
			UpdateMinute();
			startTime2 = 0f;
		}
		if (startTime3 >= 0.1f)
		{
			UpdateDelayTime();
			startTime3 = 0f;
		}
	}

	private string GetNetStateSpriteName(int time, int netState)
	{
		if (time > 0)
		{
			int num = -1;
			num = ((time >= 50) ? ((time < 200) ? 1 : ((time >= 500) ? 3 : 2)) : 0);
			switch (netState)
			{
			case 0:
				if (num > 2)
				{
					num = 2;
				}
				return wifiName[num];
			case 1:
				return mobileName[num];
			}
		}
		else
		{
			switch (netState)
			{
			case 0:
				return wifiName[3];
			case 1:
				return mobileName[4];
			}
		}
		return wifiName[3];
	}

	private void UpdateDelayTime()
	{
		int netState = -1;
		int netDelayTime = SingletonDontDestoryUnity<NetManager>.Instance.NetDelayTime;
		if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
		{
			netState = 0;
		}
		else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
		{
			netState = 1;
		}
		NetStateSprite.spriteName = GetNetStateSpriteName(netDelayTime, netState);
		NetStateSprite.MakePixelPerfect();
	}

	private void UpdataBattery()
	{
		int batteryLevel = GetBatteryLevel();
		BatterySlider.value = Mathf.Clamp01((float)batteryLevel / 100f);
	}

	private int GetBatteryLevel()
	{
		int num = 50;
		try
		{
			string s = File.ReadAllText("/sys/class/power_supply/battery/capacity");
			return int.Parse(s);
		}
		catch (Exception)
		{
			return SingletonDontDestoryUnity<GameManager>.Instance.GetBatteryState();
		}
	}

	private void UpdateMinute()
	{
		DateTime now = DateTime.Now;
		TimeLabel.text = $"{now.Hour:D2}:{now.Minute:D2}";
	}
}
