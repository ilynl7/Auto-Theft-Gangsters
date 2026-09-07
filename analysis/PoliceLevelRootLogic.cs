public class PoliceLevelRootLogic : SingletonUnity<PoliceLevelRootLogic>
{
	public UISprite[] MaskPic;

	public TweenColor[] TweenStar;

	private int mCurLevel = -9;

	private int mPoliceCarSoundId = 35;

	public void SetPoliceLevel(int level)
	{
		if (mCurLevel == level)
		{
			return;
		}
		mCurLevel = level;
		for (int i = 0; i < MaskPic.Length; i++)
		{
			if (level < 0)
			{
				MaskPic[i].enabled = true;
				TweenStar[i].enabled = false;
				TweenStar[i].ResetToBeginning();
			}
			else if (i <= level)
			{
				MaskPic[i].enabled = false;
				TweenStar[i].enabled = true;
				TweenStar[i].Play();
			}
			else
			{
				MaskPic[i].enabled = true;
				TweenStar[i].enabled = false;
				TweenStar[i].ResetToBeginning();
			}
		}
		if (mCurLevel >= 0)
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(mPoliceCarSoundId);
		}
		else
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.StopSoundEffect(mPoliceCarSoundId);
		}
	}

	private void OnDisable()
	{
		mCurLevel = -9;
		if (SingletonDontDestoryUnity<SoundManager>.Exists)
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.StopSoundEffect(mPoliceCarSoundId);
		}
	}
}
