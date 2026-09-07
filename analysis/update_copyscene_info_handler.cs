using Sproto;
using SprotoType;

public class update_copyscene_info_handler
{
	public static SprotoTypeBase update_copyscene_info_request(SprotoTypeBase req)
	{
		if (req is update_copyscene_info.request request)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.SyncCopyInfo(request.copyscene);
		}
		return null;
	}
}
