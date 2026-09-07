using Sproto;
using SprotoType;

public class mail_delete_handler
{
	public static SprotoTypeBase mail_delete_request(SprotoTypeBase req)
	{
		if (req is mail_delete.request { HasMailId: not false } request)
		{
			FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
			friendInfo.DelMail(request.mailId);
		}
		return null;
	}
}
