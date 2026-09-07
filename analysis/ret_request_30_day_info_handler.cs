using Sproto;
using SprotoType;

public class ret_request_30_day_info_handler
{
	public static SprotoTypeBase ret_request_30_day_info_request(SprotoTypeBase req)
	{
		if (req is ret_request_30_day_info.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.welfareData.SetMonthFlag(request);
			if (SingletonUnity<SignMonthRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SignMonthRootLogic>.Instance.gameObject))
			{
				SingletonUnity<SignMonthRootLogic>.Instance.UpdateInfo(request);
			}
		}
		return null;
	}
}
