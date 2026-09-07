using Sproto;
using SprotoType;

public class survive_battle_finish_handler
{
	public static SprotoTypeBase survive_battle_finish_request(SprotoTypeBase req)
	{
		survive_battle_finish.request request = req as survive_battle_finish.request;
		if (request != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SurviveBattleResultRoot, delegate
			{
				SingletonUnity<SurviveBattleResultRootLogic>.Instance.Reset(request);
			});
		}
		return null;
	}
}
