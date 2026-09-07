using SprotoType;
using UnityEngine;

public class DonateItem : MonoBehaviour
{
	public UILabel donateLabel;

	public UILabel contributeLabel;

	public UILabel costLabel;

	public UISprite BkSprite;

	private GuildDonateData curDonateData;

	private donate_record curRecod;

	private ItemData curItemData;

	public void Init(GuildDonateData data, donate_record record = null)
	{
		curDonateData = data;
		curRecod = record;
		curItemData = DataManager.GetItemDataByID(curDonateData.ItemID);
		donateLabel.text = $"+{curDonateData.Contribute}";
		contributeLabel.text = $"+{curDonateData.GuildExp}";
		costLabel.text = GameMoneyHelper.GetMoneyValStr(curDonateData.ItemCount, curDonateData.ItemID);
		if (record.DonateCount <= 0)
		{
			BkSprite.alpha = 0.5f;
		}
		else
		{
			BkSprite.alpha = 1f;
		}
	}

	public void OnClickDonate()
	{
		if (curRecod != null && curRecod.DonateCount > 0)
		{
			Singleton<ObjManager>.Instance.MainPlayer.GuildDonate(curDonateData);
		}
	}
}
