using UnityEngine;

public class CarBestTimeCountRoot : SingletonUnity<CarBestTimeCountRoot>
{
	public UILabel BestTimeLabel;

	public UILabel CurTimeLabel;

	public UILabel SpeedLabel;

	private bool mEnableTimeCountFlag;

	private float mStartTime;

	private Color green = new Color(0.7607843f, 1f, 0f, 1f);

	private Color yellow = new Color(1f, 11f / 15f, 0f, 1f);

	private Color oriange = new Color(1f, 0.5254902f, 0f, 1f);

	private Color red = new Color(1f, 16f / 85f, 0f, 1f);

	private int mTempTime;

	private int mCurTime;

	public void Reset(int bestTime)
	{
		if (bestTime == -1)
		{
			BestTimeLabel.text = $"-- -- --";
		}
		else
		{
			BestTimeLabel.text = TimeTools.GetCentiSecondStr(bestTime);
		}
		CurTimeLabel.text = TimeTools.GetCentiSecondStr(0);
		mEnableTimeCountFlag = false;
		SetSpeedLable(0);
	}

	public void SetSpeedLable(int speed)
	{
		if (speed < 0)
		{
			speed = 0;
		}
		SpeedLabel.text = $"{speed}";
		if (speed <= 50)
		{
			SpeedLabel.color = green;
		}
		else if (speed <= 100)
		{
			SpeedLabel.color = yellow;
		}
		else if (speed <= 150)
		{
			SpeedLabel.color = oriange;
		}
		else
		{
			SpeedLabel.color = red;
		}
	}

	public void EnableTimeCount(float startTime)
	{
		mEnableTimeCountFlag = true;
		mStartTime = startTime;
	}

	public void DisableTimeCount()
	{
		mEnableTimeCountFlag = false;
	}

	private void Update()
	{
		if (mEnableTimeCountFlag)
		{
			mTempTime = (int)((Time.time - mStartTime) * 100f);
			if (mCurTime != mTempTime)
			{
				mCurTime = mTempTime;
				CurTimeLabel.text = TimeTools.GetCentiSecondStr(mCurTime);
			}
		}
	}
}
