using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class LevelPackRewardRootLogic : SingletonUnity<LevelPackRewardRootLogic>
{
	public UITexture bannerTexture;

	public UIWrapContentNew uiWrapContent;

	private int lineMinCount = 6;

	public UIWidget WrapContentBottomWidget;

	public UIScrollView uiScrollView;

	private Dictionary<string, level_pack> Level_Dic;

	public List<LevelPackLineItem> LineItems;

	private List<level_pack> Level_list;

	private List<LevelPackageData> LevelDataList = new List<LevelPackageData>();

	protected override void Awake()
	{
		base.Awake();
		uiWrapContent.enabled = false;
		UIWrapContentNew uIWrapContentNew = uiWrapContent;
		uIWrapContentNew.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(uIWrapContentNew.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeItem));
	}

	public void EnableReset()
	{
		for (int i = 0; i < LineItems.Count; i++)
		{
			UnityVersionUtil.SetActiveRecursive(LineItems[i].gameObject, state: false);
		}
		InitTexture();
	}

	public void InitTexture()
	{
		if (bannerTexture.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(GameDefine.TextureBannerLevel, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(string name, Texture textureObj)
	{
		bannerTexture.mainTexture = textureObj;
	}

	public void Reset(ret_request_level_pack.request request)
	{
		LevelDataList.Clear();
		Level_Dic = request.level_pack;
		Level_list = new List<level_pack>(request.level_pack.Values);
		for (int i = 0; i < Level_list.Count; i++)
		{
			LevelDataList.Add(DataManager.GetLevelPackageDataBuyId(Level_list[i].ID));
		}
		LevelDataList.Sort((LevelPackageData x, LevelPackageData y) => x.LvTarget - y.LvTarget);
		int num = Mathf.Min(LevelDataList.Count, lineMinCount) - LineItems.Count;
		if (num > 0)
		{
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(LineItems[0].gameObject) as GameObject;
				LevelPackLineItem component = gameObject.GetComponent<LevelPackLineItem>();
				gameObject.name = $"{LineItems.Count:D2}";
				gameObject.transform.parent = uiWrapContent.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				LineItems.Add(component);
			}
		}
		for (int k = 0; k < LineItems.Count; k++)
		{
			if (k < LevelDataList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(LineItems[k].gameObject, state: true);
				LineItems[k].UpdateInfo(Level_Dic[LevelDataList[k].ID], LevelDataList[k]);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(LineItems[k].gameObject, state: false);
			}
		}
		uiWrapContent.minIndex = 1 - LevelDataList.Count;
		WrapContentBottomWidget.height = LevelDataList.Count * uiWrapContent.itemSize;
		if (LevelDataList.Count == 1)
		{
			uiWrapContent.maxIndex = 1;
		}
		uiWrapContent.SortBasedOnScrollMovement();
		uiScrollView.ResetPosition();
		uiWrapContent.enabled = true;
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "LevelGift", "open");
	}

	public void UpdateInfo(string id)
	{
		if (Level_Dic.ContainsKey(id))
		{
			Level_Dic[id].state = 2L;
		}
		for (int i = 0; i < LevelDataList.Count; i++)
		{
			if (LevelDataList[i].ID.Equals(id))
			{
				for (int j = 0; j < LineItems.Count; j++)
				{
					LineItems[j].refershInfo(Level_Dic[LevelDataList[i].ID], LevelDataList[i]);
				}
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "LevelGift", $"require_{id}");
				break;
			}
		}
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		LevelPackLineItem itemLogic = LineItems[index];
		ResetItemLine(itemLogic, Mathf.Abs(realIndex));
	}

	private void ResetItemLine(LevelPackLineItem itemLogic, int idx)
	{
		if (idx < LevelDataList.Count)
		{
			itemLogic.UpdateInfo(Level_Dic[LevelDataList[idx].ID], LevelDataList[idx]);
		}
	}
}
