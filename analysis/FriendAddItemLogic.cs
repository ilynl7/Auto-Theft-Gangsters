using SprotoType;
using UnityEngine;

public class FriendAddItemLogic : MonoBehaviour
{
	public UISprite BkSprite;

	public UISprite IconSprite;

	public UILabel LevelLabel;

	public UILabel FightLabel;

	public UILabel GuildNameLabel;

	public UILabel FriendNamelabel;

	public UISprite ProfesssionFlag;

	private friend_info curFriendInfo;

	public void UpdateFriendInfo(friend_info info)
	{
		FriendNamelabel.text = info.name;
		LevelLabel.text = $"Lv.{info.level}";
		IconSprite.spriteName = GameDefine.Player_Icon_Small_Pic[info.profession];
		ProfesssionFlag.spriteName = GameDefine.Profession_PicName[info.profession];
		if (info.HasGuildName)
		{
			GuildNameLabel.text = info.guildName;
		}
		else
		{
			GuildNameLabel.text = StrDictionary.GetDictionaryString("#{100240}");
		}
		FightLabel.text = string.Format("{0}: {1}", StrDictionary.GetDictionaryString("#{100421}"), info.combValue.ToString());
		curFriendInfo = info;
	}

	public void OnClickAdd()
	{
		add_friend.request request = new add_friend.request();
		request.characterId = curFriendInfo.characterId;
		NetLogic.GetInstance().Send<Protocol.add_friend>(request);
		FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
		if (friendInfo.MainPlayerRandomFriendDic.ContainsKey(curFriendInfo.characterId))
		{
			friendInfo.MainPlayerRandomFriendDic.Remove(curFriendInfo.characterId);
		}
		if (friendInfo.MainPlayerSearchFriendDic.ContainsKey(curFriendInfo.characterId))
		{
			friendInfo.MainPlayerSearchFriendDic.Remove(curFriendInfo.characterId);
		}
		if (SingletonUnity<FriendAddUILogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FriendAddUILogic>.Instance.gameObject))
		{
			SingletonUnity<FriendAddUILogic>.Instance.UpdateFriendList();
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Social", "Friend", "apply_times");
	}
}
