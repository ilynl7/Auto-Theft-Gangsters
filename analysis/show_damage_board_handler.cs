using System.Collections.Generic;
using Sproto;
using SprotoType;

public class show_damage_board_handler
{
	public static SprotoTypeBase show_damage_board_request(SprotoTypeBase req)
	{
		if (req is show_damage_board.request request)
		{
			if (!request.HasDamges)
			{
				return null;
			}
			List<acceptdamge> damges = request.damges;
			for (int i = 0; i < damges.Count; i++)
			{
				long id = damges[i].id;
				ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(id);
				if (!(objCharacter != null))
				{
					continue;
				}
				if (damges[i].damage > 0)
				{
					if (damges[i].cri)
					{
						objCharacter.UpdateDamgeBoard(GameDefine.DAMAGEBOARD_TYPE.TARGET_ATTACK_CRITICAL, (int)damges[i].damage);
					}
					else
					{
						objCharacter.UpdateDamgeBoard(GameDefine.DAMAGEBOARD_TYPE.PLAYER_HP_DOWN, (int)damges[i].damage);
					}
				}
				else
				{
					objCharacter.UpdateDamgeBoard(GameDefine.DAMAGEBOARD_TYPE.TARGET_ATTACK_MISS, (int)damges[i].damage);
				}
			}
		}
		return null;
	}
}
