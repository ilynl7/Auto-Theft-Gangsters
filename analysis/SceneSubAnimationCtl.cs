using System.Collections.Generic;
using UnityEngine;

public class SceneSubAnimationCtl : MonoBehaviour
{
	private int mAnimaIndex;

	private int mAnimaCount;

	private DelegateDefine.NoParamDelegate onAnimaFinished;

	public SceneAnimationObjData CameraAnimationObj;

	public List<SceneAnimationObjData> AnimationObjList;

	public List<ParticleData> ParticleData;

	public List<AnimaSoundData> SoundDataList;

	private float mStartTime;

	private bool[] mHasPlayedList;

	private bool[] mSoundPlayedList;

	private SoundManager soundManager;

	private void OnEnable()
	{
		mStartTime = Time.time;
		mHasPlayedList = new bool[ParticleData.Count];
		for (int i = 0; i < mHasPlayedList.Length; i++)
		{
			mHasPlayedList[i] = false;
		}
		mSoundPlayedList = new bool[SoundDataList.Count];
		for (int j = 0; j < mSoundPlayedList.Length; j++)
		{
			mSoundPlayedList[j] = false;
		}
		soundManager = SingletonDontDestoryUnity<SoundManager>.Instance;
	}

	private void OnDisable()
	{
		if (SingletonDontDestoryUnity<SoundManager>.Exists)
		{
			for (int i = 0; i < SoundDataList.Count; i++)
			{
				soundManager.StopSoundEffect(SoundDataList[i].SoundId);
			}
		}
	}

	private void Update()
	{
		if (!CameraAnimationObj.AnimationObj.IsPlaying(CameraAnimationObj.AnimationNameList[mAnimaIndex]))
		{
			mAnimaIndex++;
			if (mAnimaIndex < mAnimaCount)
			{
				PlayAnimation();
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
				if (onAnimaFinished != null)
				{
					onAnimaFinished();
				}
			}
		}
		if (ParticleData.Count > 0)
		{
			for (int i = 0; i < ParticleData.Count; i++)
			{
				if (!mHasPlayedList[i] && Time.time - mStartTime >= ParticleData[i].ParticleTime)
				{
					ParticleData[i].ParticleObj.Play();
					mHasPlayedList[i] = true;
				}
			}
		}
		if (SoundDataList.Count <= 0)
		{
			return;
		}
		for (int j = 0; j < SoundDataList.Count; j++)
		{
			if (!mSoundPlayedList[j] && Time.time - mStartTime >= SoundDataList[j].SoundTime)
			{
				soundManager.PlaySoundEffect(SoundDataList[j].SoundId);
				mSoundPlayedList[j] = true;
			}
		}
	}

	public void Init(DelegateDefine.NoParamDelegate func)
	{
		mAnimaCount = CameraAnimationObj.AnimationNameList.Count;
		onAnimaFinished = func;
		for (int i = 0; i < AnimationObjList.Count; i++)
		{
			if (string.IsNullOrEmpty(AnimationObjList[i].SubModelId))
			{
				continue;
			}
			AnimationObjList[i].SubModelData = DataManager.GetCharacterModelDataByID(AnimationObjList[i].SubModelId);
			if (AnimationObjList[i].SubModelData.TypeID == 0)
			{
				ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
				if (mainPlayer != null)
				{
					AnimationObjList[i].SubFakeObj = new FakeObjLogic();
					AnimationObjList[i].SubModelData = mainPlayer.CurrentCharacterModelData;
					AnimationObjList[i].SubFakeObj.InitFakeObject(mainPlayer.PartObjId[0], mainPlayer.PartObjId[1], mainPlayer.PartObjId[2], mainPlayer.PartObjId[3], AnimationObjList[i].SubObjRoot, null, "Default");
				}
				else
				{
					AnimationObjList[i].SubFakeObj = new FakeObjLogic();
					AnimationObjList[i].SubFakeObj.InitFakeObject("XD_A_WQ", "XD_A_T", "XD_A_S", "XD_A_X", AnimationObjList[i].SubObjRoot, null, "Default");
				}
			}
			else
			{
				AnimationObjList[i].SubFakeObj = new FakeObjLogic();
				AnimationObjList[i].SubFakeObj.InitAnimaFakeNpcObj(AnimationObjList[i].SubModelId, AnimationObjList[i].SubObjRoot, AnimationObjList[i].SubAniamtionNameList[0]);
			}
		}
	}

	public void StartScene()
	{
		mAnimaIndex = 0;
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		PlayAnimation();
	}

	private void PlayAnimation()
	{
		if (CameraAnimationObj.AnimationSpeedList.Count > mAnimaIndex)
		{
			CameraAnimationObj.AnimationObj[CameraAnimationObj.AnimationNameList[mAnimaIndex]].speed = CameraAnimationObj.AnimationSpeedList[mAnimaIndex];
		}
		CameraAnimationObj.AnimationObj.Play(CameraAnimationObj.AnimationNameList[mAnimaIndex]);
		for (int i = 0; i < AnimationObjList.Count; i++)
		{
			if (!AnimationObjList[i].AnimationObj.IsPlaying(AnimationObjList[i].AnimationNameList[mAnimaIndex]))
			{
				if (AnimationObjList[i].AnimationSpeedList.Count > mAnimaIndex)
				{
					AnimationObjList[i].AnimationObj[AnimationObjList[i].AnimationNameList[mAnimaIndex]].speed = AnimationObjList[i].AnimationSpeedList[mAnimaIndex];
				}
				AnimationObjList[i].AnimationObj.Play(AnimationObjList[i].AnimationNameList[mAnimaIndex]);
			}
			if (AnimationObjList[i].SubAnimationObj != null && !AnimationObjList[i].SubAnimationObj.IsPlaying(AnimationObjList[i].SubAniamtionNameList[mAnimaIndex]))
			{
				if (AnimationObjList[i].AnimationSpeedList.Count > mAnimaIndex)
				{
					AnimationObjList[i].SubAnimationObj[AnimationObjList[i].SubAniamtionNameList[mAnimaIndex]].speed = AnimationObjList[i].AnimationSpeedList[mAnimaIndex];
				}
				AnimationObjList[i].SubAnimationObj.Play(AnimationObjList[i].SubAniamtionNameList[mAnimaIndex]);
			}
			if (AnimationObjList[i].SubObjRoot != null && AnimationObjList[i].SubFakeObj.FakeObj != null)
			{
				AnimationObjList[i].SubFakeObj.PlayAnim(AnimationObjList[i].SubAniamtionNameList[mAnimaIndex], AnimationObjList[i].SubModelData);
			}
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < AnimationObjList.Count; i++)
		{
			if (AnimationObjList[i].SubFakeObj != null)
			{
				AnimationObjList[i].SubFakeObj.StopLoadMesh();
			}
		}
	}
}
