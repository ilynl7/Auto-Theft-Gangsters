using System.Collections.Generic;
using UnityEngine;

public class TeamTargetTabLine : MonoBehaviour
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UILabel TitleLabel;

	public TweenRotation ArrowPicTw;

	public TweenScale SubTabRootTw;

	public UIGrid SubTabGride;

	public UISprite BottomPic;

	public List<TeamTargetTabSubLine> SubLineList;

	private DelegateDefine.StringGameObjectDelegate onClickTab;

	private string mKey;

	private bool IsClose;

	private bool mHasSub;

	public string Key => mKey;

	public bool HasSubLine => mHasSub;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	public void Reset(string title, List<string> subTitle, string key, List<string> subKey, DelegateDefine.StringGameObjectDelegate clickFunc)
	{
		TitleLabel.text = title;
		if (!string.IsNullOrEmpty(key))
		{
			mKey = key;
			onClickTab = clickFunc;
		}
		if (subTitle != null && subTitle.Count > 0)
		{
			mHasSub = true;
			UnityVersionUtil.SetActiveRecursive(ArrowPicTw.gameObject, state: true);
			ArrowPicTw.ResetToBeginning();
			UnityVersionUtil.SetActiveRecursive(SubTabRootTw.gameObject, state: true);
			int num = subTitle.Count - SubLineList.Count;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = Object.Instantiate(SubLineList[0].gameObject) as GameObject;
					gameObject.transform.parent = SubTabGride.transform;
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					TeamTargetTabSubLine component = gameObject.GetComponent<TeamTargetTabSubLine>();
					SubLineList.Add(component);
				}
				SubTabGride.Reposition();
			}
			for (int j = 0; j < SubLineList.Count; j++)
			{
				if (j < subTitle.Count)
				{
					UnityVersionUtil.SetActiveRecursive(SubLineList[j].gameObject, state: true);
					SubLineList[j].Reset(subTitle[j], subKey[j], clickFunc);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(SubLineList[j].gameObject, state: false);
				}
			}
		}
		else
		{
			mHasSub = false;
			UnityVersionUtil.SetActiveRecursive(ArrowPicTw.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(SubTabRootTw.gameObject, state: false);
		}
		SubTabRootTw.ResetToBeginning();
		IsClose = true;
	}

	public void OnClickTab()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.CREATE_TEAM_CHOOSE_COPY)
		{
			CheckTutorialEvent();
		}
		if (!mHasSub)
		{
			if (onClickTab != null)
			{
				onClickTab(mKey, base.gameObject);
			}
		}
		else if (IsClose)
		{
			IsClose = false;
			ArrowPicTw.PlayForward();
			SubTabRootTw.PlayForward();
		}
		else
		{
			IsClose = true;
			ArrowPicTw.PlayReverse();
			SubTabRootTw.PlayReverse();
		}
	}
}
