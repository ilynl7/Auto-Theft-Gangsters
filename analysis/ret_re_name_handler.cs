using Sproto;
using SprotoType;

public class ret_re_name_handler
{
	public static SprotoTypeBase ret_re_name_request(SprotoTypeBase req)
	{
		if (req is ret_re_name.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (request.HasState && request.state == 1 && request.HasName)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.ReName(request.name);
				Singleton<ObjManager>.Instance.MainPlayer.reName(request.name);
			}
		}
		return null;
	}
}
