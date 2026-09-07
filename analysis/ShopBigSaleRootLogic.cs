using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ShopBigSaleRootLogic : SingletonUnity<ShopBigSaleRootLogic>
{
	public List<ShopBigSaleItemLogic> ShopItemList;

	private int PageMax = 8;

	private List<special_big_pack> SpePackList = new List<special_big_pack>();

	public UIGrid ItemGrid;

	private int CurPage;

	private int MaxPage;

	private special_big_pack CurSelectInfo;

	public ShopBigSaleItemInfo ItemInfoRoot;

	public UISprite BuyBtnSp;

	public UILabel BuyBtnLabel;

	private BigPackageData CurBigPackData;

	private bool IsDollorBuy;

	private Color ambientLight;

	public UILabel PageLabel;

	public void EnableReset()
	{
		CurPage = 1;
		MaxPage = 1;
		ambientLight = RenderSettings.ambientLight;
		for (int i = 0; i < ShopItemList.Count; i++)
		{
			NGUITools.SetActive(ShopItemList[i].gameObject, state: false);
		}
		ItemInfoRoot.Reset();
	}

	public void Reset(ret_special_big_pack.request request)
	{
		SpePackList.Clear();
		SpePackList = new List<special_big_pack>(request.special_big_packs.Values);
		for (int num = SpePackList.Count - 1; num >= 0; num--)
		{
			BigPackageData bigPackageDataById = DataManager.GetBigPackageDataById(SpePackList[num].ID);
			if (bigPackageDataById != null)
			{
				if (bigPackageDataById.SellType != 2 || SpePackList[num].state != 0L)
				{
					SpePackList.RemoveAt(num);
				}
			}
			else
			{
				SpePackList.RemoveAt(num);
			}
		}
		SpePackList.Sort(delegate(special_big_pack x, special_big_pack y)
		{
			BigPackageData bigPackageDataById2 = DataManager.GetBigPackageDataById(x.ID);
			BigPackageData bigPackageDataById3 = DataManager.GetBigPackageDataById(y.ID);
			return (bigPackageDataById2.sortID == bigPackageDataById3.sortID) ? (int.Parse(x.ID) - int.Parse(y.ID)) : (bigPackageDataById2.sortID - bigPackageDataById3.sortID);
		});
		int num2 = Mathf.Min(SpePackList.Count, PageMax) - ShopItemList.Count;
		int count = ShopItemList.Count;
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				GameObject gameObject = Object.Instantiate(ShopItemList[0].gameObject) as GameObject;
				gameObject.name = $"shangPing_{count + i}";
				ItemGrid.AddChild(gameObject.transform);
				gameObject.transform.localScale = Vector3.one;
				ShopItemList.Add(gameObject.GetComponent<ShopBigSaleItemLogic>());
			}
		}
		MaxPage = (SpePackList.Count - 1) / PageMax + 1;
		if (MaxPage <= 0)
		{
			MaxPage = 1;
		}
		ShowPage(1);
	}

	public void ShowPage(int page)
	{
		CurPage = page;
		List<special_big_pack> list = new List<special_big_pack>();
		for (int i = (page - 1) * PageMax; i < page * PageMax; i++)
		{
			if (i < SpePackList.Count)
			{
				list.Add(SpePackList[i]);
			}
		}
		for (int j = 0; j < ShopItemList.Count; j++)
		{
			if (j < list.Count)
			{
				NGUITools.SetActive(ShopItemList[j].gameObject, state: true);
				ShopItemList[j].Reset(list[j], OnClickItemBtn);
			}
			else
			{
				NGUITools.SetActive(ShopItemList[j].gameObject, state: false);
			}
		}
		ItemGrid.Reposition();
		PageLabel.text = $"{CurPage}/{MaxPage}";
		if (list.Count > 0)
		{
			ShopItemList[0].OnClickItemBtn();
		}
	}

	public void OnClickItemBtn(special_big_pack selectinfo, GameObject itemobj)
	{
		if (CurSelectInfo == null || !CurSelectInfo.ID.Equals(selectinfo))
		{
			CurSelectInfo = selectinfo;
			CurBigPackData = DataManager.GetBigPackageDataById(CurSelectInfo.ID);
			UpdateSelectObj();
			ItemInfoRoot.RefershInfo(CurSelectInfo);
			UpdateBuyLabel();
		}
	}

	public void UpdateBuyLabel()
	{
		if (string.IsNullOrEmpty(CurBigPackData.ProductId))
		{
			IsDollorBuy = false;
			BuyBtnLabel.text = GameMoneyHelper.GetMoneyValStr(CurBigPackData.PriceCost, CurBigPackData.PriceType);
		}
		else
		{
			IsDollorBuy = true;
			BuyBtnLabel.text = $"${CurBigPackData.Dollor}";
		}
		if (CurSelectInfo.state != 0L)
		{
			BuyBtnSp.spriteName = GameDefine.BtnIcon[2];
		}
		else
		{
			BuyBtnSp.spriteName = GameDefine.BtnIcon[1];
		}
	}

	public void OnClickBuyBtn()
	{
		if (CurSelectInfo == null || CurSelectInfo.state != 0L)
		{
			return;
		}
		if (IsDollorBuy)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.Billing(CurBigPackData.ProductId);
			if (GameSettingData.IsTestBilling)
			{
				WaitResponseUIRootLogic.OpenWaitBox(266, 10f, 0f);
				check_purchase.request request = new check_purchase.request();
				request.productId = CurBigPackData.ProductId;
				NetLogic.GetInstance().Send<Protocol.check_purchase>(request);
			}
		}
		else if (GameMoneyHelper.BeforeCheckBuy(CurBigPackData.PriceType, CurBigPackData.PriceCost))
		{
			WaitResponseUIRootLogic.OpenWaitBox(272, 10f, 0f);
			buy_big_pack.request request2 = new buy_big_pack.request();
			request2.ID = CurBigPackData.ID;
			NetLogic.GetInstance().Send<Protocol.buy_big_pack>(request2);
		}
	}

	public void UpdateSelectObj()
	{
		for (int i = 0; i < ShopItemList.Count; i++)
		{
			if (UnityVersionUtil.IsActive(ShopItemList[i].gameObject))
			{
				ShopItemList[i].UpdateSelect(CurSelectInfo);
			}
		}
	}

	public void OnClickLeftBtn()
	{
		if (CurPage > 1)
		{
			CurPage--;
			ShowPage(CurPage);
		}
	}

	public void OnClickRightBtn()
	{
		if (CurPage < MaxPage)
		{
			CurPage++;
			ShowPage(CurPage);
		}
	}

	public void ResetNormalLight()
	{
		RenderSettings.ambientLight = ambientLight;
	}

	private void OnDisable()
	{
		ResetNormalLight();
		ItemInfoRoot.UnLoadFakeObj();
	}

	public void UpdateInfo(string id)
	{
		for (int i = 0; i < SpePackList.Count; i++)
		{
			if (SpePackList[i].ID.Equals(id))
			{
				SpePackList[i].state = 2L;
				if (CurSelectInfo.ID.Equals(id))
				{
					CurSelectInfo = SpePackList[i];
					UpdateBuyLabel();
				}
				for (int j = 0; j < ShopItemList.Count; j++)
				{
					ShopItemList[j].updateinfo(SpePackList[i]);
				}
				break;
			}
		}
	}
}
