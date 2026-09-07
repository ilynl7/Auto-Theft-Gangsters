using Sproto;
using SprotoType;

public class guild_battle_finish_info_handler
{
	public static SprotoTypeBase guild_battle_finish_info_request(SprotoTypeBase req)
	{
		guild_battle_finish_info.request request = req as guild_battle_finish_info.request;
		if (request != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildBattleResultRoot, delegate
			{
				SingletonUnity<GuildBattleResultRootLogic>.Instance.RefreshInfo(request);
			});
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CompleteMission();
		}
		return null;
	}
}
