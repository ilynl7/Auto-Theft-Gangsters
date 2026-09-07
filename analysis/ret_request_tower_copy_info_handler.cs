using Sproto;
using SprotoType;

public class ret_request_tower_copy_info_handler
{
	public static SprotoTypeBase ret_request_tower_copy_info_request(SprotoTypeBase req)
	{
		ret_request_tower_copy_info.request request = req as ret_request_tower_copy_info.request;
		WaitResponseUIRootLogic.CloseBox();
		if (request != null)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TowerData.UpdateTowerData(request);
			if (SingletonUnity<TowerUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TowerUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<TowerUIRootLogic>.Instance.UpdateTowerCopyInfo(request);
			}
			if (SingletonUnity<NewDailyCopyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyCopyUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewDailyCopyUIRootLogic>.Instance.ResetTowerInfo(request);
			}
		}
		return null;
	}
}
