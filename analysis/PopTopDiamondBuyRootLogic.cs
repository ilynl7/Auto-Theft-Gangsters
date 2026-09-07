using System.Collections.Generic;
using SprotoType;

public class PopTopDiamondBuyRootLogic : SingletonUnity<PopTopDiamondBuyRootLogic>
{
	private List<PurchaseData> mCurPurchaseDataList = new List<PurchaseData>();

	public List<BuyDiamondItem> BuyDiamondItemList = new List<BuyDiamondItem>();

	public void EnableReset()
	{
		for (int i = 0; i < BuyDiamondItemList.Count; i++)
		{
			NGUITools.SetActive(BuyDiamondItemList[i].gameObject, state: false);
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Shop_dollar", "open", "times");
	}

	public void Reset(ret_ask_shop_list.request request)
	{
		mCurPurchaseDataList.Clear();
		for (int i = 0; i < request.shop_list.Count; i++)
		{
			mCurPurchaseDataList.Add(DataManager.GetPurchaseDataBuyId(request.shop_list[i].ID));
		}
		mCurPurchaseDataList.Sort((PurchaseData pre, PurchaseData next) => pre.Key.CompareTo(next.Key));
		for (int j = 0; j < BuyDiamondItemList.Count; j++)
		{
			NGUITools.SetActive(BuyDiamondItemList[j].gameObject, j < mCurPurchaseDataList.Count);
			if (j < mCurPurchaseDataList.Count)
			{
				BuyDiamondItemList[j].Reset(mCurPurchaseDataList[j]);
			}
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PopTopDiamondBuyRoot);
	}
}
