using Sproto;
using SprotoType;

public class car_copy_result_handler
{
	public static SprotoTypeBase car_copy_result_request(SprotoTypeBase req)
	{
		car_copy_result.request request = req as car_copy_result.request;
		if (request != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CarRewardPageRootLogic, delegate
			{
				SingletonUnity<CarRewardPageRootLogic>.Instance.ResetCarRewardPageRoot(request);
			});
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CompleteMission();
		}
		return null;
	}
}
