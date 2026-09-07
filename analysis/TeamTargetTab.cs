using System.Collections.Generic;
using UnityEngine;

public class TeamTargetTab : MonoBehaviour
{
	public UITable LineRoot;

	public List<TeamTargetTabLine> TargetTabLineList;

	private string mCurChooseLineID = string.Empty;

	private DelegateDefine.OneStringParamDelegate onClickTab;

	private UISprite mPreChoosedPic;

	private bool mIsPreIsSub;

	public void Reset(List<TeamTargetTabData> targetData, DelegateDefine.OneStringParamDelegate onClick)
	{
		TeamTargetTabData teamTargetTabData = null;
		for (int num = targetData.Count - 1; num >= 0; num--)
		{
			if (targetData[num].Key.Equals("1"))
			{
				teamTargetTabData = targetData[num];
				targetData.RemoveAt(num);
				break;
			}
		}
		for (int num2 = targetData.Count - 1; num2 >= 0; num2--)
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(targetData[num2].Key);
			if (!CheckLevel(copySceneDataById.MinLevel, copySceneDataById.MaxLevel))
			{
				targetData.RemoveAt(num2);
			}
		}
		targetData.Sort((TeamTargetTabData x, TeamTargetTabData y) => x.Key.CompareTo(y.Key));
		if (teamTargetTabData != null)
		{
			targetData.Add(teamTargetTabData);
		}
		onClickTab = onClick;
		int num3 = targetData.Count - TargetTabLineList.Count;
		if (num3 > 0)
		{
			for (int i = 0; i < num3; i++)
			{
				GameObject gameObject = Object.Instantiate(TargetTabLineList[0].gameObject) as GameObject;
				gameObject.transform.parent = LineRoot.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				TeamTargetTabLine component = gameObject.GetComponent<TeamTargetTabLine>();
				TargetTabLineList.Add(component);
			}
		}
		for (int j = 0; j < TargetTabLineList.Count; j++)
		{
			TargetTabLineList[j].gameObject.name = j.ToString();
			if (j < targetData.Count)
			{
				UnityVersionUtil.SetActiveRecursive(TargetTabLineList[j].gameObject, state: true);
				TargetTabLineList[j].Reset(targetData[j].Title, targetData[j].SubTitle, targetData[j].Key, targetData[j].SubKey, OnClickTab);
				TargetTabLineList[j].transform.localPosition = new Vector3(0f, j * -46, 0f);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(TargetTabLineList[j].gameObject, state: false);
			}
		}
		LineRoot.Reposition();
		mCurChooseLineID = string.Empty;
	}

	public bool CheckLevel(int minLevel, int maxLevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(minLevel, maxLevel);
	}

	private void OnClickTab(string key, GameObject obj)
	{
		if (mCurChooseLineID.Equals(key))
		{
			return;
		}
		mCurChooseLineID = key;
		if (mPreChoosedPic != null)
		{
			if (mIsPreIsSub)
			{
				mPreChoosedPic.spriteName = "CZ_huaDongBG_2";
			}
			else
			{
				mPreChoosedPic.spriteName = "CZ_huaDongBG";
			}
		}
		mPreChoosedPic = obj.GetComponent<UISprite>();
		if (obj.transform.childCount > 2)
		{
			mIsPreIsSub = false;
			mPreChoosedPic.spriteName = "CZ_huaDongBG_1";
		}
		else
		{
			mIsPreIsSub = true;
			mPreChoosedPic.spriteName = "CZ_huaDongBG_2_Xuan";
		}
		if (onClickTab != null)
		{
			onClickTab(mCurChooseLineID);
		}
	}
}
