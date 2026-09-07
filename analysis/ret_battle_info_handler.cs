using Sproto;
using SprotoType;

public class ret_battle_info_handler
{
	public static SprotoTypeBase ret_battle_info_request(SprotoTypeBase req)
	{
		if (req is ret_battle_info.request request)
		{
			SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			if (sceneManager.IsBigWorld())
			{
				return null;
			}
			if (request.battle_info.HasDamage_list && request.battle_info.damage_list.Count > 0 && SingletonUnity<MultiRankSmallRootLogic>.Exists)
			{
				if (request.type == 4)
				{
					SingletonUnity<MultiRankSmallRootLogic>.Instance.Reset(request.battle_info, GameDefine.ACTIVITY_TYPE.BAR_FIGHT);
				}
				else
				{
					SingletonUnity<MultiRankSmallRootLogic>.Instance.Reset(request.battle_info);
				}
			}
			if (request.battle_info.HasEnd_time && SingletonUnity<CountDownTimeLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CountDownTimeLogic>.Instance.gameObject))
			{
				SingletonUnity<CountDownTimeLogic>.Instance.SetReamainTime(request.battle_info.end_time);
			}
		}
		return null;
	}
}
