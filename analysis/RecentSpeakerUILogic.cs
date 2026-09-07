using System.Collections.Generic;
using UnityEngine;

public class RecentSpeakerUILogic : MonoBehaviour
{
	private List<RecentSpeaker> mRecentSpeakerList;

	public List<RecentSpeakerBtnLogic> mSpeakerBtnList;

	public RecentSpeakerBtnLogic SpeakerBtnPrefab;

	public UISprite ChoosedPic;

	public UIScrollView ScrollView;

	public UIPanel mPanel;

	public void Reset(List<RecentSpeaker> speakerList)
	{
		mRecentSpeakerList = speakerList;
		int num = speakerList.Count - mSpeakerBtnList.Count;
		GameObject gameObject = null;
		for (int i = 0; i < num; i++)
		{
			gameObject = Object.Instantiate(SpeakerBtnPrefab.gameObject) as GameObject;
			gameObject.transform.parent = SpeakerBtnPrefab.transform.parent;
			gameObject.transform.localScale = Vector3.one;
			mSpeakerBtnList.Add(gameObject.GetComponent<RecentSpeakerBtnLogic>());
		}
		int num2 = 20;
		int num3 = 0;
		for (int j = 0; j < mSpeakerBtnList.Count; j++)
		{
			if (j < speakerList.Count)
			{
				mSpeakerBtnList[j].Reset(mRecentSpeakerList[mRecentSpeakerList.Count - 1 - j]);
				mSpeakerBtnList[j].transform.localPosition = new Vector3(num3 + 5 + mSpeakerBtnList[j].BottomSprite.width / 2, num2, 0f);
				num3 += mSpeakerBtnList[j].BottomSprite.width + 5;
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(mSpeakerBtnList[j].gameObject, state: false);
			}
		}
		UnityVersionUtil.SetActiveRecursive(SpeakerBtnPrefab.gameObject, state: false);
		if (speakerList.Count == 1)
		{
			OnClickBtn(speakerList[0]);
		}
		else
		{
			OnClickBtn(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.RecentSpeakers.GetLastSpeaker());
		}
		vp_Timer.In(0.1f, delegate
		{
			mPanel.SetDirty();
		});
		ScrollView.ResetPosition();
	}

	public void OnClickBtn(RecentSpeaker speakerInfo)
	{
		if (speakerInfo == null)
		{
			UnityVersionUtil.SetActiveRecursive(ChoosedPic.gameObject, state: false);
			return;
		}
		UnityVersionUtil.SetActiveRecursive(ChoosedPic.gameObject, state: true);
		for (int i = 0; i < mSpeakerBtnList.Count; i++)
		{
			if (speakerInfo == mSpeakerBtnList[i].SpeakerInfo)
			{
				ChoosedPic.transform.parent = mSpeakerBtnList[i].transform;
				ChoosedPic.transform.transform.localPosition = Vector3.zero;
				ChoosedPic.width = mSpeakerBtnList[i].BottomSprite.width;
				ChoosedPic.height = mSpeakerBtnList[i].BottomSprite.height;
				UnityVersionUtil.SetActiveRecursive(mSpeakerBtnList[i].TipPic.gameObject, state: false);
				UnityVersionUtil.SetActiveRecursive(mSpeakerBtnList[i].SelfTipPic.gameObject, state: false);
				break;
			}
		}
	}
}
