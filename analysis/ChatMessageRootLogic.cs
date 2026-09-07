using System;
using System.Collections.Generic;
using UnityEngine;

public class ChatMessageRootLogic : MonoBehaviour
{
	private ChatMSGLinePool mChatMSGLinePool = new ChatMSGLinePool();

	private List<PlayerChatHistoryInfo> mCurHistoryInfoList = new List<PlayerChatHistoryInfo>();

	public UIPanel ChatMessageRootPanel;

	public Transform ChatMessageRoot;

	public UIScrollView ScrollView;

	public bool NeedResetPosFlag;

	public ChatMessageLineLogic MSGLinePrefab;

	public UIScrollBar ScrollBar;

	public UIEventListener ChatBottomEventListener;

	public GameObject NewMessageTipObj;

	public GameDefine.CHAT_CHANNEL_TYPE mCurChannelType;

	private PlayerData mCachePlayerData;

	private int mNewMessageCount;

	private List<ChatMessageLineLogic> EnableMsgList => mChatMSGLinePool.EnableMsgList;

	private PlayerData mPlayerData
	{
		get
		{
			if (mCachePlayerData == null)
			{
				mCachePlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			}
			return mCachePlayerData;
		}
	}

	private int mLatestIndex
	{
		get
		{
			if (mChatMSGLinePool.GetLatestMSGLine() != null)
			{
				return mCurHistoryInfoList.IndexOf(mChatMSGLinePool.GetLatestMSGLine().CurInfo);
			}
			return -1;
		}
	}

	private int mNewestIndex
	{
		get
		{
			if (mChatMSGLinePool.GetNewestMSGLine() != null)
			{
				return mCurHistoryInfoList.IndexOf(mChatMSGLinePool.GetNewestMSGLine().CurInfo);
			}
			return -1;
		}
	}

	private int mCurCanAddPreCount => Mathf.Min(mLatestIndex, 3);

	private int curCanAddNewestCount => Mathf.Min(mCurHistoryInfoList.Count - 1 - mNewestIndex, 3);

	private void Awake()
	{
		UIEventListener chatBottomEventListener = ChatBottomEventListener;
		chatBottomEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(chatBottomEventListener.onDrag, new UIEventListener.VectorDelegate(OnDrag));
		mChatMSGLinePool.Reset(MSGLinePrefab);
		ChatMSGLinePool chatMSGLinePool = mChatMSGLinePool;
		chatMSGLinePool.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(chatMSGLinePool.onDrag, new UIEventListener.VectorDelegate(OnDrag));
	}

	public void AddMessage(PlayerChatHistoryInfo info, bool AddInHead)
	{
		ChatMessageLineLogic chatMessageLineLogic = mChatMSGLinePool.AddItem(AddInHead);
		chatMessageLineLogic.Reset(info);
		if (NeedResetPosFlag)
		{
			ResetMSGLinePos();
		}
		else if (AddInHead)
		{
			if (mChatMSGLinePool.EnableMsgList.Count > 1)
			{
				chatMessageLineLogic.gameObject.transform.localPosition = mChatMSGLinePool.GetPreNewestMSGPOS() - Vector3.up * chatMessageLineLogic.RootWidget.height;
			}
			else
			{
				chatMessageLineLogic.gameObject.transform.localPosition = Vector3.zero;
			}
		}
		else if (mChatMSGLinePool.GetPreLatestMSGLine() != null)
		{
			chatMessageLineLogic.gameObject.transform.localPosition = mChatMSGLinePool.GetPreLastMSGPOS() + Vector3.up * mChatMSGLinePool.GetPreLatestMSGLine().RootWidget.height;
		}
		else
		{
			Debug.Log("Not Enough MSGLine");
		}
		ScrollView.UpdateScrollbars(recalculateBounds: true);
	}

	public void ResetMSGLinePos()
	{
		if (EnableMsgList.Count > 0)
		{
			ChatMessageLineLogic chatMessageLineLogic = null;
			int num = 0;
			for (int i = 0; i < EnableMsgList.Count; i++)
			{
				chatMessageLineLogic = EnableMsgList[EnableMsgList.Count - i - 1];
				chatMessageLineLogic.transform.localPosition = Vector3.up * num;
				num += chatMessageLineLogic.RootWidget.height;
			}
			ScrollView.ResetPosition();
		}
	}

