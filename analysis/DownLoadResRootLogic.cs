using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DownLoadResRootLogic : SingletonUnity<DownLoadResRootLogic>
{
	public UILabel TitleLabel;

	public UILabel TextLabel;

	public UILabel TipTextLabel;

	public ShowRewardItems RewardItems;

	public UIButtonColor BtnColor;

	public UILabel DownloadBtnLabel;

	public UILabel DownloadBtnSizeLabel;

	public UIEventListener RotateModelBtnListener;

	public UILabel ProgressPercentLabel;

	public GameObject ProgressLineAnimaObj;

	private float startDownloadTime;

	private int curAutoReDownloadTimes;

	private bool mIsAutoDownload;

	private bool mIsFullDownload;

	public GameObject DownloadBtnRoot;

	public GameObject DownloadFinishBtnRoot;

	public GameObject ProgressRoot;

	public GameObject CloseBtnRoot;

	public GameObject DownloadInfoLabelRoot;

	private bool isNeedAddFunc;

	public UISprite ProgressLinePic;

	private int mProgressLineWidth = -1;

	private int mProgressTargetWidth;

	public UILabel ProgressInfoLabel;

	private string mCurLocalVersion = string.Empty;

	private string mCurApkVersion = string.Empty;

	private FileUpdateHelper updateHelper;

	private UPDATE_STEP mLastUpdateStep;

	private float mNeedDownLoadSize;

	private string mDownLoadUnit;

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

	public void OnClickDownLoadBtn()
	{
		StartDownload();
		NGUITools.SetActive(DownloadBtnRoot.gameObject, state: false);
		NGUITools.SetActive(ProgressPercentLabel.gameObject, state: true);
		if (mIsFullDownload)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.IsNeedCountDownload)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewDownload", "RealDownloadStart");
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("tutorial", "NewDownload", "StartTimes");
				LocalDataSaveManager.SetDownloadCountFinish();
			}
			startDownloadTime = Time.realtimeSinceStartup;
		}
		else if (SingletonUnity<DownloadResTipRootLogic>.Exists)
		{
			startDownloadTime = SingletonUnity<DownloadResTipRootLogic>.Instance.StartDownloadTime;
		}
	}

	private new void Awake()
	{
		base.Awake();
		curAutoReDownloadTimes = 0;
		DownLoadFinishFlag = false;
		mIsAutoDownload = false;
		ProgressLinePic.transform.localPosition = new Vector3(-ProgressLinePic.width / 2, 0f, 0f);
		mProgressLineWidth = ProgressLinePic.width;
		SetProgressLineWidth(0);
		isNeedAddFunc = false;
		if (SingletonUnity<DownloadTipRootLogic>.Exists && SingletonUnity<DownloadTipRootLogic>.Instance.UpdateHelper != null)
		{
			updateHelper = SingletonUnity<DownloadTipRootLogic>.Instance.UpdateHelper;
			isNeedAddFunc = true;
		}
		else
		{
			GameObject gameObject = new GameObject();
			gameObject.name = "FileUpdateHelper";
			UnityVersionUtil.SetActiveRecursive(gameObject, state: true);
			updateHelper = gameObject.AddComponent<FileUpdateHelper>();
			if (SingletonUnity<DownloadTipRootLogic>.Exists)
			{
				SingletonUnity<DownloadTipRootLogic>.Instance.UpdateHelper = updateHelper;
			}
		}
		DownloadRewardData downloadRewardDataBuyId = DataManager.GetDownloadRewardDataBuyId("1");
		List<string> list = new List<string>();
		list.Add(downloadRewardDataBuyId.ItemID1);
		list.Add(downloadRewardDataBuyId.ItemID2);
		list.Add(downloadRewardDataBuyId.ItemID3);
		list.Add(downloadRewardDataBuyId.ItemID4);
		List<int> list2 = new List<int>();
		list2.Add(downloadRewardDataBuyId.ItemCount1);
		list2.Add(downloadRewardDataBuyId.ItemCount2);
		list2.Add(downloadRewardDataBuyId.ItemCount3);
		list2.Add(downloadRewardDataBuyId.ItemCount4);
		List<int> list3 = new List<int>();
		list3.Add(downloadRewardDataBuyId.Quality1);
		list3.Add(downloadRewardDataBuyId.Quality2);
		list3.Add(downloadRewardDataBuyId.Quality3);
		list3.Add(downloadRewardDataBuyId.Quality4);
		RewardItems.ShowRewards(list, list3, list2);
		DownloadBtnLabel.text = StrDictionary.GetDictionaryString("#{102212}");
		TextLabel.pivot = UIWidget.Pivot.TopLeft;
		TextLabel.text = StrDictionary.GetDictionaryString("#{201013}");
		TipTextLabel.enabled = true;
		TipTextLabel.text = StrDictionary.GetDictionaryString("201014");
		NGUITools.SetActive(DownloadBtnRoot.gameObject, state: false);
		NGUITools.SetActive(DownloadFinishBtnRoot.gameObject, state: false);
		NGUITools.SetActive(ProgressPercentLabel.gameObject, state: true);
		ProgressPercentLabel.text = "2";
		PlayerData.downLoadFlag = 1;
		UPDATE_STEP curUpdateStape = updateHelper.CurUpdateStape;
		UPDATE_RESULT curUpdateResult = updateHelper.CurUpdateResult;
		if (curUpdateStape == UPDATE_STEP.INVALID)
		{
			mIsFullDownload = true;
			BeginDownload();
		}
		else
		{
			mIsFullDownload = false;
			switch (curUpdateResult)
			{
			case UPDATE_RESULT.SUCCESS:
				if (!updateHelper.IsNeedCopyRes)
				{
					updateHelper.IsNeedCopyRes = true;
					BeginDownload();
				}
				else
				{
					ResetDownloadSuccess();
				}
				break;
			default:
				BeginDownload();
				break;
			case UPDATE_RESULT.INVALID:
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
				if (curUpdateStape == UPDATE_STEP.CHECK_IS_DOWNLOAD)
				{
					mIsFullDownload = true;
				}
				OnChangeUpdateStep(curUpdateStape, isForce: true);
				break;
			}
		}
		updateHelper.IsNeedCopyRes = true;
	}

	private void SetProgressLineWidth(int width)
	{
		ProgressLinePic.width = width;
		ProgressLineAnimaObj.transform.localPosition = new Vector3(width, 0f, 0f);
	}

	private void BeginDownload()
	{
		updateHelper.StartCheckRes(updateHelper.mServerUrl, OnChangeUpdateStep, isNeedSameVersion: true, isNeedCopyRes: true);
		if (SingletonUnity<DownloadTipRootLogic>.Exists)
		{
			FileUpdateHelper fileUpdateHelper = updateHelper;
			fileUpdateHelper.OnChangeUpdateStep = (FileUpdateHelper.OnChangeUpdateStepDelegate)Delegate.Combine(fileUpdateHelper.OnChangeUpdateStep, new FileUpdateHelper.OnChangeUpdateStepDelegate(SingletonUnity<DownloadTipRootLogic>.Instance.OnChangeUpdateStep));
		}
	}

	public void OnChangeUpdateStep(UPDATE_STEP newStep)
	{
		if (!(base.gameObject == null) && UnityVersionUtil.IsActive(base.gameObject))
		{
			OnChangeUpdateStep(newStep, isForce: false);
		}
	}

	public void OnChangeUpdateStep(UPDATE_STEP newStep, bool isForce)
	{
		switch (newStep)
		{
		case UPDATE_STEP.CHECK_VERSION:
			SetProgressLineVal(0.3f, isForce);
			ProgressInfoLabel.text = StrDictionary.GetDictionaryString("#{102203}");
			break;
		case UPDATE_STEP.GET_FILELIST:
			SetProgressLineVal(0.6f, isForce);
			ProgressInfoLabel.text = StrDictionary.GetDictionaryString("#{102204}");
			break;
		case UPDATE_STEP.COMPARE_RES:
			SetProgressLineVal(1f, isForce);
			ProgressInfoLabel.text = StrDictionary.GetDictionaryString("#{102205}");
			break;
		case UPDATE_STEP.CHECK_IS_DOWNLOAD:
			if (mLastUpdateStep == UPDATE_STEP.DOWNLOAD_RES)
			{
				return;
			}
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
			CheckDownloadRes();
			break;
		case UPDATE_STEP.DOWNLOAD_RES:
			NGUITools.SetActive(DownloadBtnRoot.gameObject, state: false);
			if (isForce)
			{
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
				SetProgressLineVal(updateHelper.GetDownloadProgress(), isForce);
				downloadSb.Length = 0;
				downloadSb.AppendFormat(downloadFormat, mNeedDownLoadSize * curProgress, mNeedDownLoadSize, mDownLoadUnit);
				ProgressInfoLabel.text = downloadSb.ToString();
			}
			break;
		case UPDATE_STEP.CHECK_RES:
			NGUITools.SetActive(DownloadBtnRoot.gameObject, state: false);
			SetProgressLineWidth(0);
			SetProgressLineVal(0.3f, isForce);
			ProgressInfoLabel.text = StrDictionary.GetDictionaryString("#{102207}");
			break;
		case UPDATE_STEP.COPY_RES:
			NGUITools.SetActive(DownloadBtnRoot.gameObject, state: false);
			SetProgressLineVal(0.6f, isForce);
			break;
		case UPDATE_STEP.CLEAR_CACHE:
			NGUITools.SetActive(DownloadBtnRoot.gameObject, state: false);
			SetProgressLineVal(0.93f, isForce);
			SingletonDontDestoryUnity<GameManager>.Instance.ReImportData();
			SingletonDontDestoryUnity<GameManager>.Instance.ReImportAnima();
			break;
		case UPDATE_STEP.FINISH:
			NGUITools.SetActive(DownloadBtnRoot.gameObject, state: false);
			if (updateHelper.CurUpdateResult == UPDATE_RESULT.SUCCESS)
			{
				SetProgressLineVal(0.95f, isForce);
				break;
			}
			SetProgressLineVal(0.3f, isForce);
			if (curAutoReDownloadTimes < GameDefine.AutoReDownloadTimes)
			{
				curAutoReDownloadTimes++;
				mIsAutoDownload = true;
				ReDownLoad();
			}
			else
			{
				mIsAutoDownload = false;
				MessageBoxLogic.OpenOKCancelBox("#{100156}", "#{100127}", ReDownLoad, OnClickExitGame, null, "#{100149}");
			}
			break;
		default:
			Debug.Log("CURSTATE :: " + newStep);
			break;
		}
		mLastUpdateStep = newStep;
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
		ProgressInfoLabel.text = string.Empty;
		SetProgressLineVal(0f);
		ProgressPercentLabel.text = "0";
		SetProgressLineWidth(0);
		if (SingletonDontDestoryUnity<GameManager>.Instance.IsAutoDownload)
		{
			OnClickDownLoadBtn();
			return;
		}
		if (mIsAutoDownload)
		{
			OnClickDownLoadBtn();
			return;
		}
		NGUITools.SetActive(DownloadBtnRoot.gameObject, state: true);
		NGUITools.SetActive(ProgressPercentLabel.gameObject, state: false);
		DownloadBtnSizeLabel.text = $"{mNeedDownLoadSize:N1}{mDownLoadUnit}";
	}

	private void OnClickExitGame()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.LeaveGame();
		MessageBoxLogic.CloseBox();
		Application.Quit();
	}

	public void StartDownload()
	{
		ProgressInfoLabel.text = $"{0.0:N1}/{mNeedDownLoadSize:N1}{mDownLoadUnit}";
		updateHelper.DownloadFileList();
	}

	public void ContinueDownLoad()
	{
		updateHelper.ContinueDownload();
	}

	public void ReDownLoad()
	{
		SetProgressLineWidth(0);
		updateHelper.StartCheckRes(updateHelper.mServerUrl, OnChangeUpdateStep, isNeedSameVersion: true, isNeedCopyRes: true);
		if (SingletonUnity<DownloadTipRootLogic>.Exists)
		{
			FileUpdateHelper fileUpdateHelper = updateHelper;
			fileUpdateHelper.OnChangeUpdateStep = (FileUpdateHelper.OnChangeUpdateStepDelegate)Delegate.Combine(fileUpdateHelper.OnChangeUpdateStep, new FileUpdateHelper.OnChangeUpdateStepDelegate(SingletonUnity<DownloadTipRootLogic>.Instance.OnChangeUpdateStep));
		}
	}

	private void SetProgressLineVal(float val, bool isForce = false)
	{
		mProgressTargetWidth = (int)(val * (float)mProgressLineWidth);
		curProgress = val;
		if (isForce)
		{
			SetProgressLineWidth(mProgressTargetWidth);
			percentSb.AppendFormat(percentFormat, (int)((float)mProgressTargetWidth / (float)mProgressLineWidth * 100f));
			ProgressPercentLabel.text = percentSb.ToString();
		}
	}

	public void OnClickTurnPageBtn()
	{
	}

	private void TurnPage()
	{
	}

	private void Update()
	{
		if (mLastUpdateStep == UPDATE_STEP.DOWNLOAD_RES)
		{
			SetProgressLineVal(updateHelper.GetDownloadProgress());
		}
		if (mLastUpdateStep == UPDATE_STEP.FINISH && updateHelper.CurUpdateResult == UPDATE_RESULT.SUCCESS && DataManager.DataInitFinishFlag)
		{
			SetProgressLineVal(1f);
		}
		if (ProgressLinePic.width >= mProgressTargetWidth)
		{
			return;
		}
		int num = ProgressLinePic.width + 10;
		if (num > mProgressTargetWidth)
		{
			num = mProgressTargetWidth;
		}
		SetProgressLineWidth(num);
		percentSb.Length = 0;
		if (mLastUpdateStep == UPDATE_STEP.DOWNLOAD_RES)
		{
			percentSb.AppendFormat(percentFormat, (int)((float)num / (float)mProgressLineWidth * 100f));
		}
		else
		{
			percentSb.AppendFormat(percentFormat, Mathf.Clamp((int)((float)num / (float)mProgressLineWidth * 100f), 2, 100));
		}
		ProgressPercentLabel.text = percentSb.ToString();
		if (mLastUpdateStep == UPDATE_STEP.DOWNLOAD_RES)
		{
			downloadSb.Length = 0;
			downloadSb.AppendFormat(downloadFormat, mNeedDownLoadSize * curProgress, mNeedDownLoadSize, mDownLoadUnit);
			ProgressInfoLabel.text = downloadSb.ToString();
		}
		if (ProgressLinePic.width >= mProgressLineWidth)
		{
			if (mLastUpdateStep == UPDATE_STEP.FINISH)
			{
				DownLoadFinishFlag = true;
				ResetDownloadSuccess();
				timeCount = Time.time;
			}
			else if (mLastUpdateStep == UPDATE_STEP.CHECK_IS_DOWNLOAD)
			{
				CheckDownloadRes();
			}
		}
	}

	public void ResetDownloadSuccess()
	{
		TipTextLabel.enabled = false;
		TextLabel.pivot = UIWidget.Pivot.Center;
		TextLabel.text = StrDictionary.GetDictionaryString("#{201016}");
		DownloadBtnLabel.text = StrDictionary.GetDictionaryString("#{201017}");
		NGUITools.SetActive(DownloadFinishBtnRoot, state: true);
		NGUITools.SetActive(ProgressRoot, state: true);
		ProgressPercentLabel.text = "100";
		NGUITools.SetActive(DownloadInfoLabelRoot, state: false);
		ProgressLinePic.width = mProgressLineWidth;
		NGUITools.SetActive(ProgressLineAnimaObj, state: false);
		NGUITools.SetActive(DownloadBtnRoot, state: false);
	}

	public void OnClickDownloadFinishBtn()
	{
		WaitResponseUIRootLogic.OpenWaitBox(270, 10f, 0f);
		NetLogic.GetInstance().Send<Protocol.download_finish>();
	}

	public void OnPressCarModelPic(GameObject btn, bool ispress)
	{
		if (ispress)
		{
			SingletonUnity<FakeCarObjRootLogic>.Instance.StopRotate();
		}
		else
		{
			SingletonUnity<FakeCarObjRootLogic>.Instance.PlayRotate();
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DownLoadResRoot);
	}

	private void OnEnable()
	{
		if (isNeedAddFunc)
		{
			FileUpdateHelper fileUpdateHelper = updateHelper;
			fileUpdateHelper.OnChangeUpdateStep = (FileUpdateHelper.OnChangeUpdateStepDelegate)Delegate.Combine(fileUpdateHelper.OnChangeUpdateStep, new FileUpdateHelper.OnChangeUpdateStepDelegate(OnChangeUpdateStep));
		}
	}

	private void OnDisable()
	{
		updateHelper.OnChangeUpdateStep = null;
		if (SingletonUnity<DownloadTipRootLogic>.Exists)
		{
			FileUpdateHelper fileUpdateHelper = updateHelper;
			fileUpdateHelper.OnChangeUpdateStep = (FileUpdateHelper.OnChangeUpdateStepDelegate)Delegate.Combine(fileUpdateHelper.OnChangeUpdateStep, new FileUpdateHelper.OnChangeUpdateStepDelegate(SingletonUnity<DownloadTipRootLogic>.Instance.OnChangeUpdateStep));
		}
	}
}
