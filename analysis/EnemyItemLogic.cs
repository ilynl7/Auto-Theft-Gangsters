using Sproto;
using SprotoType;
using UnityEngine;

public class EnemyItemLogic : MonoBehaviour
{
	public UISprite BkSprite;

	public UISprite IconSprite;

	public UILabel LevelLabel;

	public UILabel FightLabel;

	public UILabel GuildNameLabel;

	public UILabel FriendScoreLabel;

	public UILabel FriendNamelabel;

	private friend_info curFriendInfo;

	public UISprite RevengeBtnSp;

	public void UpdateFriendInfo(friend_info info)
	{
		curFriendInfo = info;
		LevelLabel.text = $"Lv.{info.level}";
		IconSprite.spriteName = GameDefine.Player_Icon_Small_Pic[info.profession];
		FriendNamelabel.text = info.name;
		FightLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100421}"), info.combValue.ToString());
		if (info.HasGuildName)
		{
			GuildNameLabel.text = info.guildName;
		}
		else
		{
			GuildNameLabel.text = StrDictionary.GetDictionaryString("#{100240}");
		}
		if (info.state == 0L)
		{
			BkSprite.spriteName = "CZ_huaDongBG_2";
			IconSprite.alpha = 0.35f;
			RevengeBtnSp.spriteName = GameDefine.BtnIcon[2];
		}
		else
		{
			BkSprite.spriteName = "CZ_huaDongBG";
			IconSprite.alpha = 1f;
			RevengeBtnSp.spriteName = GameDefine.BtnIcon[1];
		}
		FriendScoreLabel.text = info.friendScore.ToString();
	}

	public void OnClickRevengeBtn()
	{
		if (curFriendInfo.state == 0L)
		{
			NoticeLogic.AddNotifyData("#{103317}");
		}
		else if (curFriendInfo != null)
		{
			update_player_map_info.request request = new update_player_map_info.request();
			request.characterId = curFriendInfo.friendId;
			NetLogic.GetInstance().Send<Protocol.update_player_map_info>(request, Ret_Ask_Character_info);
		}
	}

	private void Ret_Ask_Character_info(SprotoTypeBase req)
	{
		if (UIManager.IsUnlockTutorialEnable())
		{
			return;
		}
		update_player_map_info.response response = req as update_player_map_info.response;
		if (response != null && response.HasState)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EnemyRevengeRoot, delegate
			{
				SingletonUnity<EnemyRevengeRootLogic>.Instance.Reset(curFriendInfo, response);
			});
		}
	}

	public void OnClickDeleteBtn()
	{
		if (curFriendInfo != null)
		{
			MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{103319}", curFriendInfo.name), StrDictionary.GetDictionaryString("#{100244}"), delegate
			{
				del_friend.request rpcReq = new del_friend.request
				{
					characterId = curFriendInfo.friendId,
					type = 1L
				};
				NetLogic.GetInstance().Send<Protocol.del_friend>(rpcReq);
				FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
				friendInfo.RemoveEnemy(curFriendInfo.friendId);
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Social", "Enemy", "del_times");
			});
		}
	}

	public void OnClickItem()
	{
		TargetBasicInfo selectTargetBasicInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SelectTargetBasicInfo;
		selectTargetBasicInfo.ResetInfo(curFriendInfo.friendId, (int)curFriendInfo.level, (int)curFriendInfo.combValue, curFriendInfo.name, (PROFESSION_TYPE)curFriendInfo.profession, (int)curFriendInfo.state, curFriendInfo.guildId, curFriendInfo.guildName, UICamera.currentTouch.pos);
		HitOtherPLayerLogic.ShowMenu(HitType.HitFriendIcon, selectTargetBasicInfo);
	}
}
