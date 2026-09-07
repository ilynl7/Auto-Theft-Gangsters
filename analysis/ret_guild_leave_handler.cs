using Sproto;
using SprotoType;

public class ret_guild_leave_handler
{
	public static SprotoTypeBase ret_guild_leave_request(SprotoTypeBase req)
	{
		if (req is ret_guild_leave.request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			playerData.PlayerGuild.ResetGuild();
			if (Singleton<ObjManager>.Instance.MainPlayer != null)
			{
				Singleton<ObjManager>.Instance.MainPlayer.SearchAllGuild();
			}
			if (SingletonUnity<NewGuildUIRootLogic>.Exists && !UnityVersionUtil.IsActive(SingletonUnity<NewGuildUIRootLogic>.Instance.gameObject))
			{
			}
		}
		return null;
	}
}
