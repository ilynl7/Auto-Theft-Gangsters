using Sproto;
using SprotoType;

public class ret_guild_job_change_handler
{
	public static SprotoTypeBase ret_guild_job_change_request(SprotoTypeBase req)
	{
		if (req is ret_guild_job_change.request { state: not false } request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (request.HasCharacterId && request.HasJob)
			{
				if ((int)request.job == 0)
				{
					playerData.PlayerGuild.GuildChiefId = request.characterId;
					playerData.PlayerGuild.GuildChiefName = playerData.PlayerGuild.getMemberName(request.characterId);
					playerData.PlayerGuild.setMemberJob(Singleton<ObjManager>.Instance.MainPlayer.ServerId, Guild_JOB.JOB_Member);
				}
				playerData.PlayerGuild.setMemberJob(request.characterId, (Guild_JOB)request.job);
				if (SingletonUnity<GuildMemberRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildMemberRootLogic>.Instance.gameObject))
				{
					SingletonUnity<GuildMemberRootLogic>.Instance.UpdateGuildMemberItemList(playerData.PlayerGuild.GuildMemberList);
				}
			}
		}
		return null;
	}
}
