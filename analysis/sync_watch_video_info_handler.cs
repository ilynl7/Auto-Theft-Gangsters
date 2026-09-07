using Sproto;
using SprotoType;

public class sync_watch_video_info_handler
{
	public static SprotoTypeBase sync_watch_video_info_request(SprotoTypeBase req)
	{
		if (req is sync_watch_video_info.request request)
		{
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
		}
		return null;
	}
}
