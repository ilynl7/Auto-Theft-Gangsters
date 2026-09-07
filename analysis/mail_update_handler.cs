using Sproto;
using SprotoType;

public class mail_update_handler
{
	public static SprotoTypeBase mail_update_request(SprotoTypeBase req)
	{
		if (req is mail_update.request data)
		{
			FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
			friendInfo.UpdateMailData(data);
		}
		return null;
	}
}
