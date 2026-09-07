using Sproto;
using SprotoType;

public class sample_activity_result_handler
{
	public static SprotoTypeBase sample_activity_result_request(SprotoTypeBase req)
	{
		if (req is sample_activity_result.request request)
		{
			SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			if (sceneManager != null && sceneManager.CurrentMapInofData != null && sceneManager.CurrentMapInofData.MapType == MAPTYPE.DOMIN_MAP)
			{
				if (request.win)
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.YouWinShowRoot);
					PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
					if (playerData.Domin_InfoDic.ContainsKey(request.id))
					{
						playerData.Domin_InfoDic[request.id].state = 1L;
					}
				}
				else
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.YouLostShowRoot);
				}
			}
		}
		return null;
	}
}
