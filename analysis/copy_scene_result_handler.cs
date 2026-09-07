using Sproto;
using SprotoType;

public class copy_scene_result_handler
{
	public static SprotoTypeBase copy_scene_result_request(SprotoTypeBase req)
	{
		copy_scene_result.request request = req as copy_scene_result.request;
		if (request != null)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (request.win)
			{
				if (request.HasSwipe)
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionPassShowRoot, delegate
					{
						SingletonUnity<MissionPassShowRootLogic>.Instance.ResetEquipSwipe(request.items);
						SimpleRewardRootLogic.AddRewards(request.items);
					});
					return null;
				}
				if (request.subType == 9)
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CopyMissionShowRoot, delegate
					{
						SingletonUnity<CopyMissionShowRootLogic>.Instance.ResetTowerCopy(request);
					});
					if (request.HasParm)
					{
						SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TowerData.UpdateTowerbest(request.parm);
						int num = (int)request.parm / 5 * 5;
						int num2 = (int)request.parm / 5 * 5 + 4;
						SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tower", $"stage{num}_{num2}", "success");
					}
				}
				else
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.StarRewardPageRoot, delegate(bool bSuccess, object param)
					{
						if (bSuccess && param is copy_scene_result.request request2)
						{
							SingletonUnity<StarRewardPageRoot>.Instance.ResetCopySceneReward(request2);
						}
					}, request);
				}
			}
			else if (request.subType == 9)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CopyFailShowRoot, delegate(bool bSuccess, object param)
				{
					if (bSuccess)
					{
						string id = param as string;
						SingletonUnity<CopyFailShowRootLogic>.Instance.ResetTowerCopy(id);
					}
				}, request.id);
				if (request.HasParm)
				{
					int num3 = (int)request.parm / 5 * 5;
					int num4 = (int)request.parm / 5 * 5 + 4;
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tower", $"stage{num3}_{num4}", "failure");
				}
			}
			else
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CopyFailShowRoot, delegate(bool bSuccess, object param)
				{
					if (bSuccess)
					{
						SingletonUnity<CopyFailShowRootLogic>.Instance.ResetNormalCopy();
					}
				});
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("DailyCopy", $"copy_{request.id}", "failure");
			}
			CountDownTimeLogic.CloseTime();
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CompleteMission();
		}
		return null;
	}

	private static void OnRewardPageShow(bool isSuccess, object param)
	{
		if (isSuccess && param is copy_scene_result.request request)
		{
			SingletonUnity<RewardPageRootLogic>.Instance.ResetCopySceneReward(request);
		}
	}
}