	public void ResetChatMessage(List<PlayerChatHistoryInfo> chatList, GameDefine.CHAT_CHANNEL_TYPE channelType)
	{
		mCurChannelType = channelType;
		UnityVersionUtil.SetActiveRecursive(MSGLinePrefab.gameObject, state: false);
		mCurHistoryInfoList.Clear();
		mChatMSGLinePool.Reset();
		UnityVersionUtil.SetActiveRecursive(NewMessageTipObj, state: false);
		if (chatList == null || chatList.Count <= 0)
		{
			return;
		}
		int num = Mathf.Max(chatList.Count - 10, 0);
		NeedResetPosFlag = false;
		for (int i = 0; i < chatList.Count; i++)
		{
			mCurHistoryInfoList.Add(chatList[i]);
			if (i >= num)
			{
				AddMessage(chatList[i], AddInHead: true);
			}
		}
		NeedResetPosFlag = true;
		mNewMessageCount = 0;
		ResetMSGLinePos();
	}

	public void OnReceiveMessage(PlayerChatHistoryInfo chatInfo)
	{
		if (mCurChannelType == GameDefine.CHAT_CHANNEL_TYPE.PRIVATE)
		{
			if (mPlayerData.RecentSpeakers.GetLastSpeaker() != null)
			{
				if (chatInfo.TellId != mPlayerData.RecentSpeakers.GetLastSpeaker().ServerId && chatInfo.SenderServerId != mPlayerData.RecentSpeakers.GetLastSpeaker().ServerId)
				{
					return;
				}
			}
			else
			{
				Debug.LogError("mPlayerData.RecentSpeakers.GetLastSpeaker() == null");
			}
		}
		mCurHistoryInfoList.Add(chatInfo);
		if (NeedResetPosFlag)
		{
			if (UnityVersionUtil.IsActive(NewMessageTipObj))
			{
				UnityVersionUtil.SetActiveRecursive(NewMessageTipObj, state: false);
			}
			ShowNewMessage();
		}
		else
		{
			mNewMessageCount++;
			if (!UnityVersionUtil.IsActive(NewMessageTipObj))
			{
				UnityVersionUtil.SetActiveRecursive(NewMessageTipObj, state: true);
			}
		}
	}

	private void ShowNewMessage()
	{
		mChatMSGLinePool.Reset();
		int num = Mathf.Max(mCurHistoryInfoList.Count - 10, 0);
		NeedResetPosFlag = false;
		for (int i = 0; i < mCurHistoryInfoList.Count; i++)
		{
			if (i >= num)
			{
				AddMessage(mCurHistoryInfoList[i], AddInHead: true);
			}
		}
		NeedResetPosFlag = true;
		mNewMessageCount = 0;
		ResetMSGLinePos();
	}

	public void Clear()
	{
	}

	public void Preview()
	{
		NeedResetPosFlag = false;
		if (mLatestIndex > 0)
		{
			AddMessage(mCurHistoryInfoList[mLatestIndex - 1], AddInHead: false);
		}
	}

	public void Forward()
	{
		NeedResetPosFlag = false;
		if (mNewestIndex < mCurHistoryInfoList.Count - 1)
		{
			AddMessage(mCurHistoryInfoList[mNewestIndex + 1], AddInHead: true);
			return;
		}
		NeedResetPosFlag = true;
		mNewMessageCount = 0;
		if (UnityVersionUtil.IsActive(NewMessageTipObj))
		{
			UnityVersionUtil.SetActiveRecursive(NewMessageTipObj, state: false);
		}
	}

	private void OnDrag(GameObject obj, Vector2 delta)
	{
		if (delta.y > 0f)
		{
			if (ScrollBar.value > 0.55f)
			{
				Forward();
			}
		}
		else if (ScrollBar.value < 0.45f)
		{
			Preview();
		}
	}
}
