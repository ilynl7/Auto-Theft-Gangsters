using UnityEngine;

public class MessageBoxLogic : SingletonUnity<MessageBoxLogic>
{
	public enum MESSAGEBOXTYPE
	{
		TYPE_OKCANCEL,
		TYPE_OK,
		TYPE_WAIT,
		TYPE_OKCANCEL_WAIT,
		TYPE_CANCEL_WAIT,
		TYPE_OK_ONE
	}

	private class OKCancelInfo
	{
		public string mText;

		public string mTitle;

		public OnYesClick mdelOnYesClick;

		public OnCancelClick mdelOnCancelClick;

		public string mYesStr;

		public string mNoStr;

		public OKCancelInfo(string text, string title = null, OnYesClick delOnYesClick = null, OnCancelClick delOnCancelClick = null, string yesStr = null, string noStr = null)
		{
			mText = text;
			mTitle = title;
			mdelOnYesClick = delOnYesClick;
			mdelOnCancelClick = delOnCancelClick;
			mYesStr = yesStr;
			mNoStr = noStr;
		}
	}

	private class WaitBoxInfo
	{
		public string mTitle;

		public string mText;

		public float mDelay;

		public float mDuration;

		public OnWaitTimeOut mDelWaitOut;

		public WaitBoxInfo(string text, string title, float delay, float duration, OnWaitTimeOut delWaitOut)
		{
			mTitle = title;
			mText = text;
			mDelay = delay;
			mDuration = duration;
			mDelWaitOut = delWaitOut;
		}
	}

	private class OKCancelWaitInfo
	{
		public string mText;

		public string mTitle;

		public float mWaitTIme;

		public OnYesClick mdelOnYesClick;

		public OnCancelClick mdelOnCancelClick;

		public OnWaitTimeOut mdelOnWaitTimeOut;

		public string mYesStr;

		public string mNoStr;

		public OKCancelWaitInfo(string text, string title = null, float waitTime = 0f, OnYesClick delOnYesClick = null, OnCancelClick delOnCancelClick = null, OnWaitTimeOut waitTimeOut = null, string yesStr = null, string noStr = null)
		{
			mText = text;
			mTitle = title;
			mWaitTIme = waitTime;
			mdelOnYesClick = delOnYesClick;
			mdelOnCancelClick = delOnCancelClick;
			mdelOnWaitTimeOut = waitTimeOut;
			mYesStr = yesStr;
			mNoStr = noStr;
		}
	}

	public delegate void OnYesClick();

	public delegate void OnCancelClick();

	public delegate void OnWaitTimeOut();

	private OnYesClick onYesClick;

	private OnCancelClick onCancelClick;

	private OnWaitTimeOut onWaitTimeOut;

	public UILabel mTitleLabel;

	public UILabel mTextLabel;

	public UILabel mTimeLabel;

	public UILabel mYesLabel;

	public UILabel mNoLabel;

	public float mWaitTime = -1f;

	public float mDealyTime = -1f;

	public UIWidget mMessageBoxOkButtonObj;

	public GameObject mMessageBoxCancelButtonObj;

	public GameObject mDetailRootObj;

	public GameObject mCloseBtnObj;

	private Vector3 leftPos = new Vector3(-110f, -115f, 0f);

	private Vector3 midPos = new Vector3(0f, -115f, 0f);

	private Vector3 rightPos = new Vector3(110f, -115f, 0f);

	public void Clear()
	{
		mDealyTime = -1f;
		mDealyTime = -1f;
		onYesClick = null;
		onCancelClick = null;
		onWaitTimeOut = null;
	}

