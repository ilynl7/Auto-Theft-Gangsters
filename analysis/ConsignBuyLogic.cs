using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ConsignBuyLogic : MonoBehaviour
{
	public UIScrollView LeftScrollView;

	public UITable LeftTable;

	public UIScrollView CenterScrollView;

	public UIGrid CenterGrid;

	public UILabel pageInfoLabel;

	public List<ConsignItemLogic> consignItemLogicList = new List<ConsignItemLogic>();

	private List<consign_item> currentConsignList;

	private int PageCount = 10;

	private int MaxPage;

	private int CurPage;

	private int type = 2;

	private int subType = 2;

	private int quality = -1;

	private int levelrange = -1;

	private bool use;

	private int profession = -1;

	public UIToggle useNowToggle;

	public UILabel LevelShow;

	public UILabel QualityLabel;

	public UISprite QualitySprite;

	public UISprite ChoosedTabLinePic;

	public TweenScale QualityTween;

	public TweenScale LevelTween;

	public GameObject QualityPopRoot;

	public GameObject LevelPopRoot;

	public ConsignPopList LeftPopList;

	public ConsignPopList RightPopList;

	public ConsignPopList ProfessionPopList;

	public List<ConsignBuyTabLogic> leftTabList;

	public UILabel PageLabel;

	private List<int> levelIndexList = new List<int> { -1, 1, 2, 3, 4, 5, 6, 7, 8 };

	private List<string> levelStrList = new List<string> { "ALL", "lv.1", "lv.2", "lv.3", "lv.4", "lv.5", "lv.6", "lv.7", "lv.8" };

	private int preTabIndex = -1;

	private void OnEnable()
	{
		EnableReset();
	}

	private void EnableReset()
	{
		List<List<ConsignBuyTabData>> consignBuyTabData = DataManager.GetConsignBuyTabData();
		int num = consignBuyTabData.Count - leftTabList.Count;
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = Object.Instantiate(leftTabList[0].gameObject) as GameObject;
			ConsignBuyTabLogic component = gameObject.GetComponent<ConsignBuyTabLogic>();
			leftTabList.Add(component);
			gameObject.transform.parent = leftTabList[0].transform.parent;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localScale = Vector3.one;
			gameObject.gameObject.name = $"Container{leftTabList.Count:d2}";
		}
		for (int j = 0; j < consignBuyTabData.Count; j++)
		{
			leftTabList[j].Reset(consignBuyTabData[j], OnClickLeftTab, j, OnClickTopTab);
		}
		UnityVersionUtil.SetActiveRecursive(ChoosedTabLinePic.gameObject, state: false);
		ChoosedTabLinePic.transform.localScale = Vector3.zero;
	}

	public void Reset()
	{
		RefreshPage(null, CurPage, isRefresh: true);
		UnityVersionUtil.SetActiveRecursive(ChoosedTabLinePic.gameObject, state: false);
		type = -1;
		UnityVersionUtil.SetActiveRecursive(LeftPopList.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(RightPopList.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(QualityPopRoot, state: true);
		UnityVersionUtil.SetActiveRecursive(LevelPopRoot, state: true);
		UnityVersionUtil.SetActiveRecursive(ProfessionPopList.gameObject, state: true);
		if (quality == -1)
		{
			UnityVersionUtil.SetActiveRecursive(QualityLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(QualitySprite.gameObject, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(QualityLabel.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(QualitySprite.gameObject, state: true);
		}
		leftTabList[0].SubTweenRoot.PlayForward();
		leftTabList[0].SetMinusPic(isMinus: true);
		leftTabList[0].SubTabList[0].OnClickTab();
		preTabIndex = 0;
	}

	private void OnClickLeftTab(int itemType, int itemSubType, int tabIndex, int subTabIndex)
	{
		if (itemType != type || subType != itemSubType)
		{
			UnityVersionUtil.SetActiveRecursive(ChoosedTabLinePic.gameObject, state: true);
			ChoosedTabLinePic.transform.parent = leftTabList[tabIndex].SubTabList[subTabIndex].transform;
			ChoosedTabLinePic.transform.localPosition = Vector3.zero;
			ChoosedTabLinePic.transform.localScale = Vector3.one;
			type = itemType;
			subType = itemSubType;
			quality = -1;
			levelrange = -1;
			CurPage = 0;
			profession = -1;
			SendRequest();
			if (itemType == 6)
			{
				UnityVersionUtil.SetActiveRecursive(LeftPopList.gameObject, state: true);
				UnityVersionUtil.SetActiveRecursive(RightPopList.gameObject, state: true);
				List<int> list = new List<int>();
				List<string> list2 = new List<string>();
				DataManager.GetBadgeTypeByColor(subType, list, list2);
				list.Insert(0, -1);
				list2.Insert(0, "ALL");
				LeftPopList.Init(list, list2, OnClickConsignTypeItem);
				RightPopList.Init(levelIndexList, levelStrList, OnClickConsignLevelItem);
				LeftPopList.textLabel.text = "ALL";
				RightPopList.textLabel.text = "ALL";
				UnityVersionUtil.SetActiveRecursive(QualityPopRoot, state: false);
				UnityVersionUtil.SetActiveRecursive(LevelPopRoot, state: false);
				UnityVersionUtil.SetActiveRecursive(ProfessionPopList.gameObject, state: false);
				QualityTween.ResetToBeginning();
				LevelTween.ResetToBeginning();
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(LeftPopList.gameObject, state: false);
				UnityVersionUtil.SetActiveRecursive(RightPopList.gameObject, state: false);
				UnityVersionUtil.SetActiveRecursive(QualityPopRoot, state: true);
				if (quality == -1)
				{
					UnityVersionUtil.SetActiveRecursive(QualityLabel.gameObject, state: true);
					UnityVersionUtil.SetActiveRecursive(QualitySprite.gameObject, state: false);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(QualityLabel.gameObject, state: false);
					UnityVersionUtil.SetActiveRecursive(QualitySprite.gameObject, state: true);
				}
				if (itemType == 2)
				{
					UnityVersionUtil.SetActiveRecursive(ProfessionPopList.gameObject, state: true);
					UnityVersionUtil.SetActiveRecursive(LevelPopRoot, state: true);
					List<int> list3 = new List<int>();
					list3.Add(-1);
					list3.Add(0);
					list3.Add(1);
					list3.Add(2);
					List<int> itemIndexList = list3;
					List<string> list4 = new List<string>();
					list4.Add(StrDictionary.GetDictionaryString("#{100663}"));
					list4.Add(StrDictionary.GetDictionaryString("#{100128}"));
					list4.Add(StrDictionary.GetDictionaryString("#{100129}"));
					list4.Add(StrDictionary.GetDictionaryString("#{100130}"));
					List<string> itemTxtList = list4;
					ProfessionPopList.Init(itemIndexList, itemTxtList, OnClickProfessionTypeItem);
					ProfessionPopList.textLabel.text = StrDictionary.GetDictionaryString("#{100663}");
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(ProfessionPopList.gameObject, state: false);
					UnityVersionUtil.SetActiveRecursive(LevelPopRoot, state: false);
				}
				LevelShow.text = "ALL";
			}
		}
		ClosePopAnima();
	}

	public void OnClickProfessionTypeItem(int index, string str)
	{
		profession = index;
		if (type != -1)
		{
			SendRequest();
		}
	}

	public void OnClickConsignTypeItem(int index, string str)
	{
		quality = index;
		CurPage = 0;
		if (type != -1)
		{
			SendRequest();
		}
	}

	public void OnClickConsignLevelItem(int index, string str)
	{
		levelrange = index;
		CurPage = 0;
		if (type != -1)
		{
			SendRequest();
		}
	}

	private void OnClickTopTab(int tabIndex)
	{
		if (preTabIndex != tabIndex && preTabIndex >= 0)
		{
			leftTabList[preTabIndex].SubTweenRoot.PlayReverse();
			leftTabList[preTabIndex].SetMinusPic(isMinus: false);
		}
		preTabIndex = tabIndex;
		ClosePopAnima();
	}

	private void InitBottomTab()
	{
	}

	private void InitShow()
	{
		useNowToggle.value = use;
	}

	public void RefreshPage(List<consign_item> list, int page, bool isRefresh)
	{
		if (list != null)
		{
			int num = Mathf.Min(list.Count, PageCount) - consignItemLogicList.Count;
			int count = consignItemLogicList.Count;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = Object.Instantiate(consignItemLogicList[0].gameObject) as GameObject;
					gameObject.name = $"ItemLine_{count + i + 1}";
					ConsignItemLogic component = gameObject.GetComponent<ConsignItemLogic>();
					if (component != null)
					{
						component.transform.parent = consignItemLogicList[0].transform.parent;
						component.transform.localScale = Vector3.one;
						component.transform.localPosition = Vector3.zero;
						consignItemLogicList.Add(component);
					}
				}
			}
			for (int j = 0; j < consignItemLogicList.Count; j++)
			{
				if (j < list.Count)
				{
					UnityVersionUtil.SetActiveRecursive(consignItemLogicList[j].gameObject, state: true);
					consignItemLogicList[j].Reset(list[j]);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(consignItemLogicList[j].gameObject, state: false);
				}
			}
		}
		else
		{
			for (int k = 0; k < consignItemLogicList.Count; k++)
			{
				UnityVersionUtil.SetActiveRecursive(consignItemLogicList[k].gameObject, state: false);
			}
		}
		if (isRefresh)
		{
			LeftScrollView.ResetPosition();
			CenterGrid.NowReposition();
			CenterScrollView.ResetPosition();
		}
		PageLabel.text = $"{Mathf.Min(CurPage + 1, MaxPage)}/{MaxPage}";
	}

	public void BuySuccess(long id)
	{
		bool flag = false;
		for (int i = 0; i < currentConsignList.Count; i++)
		{
			if (currentConsignList[i].id == id)
			{
				currentConsignList.RemoveAt(i);
				flag = true;
				break;
			}
		}
		if (flag)
		{
			RefreshPage(currentConsignList, CurPage, isRefresh: true);
		}
	}

	public void ClickQuality(int index)
	{
		if (quality != index)
		{
			quality = index;
			CurPage = 0;
			if (type != -1)
			{
				SendRequest();
			}
			if (index == -1)
			{
				UnityVersionUtil.SetActiveRecursive(QualityLabel.gameObject, state: true);
				UnityVersionUtil.SetActiveRecursive(QualitySprite.gameObject, state: false);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(QualityLabel.gameObject, state: false);
				UnityVersionUtil.SetActiveRecursive(QualitySprite.gameObject, state: true);
				switch (index)
				{
				case 0:
					QualitySprite.color = Color.white;
					break;
				case 1:
					QualitySprite.color = Color.green;
					break;
				case 2:
					QualitySprite.color = Color.blue;
					break;
				case 3:
					QualitySprite.color = new Color(1f, 0f, 1f, 1f);
					break;
				}
			}
		}
		QualityTween.PlayReverse();
	}

	public void ClickLevel(int level)
	{
		if (levelrange != level)
		{
			levelrange = level;
			CurPage = 0;
			if (type != -1)
			{
				SendRequest();
			}
			switch (level)
			{
			case -1:
				LevelShow.text = $"ALL";
				break;
			case 0:
				LevelShow.text = $"0-29";
				break;
			case 1:
				LevelShow.text = $"30-59";
				break;
			case 2:
				LevelShow.text = $"60-89";
				break;
			case 3:
				LevelShow.text = $"90+";
				break;
			}
		}
		LevelTween.PlayReverse();
	}

	public void ClickUseNow(bool isUse)
	{
		if (use != isUse)
		{
			use = isUse;
			CurPage = 0;
			if (type != -1)
			{
				SendRequest();
			}
		}
	}

	public void UpdateList(ret_consign_ask_items_info.request request)
	{
		if (request.success == 0L)
		{
			CurPage = (int)request.curPage;
			MaxPage = (int)request.maxPage;
			if (request.HasConsign_items)
			{
				currentConsignList = new List<consign_item>(request.consign_items.Values);
			}
			else
			{
				currentConsignList = null;
			}
			RefreshPage(currentConsignList, CurPage, isRefresh: true);
		}
		else
		{
			CurPage = 0;
			MaxPage = 0;
			RefreshPage(null, CurPage, isRefresh: true);
		}
	}

	private void SendRequest()
	{
		consign_ask_items_info.request request = new consign_ask_items_info.request();
		request.type = type;
		request.subType = subType;
		request.quality = quality;
		request.levelRange = levelrange;
		request.use = use;
		request.curPage = CurPage;
		if (profession != -1)
		{
			request.profession = profession;
		}
		NetLogic.GetInstance().Send<Protocol.consign_ask_items_info>(request);
	}

	public void OnClickQualityBtn(GameObject btn)
	{
		int result = -1;
		int.TryParse(btn.name, out result);
		ClickQuality(result);
	}

	public void OnClickLevelBtn(GameObject btn)
	{
		int result = -1;
		int.TryParse(btn.name, out result);
		ClickLevel(result);
	}

	public void OnClickLeftPageBtn()
	{
		if (CurPage > 0)
		{
			CurPage--;
			SendRequest();
		}
	}

	public void OnClickRightPageBtn()
	{
		if (CurPage < MaxPage - 1)
		{
			CurPage++;
			SendRequest();
		}
	}

	private void OnDisable()
	{
		if (QualityTween != null)
		{
			QualityTween.ResetToBeginning();
		}
		if (LevelTween != null)
		{
			LevelTween.ResetToBeginning();
		}
	}

	public void ClosePopAnima()
	{
		QualityTween.ResetToBeginning();
		LevelTween.ResetToBeginning();
		LeftPopList.CloseAnima();
		RightPopList.CloseAnima();
		ProfessionPopList.CloseAnima();
	}
}
