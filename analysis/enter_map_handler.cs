using Sproto;
using SprotoType;

public class enter_map_handler
{
	public static SprotoTypeBase enter_map_request(SprotoTypeBase req)
	{
		if (req is enter_map.request request)
		{
			NetLogic.GetInstance().CanProcessPack = false;
			int sceneDefine = int.Parse(request.mapInfoId);
			if (request.HasLine_count)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.LineCount = (int)request.line_count;
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CurLineIndex = (int)request.line_index;
			}
			else
			{
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.LineCount = 1;
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CurLineIndex = 1;
			}
			LoadingWindow.LoadScene(sceneDefine);
		}
		return null;
	}
}
