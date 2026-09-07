using Sproto;
using SprotoType;

public class sync_dance_state_info_handler
{
	public static SprotoTypeBase sync_dance_state_info_request(SprotoTypeBase req)
	{
		if (req is sync_dance_state_info.request request)
		{
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.ActivityData.SyncDanceStateInfo(request);
			if (SingletonUnity<MissionTeamTipLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MissionTeamTipLogic>.Instance.gameObject))
			{
				SingletonUnity<MissionTeamTipLogic>.Instance.UpdateDanceInfo();
			}
			if (SingletonUnity<DanceBtnRootLogic>.Exists)
			{
				SingletonUnity<DanceBtnRootLogic>.Instance.UpdateCDTime();
			}
		}
		return null;
	}
}
