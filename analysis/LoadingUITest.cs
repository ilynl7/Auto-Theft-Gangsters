using UnityEngine;

public class LoadingUITest : MonoBehaviour
{
	private FileUpdateHelper updateHelper;

	public UISlider ProcessSlider;

	public UILabel UpdateResultLabel;

	public UILabel UpdateStateLabel;

	public GameObject CheckRootObj;

	public UILabel DownloadSizeLabel;

	public GameObject ReDownloadRootObj;

	public GameObject ContinueDownloadRootObj;

	public UILabel curDownloadFileNameLabel;

	private UPDATE_STEP mLastUpdateStep = UPDATE_STEP.INVALID;

	private float targetPercent;

	private void Awake()
	{
		updateHelper = base.gameObject.GetComponent<FileUpdateHelper>();
		if (updateHelper == null)
		{
			updateHelper = base.gameObject.AddComponent<FileUpdateHelper>();
		}
	}

	private void Start()
	{
		UnityVersionUtil.SetActiveRecursive(CheckRootObj.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(ReDownloadRootObj.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(ContinueDownloadRootObj, state: false);
		updateHelper.StartCheckRes(updateHelper.mServerUrl, OnChangeUpdateStep, isNeedSameVersion: false, isNeedCopyRes: true);
		ProcessSlider.value = 0f;
	}

	private void Update()
	{
		if (mLastUpdateStep == UPDATE_STEP.DOWNLOAD_RES)
		{
			SetSlider(0.3f + 0.6f * updateHelper.GetDownloadProgress());
		}
		if (ProcessSlider.value < targetPercent)
		{
			float num = ProcessSlider.value + Time.deltaTime;
			if (num > targetPercent)
			{
				num = targetPercent;
			}
			ProcessSlider.value = num;
			if (ProcessSlider.value > 0.99f)
			{
				Application.LoadLevel("Login");
			}
		}
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	public void OnChangeUpdateStep(UPDATE_STEP newStep)
	{
		UpdateStateLabel.text = string.Empty + newStep;
		switch (newStep)
		{
		case UPDATE_STEP.CHECK_VERSION:
			SetSlider(0f);
			break;
		case UPDATE_STEP.GET_FILELIST:
			SetSlider(0.1f);
			break;
		case UPDATE_STEP.COMPARE_RES:
			SetSlider(0.2f);
			break;
		case UPDATE_STEP.CHECK_IS_DOWNLOAD:
			SetSlider(0.3f);
			DownloadSizeLabel.text = (float)updateHelper.NeedDownloadSize / 1024f / 1024f + "MB";
			UnityVersionUtil.SetActiveRecursive(CheckRootObj.gameObject, state: true);
			break;
		case UPDATE_STEP.CHECK_RES:
			SetSlider(0.9f);
			break;
		case UPDATE_STEP.COPY_RES:
			SetSlider(0.95f);
			break;
		case UPDATE_STEP.CLEAR_CACHE:
			SetSlider(0.98f);
			break;
		case UPDATE_STEP.FINISH:
			if (updateHelper.CurUpdateResult == UPDATE_RESULT.SUCCESS)
			{
				SetSlider(1f);
			}
			else if (mLastUpdateStep == UPDATE_STEP.DOWNLOAD_RES)
			{
				UnityVersionUtil.SetActiveRecursive(ContinueDownloadRootObj, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(ReDownloadRootObj.gameObject, state: true);
			}
			UpdateResultLabel.text = string.Empty + newStep;
			break;
		}
		mLastUpdateStep = newStep;
	}

	private void SetSlider(float percent)
	{
		targetPercent = percent;
	}

	public void StartDownload()
	{
		UnityVersionUtil.SetActiveRecursive(CheckRootObj.gameObject, state: false);
		updateHelper.DownloadFileList();
	}

	public void ReDownLoad()
	{
		ProcessSlider.value = 0f;
		updateHelper.StartCheckRes(updateHelper.mServerUrl, OnChangeUpdateStep, isNeedSameVersion: false, isNeedCopyRes: true);
		UnityVersionUtil.SetActiveRecursive(ReDownloadRootObj.gameObject, state: false);
	}

	public void ContinueDownLoad()
	{
		updateHelper.ContinueDownload();
		UnityVersionUtil.SetActiveRecursive(ContinueDownloadRootObj, state: false);
	}
}
