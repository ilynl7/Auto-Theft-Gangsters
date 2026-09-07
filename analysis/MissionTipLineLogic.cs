using System;
using UnityEngine;

public class MissionTipLineLogic : MonoBehaviour
{
	private TutorialManager.OnClickTutorialBtn OnClickTutorialBtn;

	public UILabel MissionInfoLabel;

	public UILabel MissionStateLabel;

	private string mMissionId = string.Empty;

	public GameObject CompleteTipObj;

	private MissionData mCurMissionData;

	private int curLevel;

	public UISprite BottomPic;

	public UISprite TimeCountBottomPic;

	public UISprite TimeClockPic;

	private long mTotalTime;

	private float mTimeCount;

	private MissionManager missionManager;

	private long restTime;

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

	public string MissionId
	{
		get
		{
			return mMissionId;
		}
		set
		{
			mMissionId = value;
		}
	}

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
			SingletonUnity<MissionTeamTipLogic>.Instance.TipScrollView.ResetPosition();
		}
	}

	public void Reset(string missionId)
	{
		curLevel = 0;
		if (!mMissionId.Equals(missionId))
		{
			mOnClickTutorialBtn = null;
		}
		mMissionId = missionId;
		mCurMissionData = DataManager.GetMissionDataByID(mMissionId);
		missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		MISSION_STATE missionState = missionManager.GetMissionState(mMissionId);
		string empty = string.Empty;
		switch (mCurMissionData.Class)
		{
		case 0:
			empty = StrDictionary.GetDictionaryString("#{100168}", missionManager.GetMissionParam(missionId, 3) + 1);
			break;
		case 1:
			empty = StrDictionary.GetDictionaryString("#{100169}");
			break;
		case 2:
			empty = StrDictionary.GetDictionaryString("#{100170}");
			break;
		case 3:
		case 6:
			empty = StrDictionary.GetDictionaryString("#{100171}");
			break;
		case 4:
			empty = StrDictionary.GetDictionaryString("#{100172}");
			break;
		case 5:
			empty = StrDictionary.GetDictionaryString("#{100173}");
			break;
		case 7:
			empty = StrDictionary.GetDictionaryString("#{100200}");
			break;
		case 8:
			empty = StrDictionary.GetDictionaryString("#{100001}");
			break;
		default:
			empty = $"[{mCurMissionData.Class}]";
			break;
		}
		if (mCurMissionData.Class == 0)
		{
			MissionInfoLabel.text = StrDictionary.GetDictionaryString("#{100171}") + "[FDAE33]" + StrDictionary.GetDictionaryString(mCurMissionData.TipDescribeID) + empty + "[-]";
		}
		else
		{
			MissionInfoLabel.text = empty + "[FDAE33]" + StrDictionary.GetDictionaryString(mCurMissionData.TipDescribeID) + "[-]";
		}
		MissionManager.GetMissionStateLabel(mCurMissionData, missionState, MissionStateLabel);
		ShowCompleteEffect(missionState);
		if (mCurMissionData.Class == 8)
		{
			TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(mCurMissionData.TimeLimitId);
			MissionInfoLabel.text = empty + "[FDAE33]" + timeLimitMissionDataByID.MName + "[-]";
			mTotalTime = timeLimitMissionDataByID.LimitTime;
			restTime = missionManager.GetMissionRestTime(mCurMissionData.ID);
			if (restTime <= 0)
			{
				TimeCountBottomPic.enabled = false;
				mTimeCount = 0.01f;
			}
			else
			{
				TimeCountBottomPic.enabled = true;
			}
			TimeClockPic.enabled = true;
		}
		else
		{
			TimeCountBottomPic.enabled = false;
			TimeClockPic.enabled = false;
		}
	}

	private void Update()
	{
		if (mCurMissionData == null || mCurMissionData.Class != 8)
		{
			return;
		}
		mTimeCount -= Time.deltaTime;
		if (!(mTimeCount < 0f))
		{
			return;
		}
		mTimeCount = 1f;
		restTime = missionManager.GetMissionRestTime(mCurMissionData.ID);
		if (restTime <= 0)
		{
			missionManager.AbandonMission(mCurMissionData.ID, isForced: true);
			mCurMissionData = null;
			TimeCountBottomPic.enabled = false;
			return;
		}
		if (!TimeCountBottomPic.enabled)
		{
			TimeCountBottomPic.enabled = true;
		}
		float num = (float)restTime / (float)mTotalTime;
		TimeCountBottomPic.width = (int)((float)(BottomPic.width - 6) * num);
	}

	private void OnEnable()
	{
		curLevel = 0;
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(RefreshMission));
	}

	private void OnDisable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(RefreshMission));
	}

	public void RefreshMission()
	{
		if (mCurMissionData != null)
		{
			int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
			if (curLevel != level)
			{
				MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
				MISSION_STATE missionState = missionManager.GetMissionState(mMissionId);
				MissionManager.GetMissionStateLabel(mCurMissionData, missionState, MissionStateLabel);
				ShowCompleteEffect(missionState);
			}
			curLevel = level;
		}
	}

	public void ShowCompleteEffect(MISSION_STATE curMissionState)
	{
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		if (mCurMissionData.Class == 1)
		{
			int num = 15;
			FunctionData functionDataById = DataManager.GetFunctionDataById(4087.ToString());
			if (functionDataById != null)
			{
				num = functionDataById.Condition;
			}
			if (level < num)
			{
				if (UnityVersionUtil.IsActive(base.gameObject))
				{
					UnityVersionUtil.SetActiveRecursive(CompleteTipObj.gameObject, state: true);
				}
				return;
			}
		}
		if (mCurMissionData.MinLv > level)
		{
			UnityVersionUtil.SetActiveRecursive(CompleteTipObj.gameObject, state: false);
		}
		else if (curMissionState == MISSION_STATE.COMPLETE)
		{
			if (UnityVersionUtil.IsActive(base.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(CompleteTipObj.gameObject, state: true);
			}
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(CompleteTipObj.gameObject, state: false);
		}
	}

	public void OnClickMissionLine()
	{
		if (mCurMissionData != null)
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.MAIN_MISSION_START || TutorialManager.CurStep == TUTORIAL_STEP.MAIN_MISSION_TIP_START || TutorialManager.CurStep == TUTORIAL_STEP.DAILY_MISSION_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.SIDE_MISSION_CLICK_START || TutorialManager.CurStep == TUTORIAL_STEP.SIDE_MISSION_CAR_TIP_START || TutorialManager.CurStep == TUTORIAL_STEP.MAIN_MISSION_CLICK_MISSION)
			{
				CheckTutorialEvent();
			}
			MissionManager.ClickMissionAction(mCurMissionData.ID);
			SingletonUnity<MissionTeamTipLogic>.Instance.MissionTipRoot.CloseHandTip();
		}
	}
}
