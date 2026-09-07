using Sproto;
using SprotoType;

public class ret_request_survive_top_handler
{
	public static SprotoTypeBase ret_request_survive_top_request(SprotoTypeBase req)
	{
		if (req is ret_request_survive_top.request request)
		{
			if (!(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager is SurvivalBattleSceneManager survivalBattleSceneManager))
			{
				return null;
			}
			if (request.HasScore_infos && SingletonUnity<MultiRankSmallRootLogic>.Exists)
			{
				SingletonUnity<MultiRankSmallRootLogic>.Instance.Reset(request);
			}
			if (request.HasEnd_time && SingletonUnity<CountDownTimeLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CountDownTimeLogic>.Instance.gameObject))
			{
				SingletonUnity<CountDownTimeLogic>.Instance.SetReamainTime(request.end_time);
			}
			if (request.HasMy_score)
			{
				survivalBattleSceneManager?.UpdatePlayerScore(request.my_score);
			}
		}
		return null;
	}
}
