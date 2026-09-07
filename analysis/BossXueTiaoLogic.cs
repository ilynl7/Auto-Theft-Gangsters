using UnityEngine;

public class BossXueTiaoLogic : SingletonUnity<BossXueTiaoLogic>
{
	public UILabel levellabel;

	public UILabel namelabel;

	public UISlider oddSlider;

	public UISlider evenSlider;

	public UISprite oddSprite;

	public UISprite evenSprite;

	private float preProgress;

	public UISlider topSlider;

	public UISlider middleSlider;

	public UISprite topSprite;

	public UISprite bottomSprite;

	public float mTopSliderProgress;

	public float mMiddleSliderProgress;

	public int OneLineHPVal = 100;

	public int topDeth = 53;

	public int bottomDeth = 51;

	public UILabel HPVal;

	public Color[] hpColor = new Color[5]
	{
		Color.red,
		new Color(1f, 0.4f, 0f, 1f),
		new Color(1f, 0.733f, 0f, 1f),
		Color.blue,
		new Color(0.07f, 0.718f, 0.004f, 1f)
	};

	private long curColorIndex;

	private void OnEnable()
	{
		topDeth = topSprite.depth;
		bottomDeth = bottomSprite.depth;
	}

	public void SetOneLineVal(int val)
	{
		OneLineHPVal = val;
	}

	public void resetHPinfo(CharacterAttributeData AttributeData)
	{
		levellabel.text = $"Lv.{AttributeData.Level}";
		namelabel.text = AttributeData.Name;
		ChangeHP(AttributeData.HP);
	}

	public void ChangeHP(long curHP)
	{
		if (curHP > 0)
		{
			if (!UnityVersionUtil.IsActive(topSlider.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(topSlider.gameObject, state: true);
			}
			if (!UnityVersionUtil.IsActive(middleSlider.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(middleSlider.gameObject, state: true);
			}
			if (!UnityVersionUtil.IsActive(bottomSprite.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(bottomSprite.gameObject, state: true);
			}
			mTopSliderProgress = ((float)curHP - 0.01f) % (float)OneLineHPVal / (float)OneLineHPVal;
			mMiddleSliderProgress = mTopSliderProgress;
			curColorIndex = (curHP - 1) / OneLineHPVal;
			HPVal.text = curHP.ToString();
			if (preProgress < mTopSliderProgress)
			{
				if (curColorIndex % 2 == 1)
				{
					topSlider = oddSlider;
					evenSlider.value = 1f;
					middleSlider.value = 1f;
					topSprite = oddSprite;
					bottomSprite = evenSprite;
					oddSprite.depth = topDeth;
					evenSprite.depth = bottomDeth;
				}
				else
				{
					topSlider = evenSlider;
					oddSlider.value = 1f;
					middleSlider.value = 1f;
					topSprite = evenSprite;
					bottomSprite = oddSprite;
					oddSprite.depth = bottomDeth;
					evenSprite.depth = topDeth;
				}
			}
			preProgress = mTopSliderProgress;
			if (curColorIndex == 0L)
			{
				if (UnityVersionUtil.IsActive(bottomSprite.gameObject))
				{
					UnityVersionUtil.SetActiveRecursive(bottomSprite.gameObject, state: false);
				}
			}
			else if (!UnityVersionUtil.IsActive(bottomSprite.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(bottomSprite.gameObject, state: true);
			}
			curColorIndex %= 5L;
			topSprite.color = hpColor[curColorIndex];
			bottomSprite.color = hpColor[(curColorIndex - 1 + 5) % 5];
		}
		else
		{
			if (UnityVersionUtil.IsActive(topSlider.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(topSlider.gameObject, state: false);
			}
			if (UnityVersionUtil.IsActive(middleSlider.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(middleSlider.gameObject, state: false);
			}
			if (UnityVersionUtil.IsActive(bottomSprite.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(bottomSprite.gameObject, state: false);
			}
		}
	}

	private void UpdateSliderValue()
	{
		if (topSlider.value > mTopSliderProgress)
		{
			topSlider.value -= Time.deltaTime * 2f;
			if (topSlider.value < mTopSliderProgress)
			{
				topSlider.value = mTopSliderProgress;
			}
		}
		else if (topSlider.value < mTopSliderProgress)
		{
			topSlider.value += Time.deltaTime * 2f;
			if (topSlider.value > mTopSliderProgress)
			{
				topSlider.value = mTopSliderProgress;
			}
		}
		if (middleSlider.value > mMiddleSliderProgress)
		{
			middleSlider.value -= Time.deltaTime / 2f;
			if (middleSlider.value < mMiddleSliderProgress)
			{
				middleSlider.value = mMiddleSliderProgress;
			}
		}
		else if (middleSlider.value < mMiddleSliderProgress)
		{
			middleSlider.value += Time.deltaTime / 2f;
			if (middleSlider.value > mMiddleSliderProgress)
			{
				middleSlider.value = mMiddleSliderProgress;
			}
		}
	}

	private void Update()
	{
		UpdateSliderValue();
	}
}
