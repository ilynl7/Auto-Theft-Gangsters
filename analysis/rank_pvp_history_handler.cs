using Sproto;
using SprotoType;

public class rank_pvp_history_handler
{
	public static SprotoTypeBase rank_pvp_history_request(SprotoTypeBase req)
	{
		if (req is rank_pvp_history.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (request.HasLogs)
			{
				if (SingletonUnity<PVPLogUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PVPLogUIRootLogic>.Instance.gameObject))
				{
					SingletonUnity<PVPLogUIRootLogic>.Instance.UpdataLogList(request.logs);
				}
			}
			else if (SingletonUnity<PVPLogUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PVPLogUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PVPLogUIRootLogic>.Instance.UpdataLogList(null);
			}
		}
		return null;
	}
}
