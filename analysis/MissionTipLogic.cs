using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTipLogic : MonoBehaviour
{
	public int LineHeight = 56;

	public MissionTipLineLogic TipLinePrefab;

	public List<MissionTipLineLogic> MissionTipLineList;

	private List<string> mCurMissionIdList = new List<string>();

	public UIWrapContentNew uiWrapContent;

	public UIWidget BottomWidget;

	public GameObject HandTipRoot;

	public UIPanel TipPanel;

	private float lastShowHandTipTime = -9f;

	private int missionTipMaxId = 18;

	private int NewMissionminId = 1000;

	private int NewMissionmaxId = 2000;

	private float CheckHandTipInterval = 10f;

	private MissionManager misManager;

	private bool flag;

	public void CheckMainMissionHandTip()
	{
		CloseHandTip();
	}

	public void CloseHandTip()
	{
		NGUITools.SetActive(HandTipRoot, state: false);
		lastShowHandTipTime = Time.time;
	}

	public void ShowMainMissionTip()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.MAIN_MISSION_START)
		{
			return;
		}
		if ((!SingletonUnity<TutorialUIRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject)) && !FunctionTipsRootLogic.IsHandTipEnable())
		{
			MissionTipLineLogic targetMissionLine = GetTargetMissionLine(1);
			if (targetMissionLine != null)
			{
				lastShowHandTipTime = Time.time;
				NGUITools.SetActive(HandTipRoot, state: true);
				HandTipRoot.transform.position = targetMissionLine.transform.position - (Vector3.up * 30f - Vector3.right * 50f) * targetMissionLine.transform.lossyScale.y;
			}
			else
			{
				NGUITools.SetActive(HandTipRoot, state: false);
			}
		}
		else
		{
			NGUITools.SetActive(HandTipRoot, state: false);
		}
	}

	public void ShowSideMissionTip(string misId)
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.MAIN_MISSION_START)
		{
			return;
		}
		if ((!SingletonUnity<TutorialUIRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject)) && !FunctionTipsRootLogic.IsHandTipEnable())
		{
			MissionTipLineLogic targetMissionLineById = GetTargetMissionLineById(misId);
			if (targetMissionLineById != null)
			{
				lastShowHandTipTime = Time.time;
				NGUITools.SetActive(HandTipRoot, state: true);
				HandTipRoot.transform.position = targetMissionLineById.transform.position - (Vector3.up * 30f - Vector3.right * 50f) * targetMissionLineById.transform.lossyScale.y;
			}
			else
			{
				NGUITools.SetActive(HandTipRoot, state: false);
			}
		}
		else
		{
			NGUITools.SetActive(HandTipRoot, state: false);
		}
	}

	public void UpdateLastShowHandTipTime()
	{
		lastShowHandTipTime = Time.time;
	}

	private void Update()
	{
		if (Time.time - lastShowHandTipTime > CheckHandTipInterval)
		{
			lastShowHandTipTime = Time.time;
			if (UnityVersionUtil.IsActive(HandTipRoot))
			{
				CloseHandTip();
			}
		}
	}

	private void Awake()
	{
		uiWrapContent.onInitializeItem = OnInitializeItem;
	}

	public void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		realIndex = Mathf.Abs(realIndex);
		if (mCurMissionIdList.Count > realIndex)
		{
			MissionTipLineList[index].Reset(mCurMissionIdList[Mathf.Abs(realIndex)]);
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.DAILY_MISSION_CLICK_START)
		{
			FunctionTipsRootLogic.ClearHandTip();
		}
	}

	public void Reset()
	{
		SingletonUnity<MissionTeamTipLogic>.Instance.TipScrollView.ResetPosition();
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		mCurMissionIdList = missionManager.GetAllMissionId();
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		for (int num = mCurMissionIdList.Count - 1; num >= 0; num--)
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(mCurMissionIdList[num]);
			if (level < missionDataByID.DisplayLv)
			{
				mCurMissionIdList.RemoveAt(num);
			}
			else if (missionDataByID.Class == 1)
			{
				text = missionDataByID.ID;
			}
			else if (missionDataByID.Class == 0 || missionDataByID.Class == 3 || missionDataByID.Class == 6)
			{
				text2 = missionDataByID.ID;
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			text3 = text2;
		}
		else if (!string.IsNullOrEmpty(text))
		{
			text3 = text;
		}
		BottomWidget.height = LineHeight * mCurMissionIdList.Count;
		int num2 = 0;
		num2 = mCurMissionIdList.Count - MissionTipLineList.Count;
		for (int i = 0; i < MissionTipLineList.Count; i++)
		{
			NGUITools.SetActive(MissionTipLineList[i].gameObject, state: true);
		}
		if (num2 < 0)
		{
			for (int num3 = 0; num3 > num2; num3--)
			{
				NGUITools.SetActive(MissionTipLineList[MissionTipLineList.Count + num3 - 1].gameObject, state: false);
			}
		}
		mCurMissionIdList.Sort(delegate(string x, string y)
		{
			long missionChangeTime = missionManager.GetMissionChangeTime(x);
			long missionChangeTime2 = missionManager.GetMissionChangeTime(y);
			if (missionChangeTime != missionChangeTime2)
			{
				return (int)(missionChangeTime2 - missionChangeTime);
			}
			return (x.Length == y.Length) ? x.CompareTo(y) : (x.Length - y.Length);
		});
		List<string> list = new List<string>();
		for (int num4 = mCurMissionIdList.Count - 1; num4 >= 0; num4--)
		{
			if (mCurMissionIdList[num4].Equals(text) || mCurMissionIdList[num4].Equals(text2))
			{
				mCurMissionIdList.RemoveAt(num4);
			}
			else if (missionManager.GetMissionState(mCurMissionIdList[num4]) == MISSION_STATE.COMPLETE)
			{
				list.Add(mCurMissionIdList[num4]);
				mCurMissionIdList.RemoveAt(num4);
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			mCurMissionIdList.Insert(0, text2);
		}
		if (!string.IsNullOrEmpty(text))
		{
			mCurMissionIdList.Insert(0, text);
		}
		if (list.Count > 0)
		{
			int num5 = 0;
			num5 = ((!string.IsNullOrEmpty(text3)) ? (mCurMissionIdList.IndexOf(text3) + 1) : 0);
			for (int j = 0; j < list.Count; j++)
			{
				mCurMissionIdList.Insert(num5, list[j]);
			}
		}
		uiWrapContent.minIndex = -(mCurMissionIdList.Count - 1);
		uiWrapContent.SortBasedOnScrollMovement();
		CheckMainMissionHandTip();
	}

	public void UpdateMission(string missionId)
	{
		for (int i = 0; i < MissionTipLineList.Count; i++)
		{
			if (MissionTipLineList[i].MissionId.Equals(missionId))
			{
				MissionTipLineList[i].Reset(missionId);
				break;
			}
		}
	}

	public MissionTipLineLogic GetDailyMissionLine()
	{
		for (int i = 0; i < MissionTipLineList.Count; i++)
		{
			if (!string.IsNullOrEmpty(MissionTipLineList[i].MissionId) && UnityVersionUtil.IsActive(MissionTipLineList[i].gameObject))
			{
				MissionData missionDataByID = DataManager.GetMissionDataByID(MissionTipLineList[i].MissionId);
				if (missionDataByID.Class == 0 || missionDataByID.Class == 3 || missionDataByID.Class == 6)
				{
					return MissionTipLineList[i];
				}
			}
		}
		return null;
	}

	public MissionTipLineLogic GetTargetMissionLine(int missionClass)
	{
		for (int i = 0; i < MissionTipLineList.Count; i++)
		{
			if (!string.IsNullOrEmpty(MissionTipLineList[i].MissionId) && UnityVersionUtil.IsActive(MissionTipLineList[i].gameObject))
			{
				MissionData missionDataByID = DataManager.GetMissionDataByID(MissionTipLineList[i].MissionId);
				if (missionDataByID.Class == missionClass)
				{
					return MissionTipLineList[i];
				}
			}
		}
		return null;
	}

	public MissionTipLineLogic GetTargetMissionLineById(string misId)
	{
		for (int i = 0; i < MissionTipLineList.Count; i++)
		{
			if (!string.IsNullOrEmpty(MissionTipLineList[i].MissionId) && UnityVersionUtil.IsActive(MissionTipLineList[i].gameObject) && MissionTipLineList[i].MissionId.Equals(misId))
			{
				return MissionTipLineList[i];
			}
		}
		return null;
	}

	public bool IsHaveMainLineMission()
	{
		if (GetTargetMissionLine(1) == null)
		{
			return false;
		}
		return true;
	}

	public bool IsHaveDailyMission()
	{
		if (GetDailyMissionLine() == null)
		{
			return false;
		}
		return true;
	}

	private void OnReshowBase()
	{
		if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(SetPanelDirty());
		}
	}

	private IEnumerator SetPanelDirty()
	{
		yield return null;
		TipPanel.SetDirty();
	}

	private void OnEnable()
	{
		UIUpdateEvent.OnReshowBase = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.OnReshowBase, new UIUpdateEvent.UpdateNoParamEvent(OnReshowBase));
	}

	private void OnDisable()
	{
		UIUpdateEvent.OnReshowBase = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.OnReshowBase, new UIUpdateEvent.UpdateNoParamEvent(OnReshowBase));
	}
}
