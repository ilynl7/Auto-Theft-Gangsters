using System.Collections.Generic;
using UnityEngine;

public class LevelUpUIRoot : SingletonUnity<LevelUpUIRoot>
{
	public UILabel[] curLabels;

	public UILabel[] nextLabels;

	private float startTime = -1f;

	public UILabel nextLabel;

	private UITweener[] array;

	public GameObject childObj;

	private int mNextlevel;

	private int mCurLevel = int.MaxValue;

	private int mNextComb;

	private int mCurComb = int.MaxValue;

	private float checkTime = -1f;

	public ParticleSystem particleSystem;

	protected override void Awake()
	{
		base.Awake();
		array = GetComponentsInChildren<UITweener>();
	}

	private void Update()
	{
		if (startTime > 0f && startTime <= Time.time)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LevelUpUIRoot);
			startTime = -1f;
			checkTime = -1f;
		}
		UpdateCheckShow();
	}

	public void CheckShowLevelPack()
	{
		int num = mNextlevel;
		List<LevelPackageData> LevelDataList = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.GetUnGetLevelPack();
		if (LevelDataList != null && LevelDataList.Count > 0)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.LevelRewardGetRoot, delegate
			{
				SingletonUnity<LevelRewardGetLogic>.Instance.EnableReset();
				SingletonUnity<LevelRewardGetLogic>.Instance.ShowRewardList(LevelDataList);
			});
		}
	}

	private bool CanShowLevelUp()
	{
		if ((SingletonUnity<DialogMissionUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DialogMissionUIRoot>.Instance.gameObject)) || (SingletonUnity<MissionPassShowRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MissionPassShowRootLogic>.Instance.gameObject)))
		{
			return false;
		}
		return true;
	}

	private void UpdateCheckShow()
	{
		if (checkTime < 0f)
		{
			return;
		}
		checkTime -= Time.deltaTime;
		if (checkTime <= 0f)
		{
			if (!CanShowLevelUp())
			{
				checkTime = 0.1f;
				return;
			}
			checkTime = -1f;
			PlayAnimal(mCurLevel, mNextlevel);
		}
	}

	private void PlayAnimal(int curLevel, int nextLevel)
	{
		NGUITools.SetActive(childObj, state: true);
		particleSystem.Clear(withChildren: false);
		particleSystem.Play();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ResetToBeginning();
			array[i].PlayForward();
		}
		BaseLvData levelDataByLevel = DataManager.GetLevelDataByLevel(curLevel);
		BaseLvData levelDataByLevel2 = DataManager.GetLevelDataByLevel(nextLevel);
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		startTime = Time.time + 4f;
		nextLabel.text = $"Lv.{nextLevel}";
		for (int j = 0; j < array.Length; j++)
		{
			array[j].ResetToBeginning();
		}
		for (int k = 0; k < array.Length; k++)
		{
			array[k].Play();
		}
		if (playerData.Profession == PROFESSION_TYPE.XD)
		{
			curLabels[0].text = levelDataByLevel.ATKXD.ToString();
			curLabels[1].text = levelDataByLevel.HPXD.ToString();
			curLabels[2].text = levelDataByLevel.DEFXD.ToString();
			curLabels[3].text = levelDataByLevel.HITXD.ToString();
			curLabels[4].text = levelDataByLevel.DGEXD.ToString();
			curLabels[5].text = levelDataByLevel.CRIXD.ToString();
			curLabels[6].text = levelDataByLevel.RESXD.ToString();
			nextLabels[0].text = levelDataByLevel2.ATKXD.ToString();
			nextLabels[1].text = levelDataByLevel2.HPXD.ToString();
			nextLabels[2].text = levelDataByLevel2.DEFXD.ToString();
			nextLabels[3].text = levelDataByLevel2.HITXD.ToString();
			nextLabels[4].text = levelDataByLevel2.DGEXD.ToString();
			nextLabels[5].text = levelDataByLevel2.CRIXD.ToString();
			nextLabels[6].text = levelDataByLevel2.RESXD.ToString();
		}
		else if (playerData.Profession == PROFESSION_TYPE.QJ)
		{
			curLabels[0].text = levelDataByLevel.ATKQJ.ToString();
			curLabels[1].text = levelDataByLevel.HPQJ.ToString();
			curLabels[2].text = levelDataByLevel.DEFQJ.ToString();
			curLabels[3].text = levelDataByLevel.HITQJ.ToString();
			curLabels[4].text = levelDataByLevel.DGEQJ.ToString();
			curLabels[5].text = levelDataByLevel.CRIQJ.ToString();
			curLabels[6].text = levelDataByLevel.RESQJ.ToString();
			nextLabels[0].text = levelDataByLevel2.ATKQJ.ToString();
			nextLabels[1].text = levelDataByLevel2.HPQJ.ToString();
			nextLabels[2].text = levelDataByLevel2.DEFQJ.ToString();
			nextLabels[3].text = levelDataByLevel2.HITQJ.ToString();
			nextLabels[4].text = levelDataByLevel2.DGEQJ.ToString();
			nextLabels[5].text = levelDataByLevel2.CRIQJ.ToString();
			nextLabels[6].text = levelDataByLevel2.RESQJ.ToString();
		}
		else if (playerData.Profession == PROFESSION_TYPE.NQS)
		{
			curLabels[0].text = levelDataByLevel.ATKNQ.ToString();
			curLabels[1].text = levelDataByLevel.HPNQ.ToString();
			curLabels[2].text = levelDataByLevel.DEFNQ.ToString();
			curLabels[3].text = levelDataByLevel.HITNQ.ToString();
			curLabels[4].text = levelDataByLevel.DGENQ.ToString();
			curLabels[5].text = levelDataByLevel.CRINQ.ToString();
			curLabels[6].text = levelDataByLevel.RESNQ.ToString();
			nextLabels[0].text = levelDataByLevel2.ATKNQ.ToString();
			nextLabels[1].text = levelDataByLevel2.HPNQ.ToString();
			nextLabels[2].text = levelDataByLevel2.DEFNQ.ToString();
			nextLabels[3].text = levelDataByLevel2.HITNQ.ToString();
			nextLabels[4].text = levelDataByLevel2.DGENQ.ToString();
			nextLabels[5].text = levelDataByLevel2.CRINQ.ToString();
			nextLabels[6].text = levelDataByLevel2.RESNQ.ToString();
		}
		mCurLevel = nextLevel;
		if (mNextComb != mCurComb)
		{
			FightingValUpgradeRootLogic.ShowFinghtingValUpgradeRoot(mCurComb, mNextComb);
		}
	}

	public void Reset(int curLevel, int nextLevel, int curValue, int nextValue)
	{
		mNextlevel = nextLevel;
		if (curLevel < mCurLevel)
		{
			mCurLevel = curLevel;
		}
		mCurComb = curValue;
		mNextComb = nextValue;
		if (CanShowLevelUp())
		{
			PlayAnimal(mCurLevel, mNextlevel);
			return;
		}
		NGUITools.SetActive(childObj, state: false);
		checkTime = 0.1f;
	}
}
