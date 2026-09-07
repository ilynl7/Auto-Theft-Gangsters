using Sproto;
using SprotoType;

public class ret_request_dance_info_handler
{
	public static SprotoTypeBase ret_request_dance_info_request(SprotoTypeBase req)
	{
		if (req is ret_request_dance_info.request request)
		{
			PlayerDanceData playerDanceData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerDanceData;
			playerDanceData.SyncPlayerDanceInfo(request);
			if (request.type == 0L && SingletonUnity<WaitResponseUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<WaitResponseUIRootLogic>.Instance.gameObject))
			{
				WaitResponseUIRootLogic.CloseBox();
				if (SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DanceChooseRoot);
				}
			}
		}
		return null;
	}
}
