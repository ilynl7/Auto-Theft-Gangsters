using Sproto;
using SprotoType;

public class ret_enter_guild_battle_handler
{
	public static SprotoTypeBase ret_enter_guild_battle_request(SprotoTypeBase req)
	{
		if (req is ret_enter_guild_battle.request)
		{
			WaitResponseUIRootLogic.CloseBox();
			NoticeLogic.AddNotifyData("#{105086}");
		}
		return null;
	}
}
