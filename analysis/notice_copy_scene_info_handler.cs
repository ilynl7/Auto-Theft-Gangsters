using Sproto;
using SprotoType;

public class notice_copy_scene_info_handler
{
	public static SprotoTypeBase notice_copy_scene_info_request(SprotoTypeBase req)
	{
		notice_copy_scene_info.request request = req as notice_copy_scene_info.request;
		if (request != null)
		{
			if (!(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager is EXPSceneManager))
			{
				return null;
			}
			if (request.HasId)
			{
				if (SingletonUnity<ExpBattleInfoRootLogic>.Exists)
				{
					SingletonUnity<ExpBattleInfoRootLogic>.Instance.UpdateInfo(request);
				}
				else
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExpBattleInfoRoot, delegate
					{
						SingletonUnity<ExpBattleInfoRootLogic>.Instance.EnableReset();
						SingletonUnity<ExpBattleInfoRootLogic>.Instance.UpdateInfo(request);
					});
				}
			}
		}
		return null;
	}
}
