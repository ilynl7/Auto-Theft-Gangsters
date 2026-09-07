using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class MonthlyCardRootLogic : SingletonUnity<MonthlyCardRootLogic>
{
	public UITexture bgtexture;

	public UITexture flagtexture;

	public UITexture flagtexture2;

	public UITexture pricetexture;

	public UILabel InfoLabel;

	public UISprite BtnSp;

	public UILabel BtnLabel;

	public TweenScale BtnAnima;

	private MonthlyCardData CurData;

	private List<GameItem> ItemList = new List<GameItem>();

	public List<RewardItem> RewardItems;

	public UIGrid ParentGrid;

	public UIGrid ParentGrid2;

	private vip curvipinfo;

	public void EnableReset()
	{
		curvipinfo = null;
		for (int i = 0; i < RewardItems.Count; i++)
		{
			NGUITools.SetActive(RewardItems[i].gameObject, state: false);
		}
	}

	public void Reset(ret_require_vip_info.request request)
	{
		if (request.HasVip)
		{
			curvipinfo = request.vip;
		}
		if (curvipinfo == null)
		{
			return;
		}
		CurData = DataManager.GetMonthlyCardDataById(curvipinfo.id);
		ChangeBtnState();
		InitTexture();
		ItemList.Clear();
		if (!string.IsNullOrEmpty(CurData.ItemID1))
		{
			ItemList.Add(new GameItem(CurData.ItemID1, (EQUIP_QUALITY)CurData.Quality1, CurData.ItemCount1));
		}
		if (!string.IsNullOrEmpty(CurData.ItemID2))
		{
			ItemList.Add(new GameItem(CurData.ItemID2, (EQUIP_QUALITY)CurData.Quality2, CurData.ItemCount2));
		}
		if (!string.IsNullOrEmpty(CurData.ItemID3))
		{
			ItemList.Add(new GameItem(CurData.ItemID3, (EQUIP_QUALITY)CurData.Quality3, CurData.ItemCount3));
		}
		if (!string.IsNullOrEmpty(CurData.ItemID4))
		{
			ItemList.Add(new GameItem(CurData.ItemID4, (EQUIP_QUALITY)CurData.Quality4, CurData.ItemCount4));
		}
		if (!string.IsNullOrEmpty(CurData.ItemID5))
		{
			ItemList.Add(new GameItem(CurData.ItemID5, (EQUIP_QUALITY)CurData.Quality5, CurData.ItemCount5));
		}
		int num = ItemList.Count - RewardItems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(RewardItems[0].gameObject) as GameObject;
				RewardItem component = gameObject.GetComponent<RewardItem>();
				gameObject.name = $"reward{RewardItems.Count:D2}";
				gameObject.transform.parent = ParentGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				RewardItems.Add(component);
			}
		}
		if (ItemList.Count > 3)
		{
			ParentGrid.transform.localPosition = new Vector3(-81f, 19f, 0f);
			ParentGrid2.transform.localPosition = new Vector3(-81f, -44f, 0f);
		}
		else
		{
			ParentGrid.transform.localPosition = new Vector3(-81f, -14f, 0f);
		}
		for (int j = 0; j < RewardItems.Count; j++)
		{
			if (j < ItemList.Count)
			{
				if (ItemList.Count > 3)
				{
					if (j < 2)
					{
						RewardItems[j].transform.parent = ParentGrid.transform;
						RewardItems[j].transform.localScale = Vector3.one;
					}
					else
					{
						RewardItems[j].transform.parent = ParentGrid2.transform;
						RewardItems[j].transform.localScale = Vector3.one;
					}
				}
				else
				{
					RewardItems[j].transform.parent = ParentGrid.transform;
					RewardItems[j].transform.localScale = Vector3.one;
				}
				int level = 0;
				if (ItemList[j].ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(ItemList[j].ItemData.SubType);
				}
				RewardItems[j].UpdateItem(ItemList[j], level);
				UnityVersionUtil.SetActiveRecursive(RewardItems[j].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(RewardItems[j].gameObject, state: false);
			}
		}
		ParentGrid.Reposition();
		ParentGrid2.Reposition();
	}

	public void RefershInfo(ret_require_vip_reward.request request)
	{
		if (request.HasVip)
		{
			curvipinfo = request.vip;
		}
		if (curvipinfo != null)
		{
			CurData = DataManager.GetMonthlyCardDataById(curvipinfo.id);
			ChangeBtnState();
		}
	}

	public void UpdateInfo(string id)
	{
		if (curvipinfo != null && curvipinfo.id.Equals(id))
		{
			curvipinfo.state = 1L;
			ChangeBtnState();
		}
	}

	public void ChangeBtnState()
	{
		BtnAnima.ResetToBeginning();
		BtnAnima.enabled = false;
		if (curvipinfo.state == -1)
		{
			BtnLabel.text = $"${CurData.Dollor}";
			BtnSp.spriteName = GameDefine.BtnIconNew[0];
			InfoLabel.text = StrDictionary.GetDictionaryString("#{300705}");
		}
		else if (curvipinfo.state == 0L)
		{
			BtnAnima.enabled = true;
			BtnAnima.PlayForward();
			BtnLabel.text = StrDictionary.GetDictionaryString("#{300402}");
			BtnSp.spriteName = GameDefine.BtnIconNew[0];
			InfoLabel.text = StrDictionary.GetDictionaryString("#{300706}", curvipinfo.count);
		}
		else
		{
			BtnLabel.text = StrDictionary.GetDictionaryString("#{300403}");
			BtnSp.spriteName = GameDefine.BtnIconNew[1];
			InfoLabel.text = StrDictionary.GetDictionaryString("#{300706}", curvipinfo.count);
		}
	}

	public void InitTexture()
	{
		List<string> list = new List<string>();
		if (!string.IsNullOrEmpty(CurData.BGName))
		{
			list.Add(CurData.BGName);
		}
		if (!string.IsNullOrEmpty(CurData.FlagName))
		{
			list.Add(CurData.FlagName);
		}
		if (!string.IsNullOrEmpty(CurData.PricePicName))
		{
			list.Add(CurData.PricePicName);
		}
		if (list.Count != 0 && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(list, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(Dictionary<string, Texture> retdic)
	{
		if (retdic.Count != 0)
		{
			if (!string.IsNullOrEmpty(CurData.BGName) && retdic.ContainsKey(CurData.BGName))
			{
				bgtexture.mainTexture = retdic[CurData.BGName];
			}
			if (!string.IsNullOrEmpty(CurData.FlagName) && retdic.ContainsKey(CurData.FlagName))
			{
				flagtexture.mainTexture = retdic[CurData.FlagName];
				flagtexture2.mainTexture = retdic[CurData.FlagName];
			}
			if (!string.IsNullOrEmpty(CurData.PricePicName) && retdic.ContainsKey(CurData.PricePicName))
			{
				pricetexture.mainTexture = retdic[CurData.PricePicName];
			}
		}
	}

	public void OnClickBtn()
	{
		if (curvipinfo == null)
		{
			return;
		}
		if (curvipinfo.state == -1)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.Billing(CurData.ProductId);
			if (GameSettingData.IsTestBilling)
			{
				WaitResponseUIRootLogic.OpenWaitBox(266, 10f, 0f);
				check_purchase.request request = new check_purchase.request();
				request.productId = CurData.ProductId;
				NetLogic.GetInstance().Send<Protocol.check_purchase>(request);
			}
		}
		else if (curvipinfo.state == 0L)
		{
			WaitResponseUIRootLogic.OpenWaitBox(300, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.require_vip_reward>();
		}
	}
}
