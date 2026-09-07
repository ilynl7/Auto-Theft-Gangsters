using Sproto;
using SprotoType;

public class ret_grant_tower_reward_handler
{
	public static SprotoTypeBase ret_grant_tower_reward_request(SprotoTypeBase req)
	{
		if (req is ret_grant_tower_reward.request request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TowerData.UpdateTowerData(request);
			if (SingletonUnity<TowerUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TowerUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<TowerUIRootLogic>.Instance.UpdateTowerInfo(request);
			}
			if (SingletonUnity<NewDailyCopyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyCopyUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewDailyCopyUIRootLogic>.Instance.ResetTowerInfo(request);
			}
		}
		return null;
	}
}