	public void MessageBoxOkOnClick()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MessageBoxUI);
		if (onYesClick != null)
		{
			onYesClick();
		}
		if (SingletonUnity<TutorialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TutorialUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<TutorialUIRootLogic>.Instance.CloseCheck();
		}
	}

	public void MessageBoxCancelOnClick()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MessageBoxUI);
		if (onCancelClick != null)
		{
			onCancelClick();
		}
	}

	public void ShowMessageBox(string title, string text, MESSAGEBOXTYPE messageType, string yesStr = null, string noStr = null)
	{
		mTitleLabel.text = StrDictionary.GetDictionaryString(title);
		mTextLabel.text = StrDictionary.GetDictionaryString(text);
		if (!string.IsNullOrEmpty(yesStr))
		{
			mYesLabel.text = StrDictionary.GetDictionaryString(yesStr);
		}
		else
		{
			mYesLabel.text = StrDictionary.GetDictionaryString("#{100242}");
		}
		if (!string.IsNullOrEmpty(noStr))
		{
			mNoLabel.text = StrDictionary.GetDictionaryString(noStr);
		}
		else
		{
			mNoLabel.text = StrDictionary.GetDictionaryString("#{100243}");
		}
		ShowBox();
		switch (messageType)
		{
		case MESSAGEBOXTYPE.TYPE_OKCANCEL:
			mMessageBoxOkButtonObj.transform.localPosition = rightPos;
			mMessageBoxCancelButtonObj.transform.localPosition = leftPos;
			NGUITools.SetActive(mMessageBoxOkButtonObj.gameObject, state: true);
			NGUITools.SetActive(mMessageBoxCancelButtonObj, state: true);
			NGUITools.SetActive(mTimeLabel.gameObject, state: false);
			NGUITools.SetActive(mCloseBtnObj, state: false);
			break;
		case MESSAGEBOXTYPE.TYPE_OK:
			mMessageBoxOkButtonObj.transform.localPosition = midPos;
			NGUITools.SetActive(mMessageBoxOkButtonObj.gameObject, state: true);
			NGUITools.SetActive(mMessageBoxCancelButtonObj, state: false);
			NGUITools.SetActive(mTimeLabel.gameObject, state: false);
			NGUITools.SetActive(mCloseBtnObj, state: false);
			break;
		case MESSAGEBOXTYPE.TYPE_WAIT:
			NGUITools.SetActive(mMessageBoxOkButtonObj.gameObject, state: false);
			NGUITools.SetActive(mMessageBoxCancelButtonObj, state: false);
			NGUITools.SetActive(mTimeLabel.gameObject, state: true);
			NGUITools.SetActive(mCloseBtnObj, state: false);
			break;
		case MESSAGEBOXTYPE.TYPE_OKCANCEL_WAIT:
			mMessageBoxOkButtonObj.transform.localPosition = rightPos;
			mMessageBoxCancelButtonObj.transform.localPosition = leftPos;
			NGUITools.SetActive(mMessageBoxOkButtonObj.gameObject, state: true);
			NGUITools.SetActive(mMessageBoxCancelButtonObj, state: true);
			NGUITools.SetActive(mTimeLabel.gameObject, state: true);
			NGUITools.SetActive(mCloseBtnObj, state: false);
			break;
		case MESSAGEBOXTYPE.TYPE_CANCEL_WAIT:
			mMessageBoxCancelButtonObj.transform.localPosition = midPos;
			NGUITools.SetActive(mMessageBoxOkButtonObj.gameObject, state: false);
			NGUITools.SetActive(mMessageBoxCancelButtonObj, state: true);
			NGUITools.SetActive(mTimeLabel.gameObject, state: true);
			NGUITools.SetActive(mCloseBtnObj, state: false);
			break;
		case MESSAGEBOXTYPE.TYPE_OK_ONE:
			mMessageBoxOkButtonObj.transform.localPosition = midPos;
			NGUITools.SetActive(mMessageBoxOkButtonObj.gameObject, state: true);
			NGUITools.SetActive(mMessageBoxCancelButtonObj, state: false);
			NGUITools.SetActive(mTimeLabel.gameObject, state: false);
			NGUITools.SetActive(mCloseBtnObj, state: false);
			break;
		}
	}

	public void HideBox()
	{
		UnityVersionUtil.SetActiveRecursive(mDetailRootObj, state: false);
	}

	public void ShowBox()
	{
		UnityVersionUtil.SetActiveRecursive(mDetailRootObj, state: true);
	}

	public static void OpenOKCancelBox(string text, string title, OnYesClick delOnYesClick = null, OnCancelClick delOnCancelClick = null, string yesStr = null, string noStr = null)
	{
		OKCancelInfo param = new OKCancelInfo(text, title, delOnYesClick, delOnCancelClick, yesStr, noStr);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRootNew);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NumRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MessageBoxUI, OnOpenOKCancelBox, param);
	}

	private static void OnOpenOKCancelBox(bool isSucess, object obj)
	{
		if (isSucess)
		{
			OKCancelInfo oKCancelInfo = obj as OKCancelInfo;
			if (SingletonUnity<MessageBoxLogic>.Exists)
			{
				SingletonUnity<MessageBoxLogic>.Instance.Clear();
				SingletonUnity<MessageBoxLogic>.Instance.onYesClick = oKCancelInfo.mdelOnYesClick;
				SingletonUnity<MessageBoxLogic>.Instance.onCancelClick = oKCancelInfo.mdelOnCancelClick;
				SingletonUnity<MessageBoxLogic>.Instance.ShowMessageBox(oKCancelInfo.mTitle, oKCancelInfo.mText, MESSAGEBOXTYPE.TYPE_OKCANCEL, oKCancelInfo.mYesStr, oKCancelInfo.mNoStr);
			}
		}
	}

	public static void OpenOKBox(string text, string title, OnYesClick delOnYesClick = null)
	{
		OKCancelInfo param = new OKCancelInfo(text, title, delOnYesClick);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRootNew);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NumRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MessageBoxUI, OnOpenOKBox, param);
	}

	private static void OnOpenOKBox(bool isSucess, object obj)
	{
		if (isSucess)
		{
			OKCancelInfo oKCancelInfo = obj as OKCancelInfo;
			if (SingletonUnity<MessageBoxLogic>.Exists)
			{
				SingletonUnity<MessageBoxLogic>.Instance.Clear();
				SingletonUnity<MessageBoxLogic>.Instance.onYesClick = oKCancelInfo.mdelOnYesClick;
				SingletonUnity<MessageBoxLogic>.Instance.ShowMessageBox(oKCancelInfo.mTitle, oKCancelInfo.mText, MESSAGEBOXTYPE.TYPE_OK);
			}
		}
	}

	public static void OpenOkBox_One(string text, string title, OnYesClick delOnYesClick = null)
	{
		OKCancelInfo param = new OKCancelInfo(text, title, delOnYesClick);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRootNew);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NumRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MessageBoxUI, OnOpenOKBox_One, param);
	}

	private static void OnOpenOKBox_One(bool isSucess, object obj)
	{
		if (isSucess)
		{
			OKCancelInfo oKCancelInfo = obj as OKCancelInfo;
			if (SingletonUnity<MessageBoxLogic>.Exists)
			{
				SingletonUnity<MessageBoxLogic>.Instance.Clear();
				SingletonUnity<MessageBoxLogic>.Instance.onYesClick = oKCancelInfo.mdelOnYesClick;
				SingletonUnity<MessageBoxLogic>.Instance.ShowMessageBox(oKCancelInfo.mTitle, oKCancelInfo.mText, MESSAGEBOXTYPE.TYPE_OK_ONE);
			}
		}
	}

	public static void OpenWaitBox(string text, string tile, float duration = 0f, float delay = 0f, OnWaitTimeOut delWaitout = null)
	{
		WaitBoxInfo param = new WaitBoxInfo(text, tile, delay, duration, delWaitout);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRootNew);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NumRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MessageBoxUI, OnOpenWaitBox, param);
	}

	private static void OnOpenWaitBox(bool isSucess, object obj)
	{
		if (isSucess && obj is WaitBoxInfo waitBoxInfo)
		{
			SingletonUnity<MessageBoxLogic>.Instance.Clear();
			SingletonUnity<MessageBoxLogic>.Instance.onWaitTimeOut = waitBoxInfo.mDelWaitOut;
			SingletonUnity<MessageBoxLogic>.Instance.mWaitTime = waitBoxInfo.mDuration;
			SingletonUnity<MessageBoxLogic>.Instance.mDealyTime = waitBoxInfo.mDelay;
			SingletonUnity<MessageBoxLogic>.Instance.ShowMessageBox(waitBoxInfo.mTitle, waitBoxInfo.mText, MESSAGEBOXTYPE.TYPE_WAIT);
			if (waitBoxInfo.mDelay > 0f)
			{
				SingletonUnity<MessageBoxLogic>.Instance.HideBox();
			}
		}
	}

	public static void OpenCancelWaitBox(string text, string tile, float waitTime = 0f, OnCancelClick delOnCancelClick = null, OnWaitTimeOut delWaitout = null)
	{
		OKCancelWaitInfo param = new OKCancelWaitInfo(text, tile, waitTime, null, delOnCancelClick, delWaitout);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRootNew);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NumRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MessageBoxUI, OnOpenCancelWaitBox, param);
	}

	private static void OnOpenCancelWaitBox(bool isSucess, object obj)
	{
		if (isSucess && obj is OKCancelWaitInfo oKCancelWaitInfo)
		{
			SingletonUnity<MessageBoxLogic>.Instance.Clear();
			SingletonUnity<MessageBoxLogic>.Instance.onWaitTimeOut = oKCancelWaitInfo.mdelOnWaitTimeOut;
			SingletonUnity<MessageBoxLogic>.Instance.mWaitTime = oKCancelWaitInfo.mWaitTIme;
			SingletonUnity<MessageBoxLogic>.Instance.onYesClick = oKCancelWaitInfo.mdelOnYesClick;
			SingletonUnity<MessageBoxLogic>.Instance.onCancelClick = oKCancelWaitInfo.mdelOnCancelClick;
			SingletonUnity<MessageBoxLogic>.Instance.ShowMessageBox(oKCancelWaitInfo.mTitle, oKCancelWaitInfo.mText, MESSAGEBOXTYPE.TYPE_CANCEL_WAIT);
		}
	}

	public static void OpenOKCancelWaitBox(string text, string title, float waitTime = 0f, OnYesClick delOnYesClick = null, OnCancelClick delOnCancelClick = null, OnWaitTimeOut delWaitout = null, string yesStr = null, string noStr = null)
	{
		OKCancelWaitInfo param = new OKCancelWaitInfo(text, title, waitTime, delOnYesClick, delOnCancelClick, delWaitout, yesStr, noStr);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ItemInfoRootNew);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NumRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MessageBoxUI, OnOpenOKCancelWaitBox, param);
	}

	private static void OnOpenOKCancelWaitBox(bool isSucess, object obj)
	{
		if (isSucess && obj is OKCancelWaitInfo oKCancelWaitInfo)
		{
			SingletonUnity<MessageBoxLogic>.Instance.Clear();
			SingletonUnity<MessageBoxLogic>.Instance.onWaitTimeOut = oKCancelWaitInfo.mdelOnWaitTimeOut;
			SingletonUnity<MessageBoxLogic>.Instance.mWaitTime = oKCancelWaitInfo.mWaitTIme;
			SingletonUnity<MessageBoxLogic>.Instance.onYesClick = oKCancelWaitInfo.mdelOnYesClick;
			SingletonUnity<MessageBoxLogic>.Instance.onCancelClick = oKCancelWaitInfo.mdelOnCancelClick;
			SingletonUnity<MessageBoxLogic>.Instance.ShowMessageBox(oKCancelWaitInfo.mTitle, oKCancelWaitInfo.mText, MESSAGEBOXTYPE.TYPE_OKCANCEL_WAIT, oKCancelWaitInfo.mYesStr, oKCancelWaitInfo.mNoStr);
		}
	}

	public static void CloseBox()
	{
		if (SingletonUnity<MessageBoxLogic>.Exists)
		{
			SingletonUnity<MessageBoxLogic>.Instance.Clear();
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MessageBoxUI);
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
			mTimeLabel.text = ((int)mWaitTime).ToString();
			if (mWaitTime <= 0f)
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MessageBoxUI);
				if (onWaitTimeOut != null)
				{
					onWaitTimeOut();
				}
			}
		}
	}
}
