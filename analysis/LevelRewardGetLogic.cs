using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class LevelRewardGetLogic : SingletonUnity<LevelRewardGetLogic>
{
	public UILabel InfoLabel;

	public List<RewardItem> rewardItems;

	private level_pack curInfo;

	private LevelPackageData curData;

	public UIGrid parentGrid;

	private List<GameItem> ItemList = new List<GameItem>();

	private List<level_pack> Level_list;

	private List<LevelPackageData> LevelDataList = new List<LevelPackageData>();

	private Dictionary<string, level_pack> Level_Dic = new Dictionary<string, level_pack>();

	private List<LevelPackageData> ShowList = new List<LevelPackageData>();

	private LevelPackageData showdata;

	private bool showuiflag;

	private float starttime;

	private int WaitTime = 20;

	public UILabel TimeLabel;

	private int mCurSecond;

	private float mTimeCount;

	private void OnEnable()
	{
		showuiflag = true;
		ShowList.Clear();
		starttime = Time.time;
		mCurSecond = WaitTime;
	}

	public void EnableReset()
	{
		for (int i = 0; i < rewardItems.Count; i++)
		{
			UnityVersionUtil.SetActiveRecursive(rewardItems[i].gameObject, state: false);
		}
	}

	public void ShowRewardList(List<LevelPackageData> list)
	{
		ShowList = list;
		Level_Dic = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.LevelPackDic;
		ShowNextLevelreward();
	}

	private void Update()
	{
		if (showuiflag)
		{
			mTimeCount = Time.time - starttime;
			if ((int)((float)WaitTime - mTimeCount) < mCurSecond)
			{
				mCurSecond = (int)((float)WaitTime - mTimeCount);
				TimeLabel.text = $"{mCurSecond}s";
			}
			if (mTimeCount >= (float)WaitTime)
			{
				OnClickBtn();
			}
		}
	}

	private void ShowNextLevelreward()
	{
		if (ShowList.Count > 0)
		{
			showdata = ShowList[0];
			ShowList.RemoveAt(0);
			if (Level_Dic.Count > 0 && Level_Dic.ContainsKey(showdata.ID))
			{
				UpdateInfo(Level_Dic[showdata.ID], showdata);
				starttime = Time.time;
				mCurSecond = WaitTime;
			}
			else
			{
				ShowNextLevelreward();
			}
		}
		else
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LevelRewardGetRoot);
		}
	}

	public void UpdateInfo(level_pack curinfo, LevelPackageData curdata)
	{
		curInfo = curinfo;
		curData = curdata;
		if (GameManager.IsSupportCurDataVersion47())
		{
			InfoLabel.text = StrDictionary.GetDictionaryString("#{301118}", curData.LvTarget);
		}
		else
		{
			InfoLabel.text = $"Congratulations to level {curData.LvTarget}";
		}
		ItemList.Clear();
		if (!string.IsNullOrEmpty(curData.ItemID1))
		{
			ItemList.Add(new GameItem(curData.ItemID1, (EQUIP_QUALITY)curData.Quality1, curData.ItemCount1));
		}
		if (!string.IsNullOrEmpty(curData.ItemID2))
		{
			ItemList.Add(new GameItem(curData.ItemID2, (EQUIP_QUALITY)curData.Quality2, curData.ItemCount2));
		}
		if (!string.IsNullOrEmpty(curData.ItemID3))
		{
			ItemList.Add(new GameItem(curData.ItemID3, (EQUIP_QUALITY)curData.Quality3, curData.ItemCount3));
		}
		if (!string.IsNullOrEmpty(curData.ItemID4))
		{
			ItemList.Add(new GameItem(curData.ItemID4, (EQUIP_QUALITY)curData.Quality4, curData.ItemCount4));
		}
		if (!string.IsNullOrEmpty(curData.ItemID5))
		{
			ItemList.Add(new GameItem(curData.ItemID5, (EQUIP_QUALITY)curData.Quality5, curData.ItemCount5));
		}
		int num = ItemList.Count - rewardItems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(rewardItems[0].gameObject) as GameObject;
				RewardItem component = gameObject.GetComponent<RewardItem>();
				gameObject.name = $"reward{rewardItems.Count:D2}";
				gameObject.transform.parent = parentGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				rewardItems.Add(component);
			}
		}
		for (int j = 0; j < rewardItems.Count; j++)
		{
			if (j < ItemList.Count)
			{
				int level = 0;
				if (ItemList[j].ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(ItemList[j].ItemData.SubType);
				}
				rewardItems[j].UpdateItem(ItemList[j], level);
				UnityVersionUtil.SetActiveRecursive(rewardItems[j].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(rewardItems[j].gameObject, state: false);
			}
		}
		parentGrid.Reposition();
	}

	public void OnClickBtn()
	{
		if (curInfo.state == 1)
		{
			require_level_reward.request request = new require_level_reward.request();
			request.ID = curInfo.ID;
			NetLogic.GetInstance().Send<Protocol.require_level_reward>(request);
		}
		ShowNextLevelreward();
	}
}
