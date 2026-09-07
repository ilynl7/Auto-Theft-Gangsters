using Sproto;
using SprotoType;

public class ret_request_random_rank_pvp_opponent_handler
{
	public static SprotoTypeBase ret_request_random_rank_pvp_opponent_request(SprotoTypeBase req)
	{
		if (req is ret_request_random_rank_pvp_opponent.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			int num = (int)request.opponentNum;
			if (num > 0 && SingletonUnity<RankPVPUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RankPVPUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<RankPVPUIRootLogic>.Instance.ResetOtherPlayer(request);
			}
		}
		return null;
	}
}
