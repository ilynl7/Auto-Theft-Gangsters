using Sproto;
using SprotoType;

public class notice_money_copy_reward_handler
{
	public static SprotoTypeBase notice_money_copy_reward_request(SprotoTypeBase req)
	{
		if (req is notice_money_copy_reward.request request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurSceneReward = (int)request.items[0].itemCount;
		}
		return null;
	}
}
