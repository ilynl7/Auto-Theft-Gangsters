using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class NoModelRankRootLogic : MonoBehaviour
{
	public RANK_TYPE CurRankType;

	private List<sort_item> mCurRankList;

	public UILabel SelfRankLabel;

	public UILabel SelfNameLabel;

	public UILabel SelfValLabel;

	private bool mInitFlag;

	public List<RankItemLogic> RankItemList;

	public UISprite selectitemPic;

	private int mCurRankNum;

	private int mCurRankObjIndex;

	public UILabel PageInfoLabel;

	private int curPagenum;

	private int MaxPageNum = 10;

	public UIScrollView RankScrollView;

	public UILabel lvLabel;

	public UILabel powerlabel;

	public UILabel mannamelabel;

	public UILabel guildnamelabel;

	public UILabel menberLabel;

	public UILabel CityLabel;

	private void Start()
	{
		if (!mInitFlag)
		{
			for (int i = 0; i < RankItemList.Count; i++)
			{
				RankItemList[i].Init(OnClickRankNumBtn, i);
			}
			mInitFlag = true;
		}
	}

	public void Reset(List<sort_item> ranklist, RANK_TYPE curtype)
	{
		mCurRankList = ranklist;
		if (ranklist.Count % 10 != 0)
		{
			MaxPageNum = ranklist.Count / 10 + 1;
		}
		else
		{
			MaxPageNum = ranklist.Count / 10;
			if (MaxPageNum == 0)
			{
				MaxPageNum = 1;
			}
		}
		CurRankType = curtype;
		curPagenum = 1;
		SelfInfoShow();
		mCurRankNum = -1;
		PageInfoLabel.text = curPagenum + "/" + MaxPageNum;
		ResetItemList((curPagenum - 1) * 10);
		OnClickRankNum(0, 0);
		RankScrollView.ResetPosition();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Rank", $"rank_{(int)curtype}", "opentimes");
	}

	public void ResetItemList(int startRanknum)
	{
		for (int i = 0; i < RankItemList.Count; i++)
		{
			if (startRanknum + i < mCurRankList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(RankItemList[i].gameObject, state: true);
				RankItemList[i].Reset(startRanknum + i, mCurRankList[startRanknum + i], CurRankType);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(RankItemList[i].gameObject, state: false);
			}
		}
	}

	public void OnClickLeftpageBtn()
	{
		if (curPagenum > 1)
		{
			curPagenum--;
			PageInfoLabel.text = curPagenum + "/" + MaxPageNum;
			ResetItemList((curPagenum - 1) * 10);
			RankScrollView.ResetPosition();
			RankItemList[0].OnClickBtn();
		}
	}

	public void OnClickRightpageBtn()
	{
		if (curPagenum < MaxPageNum)
		{
			curPagenum++;
			PageInfoLabel.text = curPagenum + "/" + MaxPageNum;
			ResetItemList((curPagenum - 1) * 10);
			RankScrollView.ResetPosition();
			RankItemList[0].OnClickBtn();
		}
	}

	public void OnClickRankNumBtn(int clickNum, int objIndex)
	{
		OnClickRankNum(clickNum, objIndex);
	}

	public void OnClickRankNum(int rankNum, int itemIndex)
	{
		if (mCurRankNum != rankNum && rankNum < mCurRankList.Count)
		{
			mCurRankNum = rankNum;
			mCurRankObjIndex = itemIndex;
			selectitemPic.transform.parent = RankItemList[mCurRankObjIndex].transform;
			selectitemPic.transform.localPosition = Vector3.zero;
			if (!UnityVersionUtil.IsActive(selectitemPic.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(selectitemPic.gameObject, state: true);
			}
		}
	}

	public void SelfInfoShow()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		RANK_TYPE curRankType = CurRankType;
		if (curRankType != RANK_TYPE.GUILD)
		{
			return;
		}
		lvLabel.text = StrDictionary.GetDictionaryString("#{101312}");
		powerlabel.text = StrDictionary.GetDictionaryString("#{101313}");
		mannamelabel.text = StrDictionary.GetDictionaryString("#{100720}");
		guildnamelabel.text = StrDictionary.GetDictionaryString("#{100734}");
		menberLabel.text = StrDictionary.GetDictionaryString("#{100703}");
		CityLabel.text = StrDictionary.GetDictionaryString("#{106028}");
		if (playerData.IsHaveGuild())
		{
			SelfRankLabel.text = string.Format("{0}", StrDictionary.GetDictionaryString("#{101322}"));
			SelfNameLabel.enabled = false;
			SelfValLabel.enabled = false;
			for (int i = 0; i < mCurRankList.Count; i++)
			{
				if (mCurRankList[i].id == playerData.PlayerGuild.ServerId)
				{
					SelfRankLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{101309}"), i + 1);
					string[] array = mCurRankList[i].name.Split('#');
					SelfNameLabel.text = array[0];
					SelfValLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{101313}"), mCurRankList[i].score >> 32);
					SelfNameLabel.enabled = true;
					SelfValLabel.enabled = true;
					break;
				}
			}
		}
		else
		{
			SelfRankLabel.text = string.Format("{0}", StrDictionary.GetDictionaryString("#{100240}"));
			SelfNameLabel.enabled = false;
			SelfValLabel.enabled = false;
		}
	}
}
