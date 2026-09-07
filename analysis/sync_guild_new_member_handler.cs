using Sproto;
using SprotoType;

public class sync_guild_new_member_handler
{
	public static SprotoTypeBase sync_guild_new_member_request(SprotoTypeBase req)
	{
		if (req is sync_guild_new_member.request { HasGuild_member_info: not false } request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			playerData.PlayerGuild.AddNewMember(request.guild_member_info);
			if (SingletonUnity<GuildMemberRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildMemberRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildMemberRootLogic>.Instance.UpdateGuildMemberItemList(playerData.PlayerGuild.GuildMemberList);
			}
		}
		return null;
	}
}
