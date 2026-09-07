using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class MysteryShopPageLogic : MonoBehaviour
{
	public UISprite BtnSp;

	public UILabel BtnLabel;

	public UILabel LimitTimesLabel;

	public UILabel EndTimeLabel;

	public UILabel timeNamelabel;

	public UILabel hintinfolabel;

	private string dayName;

	public UIGrid ParentGrid;

	public UIGrid ParentGrid2;

	public List<RewardItem> RewardList;

	private List<GameItem> ItemList = new List<GameItem>();

	private special_big_pack CurInfo;

	private BigPackageData curData;

	private long reamainTime;

	private bool IsDollorBuy;

	private TimeSpan LimitTimeSpan;

	public UITexture bannerTexture;

	public UITexture ShowTexture;

	private float tempCountTime;

	public void EnableReset()
	{
		for (int i = 0; i < RewardList.Count; i++)
		{
			UnityVersionUtil.SetActiveRecursive(RewardList[i].gameObject, state: false);
		}
		UnityVersionUtil.SetActiveRecursive(BtnSp.gameObject, state: false);
		EndTimeLabel.text = string.Empty;
	}

	public void RefershInfo(special_big_pack curPackinfo)
	{
		CurInfo = curPackinfo;
		curData = DataManager.GetBigPackageDataById(curPackinfo.ID);
		if (curData.MaxCount > 0)
		{
			int num = curData.MaxCount - (int)curPackinfo.remain_times;
			if (num < 0)
			{
				num = 0;
			}
			LimitTimesLabel.enabled = true;
			LimitTimesLabel.text = string.Format("{0} {1}/{2}", StrDictionary.GetDictionaryString("#{301101}"), num, curData.MaxCount);
			if (num > 0)
			{
				LimitTimesLabel.color = Color.white;
			}
			else
			{
				LimitTimesLabel.color = Color.red;
			}
		}
		else
		{
			LimitTimesLabel.enabled = false;
		}
		reamainTime = 0L;
		tempCountTime = 0f;
		EndTimeLabel.enabled = false;
		timeNamelabel.enabled = false;
		if (!string.IsNullOrEmpty(curData.TimeList))
		{
			dayName = StrDictionary.GetDictionaryString("#{100241}");
			reamainTime = (long)TimeTools.GetShopItemTime(curData.GetCurTimeEnd()).TotalSeconds;
			if (reamainTime > 0)
			{
				EndTimeLabel.text = TimeTools.GetFullTime(reamainTime);
				EndTimeLabel.enabled = true;
				timeNamelabel.enabled = true;
				hintinfolabel.enabled = true;
			}
			else
			{
				hintinfolabel.enabled = false;
			}
		}
		else
		{
			hintinfolabel.enabled = false;
		}
		if (string.IsNullOrEmpty(curData.ProductId))
		{
			IsDollorBuy = false;
			BtnLabel.text = GameMoneyHelper.GetMoneyValStr(curData.PriceCost, curData.PriceType);
		}
		else
		{
			IsDollorBuy = true;
			BtnLabel.text = $"${curData.Dollor}";
		}
		if (CurInfo.state == 0L)
		{
			BtnSp.spriteName = GameDefine.BtnIconNew[0];
		}
		else
		{
			BtnSp.spriteName = GameDefine.BtnIconNew[1];
		}
		UnityVersionUtil.SetActiveRecursive(BtnSp.gameObject, state: true);
		ShowRewards();
		InitTexture();
	}

	public void InitTexture()
	{
		List<string> list = new List<string>();
		if (!string.IsNullOrEmpty(curData.TextureTitle1))
		{
			list.Add(curData.TextureTitle1);
		}
		if (!string.IsNullOrEmpty(curData.TextureTitle2))
		{
			list.Add(curData.TextureTitle2);
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
			if (!string.IsNullOrEmpty(curData.TextureTitle1) && retdic.ContainsKey(curData.TextureTitle1))
			{
				bannerTexture.mainTexture = retdic[curData.TextureTitle1];
				bannerTexture.MakePixelPerfect();
			}
			if (!string.IsNullOrEmpty(curData.TextureTitle2) && retdic.ContainsKey(curData.TextureTitle2))
			{
				ShowTexture.mainTexture = retdic[curData.TextureTitle2];
				ShowTexture.MakePixelPerfect();
			}
		}
	}

	public void UpdateInfo(special_big_pack curPackinfo)
	{
		if (CurInfo.ID.Equals(curPackinfo.ID))
		{
			RefershInfo(curPackinfo);
		}
	}

	public void OnClickBuyBtn()
	{
		if (CurInfo.state != 0L)
		{
			return;
		}
		if (IsDollorBuy)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.Billing(curData.ProductId);
			if (GameSettingData.IsTestBilling)
			{
				WaitResponseUIRootLogic.OpenWaitBox(266, 10f, 0f);
				check_purchase.request request = new check_purchase.request();
				request.productId = curData.ProductId;
				NetLogic.GetInstance().Send<Protocol.check_purchase>(request);
			}
		}
		else if (GameMoneyHelper.BeforeCheckBuy(curData.PriceType, curData.PriceCost))
		{
			WaitResponseUIRootLogic.OpenWaitBox(272, 10f, 0f);
			buy_big_pack.request request2 = new buy_big_pack.request();
			request2.ID = curData.ID;
			NetLogic.GetInstance().Send<Protocol.buy_big_pack>(request2);
		}
	}

	private void Update()
	{
		if (reamainTime > 0)
		{
			tempCountTime += Time.deltaTime;
			if (tempCountTime >= 1f)
			{
				reamainTime--;
				tempCountTime -= 1f;
				EndTimeLabel.text = TimeTools.GetFullTime(reamainTime);
			}
			if (reamainTime <= 0)
			{
				tempCountTime = 0f;
				EndTimeLabel.enabled = false;
				timeNamelabel.enabled = false;
				hintinfolabel.enabled = false;
			}
		}
	}

	public void ShowRewards()
	{
		ItemList.Clear();
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		switch (playerData.Profession)
		{
		case PROFESSION_TYPE.XD:
			if (!string.IsNullOrEmpty(curData.XDItemID1))
			{
				ItemList.Add(new GameItem(curData.XDItemID1, (EQUIP_QUALITY)curData.XDQuality1, curData.XDItemCount1));
			}
			if (!string.IsNullOrEmpty(curData.XDItemID2))
			{
				ItemList.Add(new GameItem(curData.XDItemID2, (EQUIP_QUALITY)curData.XDQuality2, curData.XDItemCount2));
			}
			if (!string.IsNullOrEmpty(curData.XDItemID3))
			{
				ItemList.Add(new GameItem(curData.XDItemID3, (EQUIP_QUALITY)curData.XDQuality3, curData.XDItemCount3));
			}
			if (!string.IsNullOrEmpty(curData.XDItemID4))
			{
				ItemList.Add(new GameItem(curData.XDItemID4, (EQUIP_QUALITY)curData.XDQuality4, curData.XDItemCount4));
			}
			break;
		case PROFESSION_TYPE.QJ:
			if (!string.IsNullOrEmpty(curData.QJItemID1))
			{
				ItemList.Add(new GameItem(curData.QJItemID1, (EQUIP_QUALITY)curData.QJQuality1, curData.QJItemCount1));
			}
			if (!string.IsNullOrEmpty(curData.QJItemID2))
			{
				ItemList.Add(new GameItem(curData.QJItemID2, (EQUIP_QUALITY)curData.QJQuality2, curData.QJItemCount2));
			}
			if (!string.IsNullOrEmpty(curData.QJItemID3))
			{
				ItemList.Add(new GameItem(curData.QJItemID3, (EQUIP_QUALITY)curData.QJQuality3, curData.QJItemCount3));
			}
			if (!string.IsNullOrEmpty(curData.QJItemID4))
			{
				ItemList.Add(new GameItem(curData.QJItemID4, (EQUIP_QUALITY)curData.QJQuality4, curData.QJItemCount4));
			}
			break;
		case PROFESSION_TYPE.NQS:
			if (!string.IsNullOrEmpty(curData.NQItemID1))
			{
				ItemList.Add(new GameItem(curData.NQItemID1, (EQUIP_QUALITY)curData.NQQuality1, curData.NQItemCount1));
			}
			if (!string.IsNullOrEmpty(curData.NQItemID2))
			{
				ItemList.Add(new GameItem(curData.NQItemID2, (EQUIP_QUALITY)curData.NQQuality2, curData.NQItemCount2));
			}
			if (!string.IsNullOrEmpty(curData.NQItemID3))
			{
				ItemList.Add(new GameItem(curData.NQItemID3, (EQUIP_QUALITY)curData.NQQuality3, curData.NQItemCount3));
			}
			if (!string.IsNullOrEmpty(curData.NQItemID4))
			{
				ItemList.Add(new GameItem(curData.NQItemID4, (EQUIP_QUALITY)curData.NQQuality4, curData.NQItemCount4));
			}
			break;
		}
		if (!string.IsNullOrEmpty(curData.ItemID5))
		{
			ItemList.Add(new GameItem(curData.ItemID5, (EQUIP_QUALITY)curData.Quality5, curData.ItemCount5));
		}
		int num = ItemList.Count - RewardList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(RewardList[0].gameObject) as GameObject;
				RewardItem component = gameObject.GetComponent<RewardItem>();
				gameObject.name = $"reward{RewardList.Count:D2}";
				gameObject.transform.parent = ParentGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				RewardList.Add(component);
			}
		}
		for (int j = 0; j < RewardList.Count; j++)
		{
			if (j < ItemList.Count)
			{
				if (j < 2)
				{
					RewardList[j].transform.parent = ParentGrid.transform;
					RewardList[j].transform.localScale = Vector3.one;
				}
				else
				{
					RewardList[j].transform.parent = ParentGrid2.transform;
					RewardList[j].transform.localScale = Vector3.one;
				}
				UnityVersionUtil.SetActiveRecursive(RewardList[j].gameObject, state: true);
				int level = 0;
				if (ItemList[j].ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.GetEquipEnhanceLevel(ItemList[j].ItemData.SubType);
				}
				RewardList[j].UpdateItem(ItemList[j], level);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(RewardList[j].gameObject, state: false);
			}
		}
		ParentGrid.Reposition();
		ParentGrid2.Reposition();
	}
}
