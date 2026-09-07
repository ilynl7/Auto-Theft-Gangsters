using Sproto;
using SprotoType;

public class notice_add_friend_handler
{
	public static SprotoTypeBase notice_add_friend_request(SprotoTypeBase req)
	{
		if (req is notice_add_friend.request { HasFriend: not false, friend: var friend })
		{
			FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
			friendInfo.AddApplyFriend(friend);
		}
		return null;
	}
}
