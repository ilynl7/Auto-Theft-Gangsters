using System.Collections.Generic;
using Sproto;
using SprotoType;

public class ret_search_guild_handler
{
	public static SprotoTypeBase ret_search_guild_request(SprotoTypeBase req)
	{
		if (req is ret_search_guild.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (request.HasGuild)
			{
				List<guild_info> list = new List<guild_info>();
				list.Clear();
				list.Add(request.guild);
				List<long> list2 = new List<long>();
				list2.Clear();
				list2.Add(request.rank);
				if (SingletonUnity<GuildListInfoRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildListInfoRootLogic>.Instance.gameObject))
				{
					SingletonUnity<GuildListInfoRootLogic>.Instance.ShowSearchResult(list, list2);
				}
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100793}");
				if (SingletonUnity<GuildListInfoRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildListInfoRootLogic>.Instance.gameObject))
				{
					SingletonUnity<GuildListInfoRootLogic>.Instance.DisableSearchFlag();
				}
			}
		}
		return null;
	}
}
