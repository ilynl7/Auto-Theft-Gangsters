using SprotoType;

public class ConsignBuyCheckRoot : SingletonUnity<ConsignBuyCheckRoot>
{
	public UILabel InfoLabel;

	public UISprite ItemIconSprite;

	public UISprite ItemCircleSprite;

	private consign_item mCurItem;

	public void Reset(consign_item curItem)
	{
		mCurItem = curItem;
		ItemData itemDataByID = DataManager.GetItemDataByID(mCurItem.itemId);
		ItemIconSprite.spriteName = itemDataByID.BackPackIcon;
		if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			ItemCircleSprite.spriteName = ((EQUIP_QUALITY)curItem.quality).ToString();
		}
		else
		{
			ItemCircleSprite.spriteName = itemDataByID.QualityType.ToString();
		}
		InfoLabel.text = StrDictionary.GetDictionaryString("#{301117}", mCurItem.price);
	}

	public void OnClickYesBtn()
	{
		if (GameMoneyHelper.BeforeCheckBuy(GameDefine.MONEY_TYPE.DIAMOND, (int)mCurItem.price))
		{
			consign_buy_item.request request = new consign_buy_item.request();
			request.id = mCurItem.id;
			request.itemId = mCurItem.itemId;
			NetLogic.GetInstance().Send<Protocol.consign_buy_item>(request);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ConsignBuyCheckRoot);
		}
	}

	public void OnClickNoBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ConsignBuyCheckRoot);
	}
}
