using System.Collections.Generic;

public class RecentSpeakerRecord
{
	private List<RecentSpeaker> mRecentSpeakerList = new List<RecentSpeaker>();

	private RecentSpeaker mLastSpeaker;

	public List<RecentSpeaker> RecentSpeakerList => mRecentSpeakerList;

	public void Add(long serverId, string name, PROFESSION_TYPE profession)
	{
		if (!IsExist(serverId))
		{
			if (mRecentSpeakerList.Count >= GameSettingData.MaxRecentSpeakerNum)
			{
				mRecentSpeakerList.RemoveAt(0);
			}
			RecentSpeaker recentSpeaker = new RecentSpeaker();
			recentSpeaker.Reset(serverId, name, profession);
			mRecentSpeakerList.Add(recentSpeaker);
		}
	}

	public bool IsExist(long serverId)
	{
		for (int i = 0; i < mRecentSpeakerList.Count; i++)
		{
			if (mRecentSpeakerList[i].ServerId == serverId)
			{
				return true;
			}
		}
		return false;
	}

	public void Reset()
	{
		mRecentSpeakerList.Clear();
		mLastSpeaker = null;
	}

	public void SetLastSpeaker(long speakerId)
	{
		for (int i = 0; i < mRecentSpeakerList.Count; i++)
		{
			if (mRecentSpeakerList[i].ServerId == speakerId)
			{
				mLastSpeaker = mRecentSpeakerList[i];
				break;
			}
		}
	}

	public RecentSpeaker GetLastSpeaker()
	{
		if (mLastSpeaker == null && mRecentSpeakerList.Count > 0)
		{
			mLastSpeaker = mRecentSpeakerList[mRecentSpeakerList.Count - 1];
		}
		return mLastSpeaker;
	}

	public RecentSpeaker GetRecentSpeaker(long serverId)
	{
		for (int i = 0; i < mRecentSpeakerList.Count; i++)
		{
			if (mRecentSpeakerList[i].ServerId == serverId)
			{
				return mRecentSpeakerList[i];
			}
		}
		return null;
	}

	public void OnReceiveMessage(PlayerChatHistoryInfo curChatInfo)
	{
		if (Singleton<ObjManager>.Exists && curChatInfo.SenderServerId != Singleton<ObjManager>.Instance.MainPlayer.ServerId)
		{
			if (!IsExist(curChatInfo.SenderServerId))
			{
				Add(curChatInfo.SenderServerId, curChatInfo.SenderName, curChatInfo.SenderProfession);
			}
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ChoosedChannelType != GameDefine.CHAT_CHANNEL_TYPE.PRIVATE || curChatInfo.SenderServerId != GetLastSpeaker().ServerId)
			{
				RecentSpeaker recentSpeaker = GetRecentSpeaker(curChatInfo.SenderServerId);
				recentSpeaker.NewMessageFlag = true;
			}
			if (SingletonUnity<ChatUIRootLogic>.Exists && SingletonUnity<ChatUIRootLogic>.Instance.CurChannelType == GameDefine.CHAT_CHANNEL_TYPE.PRIVATE)
			{
				SingletonUnity<ChatUIRootLogic>.Instance.RecenSpeakerRoot.Reset(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.RecentSpeakers.RecentSpeakerList);
			}
		}
	}

	public bool HasNewMessage()
	{
		for (int i = 0; i < mRecentSpeakerList.Count; i++)
		{
			if (mRecentSpeakerList[i].NewMessageFlag)
			{
				return true;
			}
		}
		return false;
	}
}
