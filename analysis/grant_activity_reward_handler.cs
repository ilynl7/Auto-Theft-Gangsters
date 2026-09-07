using Sproto;
using SprotoType;
using UnityEngine;

public class grant_activity_reward_handler
{
	public static SprotoTypeBase grant_activity_reward_request(SprotoTypeBase req)
	{
		grant_activity_reward.request request = req as grant_activity_reward.request;
		if (request != null)
		{
			if (request.type == 1)
			{
				if (request.win)
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionPassShowRoot, delegate
					{
						SingletonUnity<MissionPassShowRootLogic>.Instance.ResetNormalMissionReward(request.win, request.items);
						SimpleRewardRootLogic.AddRewards(request.items);
					});
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", $"activity_{request.type}", "success");
				}
				else
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CopyFailShowRoot, delegate
					{
						SingletonUnity<CopyFailShowRootLogic>.Instance.ResetMission(request.items);
						SimpleRewardRootLogic.AddRewards(request.items);
					});
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", $"activity_{request.type}", "failure");
				}
				MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
				CurMission escortMission = missionManager.GetEscortMission();
				escortMission.SetMissionState(MISSION_STATE.COMPLETE);
				missionManager.CompleteMissionSuccess(escortMission.MissionId);
			}
			else if (request.type == 2)
			{
				if (request.win)
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionPassShowRoot, delegate
					{
						SingletonUnity<MissionPassShowRootLogic>.Instance.ResetNormalMissionReward(request.win, request.items);
						SimpleRewardRootLogic.AddRewards(request.items);
					});
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", $"activity_{request.type}", "success");
				}
				else
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CopyFailShowRoot, delegate
					{
						SingletonUnity<CopyFailShowRootLogic>.Instance.ResetMission(request.items);
						SimpleRewardRootLogic.AddRewards(request.items);
					});
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", $"activity_{request.type}", "failure");
				}
				MissionManager missionManager2 = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
				CurMission curMissionByClassType = missionManager2.GetCurMissionByClassType(MISSION_CLASS_TYPE.ROBBERY);
				curMissionByClassType.SetMissionState(MISSION_STATE.COMPLETE);
				missionManager2.CompleteMissionSuccess(curMissionByClassType.MissionId);
			}
			else if (request.type == 4 || request.type == 5 || request.type == 6)
			{
				if (request.battle_info.HasDamage_list && request.battle_info.damage_list.Count > 0)
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MultiCopyRankResultRoot, delegate
					{
						SingletonUnity<MultiCopyRankResultRootLogic>.Instance.Reset(request.win, request.battle_info, request.items, (int)request.type, request.ID);
						if (request.HasItems)
						{
							SimpleRewardRootLogic.AddRewards(request.items);
						}
					});
					if (request.win)
					{
						SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", $"activity_{request.type}", "success");
					}
					else
					{
						SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", $"activity_{request.type}", "failure");
					}
				}
				else
				{
					Debug.Log("don't have damage list!");
				}
			}
			else if (request.type == 3)
			{
				SimpleRewardRootLogic.AddRewards(request.items);
			}
		}
		return null;
	}
}
