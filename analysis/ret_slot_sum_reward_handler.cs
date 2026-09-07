using Sproto;
using SprotoType;

public class ret_slot_sum_reward_handler
{
	public static SprotoTypeBase ret_slot_sum_reward_request(SprotoTypeBase req)
	{
		if (req is ret_slot_sum_reward.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (request.HasItems && SingletonUnity<SlotUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SlotUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<SlotUIRootLogic>.Instance.ShowSumReward(request.items, request.sumNum);
			}
		}
		return null;
	}
}
