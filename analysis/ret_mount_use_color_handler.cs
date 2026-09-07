using Sproto;
using SprotoType;

public class ret_mount_use_color_handler
{
	public static SprotoTypeBase ret_mount_use_color_request(SprotoTypeBase req)
	{
		if (req is ret_mount_use_color.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (SingletonUnity<PlayerCarRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PlayerCarRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PlayerCarRootLogic>.Instance.buyColorSuccess(request);
			}
			if (Singleton<ObjManager>.Instance.MainPlayer != null)
			{
				Singleton<ObjManager>.Instance.MainPlayer.ChangeMountColor(request.mountId, request.colorId);
			}
		}
		return null;
	}
}
