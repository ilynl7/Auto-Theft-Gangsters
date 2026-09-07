using SprotoType;
using UnityEngine;

public class TeleportPointNew : MonoBehaviour
{
	public int ActiveRadius = 3;

	private bool mbValid = true;

	private float mfLastInvaildTime;

	private Transform mMainPlayerTransform;

	private Transform mTeleportTransform;

	private AutoSearchPathManager mAutoSearchPathManager;

	private float SqrActiveRadius;

	private float ExitSqrActiveRadius;

	private bool mInCircleFlag = true;

	private float sqrDis;

	private void Start()
	{
		mTeleportTransform = base.transform;
		if (mAutoSearchPathManager == null)
		{
			mAutoSearchPathManager = SingletonDontDestoryUnity<GameManager>.Instance.AutoSearchPath;
		}
		SqrActiveRadius = ActiveRadius * ActiveRadius;
		ExitSqrActiveRadius = (ActiveRadius + 1) * (ActiveRadius + 1);
	}

	private void GoNextScene()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange)
		{
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
			{
				SingletonUnity<UIManager>.Instance.CloseAllPOPUI();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DownLoadResRoot);
				return;
			}
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
			enter_new_map.request request = new enter_new_map.request();
			request.mapInfoId = mAutoSearchPathManager.TargetSceneId;
			NetLogic.GetInstance().Send<Protocol.enter_new_map>(request);
			mfLastInvaildTime = Time.time;
			mbValid = false;
			WaitResponseUIRootLogic.OpenWaitBox(106, 10f, 0f);
		}
	}

	private void FixedUpdate()
	{
		if (!mbValid)
		{
			if (Time.time - mfLastInvaildTime < 3f)
			{
				return;
			}
			mbValid = true;
		}
		if (null == mMainPlayerTransform)
		{
			if (null != Singleton<ObjManager>.Instance.MainPlayer)
			{
				mMainPlayerTransform = Singleton<ObjManager>.Instance.MainPlayer.transform;
			}
			if (null == mMainPlayerTransform)
			{
				return;
			}
		}
		if (!(null != Singleton<ObjManager>.Instance.MainPlayer))
		{
			return;
		}
		sqrDis = (mMainPlayerTransform.position - mTeleportTransform.position).sqrMagnitude;
		if (sqrDis <= SqrActiveRadius)
		{
			if (mInCircleFlag)
			{
				return;
			}
			if (mAutoSearchPathManager.IsAutoMovingFlag)
			{
				GoNextScene();
			}
			else if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange && !UIManager.IsUnlockTutorialEnable() && !SingletonUnity<UIManager>.Instance.IsHideBaseUI && (!SingletonUnity<MapUIRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<MapUIRootLogic>.Instance.gameObject)))
			{
				SingletonUnity<UIManager>.Instance.CloseAllPOPUI();
				if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DownLoadResRoot);
				}
				else
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MapUIRoot, delegate
					{
						SingletonUnity<MapUIRootLogic>.Instance.Reset();
						SingletonUnity<MapUIRootLogic>.Instance.ChangeToLocalMap(isTrue: false);
					});
				}
			}
			mInCircleFlag = true;
			mAutoSearchPathManager.IsInTelePortCircle = true;
		}
		else if (sqrDis > ExitSqrActiveRadius)
		{
			mInCircleFlag = false;
			mAutoSearchPathManager.IsInTelePortCircle = false;
		}
	}
}
