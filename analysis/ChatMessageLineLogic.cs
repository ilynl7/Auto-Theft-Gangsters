using System;
using SprotoType;
using UnityEngine;

public class ChatMessageLineLogic : MonoBehaviour
{
	public UIWidget RootWidget;

	public UISprite PlayerIcon;

	public UILabel PlayerName;

	public UILabel GuildName;

	public UILabel MessageLabel;

	public UISprite MessageSprite;

	public BoxCollider MessageCollider;

	public UIEventListener MessageListener;

	public UILabel LevelLabel;

	private PlayerChatHistoryInfo mCurInfo;

	public PlayerChatHistoryInfo CurInfo => mCurInfo;

	private void Awake()
	{
		UIEventListener messageListener = MessageListener;
		messageListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(messageListener.onClick, new UIEventListener.VoidDelegate(OnClickMessage));
	}

	public void Reset(PlayerChatHistoryInfo info)
	{
		mCurInfo = info;
		if (mCurInfo.ChannelType == GameDefine.CHAT_CHANNEL_TYPE.SYSTEM)
		{
			UnityVersionUtil.SetActiveRecursive(PlayerIcon.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(MessageSprite.gameObject, state: true);
			PlayerIcon.spriteName = GameDefine.Player_Icon_Small_Pic[3];
			PlayerName.color = Color.white;
			PlayerName.text = StrDictionary.GetDictionaryString("#{100248}");
			GuildName.text = string.Empty;
			MessageLabel.width = 183;
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsNeedTranslation)
			{
				MessageLabel.text = NGUIText.StripSymbols(mCurInfo.ChatInfo2);
			}
			else
			{
				MessageLabel.text = NGUIText.StripSymbols(mCurInfo.ChatInfo);
			}
			if (mCurInfo.LinkType != GameDefine.CHAT_LINK_TYPE.INVALID)
			{
				MessageLabel.color = Color.blue;
			}
			else
			{
				MessageLabel.color = Color.black;
			}
			MessageSprite.height = MessageLabel.height + 15;
			MessageCollider.size = MessageSprite.localSize;
			MessageCollider.center = new Vector3(MessageCollider.size.x / 2f, (0f - MessageCollider.size.y) / 2f, 0f);
			PlayerIcon.transform.localPosition = new Vector3(38f, MessageSprite.height - 20, 0f);
			PlayerName.transform.localPosition = new Vector3(81f, MessageSprite.height + 5, 0f);
			GuildName.transform.localPosition = new Vector3(142f, MessageSprite.height + 5, 0f);
			MessageLabel.transform.localPosition = new Vector3(90f, MessageSprite.height - 10, 0f);
			MessageSprite.transform.localPosition = new Vector3(68f, MessageSprite.height, 0f);
			MessageSprite.transform.localScale = Vector3.one;
			RootWidget.height = MessageSprite.height + 25;
			LevelLabel.text = string.Empty;
			return;
		}
		UnityVersionUtil.SetActiveRecursive(PlayerIcon.gameObject, state: true);
		UnityVersionUtil.SetActiveRecursive(MessageSprite.gameObject, state: true);
		PlayerIcon.spriteName = GameDefine.Player_Icon_Small_Pic[(int)mCurInfo.SenderProfession];
		PlayerName.color = Color.white;
		PlayerName.text = mCurInfo.SenderName;
		LevelLabel.text = $"Lv.{mCurInfo.Level}";
		if (string.IsNullOrEmpty(mCurInfo.GuildName))
		{
			GuildName.text = string.Empty;
		}
		else
		{
			GuildName.text = $"[{mCurInfo.GuildName}]";
		}
		MessageLabel.width = 183;
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsNeedTranslation)
		{
			MessageLabel.text = NGUIText.StripSymbols(mCurInfo.ChatInfo2);
		}
		else
		{
			MessageLabel.text = NGUIText.StripSymbols(mCurInfo.ChatInfo);
		}
		if (mCurInfo.LinkType != GameDefine.CHAT_LINK_TYPE.INVALID)
		{
			MessageLabel.color = Color.blue;
		}
		else
		{
			MessageLabel.color = Color.black;
		}
		MessageSprite.height = MessageLabel.height + 15;
		MessageCollider.size = MessageSprite.localSize;
		MessageCollider.center = new Vector3(MessageCollider.size.x / 2f, (0f - MessageCollider.size.y) / 2f, 0f);
		if (IsMainPlayer(mCurInfo.SenderServerId))
		{
			PlayerIcon.transform.localPosition = new Vector3(260f, MessageSprite.height - 20, 0f);
			PlayerName.transform.localPosition = new Vector3(25f, MessageSprite.height + 5, 0f);
			GuildName.transform.localPosition = new Vector3(142f, MessageSprite.height + 5, 0f);
			MessageLabel.transform.localPosition = new Vector3(28f, MessageSprite.height - 10, 0f);
			MessageSprite.transform.localPosition = new Vector3(230f, MessageSprite.height, 0f);
			MessageSprite.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			PlayerIcon.transform.localPosition = new Vector3(38f, MessageSprite.height - 20, 0f);
			PlayerName.transform.localPosition = new Vector3(81f, MessageSprite.height + 5, 0f);
			GuildName.transform.localPosition = new Vector3(190f, MessageSprite.height + 5, 0f);
			MessageLabel.transform.localPosition = new Vector3(90f, MessageSprite.height - 10, 0f);
			MessageSprite.transform.localPosition = new Vector3(68f, MessageSprite.height, 0f);
			MessageSprite.transform.localScale = Vector3.one;
		}
		RootWidget.height = MessageSprite.height + 25;
	}

	private bool IsMainPlayer(long serverId)
	{
		if (serverId == Singleton<ObjManager>.Instance.MainPlayer.ServerId)
		{
			return true;
		}
		return false;
	}

	private void OnDragMessage(Vector2 delta)
	{
	}

	public void OnClickPlayerIcon()
	{
		if (!IsMainPlayer(mCurInfo.SenderServerId) && mCurInfo.ChannelType != 0)
		{
			TargetBasicInfo selectTargetBasicInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SelectTargetBasicInfo;
			selectTargetBasicInfo.ResetInfo(mCurInfo.SenderServerId, mCurInfo.Level, mCurInfo.ComboValue, mCurInfo.SenderName, mCurInfo.SenderProfession, 1, mCurInfo.GuildId, mCurInfo.GuildName, UICamera.currentTouch.pos);
			HitOtherPLayerLogic.ShowMenu(HitType.HitChatListItemIcon, selectTargetBasicInfo);
		}
	}

	public void OnClickMessage(GameObject obj)
	{
		switch (mCurInfo.LinkType)
		{
		case GameDefine.CHAT_LINK_TYPE.INVALID:
			break;
		case GameDefine.CHAT_LINK_TYPE.ITEM:
			OnClickItemLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.EQUIP:
			OnClickEquipLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.TEAM:
			OnClickTeamLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.GUILD:
			OnClickGuildLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.GUILD_BOSS:
			OnClickGuildBossLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.BAR_FIGHT:
			OnClickBarFightLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.DANCE:
			OnClickDanceLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.ESCORT:
			OnClickEscortLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.ATTACK_ESCORT:
			OnClickAttackEscortLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.WILD_BOSS:
			OnClickWildBossLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.SURVIVE:
			OnClickSurviveLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.GUILD_DANCE:
			OnClickGuildDanceLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.FIRST_GUILD_DANCE:
			break;
		case GameDefine.CHAT_LINK_TYPE.GUILD_DONMINE:
			OnClickGuildCityLink();
			break;
		case GameDefine.CHAT_LINK_TYPE.GUILD_DONMINE_RES:
			break;
		}
	}

	private void OnClickGuildCityLink()
	{
		if (IsCanGoto() && CheckUnlockFunction(FUNCTION_TYPE.GUILD_ACTIVITY))
		{
			SingletonUnity<ChatUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_DONMINE);
			});
		}
	}

	private void OnClickGuildDanceLink()
	{
		if (IsCanGoto() && CheckUnlockFunction(FUNCTION_TYPE.GUILD_ACTIVITY))
		{
			SingletonUnity<ChatUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_DANCE);
			});
		}
	}

	private void OnClickItemLink()
	{
	}

	private void OnClickEquipLink()
	{
	}

	private void OnClickTeamLink()
	{
		if (!IsCanGoto())
		{
			return;
		}
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.TEAM))
		{
			NoticeLogic.AddNotifyData("#{100276}");
			return;
		}
		int minlevel = (int)mCurInfo.LongData[1];
		int maxlevel = (int)mCurInfo.LongData[2];
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(minlevel, maxlevel))
		{
			NoticeLogic.AddNotifyData("#{101539}");
			return;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveTeam())
		{
			NoticeLogic.AddNotifyData("#{100277}");
			return;
		}
		long num = mCurInfo.LongData[0];
		NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100788}"));
		req_join_team.request request = new req_join_team.request();
		request.teamid = num;
		request.isapply = false;
		NetLogic.GetInstance().Send<Protocol.req_join_team>(request);
		team team = new team();
		team.id = num;
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.AddApplyTeam(team);
	}

	private void OnClickGuildLink()
	{
		if (IsCanGoto() && CheckUnlockFunction(FUNCTION_TYPE.GUILD))
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
			{
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100785}"));
				return;
			}
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100788}"));
			Singleton<ObjManager>.Instance.MainPlayer.JoinGuild(mCurInfo.LongData[0]);
		}
	}

	private void OnClickGuildBossLink()
	{
		if (IsCanGoto() && CheckUnlockFunction(FUNCTION_TYPE.GUILD_ACTIVITY))
		{
			SingletonUnity<ChatUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_BOSS);
			});
		}
	}

	private void OnClickEscortLink()
	{
		if (IsCanGoto() && CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
		{
			SingletonUnity<ChatUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.ESCORT);
			});
		}
	}

	private void OnClickAttackEscortLink()
	{
		if (IsCanGoto() && CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
		{
			SingletonUnity<ChatUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT);
			});
		}
	}

	private void OnClickDanceLink()
	{
		if (IsCanGoto() && CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
		{
			SingletonUnity<ChatUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.CITY_DANCE);
			});
		}
	}

	private void OnClickSurviveLink()
	{
		if (IsCanGoto() && CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
		{
			SingletonUnity<ChatUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE);
			});
		}
	}

	private void OnClickBarFightLink()
	{
		if (IsCanGoto() && CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
		{
			SingletonUnity<ChatUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.BAR_FIGHT);
			});
		}
	}

	private void OnClickWildBossLink()
	{
		if (IsCanGoto() && CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_WORLDBOSS))
		{
			SingletonUnity<ChatUIRootLogic>.Instance.OnClickCloseBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.WILD_BOSS);
			});
		}
	}

	public bool IsCanGoto()
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager != null && (sceneManager.IsBigWorld() || sceneManager.IsTutorialScene()))
		{
			return true;
		}
		NoticeLogic.AddNotifyData("#{102042}");
		return false;
	}

	public bool CheckUnlockFunction(FUNCTION_TYPE type)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData.IsFunctionUnlock(type))
		{
			return true;
		}
		NoticeLogic.AddNotifyData("#{101539}");
		return false;
	}
}
