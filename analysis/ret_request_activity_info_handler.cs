using Sproto;
using SprotoType;

public class ret_request_activity_info_handler
{
	public static SprotoTypeBase ret_request_activity_info_request(SprotoTypeBase req)
	{
		if (req is ret_request_activity_info.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.ActivityData.SyncActivityInfoData(request);
			if (SingletonUnity<DailyActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DailyActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<DailyActivityUIRootLogic>.Instance.RefreshActivityPage(request);
			}
			if (SingletonUnity<NewDailyActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewDailyActivityUIRootLogic>.Instance.RefreshActivityPage();
			}
			instance.SceneManager.CheckSceneActivity();
		}
		return null;
	}
}
