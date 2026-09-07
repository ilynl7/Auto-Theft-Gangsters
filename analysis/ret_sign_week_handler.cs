using Sproto;
using SprotoType;

public class ret_sign_week_handler
{
	public static SprotoTypeBase ret_sign_week_request(SprotoTypeBase req)
	{
		if (req is ret_sign_week.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.welfareData.SetWeekFlag(request);
			if (SingletonUnity<SignWeekRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SignWeekRootLogic>.Instance.gameObject))
			{
				SingletonUnity<SignWeekRootLogic>.Instance.UpdateInfo(request);
			}
		}
		return null;
	}
}
