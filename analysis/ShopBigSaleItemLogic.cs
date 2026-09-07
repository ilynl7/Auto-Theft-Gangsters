using System;
using SprotoType;
using UnityEngine;

public class ShopBigSaleItemLogic : MonoBehaviour
{
	public delegate void OnClickShopBigSaleItemDelegate(special_big_pack info, GameObject itemobj);

	public OnClickShopBigSaleItemDelegate onClickShopItem;

	public UISprite Icon;

	public UILabel NameLabel;

	public UILabel PriceLabel;

	public UISprite ItemQualitySprite;

	public UISprite BkSprite;

	public GameObject ForSaleObj;

	public UILabel saleValueLabel;

	public UISprite ExpireFlag;

	public UILabel limittimelabel;

	public GameObject limittimeobj;

	private bool isHaveLimittime;

	private float starttime;

	private float limitTime;

	private special_big_pack CurInfo;

	public void updateinfo(special_big_pack curinfo)
	{
		if (curinfo.ID.Equals(CurInfo.ID))
		{
			CurInfo = curinfo;
		}
	}

	public void Reset(special_big_pack curinfo, OnClickShopBigSaleItemDelegate clickitem = null)
	{
		CurInfo = curinfo;
		onClickShopItem = clickitem;
		BigPackageData bigPackageDataById = DataManager.GetBigPackageDataById(CurInfo.ID);
		if (bigPackageDataById == null)
		{
			return;
		}
		Icon.spriteName = bigPackageDataById.Icon;
		ItemQualitySprite.spriteName = ((EQUIP_QUALITY)bigPackageDataById.IconQuality).ToString();
		NameLabel.text = StrDictionary.GetDictionaryString(bigPackageDataById.Name);
		if (string.IsNullOrEmpty(bigPackageDataById.ProductId))
		{
			PriceLabel.text = GameMoneyHelper.GetMoneyValStr(bigPackageDataById.PriceCost, bigPackageDataById.PriceType);
		}
		else
		{
			PriceLabel.text = $"${bigPackageDataById.Dollor}";
		}
		if (bigPackageDataById.Discount < 100 && bigPackageDataById.Discount != 0)
		{
			saleValueLabel.text = $"{bigPackageDataById.Discount}";
			UnityVersionUtil.SetActiveRecursive(ForSaleObj, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ForSaleObj, state: false);
		}
		ExpireFlag.enabled = false;
		isHaveLimittime = false;
		if (!string.IsNullOrEmpty(bigPackageDataById.TimeList))
		{
			TimeSpan shopItemTime = TimeTools.GetShopItemTime(bigPackageDataById.GetCurTimeEnd());
			limitTime = (float)shopItemTime.TotalSeconds;
			isHaveLimittime = true;
			starttime = Time.time;
			if (shopItemTime.Days > 0)
			{
				limittimelabel.text = $"{shopItemTime.Days + 1}D";
			}
			else if (shopItemTime.Hours > 0)
			{
				limittimelabel.text = $"{shopItemTime.Hours + 1}H";
			}
			else if (shopItemTime.Minutes > 0)
			{
				limittimelabel.text = $"{shopItemTime.Minutes + 1}M";
			}
			else if (shopItemTime.Seconds > 0)
			{
				limittimelabel.text = $"{shopItemTime.Seconds}S";
			}
			if (shopItemTime.TotalSeconds <= 0.0)
			{
				limittimelabel.text = "0S";
			}
			NGUITools.SetActive(limittimeobj.gameObject, state: true);
		}
		else
		{
			NGUITools.SetActive(limittimeobj.gameObject, state: false);
		}
	}

	public void OnClickItemBtn()
	{
		if (onClickShopItem != null)
		{
			onClickShopItem(CurInfo, base.gameObject);
		}
	}

	public void UpdateSelect(special_big_pack item)
	{
		if (CurInfo != null && CurInfo.ID.Equals(item.ID))
		{
			BkSprite.spriteName = "CZ_huaDongBG_1";
		}
		else
		{
			BkSprite.spriteName = "CZ_huaDongBG";
		}
	}

	private void Update()
	{
		if (isHaveLimittime)
		{
			limitTime -= Time.deltaTime;
			TimeSpan timeSpan = new TimeSpan(0, 0, (int)limitTime);
			if (timeSpan.Days > 0)
			{
				limittimelabel.text = $"{timeSpan.Days + 1}D";
			}
			else if (timeSpan.Hours > 0)
			{
				limittimelabel.text = $"{timeSpan.Hours + 1}H";
			}
			else if (timeSpan.Minutes > 0)
			{
				limittimelabel.text = $"{timeSpan.Minutes + 1}M";
			}
			else if (timeSpan.Seconds > 0)
			{
				limittimelabel.text = $"{timeSpan.Seconds}S";
			}
			if (timeSpan.TotalSeconds <= 0.0)
			{
				limittimelabel.text = "0S";
				isHaveLimittime = false;
			}
		}
	}
}
