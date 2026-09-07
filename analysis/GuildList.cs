using System.Collections.Generic;
using SprotoType;

public class GuildList
{
	private List<GuildInfo> mGuildInfoList = new List<GuildInfo>();

	public List<GuildInfo> GuildInfoList => mGuildInfoList;

	public void Init()
	{
	}

	public void UpDataGuildList(List<guild_info> list)
	{
		if (list.Count > 0)
		{
			if (mGuildInfoList == null)
			{
				mGuildInfoList = new List<GuildInfo>();
			}
			else
			{
				mGuildInfoList.Clear();
			}
			for (int i = 0; i < list.Count; i++)
			{
				GuildInfo item = new GuildInfo(list[i]);
				mGuildInfoList.Add(item);
			}
		}
	}
}
