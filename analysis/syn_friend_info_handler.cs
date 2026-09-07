using Sproto;
using SprotoType;

public class syn_friend_info_handler
{
	public static SprotoTypeBase syn_friend_info_request(SprotoTypeBase req)
	{
		if (req is syn_friend_info.request { HasFriend: not false, friend: var friend })
		{
			FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
			friendInfo.UpdateFriendInfo(friend);
		}
		return null;
	}
}
