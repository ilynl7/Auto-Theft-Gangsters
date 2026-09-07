using Sproto;
using SprotoType;

public class sample_copy_result_handler
{
	public static SprotoTypeBase sample_copy_result_request(SprotoTypeBase req)
	{
		if (req is sample_copy_result.request request)
		{
			CountDownTimeLogic.CloseTime();
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CompleteMission();
			if (request.HasWin && !request.win)
			{
				LocalDataSaveManager.SetDiedFlag(1);
			}
		}
		return null;
	}
}
