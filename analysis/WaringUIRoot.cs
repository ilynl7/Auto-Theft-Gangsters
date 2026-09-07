using UnityEngine;

public class WaringUIRoot : SingletonUnity<WaringUIRoot>
{
	public UILabel WaringLabel;

	private float mStartTime;

	private float mDuration;

	public void Reset(string waringStr, float duration)
	{
		WaringLabel.text = waringStr;
		mStartTime = Time.time;
		mDuration = duration;
	}

	private void Update()
	{
		if (Time.time - mStartTime >= mDuration)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.WaringUIRoot);
		}
	}
}
