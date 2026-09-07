using Sproto;
using SprotoType;

public class ret_add_friend_handler
{
	public static SprotoTypeBase ret_add_friend_request(SprotoTypeBase req)
	{
		if (req is ret_add_friend.request { HasFriend: not false, friend: var friend })
		{
			FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
			friendInfo.AddFriend(friend);
		}
		return null;
	}
}
