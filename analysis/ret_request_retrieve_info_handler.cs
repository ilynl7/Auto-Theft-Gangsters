using Sproto;
using SprotoType;

public class ret_request_retrieve_info_handler
{
	public static SprotoTypeBase ret_request_retrieve_info_request(SprotoTypeBase req)
	{
		if (req is ret_request_retrieve_info.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.SetRetrieve(request);
			if (SingletonUnity<RetrieveRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RetrieveRootLogic>.Instance.gameObject))
			{
				SingletonUnity<RetrieveRootLogic>.Instance.Reset(request);
			}
		}
		return null;
	}
}
