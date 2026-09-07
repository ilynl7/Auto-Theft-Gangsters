using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ChatUIRootLogic : SingletonUnity<ChatUIRootLogic>
{
	public UIGrid ChannelGrid;

	public List<GameObject> ChannelBtn;

	public List<UISprite> ChannelIconSprite;

	public ChatMessageRootLogic ChatMessageRootLogic;

	public TweenPosition ObjTweenPos;

	public UIInput ChatInput;

	public RecentSpeakerUILogic RecenSpeakerRoot;

	public UISprite NewPrivateMessagePic;

	public UILabel TranslationLabel;

	public GameObject TranslationRoot;

	public UILabel SendLable;

	public UIButton SendBtn;

	private GameItem SpeakerItem;

	private PlayerData mPlayerDataCache;

	private GameDefine.CHAT_CHANNEL_TYPE mCurChannelType = GameDefine.CHAT_CHANNEL_TYPE.WORLD;

	private GameDefine.CHAT_LINK_TYPE mCurLinkType = GameDefine.CHAT_LINK_TYPE.INVALID;

	private List<long> mLinkLongData = new List<long>();

	private List<string> mLinkStrData = new List<string>();

	private string mLinkText;

	private float mLastSendTime;

	private float SEND_MESSAGE_TIME_INTERVAL = 5f;

	private int MAX_SEND_MESSAHE_COUNT = 256;

	private PlayerData mPlayerData
	{
		get
		{
			if (mPlayerDataCache == null)
			{
				mPlayerDataCache = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			}
			return mPlayerDataCache;
		}
	}

	public GameDefine.CHAT_CHANNEL_TYPE CurChannelType
	{
		get
		{
			return mCurChannelType;
		}
		set
		{
			mCurChannelType = value;
		}
	}

	public static void ResetPrivateChat(long targetId, string targetName, PROFESSION_TYPE targetProfession)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		playerData.RecentSpeakers.Add(targetId, targetName, targetProfession);
		playerData.RecentSpeakers.SetLastSpeaker(targetId);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ChatRoot, OnChatUIShow, GameDefine.CHAT_CHANNEL_TYPE.PRIVATE);
	}

	public static void OnChatUIShow(bool isSuccess, object param)
	{
		if (isSuccess)
		{
			SingletonUnity<ChatUIRootLogic>.Instance.Reset((GameDefine.CHAT_CHANNEL_TYPE)(int)param);
		}
	}

	public void UpdateSendInfo()
	{
		string dictionaryString = StrDictionary.GetDictionaryString("#{100255}");
		if (mCurChannelType == GameDefine.CHAT_CHANNEL_TYPE.WORLD)
		{
			SpeakerItem = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack.GetItemByItemId2("5026");
			int num = 0;
			if (SpeakerItem != null && !SpeakerItem.IsEmpty())
			{
				num = SpeakerItem.StackNum;
			}
			if (num < 0)
			{
				num = 0;
			}
			SendLable.text = $"{dictionaryString}({num})";
			SendBtn.isEnabled = num > 0;
			if (num > 0)
			{
				ChatInput.defaultText = StrDictionary.GetDictionaryString("#{100254}");
			}
			else
			{
				ChatInput.defaultText = StrDictionary.GetDictionaryString("#{100298}");
			}
		}
		else
		{
			SendLable.text = dictionaryString;
			SendBtn.isEnabled = true;
			ChatInput.defaultText = StrDictionary.GetDictionaryString("#{100254}");
		}
	}

	private void OnEnable()
	{
		UIUpdateEvent.UpdateBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.UpdateBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateSendInfo));
	}

	private void OnDisable()
	{
		UIUpdateEvent.UpdateBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.UpdateBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateSendInfo));
	}

	public void Reset(GameDefine.CHAT_CHANNEL_TYPE channelType)
	{
		SetCurChannel(channelType);
		UpdateTranslationState();
		ObjTweenPos.ResetToBeginning();
		ObjTweenPos.PlayForward();
	}

	public static void ResetLinkChat(GameDefine.CHAT_LINK_TYPE linkType, GameDefine.CHAT_CHANNEL_TYPE channelType, object data)
	{
		object[] param = new object[3] { linkType, channelType, data };
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ChatRoot, OnLinkChatUIShow, param);
	}

	public static void OnLinkChatUIShow(bool isSuccess, object param)
	{
		if (isSuccess)
		{
			object[] array = param as object[];
			GameDefine.CHAT_LINK_TYPE cHAT_LINK_TYPE = (GameDefine.CHAT_LINK_TYPE)(int)array[0];
			SingletonUnity<ChatUIRootLogic>.Instance.Reset((GameDefine.CHAT_CHANNEL_TYPE)(int)array[1]);
			switch (cHAT_LINK_TYPE)
			{
			case GameDefine.CHAT_LINK_TYPE.TEAM:
			{
				Team teamInfo = array[2] as Team;
				SingletonUnity<ChatUIRootLogic>.Instance.InsertTeamLink(teamInfo);
				break;
			}
			case GameDefine.CHAT_LINK_TYPE.GUILD:
			{
				Guild guildInfo = array[2] as Guild;
				SingletonUnity<ChatUIRootLogic>.Instance.InsertGuildLink(guildInfo);
				break;
			}
			}
		}
	}

	private void UpdateSelectChange(GameDefine.CHAT_CHANNEL_TYPE channelType)
	{
		for (int i = 0; i < ChannelIconSprite.Count; i++)
		{
			if (i != (int)channelType)
			{
				ChannelIconSprite[i].spriteName = "CZ_huaDongBG";
			}
			else
			{
				ChannelIconSprite[i].spriteName = "CZ_huaDongBG_1";
			}
		}
	}

	private void SetCurChannel(GameDefine.CHAT_CHANNEL_TYPE channelType)
	{
		mCurChannelType = channelType;
		mPlayerData.ChoosedChannelType = mCurChannelType;
		UpdateSelectChange(mCurChannelType);
		if (mCurChannelType == GameDefine.CHAT_CHANNEL_TYPE.SYSTEM)
		{
			UnityVersionUtil.SetActiveRecursive(ChatInput.gameObject, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ChatInput.gameObject, state: true);
		}
		if (mCurChannelType == GameDefine.CHAT_CHANNEL_TYPE.PRIVATE)
		{
			NGUITools.SetActive(RecenSpeakerRoot.gameObject, state: true);
			RecenSpeakerRoot.Reset(mPlayerData.RecentSpeakers.RecentSpeakerList);
			ChatMessageRootLogic.ChatMessageRootPanel.baseClipRegion = new Vector4(0f, 0f, 300f, 370f);
			ChatMessageRootLogic.ChatMessageRootPanel.clipOffset = new Vector2(150f, 205f);
			ChatMessageRootLogic.ChatMessageRoot.transform.localPosition = Vector3.up * 20f;
			ChatMessageRootLogic.ScrollView.ResetPosition();
			OnClickRecentSpeaker(mPlayerData.RecentSpeakers.GetLastSpeaker());
		}
		else
		{
			NGUITools.SetActive(RecenSpeakerRoot.gameObject, state: false);
			ChatMessageRootLogic.ResetChatMessage(mPlayerData.CurChannelChatHistoryList, mCurChannelType);
			ChatMessageRootLogic.ChatMessageRootPanel.baseClipRegion = new Vector4(0f, 0f, 300f, 410f);
			ChatMessageRootLogic.ChatMessageRootPanel.clipOffset = new Vector2(150f, 205f);
			ChatMessageRootLogic.ChatMessageRoot.transform.localPosition = Vector3.zero;
			ChatMessageRootLogic.ScrollView.ResetPosition();
		}
		CheckNewPrivateMessage();
		UpdateSendInfo();
	}

	public void OnClickChannelSystemBtn()
	{
		if (mCurChannelType != 0)
		{
			SetCurChannel(GameDefine.CHAT_CHANNEL_TYPE.SYSTEM);
		}
	}

	public void OnClickChannelNormalBtn()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsSingleCopyScene())
		{
			NoticeLogic.AddNotifyData("#{200089}");
		}
		else if (mCurChannelType != GameDefine.CHAT_CHANNEL_TYPE.NORMAL)
		{
			SetCurChannel(GameDefine.CHAT_CHANNEL_TYPE.NORMAL);
		}
	}

	public void OnClickChannelWorldBtn()
	{
		if (mCurChannelType != GameDefine.CHAT_CHANNEL_TYPE.WORLD)
		{
			SetCurChannel(GameDefine.CHAT_CHANNEL_TYPE.WORLD);
		}
	}

	public void OnClickChannelTeamBtn()
	{
		if (!TeamSendCheck())
		{
			NoticeLogic.AddNotifyData("#{100280}");
		}
		else if (mCurChannelType != GameDefine.CHAT_CHANNEL_TYPE.TEAM)
		{
			SetCurChannel(GameDefine.CHAT_CHANNEL_TYPE.TEAM);
		}
	}

	public void OnClickChannelGuildBtn()
	{
		if (!GuildSendCheck())
		{
			NoticeLogic.AddNotifyData("#{100281}");
		}
		else if (mCurChannelType != GameDefine.CHAT_CHANNEL_TYPE.GUILD)
		{
			SetCurChannel(GameDefine.CHAT_CHANNEL_TYPE.GUILD);
		}
	}

	public void OnClickChannelPrivateBtn()
	{
		if (mCurChannelType != GameDefine.CHAT_CHANNEL_TYPE.PRIVATE)
		{
			SetCurChannel(GameDefine.CHAT_CHANNEL_TYPE.PRIVATE);
		}
	}

	public void OnClickInputSendBtn()
	{
		if (!SendBtn.isEnabled)
		{
			return;
		}
		if (GameManager.IsSupportCurDataVersion177() && !SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.CHAT))
		{
			int condition = DataManager.GetFunctionDataById(4084.ToString()).Condition;
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", condition));
			return;
		}
		string text = ChatInput.value;
		if (string.IsNullOrEmpty(text))
		{
			ClearLinkInfo();
			return;
		}
		if (mCurLinkType == GameDefine.CHAT_LINK_TYPE.INVALID)
		{
			text = text.Replace("\r", " ");
			text = text.Replace("\n", " ");
		}
		if (text.Length > MAX_SEND_MESSAHE_COUNT)
		{
			NoticeLogic.AddNotifyData("#{100279}");
			return;
		}
		if (!CanSendMessage())
		{
			ChatInput.value = string.Empty;
			ClearLinkInfo();
			return;
		}
		if (Time.time - mLastSendTime <= SEND_MESSAGE_TIME_INTERVAL)
		{
			NoticeLogic.AddNotifyData("#{100273}");
			return;
		}
		mLastSendTime = Time.time;
		if (mCurLinkType != GameDefine.CHAT_LINK_TYPE.INVALID && !text.Contains(mLinkText))
		{
			ChatInput.value = string.Empty;
			ClearLinkInfo();
			return;
		}
		chat.request request = new chat.request();
		request.chattype = (long)mCurChannelType;
		request.linktype = (long)mCurLinkType;
		if (mCurLinkType != GameDefine.CHAT_LINK_TYPE.INVALID)
		{
			if (mLinkLongData.Count != 0)
			{
				request.intdata = mLinkLongData;
			}
			if (mLinkStrData.Count != 0)
			{
				request.stringdata = mLinkStrData;
			}
			text = text.Replace(mLinkText, $"[FF0000]{mLinkText}[-]");
		}
		if (mCurChannelType == GameDefine.CHAT_CHANNEL_TYPE.PRIVATE)
		{
			request.tellId = mPlayerData.RecentSpeakers.GetLastSpeaker().ServerId;
		}
		request.chatInfo = text;
		NetLogic.GetInstance().Send<Protocol.chat>(request);
		ChatInput.value = string.Empty;
		ClearLinkInfo();
	}

	private bool CanSendMessage()
	{
		return mCurChannelType switch
		{
			GameDefine.CHAT_CHANNEL_TYPE.NORMAL => NormalSendCheck(), 
			GameDefine.CHAT_CHANNEL_TYPE.WORLD => WorldSendCheck(), 
			GameDefine.CHAT_CHANNEL_TYPE.GUILD => GuildSendCheck(), 
			GameDefine.CHAT_CHANNEL_TYPE.PRIVATE => PrivateSendCheck(), 
			GameDefine.CHAT_CHANNEL_TYPE.TEAM => TeamSendCheck(), 
			_ => false, 
		};
	}

	private bool NormalSendCheck()
	{
		return true;
	}

	private bool WorldSendCheck()
	{
		if (mPlayerData.Level >= GameSettingData.MinPlayerLevelInWorldSpeak)
		{
			return true;
		}
		return false;
	}

	private bool GuildSendCheck()
	{
		return mPlayerData.IsHaveGuild();
	}

	private bool PrivateSendCheck()
	{
		if (mPlayerData.RecentSpeakers.GetLastSpeaker() == null)
		{
			return false;
		}
		return true;
	}

	private bool TeamSendCheck()
	{
		return mPlayerData.IsHaveTeam();
	}

	public void OnClickCloseBtn()
	{
		ObjTweenPos.PlayReverse();
		vp_Timer.In(0.5f, delegate
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ChatRoot);
		});
	}

	public void OnReceiveMessage(PlayerChatHistoryInfo chatInfo)
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			ChatMessageRootLogic.OnReceiveMessage(chatInfo);
		}
	}

	public void OnClickRecentSpeaker(RecentSpeaker speakerInfo)
	{
		if (speakerInfo == null)
		{
			ChatMessageRootLogic.ResetChatMessage(null, GameDefine.CHAT_CHANNEL_TYPE.PRIVATE);
			RecenSpeakerRoot.OnClickBtn(null);
			return;
		}
		speakerInfo.NewMessageFlag = false;
		mPlayerData.RecentSpeakers.SetLastSpeaker(speakerInfo.ServerId);
		RecenSpeakerRoot.OnClickBtn(speakerInfo);
		ChatMessageRootLogic.ResetChatMessage(mPlayerData.ChatHistory.GetHistoryBySenderId(speakerInfo.ServerId), mCurChannelType);
		CheckNewPrivateMessage();
	}

	public void CheckNewPrivateMessage()
	{
		if (mPlayerData.RecentSpeakers.HasNewMessage())
		{
			if (!UnityVersionUtil.IsActive(NewPrivateMessagePic.gameObject))
			{
				NGUITools.SetActive(NewPrivateMessagePic.gameObject, state: true);
			}
		}
		else if (UnityVersionUtil.IsActive(NewPrivateMessagePic.gameObject))
		{
			NGUITools.SetActive(NewPrivateMessagePic.gameObject, state: false);
		}
	}

	public void InsertItemLink(GameItem item)
	{
		ChatInput.value = string.Empty;
		ClearLinkInfo();
		mCurLinkType = GameDefine.CHAT_LINK_TYPE.ITEM;
		mLinkStrData.Add(item.ItemId);
		mLinkText = string.Format("[{0}] ", item.ItemData.MName.Replace(" ", string.Empty));
		ChatInput.value = mLinkText;
	}

	public void InsertEquipLink(GameItem item)
	{
		ChatInput.value = string.Empty;
		ClearLinkInfo();
		mCurLinkType = GameDefine.CHAT_LINK_TYPE.EQUIP;
		mLinkLongData.Add(item.IndexId);
		mLinkText = string.Format("[{0}] ", item.ItemData.MName.Replace(" ", string.Empty));
		ChatInput.value = mLinkText;
	}

	public void InsertTeamLink(Team teamInfo)
	{
		ChatInput.value = string.Empty;
		ClearLinkInfo();
		mCurLinkType = GameDefine.CHAT_LINK_TYPE.TEAM;
		mLinkLongData.Add(teamInfo.TeamID);
		mLinkLongData.Add(teamInfo.MinLimitLevel);
		mLinkLongData.Add(teamInfo.MaxLimitLevel);
		string empty = string.Empty;
		empty = ((teamInfo.TeamGoalData.GoalType != 0) ? DataManager.GetCopySceneDataById(teamInfo.TeamGoalData.CopyId).MName : teamInfo.TeamGoalData.MTitleName);
		mLinkText = StrDictionary.GetDictionaryString("#{100262}", empty);
		ChatInput.value = mLinkText;
	}

	public void InsertGuildLink(Guild guildInfo)
	{
		ChatInput.value = string.Empty;
		ClearLinkInfo();
		mCurLinkType = GameDefine.CHAT_LINK_TYPE.GUILD;
		mLinkLongData.Add(guildInfo.ServerId);
		mLinkText = StrDictionary.GetDictionaryString("#{100261}", guildInfo.GuilName);
		ChatInput.value = mLinkText;
	}

	private void ClearLinkInfo()
	{
		mLinkText = string.Empty;
		mCurLinkType = GameDefine.CHAT_LINK_TYPE.INVALID;
		mLinkLongData.Clear();
		mLinkStrData.Clear();
	}

	private void Update()
	{
	}

	public void OnClickTranslationBtn()
	{
		mPlayerData.IsNeedTranslation = !mPlayerData.IsNeedTranslation;
		LocalDataSaveManager.SetTranslationFlag(mPlayerData.IsNeedTranslation);
		UpdateTranslationState();
		SetCurChannel(mCurChannelType);
		if (SingletonUnity<ChatBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ChatBaseRootLogic>.Instance.gameObject))
		{
			SingletonUnity<ChatBaseRootLogic>.Instance.UpdateMessage();
		}
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateMessage();
		}
	}

	private void UpdateTranslationState()
	{
		if (mPlayerData.IsNeedTranslation)
		{
			TranslationLabel.text = StrDictionary.GetDictionaryString("#{100291}");
		}
		else
		{
			TranslationLabel.text = StrDictionary.GetDictionaryString("#{100292}");
		}
	}
}
