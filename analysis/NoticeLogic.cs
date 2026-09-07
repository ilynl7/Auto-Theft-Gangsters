using System.Collections.Generic;
using UnityEngine;

public class NoticeLogic : MonoBehaviour
{
	public class NoticeData
	{
		public string Str;

		public bool IsWarning;

		public NoticeData(string str, bool isWarning = false)
		{
			Str = str;
			IsWarning = isWarning;
		}
	}

	private static List<NoticeData> notifyDataList = new List<NoticeData>();

	public UILabel[] mNewLabels;

	public GameObject[] mNewsObject;

	public UISprite[] mNewsBottomPic;

	private Transform[] mObjTransform = new Transform[3];

	private float[] mfNewTimes = new float[3];

	private int mTimeMax = 5;

	public static void AddNotifyData(string data, bool isWarning = true, bool isFilterRepeate = false)
	{
		string text = string.Empty;
		if (!string.IsNullOrEmpty(data))
		{
			char c = data[0];
			text = ((c == '#') ? StrDictionary.GetServerDictionaryString(data) : data);
		}
		NoticeData item = new NoticeData(text, isWarning);
		if (notifyDataList.Count > 0 && isFilterRepeate)
		{
			if (notifyDataList[notifyDataList.Count - 1].Str != text)
			{
				notifyDataList.Add(item);
			}
		}
		else
		{
			notifyDataList.Add(item);
		}
	}

	public static void AddNotifyData2Client(bool isFilterRepeate, string data, bool isWarning = false, params object[] args)
	{
		string text = string.Empty;
		if (!string.IsNullOrEmpty(data))
		{
			char c = data[0];
			text = ((c == '#') ? StrDictionary.GetClientDictionaryString(data, args) : data);
		}
		NoticeData item = new NoticeData(text, isWarning);
		if (notifyDataList.Count > 0 && isFilterRepeate)
		{
			if (notifyDataList[notifyDataList.Count - 1].Str != text)
			{
				notifyDataList.Add(item);
			}
		}
		else
		{
			notifyDataList.Add(item);
		}
	}

	public static NoticeData GetNotifyData()
	{
		NoticeData result = null;
		if (notifyDataList.Count > 0)
		{
			result = notifyDataList[0];
			notifyDataList.RemoveAt(0);
		}
		return result;
	}

	private void Start()
	{
		for (int i = 0; i < mNewsObject.Length; i++)
		{
			UnityVersionUtil.SetActiveRecursive(mNewsObject[i], state: false);
			mfNewTimes[i] = Time.realtimeSinceStartup;
			mObjTransform[i] = mNewsObject[i].transform;
		}
	}

	private void AddNotice(NoticeData notice)
	{
		for (int i = 0; i < mObjTransform.Length; i++)
		{
			Vector3 localPosition = mObjTransform[i].localPosition;
			localPosition.y += 32f;
			if (localPosition.y > 214f)
			{
				localPosition.y = 150f;
				mNewLabels[i].text = notice.Str;
				UnityVersionUtil.SetActiveRecursive(mNewsObject[i], state: true);
				mfNewTimes[i] = Time.realtimeSinceStartup;
				mNewsBottomPic[i].color = new Color(0f, 0f, 0f, 0.7f);
			}
			mObjTransform[i].localPosition = localPosition;
		}
	}

	private void Update()
	{
		NoticeData notifyData = GetNotifyData();
		if (notifyData != null)
		{
			AddNotice(notifyData);
		}
		for (int i = 0; i < mNewsObject.Length; i++)
		{
			if (UnityVersionUtil.IsActive(mNewsObject[i]) && Time.realtimeSinceStartup - mfNewTimes[i] > (float)mTimeMax)
			{
				mfNewTimes[i] = Time.realtimeSinceStartup;
				UnityVersionUtil.SetActiveRecursive(mNewsObject[i], state: false);
			}
		}
	}
}
