using UnityEngine;

public class CarPointBestTimeRoot : SingletonUnity<CarPointBestTimeRoot>
{
	public UILabel CurTimeLabel;

	public UILabel BestTimeDifLabel;

	public GameObject BestTimeRoot;

	public void Reset(int curTime, int BestTime)
	{
		if (BestTime == -1)
		{
			NGUITools.SetActive(BestTimeRoot, state: false);
		}
		else
		{
			NGUITools.SetActive(BestTimeRoot, state: true);
			if (BestTime < curTime)
			{
				BestTimeDifLabel.color = Color.red;
				BestTimeDifLabel.text = $"+{TimeTools.GetCentiSecondStr(curTime - BestTime)}";
			}
			else
			{
				BestTimeDifLabel.color = Color.green;
				BestTimeDifLabel.text = $"-{TimeTools.GetCentiSecondStr(BestTime - curTime)}";
			}
		}
		CurTimeLabel.text = TimeTools.GetCentiSecondStr(curTime);
		vp_Timer.In(2f, delegate
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CarPointBestTimeRoot);
		});
	}
}
