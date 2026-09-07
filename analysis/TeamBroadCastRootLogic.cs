using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class TeamBroadCastRootLogic : SingletonUnity<TeamBroadCastRootLogic>
{
	private bool IsWorldFlag;

	private bool IsNearbyFlag;

	private bool IsGangFlag;

	public UISprite WorldFlag;

	public UISprite NearbyFlag;

	public UISprite GangFlag;

	private string mLinkText;

	private GameDefine.CHAT_LINK_TYPE mCurLinkType = GameDefine.CHAT_LINK_TYPE.INVALID;

	private List<long> mLinkLongData = new List<long>();

	private List<string> mLinkStrData = new List<string>();

	private string ChatInfo = string.Empty;

	private int MAX_SEND_MESSAHE_COUNT = 256;

	private static float LastTime;

	private void OnEnable()
	{
		IsWorldFlag = true;
		IsNearbyFlag = true;
		IsGangFlag = true;
		WorldFlag.enabled = true;
		NearbyFlag.enabled = true;
		GangFlag.enabled = true;
	}

	public void OnClickWorldBtn()
	{
		if (IsWorldFlag)
		{
			WorldFlag.enabled = false;
		}
		else
		{
			WorldFlag.enabled = true;
		}
		IsWorldFlag = !IsWorldFlag;
	}

	public void OnClickNearbyBtn()
	{
		if (IsNearbyFlag)
		{
			NearbyFlag.enabled = false;
		}
		else
		{
			NearbyFlag.enabled = true;
		}
		IsNearbyFlag = !IsNearbyFlag;
	}

	public void OnClickGangBtn()
	{
		if (IsGangFlag)
		{
			GangFlag.enabled = false;
		}
		else
		{
			GangFlag.enabled = true;
		}
		IsGangFlag = !IsGangFlag;
	}

	public void OnClickBroadCast()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveTeam())
		{
			InsertTeamLink(playerData.TeamInfo);
			if (Time.time - LastTime > 5f)
			{
				if (IsWorldFlag)
				{
					SendChatInfo(GameDefine.CHAT_CHANNEL_TYPE.WORLD);
				}
				if (IsNearbyFlag)
				{
					SendChatInfo(GameDefine.CHAT_CHANNEL_TYPE.NORMAL);
				}
				if (IsGangFlag && playerData.IsHaveGuild())
				{
					SendChatInfo(GameDefine.CHAT_CHANNEL_TYPE.GUILD);
				}
			}
		}
		OnClickCloseBtn();
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TeamBroadCastRoot);
	}

	public void InsertTeamLink(Team teamInfo)
	{
		ChatInfo = string.Empty;
		ClearLinkInfo();
		mCurLinkType = GameDefine.CHAT_LINK_TYPE.TEAM;
		mLinkLongData.Add(teamInfo.TeamID);
		mLinkLongData.Add(teamInfo.MinLimitLevel);
		mLinkLongData.Add(teamInfo.MaxLimitLevel);
		string empty = string.Empty;
		empty = ((teamInfo.TeamGoalData.GoalType != 0) ? DataManager.GetCopySceneDataById(teamInfo.TeamGoalData.CopyId).MName : teamInfo.TeamGoalData.MTitleName);
		mLinkText = StrDictionary.GetDictionaryString("#{100262}", empty);
		ChatInfo = mLinkText;
	}

	public void SendChatInfo(GameDefine.CHAT_CHANNEL_TYPE mCurChannelType)
	{
		string text = ChatInfo;
		if (string.IsNullOrEmpty(text))
		{
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
		}
		else
		{
			if (mCurLinkType != GameDefine.CHAT_LINK_TYPE.INVALID && !text.Contains(mLinkText))
			{
				return;
			}
			LastTime = Time.time;
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
			request.chatInfo = text;
			NetLogic.GetInstance().Send<Protocol.chat>(request);
		}
	}

	private void OnDisable()
	{
		ClearLinkInfo();
	}

	private void ClearLinkInfo()
	{
		mLinkText = string.Empty;
		mCurLinkType = GameDefine.CHAT_LINK_TYPE.INVALID;
		mLinkLongData.Clear();
		mLinkStrData.Clear();
	}
}
