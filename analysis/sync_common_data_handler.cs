using Sproto;
using SprotoType;

public class sync_common_data_handler
{
	public static SprotoTypeBase sync_common_data_request(SprotoTypeBase req)
	{
		if (req is sync_common_data.request request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.SyncCommonData(request);
		}
		return null;
	}
}
