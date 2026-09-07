using System.Collections;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DominRootLogic : SingletonUnity<DominRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public List<DominLineLogic> DominLineList = new List<DominLineLogic>();

	private Dictionary<string, domin_info> curDominInfoDic = new Dictionary<string, domin_info>();

	private Dictionary<long, character_look> curCharacterDic = new Dictionary<long, character_look>();

	private string mTargetId = string.Empty;

	public UITable TableRoot;

	public UIScrollView ScrollView;

	private string mTargetMissionId = string.Empty;

	private NewMissionLineLogic mCurMissionLine;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			TutorialManager.OnClickTutorialBtn onClickTutorialBtn = mOnClickTutorialBtn;
			mOnClickTutorialBtn = null;
			onClickTutorialBtn();
		}
	}

	public void EnableReset(string targetId)
	{
		mTargetId = targetId;
		for (int i = 0; i < DominLineList.Count; i++)
		{
			NGUITools.SetActive(DominLineList[i].gameObject, state: false);
		}
	}

	public void Reset(Dictionary<string, domin_info> infoDic, Dictionary<long, character_look> lookDic)
	{
		WaitResponseUIRootLogic.CloseBox();
		curDominInfoDic = infoDic;
		curCharacterDic = lookDic;
		List<string> list = new List<string>(curDominInfoDic.Keys);
		list.Sort((string x, string y) => x.CompareTo(y));
		int num = list.Count - DominLineList.Count;
		if (num > 0)
		{
			GameObject gameObject = null;
			for (int i = 0; i < num; i++)
			{
				gameObject = Object.Instantiate(DominLineList[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{DominLineList.Count + 1:D2}";
				gameObject.transform.parent = DominLineList[0].transform.parent;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
				gameObject.transform.localScale = Vector3.one;
				DominLineList.Add(gameObject.GetComponent<DominLineLogic>());
			}
		}
		for (int j = 0; j < DominLineList.Count; j++)
		{
			if (j < list.Count)
			{
				NGUITools.SetActive(DominLineList[j].gameObject, state: true);
				domin_info domin_info = curDominInfoDic[list[j]];
				if (domin_info.state == 0L)
				{
					if (curCharacterDic.ContainsKey(domin_info.serverId))
					{
						DominLineList[j].Reset(domin_info, curCharacterDic[domin_info.serverId], ClickTargetLine);
					}
					else
					{
						DominLineList[j].Reset(domin_info, null, ClickTargetLine);
					}
				}
				else
				{
					DominLineList[j].Reset(domin_info, null, ClickTargetLine);
				}
			}
			else
			{
				NGUITools.SetActive(DominLineList[j].gameObject, state: false);
			}
		}
		TableRoot.Reposition();
		ScrollView.ResetPosition();
		StartCoroutine(DelayFlash());
		if (SingletonUnity<NewMapUIRootLogic>.Exists)
		{
			SingletonUnity<NewMapUIRootLogic>.Instance.SetActivityObjNormal();
		}
		if (!string.IsNullOrEmpty(mTargetId))
		{
			ClickTargetLine(mTargetId);
			mTargetId = string.Empty;
		}
	}

	private IEnumerator DelayFlash()
	{
		yield return null;
		TableRoot.Reposition();
		ScrollView.ResetPosition();
		if (TutorialManager.CurStep == TUTORIAL_STEP.CAPTURE_WAIT_DATA)
		{
			CheckTutorialEvent();
		}
	}

	public void ClickTargetLine(string dominId)
	{
		if (!curDominInfoDic.ContainsKey(dominId))
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominPageRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DominInfoRoot, delegate
		{
			domin_info domin_info = curDominInfoDic[dominId];
			if (curCharacterDic.ContainsKey(domin_info.serverId))
			{
				SingletonUnity<DominInfoRootLogic>.Instance.Reset(domin_info, curCharacterDic[domin_info.serverId]);
			}
			else
			{
				SingletonUnity<DominInfoRootLogic>.Instance.Reset(domin_info, null);
			}
			if (TutorialManager.CurStep == TUTORIAL_STEP.CAPTURE_CLICK_ITEM)
			{
				CheckTutorialEvent();
			}
		});
	}

	private void OnDisable()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.CAPTURE_START)
		{
			CheckTutorialEvent();
		}
	}
}
