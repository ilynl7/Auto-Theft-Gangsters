using Sproto;
using SprotoType;

public class ret_request_update_friend_useinfo_handler
{
	public static SprotoTypeBase ret_request_update_friend_useinfo_request(SprotoTypeBase req)
	{
		if (req is ret_request_update_friend_useinfo.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (request.HasFriend_list)
			{
				FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
				if (request.HasType && request.type == 1)
				{
					friendInfo.FilterEnemy(request.friend_list);
				}
				else
				{
					friendInfo.FilterFriend(request.friend_list);
				}
				if (SingletonUnity<TeamInviteRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamInviteRootLogic>.Instance.gameObject))
				{
					SingletonUnity<TeamInviteRootLogic>.Instance.UpdateFriendListInfo();
				}
				if (SingletonUnity<EnemyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<EnemyUIRootLogic>.Instance.gameObject))
				{
					SingletonUnity<EnemyUIRootLogic>.Instance.UpdateEnemyList();
				}
			}
		}
		return null;
	}
}
