using Sproto;
using SprotoType;

public class ret_del_friend_handler
{
	public static SprotoTypeBase ret_del_friend_request(SprotoTypeBase req)
	{
		if (req is ret_del_friend.request { HasCharacterId: not false } request)
		{
			FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
			friendInfo.RemoveFriend(request.characterId);
		}
		return null;
	}
}
