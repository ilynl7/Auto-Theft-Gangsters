using Sproto;
using SprotoType;

public class tiantti_result_handler
{
	public static SprotoTypeBase tiantti_result_request(SprotoTypeBase req)
	{
		tiantti_result.request request = req as tiantti_result.request;
		if (request != null)
		{
			if (request.win)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CopyMissionShowRoot, delegate
				{
					SingletonUnity<CopyMissionShowRootLogic>.Instance.ResetRankPvP(request);
				});
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.RankPVPData.UpdateRankPvpInfo(request);
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("PVP", "PVP", "success");
			}
			else
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CopyFailShowRoot, delegate
				{
					SingletonUnity<CopyFailShowRootLogic>.Instance.ResetRankPvp(request.items);
				});
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("PVP", "PVP", "failure");
			}
			CountDownTimeLogic.CloseTime();
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CompleteMission();
		}
		return null;
	}
}
