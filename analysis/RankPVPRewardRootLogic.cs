using System;
using System.Collections.Generic;
using UnityEngine;

public class RankPVPRewardRootLogic : SingletonUnity<RankPVPRewardRootLogic>
{
	public List<RankPVPLineReward> rewardList;

	public RankPVPLineReward MyReward;

	public UILabel OutRankLabel;

	public UIWrapContentNew uiWrapContent;

	private int lineMinCount = 6;

	public UIWidget WrapContentBottomWidget;

	private List<LadderRewardData> AllRewardList = new List<LadderRewardData>();

	public UIScrollView uiScrollView;

	protected override void Awake()
	{
		base.Awake();
		uiWrapContent.enabled = false;
		UIWrapContentNew uIWrapContentNew = uiWrapContent;
		uIWrapContentNew.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(uIWrapContentNew.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeItem));
	}

	public void Reset()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		LadderRewardData ladderRewardDataByRank = DataManager.GetLadderRewardDataByRank(playerData.RankPVPData.RankPVPRankNum);
		AllRewardList = DataManager.GetLadderRewardDataList();
		AllRewardList.Sort((LadderRewardData x, LadderRewardData y) => (x.ID.Length == y.ID.Length) ? x.ID.CompareTo(y.ID) : (x.ID.Length - y.ID.Length));
		int num = Mathf.Min(AllRewardList.Count, lineMinCount) - rewardList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(rewardList[0].gameObject) as GameObject;
				gameObject.transform.parent = uiWrapContent.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.name = $"{rewardList.Count:d2}";
				rewardList.Add(gameObject.GetComponent<RankPVPLineReward>());
			}
		}
		for (int j = 0; j < rewardList.Count; j++)
		{
			if (j < AllRewardList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(rewardList[j].gameObject, state: true);
				rewardList[j].ShowRewardInfo(AllRewardList[j]);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(rewardList[j].gameObject, state: false);
			}
		}
		uiWrapContent.minIndex = 1 - AllRewardList.Count;
		WrapContentBottomWidget.height = AllRewardList.Count * uiWrapContent.itemSize;
		if (AllRewardList.Count == 1)
		{
			uiWrapContent.maxIndex = 1;
		}
		uiWrapContent.SortBasedOnScrollMovement();
		uiScrollView.ResetPosition();
		uiWrapContent.enabled = true;
		if (ladderRewardDataByRank != null)
		{
			UnityVersionUtil.SetActiveRecursive(MyReward.gameObject, state: true);
			MyReward.ShowRewardInfo(ladderRewardDataByRank);
			OutRankLabel.enabled = false;
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(MyReward.gameObject, state: false);
			OutRankLabel.enabled = true;
			OutRankLabel.text = StrDictionary.GetDictionaryString("#{101021}");
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RankPvpShowRewardRoot);
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		RankPVPLineReward itemLogic = rewardList[index];
		ResetItemLine(itemLogic, Mathf.Abs(realIndex));
	}

	private void ResetItemLine(RankPVPLineReward itemLogic, int idx)
	{
		if (idx < AllRewardList.Count)
		{
			itemLogic.ShowRewardInfo(AllRewardList[idx]);
		}
	}
}
