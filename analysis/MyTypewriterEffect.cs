using UnityEngine;

public class MyTypewriterEffect : MonoBehaviour
{
	public int charsPerSecond = 40;

	private UILabel mLabel;

	private string mText;

	private int mOffset;

	private float mNextChar;

	private bool mReset = true;

	private DelegateDefine.NoParamDelegate onFinished;

	private void OnEnable()
	{
		mReset = true;
	}

	private void Update()
	{
		if (mOffset < mText.Length && mNextChar <= RealTime.time)
		{
			charsPerSecond = Mathf.Max(1, charsPerSecond);
			float num = 1f / (float)charsPerSecond;
			char c = mText[mOffset];
			if (c == '.' || c == '\n' || c == '!' || c == '?')
			{
				num *= 4f;
			}
			NGUIText.ParseSymbol(mText, ref mOffset);
			mNextChar = RealTime.time + num;
			mLabel.text = mText.Substring(0, ++mOffset);
			if (mOffset >= mText.Length && onFinished != null)
			{
				onFinished();
			}
		}
	}

	public void ShowAll()
	{
		mLabel.text = mText;
		mOffset = mText.Length;
		if (onFinished != null)
		{
			onFinished();
		}
	}

	public void Reset(string str, DelegateDefine.NoParamDelegate func)
	{
		if (mLabel == null)
		{
			mLabel = GetComponent<UILabel>();
		}
		mOffset = 0;
		mText = str;
		onFinished = func;
	}
}
