using SprotoType;
using UnityEngine;

public class FriendItemLogic : MonoBehaviour
{
	public UISprite BkSprite;

	public UISprite IconSprite;

	public UILabel LevelLabel;

	public UILabel FightLabel;

	public UILabel GuildNameLabel;

	public UILabel FriendScoreLabel;

	public UILabel FriendNamelabel;

	private friend_info curFriendInfo;

	public void UpdateFriendInfo(friend_info info)
	{
		FriendNamelabel.text = info.name;
		LevelLabel.text = $"Lv.{info.level}";
		IconSprite.spriteName = GameDefine.Player_Icon_Small_Pic[info.profession];
		if (info.state == 0L)
		{
			BkSprite.spriteName = "CZ_huaDongBG_2";
			IconSprite.alpha = 0.35f;
		}
		else
		{
			BkSprite.spriteName = "CZ_huaDongBG";
			IconSprite.alpha = 1f;
		}
		FriendScoreLabel.text = info.friendScore.ToString();
		if (info.HasGuildName)
		{
			GuildNameLabel.text = info.guildName;
		}
		else
		{
			GuildNameLabel.text = StrDictionary.GetDictionaryString("#{100240}");
		}
		FightLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100421}"), info.combValue.ToString());
		curFriendInfo = info;
	}

	public void OnClickItem()
	{
		TargetBasicInfo selectTargetBasicInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SelectTargetBasicInfo;
		selectTargetBasicInfo.ResetInfo(curFriendInfo.friendId, (int)curFriendInfo.level, (int)curFriendInfo.combValue, curFriendInfo.name, (PROFESSION_TYPE)curFriendInfo.profession, (int)curFriendInfo.state, curFriendInfo.guildId, curFriendInfo.guildName, UICamera.currentTouch.pos);
		HitOtherPLayerLogic.ShowMenu(HitType.HitFriendIcon, selectTargetBasicInfo);
	}

	public void OnClickTalk()
	{
		TargetBasicInfo selectTargetBasicInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SelectTargetBasicInfo;
		selectTargetBasicInfo.ResetInfo(curFriendInfo.friendId, (int)curFriendInfo.level, (int)curFriendInfo.combValue, curFriendInfo.name, (PROFESSION_TYPE)curFriendInfo.profession, (int)curFriendInfo.state, curFriendInfo.guildId, curFriendInfo.guildName, UICamera.currentTouch.pos);
		SingletonUnity<SocialUIRootLogic>.Instance.OnClickCloseBtn();
		ChatUIRootLogic.ResetPrivateChat(selectTargetBasicInfo.ServerId, selectTargetBasicInfo.Name, selectTargetBasicInfo.profession);
	}
}
