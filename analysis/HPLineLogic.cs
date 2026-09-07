using UnityEngine;

public class HPLineLogic : MonoBehaviour
{
	private const float mLerpTime = 0.5f;

	private bool mChangeFlag;

	private float mStartLerpTime;

	private float mPreVal;

	private float mTargetVal;

	public UISlider mHpLine;

	public UISprite HPLine;

	private float tempVal;

	private void Update()
	{
		if (mChangeFlag)
		{
			tempVal = (Time.time - mStartLerpTime) / 0.5f;
			SetVal(tempVal);
			if (tempVal >= 1f)
			{
				mChangeFlag = false;
			}
		}
	}

	public void ChangeVal(float hpPercent)
	{
		ForceSetVal(hpPercent);
	}

	public void ForceSetVal(float val)
	{
		mChangeFlag = false;
		mHpLine.value = val;
		if ((double)val < 0.0001)
		{
			HPLine.enabled = false;
		}
		else
		{
			HPLine.enabled = true;
		}
	}

	public void SetVal(float val)
	{
		mHpLine.value = Mathf.Lerp(mPreVal, mTargetVal, val);
		if ((double)mHpLine.value < 0.0001)
		{
			HPLine.enabled = false;
		}
		else
		{
			HPLine.enabled = true;
		}
	}
}
