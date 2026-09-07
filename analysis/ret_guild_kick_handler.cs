using Sproto;
using SprotoType;

public class ret_guild_kick_handler
{
	public static SprotoTypeBase ret_guild_kick_request(SprotoTypeBase req)
	{
		if (req is ret_guild_kick.request { HasState: not false, state: not false, HasCharacterId: not false } request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			playerData.PlayerGuild.RemoveByID(request.characterId);
			if (SingletonUnity<GuildMemberRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildMemberRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildMemberRootLogic>.Instance.UpdateGuildMemberItemList(playerData.PlayerGuild.GuildMemberList);
			}
		}
		return null;
	}
}
