using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DailyBuyPackRootLogic : SingletonUnity<DailyBuyPackRootLogic>
{
	public UITexture BannerTexture;

	public List<DailyBuyItem> buyItems;

	private List<daily_buy> daily_buy_list;

	public UIGrid parentGrid;

	public void EnableReset()
	{
		for (int i = 0; i < buyItems.Count; i++)
		{
			UnityVersionUtil.SetActiveRecursive(buyItems[i].gameObject, state: false);
		}
		InitTexture();
	}

	public void InitTexture()
	{
		if (BannerTexture.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(GameDefine.TextureBannerDaily, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(string name, Texture textureObj)
	{
		BannerTexture.mainTexture = textureObj;
	}

	public void Reset(ret_request_daily_buy.request request)
	{
		daily_buy_list = new List<daily_buy>(request.daily_buys.Values);
		daily_buy_list.Sort((daily_buy x, daily_buy y) => int.Parse(x.ID) - int.Parse(y.ID));
		int num = daily_buy_list.Count - buyItems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(buyItems[0].gameObject) as GameObject;
				DailyBuyItem component = gameObject.GetComponent<DailyBuyItem>();
				gameObject.name = $"dailybuyitem{buyItems.Count:D2}";
				gameObject.transform.parent = parentGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				buyItems.Add(component);
			}
		}
		for (int j = 0; j < buyItems.Count; j++)
		{
			if (j < daily_buy_list.Count)
			{
				UnityVersionUtil.SetActiveRecursive(buyItems[j].gameObject, state: true);
				buyItems[j].UpdateInfo(daily_buy_list[j]);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(buyItems[j].gameObject, state: false);
			}
		}
		parentGrid.Reposition();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "DailyBuy", "open");
	}

	public void UpdateInfo(string id)
	{
		for (int i = 0; i < daily_buy_list.Count; i++)
		{
			if (daily_buy_list[i].ID.Equals(id))
			{
				daily_buy_list[i].state = 2L;
				buyItems[i].UpdateInfo(daily_buy_list[i]);
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "DailyBuy", $"require_{id}");
				break;
			}
		}
	}
}
