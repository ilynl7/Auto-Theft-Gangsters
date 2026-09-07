using System.Collections.Generic;
using System.Text;

public class ChatBaseRootLogic : SingletonUnity<ChatBaseRootLogic>
{
	public int MaxLineCount = 2;

	private bool[] AcceptChannelType = new bool[8] { true, true, true, true, true, true, true, true };

	public UILabel textLabel;

	private List<PlayerChatHistoryInfo> mHistoryListCache;

	private PlayerData mPlayerDataCache;

	private StringBuilder outPutStr = new StringBuilder();

	private StringBuilder tempStr = new StringBuilder();

	private List<PlayerChatHistoryInfo> mHistoryList
	{
		get
		{
			if (mHistoryListCache == null)
			{
				mHistoryListCache = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ChatHistory.ChatHistoryList;
			}
			return mHistoryListCache;
		}
	}

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

	private void Start()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ChatHistory.InitOfflineChat();
	}

	public void OnReceiveMessage()
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			UpdateMessage();
		}
	}

	public void UpdateMessage()
	{
		if (mHistoryList.Count == 0)
		{
			return;
		}
		textLabel.UpdateNGUIText();
		outPutStr.Length = 0;
		int num = 0;
		string finalText = null;
		for (int num2 = mHistoryList.Count - 1; num2 >= 0; num2--)
		{
			if (IsAcceptChannel(mHistoryList[num2].ChannelType))
			{
				NGUIText.WrapText(FormatStr(mHistoryList[num2]), out finalText);
				if (finalText[finalText.Length - 1] != '\n')
				{
					finalText = $"{finalText}\n";
				}
				num += GetLineCount(finalText);
				if (num > MaxLineCount)
				{
					int num3 = num - MaxLineCount;
					if (num3 > 0)
					{
						finalText = finalText.Substring(GetIndexOfCount(finalText, '\n', num3) + 1);
						outPutStr.Insert(0, finalText);
					}
					break;
				}
				outPutStr.Insert(0, finalText);
				if (num == MaxLineCount)
				{
					break;
				}
			}
		}
		if (outPutStr.Length > 0)
		{
			outPutStr.Length -= 1;
		}
		textLabel.text = outPutStr.ToString();
	}

	private int GetIndexOfCount(string str, char val, int count)
	{
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == val)
			{
				count--;
				if (count <= 0)
				{
					return i;
				}
			}
		}
		return -1;
	}

	private int GetLineCount(string str)
	{
		int num = 0;
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == '\n')
			{
				num++;
			}
		}
		return num;
	}

	public string FormatStr(PlayerChatHistoryInfo info)
	{
		tempStr.Length = 0;
		if (mPlayerData.IsNeedTranslation)
		{
			tempStr.AppendFormat("{0} [8FF4F3]{1}[-]: {2}\n", StrDictionary.GetDictionaryString(GameDefine.CHANNEL_PRE_WORD[(int)info.ChannelType]), info.SenderName, info.ChatInfo2);
		}
		else
		{
			tempStr.AppendFormat("{0} [8FF4F3]{1}[-]: {2}\n", StrDictionary.GetDictionaryString(GameDefine.CHANNEL_PRE_WORD[(int)info.ChannelType]), info.SenderName, info.ChatInfo);
		}
		return tempStr.ToString();
	}

	private bool IsAcceptChannel(GameDefine.CHAT_CHANNEL_TYPE type)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData?.IsAcceptChannel(type) ?? true;
	}

	public void SetChannelAccept(GameDefine.CHAT_CHANNEL_TYPE type, bool IsAccept)
	{
		AcceptChannelType[(int)type] = IsAccept;
	}

	public void OnClickOpenChatBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ChatRoot, OnChatRootShow);
	}

	private void OnChatRootShow(bool isSuccess, object param)
	{
		if (isSuccess)
		{
			SingletonUnity<ChatUIRootLogic>.Instance.Reset(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ChoosedChannelType);
		}
	}

	public void OnClickBackPackBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot);
	}

	public void OnClickDanceBtn()
	{
	}

	public void OnClickMailBtn()
	{
		SingletonUnity<SocialUIRootLogic>.Instance.ShowSocialUI();
		SingletonUnity<SocialUIRootLogic>.Instance.OnClickMailBtn();
	}

	public void OnClickSocialBtn()
	{
		SingletonUnity<SocialUIRootLogic>.Instance.ShowSocialUI();
		SingletonUnity<SocialUIRootLogic>.Instance.SelectFriendInfobtn();
	}

	public void ShowOrCloseTips(bool show, GameDefine.TIPS_TYPE type)
	{
	}
}
