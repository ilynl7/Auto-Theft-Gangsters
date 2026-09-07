using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SprotoType;

public class PlayerChatHistory
{
	private List<PlayerChatHistoryInfo> mChatHistoryList = new List<PlayerChatHistoryInfo>();

	private List<PlayerChatHistoryInfo>[] mChannelChatHistoryList = new List<PlayerChatHistoryInfo>[8];

	private int[] ChannelHistoryMaxCount = new int[6] { 20, 20, 20, 20, 20, 20 };

	private bool mInitOfflineChatFlag;

	private bool mNewPrivateChatFlag;

	public List<PlayerChatHistoryInfo> ChatHistoryList => mChatHistoryList;

	public List<PlayerChatHistoryInfo>[] ChannelChatHistoryList => mChannelChatHistoryList;

	public bool NewPrivateChatFlag
	{
		get
		{
			return mNewPrivateChatFlag;
		}
		set
		{
			mNewPrivateChatFlag = value;
		}
	}

	public void OnReceiveChatMessage(chat_item chatInfo)
	{
		if (chatInfo == null || chatInfo.chattype >= mChannelChatHistoryList.Length || chatInfo.chattype < 0)
		{
			return;
		}
		List<PlayerChatHistoryInfo> list = mChannelChatHistoryList[chatInfo.chattype];
		if (list.Count >= ChannelHistoryMaxCount[chatInfo.chattype])
		{
			PlayerChatHistoryInfo item = list[0];
			list.RemoveAt(0);
			mChatHistoryList.Remove(item);
		}
		PlayerChatHistoryInfo playerChatHistoryInfo = new PlayerChatHistoryInfo();
		if (chatInfo.HasSenderId)
		{
			playerChatHistoryInfo.SenderServerId = chatInfo.senderId;
		}
		if (chatInfo.HasSenderName)
		{
			playerChatHistoryInfo.SenderName = chatInfo.senderName;
		}
		if (chatInfo.HasTellId)
		{
			playerChatHistoryInfo.TellId = chatInfo.tellId;
		}
		if (chatInfo.HasTellName)
		{
			playerChatHistoryInfo.TellName = chatInfo.tellName;
		}
		if (chatInfo.HasChatInfo)
		{
			playerChatHistoryInfo.ChatInfo = chatInfo.chatInfo;
		}
		if (chatInfo.HasChatInfo2)
		{
			playerChatHistoryInfo.ChatInfo2 = GetFormatStr(chatInfo.chatInfo2);
		}
		else
		{
			playerChatHistoryInfo.ChatInfo2 = chatInfo.chatInfo;
		}
		if (chatInfo.HasChattype)
		{
			playerChatHistoryInfo.ChannelType = (GameDefine.CHAT_CHANNEL_TYPE)chatInfo.chattype;
		}
		if (chatInfo.HasLinktype)
		{
			playerChatHistoryInfo.LinkType = (GameDefine.CHAT_LINK_TYPE)chatInfo.linktype;
		}
		if (chatInfo.HasIntdata)
		{
			for (int i = 0; i < chatInfo.intdata.Count; i++)
			{
				playerChatHistoryInfo.LongData.Add(chatInfo.intdata[i]);
			}
		}
		if (chatInfo.HasStringdata)
		{
			for (int j = 0; j < chatInfo.stringdata.Count; j++)
			{
				playerChatHistoryInfo.StringData.Add(chatInfo.stringdata[j]);
			}
		}
		if (chatInfo.HasSenderProfession)
		{
			playerChatHistoryInfo.SenderProfession = (PROFESSION_TYPE)chatInfo.senderProfession;
		}
		if (chatInfo.HasLevel)
		{
			playerChatHistoryInfo.Level = (int)chatInfo.level;
		}
		if (chatInfo.HasCombValue)
		{
			playerChatHistoryInfo.ComboValue = (int)chatInfo.combValue;
		}
		if (chatInfo.HasGuildId)
		{
			playerChatHistoryInfo.GuildId = chatInfo.guildId;
			playerChatHistoryInfo.GuildName = chatInfo.guildName;
		}
		else
		{
			playerChatHistoryInfo.GuildId = 0L;
			playerChatHistoryInfo.GuildName = string.Empty;
		}
		list.Add(playerChatHistoryInfo);
		ChatHistoryList.Add(playerChatHistoryInfo);
		if (!SingletonDontDestoryUnity<GameManager>.Exists)
		{
			return;
		}
		if (playerChatHistoryInfo.ChannelType == GameDefine.CHAT_CHANNEL_TYPE.PRIVATE)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.RecentSpeakers.OnReceiveMessage(playerChatHistoryInfo);
			if (SingletonUnity<ChatUIRootLogic>.Exists)
			{
				SingletonUnity<ChatUIRootLogic>.Instance.CheckNewPrivateMessage();
			}
		}
		if (playerChatHistoryInfo.ChannelType == SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ChoosedChannelType && SingletonUnity<ChatUIRootLogic>.Exists)
		{
			SingletonUnity<ChatUIRootLogic>.Instance.OnReceiveMessage(playerChatHistoryInfo);
		}
		if (SingletonUnity<ChatBaseRootLogic>.Exists)
		{
			SingletonUnity<ChatBaseRootLogic>.Instance.OnReceiveMessage();
		}
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.OnReceiveMessage();
		}
	}

	public List<PlayerChatHistoryInfo> GetHistoryBySenderId(long targetId)
	{
		List<PlayerChatHistoryInfo> list = new List<PlayerChatHistoryInfo>();
		list.Clear();
		List<PlayerChatHistoryInfo> list2 = mChannelChatHistoryList[5];
		for (int i = 0; i < list2.Count; i++)
		{
			if (list2[i].SenderServerId == targetId || list2[i].TellId == targetId)
			{
				list.Add(list2[i]);
			}
		}
		return list;
	}

	public void Reset()
	{
		mChatHistoryList.Clear();
		for (int i = 0; i < mChannelChatHistoryList.Length; i++)
		{
			if (mChannelChatHistoryList[i] == null)
			{
				mChannelChatHistoryList[i] = new List<PlayerChatHistoryInfo>();
			}
			mChannelChatHistoryList[i].Clear();
		}
		mInitOfflineChatFlag = false;
	}

	public void InitOfflineChat()
	{
		if (!mInitOfflineChatFlag)
		{
			mInitOfflineChatFlag = true;
			req_offline_chat.request rpcReq = new req_offline_chat.request();
			NetLogic.GetInstance().Send<Protocol.req_offline_chat>(rpcReq);
		}
	}

	private string GetFormatStr(string str)
	{
		Regex regex = new Regex("(&#[^;]+;)|([:#] )");
		try
		{
			str = regex.Replace(str, delegate(Match match)
			{
				string text = match.Value.ToString().Replace("&#", string.Empty).Replace(";", string.Empty)
					.Replace("# ", "#")
					.Replace(": ", ":");
				int result;
				return int.TryParse(text, out result) ? Convert.ToChar(result).ToString() : text;
			});
			return str;
		}
		catch (Exception)
		{
			return str;
		}
	}
}
