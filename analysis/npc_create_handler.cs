using Sproto;
using SprotoType;

public class npc_create_handler
{
	public static SprotoTypeBase npc_create_request(SprotoTypeBase req)
	{
		if (req is npc_create.request request)
		{
			ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(request.npc_attribute.id);
			if (objCharacter != null && objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
			{
				Singleton<ObjManager>.Instance.RecycleNpc(objCharacter as ObjNPC);
			}
			ObjInitNpcData objInitNpcData = new ObjInitNpcData();
			objInitNpcData.InitData(request.npc_attribute);
			Singleton<ObjManager>.Instance.CreateNPC(objInitNpcData);
		}
		return null;
	}
}
