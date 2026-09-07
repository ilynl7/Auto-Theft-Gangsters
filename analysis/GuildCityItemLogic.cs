using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildCityItemLogic : MonoBehaviour
{
	public UISprite CityIcon;

	public UILabel CityLabel;

	public UILabel AttLabel;

	public UISprite GangIcon;

	public UILabel GangLabel;

	public ShowRewardItems ShowRewardRoot;

	public UISprite BtnSp;

	public UILabel BtnLabel;

	public UISprite CompleteFlag;

	private guild_map_info CurInfo;

	private GuildCaptureData CurCaptureData;

	private MapInfoData CurMapData;

	public void UpdateInfo(guild_map_info curdata)
	{
		CurInfo = curdata;
		CurCaptureData = DataManager.GetGuildCaptureDataByID(CurInfo.id);
		CurMapData = DataManager.GetMapInfoDataByID(CurCaptureData.MapID);
		CityIcon.spriteName = CurMapData.MapIcon;
		CityLabel.text = StrDictionary.GetDictionaryString(CurMapData.Name);
		AttLabel.text = $"{GameDefine.GetAttributeName_S(CurCaptureData.Basestatus)} {GameDefine.GetAttributeValueStr(CurCaptureData.Basestatus, CurCaptureData.BSValue)}";
		if (CurInfo.HasGuildId && CurInfo.guildId != 0L)
		{
			GangIcon.enabled = true;
			GangLabel.enabled = true;
			GangIcon.spriteName = GameDefine.GuildIcon[CurInfo.guildIcon];
			GangLabel.text = CurInfo.guildName;
		}
		else
		{
			GangIcon.enabled = false;
			GangLabel.enabled = false;
		}
		ShowRewardData showRewardData = null;
		showRewardData = DataManager.GetShowRewardDataByID(CurCaptureData.ShowRewardID);
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardRoot.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardRoot.gameObject, state: false);
		}
		if (CurInfo.state == 1)
		{
			NGUITools.SetActive(BtnSp.gameObject, state: true);
			BtnSp.spriteName = GameDefine.BtnIcon[0];
			BtnLabel.text = StrDictionary.GetDictionaryString("#{106036}");
			CompleteFlag.enabled = false;
		}
		else if (CurInfo.HasGuildId && CurInfo.guildId != 0L && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.ServerId == CurInfo.guildId)
		{
			if (CurInfo.HasRequireState)
			{
				if (CurInfo.requireState == 1)
				{
					NGUITools.SetActive(BtnSp.gameObject, state: true);
					BtnSp.spriteName = GameDefine.BtnIcon[0];
					BtnLabel.text = StrDictionary.GetDictionaryString("#{103006}");
					CompleteFlag.enabled = false;
				}
				else if (CurInfo.requireState == 2)
				{
					NGUITools.SetActive(BtnSp.gameObject, state: false);
					CompleteFlag.enabled = true;
				}
				else
				{
					NGUITools.SetActive(BtnSp.gameObject, state: true);
					BtnSp.spriteName = GameDefine.BtnIcon[2];
					BtnLabel.text = StrDictionary.GetDictionaryString("#{103006}");
					CompleteFlag.enabled = false;
				}
			}
			else
			{
				NGUITools.SetActive(BtnSp.gameObject, state: true);
				BtnSp.spriteName = GameDefine.BtnIcon[2];
				BtnLabel.text = StrDictionary.GetDictionaryString("#{103006}");
				CompleteFlag.enabled = false;
			}
		}
		else
		{
			NGUITools.SetActive(BtnSp.gameObject, state: true);
			BtnSp.spriteName = GameDefine.BtnIcon[2];
			BtnLabel.text = StrDictionary.GetDictionaryString("#{103006}");
			CompleteFlag.enabled = false;
		}
	}

	public bool UpdateRewardInfo(guild_map_info updateinfo)
	{
		if (CurInfo.id.Equals(updateinfo.id))
		{
			UpdateInfo(updateinfo);
			return true;
		}
		return false;
	}

	public void OnClickBtn()
	{
		if (CurInfo.state == 1)
		{
			if (CurMapData != null && !SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(CurMapData.OpenLv))
			{
				NoticeLogic.AddNotifyData2Client(false, "#{100154}", false, CurMapData.OpenLv);
				return;
			}
			MessageBoxLogic.OpenOKCancelBox("#{106032}", "#{100127}", delegate
			{
				MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
				if (CurCaptureData != null)
				{
					enter_guild_city_scene.request rpcReq = new enter_guild_city_scene.request
					{
						id = CurCaptureData.ID
					};
					NetLogic.GetInstance().Send<Protocol.enter_guild_city_scene>(rpcReq);
				}
				SingletonUnity<NewGuildUIRootLogic>.Instance.OnClickCloseBtn();
			});
		}
		else if (CurInfo.HasGuildId && CurInfo.guildId != 0L && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.ServerId == CurInfo.guildId)
		{
			if (CurInfo.HasRequireState && CurInfo.requireState == 1)
			{
				request_guild_map_reward.request request = new request_guild_map_reward.request();
				request.id = CurInfo.id;
				NetLogic.GetInstance().Send<Protocol.request_guild_map_reward>(request);
			}
		}
		else
		{
			NoticeLogic.AddNotifyData("#{106037}");
		}
	}

	private void SetRewardItem(List<string> itemIds, List<EQUIP_QUALITY> qualitys, List<int> counts)
	{
		ShowRewardRoot.ShowRewards(itemIds, qualitys, counts);
	}
}
