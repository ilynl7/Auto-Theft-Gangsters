using System.Collections.Generic;
using SprotoType;

public class FriendApplyUIRootLogic : SingletonUnity<FriendApplyUIRootLogic>
{
	public TwoColumnPlayerInfoPage mPlayerInfoPage;

	protected override void Awake()
	{
		base.Awake();
	}

	public void UpdateFriendList()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		List<friend_info> list = new List<friend_info>(playerData.FriendInfo.ApplyFriendDic.Values);
		list = FriendInfo.SortFrientList(list);
		List<PlayerInfoItemData> list2 = new List<PlayerInfoItemData>();
		for (int i = 0; i < list.Count; i++)
		{
			PlayerInfoItemData playerInfoItemData = new PlayerInfoItemData();
			playerInfoItemData.Profession = (int)list[i].profession;
			playerInfoItemData.Name = list[i].name;
			playerInfoItemData.Level = (int)list[i].level;
			playerInfoItemData.ComboVal = (int)list[i].combValue;
			playerInfoItemData.Key = list[i].friendId;
			playerInfoItemData.IsEnable = true;
			playerInfoItemData.GuildId = list[i].guildId;
			playerInfoItemData.GuildName = list[i].guildName;
			list2.Add(playerInfoItemData);
		}
		mPlayerInfoPage.Reset(list2, StrDictionary.GetDictionaryString("#{100219}"), StrDictionary.GetDictionaryString("#{100219}"), OnClickAcceptFriend);
	}

	public void OnClickAcceptFriend(long key)
	{
		approve_resverve_friend.request request = new approve_resverve_friend.request();
		request.characterId = key;
		request.isAgree = 1L;
		NetLogic.GetInstance().Send<Protocol.approve_resverve_friend>(request);
		FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
		friendInfo.RemoveApply(key);
		if (SingletonUnity<SocialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SocialUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<SocialUIRootLogic>.Instance.UpdateSocialInfo();
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Social", "Friend", "accept_times");
	}

	public void OnClickAddAll()
	{
		MessageBoxLogic.OpenOKCancelBox("#{100224}", "#{100127}", OnClickAllAgree, OnNoClick);
	}

	public void OnClikcRefuseAll()
	{
		MessageBoxLogic.OpenOKCancelBox("#{100223}", "#{100127}", OnClickAllRefuse, OnNoClick);
	}

	private void OnClickAllAgree()
	{
		FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
		Dictionary<long, friend_info> applyFriendDic = friendInfo.ApplyFriendDic;
		if (applyFriendDic.Count + friendInfo.FriendCount >= GameDefine.MAX_FRIENT_COUNT)
		{
			NoticeLogic.AddNotifyData("#{100227}");
			return;
		}
		approve_resverve_friend.request request = new approve_resverve_friend.request();
		foreach (KeyValuePair<long, friend_info> item in applyFriendDic)
		{
			request.characterId = item.Value.friendId;
			request.isAgree = 1L;
			NetLogic.GetInstance().Send<Protocol.approve_resverve_friend>(request);
		}
		applyFriendDic.Clear();
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo.RefreshUI();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Social", "Friend", "accept_times");
	}

	private void OnNoClick()
	{
	}

	public void OnClickAllRefuse()
	{
		Dictionary<long, friend_info> applyFriendDic = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo.ApplyFriendDic;
		approve_resverve_friend.request request = new approve_resverve_friend.request();
		foreach (KeyValuePair<long, friend_info> item in applyFriendDic)
		{
			request.characterId = item.Value.friendId;
			request.isAgree = 0L;
			NetLogic.GetInstance().Send<Protocol.approve_resverve_friend>(request);
		}
		applyFriendDic.Clear();
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo.RefreshUI();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Social", "Friend", "refuse_times");
	}

	public void OnClickAddFriend()
	{
		FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
		if (friendInfo.FriendCount >= GameDefine.MAX_FRIENT_COUNT)
		{
			NoticeLogic.AddNotifyData("#{100227}");
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.FriendAddUILogic, delegate
		{
			SingletonUnity<FriendAddUILogic>.Instance.Reset();
			NetLogic.GetInstance().Send<Protocol.req_random_online_character_list>();
		});
	}
}
