using Sproto;
using SprotoType;

public class ret_top_rank_list_handler
{
	public static SprotoTypeBase ret_top_rank_list_request(SprotoTypeBase req)
	{
		if (req is ret_top_rank_list.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (SingletonUnity<PlayerRankInfoRootLogic>.Exists)
			{
				SingletonUnity<PlayerRankInfoRootLogic>.Instance.UpdateRankTypeList(request);
			}
		}
		return null;
	}
}
