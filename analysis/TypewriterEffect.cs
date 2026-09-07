using UnityEngine;

[AddComponentMenu("NGUI/Examples/Typewriter Effect")]
[RequireComponent(typeof(UILabel))]
public class TypewriterEffect : MonoBehaviour
{
	public int charsPerSecond = 40;

	private UILabel mLabel;

	private string mText;

	private int mOffset;

	private float mNextChar;

	private bool mReset = true;

	private void OnEnable()
	{
		mReset = true;
	}

	private void Update()
	{
		if (mReset)
		{
			mOffset = 0;
			mReset = false;
			mLabel = GetComponent<UILabel>();
			mText = mLabel.processedText;
		}
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
		}
	}
}
