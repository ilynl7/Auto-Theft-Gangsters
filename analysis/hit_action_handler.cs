using Sproto;
using SprotoType;

public class hit_action_handler
{
	public static SprotoTypeBase hit_action_request(SprotoTypeBase req)
	{
		if (req is hit_action.request request)
		{
			ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(request.targetid);
			ObjCharacter objCharacter2 = Singleton<ObjManager>.Instance.FindObjInScene(request.senderId);
			if (objCharacter != null && objCharacter2 != null)
			{
				EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(request.effinfoId);
			}
		}
		return null;
	}
}
