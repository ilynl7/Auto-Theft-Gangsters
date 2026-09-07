using System.Collections.Generic;
using Sproto;
using SprotoType;

public class main_player_create_handler
{
	public static SprotoTypeBase main_player_create_request(SprotoTypeBase req)
	{
		if (req is main_player_create.request request)
		{
			if (request.HasCharacter)
			{
				ObjInitPlayerData objInitPlayerData = new ObjInitPlayerData();
				objInitPlayerData.InitData(request.character);
				GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
				instance.PlayerData.IsTutorialFinish = request.character.general.HasTutorial && request.character.general.tutorial == 1;
				if (request.character.attribute_other.HasGuildId)
				{
					instance.PlayerData.PlayerGuild.ServerId = request.character.attribute_other.guildId;
					instance.PlayerData.PlayerGuild.GuilName = request.character.attribute_other.guildName;
					instance.PlayerData.PlayerGuild.PlayerJob = (Guild_JOB)request.character.attribute_other.guildJob;
				}
				GameMoneyHelper.UpdateMoney(request.character.property.money1, request.character.property.money2, request.character.property.money3, request.character.property.money4, request.character.property.money5, request.character.property.money6);
				if (request.character.HasPotionIndex)
				{
					instance.PlayerData.CurSelectPotionIndex = request.character.potionIndex;
					if (UIUpdateEvent.SyncBackPackEvent != null)
					{
						UIUpdateEvent.SyncBackPackEvent();
					}
				}
				if (request.character.HasDownload && request.character.download == 2)
				{
					instance.PlayerData.IsFinishDownload = true;
				}
				else
				{
					instance.PlayerData.IsFinishDownload = false;
				}
				ItemContainer itemContainer = instance.PlayerData.GetItemContainer(ITEM_CONTAINER_TYPE.EQUIPPACK);
				if (itemContainer != null && request.character.HasEquip)
				{
					SyncPack(new List<gameitem>(request.character.equip.Values), itemContainer);
				}
				itemContainer = instance.PlayerData.GetItemContainer(ITEM_CONTAINER_TYPE.BADGE_EQUIPPACK);
				if (itemContainer != null && request.character.HasBadge_equip)
				{
					SyncPack(new List<gameitem>(request.character.badge_equip.Values), itemContainer);
				}
				itemContainer = instance.PlayerData.GetItemContainer(ITEM_CONTAINER_TYPE.FASHION_EQUIPPACK);
				if (itemContainer != null && request.character.HasFashion_equip)
				{
					SyncPack(new List<gameitem>(request.character.fashion_equip.Values), itemContainer);
				}
				Singleton<ObjManager>.Instance.CreateMainPlayer(objInitPlayerData);
				instance.PlayerData.UpdateEquipsTips();
			}
			else
			{
				Singleton<ObjManager>.Instance.CreateMainPlayer(request.movement);
			}
		}
		return null;
	}

	private static void SyncPack(List<gameitem> itemList, ItemContainer container)
	{
		for (int i = 0; i < itemList.Count; i++)
		{
			gameitem gameitem = itemList[i];
			GameItem gameItem = null;
			if (container.ContainType == ITEM_CONTAINER_TYPE.EQUIPPACK)
			{
				gameItem = container.GetItemByIndexId(gameitem.indexId);
			}
			else if (container.ContainType == ITEM_CONTAINER_TYPE.BADGE_EQUIPPACK)
			{
				gameItem = container.GetItemByIndex((int)gameitem.parm[0]);
			}
			else if (container.ContainType == ITEM_CONTAINER_TYPE.FASHION_EQUIPPACK)
			{
				gameItem = container.GetItemByIndexId(gameitem.indexId);
			}
			if (gameItem != null)
			{
				gameItem.ItemId = gameitem.itemId;
				gameItem.ContainerType = container.ContainType;
				if (gameitem.HasParm)
				{
					gameItem.SetParm(gameitem.parm);
				}
				if (gameitem.HasBindflag)
				{
					gameItem.BindFlag = gameitem.bindflag;
				}
				else
				{
					gameItem.BindFlag = false;
				}
				if (gameitem.HasStack)
				{
					gameItem.StackNum = (int)gameitem.stack;
				}
				else
				{
					gameItem.StackNum = 1;
				}
				if (gameitem.HasIndexId)
				{
					gameItem.IndexId = gameitem.indexId;
				}
				else
				{
					gameItem.IndexId = -1L;
				}
				if (gameitem.HasQuality)
				{
					gameItem.Quality = (EQUIP_QUALITY)gameitem.quality;
				}
				else
				{
					gameItem.Quality = EQUIP_QUALITY.INVALID;
				}
				if (gameitem.HasLevel)
				{
					gameItem.ItemLevel = (int)gameitem.level;
				}
				else
				{
					gameItem.ItemLevel = 0;
				}
				if (gameitem.HasAppraise)
				{
					gameItem.Appraise = (int)gameitem.appraise;
				}
				else
				{
					gameItem.Appraise = 0;
				}
				if (gameitem.HasRandom_attri)
				{
					gameItem.Random_AttriDic = gameitem.random_attri;
				}
				else
				{
					gameItem.Random_AttriDic = null;
				}
				if (gameitem.HasInlay)
				{
					gameItem.InlayDic = gameitem.inlay;
				}
				else
				{
					gameItem.InlayDic = null;
				}
			}
		}
	}
}
