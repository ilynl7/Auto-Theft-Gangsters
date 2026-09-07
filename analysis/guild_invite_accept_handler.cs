using Sproto;
using SprotoType;

public class guild_invite_accept_handler
{
	public static SprotoTypeBase guild_invite_accept_request(SprotoTypeBase req)
	{
		if (req is guild_invite_accept.request request)
		{
			string name = request.name;
			string guildName = request.guildName;
			long guildId = request.guildId;
			MessageBoxLogic.OpenOKCancelWaitBox(StrDictionary.GetDictionaryString("#{100776}", name, guildName), "#{100127}", 10f, delegate
			{
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GUILD))
				{
					guild_join.request rpcReq = new guild_join.request
					{
						guildId = guildId
					};
					NetLogic.GetInstance().Send<Protocol.guild_join>(rpcReq);
				}
				else
				{
					NoticeLogic.AddNotifyData("#{100288}");
				}
			});
		}
		return null;
	}
}
