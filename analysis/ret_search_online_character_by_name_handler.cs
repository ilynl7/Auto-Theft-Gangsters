using Sproto;
using SprotoType;

public class ret_search_online_character_by_name_handler
{
	public static SprotoTypeBase ret_search_online_character_by_name_request(SprotoTypeBase req)
	{
		if (req is ret_search_online_character_by_name.request request)
		{
			if (request.HasFriend_list && request.friend_list.Count > 0)
			{
				FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
				friendInfo.FilterSearchFriend(request.friend_list);
				if (SingletonUnity<SocialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SocialUIRootLogic>.Instance.gameObject))
				{
					SingletonUnity<SocialUIRootLogic>.Instance.UpdateSocialInfo();
				}
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100222}");
			}
		}
		return null;
	}
}
