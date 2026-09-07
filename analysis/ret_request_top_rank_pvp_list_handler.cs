using Sproto;
using SprotoType;

public class ret_request_top_rank_pvp_list_handler
{
	public static SprotoTypeBase ret_request_top_rank_pvp_list_request(SprotoTypeBase req)
	{
		if (req is ret_request_top_rank_pvp_list.request)
		{
			WaitResponseUIRootLogic.CloseBox();
		}
		return null;
	}
}
