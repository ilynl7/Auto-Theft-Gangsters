using SprotoType;
using UnityEngine;

public class GuildBattleMemberLine : MonoBehaviour
{
	public UILabel NameLabel;

	public UILabel PowerLabel;

	public UISprite playericon;

	public UILabel professLabel;

	public UILabel BtnLabel;

	public UISprite SelectPic;

	public UISprite BkSprite;

	private int mCurIndex = -1;

	private guild_member_info mCurInfo;

	public void UpdateInfo(guild_member_info curinfo, int index)
	{
		mCurIndex = index;
		mCurInfo = curinfo;
		NameLabel.text = curinfo.name;
		PowerLabel.text = string.Format("{0} {1}", StrDictionary.GetDictionaryString("#{100421}"), curinfo.combValue);
		playericon.spriteName = GameDefine.Player_Icon_Pic[curinfo.profession];
		professLabel.text = StrDictionary.GetDictionaryString(GameDefine.GuildJobStr[curinfo.job]);
		if (curinfo.state == 1)
		{
			playericon.alpha = 1f;
			if (curinfo.characterId == PlayerData.MainPlayerServerId)
			{
				BkSprite.spriteName = "CZ_huaDongBG_1";
			}
			else
			{
				BkSprite.spriteName = "CZ_huaDongBG";
			}
		}
		else
		{
			playericon.alpha = 0.35f;
			BkSprite.spriteName = "CZ_huaDongBG_2";
		}
		UpdateBtn(curinfo);
	}

	public void UpdateBtn(guild_member_info curinfo)
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.isGuildChief())
		{
			if (curinfo.characterId == PlayerData.MainPlayerServerId)
			{
				NGUITools.SetActive(SelectPic.gameObject, state: true);
				BtnLabel.text = StrDictionary.GetDictionaryString("#{105049}");
			}
			else if (curinfo.battle == 0L)
			{
				NGUITools.SetActive(SelectPic.gameObject, state: false);
				BtnLabel.text = StrDictionary.GetDictionaryString("#{105048}");
			}
			else
			{
				NGUITools.SetActive(SelectPic.gameObject, state: true);
				BtnLabel.text = StrDictionary.GetDictionaryString("#{105049}");
			}
		}
		else if (curinfo.battle == 0L)
		{
			NGUITools.SetActive(SelectPic.gameObject, state: false);
			BtnLabel.text = StrDictionary.GetDictionaryString("#{105048}");
		}
		else
		{
			NGUITools.SetActive(SelectPic.gameObject, state: true);
			BtnLabel.text = StrDictionary.GetDictionaryString("#{105049}");
		}
	}

	public void OnClickBtn()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.isGuildChief())
		{
			if (mCurInfo.characterId == PlayerData.MainPlayerServerId)
			{
				NoticeLogic.AddNotifyData("#{105084}");
				return;
			}
			if (SingletonUnity<GuildBattleMemberRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildBattleMemberRootLogic>.Instance.gameObject))
			{
				if (mCurInfo.battle == 0L)
				{
					if (SingletonUnity<GuildBattleMemberRootLogic>.Instance.OnClickSelectAdd(mCurInfo.characterId))
					{
						mCurInfo.battle = 1L;
						UpdateBtn(mCurInfo);
					}
				}
				else
				{
					SingletonUnity<GuildBattleMemberRootLogic>.Instance.OnClickSelectDec(mCurInfo.characterId);
					mCurInfo.battle = 0L;
					UpdateBtn(mCurInfo);
				}
			}
			if (mCurInfo.battle == 0L)
			{
				NGUITools.SetActive(SelectPic.gameObject, state: false);
				BtnLabel.text = StrDictionary.GetDictionaryString("#{105048}");
			}
			else
			{
				NGUITools.SetActive(SelectPic.gameObject, state: true);
				BtnLabel.text = StrDictionary.GetDictionaryString("#{105049}");
			}
		}
		else
		{
			NoticeLogic.AddNotifyData("#{105083}");
		}
	}

	public void OnClickIconBtn()
	{
		if (mCurInfo.characterId != PlayerData.MainPlayerServerId)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			TargetBasicInfo selectTargetBasicInfo = playerData.SelectTargetBasicInfo;
			selectTargetBasicInfo.ResetInfo(mCurInfo.characterId, (int)mCurInfo.level, (int)mCurInfo.combValue, mCurInfo.name, (PROFESSION_TYPE)mCurInfo.profession, (int)mCurInfo.state, mCurInfo.guildId, playerData.PlayerGuild.GuilName, UICamera.currentTouch.pos);
			HitOtherPLayerLogic.ShowMenu(HitType.HitGuildMember, selectTargetBasicInfo);
		}
	}
}
