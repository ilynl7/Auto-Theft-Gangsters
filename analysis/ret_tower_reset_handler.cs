using Sproto;
using SprotoType;

public class ret_tower_reset_handler
{
	public static SprotoTypeBase ret_tower_reset_request(SprotoTypeBase req)
	{
		if (req is ret_tower_reset.request { state: not false })
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TowerData.ResetTowerData();
			if (SingletonUnity<TowerUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TowerUIRootLogic>.Instance.gameObject))
			{
				tower_info playerTowerInfo = SingletonUnity<TowerUIRootLogic>.Instance.PlayerTowerInfo;
				playerTowerInfo.times = 0L;
				playerTowerInfo.cur_floor = 0L;
				SingletonUnity<TowerUIRootLogic>.Instance.UpdateTowerInfo(playerTowerInfo);
			}
			if (SingletonUnity<NewDailyCopyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyCopyUIRootLogic>.Instance.gameObject))
			{
				tower_info mPlayerTowerInfo = SingletonUnity<NewDailyCopyUIRootLogic>.Instance.mPlayerTowerInfo;
				mPlayerTowerInfo.times = 0L;
				mPlayerTowerInfo.cur_floor = 0L;
				SingletonUnity<NewDailyCopyUIRootLogic>.Instance.ResetTowerInfo(mPlayerTowerInfo);
			}
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tower", "reset", "resettimes");
		}
		return null;
	}
}
