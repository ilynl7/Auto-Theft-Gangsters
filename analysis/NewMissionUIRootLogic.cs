using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMissionUIRootLogic : SingletonUnity<NewMissionUIRootLogic>
{
	public List<NewMissionLineLogic> MissionLineList = new List<NewMissionLineLogic>();

	public UITable TableRoot;

	public UIScrollView ScrollView;

	private List<MissionData> mCurMissionList = new List<MissionData>();

	private List<MissionData> allSideMissionList = new List<MissionData>();

	private List<MissionData> curAcceptableMissionDataList = new List<MissionData>();

	private List<MissionData> curAcceptedMissionList = new List<MissionData>();

	private string mTargetMissionId = string.Empty;

	private NewMissionLineLogic mCurMissionLine;

	public void Reset(string targetMissionId)
	{
		curAcceptableMissionDataList.Clear();
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		allSideMissionList = DataManager.GetAllSideMissionList();
		if (allSideMissionList != null)
		{
			for (int i = 0; i < allSideMissionList.Count; i++)
			{
				if (missionManager.IsMissionAcceptable(allSideMissionList[i].ID))
				{
					curAcceptableMissionDataList.Add(allSideMissionList[i]);
				}
			}
		}
		curAcceptableMissionDataList.Sort((MissionData x, MissionData y) => x.ShowRank - y.ShowRank);
		curAcceptedMissionList.Clear();
		List<string> acceptedMissionList = GetAcceptedMissionList();
		for (int j = 0; j < acceptedMissionList.Count; j++)
		{
			curAcceptedMissionList.Add(DataManager.GetMissionDataByID(acceptedMissionList[j]));
		}
		int num = curAcceptableMissionDataList.Count + curAcceptedMissionList.Count - MissionLineList.Count;
		if (num > 0)
		{
			GameObject gameObject = null;
			for (int k = 0; k < num; k++)
			{
				gameObject = Object.Instantiate(MissionLineList[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{MissionLineList.Count + 1:D3}";
				gameObject.transform.parent = MissionLineList[0].transform.parent;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				MissionLineList.Add(gameObject.GetComponent<NewMissionLineLogic>());
			}
		}
		int num2 = curAcceptableMissionDataList.Count + curAcceptedMissionList.Count;
		for (int l = 0; l < MissionLineList.Count; l++)
		{
			NGUITools.SetActive(MissionLineList[l].gameObject, state: false);
		}
		if (string.IsNullOrEmpty(targetMissionId))
		{
			SingletonUnity<NewMapUIRootLogic>.Instance.Reset(sceneManager.CurrentMapInofData.ID);
		}
		else
		{
			MISSION_STATE missionState = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetMissionState(targetMissionId);
			MissionData missionDataByID = DataManager.GetMissionDataByID(targetMissionId);
			string text = string.Empty;
			switch (missionState)
			{
			case MISSION_STATE.INVALID:
				text = missionDataByID.AcceptMapId;
				break;
			case MISSION_STATE.ACCEPTED:
				text = missionDataByID.TargetMapId;
				break;
			case MISSION_STATE.COMPLETE:
				text = missionDataByID.SubmitMapId;
				break;
			}
			if (string.IsNullOrEmpty(text))
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID);
			}
			else
			{
				SingletonUnity<NewMapUIRootLogic>.Instance.Reset(text);
			}
		}
		if (string.IsNullOrEmpty(targetMissionId))
		{
			SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjNormal();
		}
		mCurMissionLine = null;
		if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(DelayReposition());
		}
		mTargetMissionId = targetMissionId;
	}

	public void ClickTargetMission(string missionId)
	{
		if (mCurMissionLine != null)
		{
			mCurMissionLine.CloseLine(directClose: true);
		}
		if (string.IsNullOrEmpty(missionId))
		{
			return;
		}
		for (int i = 0; i < MissionLineList.Count; i++)
		{
			if (UnityVersionUtil.IsActive(MissionLineList[i].gameObject) && MissionLineList[i].curMissionId.Equals(missionId))
			{
				MissionLineList[i].OnClickItemLine();
				MissionLineList[i].MyCenterOn.CenterOn(MissionLineList[i].transform);
			}
		}
	}

	private List<string> GetAcceptedMissionList()
	{
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		List<string> allMissionId = missionManager.GetAllMissionId();
		string text = string.Empty;
		string text2 = string.Empty;
		string text3 = string.Empty;
		for (int num = allMissionId.Count - 1; num >= 0; num--)
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(allMissionId[num]);
			if (level < missionDataByID.DisplayLv)
			{
				allMissionId.RemoveAt(num);
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
		allMissionId.Sort(delegate(string x, string y)
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
		for (int num2 = allMissionId.Count - 1; num2 >= 0; num2--)
		{
			if (allMissionId[num2].Equals(text) || allMissionId[num2].Equals(text2))
			{
				allMissionId.RemoveAt(num2);
			}
			else if (missionManager.GetMissionState(allMissionId[num2]) == MISSION_STATE.COMPLETE)
			{
				list.Add(allMissionId[num2]);
				allMissionId.RemoveAt(num2);
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			allMissionId.Insert(0, text2);
		}
		if (!string.IsNullOrEmpty(text))
		{
			allMissionId.Insert(0, text);
		}
		if (list.Count > 0)
		{
			int num3 = 0;
			num3 = ((!string.IsNullOrEmpty(text3)) ? (allMissionId.IndexOf(text3) + 1) : 0);
			for (int i = 0; i < list.Count; i++)
			{
				allMissionId.Insert(num3, list[i]);
			}
		}
		return allMissionId;
	}

	private IEnumerator DelayReposition()
	{
		yield return null;
		int allMisCount = curAcceptableMissionDataList.Count + curAcceptedMissionList.Count;
		for (int j = 0; j < MissionLineList.Count; j++)
		{
			if (j < allMisCount)
			{
				NGUITools.SetActive(MissionLineList[j].gameObject, state: true);
				if (j < curAcceptedMissionList.Count)
				{
					MissionLineList[j].ResetLine(curAcceptedMissionList[j], OnClickItemLine);
				}
				else
				{
					MissionLineList[j].ResetLine(curAcceptableMissionDataList[j - curAcceptedMissionList.Count], OnClickItemLine);
				}
			}
			else
			{
				NGUITools.SetActive(MissionLineList[j].gameObject, state: false);
			}
		}
		TableRoot.Reposition();
		ScrollView.ResetPosition();
		if (!string.IsNullOrEmpty(mTargetMissionId))
		{
			for (int i = 0; i < MissionLineList.Count; i++)
			{
				if (UnityVersionUtil.IsActive(MissionLineList[i].gameObject) && MissionLineList[i].curMissionId.Equals(mTargetMissionId))
				{
					MissionLineList[i].OnClickItemLine();
					MissionLineList[i].MyCenterOn.CenterOn(MissionLineList[i].transform);
				}
			}
		}
		MissionLineList[MissionLineList.Count - 1].InfoTween.updateTable = false;
		mTargetMissionId = string.Empty;
	}

	public void OnClickItemLine(NewMissionLineLogic clickLine)
	{
		if (mCurMissionLine == clickLine)
		{
			mCurMissionLine = null;
			SingletonUnity<NewMapUIRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID);
			SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjNormal();
			return;
		}
		if (mCurMissionLine != null)
		{
			mCurMissionLine.CloseLine(directClose: true);
		}
		mCurMissionLine = clickLine;
		mCurMissionLine.MyCenterOn.CenterOn(mCurMissionLine.transform);
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		if (missionManager.IsMissionAccepted(mCurMissionLine.curMissionId))
		{
			switch (missionManager.GetMissionState(mCurMissionLine.CurMissionData.ID))
			{
			case MISSION_STATE.ACCEPTED:
				if (mCurMissionLine.CurMissionData.MissionLogicType == MISSION_LOGICTYPE.SURVEY)
				{
					SurveyMissionData surveyMissionDataById = DataManager.GetSurveyMissionDataById(mCurMissionLine.CurMissionData.LogicID);
					SingletonUnity<NewMapUIRootLogic>.Instance.Reset(surveyMissionDataById.SceneID);
					SingletonUnity<NewMapUIRootLogic>.Instance.ChooseActivityObj(mCurMissionLine.curMissionId, isMission: true);
				}
				else if (string.IsNullOrEmpty(mCurMissionLine.CurMissionData.TargetMapId))
				{
					SingletonUnity<NewMapUIRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID);
					SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjNormal();
				}
				else
				{
					SingletonUnity<NewMapUIRootLogic>.Instance.Reset(mCurMissionLine.CurMissionData.TargetMapId);
					SingletonUnity<NewMapUIRootLogic>.Instance.ChooseActivityObj(mCurMissionLine.curMissionId, isMission: true);
				}
				break;
			case MISSION_STATE.COMPLETE:
				if (string.IsNullOrEmpty(mCurMissionLine.CurMissionData.SubmitMapId))
				{
					SingletonUnity<NewMapUIRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID);
					SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjNormal();
				}
				else
				{
					SingletonUnity<NewMapUIRootLogic>.Instance.Reset(mCurMissionLine.CurMissionData.SubmitMapId);
					SingletonUnity<NewMapUIRootLogic>.Instance.ChooseActivityObj(mCurMissionLine.curMissionId, isMission: true);
				}
				break;
			}
		}
		else
		{
			SingletonUnity<NewMapUIRootLogic>.Instance.Reset(mCurMissionLine.CurMissionData.AcceptMapId);
			SingletonUnity<NewMapUIRootLogic>.Instance.ChooseActivityObj(mCurMissionLine.curMissionId, isMission: true);
		}
	}
}
