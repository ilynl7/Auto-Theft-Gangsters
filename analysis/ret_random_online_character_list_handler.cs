using Sproto;
using SprotoType;

public class ret_random_online_character_list_handler
{
	public static SprotoTypeBase ret_random_online_character_list_request(SprotoTypeBase req)
	{
		if (req is ret_random_online_character_list.request { HasFriend_list: not false } request)
		{
			FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
			friendInfo.FilterRandomFriendDic(request.friend_list);
			if (SingletonUnity<SocialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SocialUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<SocialUIRootLogic>.Instance.UpdateSocialInfo();
			}
			if (SingletonUnity<TeamInviteRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamInviteRootLogic>.Instance.gameObject))
			{
				SingletonUnity<TeamInviteRootLogic>.Instance.UpdateNearByPlayerList();
			}
		}
		return null;
	}
}
