using System.Collections.Generic;
using UnityEngine;

public class SingleRunPointSceneManager : SceneManager
{
	private List<Vector3> mPathPointList = new List<Vector3>();

	private MovePathPoint mMoveTargetPoint;

	private int mCurPointIndex;

	private GameObject mAnimaObj;

	public SceneAnimationCtl CurAnimaCtl;

	public override void Init(string id)
	{
		base.Init(id);
		mPathPointList = base.CurrentMapInofData.PlayerPathPointList;
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("Items/MovePathPoint") as GameObject;
		mMoveTargetPoint = gameObject.GetComponent<MovePathPoint>();
		UnityVersionUtil.SetActiveRecursive(mMoveTargetPoint.gameObject, state: false);
		mMoveTargetPoint.RegisterOnArrivePathPoint(OnArriveMovePoint);
		mCurPointIndex = 0;
		mAnimaObj = null;
		if (!string.IsNullOrEmpty(mapInfoData.Param2))
		{
			mAnimaObj = ResourcesManager.LoadAndInstantiate($"StartSceneAnima/{mapInfoData.Param2}") as GameObject;
			UnityVersionUtil.SetActiveRecursive(mAnimaObj, state: false);
		}
		SingletonUnity<MyEvent>.Instance.Register("OnLoadingOver", this, "OnLoadingOver");
	}

	public override void OnLoadingOver()
	{
		SingletonUnity<MyEvent>.Instance.DeRegister("OnLoadingOver", this, "OnLoadingOver");
		base.LoadingFlag = false;
		if (IsCanShowCheckPopUI())
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.CheckPopTipsUI();
		}
		else
		{
			SingletonUnity<UIManager>.Instance.ClearReshowUI();
		}
		if (mapInfoData.ID.Equals("312"))
		{
			TutorialManager.ShowTutorial(TUTORIAL_STEP.AUTO_FIGHT_CLICK);
			return;
		}
		NetLogic.GetInstance().Send<Protocol.map_ready>();
		LoadingWindow.isSendMapReady = true;
	}

	public void OpenBlock()
	{
		if (mCurPointIndex < mPathPointList.Count)
		{
			mMoveTargetPoint.transform.position = mPathPointList[mCurPointIndex];
			UnityVersionUtil.SetActiveRecursive(mMoveTargetPoint.gameObject, state: true);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CarTargetRoot, delegate
			{
				SingletonUnity<CarTargetUIRootLogic>.Instance.Reset(mMoveTargetPoint.gameObject, Singleton<ObjManager>.Instance.MainPlayer.gameObject);
			});
			mCurPointIndex++;
		}
	}

	private void OnArriveMovePoint(Vector3 pos)
	{
		if (mCurPointIndex == mPathPointList.Count && mAnimaObj != null)
		{
			UnityVersionUtil.SetActiveRecursive(mAnimaObj, state: true);
			CurAnimaCtl = mAnimaObj.GetComponent<SceneAnimationCtl>();
			CurAnimaCtl.RegisterOnFinished(delegate
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CarTargetRoot);
				NetLogic.GetInstance().Send<Protocol.start_battle>();
				Singleton<ObjManager>.Instance.MainPlayer.CameraController.CurCamera.enabled = true;
				if (SingletonUnity<ScreenBottomBtn>.Exists)
				{
					SingletonUnity<ScreenBottomBtn>.Instance.LockBtn = false;
				}
				UICamera.mainCamera.depth = 0f;
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SkipBtnRoot);
			});
			Singleton<ObjManager>.Instance.MainPlayer.CameraController.CurCamera.enabled = false;
			if (SingletonUnity<ScreenBottomBtn>.Exists)
			{
				SingletonUnity<ScreenBottomBtn>.Instance.LockBtn = true;
			}
			UICamera.mainCamera.depth = 2f;
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SkipBtnRoot);
		}
		else
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CarTargetRoot);
			NetLogic.GetInstance().Send<Protocol.start_battle>();
		}
	}

	public override void AutoFightAction()
	{
		if (UnityVersionUtil.IsActive(mMoveTargetPoint.gameObject))
		{
			Singleton<ObjManager>.Instance.MainPlayer.MoveTo(mMoveTargetPoint.transform.position);
		}
	}
}
