using SprotoType;
using UnityEngine;

public class GongNengQuLogic : SingletonUnity<GongNengQuLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UILabel viewStateLabel;

	public UILabel MapNameLabel;

	public UISprite MenuBtnSprite;

	public UIWidget ViewBtnRoot;

	public GameObject MapNameRoot;

	public GameObject MenuBtnRoot;

	public GameObject ExitBtnRoot;

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

	private void UpdateViewState()
	{
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			CameraController cameraController = Singleton<ObjManager>.Instance.MainPlayer.CameraController;
			if (cameraController != null)
			{
				if (CameraController.CurrentViewState == CameraController.CAMERAVIEWSTATE.FREE || CameraController.CurrentViewState == CameraController.CAMERAVIEWSTATE.FIXED_2_FREE)
				{
					viewStateLabel.text = "3D";
				}
				else
				{
					viewStateLabel.text = "2.5D";
				}
			}
			else
			{
				Log.ERROR_MSG("GongNengQuLogic CameraController is NULL!");
			}
		}
		else
		{
			viewStateLabel.text = "3D";
		}
	}

	public void ChangViewOnClick()
	{
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			CameraController cameraController = Singleton<ObjManager>.Instance.MainPlayer.CameraController;
			if (cameraController != null)
			{
				if (CameraController.CurrentViewState == CameraController.CAMERAVIEWSTATE.FREE)
				{
					cameraController.ChangeCameraView(CameraController.CAMERAVIEWSTATE.FIXED);
				}
				else if (CameraController.CurrentViewState == CameraController.CAMERAVIEWSTATE.FIXED)
				{
					cameraController.ChangeCameraView(CameraController.CAMERAVIEWSTATE.FREE);
				}
			}
		}
		UpdateViewState();
	}

	public void OnClickMenuBtn()
	{
	}

	public void OnClickExitBtn()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.CanSendToServer(108, 3f))
		{
			NoticeLogic.AddNotifyData("#{200066}");
		}
		else
		{
			MessageBoxLogic.OpenOKCancelBox("#{100319}", "#{101506}", ExitMission);
		}
	}

	private void ExitMission()
	{
		leave_copy_scene.request rpcReq = new leave_copy_scene.request();
		NetLogic.GetInstance().Send<Protocol.leave_copy_scene>(rpcReq);
	}

	private new void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
		UpdateViewState();
	}

	private void OnEnable()
	{
		Reset();
	}

	public void Reset()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsBigWorld())
		{
			UnityVersionUtil.SetActiveRecursive(ExitBtnRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(MapNameRoot, state: true);
			MapNameLabel.text = StrDictionary.GetDictionaryString(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.Name);
			ViewBtnRoot.transform.localPosition = new Vector3(-286f, -35f, 0f);
			MenuBtnRoot.transform.localPosition = new Vector3(-35f, -35f, 0f);
		}
		else if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsTutorialScene())
		{
			UnityVersionUtil.SetActiveRecursive(ExitBtnRoot, state: true);
			UnityVersionUtil.SetActiveRecursive(MenuBtnRoot.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(MapNameRoot, state: false);
			ViewBtnRoot.transform.localPosition = new Vector3(-105f, -35f, 0f);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ExitBtnRoot, state: true);
			UnityVersionUtil.SetActiveRecursive(MapNameRoot, state: false);
			ViewBtnRoot.transform.localPosition = new Vector3(-175f, -35f, 0f);
			MenuBtnRoot.transform.localPosition = new Vector3(-105f, -35f, 0f);
		}
	}

	public void OnClickMapBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MapUIRoot, delegate
		{
			SingletonUnity<MapUIRootLogic>.Instance.Reset();
		});
	}
}
