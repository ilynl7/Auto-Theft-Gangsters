using Sproto;
using SprotoType;

public class ret_sign_30_day_handler
{
	public static SprotoTypeBase ret_sign_30_day_request(SprotoTypeBase req)
	{
		if (req is ret_sign_30_day.request request)
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
