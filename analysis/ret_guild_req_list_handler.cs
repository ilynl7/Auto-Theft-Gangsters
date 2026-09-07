using Sproto;
using SprotoType;

public class ret_guild_req_list_handler
{
	public static SprotoTypeBase ret_guild_req_list_request(SprotoTypeBase req)
	{
		if (req is ret_guild_req_list.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			if (request.HasApplyGuildId && mainPlayer != null)
			{
				mainPlayer.ApplyGuildIDList = request.applyGuildId;
			}
			else if (mainPlayer != null)
			{
				mainPlayer.ApplyGuildIDList.Clear();
			}
			if (request.HasLeave_time)
			{
				mainPlayer.LeaveGuildTime = request.leave_time;
			}
			if (SingletonUnity<NewGuildUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewGuildUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewGuildUIRootLogic>.Instance.ShowHasNoGuildInfo(request);
			}
		}
		return null;
	}
}
