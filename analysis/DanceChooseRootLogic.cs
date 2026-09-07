using SprotoType;
using UnityEngine;

public class DanceChooseRootLogic : SingletonUnity<DanceChooseRootLogic>
{
	public UILabel CostLabel;

	public UILabel DanceNameLabel;

	private void OnEnable()
	{
		Reset();
	}

	public void OnClickFreeDanceBtn()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsInGatherArea(Singleton<ObjManager>.Instance.MainPlayer.Position))
		{
			OnClickCloseBtn();
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		use_dance.request request = new use_dance.request();
		request.id = playerData.PlayerDanceData.CurNormalDanceData.ID;
		NetLogic.GetInstance().Send<Protocol.use_dance>(request);
		OnClickCloseBtn();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", "dance", $"usedance_{request.id}");
	}

	public void OnClickSpecialDanceBtn()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsInGatherArea(Singleton<ObjManager>.Instance.MainPlayer.Position))
		{
			OnClickCloseBtn();
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerDanceData.IsSpecialDanceDataEnable())
		{
			Singleton<ObjManager>.Instance.MainPlayer.StopDance();
			use_dance.request request = new use_dance.request();
			request.id = playerData.PlayerDanceData.CurSpecialDanceData.ID;
			NetLogic.GetInstance().Send<Protocol.use_dance>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", "dance", $"usedance_{request.id}");
		}
		else if (GameMoneyHelper.BeforeCheckBuy(playerData.PlayerDanceData.CurSpecialDanceData.PriceType, playerData.PlayerDanceData.CurSpecialDanceData.Price))
		{
			Singleton<ObjManager>.Instance.MainPlayer.StopDance();
			use_dance.request request2 = new use_dance.request();
			request2.id = playerData.PlayerDanceData.CurSpecialDanceData.ID;
			NetLogic.GetInstance().Send<Protocol.use_dance>(request2);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", "dance", $"usedance_{request2.id}");
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", "dance", $"buydance_{request2.id}");
		}
		OnClickCloseBtn();
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DanceChooseRoot);
	}

	public void Reset()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerDanceData.IsSpecialDanceDataEnable())
		{
			NGUITools.SetActive(CostLabel.gameObject, state: false);
			DanceNameLabel.transform.localPosition = Vector3.zero;
			return;
		}
		NGUITools.SetActive(CostLabel.gameObject, state: true);
		GameDefine.MONEY_TYPE priceType = (GameDefine.MONEY_TYPE)playerData.PlayerDanceData.CurSpecialDanceData.PriceType;
		CostLabel.text = GameMoneyHelper.GetMoneyValStr(playerData.PlayerDanceData.CurSpecialDanceData.Price, priceType);
		DanceNameLabel.transform.localPosition = Vector3.up * 8f;
	}
}
