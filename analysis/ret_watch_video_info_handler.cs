using Sproto;
using SprotoType;

public class ret_watch_video_info_handler
{
	public static SprotoTypeBase ret_watch_video_info_request(SprotoTypeBase req)
	{
		if (req is ret_watch_video_info.request request)
		{
			if (request.HasState && request.state == 1)
			{
				NoticeLogic.AddNotifyData("Video rewards have been released!");
			}
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			if (request.HasCur_times)
			{
				instance.PlayerData.SetVideoTimes(request.cur_times);
			}
			if (request.HasMax_times)
			{
				instance.PlayerData.SetVideoMaxTimes(request.max_times);
			}
			if (request.HasEvery_time)
			{
				instance.PlayerData.SetVideoDiamond(request.every_time);
			}
			if (SingletonDontDestoryUnity<GameManager>.Instance.OnUnityAdsStateChange != null)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.OnUnityAdsStateChange(SingletonDontDestoryUnity<GameManager>.Instance.IsUnityAdsReady);
			}
		}
		return null;
	}
}
