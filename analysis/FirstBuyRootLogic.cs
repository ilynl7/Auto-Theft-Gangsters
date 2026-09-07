using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class FirstBuyRootLogic : SingletonUnity<FirstBuyRootLogic>
{
	public UISprite btnSp;

	public UILabel btnLabel;

	public List<RewardItem> rewardItems;

	private List<GameItem> ItemList = new List<GameItem>();

	public UIGrid parentGrid;

	private FirstBuyData curData;

	private int CurState;

	public GameObject FakeItemObj;

	public UITexture ModelPic;

	private string ShowModelID;

	public UITexture BgTexture;

	public UITexture fontTexture;

	private List<string> textureList = new List<string>();

	public void EnableReset()
	{
		for (int i = 0; i < rewardItems.Count; i++)
		{
			UnityVersionUtil.SetActiveRecursive(rewardItems[i].gameObject, state: false);
		}
		ResetFakeObjRoot();
		InitTexture();
	}

	public void InitTexture()
	{
		textureList.Clear();
		if (BgTexture.mainTexture == null)
		{
			textureList.Add(GameDefine.TextureFirstBuyBG);
		}
		if (fontTexture.mainTexture == null)
		{
			textureList.Add(GameDefine.TextureFirstBuy);
		}
		if (textureList.Count != 0 && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(textureList, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(Dictionary<string, Texture> retdic)
	{
		if (retdic.Count != 0)
		{
			if (retdic.ContainsKey(GameDefine.TextureFirstBuyBG))
			{
				BgTexture.mainTexture = retdic[GameDefine.TextureFirstBuyBG];
			}
			if (retdic.ContainsKey(GameDefine.TextureFirstBuy))
			{
				fontTexture.mainTexture = retdic[GameDefine.TextureFirstBuy];
			}
		}
	}

	private void UnloadTexture()
	{
		BundleManager.UnloadTexture(textureList);
	}

	public void Reset(ret_request_first_buy.request request)
	{
		curData = DataManager.GetFirstBuyDataById(request.ID);
		CurState = (int)request.state;
		ItemList.Clear();
		ShowModelID = string.Empty;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		switch (playerData.Profession)
		{
		case PROFESSION_TYPE.XD:
			if (!string.IsNullOrEmpty(curData.XDItemID1))
			{
				ItemList.Add(new GameItem(curData.XDItemID1, (EQUIP_QUALITY)curData.XDQuality1, curData.XDItemCount1));
			}
			ShowModelID = curData.XDShowModelID;
			break;
		case PROFESSION_TYPE.QJ:
			if (!string.IsNullOrEmpty(curData.QJItemID1))
			{
				ItemList.Add(new GameItem(curData.QJItemID1, (EQUIP_QUALITY)curData.QJQuality1, curData.QJItemCount1));
			}
			ShowModelID = curData.QJShowModelID;
			break;
		case PROFESSION_TYPE.NQS:
			if (!string.IsNullOrEmpty(curData.NQItemID1))
			{
				ItemList.Add(new GameItem(curData.NQItemID1, (EQUIP_QUALITY)curData.NQQuality1, curData.NQItemCount1));
			}
			ShowModelID = curData.NQShowModelID;
			break;
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
		if (CurState == 0)
		{
			btnLabel.text = StrDictionary.GetDictionaryString("#{301113}");
		}
		else if (CurState == 1)
		{
			btnLabel.text = StrDictionary.GetDictionaryString("#{300301}");
		}
		else
		{
			btnLabel.text = StrDictionary.GetDictionaryString("#{300403}");
		}
		showItemVisual();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "FirstBuy", "open");
	}

	public void showItemVisual()
	{
		if (FakeItemObj != null)
		{
			Object.Destroy(FakeItemObj);
		}
		FakeItemObj = null;
		if (!string.IsNullOrEmpty(ShowModelID))
		{
			ShowModelData showModelDataById = DataManager.GetShowModelDataById(ShowModelID);
			if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadItem(showModelDataById.ModelName, ItemLoadFinish));
			}
		}
	}

	private void ItemLoadFinish(string name, Object fakeobj, object param1 = null, object param2 = null)
	{
		if (fakeobj != null)
		{
			ShowModelData showModelDataById = DataManager.GetShowModelDataById(ShowModelID);
			FakeItemObj = Object.Instantiate(fakeobj) as GameObject;
			if (FakeItemObj != null)
			{
				BundleManager.ResetAllShader(FakeItemObj.transform);
				NGUITools.SetLayer(FakeItemObj, SingletonUnity<FakeObjRootLogic>.Instance.gameObject.layer);
				FakeItemObj.transform.parent = SingletonUnity<FakeObjRootLogic>.Instance.MeshRoot.transform;
				FakeItemObj.transform.localPosition = showModelDataById.Position;
				FakeItemObj.transform.localRotation = Quaternion.Euler(showModelDataById.Rotation);
			}
		}
	}

	private void ResetFakeObjRoot()
	{
		if (FakeItemObj != null)
		{
			Object.Destroy(FakeItemObj);
		}
		FakeObjRootLogic instance = SingletonUnity<FakeObjRootLogic>.Instance;
		if (instance == null)
		{
			ResourcesManager.LoadAndInstantiate("UIRoot/FakeObjRoot");
			instance = SingletonUnity<FakeObjRootLogic>.Instance;
		}
		SingletonUnity<FakeObjRootLogic>.Instance.SetPicValue(1f);
		SingletonUnity<FakeObjRootLogic>.Instance.EnableFakeObjRoot();
		ModelPic.mainTexture = instance.ModelPic;
	}

	public void OnClickBuyBtn()
	{
		if (CurState == 1)
		{
			WaitResponseUIRootLogic.OpenWaitBox(264, 10f, 0f);
			require_first_buy_reward.request request = new require_first_buy_reward.request();
			request.ID = curData.ID;
			NetLogic.GetInstance().Send<Protocol.require_first_buy_reward>(request);
		}
		else if (CurState == 0)
		{
			OnClickCloseBtn();
			NoticeLogic.AddNotifyData("#{300011}");
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PopDiamondBuyRoot, delegate
			{
				SingletonUnity<PopDiamondBuyRootLogic>.Instance.EnableReset();
				ask_shop_list.request rpcReq = new ask_shop_list.request
				{
					type = 4L,
					subType = 1L
				};
				NetLogic.GetInstance().Send<Protocol.ask_shop_list>(rpcReq);
				WaitResponseUIRootLogic.OpenWaitBox(143, 10f, 0f);
			});
		}
	}

	public void UpdateInfo(string id)
	{
		if (id.Equals(curData.ID))
		{
			CurState = 2;
			btnLabel.text = StrDictionary.GetDictionaryString("#{300403}");
			OnClickCloseBtn();
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "FirstBuy", $"require_{id}");
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FirstBuyRoot);
		if (SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
		{
			SingletonUnity<AutoPopUIRoot>.Instance.NextPop();
		}
	}

	private void OnDisable()
	{
		UnLoadFakeObj();
		UnloadTexture();
	}

	public void UnLoadFakeObj()
	{
		if (SingletonUnity<FakeObjRootLogic>.Instance != null)
		{
			SingletonUnity<FakeObjRootLogic>.Instance.DisableFakeObjRoot();
		}
		if (FakeItemObj != null)
		{
			Object.Destroy(FakeItemObj);
		}
	}
}
