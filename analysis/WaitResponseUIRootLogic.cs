using UnityEngine;

public class WaitResponseUIRootLogic : SingletonUnity<WaitResponseUIRootLogic>
{
	private class WaitResponseBoxInfo
	{
		public int mType;

		public string mText;

		public float mDelay;

		public float mDuration;

		public OnWaitTimeOut mDelWaitOut;

		public WaitResponseBoxInfo(int type, float delay, float duration, OnWaitTimeOut delWaitOut)
		{
			mType = type;
			mDelay = delay;
			mDuration = duration;
			mDelWaitOut = delWaitOut;
		}
	}

	public delegate void OnWaitTimeOut();

	public GameObject mDetailObject;

	public float mWaitTime;

	public float mDealyTime;

	private OnWaitTimeOut onWaitTimeOut;

	public void Clear()
	{
		mWaitTime = 0f;
		mDealyTime = -1f;
	}

	public void HideBox()
	{
		if (mDetailObject != null)
		{
			UnityVersionUtil.SetActiveRecursive(mDetailObject, state: false);
		}
	}

	private void ShowBox()
	{
		if (mDetailObject != null)
		{
			UnityVersionUtil.SetActiveRecursive(mDetailObject, state: true);
		}
	}

	public void ResetBoxInfo(int type)
	{
	}

	public static void WaitTimeOut()
	{
		NoticeLogic.AddNotifyData("#{100786}");
	}

	public static void OpenWaitBox(int type, float duration = 10f, float delay = 0f, OnWaitTimeOut delWaitout = null)
	{
		CloseBox();
		WaitResponseBoxInfo param = new WaitResponseBoxInfo(type, delay, duration, delWaitout);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.WaitResponseRoot, OnOpenWaitBox, param);
	}

	private static void OnOpenWaitBox(bool isSucess, object obj)
	{
		if (isSucess && obj is WaitResponseBoxInfo waitResponseBoxInfo)
		{
			SingletonUnity<WaitResponseUIRootLogic>.Instance.Clear();
			SingletonUnity<WaitResponseUIRootLogic>.Instance.onWaitTimeOut = waitResponseBoxInfo.mDelWaitOut;
			if (waitResponseBoxInfo.mDelWaitOut == null)
			{
				SingletonUnity<WaitResponseUIRootLogic>.Instance.onWaitTimeOut = WaitTimeOut;
			}
			SingletonUnity<WaitResponseUIRootLogic>.Instance.mWaitTime = waitResponseBoxInfo.mDuration;
			SingletonUnity<WaitResponseUIRootLogic>.Instance.mDealyTime = waitResponseBoxInfo.mDelay;
			SingletonUnity<WaitResponseUIRootLogic>.Instance.ResetBoxInfo(waitResponseBoxInfo.mType);
			if (waitResponseBoxInfo.mDelay > 0f)
			{
				SingletonUnity<WaitResponseUIRootLogic>.Instance.HideBox();
			}
		}
	}

	public static void CloseBox()
	{
		if (SingletonUnity<WaitResponseUIRootLogic>.Exists)
		{
			SingletonUnity<WaitResponseUIRootLogic>.Instance.Clear();
		}
		if (SingletonUnity<UIManager>.Exists)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.WaitResponseRoot);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (mDealyTime > 0f)
		{
			mDealyTime -= Time.deltaTime;
			if (mDealyTime <= 0f)
			{
				ShowBox();
			}
		}
		else
		{
			if (!(mWaitTime > 0f))
			{
				return;
			}
			mWaitTime -= Time.deltaTime;
			if (mWaitTime <= 0f)
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.WaitResponseRoot);
				if (onWaitTimeOut != null)
				{
					onWaitTimeOut();
				}
			}
		}
	}
}
