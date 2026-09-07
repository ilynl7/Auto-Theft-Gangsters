using System.Collections.Generic;
using UnityEngine;

public class SendServerCheckManager
{
	public class SendServerCheckData
	{
		private float mLastSendTime;

		private float mSendInterval = 0.5f;

		public SendServerCheckData(float lastTime, float interval = 0.5f)
		{
			mLastSendTime = lastTime;
			mSendInterval = interval;
		}

		public bool CanSendToServer()
		{
			if (Time.time - mLastSendTime > mSendInterval)
			{
				mLastSendTime = Time.time;
				return true;
			}
			return false;
		}
	}

	private Dictionary<int, SendServerCheckData> mCheckEventDic = new Dictionary<int, SendServerCheckData>();

	public Dictionary<int, SendServerCheckData> CheckEventDic
	{
		get
		{
			return mCheckEventDic;
		}
		set
		{
			mCheckEventDic = value;
		}
	}

	public void RegisterCheckEvent(int eventId, float Interval = 0.5f)
	{
		if (!mCheckEventDic.ContainsKey(eventId))
		{
			mCheckEventDic.Add(eventId, new SendServerCheckData(-2.1474836E+09f, Interval));
		}
	}

	public bool CanSendToServer(int eventId, float Interval = 0.5f)
	{
		if (mCheckEventDic.ContainsKey(eventId))
		{
			return mCheckEventDic[eventId].CanSendToServer();
		}
		RegisterCheckEvent(eventId, Interval);
		return mCheckEventDic[eventId].CanSendToServer();
	}

	public void Reset()
	{
	}
}
