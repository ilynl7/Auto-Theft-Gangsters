using Sproto;
using SprotoType;

public class syn_rank_pvp_data_handler
{
	public static SprotoTypeBase syn_rank_pvp_data_request(SprotoTypeBase req)
	{
		syn_rank_pvp_data.request request = req as syn_rank_pvp_data.request;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (request != null)
		{
			playerData.RankPVPData.UpdateRankPvPData(request);
			if (SingletonUnity<RankPVPUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RankPVPUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<RankPVPUIRootLogic>.Instance.ResetPlayerInfo();
			}
		}
		return null;
	}
}
