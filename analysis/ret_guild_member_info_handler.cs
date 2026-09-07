using Sproto;
using SprotoType;

public class ret_guild_member_info_handler
{
	public static SprotoTypeBase ret_guild_member_info_request(SprotoTypeBase req)
	{
		if (req is ret_guild_member_info.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (request.HasGuild_member_info)
			{
				PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
				playerData.PlayerGuild.UpDataGuildMemberList(request.guild_member_info);
				if (SingletonUnity<GuildMemberRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildMemberRootLogic>.Instance.gameObject))
				{
					SingletonUnity<GuildMemberRootLogic>.Instance.UpdateGuildMemberItemList(playerData.PlayerGuild.GuildMemberList);
				}
				if (SingletonUnity<TeamInviteRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamInviteRootLogic>.Instance.gameObject))
				{
					SingletonUnity<TeamInviteRootLogic>.Instance.UpdateGuildMemberInfo();
				}
			}
		}
		return null;
	}
}
