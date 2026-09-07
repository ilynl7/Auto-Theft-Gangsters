using Sproto;
using SprotoType;

public class be_deleted_friend_handler
{
	public static SprotoTypeBase be_deleted_friend_request(SprotoTypeBase req)
	{
		if (req is be_deleted_friend.request { HasCharacterId: not false } request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			playerData.FriendInfo.RemoveFriend(request.characterId);
		}
		return null;
	}
}
