using Sproto;
using SprotoType;

public class aoi_add_handler
{
	public static SprotoTypeBase aoi_add_request(SprotoTypeBase req)
	{
		if (req is aoi_add.request request)
		{
			ObjInitPlayerData objInitPlayerData = new ObjInitPlayerData();
			objInitPlayerData.InitData(request.character);
			ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(objInitPlayerData.mServerID);
			if (objCharacter != null)
			{
				if (objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
				{
					Singleton<ObjManager>.Instance.RecycleOtherPlayer(objCharacter as ObjOtherPlayer);
				}
			}
			else if (Singleton<ObjManager>.Instance.GetNoLogicOtherPlayerData(objInitPlayerData.mServerID) != null)
			{
				Singleton<ObjManager>.Instance.RemoveNoLogicOtherPlayerData(objInitPlayerData.mServerID);
			}
			Singleton<ObjManager>.Instance.CreateOtherPlayer(objInitPlayerData);
		}
		return null;
	}
}
