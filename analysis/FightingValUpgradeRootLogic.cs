using UnityEngine;

public class FightingValUpgradeRootLogic : SingletonUnity<FightingValUpgradeRootLogic>
{
	public UILabel FightingValLabel;

	public UILabel ChangeValLabel;

	public UITweener[] TweenList;

	public ParticleSystem ParticleEffect;

	private long mPreVal;

	private long mTargetVal;

	private float mChangeTime = 1f;

	private float mLastTime = 2f;

	private float mStartTime;

	private float mChangePercent;

	public static void ShowFinghtingValUpgradeRoot(long preVal, long targetVal)
	{
		if (SingletonUnity<FightingValUpgradeRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FightingValUpgradeRootLogic>.Instance.gameObject))
		{
			SingletonUnity<FightingValUpgradeRootLogic>.Instance.Reset(preVal, targetVal);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.FightingValUpgradeRoot, delegate
		{
			SingletonUnity<FightingValUpgradeRootLogic>.Instance.Reset(preVal, targetVal);
		});
	}

	public void Reset(long preVal, long targetVal)
	{
		SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(17);
		mPreVal = preVal;
		mTargetVal = targetVal;
		FightingValLabel.text = mPreVal.ToString();
		ChangeValLabel.text = (mTargetVal - mPreVal).ToString();
		mStartTime = Time.time;
		mChangePercent = 0f;
		for (int i = 0; i < TweenList.Length; i++)
		{
			TweenList[i].enabled = true;
			TweenList[i].ResetToBeginning();
			TweenList[i].PlayForward();
		}
		ParticleEffect.Clear();
		ParticleEffect.Play(withChildren: true);
	}

	private void Update()
	{
		if (Time.time > mStartTime + mLastTime)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FightingValUpgradeRoot);
			return;
		}
		mChangePercent = (Time.time - mStartTime) / mChangeTime;
		if (mChangePercent >= 1f)
		{
			if (TweenList[0].enabled)
			{
				for (int i = 0; i < TweenList.Length; i++)
				{
					TweenList[i].enabled = false;
				}
				FightingValLabel.text = mTargetVal.ToString();
				FightingValLabel.transform.localScale = Vector3.one;
			}
		}
		else
		{
			FightingValLabel.text = ((int)Mathf.Lerp(mPreVal, mTargetVal, mChangePercent)).ToString();
		}
	}
}
