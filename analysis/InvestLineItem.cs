using SprotoType;
using UnityEngine;

public class InvestLineItem : MonoBehaviour
{
	public UILabel InfoLabel;

	private invest_pack curInfo;

	private InvestData curData;

	public UILabel BtnLabel;

	public UISprite btnSp;

	public UILabel DiamondLabel;

	public int ItmeIndex;

	public UISprite completeFlag;

	public void refershinfo(invest_pack curpark, InvestData curdata, int index)
	{
		if (curpark.ID.Equals(curInfo.ID))
		{
			UpdateInfo(curpark, curdata, index);
		}
	}

	public void UpdateInfo(invest_pack curpark, InvestData curdata, int index)
	{
		curInfo = curpark;
		curData = curdata;
		ItmeIndex = index;
		InfoLabel.text = StrDictionary.GetDictionaryString("#{300401}", curData.LvTarget);
		completeFlag.enabled = false;
		long state = curInfo.state;
		if (state >= -1 && state <= 2)
		{
			switch (state - -1)
			{
			case 0L:
				btnSp.spriteName = GameDefine.BtnIconNew[1];
				BtnLabel.text = StrDictionary.GetDictionaryString("#{300402}");
				break;
			case 1L:
				btnSp.spriteName = GameDefine.BtnIconNew[1];
				BtnLabel.text = StrDictionary.GetDictionaryString("#{300402}");
				break;
			case 2L:
				btnSp.spriteName = GameDefine.BtnIconNew[0];
				BtnLabel.text = StrDictionary.GetDictionaryString("#{300402}");
				break;
			case 3L:
				btnSp.spriteName = GameDefine.BtnIconNew[1];
				BtnLabel.text = StrDictionary.GetDictionaryString("#{300403}");
				completeFlag.enabled = true;
				break;
			}
		}
		DiamondLabel.text = $"{curData.ItemCount1}";
	}

	public void OnClickBtn()
	{
		if (curInfo.state != -1)
		{
			if (curInfo.state == 1)
			{
				WaitResponseUIRootLogic.OpenWaitBox(263, 10f, 0f);
				require_invest_reward.request request = new require_invest_reward.request();
				request.ID = curInfo.ID;
				NetLogic.GetInstance().Send<Protocol.require_invest_reward>(request);
			}
			else if (curInfo.state == 0L)
			{
				NoticeLogic.AddNotifyData("#{300404}");
			}
		}
	}
}
