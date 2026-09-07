using Sproto;
using SprotoType;

public class ret_tower_wipe_out_handler
{
	public static SprotoTypeBase ret_tower_wipe_out_request(SprotoTypeBase req)
	{
		if (req is ret_tower_wipe_out.request request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TowerData.UpdateTowerData(request.tower_info);
			if (SingletonUnity<TowerUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TowerUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<TowerUIRootLogic>.Instance.UpdateTowerInfo(request.tower_info);
			}
			if (SingletonUnity<NewDailyCopyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyCopyUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewDailyCopyUIRootLogic>.Instance.ResetTowerInfo(request.tower_info);
			}
			if (SingletonUnity<WaitResponseUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<WaitResponseUIRootLogic>.Instance.gameObject) && SingletonUnity<TowerWipeOutRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TowerWipeOutRootLogic>.Instance.gameObject) && request.tower_info.wipe_out_state == 2)
			{
				WaitResponseUIRootLogic.CloseBox();
				SingletonUnity<TowerWipeOutRootLogic>.Instance.ResetWipeOutRewardPage((int)request.tower_info.cur_floor, (int)request.tower_info.floor);
			}
		}
		return null;
	}
}
