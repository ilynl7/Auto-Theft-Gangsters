using Sproto;
using SprotoType;

public class aoi_social_dance_handler
{
	public static SprotoTypeBase aoi_social_dance_request(SprotoTypeBase req)
	{
		if (req is aoi_social_dance.request { id: var id, danceId: var danceId })
		{
			ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(id);
			if (objCharacter != null && objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
			{
				ObjOtherPlayer objOtherPlayer = objCharacter as ObjOtherPlayer;
				if (objOtherPlayer != null && objOtherPlayer.IsVisible())
				{
					objOtherPlayer.PlayeSocialDance(danceId);
				}
			}
		}
		return null;
	}
}
