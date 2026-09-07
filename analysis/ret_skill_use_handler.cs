using Sproto;
using SprotoType;

public class ret_skill_use_handler
{
	public static SprotoTypeBase ret_skill_use_request(SprotoTypeBase req)
	{
		if (req is ret_skill_use.request request)
		{
			ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(request.sendderId);
			if (objCharacter != null)
			{
				if (objCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER && !(objCharacter as ObjOtherPlayer).IsVisible())
				{
					return null;
				}
				objCharacter.SkillLogic.ServerUseSkill(request.skillId.ToString(), request.sendderId, request.targetId, request.attack_list);
			}
		}
		return null;
	}
}
