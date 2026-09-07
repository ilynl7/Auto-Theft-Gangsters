using Sproto;
using SprotoType;

public class ret_request_big_pack_handler
{
	public static SprotoTypeBase ret_request_big_pack_request(SprotoTypeBase req)
	{
		if (req is ret_request_big_pack.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.SetBigPack(request);
			if (SingletonUnity<BigPackRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<BigPackRootLogic>.Instance.gameObject))
			{
				SingletonUnity<BigPackRootLogic>.Instance.Reset(request);
			}
		}
		return null;
	}
}
