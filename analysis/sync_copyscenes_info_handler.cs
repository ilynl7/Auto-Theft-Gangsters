using Sproto;
using SprotoType;

public class sync_copyscenes_info_handler
{
	public static SprotoTypeBase sync_copyscenes_info_request(SprotoTypeBase req)
	{
		if (req is sync_copyscenes_info.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.SyncCopyInfo(request.copyscenes);
			if (SingletonUnity<CreateTeamRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CreateTeamRootLogic>.Instance.gameObject))
			{
				SingletonUnity<CreateTeamRootLogic>.Instance.RefreshPage();
			}
			else if (SingletonUnity<SearchTeamRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SearchTeamRootLogic>.Instance.gameObject))
			{
				SingletonUnity<SearchTeamRootLogic>.Instance.Init();
			}
			else if (SingletonUnity<NewDailyCopyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewDailyCopyUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewDailyCopyUIRootLogic>.Instance.RefreshCopyPage();
			}
		}
		return null;
	}
}
