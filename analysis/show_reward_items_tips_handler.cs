using Sproto;
using SprotoType;

public class show_reward_items_tips_handler
{
	public static SprotoTypeBase show_reward_items_tips_request(SprotoTypeBase req)
	{
		if (req is show_reward_items_tips.request request)
		{
			SimpleRewardRootLogic.AddRewards(request.items);
		}
		return null;
	}
}
