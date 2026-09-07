using System.Text;
using SprotoType;
using UnityEngine;

public class SexGameUIRootLogic : SingletonUnity<SexGameUIRootLogic>
{
	public UIPanel LinePanel;

	public UILabel ScoresLabel;

	public UILabel TimeLabel;

	public UILabel ComboLabel;

	public TweenScale ComboRoot;

	public TweenPosition ComboSubObj;

	public TweenAlpha TouchRoot;

	public UISprite LinePic;

	public UISprite CZPosPic;

	public GameObject ScoresResultRoot;

	public UILabel ScoresResultLabel;

	public Transform BottomPosObj;

	public float TimeLimit;

	public float AddScores;

	public float MaxScoresAddRate;

	public float ScoresAddRate;

	public int ComboAddRateNum;

	public float MaxGameVal;

	public float GameAddVal;

	public float GameReduceVal;

	public float MinCZVal;

	public float MaxCZVal;

	public float MinScoreCZVal;

	public float MaxScoreCZVal;

	public int NormalManSound;

	public int ComboManSound;

	public int NormalWomenSound;

	public int ComboWomenSound;

	public int SoundClickNum;

	private float mCurScores;

	private float mCurScoresRate;

	private float mCurRestTime;

	private float mCurGameVal;

	private int mCurComboVal;

	private float PicLength;

	private Transform SexGameCamPos;

	private ParticleSystem SexGameParticleEffect;

	private bool mIsMan;

	private string activityId;

	private NpcData curNpcData;

	private int clickCount;

	private float screenWidth;

	private float mCurShowScores;

	private float mCurShowGameVal;

	private float changeValTime;

	private StringBuilder mScoresSb = new StringBuilder(512);

	private StringBuilder mTimeSb = new StringBuilder(512);

	private float curLinePercent;

	private bool mFinishFlag;

	private bool mStartFlag;

	private bool mPauseFlag;

	private float clickStartTime;

