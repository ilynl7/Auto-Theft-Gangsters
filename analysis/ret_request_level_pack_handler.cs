using Sproto;
using SprotoType;

public class ret_request_level_pack_handler
{
	public static SprotoTypeBase ret_request_level_pack_request(SprotoTypeBase req)
	{
		if (req is ret_request_level_pack.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.InitLevelPack(request);
			if (SingletonUnity<LevelPackRewardRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<LevelPackRewardRootLogic>.Instance.gameObject))
			{
				SingletonUnity<LevelPackRewardRootLogic>.Instance.Reset(request);
			}
		}
		return null;
	}
}
