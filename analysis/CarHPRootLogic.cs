using UnityEngine;

public class CarHPRootLogic : SingletonUnity<CarHPRootLogic>
{
	public UILabel HPLabel;

	public UISlider HPTopLineSlider;

	public UISlider HPBottomLineSlider;

	public UILabel TimeLabel;

	private float mTargetHPPercent;

	private int mRestSec;

	private void Start()
	{
		mTargetHPPercent = 1f;
	}

	private void Update()
	{
		if (HPTopLineSlider.value > mTargetHPPercent)
		{
			HPTopLineSlider.value -= 0.2f * Time.deltaTime;
			HPTopLineSlider.value = Mathf.Max(HPTopLineSlider.value, mTargetHPPercent);
		}
		if (HPBottomLineSlider.value > mTargetHPPercent)
		{
			HPBottomLineSlider.value -= 0.15f * Time.deltaTime;
			HPBottomLineSlider.value = Mathf.Max(HPBottomLineSlider.value, mTargetHPPercent);
		}
	}

	public void ChangeHP(int newHP)
	{
		mTargetHPPercent = (float)newHP / (float)ObjPlayerCar.PLAYERCAR_MAXHP;
	}

	public void UpdateTime(int newTime)
	{
		if (mRestSec != newTime)
		{
			mRestSec = newTime;
			TimeLabel.text = $"{mRestSec / 60}:{mRestSec % 60}";
		}
	}
}
