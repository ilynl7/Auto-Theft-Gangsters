using Sproto;
using SprotoType;

public class ret_level_reward_handler
{
	public static SprotoTypeBase ret_level_reward_request(SprotoTypeBase req)
	{
		if (req is ret_level_reward.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.SyncLevelReward(request);
			if (SingletonUnity<LevelRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<LevelRewardRootLogic>.Instance.gameObject))
			{
				SingletonUnity<LevelRewardRootLogic>.Instance.Reset(request);
			}
		}
		return null;
	}
}
