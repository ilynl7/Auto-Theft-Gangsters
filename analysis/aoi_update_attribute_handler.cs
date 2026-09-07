using Sproto;
using SprotoType;
using UnityEngine;

public class aoi_update_attribute_handler
{
	public static SprotoTypeBase aoi_update_attribute_request(SprotoTypeBase req)
	{
		if (req is aoi_update_attribute.request request)
		{
			long id = request.character.id;
			ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(id);
			if (objCharacter != null)
			{
				if (objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
				{
					if (request.character.HasAttribute)
					{
						objCharacter.AttributeData.MaxHP = request.character.attribute.max_hp;
					}
					if (request.character.HasAttribute_other)
					{
						long hp = request.character.attribute_other.hp;
						objCharacter.ChangeHPVal(hp);
						long guildId = ((!request.character.attribute_other.HasGuildId) ? (-1) : request.character.attribute_other.guildId);
						long teamId = ((!request.character.attribute_other.HasGuildJob) ? (-1) : request.character.attribute_other.guildJob);
						ObjNPC objNPC = objCharacter as ObjNPC;
						objNPC.UpdateEscortNpcCamp(guildId, teamId);
					}
					return null;
				}
				if (request.character.HasVisual)
				{
					SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
					if (objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
					{
						ObjOtherPlayer objOtherPlayer = objCharacter as ObjOtherPlayer;
						if (objOtherPlayer != null)
						{
							characterVisual visual = request.character.visual;
							objOtherPlayer.IsServerRidingMount = visual.mount_state == 1;
							objOtherPlayer.ReLoadPlayerVisual(visual);
							if (visual.mount_state == 0L)
							{
								objOtherPlayer.MountId = visual.MountId;
								objOtherPlayer.MountColor = visual.mount_color;
								if (sceneManager.IsBigWorld())
								{
									objOtherPlayer.DisMountCar();
								}
							}
							else if (sceneManager.IsBigWorld())
							{
								objOtherPlayer.MountCar(visual.MountId, visual.mount_color);
							}
						}
					}
					else if (objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
					{
						ObjMainPlayer objMainPlayer = objCharacter as ObjMainPlayer;
						if (objMainPlayer != null)
						{
							characterVisual visual2 = request.character.visual;
							objMainPlayer.IsServerRidingMount = visual2.mount_state == 1;
							objMainPlayer.UpdateMainPlayerVisual(visual2);
							objMainPlayer.ReLoadPlayerVisual(visual2);
							objMainPlayer.MountId = visual2.MountId;
							objMainPlayer.MountColor = visual2.mount_color;
						}
					}
				}
				if (request.character.HasAttribute || request.character.HasAttribute_other || request.character.HasAttribute_all)
				{
					CharacterAttributeData attributeData = objCharacter.AttributeData;
					if (request.character.HasAttribute && request.character.attribute.HasMax_hp)
					{
						objCharacter.AttributeData.MaxHP = request.character.attribute.max_hp;
						objCharacter.ChangeHPVal(attributeData.HP);
					}
					if (request.character.HasAttribute_other)
					{
						long hp2 = request.character.attribute_other.hp;
						objCharacter.ChangeHPVal(hp2);
						objCharacter.ChangeLevel((int)request.character.attribute_other.level, (int)request.character.attribute_other.combValue);
					}
					attributeData.InitData(request.character, objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER);
					objCharacter.UpdatePlayerSpeed();
					if (objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
					{
						SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SetPKModeState(attributeData.PkMode);
					}
					if (request.character.HasAttribute_other)
					{
						objCharacter.RefreshHeadInfo();
						if (objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
						{
							ExpLineRootLogic.UpdateExp();
							PlayerModelPageRootLogic.UpdateCombo(attributeData.ComboValue);
							if (request.character.attribute_other.dance_state == 0L)
							{
								ObjMainPlayer objMainPlayer2 = objCharacter as ObjMainPlayer;
								objMainPlayer2.StopDance();
							}
						}
						else if (objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
						{
							if (request.character.attribute_other.dance_state == 1)
							{
								ObjOtherPlayer objOtherPlayer2 = objCharacter as ObjOtherPlayer;
								objOtherPlayer2.StartDance(request.character.attribute_other.dance_id);
							}
							else
							{
								ObjOtherPlayer objOtherPlayer3 = objCharacter as ObjOtherPlayer;
								objOtherPlayer3.StopDance();
							}
						}
					}
					if (SingletonUnity<JSSXKuangRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<JSSXKuangRootLogic>.Instance.gameObject))
					{
						SingletonUnity<JSSXKuangRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData);
					}
				}
				if (request.character.HasProperty && objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
				{
					GameMoneyHelper.UpdateMoney(request.character.property.money1, request.character.property.money2, request.character.property.money3, request.character.property.money4, request.character.property.money5, request.character.property.money6);
					if (UIUpdateEvent.UpdateMoneyEvent != null)
					{
						UIUpdateEvent.UpdateMoneyEvent();
					}
				}
			}
			else
			{
				ObjInitPlayerData noLogicOtherPlayerData = Singleton<ObjManager>.Instance.GetNoLogicOtherPlayerData(id);
				if (noLogicOtherPlayerData != null)
				{
					noLogicOtherPlayerData.InitData(request.character);
				}
				else
				{
					Debug.Log("No PlayerData In Scene!!!!!!!!!!!!!!!");
				}
			}
		}
		return null;
	}
}
