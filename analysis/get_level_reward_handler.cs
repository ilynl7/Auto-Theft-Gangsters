using Sproto;
using SprotoType;

public class get_level_reward_handler
{
	public static SprotoTypeBase get_level_reward_request(SprotoTypeBase req)
	{
		if (req is get_level_reward.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.SyncLevelReward(request);
			if (SingletonUnity<LevelRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<LevelRewardRootLogic>.Instance.gameObject))
			{
				SingletonUnity<LevelRewardRootLogic>.Instance.UpdateInfo(request);
			}
			if (request.HasItems && request.items.Count > 0)
			{
				SimpleRewardRootLogic.AddRewards(request.items);
			}
		}
		return null;
	}
}
