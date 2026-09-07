using Sproto;
using SprotoType;

public class ret_guild_skill_level_handler
{
	public static SprotoTypeBase ret_guild_skill_level_request(SprotoTypeBase req)
	{
		if (req is ret_guild_skill_level.request { state: 0L } request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (request.HasContribute)
			{
				GameMoneyHelper.SetGuildContribute((int)request.contribute);
			}
			if (request.HasGuildSkillType)
			{
				playerData.PlayerGuild.UpdateSkillType(request.guildSkillType, request.level);
			}
			if (SingletonUnity<GuildSkillRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildSkillRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildSkillRootLogic>.Instance.UpdateGuildSKill();
			}
			NoticeLogic.AddNotifyData("#{101145}");
		}
		return null;
	}
}
