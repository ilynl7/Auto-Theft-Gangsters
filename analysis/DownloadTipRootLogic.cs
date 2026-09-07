using UnityEngine;

public class DownloadTipRootLogic : SingletonUnity<DownloadTipRootLogic>
{
	public UILabel TipLabel;

	public UILabel InfoLabel;

	public GameObject FinishRoot;

	public TweenPosition DirAnima;

	public TweenAlpha BgAnima;

	private FileUpdateHelper updateHelper;

	private UPDATE_STEP mCurUpdateStep;

	private bool autoContinueFlag;

	private float startDownloadTime;

	private bool isBackGroundDownload;

	public FileUpdateHelper UpdateHelper
	{
		get
		{
			return updateHelper;
		}
		set
		{
			updateHelper = value;
		}
	}

	public void Reset()
	{
		isBackGroundDownload = false;
		if (PlayerData.ServerDataVersion != PlayerData.LocalDataVersion)
		{
			PlayerData.downLoadFlag = 1;
			if (updateHelper == null)
			{
				GameObject gameObject = GameObject.Find("FileUpdateHelper");
				if (gameObject == null)
				{
					gameObject = new GameObject();
					gameObject.name = "FileUpdateHelper";
					UnityVersionUtil.SetActiveRecursive(gameObject, state: true);
					updateHelper = gameObject.AddComponent<FileUpdateHelper>();
				}
				else
				{
					updateHelper = gameObject.GetComponent<FileUpdateHelper>();
					if (updateHelper == null)
					{
						updateHelper = gameObject.AddComponent<FileUpdateHelper>();
					}
				}
			}
			BeginDownload();
			if (updateHelper.CurUpdateStape == UPDATE_STEP.FINISH && updateHelper.CurUpdateResult == UPDATE_RESULT.SUCCESS)
			{
				UnityVersionUtil.SetActiveRecursive(FinishRoot, state: true);
				ShowTipsLabel(isFinish: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(FinishRoot, state: false);
				ShowTipsLabel(isFinish: false);
			}
			autoContinueFlag = true;
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(FinishRoot, state: true);
			ShowTipsLabel(isFinish: true);
		}
	}

	public void ShowTipsLabel(bool isFinish)
	{
		if (isFinish)
		{
			if (GameManager.IsSupportCurDataVersion167())
			{
				TipLabel.text = StrDictionary.GetDictionaryString("#{300802}");
			}
			else
			{
				TipLabel.text = "Complete";
			}
			DirAnima.enabled = false;
			DirAnima.transform.localPosition = new Vector3(DirAnima.transform.localPosition.x, 0f, DirAnima.transform.localPosition.z);
			BgAnima.enabled = true;
			BgAnima.PlayForward();
		}
		else
		{
			if (GameManager.IsSupportCurDataVersion167())
			{
				TipLabel.text = StrDictionary.GetDictionaryString("#{102201}");
			}
			else
			{
				TipLabel.text = "Download";
			}
			DirAnima.enabled = true;
			DirAnima.PlayForward();
			BgAnima.enabled = false;
			BgAnima.ResetToBeginning();
		}
		if (GameManager.IsSupportCurDataVersion167())
		{
			InfoLabel.text = StrDictionary.GetDictionaryString("#{102214}");
		}
		else
		{
			InfoLabel.text = "With New Car";
		}
	}

	public void OnClickDownloadTipBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DownLoadResRoot);
	}

	public void BeginDownload()
	{
		if (GameSettingData.GetPhoneClass() != 0)
		{
			updateHelper.StartCheckRes(updateHelper.mServerUrl, OnChangeUpdateStep, isNeedSameVersion: true, isNeedCopyRes: false);
		}
	}

	public void OnChangeUpdateStep(UPDATE_STEP newStep)
	{
		mCurUpdateStep = newStep;
		switch (mCurUpdateStep)
		{
		case UPDATE_STEP.CLEAR_CACHE:
			if (SingletonDontDestoryUnity<GameManager>.Instance.IsNeedCountDownloadFinish)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.IsNeedCountDownloadFinish = false;
				LocalDataSaveManager.SetDownloadFinishCountFinish();
				if (isBackGroundDownload)
				{
					DownloadFlurryCount(Time.realtimeSinceStartup - startDownloadTime);
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewDownload", "BackGroundDownloadFinish");
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewDownload", "FinishTimes");
				}
				else
				{
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewDownload", "RealDownloadFinish");
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewDownload", "FinishTimes");
					DownloadFlurryCount(Time.realtimeSinceStartup - startDownloadTime);
				}
			}
			break;
		case UPDATE_STEP.FINISH:
			if (updateHelper.CurUpdateResult == UPDATE_RESULT.SUCCESS)
			{
				ShowDownloadFinishTip();
			}
			else if (!SingletonUnity<DownLoadResRootLogic>.Exists && autoContinueFlag)
			{
				autoContinueFlag = false;
				ReDownLoad();
			}
			break;
		case UPDATE_STEP.CHECK_IS_DOWNLOAD:
			if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
			{
				updateHelper.DownloadFileList();
				if (SingletonUnity<DownLoadResRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DownLoadResRootLogic>.Instance.gameObject))
				{
					SingletonUnity<DownLoadResRootLogic>.Instance.OnChangeUpdateStep(UPDATE_STEP.DOWNLOAD_RES, isForce: true);
				}
				isBackGroundDownload = true;
				if (SingletonDontDestoryUnity<GameManager>.Instance.IsNeedCountDownload)
				{
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewDownload", "BackGroundDownloadStart");
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewDownload", "StartTimes");
					LocalDataSaveManager.SetDownloadCountFinish();
					startDownloadTime = Time.realtimeSinceStartup;
				}
			}
			break;
		case UPDATE_STEP.DOWNLOAD_RES:
		case UPDATE_STEP.CHECK_RES:
		case UPDATE_STEP.COPY_RES:
			break;
		}
	}

	public void ReDownLoad()
	{
		updateHelper.StartCheckRes(updateHelper.mServerUrl, OnChangeUpdateStep, isNeedSameVersion: true, isNeedCopyRes: false);
	}

	public void ShowDownloadFinishTip()
	{
		UnityVersionUtil.SetActiveRecursive(FinishRoot, state: true);
		ShowTipsLabel(isFinish: true);
		if (SingletonUnity<CitySimController>.Exists)
		{
			SingletonUnity<CitySimController>.Instance.OnDownloadFlashNPC();
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
}
