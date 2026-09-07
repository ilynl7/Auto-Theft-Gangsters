using System;
using System.IO;
using System.Text;
using UnityEngine;

public class ExpLineRootLogic : SingletonUnity<ExpLineRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UILabel ExpLabel;

	public UISprite ExpSprite;

	public UISprite BottomPic;

	public UISprite TopPic;

	public UISprite ViewBtnPic;

	public UILabel ViewLabel;

	private PlayerData mPlayerData;

	private int mCurLevel = -1;

	private long mTargetExp;

	private float mCurPercent;

	private StringBuilder mTempStr = new StringBuilder();

	private float startTime;

	private float startTime2;

	private float startTime3;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	public static void UpdateExp()
	{
		if (SingletonUnity<ExpLineRootLogic>.Exists)
		{
			SingletonUnity<ExpLineRootLogic>.Instance.UpdateExpVal();
		}
	}

	private void Start()
	{
		UpdateExpVal();
		startTime = 300f;
		startTime2 = 300f;
		startTime3 = 10f;
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.CurrentMapInofData.MapType == MAPTYPE.CAR_CHASE_COPY)
		{
			UnityVersionUtil.SetActiveRecursive(ViewBtnPic.gameObject, state: false);
		}
		UpdateViewType();
	}

	public void UpdateViewType()
	{
		if (mPlayerData == null)
		{
			mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		}
		if (mPlayerData.ViewType == CameraController.CAMERAVIEWSTATE.FIXED || mPlayerData.ViewType == CameraController.CAMERAVIEWSTATE.FIXED_3D_2_FIXED)
		{
			ViewLabel.text = "2.5D";
		}
		else if (mPlayerData.ViewType == CameraController.CAMERAVIEWSTATE.FREE || mPlayerData.ViewType == CameraController.CAMERAVIEWSTATE.FIXED_3D_2_FREE)
		{
			ViewLabel.text = "3D";
		}
	}

	public void UpdateExpVal()
	{
		if (mPlayerData == null)
		{
			mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		}
		if (mCurLevel != mPlayerData.Level)
		{
			mTargetExp = DataManager.GetLevelDataByLevel(mPlayerData.Level).Exp;
			mCurLevel = mPlayerData.Level;
		}
		mCurPercent = Mathf.Clamp01((float)mPlayerData.MainPlayerAttrData.CurEXP / (float)mTargetExp);
		ExpSprite.width = (int)((float)Screen.width * mCurPercent);
		TopPic.width = (int)((float)BottomPic.width * mCurPercent);
		mTempStr.Length = 0;
		ExpLabel.text = mTempStr.AppendFormat("Exp ({0:0.0}%)", mCurPercent * 100f).ToString();
	}

	private void UpdateDelayTime()
	{
	}

	private int GetBatteryLevel()
	{
		int num = 50;
		try
		{
			string s = File.ReadAllText("/sys/class/power_supply/battery/capacity");
			return int.Parse(s);
		}
		catch (Exception)
		{
			return SingletonDontDestoryUnity<GameManager>.Instance.GetBatteryState();
		}
	}

	public void OnClickChangeViewBtn()
	{
		if (!(Singleton<ObjManager>.Instance.MainPlayer != null))
		{
			return;
		}
		CameraController cameraController = Singleton<ObjManager>.Instance.MainPlayer.CameraController;
		if (cameraController != null)
		{
			if (CameraController.CurrentViewState == CameraController.CAMERAVIEWSTATE.FREE)
			{
				cameraController.ChangeCameraView(CameraController.CAMERAVIEWSTATE.FIXED);
				UpdateViewType();
			}
			else if (CameraController.CurrentViewState == CameraController.CAMERAVIEWSTATE.FIXED)
			{
				cameraController.ChangeCameraView(CameraController.CAMERAVIEWSTATE.FREE);
				UpdateViewType();
			}
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.SWITCH_VIEW)
		{
			CheckTutorialEvent();
		}
	}
}
