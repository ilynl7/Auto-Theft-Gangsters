using SprotoType;
using UnityEngine;

public class BuyDiamondItem : MonoBehaviour
{
	public UILabel GetNumLabel;

	public UILabel PriceLabel;

	public UISprite AdFreePic;

	private PurchaseData mCurPurchaseData;

	public UISprite SaleInfoSp;

	public void Reset(PurchaseData curData)
	{
		NGUITools.SetActive(AdFreePic.gameObject, curData.AdFree == 1 && !LocalDataSaveManager.AdFree);
		GetNumLabel.text = curData.ShowPriceCost.ToString();
		PriceLabel.text = $"${curData.Dollor}";
		mCurPurchaseData = curData;
		if (mCurPurchaseData.BuyCount == 1)
		{
			if (!string.IsNullOrEmpty(mCurPurchaseData.SalePicName))
			{
				SaleInfoSp.spriteName = mCurPurchaseData.SalePicName;
				SaleInfoSp.MakePixelPerfect();
			}
			UnityVersionUtil.SetActiveRecursive(SaleInfoSp.gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(SaleInfoSp.gameObject, state: false);
		}
	}

	public void OnClickItem()
	{
		SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(4);
		SingletonDontDestoryUnity<GameManager>.Instance.Billing(mCurPurchaseData.ProductId);
		if (GameSettingData.IsTestBilling)
		{
			check_purchase.request request = new check_purchase.request();
			request.productId = mCurPurchaseData.ProductId;
			NetLogic.GetInstance().Send<Protocol.check_purchase>(request);
		}
	}
}