	public void Reset(bool isMan, string actId, NpcData npcData)
	{
		mCurScores = 0f;
		mCurScoresRate = 1f;
		mCurRestTime = TimeLimit;
		mCurGameVal = 0f;
		PicLength = LinePic.height;
		CZPosPic.transform.localPosition = new Vector3(0f, PicLength * (MinCZVal / MaxGameVal), 0f);
		CZPosPic.width = Mathf.RoundToInt(PicLength * (MaxCZVal - MinCZVal) / MaxGameVal);
		BottomPosObj.localPosition = new Vector3(-8f, PicLength * (MinCZVal / MaxGameVal), 0f);
		NGUITools.SetActive(TouchRoot.gameObject, state: false);
		NGUITools.SetActive(ComboRoot.gameObject, state: false);
		NGUITools.SetActive(ScoresResultRoot, state: false);
		mFinishFlag = false;
		mStartFlag = false;
		mPauseFlag = false;
		SexGameCamPos = GameObject.Find("DSJ_zhuCheng/SexGamCamPos").transform;
		SexGameCamPos.animation.Play();
		SexGameParticleEffect = GameObject.Find("DSJ_zhuCheng").transform.FindChild("SexGameParticleEffect").gameObject.GetComponent<ParticleSystem>();
		UnityVersionUtil.SetActiveRecursive(SexGameParticleEffect.gameObject, state: true);
		SexGameParticleEffect.startSpeed = 5f;
		SexGameParticleEffect.emissionRate = 10f;
		Singleton<ObjManager>.Instance.MainPlayer.CameraController.LerpToTargetLocalZero(SexGameCamPos, 1f);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CarMissionStartTimeCountRoot, delegate
		{
			SingletonUnity<CarMissionStartTimeCountRoot>.Instance.Reset(Time.time, StartGame);
		});
		mIsMan = isMan;
		activityId = actId;
		TimeLabel.text = TimeTools.GetMinuteSecondStr(Mathf.FloorToInt(mCurRestTime));
		curNpcData = npcData;
		clickCount = 0;
		screenWidth = Mathf.RoundToInt(480f * ((float)Screen.width / (float)Screen.height));
	}

	private void StartGame()
	{
		mStartFlag = true;
	}

	public void PauseGame()
	{
		mPauseFlag = true;
	}

	public void ResumeGame()
	{
		mPauseFlag = false;
	}

	public void OnClickExitBtn()
	{
		if (mFinishFlag)
		{
			return;
		}
		PauseGame();
		MessageBoxLogic.OpenOKCancelBox("#{102001}", "#{100127}", delegate
		{
			if (SingletonUnity<CarMissionStartTimeCountRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CarMissionStartTimeCountRoot>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CarMissionStartTimeCountRoot);
			}
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SexGameUIRoot);
			Singleton<ObjManager>.Instance.MainPlayer.CameraController.LerpBackToPlayer(1f);
			UnityVersionUtil.SetActiveRecursive(SexGameParticleEffect.gameObject, state: false);
			StoryDialogRootLogic.ShowStory("107", curNpcData);
		}, delegate
		{
			ResumeGame();
		});
	}

	public void OnClickScreen()
	{
		if (mFinishFlag || !mStartFlag || mPauseFlag)
		{
			return;
		}
		clickCount++;
		ShowTouch();
		mCurGameVal += GameAddVal;
		mCurGameVal = ((!(mCurGameVal > MaxGameVal)) ? mCurGameVal : MaxGameVal);
		if (mCurGameVal > MinScoreCZVal && mCurGameVal < MaxScoreCZVal)
		{
			if (mCurGameVal > MinCZVal && mCurGameVal < MaxCZVal)
			{
				mCurComboVal++;
				ShowCombo();
				mCurScoresRate = 1f + ScoresAddRate * (float)(mCurComboVal / ComboAddRateNum);
				mCurScoresRate = ((!(mCurScoresRate > MaxScoresAddRate)) ? mCurScoresRate : MaxScoresAddRate);
				mCurScores += AddScores * mCurScoresRate;
				if (clickCount >= SoundClickNum)
				{
					clickCount = 0;
					PlayComboSound();
				}
			}
			else
			{
				HideCombo();
				mCurScores += AddScores;
				if (clickCount >= SoundClickNum)
				{
					clickCount = 0;
					PlayNormalSound();
				}
			}
		}
		else
		{
			HideCombo();
		}
		changeValTime = 1f;
	}

	private void PlayNormalSound()
	{
		if (mIsMan)
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(NormalManSound);
		}
		else
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(NormalWomenSound);
		}
	}

	private void PlayComboSound()
	{
		if (mIsMan)
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(ComboManSound);
		}
		else
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(ComboWomenSound);
		}
	}

	private void ShowCombo()
	{
		NGUITools.SetActive(ComboRoot.gameObject, state: true);
		ComboRoot.ResetToBeginning();
		ComboRoot.PlayForward();
		ComboSubObj.ResetToBeginning();
		ComboSubObj.PlayForward();
		ComboLabel.text = $"[i]{mCurComboVal}[/i]";
		SexGameParticleEffect.startSpeed = -5f;
		SexGameParticleEffect.emissionRate = 30f;
		clickStartTime = float.MaxValue;
	}

	public void DisableCombat()
	{
		NGUITools.SetActive(ComboRoot.gameObject, state: false);
	}

	private void HideCombo()
	{
		mCurComboVal = 0;
		NGUITools.SetActive(ComboRoot.gameObject, state: false);
		clickStartTime = Time.time;
		SexGameParticleEffect.startSpeed = 0f;
		SexGameParticleEffect.emissionRate = 10f;
	}

	private void ShowTouch()
	{
		NGUITools.SetActive(TouchRoot.gameObject, state: true);
		TouchRoot.ResetToBeginning();
		TouchRoot.PlayForward();
		TouchRoot.transform.localPosition = new Vector3(UICamera.lastTouchPosition.x / (float)Screen.width * screenWidth, UICamera.lastTouchPosition.y / (float)Screen.height * 480f, 0f);
	}

	private void Update()
	{
		if (!mStartFlag || mPauseFlag)
		{
			return;
		}
		mCurRestTime -= Time.deltaTime;
		if (mCurRestTime > 0f)
		{
			TimeLabel.text = TimeTools.GetMinuteSecondStr(Mathf.FloorToInt(mCurRestTime));
			if (changeValTime > 0f)
			{
				changeValTime -= Time.deltaTime * 3f;
				mCurShowScores = Mathf.FloorToInt(Mathf.Lerp(mCurScores, mCurShowScores, changeValTime));
				mScoresSb.Length = 0;
				mScoresSb.AppendFormat("{0}", mCurShowScores);
				ScoresLabel.text = mScoresSb.ToString();
			}
			else
			{
				changeValTime = 0f;
			}
			mCurShowGameVal = Mathf.FloorToInt(Mathf.Lerp(mCurGameVal, mCurShowGameVal, changeValTime));
			curLinePercent = mCurShowGameVal / MaxGameVal;
			LinePanel.SetRect(0f, curLinePercent * PicLength / 2f, 75f, curLinePercent * PicLength);
			mCurGameVal -= Time.deltaTime * GameReduceVal;
			if (mCurGameVal < 0f)
			{
				mCurGameVal = 0f;
			}
			if (mCurGameVal < MinCZVal && SexGameParticleEffect.startSpeed < 0f)
			{
				HideCombo();
			}
		}
		else if (!mFinishFlag)
		{
			mFinishFlag = true;
			FinishGame();
		}
		if (Time.time - clickStartTime > 0.3f)
		{
			SexGameParticleEffect.startSpeed = 5f;
			SexGameParticleEffect.emissionRate = 10f;
			clickStartTime = float.MaxValue;
		}
	}

	public void FinishGame()
	{
		NGUITools.SetActive(ScoresResultRoot, state: true);
		NGUITools.SetActive(ComboRoot.gameObject, state: false);
		ScoresResultLabel.text = $"[i]{mCurScores}[/i]";
		update_sex_mini_score.request request = new update_sex_mini_score.request();
		request.id = activityId;
		request.score = (long)mCurScores;
		NetLogic.GetInstance().Send<Protocol.update_sex_mini_score>(request);
		vp_Timer.In(2f, delegate
		{
			if (SingletonUnity<CarMissionStartTimeCountRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CarMissionStartTimeCountRoot>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CarMissionStartTimeCountRoot);
			}
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SexGameUIRoot);
			Singleton<ObjManager>.Instance.MainPlayer.CameraController.LerpBackToPlayer(1f);
			UnityVersionUtil.SetActiveRecursive(SexGameParticleEffect.gameObject, state: false);
			StoryDialogRootLogic.ShowStory("107", curNpcData);
		});
		SexGameCamPos.animation.Stop();
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.CurActivityDataDic[activityId].CurNum++;
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", "activity_8", "finish");
	}
}
