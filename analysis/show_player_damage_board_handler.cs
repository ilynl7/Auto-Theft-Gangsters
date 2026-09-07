using Sproto;
using SprotoType;

public class show_player_damage_board_handler
{
	public static SprotoTypeBase show_player_damage_board_request(SprotoTypeBase req)
	{
		if (req is show_player_damage_board.request request)
		{
			ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(request.id);
			if (objCharacter != null)
			{
				objCharacter.UpdateDamgeBoard(GameDefine.DAMAGEBOARD_TYPE.PLAYER_HP_UP, request.hp);
			}
		}
		return null;
	}
}
