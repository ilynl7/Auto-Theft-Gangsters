using System;
using SprotoType;
using UnityEngine;

public class ShopItemBtnLogic : MonoBehaviour
{
	public delegate void OnClickShopItemDelegate(shop_item itemKey, GameObject itemobj);

	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public OnClickShopItemDelegate onClickShopItem;

	public UISprite Icon;

	public UILabel NameLabel;

	public UILabel PriceLabel;

	public UISprite ItemQualitySprite;

	public UISprite BkSprite;

	private GameDefine.SHOP_TYPE curType;

	private shop_item mCurShopItem;

	public GameObject ForSaleObj;

	public UILabel saleValueLabel;

	public UISprite ExpireFlag;

	public UILabel limittimelabel;

	public GameObject limittimeobj;

	private bool isHaveLimittime;

	private float starttime;

	private float limitTime;

	public UILabel NumLimitLabel;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (SingletonUnity<TutorialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<TutorialUIRootLogic>.Instance.CloseCheck();
		}
		if (mOnClickTutorialBtn != null)
		{
			TutorialManager.OnClickTutorialBtn onClickTutorialBtn = mOnClickTutorialBtn;
			mOnClickTutorialBtn = null;
			onClickTutorialBtn();
		}
	}

	public void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	public void Reset(shop_item item, GameDefine.SHOP_TYPE type, OnClickShopItemDelegate clickfun)
	{
		onClickShopItem = clickfun;
		curType = type;
		mCurShopItem = item;
		ItemData itemDataByID = DataManager.GetItemDataByID(item.ItemID);
		if (itemDataByID != null)
		{
			Icon.spriteName = itemDataByID.BackPackIcon;
			NameLabel.text = itemDataByID.MName;
			PriceLabel.text = GameMoneyHelper.GetMoneyValStr(item.Price, item.PriceType);
			if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
			{
				UnityVersionUtil.SetActiveRecursive(ItemQualitySprite.gameObject, state: true);
				ItemQualitySprite.spriteName = ((EQUIP_QUALITY)item.Quality).ToString();
				NameLabel.color = GameDefine.GetColorByQuality((EQUIP_QUALITY)item.Quality);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(ItemQualitySprite.gameObject, state: true);
				ItemQualitySprite.spriteName = itemDataByID.QualityType.ToString();
				NameLabel.color = GameDefine.GetColorByQuality(itemDataByID.QualityType);
			}
			if (itemDataByID.UseHour > 0)
			{
				if (ExpireFlag != null)
				{
					ExpireFlag.enabled = true;
				}
			}
			else if (ExpireFlag != null)
			{
				ExpireFlag.enabled = false;
			}
		}
		if (mCurShopItem.HasDiscount && mCurShopItem.Discount < 100)
		{
			saleValueLabel.text = $"{mCurShopItem.Discount}";
			UnityVersionUtil.SetActiveRecursive(ForSaleObj, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ForSaleObj, state: false);
		}
		if (mCurShopItem.Limit > 0)
		{
			int num = (int)(mCurShopItem.Limit - mCurShopItem.curNum);
			NumLimitLabel.text = StrDictionary.GetDictionaryString("#{100746}", num);
			NumLimitLabel.enabled = true;
		}
		else
		{
			NumLimitLabel.enabled = false;
		}
		isHaveLimittime = false;
		ShopData shopDataByID = DataManager.GetShopDataByID(item.ID);
		if (shopDataByID == null)
		{
			return;
		}
		if (string.IsNullOrEmpty(shopDataByID.EndTime))
		{
			UnityVersionUtil.SetActiveRecursive(limittimeobj, state: false);
			return;
		}
		TimeSpan shopItemTime = TimeTools.GetShopItemTime(shopDataByID.EndTimes);
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
		UnityVersionUtil.SetActiveRecursive(limittimeobj, state: true);
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

	public void UpdateSelect(shop_item item)
	{
		if (item != null && mCurShopItem.ID == item.ID)
		{
			BkSprite.spriteName = "CZ_huaDongBG_1";
		}
		else
		{
			BkSprite.spriteName = "CZ_huaDongBG";
		}
	}

	public void OnClickBtn()
	{
		if (onClickShopItem != null)
		{
			onClickShopItem(mCurShopItem, base.gameObject);
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.BUY_BADGE_CHOOSE_BADGE)
		{
			CheckTutorialEvent();
		}
	}

	public void UpdateInfo(shop_item newitem)
	{
		if (newitem.ID == mCurShopItem.ID)
		{
			mCurShopItem.curNum = newitem.curNum;
		}
		if (mCurShopItem.Limit > 0)
		{
			int num = (int)(mCurShopItem.Limit - mCurShopItem.curNum);
			NumLimitLabel.text = StrDictionary.GetDictionaryString("#{100746}", num);
			NumLimitLabel.enabled = true;
		}
		else
		{
			NumLimitLabel.enabled = false;
		}
	}
}
