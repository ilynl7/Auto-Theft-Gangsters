using Sproto;
using SprotoType;

public class notify_copy_start_info_handler
{
	public static SprotoTypeBase notify_copy_start_info_request(SprotoTypeBase req)
	{
		if (req is notify_copy_start_info.request { HasEnd_time: not false, end_time: var end_time } request)
		{
			long type = ((!request.HasType) ? 0 : request.type);
			if (SingletonUnity<CountDownTimeLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CountDownTimeLogic>.Instance.gameObject))
			{
				SingletonUnity<CountDownTimeLogic>.Instance.SetReamainTime(end_time, type);
			}
		}
		return null;
	}
}
