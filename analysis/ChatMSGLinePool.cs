using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChatMSGLinePool
{
	public const int MAX_MESSAGE_NUM = 10;

	private int mCount;

	public ChatMessageLineLogic MSGPrefab;

	private List<ChatMessageLineLogic> mEnableMsgList = new List<ChatMessageLineLogic>();

	private List<ChatMessageLineLogic> mDisableMsgList = new List<ChatMessageLineLogic>();

	public UIEventListener.VectorDelegate onDrag;

	public List<ChatMessageLineLogic> EnableMsgList => mEnableMsgList;

	public List<ChatMessageLineLogic> DisableMsgList => mDisableMsgList;

	public void Reset(ChatMessageLineLogic prefab = null)
	{
		if (mEnableMsgList.Count > 0)
		{
			for (int num = mEnableMsgList.Count - 1; num >= 0; num--)
			{
				RecycleItem(mEnableMsgList[num]);
			}
		}
		mEnableMsgList.Clear();
		for (int i = 0; i < mDisableMsgList.Count; i++)
		{
			UnityVersionUtil.SetActiveRecursive(mDisableMsgList[i].gameObject, state: false);
		}
		mCount = mDisableMsgList.Count;
		if (prefab != null)
		{
			MSGPrefab = prefab;
			UnityVersionUtil.SetActiveRecursive(MSGPrefab.gameObject, state: false);
		}
	}

	public ChatMessageLineLogic AddItem(bool IsNew)
	{
		ChatMessageLineLogic chatMessageLineLogic = null;
		if (mDisableMsgList.Count > 0)
		{
			chatMessageLineLogic = mDisableMsgList[0];
			mDisableMsgList.RemoveAt(0);
			mEnableMsgList.Add(chatMessageLineLogic);
		}
		else if (mCount < 10)
		{
			mCount++;
			GameObject gameObject = UnityEngine.Object.Instantiate(MSGPrefab.gameObject) as GameObject;
			gameObject.transform.parent = MSGPrefab.transform.parent;
			gameObject.transform.localScale = Vector3.one;
			gameObject.gameObject.name = $"{mEnableMsgList.Count}";
			chatMessageLineLogic = gameObject.GetComponent<ChatMessageLineLogic>();
			UIEventListener messageListener = chatMessageLineLogic.MessageListener;
			messageListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(messageListener.onDrag, new UIEventListener.VectorDelegate(OnDrag));
			mEnableMsgList.Add(chatMessageLineLogic);
		}
		else if (IsNew)
		{
			chatMessageLineLogic = mEnableMsgList[0];
			mEnableMsgList.RemoveAt(0);
			mEnableMsgList.Add(chatMessageLineLogic);
		}
		else
		{
			chatMessageLineLogic = mEnableMsgList[mEnableMsgList.Count - 1];
			mEnableMsgList.RemoveAt(mEnableMsgList.Count - 1);
			mEnableMsgList.Insert(0, chatMessageLineLogic);
		}
		UnityVersionUtil.SetActiveRecursive(chatMessageLineLogic.gameObject, state: true);
		return chatMessageLineLogic;
	}

	public Vector3 GetPreLastMSGPOS()
	{
		if (mEnableMsgList.Count > 1)
		{
			return mEnableMsgList[1].transform.localPosition;
		}
		return Vector3.zero;
	}

	public Vector3 GetPreNewestMSGPOS()
	{
		if (mEnableMsgList.Count > 1)
		{
			return mEnableMsgList[mEnableMsgList.Count - 2].transform.localPosition;
		}
		return Vector3.zero;
	}

	public ChatMessageLineLogic GetLatestMSGLine()
	{
		if (mEnableMsgList.Count > 0)
		{
			return mEnableMsgList[0];
		}
		return null;
	}

	public ChatMessageLineLogic GetPreLatestMSGLine()
	{
		if (mEnableMsgList.Count > 1)
		{
			return mEnableMsgList[1];
		}
		return null;
	}

	public ChatMessageLineLogic GetNewestMSGLine()
	{
		if (mEnableMsgList.Count > 0)
		{
			return mEnableMsgList[mEnableMsgList.Count - 1];
		}
		return null;
	}

	public void RecycleItem(ChatMessageLineLogic curItem)
	{
		mEnableMsgList.Remove(curItem);
		UnityVersionUtil.SetActiveRecursive(curItem.gameObject, state: false);
		mDisableMsgList.Add(curItem);
	}

	private void OnDrag(GameObject obj, Vector2 delta)
	{
		if (onDrag != null)
		{
			onDrag(obj, delta);
		}
	}
}
