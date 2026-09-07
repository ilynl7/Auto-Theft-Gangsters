using Sproto;
using SprotoType;

public class aoi_remove_handler
{
	public static SprotoTypeBase aoi_remove_request(SprotoTypeBase req)
	{
		if (req is aoi_remove.request request)
		{
			ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(request.character);
			if (objCharacter != null)
			{
				if (objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
				{
					Singleton<ObjManager>.Instance.RecycleOtherPlayer(objCharacter as ObjOtherPlayer);
				}
				else if (objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
				{
					if (objCharacter.AttributeData.Camp == GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC)
					{
						CurMission escortMission = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetEscortMission();
						if (escortMission != null && escortMission.GetParam(1) == objCharacter.ServerId)
						{
							escortMission.SetParam(4, (long)(objCharacter.Position.x * 100f));
							escortMission.SetParam(5, (long)(objCharacter.Position.z * 100f));
						}
					}
					Singleton<ObjManager>.Instance.RecycleNpc(objCharacter as ObjNPC);
				}
			}
			else
			{
				Singleton<ObjManager>.Instance.RemoveNoLogicOtherPlayerData(request.character);
			}
		}
		return null;
	}
}
