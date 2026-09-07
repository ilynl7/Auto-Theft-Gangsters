using SprotoType;
using UnityEngine;

public class ConsignItemLogic : MonoBehaviour
{
	public UILabel SellCountLabel;

	public UILabel UseLevelLabel;

	public UILabel PriceLabel;

	public UILabel TimeLeftLabel;

	public UISprite ItemIconSprite;

	public UISprite ItemIconQualitySprite;

	public UILabel ItemNameLabel;

	public UILabel BtnLabel;

	private consign_item currentConsignItem;

	public string GetTimeShow()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		long num = playerCommonData.GetCurServerTime() - currentConsignItem.startTime;
		long num2 = (1 << (int)currentConsignItem.time) * 12 * 60 * 60 - num;
		long num3 = num2 / 86400;
		long num4 = (num2 - num3 * 60 * 60 * 24) / 3600;
		long num5 = num2 % 60;
		if (num3 > 0 && num4 > 0)
		{
			return $"{num3}d{num4}h";
		}
		if (num3 > 0)
		{
			return $"{num3}d";
		}
		if (num4 > 0)
		{
			return $"{num4}h";
		}
		if (num5 > 0)
		{
			return $"{num5}m";
		}
		return "expire";
	}

	public void Reset(consign_item item)
	{
		currentConsignItem = item;
		ItemData itemDataByID = DataManager.GetItemDataByID(currentConsignItem.itemId);
		ItemIconSprite.spriteName = itemDataByID.BackPackIcon;
		if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			ItemIconQualitySprite.spriteName = ((EQUIP_QUALITY)currentConsignItem.quality).ToString();
		}
		else
		{
			ItemIconQualitySprite.spriteName = itemDataByID.QualityType.ToString();
		}
		if (item.stack > 1)
		{
			SellCountLabel.text = currentConsignItem.stack.ToString();
		}
		else
		{
			SellCountLabel.text = string.Empty;
		}
		ItemNameLabel.text = itemDataByID.MName;
		PriceLabel.text = currentConsignItem.price.ToString();
		if (itemDataByID.Type == GameDefine.ITEM_TYPE.BADGE)
		{
			BadgeData badgeDataById = DataManager.GetBadgeDataById(itemDataByID.ID);
			UseLevelLabel.text = badgeDataById.Lv.ToString();
		}
		else
		{
			UseLevelLabel.text = itemDataByID.Level.ToString();
		}
		TimeLeftLabel.text = GetTimeShow();
		if (currentConsignItem.characterId == Singleton<ObjManager>.Instance.MainPlayer.ServerId)
		{
			BtnLabel.text = StrDictionary.GetDictionaryString("#{101218}");
		}
		else
		{
			BtnLabel.text = StrDictionary.GetDictionaryString("#{101201}");
		}
	}

	public void ClickUndoBtn()
	{
		if (currentConsignItem.characterId == Singleton<ObjManager>.Instance.MainPlayer.ServerId)
		{
			consign_cancel_sale.request request = new consign_cancel_sale.request();
			request.id = currentConsignItem.id;
			NetLogic.GetInstance().Send<Protocol.consign_cancel_sale>(request);
		}
		else if (GameMoneyHelper.BeforeCheckBuy(GameDefine.MONEY_TYPE.DIAMOND, (int)currentConsignItem.price))
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ConsignBuyCheckRoot, delegate
			{
				SingletonUnity<ConsignBuyCheckRoot>.Instance.Reset(currentConsignItem);
			});
		}
	}

	public void OnClickItem()
	{
		ItemInfoRootLogicNew.ShowItemTips(currentConsignItem);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
