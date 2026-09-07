using Sproto;
using SprotoType;

public class grant_daily_mission_reward_handler
{
	public static SprotoTypeBase grant_daily_mission_reward_request(SprotoTypeBase req)
	{
		grant_daily_mission_reward.request request = req as grant_daily_mission_reward.request;
		if (request != null)
		{
			if (request.HasItems2 && request.HasItems)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionPassShowRoot, delegate
				{
					SingletonUnity<MissionPassShowRootLogic>.Instance.ResetDailyMissionReward(request.items, delegate
					{
						SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AutoDailyMissionCheck();
						vp_Timer.In(0.5f, delegate
						{
							SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionPassShowRoot, delegate
							{
								SingletonUnity<MissionPassShowRootLogic>.Instance.ResetDailyFinishReward(request.items2);
							});
						});
					});
				});
			}
			else if (request.HasItems)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionPassShowRoot, delegate
				{
					SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AutoDailyMissionCheck();
					SingletonUnity<MissionPassShowRootLogic>.Instance.ResetDailyMissionReward(request.items);
				});
			}
			else if (request.HasItems2)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionPassShowRoot, delegate
				{
					SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AutoDailyMissionCheck();
					SingletonUnity<MissionPassShowRootLogic>.Instance.ResetDailyFinishReward(request.items2);
				});
			}
			else
			{
				SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AutoDailyMissionCheck();
			}
		}
		return null;
	}
}
