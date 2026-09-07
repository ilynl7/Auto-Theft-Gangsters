using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class EnemyRevengeRootLogic : SingletonUnity<EnemyRevengeRootLogic>
{
	private friend_info curEnemyinfo;

	private update_player_map_info.response CurRes;

	public void Reset(friend_info enemy, update_player_map_info.response response)
	{
		curEnemyinfo = enemy;
		CurRes = response;
	}

	public void OnClickTraceBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EnemyRevengeRoot);
		if (CurRes.state != 1)
		{
			NoticeLogic.AddNotifyData("#{103318}");
		}
		else if (CurRes.HasMapid && CurRes.HasPos)
		{
			MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			float x = (float)CurRes.pos.x / 100f;
			float z = (float)CurRes.pos.z / 100f;
			Vector3 pos = new Vector3(x, SceneManager.GetHitHeight(new Vector3(x, 0f, z)), z);
			MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(CurRes.mapid);
			if (mapInfoDataByID != null && mapInfoDataByID.MapType == MAPTYPE.BIG_WORLD)
			{
				missionManager.AutoMoveDest(CurRes.mapid, pos, AUTO_SEARCH_PARTH_FINISHEVENT.INVALID);
				SingletonUnity<SocialUIRootLogic>.Instance.OnClickCloseBtn();
			}
			else
			{
				NoticeLogic.AddNotifyData("#{103318}");
			}
		}
	}

	public void OnClickWarpBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EnemyRevengeRoot);
		if (CurRes.state != 1)
		{
			NoticeLogic.AddNotifyData("#{103318}");
			return;
		}
		if (CurRes.HasMapid && CurRes.HasPos)
		{
			MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(CurRes.mapid);
			if (mapInfoDataByID == null || mapInfoDataByID.MapType != MAPTYPE.BIG_WORLD)
			{
				NoticeLogic.AddNotifyData("#{103318}");
				return;
			}
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		int itemStackNumById = playerData.ItemBackPack.GetItemStackNumById(GameDefine.EnemyWarpToolItem);
		if (itemStackNumById > 0)
		{
			ItemContainer itemBackPack = playerData.ItemBackPack;
			List<GameItem> targetItemByID = ItemContainerTool.GetTargetItemByID(itemBackPack, GameDefine.EnemyWarpToolItem);
			bool flag = false;
			GameItem gameItem = null;
			if (targetItemByID == null || targetItemByID.Count == 0)
			{
				flag = false;
			}
			else
			{
				flag = true;
				gameItem = targetItemByID[0];
			}
			if (flag)
			{
				gather_other_player.request request = new gather_other_player.request();
				request.characterid = curEnemyinfo.friendId;
				NetLogic.GetInstance().Send<Protocol.gather_other_player>(request);
			}
		}
		else
		{
			GameMoneyHelper.ShowItemProduct(GameDefine.EnemyWarpToolItem);
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EnemyRevengeRoot);
	}
}
