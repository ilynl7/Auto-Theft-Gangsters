using Sproto;
using SprotoType;

public class ret_guild_donate_handler
{
	public static SprotoTypeBase ret_guild_donate_request(SprotoTypeBase req)
	{
		if (req is ret_guild_donate.request request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (request.state == 0L)
			{
				playerData.PlayerGuild.UseDonate(request.id);
				playerData.PlayerGuild.GuilLevel = (int)request.level;
				playerData.PlayerGuild.GuildExp = (int)request.exp;
				playerData.PlayerGuild.UpdateAllContribute((int)(request.HasAll_contribute ? request.all_contribute : 0));
				if (request.HasContribute)
				{
					GameMoneyHelper.SetGuildContribute((int)request.contribute);
				}
				else
				{
					GameMoneyHelper.SetGuildContribute(0L);
				}
				if (SingletonUnity<GuildInfoRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildInfoRootLogic>.Instance.gameObject))
				{
					SingletonUnity<GuildInfoRootLogic>.Instance.UpDateGuildInfo(playerData.PlayerGuild);
				}
				NoticeLogic.AddNotifyData("#{100789}");
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100790}");
			}
		}
		return null;
	}
}
