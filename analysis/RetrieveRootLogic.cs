using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class RetrieveRootLogic : SingletonUnity<RetrieveRootLogic>
{
	public UITexture bannerTexture;

	public UIWrapContentNew uiWrapContent;

	private int lineMinCount = 6;

	public UIWidget WrapContentBottomWidget;

	public UIScrollView uiScrollView;

	private Dictionary<string, retrieve_info> Retrieve_Dic;

	public List<RetrieveLineLogic> LineItems;

	private List<retrieve_info> Retrieve_list;

	private List<RetrieveData> RetrieveDataList = new List<RetrieveData>();

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
		if (SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
		{
			SingletonUnity<AutoPopUIRoot>.Instance.RemoveUI(GameDefine.AUTOPOPTYPE.RETRIEVE);
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "Retrive", "open");
	}

	public void InitTexture()
	{
		if (bannerTexture.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(GameDefine.TextureBannerRetrieve, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(string name, Texture textureObj)
	{
		bannerTexture.mainTexture = textureObj;
	}

	public void Reset(ret_request_retrieve_info.request request)
	{
		RetrieveDataList.Clear();
		Retrieve_Dic = request.info;
		Retrieve_list = new List<retrieve_info>(request.info.Values);
		for (int num = Retrieve_list.Count - 1; num >= 0; num--)
		{
			if (Retrieve_list[num].state == 1)
			{
				Retrieve_list.RemoveAt(num);
			}
			else
			{
				RetrieveData retrieveDataBuyId = DataManager.GetRetrieveDataBuyId(Retrieve_list[num].ID);
				if (retrieveDataBuyId == null || (retrieveDataBuyId.isGuildDance && !SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild()))
				{
					Retrieve_list.RemoveAt(num);
				}
			}
		}
		for (int i = 0; i < Retrieve_list.Count; i++)
		{
			RetrieveDataList.Add(DataManager.GetRetrieveDataBuyId(Retrieve_list[i].ID));
		}
		RetrieveDataList.Sort((RetrieveData x, RetrieveData y) => (Retrieve_Dic[x.ID].state == Retrieve_Dic[y.ID].state) ? ((x.ID.Length != y.ID.Length) ? (x.ID.Length - y.ID.Length) : x.ID.CompareTo(y.ID)) : ((int)Retrieve_Dic[x.ID].state - (int)Retrieve_Dic[y.ID].state));
		int num2 = Mathf.Min(RetrieveDataList.Count, lineMinCount) - LineItems.Count;
		if (num2 > 0)
		{
			for (int j = 0; j < num2; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(LineItems[0].gameObject) as GameObject;
				RetrieveLineLogic component = gameObject.GetComponent<RetrieveLineLogic>();
				gameObject.name = $"{LineItems.Count:D2}";
				gameObject.transform.parent = uiWrapContent.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				LineItems.Add(component);
			}
		}
		for (int k = 0; k < LineItems.Count; k++)
		{
			if (k < RetrieveDataList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(LineItems[k].gameObject, state: true);
				LineItems[k].UpdateInfo(Retrieve_Dic[RetrieveDataList[k].ID], RetrieveDataList[k]);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(LineItems[k].gameObject, state: false);
			}
		}
		uiWrapContent.minIndex = 1 - RetrieveDataList.Count;
		WrapContentBottomWidget.height = RetrieveDataList.Count * uiWrapContent.itemSize;
		uiWrapContent.SortBasedOnScrollMovement();
		uiScrollView.ResetPosition();
		uiWrapContent.enabled = true;
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		RetrieveLineLogic itemLogic = LineItems[index];
		ResetItemLine(itemLogic, Mathf.Abs(realIndex));
	}

	private void ResetItemLine(RetrieveLineLogic itemLogic, int idx)
	{
		if (idx < RetrieveDataList.Count)
		{
			itemLogic.UpdateInfo(Retrieve_Dic[RetrieveDataList[idx].ID], RetrieveDataList[idx]);
		}
	}
}
