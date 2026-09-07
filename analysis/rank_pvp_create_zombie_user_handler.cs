using Sproto;
using SprotoType;

public class rank_pvp_create_zombie_user_handler
{
	public static SprotoTypeBase rank_pvp_create_zombie_user_request(SprotoTypeBase req)
	{
		if (req is rank_pvp_create_zombie_user.request request)
		{
			ObjInitPlayerData objInitPlayerData = new ObjInitPlayerData();
			objInitPlayerData.InitData(request.character);
			Singleton<ObjManager>.Instance.CreateZombiePlayer(objInitPlayerData);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PVPBeforeStartRoot, OnPVPBeforeStartShow, request.character);
		}
		return null;
	}

	private static void OnPVPBeforeStartShow(bool isSuccess, object param)
	{
		if (isSuccess)
		{
			character character = param as character;
			SingletonUnity<PVPBeforeStartRootLogic>.Instance.ResetRankPVPPage(character);
		}
	}
}
