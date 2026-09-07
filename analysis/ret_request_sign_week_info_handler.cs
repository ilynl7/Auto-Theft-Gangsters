using Sproto;
using SprotoType;

public class ret_request_sign_week_info_handler
{
	public static SprotoTypeBase ret_request_sign_week_info_request(SprotoTypeBase req)
	{
		if (req is ret_request_sign_week_info.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.welfareData.SetWeekFlag(request);
			if (SingletonUnity<SignWeekRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SignWeekRootLogic>.Instance.gameObject))
			{
				SingletonUnity<SignWeekRootLogic>.Instance.Reset(request);
			}
		}
		return null;
	}
}
