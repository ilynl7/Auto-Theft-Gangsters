using System.Text;
using UnityEngine;

public class DownloadResTipRootLogic : SingletonUnity<DownloadResTipRootLogic>
{
	public GameObject ProgressLineAnimaObj;

	public GameObject[] StarPic;

	public UILabel StepTargetLabel;

	private string[] targetWords = new string[3] { "#{600067}", "#{600067}", "#{600068}" };

	private float startDownloadTime;

	private float downloadProgress;

	private float[] statePercent = new float[4] { 0.3f, 0.65f, 1f, 1f };

	private int mTutorialStep = -1;

	public UISprite ProgressLinePic;

	private int mProgressLineWidth = -1;

	private int mProgressTargetWidth;

	private string mCurLocalVersion = string.Empty;

	private string mCurApkVersion = string.Empty;

	private FileUpdateHelper updateHelper;

	private UPDATE_STEP mLastUpdateStep;

	private float mNeedDownLoadSize;

	private string mDownLoadUnit;

	private bool autoContinueFlag = true;

	private float curProgress;

	private string[] TitleLocIdList = new string[5] { "#{101511}", "#{101512}", "#{101516}", "#{101518}", "#{101519}" };

	private string[] TextLocIdList = new string[5] { "#{101601}", "#{101603}", "#{101613}", "#{101615}", "#{101617}" };

	private float TurnPageTime = 8f;

	private int mCurIndex;

	private float mLastTurnPageTime;

	private StringBuilder percentSb = new StringBuilder(512);

	private string percentFormat = "{0}";

	private StringBuilder downloadSb = new StringBuilder(512);

	private string downloadFormat = "{0:N1}/{1:N1}{2}";

	public bool DownLoadFinishFlag;

	private float timeCount;

	public float StartDownloadTime => startDownloadTime;

	public float DownloadProgress => downloadProgress;

	public FileUpdateHelper UpdateHelper => updateHelper;

	public void UpdateTutorialStep(int step, float percent)
	{
		if (step > 0 && step < statePercent.Length)
		{
			SetProgressLineVal(percent * (statePercent[step] - statePercent[step - 1]) + statePercent[step - 1]);
			if (mTutorialStep != step)
			{
				if (step >= targetWords.Length)
				{
					NGUITools.SetActive(StepTargetLabel.gameObject, state: false);
				}
				else
				{
					NGUITools.SetActive(StepTargetLabel.gameObject, state: true);
					StepTargetLabel.text = StrDictionary.GetDictionaryString(targetWords[step]);
				}
			}
		}
		else
		{
			SetProgressLineVal(percent * statePercent[step]);
			if (mTutorialStep != step)
			{
				StepTargetLabel.text = StrDictionary.GetDictionaryString(targetWords[step]);
			}
		}
		if (mTutorialStep < step)
		{
			for (int i = 0; i < StarPic.Length; i++)
			{
				if (i + 1 <= step)
				{
					NGUITools.SetActive(StarPic[i], state: true);
				}
				else
				{
					NGUITools.SetActive(StarPic[i], state: false);
				}
			}
		}
		mTutorialStep = step;
	}

	public void UpdateTutorialStep(float percent)
	{
		SetProgressLineVal(percent);
		for (int i = 0; i < statePercent.Length - 1; i++)
		{
			if (percent + float.Epsilon > statePercent[i])
			{
				if (!UnityVersionUtil.IsActive(StarPic[i]))
				{
					NGUITools.SetActive(StarPic[i], state: true);
				}
			}
			else if (UnityVersionUtil.IsActive(StarPic[i]))
			{
				NGUITools.SetActive(StarPic[i], state: false);
			}
		}
	}

