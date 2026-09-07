using Sproto;
using SprotoType;

public class ret_guild_approve_resverve_handler
{
	public static SprotoTypeBase ret_guild_approve_resverve_request(SprotoTypeBase req)
	{
		if (req is ret_guild_approve_resverve.request request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (request.isAgree == 0L)
			{
				playerData.PlayerGuild.RemoveByID(request.characterId);
			}
			else
			{
				playerData.PlayerGuild.setMemberJob(request.characterId, Guild_JOB.JOB_Member);
			}
			if (SingletonUnity<GuildMemberRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildMemberRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildMemberRootLogic>.Instance.UpdateGuildMemberItemList(playerData.PlayerGuild.GuildMemberList);
			}
		}
		return null;
	}
}
