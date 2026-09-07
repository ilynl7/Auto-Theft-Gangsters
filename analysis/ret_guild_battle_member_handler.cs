using Sproto;
using SprotoType;

public class ret_guild_battle_member_handler
{
	public static SprotoTypeBase ret_guild_battle_member_request(SprotoTypeBase req)
	{
		if (req is ret_guild_battle_member.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.ActivityData.SyncGuildBattleMember(request);
			if (SingletonUnity<GuildBattleMemberRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildBattleMemberRootLogic>.Instance.gameObject))
			{
				SingletonUnity<GuildBattleMemberRootLogic>.Instance.RefershInfo(request);
			}
		}
		return null;
	}
}