	public void OnClickDownLoadBtn()
	{
		StartDownload();
		if (SingletonDontDestoryUnity<GameManager>.Instance.IsNeedCountDownload)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewDownload", "BackGroundDownloadStart");
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewDownload", "StartTimes");
			LocalDataSaveManager.SetDownloadCountFinish();
		}
		startDownloadTime = Time.realtimeSinceStartup;
	}

	private new void Awake()
	{
		base.Awake();
		DownLoadFinishFlag = false;
		ProgressLinePic.width = UIWidgetControl.GetFitWidth(ProgressLinePic.width);
		ProgressLinePic.transform.localPosition = new Vector3(-ProgressLinePic.width / 2, 0f, 0f);
		mProgressLineWidth = ProgressLinePic.width;
		SetProgressLineWidth(0);
		if (updateHelper == null)
		{
			GameObject gameObject = new GameObject();
			UnityVersionUtil.SetActiveRecursive(gameObject, state: true);
			updateHelper = gameObject.AddComponent<FileUpdateHelper>();
		}
		PlayerData.downLoadFlag = 1;
		BeginDownload();
		downloadProgress = 0f;
		autoContinueFlag = true;
		UpdateTutorialStep(0, 0f);
	}

	private void SetProgressLineWidth(int width)
	{
		ProgressLinePic.width = width;
		ProgressLineAnimaObj.transform.localPosition = new Vector3(width, 0f, 0f);
	}

	private void BeginDownload()
	{
		if (GameSettingData.GetPhoneClass() != 0)
		{
			updateHelper.StartCheckRes(updateHelper.mServerUrl, OnChangeUpdateStep, isNeedSameVersion: true, isNeedCopyRes: false);
		}
	}

	public void OnChangeUpdateStep(UPDATE_STEP newStep)
	{
		mLastUpdateStep = newStep;
		switch (newStep)
		{
		case UPDATE_STEP.CHECK_VERSION:
			downloadProgress = 0f;
			break;
		case UPDATE_STEP.GET_FILELIST:
			downloadProgress = 0f;
			break;
		case UPDATE_STEP.COMPARE_RES:
			downloadProgress = 0f;
			break;
		case UPDATE_STEP.CHECK_IS_DOWNLOAD:
			if (updateHelper.NeedDownloadSize > 1048576)
			{
				mNeedDownLoadSize = (float)updateHelper.NeedDownloadSize / 1024f / 1024f;
				mDownLoadUnit = "MB";
			}
			else
			{
				mNeedDownLoadSize = (float)updateHelper.NeedDownloadSize / 1024f;
				mDownLoadUnit = "KB";
			}
			OnClickDownLoadBtn();
			break;
		case UPDATE_STEP.DOWNLOAD_RES:
			break;
		case UPDATE_STEP.CHECK_RES:
			downloadProgress = 0.93f;
			break;
		case UPDATE_STEP.COPY_RES:
			downloadProgress = 0.93f;
			break;
		case UPDATE_STEP.CLEAR_CACHE:
			downloadProgress = 0.93f;
			SingletonDontDestoryUnity<GameManager>.Instance.ReImportData();
			SingletonDontDestoryUnity<GameManager>.Instance.ReImportAnima();
			break;
		case UPDATE_STEP.FINISH:
			if (updateHelper.CurUpdateResult == UPDATE_RESULT.SUCCESS)
			{
				downloadProgress = 0.95f;
				DownloadFlurryCount(Time.realtimeSinceStartup - startDownloadTime);
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewDownload", "BackGroundDownloadFinish");
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewDownload", "FinishTimes");
			}
			else if (autoContinueFlag)
			{
				autoContinueFlag = false;
				ReDownLoad();
			}
			break;
		}
	}

	private void DownloadFlurryCount(float downloadTime)
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.IsNeedCountDownloadTime)
		{
			if (downloadTime < 60f)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewFullDownloadTime", "1Min");
			}
			else if (downloadTime < 120f)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewFullDownloadTime", "2Min");
			}
			else if (downloadTime < 180f)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewFullDownloadTime", "3Min");
			}
			else if (downloadTime < 240f)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewFullDownloadTime", "4Min");
			}
			else if (downloadTime < 300f)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewFullDownloadTime", "5Min");
			}
			else if (downloadTime < 420f)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewFullDownloadTime", "7Min");
			}
			else if (downloadTime < 540f)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewFullDownloadTime", "9Min");
			}
			else if (downloadTime < 660f)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewFullDownloadTime", "11Min");
			}
			else
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewFullDownloadTime", "11+Min");
			}
		}
	}

	private void CheckDownloadRes()
	{
		SetProgressLineVal(0f);
		SetProgressLineWidth(0);
		OnClickDownLoadBtn();
	}

	private void OnClickExitGame()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.LeaveGame();
		MessageBoxLogic.CloseBox();
		Application.Quit();
	}

	public void StartDownload()
	{
		updateHelper.DownloadFileList();
	}

	public void ContinueDownLoad()
	{
		updateHelper.ContinueDownload();
	}

	public void ReDownLoad()
	{
		downloadProgress = 0f;
		updateHelper.StartCheckRes(updateHelper.mServerUrl, OnChangeUpdateStep, isNeedSameVersion: true, isNeedCopyRes: false);
	}

	private void SetProgressLineVal(float val)
	{
		if (val > curProgress)
		{
			mProgressTargetWidth = (int)(val * (float)mProgressLineWidth);
			curProgress = val;
		}
	}

	private void Update()
	{
		if (mLastUpdateStep == UPDATE_STEP.DOWNLOAD_RES && mTutorialStep == 2)
		{
			downloadProgress = updateHelper.GetDownloadProgress() * 0.93f;
		}
		if (mLastUpdateStep == UPDATE_STEP.FINISH && updateHelper.CurUpdateResult == UPDATE_RESULT.SUCCESS && DataManager.DataInitFinishFlag)
		{
			downloadProgress = 1f;
		}
		if (mTutorialStep == 2)
		{
			UpdateTutorialStep(2, downloadProgress);
		}
		if (ProgressLinePic.width < mProgressTargetWidth)
		{
			int num = ProgressLinePic.width + 10;
			if (num > mProgressTargetWidth)
			{
				num = mProgressTargetWidth;
			}
			SetProgressLineWidth(num);
			if (ProgressLinePic.width >= mProgressLineWidth && mLastUpdateStep == UPDATE_STEP.FINISH)
			{
				DownLoadFinishFlag = true;
				timeCount = Time.time;
			}
		}
	}
}
