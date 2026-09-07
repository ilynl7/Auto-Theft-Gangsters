using Sproto;
using SprotoType;

public class update_line_state_handler
{
	public static SprotoTypeBase update_line_state_request(SprotoTypeBase req)
	{
		if (req is update_line_state.request request && SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr == request.mapInfoId)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			playerData.LineCount = (int)request.line_count;
			if (request.HasLine_states)
			{
				playerData.LineStates = request.line_states;
			}
			if (SingletonUnity<MapLineInfoLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MapLineInfoLogic>.Instance.gameObject))
			{
				SingletonUnity<MapLineInfoLogic>.Instance.UpdateItems();
			}
		}
		return null;
	}
}
