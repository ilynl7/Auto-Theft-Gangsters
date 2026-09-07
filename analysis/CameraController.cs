using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public enum CAMERAVIEWSTATE
	{
		FIXED,
		FREE,
		FIXED_2_FREE,
		FREE_2_FIXED,
		FIXED_3D,
		FREE_2_FIXED_3D,
		FIXED_3D_2_FREE,
		FEXED_2_FIXED_3D,
		FIXED_3D_2_FIXED
	}

	public delegate void OnCamMoveOverDelegate();

	public delegate void OnLerpComplete();

	public Vector3 DialogRelativePos = new Vector3(1.7f, 1.5f, 1.7f);

	public float mPinchSpeed = 100f;

	public float mScale = 0.3f;

	public float mPinchMax = 10f;

	public float view2_5Angle = 30f;

	public float mChangeSpeed = 6f;

	private Transform mPlayerTransform;

	private Transform mCameraRootTransform;

	private Transform mCameraTransform;

	private static CAMERAVIEWSTATE mCurrentViewState = CAMERAVIEWSTATE.FREE;

	private bool isCamCanUse;

	public float initialDistance30 = 15f;

	public float initialDistance25 = 6f;

	public float minDistance25 = 0.5f;

	public float maxDistance25 = 15f;

	public float minDistance30 = 1.5f;

	public float maxDistance30 = 12f;

	public float fixed3DDistance = 5f;

	public float yawSensitivity = 25f;

	public float pitchSensitivity = 25f;

	public bool clampYawAngle;

	public float minYaw = -75f;

	public float maxYaw = 75f;

	public bool clampPitchAngle = true;

	public float minPitch = -3f;

	public float maxPitch = 70f;

	public bool allowPinchZoom = true;

	public float pinchZoomSensitivity = 5f;

	public bool smoothMotion = true;

	public float smoothZoomSpeed = 5f;

	public float smoothOrbitSpeed = 10f;

	public bool smoothPanning = true;

	public float smoothPanningSpeed = 12f;

	private float distance = 10f;

	private float yaw;

	private float pitch;

	private float idealDistance25;

	private float idealDistance30;

	private float idealYaw;

	private float idealPitch;

	public Vector3 idealPanOffset = Vector3.zero;

	private Vector3 panOffset = Vector3.zero;

	private Quaternion targetQuation;

	private Vector3 targetPosition;

	private Quaternion orginalQuation;

	private Vector3 orginalPosition;

	private float time = 1f;

	private float changeTime = 0.5f;

	private float currentTime;

	private float mLastViewDistance = 0.7f;

	private bool mInitFlag;

	private PlayerData mPlayerData;

	public Camera CurCamera;

	private RaycastHit rayCastHitInfo;

	private float tempDistance;

	private float lastScale;

	private float tempScale;

	private bool hitFlag;

	private float preCamDistance = 3f;

	private int WALL_LAYER_VAL = 26;

	public float LookHeight = 1.2f;

	private vp_Timer.Handle enableCamHandle = new vp_Timer.Handle();

	private OnCamMoveOverDelegate onMoveOver;

	public float moveForwardDis = 2f;

	public float moveUp = 1.5f;

	public float moveRight = 1f;

	public float LookUp = 1.3f;

	public float LookRight = -1f;

	private List<CamRockInfo> mCamRockInfoList = new List<CamRockInfo>();

	private Vector3 mTempRockPos;

	private Quaternion mTempRockRot;

	public static CAMERAVIEWSTATE CurrentViewState => mCurrentViewState;

	public bool IsCamCanUse
	{
		get
		{
			return isCamCanUse;
		}
		set
		{
			isCamCanUse = value;
		}
	}

	public float IdealDistance25
	{
		get
		{
			return idealDistance25;
		}
		set
		{
			idealDistance25 = Mathf.Clamp(value, minDistance25, maxDistance25);
		}
	}

	public float IdealDistance30
	{
		get
		{
			return idealDistance30;
		}
		set
		{
			idealDistance30 = Mathf.Clamp(value, minDistance30, maxDistance30);
		}
	}

	public float Yaw => yaw;

	public float IdealYaw
	{
		get
		{
			return idealYaw;
		}
		set
		{
			idealYaw = ((!clampYawAngle) ? value : ClampAngle(value, minYaw, maxYaw));
		}
	}

	public float Pitch => pitch;

	public float IdealPitch
	{
		get
		{
			return idealPitch;
		}
		set
		{
			idealPitch = ((!clampPitchAngle) ? value : ClampAngle(value, minPitch, maxPitch));
		}
	}

	public bool InitFlag => mInitFlag;

	public void Init(CAMERAVIEWSTATE initState = CAMERAVIEWSTATE.FREE)
	{
		if (!mInitFlag)
		{
			mInitFlag = true;
			mCurrentViewState = initState;
			if (mPlayerTransform == null)
			{
				mPlayerTransform = Singleton<ObjManager>.Instance.MainPlayer.CacheTransform;
			}
			if (mCameraRootTransform == null)
			{
				mCameraTransform = Camera.mainCamera.transform;
				mCameraRootTransform = mCameraTransform.parent;
			}
			InitCameraView();
		}
	}

	private void Awake()
	{
		CurCamera = Camera.main;
		if (mPlayerData == null)
		{
			mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		}
		int cullingMask = CurCamera.cullingMask;
		cullingMask &= -4194305;
		CurCamera.cullingMask = cullingMask & -2097153;
	}

	private void Start()
	{
		Init(mPlayerData.ViewType);
	}

	private void Update()
	{
		ColliderDistanceCheck();
		switch (mCurrentViewState)
		{
		case CAMERAVIEWSTATE.FIXED:
			Update2_5_Camera();
			break;
		case CAMERAVIEWSTATE.FIXED_2_FREE:
			Fixed2Free();
			break;
		case CAMERAVIEWSTATE.FREE:
			Update3_0_Camera();
			break;
		case CAMERAVIEWSTATE.FREE_2_FIXED:
			Free2Fixed();
			break;
		case CAMERAVIEWSTATE.FIXED_3D:
			UpdateFixed3DCamera();
			break;
		}
		if (!CheckRock())
		{
		}
	}

	public void UpdateNow()
	{
		switch (mCurrentViewState)
		{
		case CAMERAVIEWSTATE.FIXED:
			Init25Camera(isForce: false);
			break;
		case CAMERAVIEWSTATE.FREE:
			Init30Camera(isForce: false);
			break;
		}
	}

	private void ColliderDistanceCheck()
	{
		Vector3 vector = mPlayerTransform.position + panOffset;
		Vector3 direction = mCameraRootTransform.position - vector;
		if (Physics.Raycast(new Ray(vector, direction), out rayCastHitInfo, preCamDistance, 1 << WALL_LAYER_VAL))
		{
			hitFlag = true;
			tempDistance = rayCastHitInfo.distance;
			if (CurrentViewState == CAMERAVIEWSTATE.FIXED || CurrentViewState == CAMERAVIEWSTATE.FREE_2_FIXED)
			{
				mScale = Mathf.Clamp01(1f - (tempDistance - minDistance25) / (maxDistance25 - minDistance25));
			}
			else if (CurrentViewState == CAMERAVIEWSTATE.FREE || CurrentViewState == CAMERAVIEWSTATE.FIXED_2_FREE)
			{
				mScale = Mathf.Clamp01(1f - (tempDistance - minDistance30) / (maxDistance30 - minDistance30));
			}
		}
		else if (hitFlag)
		{
			hitFlag = false;
			mScale = lastScale;
		}
		else
		{
			preCamDistance = distance;
			lastScale = mScale;
		}
	}

	public void ChangeToCarView()
	{
		if (mCurrentViewState == CAMERAVIEWSTATE.FREE)
		{
			mScale = 0f;
			maxDistance30 = 15f;
			ChangeCameraView(CAMERAVIEWSTATE.FREE);
			if (SingletonUnity<ExpLineRootLogic>.Exists)
			{
				SingletonUnity<ExpLineRootLogic>.Instance.UpdateViewType();
			}
		}
	}

	public void BackToNormalView()
	{
		maxDistance30 = 12f;
		if (mCurrentViewState == CAMERAVIEWSTATE.FREE)
		{
			mScale = mLastViewDistance;
		}
	}

	public void LerpToTarget(Transform targetTrans, float durationTime, OnLerpComplete onComplete = null)
	{
		base.enabled = false;
		Vector3 position = targetTrans.position;
		OnBeforeCamLerp();
		base.transform.DOMove(position, durationTime).SetEase(Ease.InOutCubic).OnComplete(delegate
		{
			if (onComplete != null)
			{
				onComplete();
			}
		})
			.SetId(0);
		base.transform.DORotate(targetTrans.eulerAngles, durationTime).SetEase(Ease.InOutCubic).SetId(0);
	}

	public void LerpToTargetLocalZero(Transform targetTrans, float durationTime, OnLerpComplete onComplete = null)
	{
		base.enabled = false;
		OnBeforeCamLerp();
		base.transform.parent = targetTrans;
		base.transform.DOLocalMove(Vector3.zero, durationTime).SetEase(Ease.InOutCubic).OnComplete(delegate
		{
			if (onComplete != null)
			{
				onComplete();
			}
		})
			.SetId(0);
		base.transform.DOLocalRotate(Vector3.zero, durationTime).SetEase(Ease.InOutCubic).SetId(0);
	}

	public void LerpToTargetPos(Vector3 targetPos, Vector3 lookTargetPos, float durationTime)
	{
		base.enabled = false;
		lookTargetPos += Vector3.up * LookHeight;
		Vector3 normalized = (lookTargetPos - targetPos).normalized;
		enableCamHandle.Cancel();
		OnBeforeCamLerp();
		base.transform.DOMove(targetPos, durationTime).SetEase(Ease.OutCubic).SetId(0);
		base.transform.DORotate(Quaternion.LookRotation(normalized).eulerAngles, durationTime).SetEase(Ease.OutCubic).SetId(0);
	}

	public void LerpBackToPlayer(float durationTime, OnLerpComplete onComplete = null)
	{
		if (base.enabled)
		{
			return;
		}
		base.transform.parent = null;
		Time.timeScale = 1f;
		targetQuation = Quaternion.Euler(pitch, yaw, 0f);
		Vector3 vector = mPlayerTransform.position + panOffset;
		Vector3 vector2 = vector - distance * (targetQuation * Vector3.forward);
		Vector3 normalized = (vector - vector2).normalized;
		enableCamHandle.Cancel();
		OnBeforeCamLerp();
		base.transform.DOMove(vector2, durationTime).SetEase(Ease.InOutCubic).OnComplete(delegate
		{
			if (onComplete != null)
			{
				onComplete();
			}
		})
			.SetId(0);
		base.transform.DORotate(Quaternion.LookRotation(normalized).eulerAngles, durationTime).SetEase(Ease.InOutCubic).SetId(0);
		vp_Timer.In(durationTime, delegate
		{
			base.enabled = true;
		}, enableCamHandle);
	}

	public void BackToPlayer(float durationTime)
	{
		if (!base.enabled)
		{
			vp_Timer.In(durationTime, delegate
			{
				IdealYaw = mPlayerTransform.eulerAngles.y;
				UpdateNow();
				base.enabled = true;
			}, enableCamHandle);
		}
	}

	public void LookAtGameObject(Transform trs, float durationTime, OnCamMoveOverDelegate moveOver = null)
	{
		base.enabled = false;
		Vector3 endValue = trs.position - trs.forward * 6f + Vector3.up * 2f;
		OnBeforeCamLerp();
		base.transform.DOMove(endValue, durationTime).SetEase(Ease.OutCubic).OnComplete(OnMoveOver)
			.OnUpdate(delegate
			{
				base.transform.LookAt(trs.position);
			})
			.SetId(0);
	}

	private void OnBeforeCamLerp()
	{
		enableCamHandle.Cancel();
		DOTween.Kill(0);
	}

	private void OnDestroy()
	{
		enableCamHandle.Cancel();
	}

	public void OnMoveOver()
	{
		if (onMoveOver != null)
		{
			onMoveOver();
		}
		onMoveOver = null;
	}

	public void LookBackToPlayer(float durationTime)
	{
		Time.timeScale = 1f;
		targetQuation = Quaternion.Euler(pitch, yaw, 0f);
		Vector3 lookAtPos = mPlayerTransform.position + panOffset;
		Vector3 endValue = lookAtPos - distance * (targetQuation * Vector3.forward);
		OnBeforeCamLerp();
		base.transform.DOMove(endValue, durationTime).SetEase(Ease.OutCubic).OnUpdate(delegate
		{
			base.transform.LookAt(lookAtPos);
		})
			.SetId(0);
		vp_Timer.In(durationTime, delegate
		{
			base.enabled = true;
		});
	}

	public void BossDieCameraEffect(ObjCharacter objCharacter)
	{
		base.enabled = false;
		Transform trs = objCharacter.transform;
		BossCameraController bossCameraController = mCameraRootTransform.gameObject.AddComponent<BossCameraController>();
		bossCameraController.Init(objCharacter);
		Time.timeScale = 0.3f;
		Vector3 normalized = (mPlayerTransform.position - trs.position).normalized;
		Vector3 endValue = trs.position + normalized * 2f + Vector3.up * 3f;
		float duration = 1.5f;
		OnBeforeCamLerp();
		base.transform.DOMove(endValue, duration).SetUpdate(isIndependentUpdate: true).SetEase(Ease.Linear)
			.OnComplete(bossCameraController.CameraArrived)
			.OnUpdate(delegate
			{
				base.transform.LookAt(trs.position);
			})
			.SetId(0);
	}

	public void FinishCopyCameraEffect(float durationTime, DelegateDefine.NoParamDelegate func)
	{
		base.enabled = false;
		Vector3 endValue = mPlayerTransform.position + mPlayerTransform.forward * moveForwardDis + mPlayerTransform.up * moveUp + mPlayerTransform.right * moveRight;
		Vector3 PlayerLookPos = mPlayerTransform.position + mPlayerTransform.up * LookUp + mPlayerTransform.right * LookRight;
		OnBeforeCamLerp();
		base.transform.DOMove(endValue, durationTime).SetEase(Ease.OutCubic).OnUpdate(delegate
		{
			base.transform.LookAt(PlayerLookPos);
		})
			.OnComplete(delegate
			{
				if (func != null)
				{
					func();
				}
			})
			.SetId(0);
	}

	private void Fixed2Free()
	{
		currentTime += Time.deltaTime;
		if (currentTime >= time)
		{
			mCurrentViewState = CAMERAVIEWSTATE.FREE;
			currentTime = time;
		}
		Vector3 vector = mPlayerTransform.position + panOffset;
		IdealDistance30 = Mathf.Lerp(maxDistance30, minDistance30, mScale);
		distance = IdealDistance30;
		Vector3 vector2 = vector - IdealDistance30 * (targetQuation * Vector3.forward);
		targetPosition = vector2;
		mCameraRootTransform.localPosition = Vector3.Lerp(orginalPosition, targetPosition, currentTime / time);
		mCameraRootTransform.rotation = Quaternion.Slerp(orginalQuation, targetQuation, currentTime / time);
	}

	private void Free2Fixed()
	{
		currentTime += Time.deltaTime;
		if (currentTime >= time)
		{
			mCurrentViewState = CAMERAVIEWSTATE.FIXED;
			currentTime = time;
		}
		Vector3 vector = mPlayerTransform.position + panOffset;
		IdealDistance25 = Mathf.Lerp(maxDistance25, minDistance25, mScale);
		distance = IdealDistance25;
		Vector3 vector2 = vector - IdealDistance25 * (targetQuation * Vector3.forward);
		targetPosition = vector2;
		mCameraRootTransform.localPosition = Vector3.Lerp(orginalPosition, targetPosition, currentTime / time);
		mCameraRootTransform.rotation = Quaternion.Slerp(orginalQuation, targetQuation, currentTime / time);
	}

	private void InitCameraView()
	{
		mCameraTransform.localPosition = Vector3.zero;
		mCameraTransform.localRotation = Quaternion.identity;
		idealPanOffset = new Vector3(0f, 1.5f, 0f);
		if (mPlayerData.IsServerRidingMount)
		{
			IdealYaw = mPlayerTransform.eulerAngles.y;
			IdealPitch = 15f;
			ChangeToCarView();
		}
		else if (mCurrentViewState == CAMERAVIEWSTATE.FIXED)
		{
			mScale = 0f;
			Vector3 eulerAngles = mCameraRootTransform.eulerAngles;
			IdealDistance30 = initialDistance25;
			IdealYaw = eulerAngles.y;
			IdealPitch = eulerAngles.x;
			IdealYaw = mPlayerTransform.eulerAngles.y;
			Init25Camera();
		}
		else if (mCurrentViewState == CAMERAVIEWSTATE.FREE)
		{
			mLastViewDistance = LocalDataSaveManager.GetLastViewDistance();
			mScale = mLastViewDistance;
			Vector3 eulerAngles2 = mCameraRootTransform.eulerAngles;
			IdealDistance30 = initialDistance30;
			IdealYaw = mPlayerTransform.eulerAngles.y;
			IdealPitch = 15f;
			Init30Camera();
		}
		else if (mCurrentViewState == CAMERAVIEWSTATE.FIXED_3D)
		{
			IdealYaw = mPlayerTransform.eulerAngles.y;
			IdealPitch = 15f;
			Singleton<ObjManager>.Instance.MainPlayer.HideHeadInfo();
		}
	}

	public void ChangeCameraView(CAMERAVIEWSTATE viewState)
	{
		if (viewState != mCurrentViewState)
		{
			switch (viewState)
			{
			case CAMERAVIEWSTATE.FREE:
			{
				if (Singleton<ObjManager>.Instance.MainPlayer.IsDrivingMount())
				{
					mScale = 0f;
				}
				else
				{
					mScale = mLastViewDistance;
				}
				Vector3 eulerAngles2 = mCameraRootTransform.eulerAngles;
				IdealDistance30 = initialDistance30;
				IdealYaw = eulerAngles2.y;
				IdealPitch = 15f;
				orginalPosition = mCameraRootTransform.localPosition;
				orginalQuation = mCameraRootTransform.rotation;
				mCurrentViewState = CAMERAVIEWSTATE.FREE;
				mPlayerData.ViewType = viewState;
				LocalDataSaveManager.SetCameraViewType(PlayerData.MainPlayerServerId, viewState);
				Singleton<ObjManager>.Instance.MainPlayer.ShowHeadInfo();
				break;
			}
			case CAMERAVIEWSTATE.FIXED:
			{
				mScale = 0f;
				IdealDistance25 = initialDistance25;
				Vector3 eulerAngles = mCameraRootTransform.eulerAngles;
				IdealYaw = eulerAngles.y;
				IdealPitch = eulerAngles.x;
				orginalPosition = mCameraRootTransform.localPosition;
				orginalQuation = mCameraRootTransform.rotation;
				mCurrentViewState = CAMERAVIEWSTATE.FIXED;
				mPlayerData.ViewType = viewState;
				LocalDataSaveManager.SetCameraViewType(PlayerData.MainPlayerServerId, viewState);
				Singleton<ObjManager>.Instance.MainPlayer.ShowHeadInfo();
				break;
			}
			case CAMERAVIEWSTATE.FIXED_3D:
				mCurrentViewState = CAMERAVIEWSTATE.FIXED_3D;
				mPlayerData.ViewType = viewState;
				LocalDataSaveManager.SetCameraViewType(PlayerData.MainPlayerServerId, viewState);
				Singleton<ObjManager>.Instance.MainPlayer.HideHeadInfo();
				break;
			}
		}
		SingletonDontDestoryUnity<GameManager>.Instance.SceneManager?.UpdateDamgeBoadScale();
	}

	private void Init25Camera(bool isForce = true)
	{
		IdealDistance25 = Mathf.Lerp(maxDistance25, minDistance25, mScale);
		distance = IdealDistance25;
		yaw = IdealYaw;
		pitch = view2_5Angle;
		panOffset = idealPanOffset;
		if (!(Singleton<ObjManager>.Instance.MainPlayer == null))
		{
			targetQuation = Quaternion.Euler(pitch, yaw, 0f);
			Vector3 vector = mPlayerTransform.position + panOffset;
			Vector3 vector2 = vector - distance * (targetQuation * Vector3.forward);
			targetPosition = vector2;
			if (isForce)
			{
				mCameraRootTransform.position = targetPosition;
				mCameraRootTransform.rotation = targetQuation;
			}
		}
	}

	private void Update2_5_Camera()
	{
		if (!(Singleton<ObjManager>.Instance.MainPlayer == null))
		{
			IdealDistance25 = Mathf.Lerp(maxDistance25, minDistance25, mScale);
			if (smoothMotion)
			{
				distance = Mathf.Lerp(distance, IdealDistance25, Time.deltaTime * smoothZoomSpeed);
				yaw = Mathf.LerpAngle(yaw, IdealYaw, Time.deltaTime * smoothOrbitSpeed);
				pitch = Mathf.LerpAngle(pitch, view2_5Angle, Time.deltaTime * smoothOrbitSpeed);
			}
			else
			{
				distance = IdealDistance25;
				yaw = IdealYaw;
				pitch = view2_5Angle;
			}
			if (smoothPanning)
			{
				panOffset = Vector3.Lerp(panOffset, idealPanOffset, Time.deltaTime * smoothPanningSpeed);
			}
			else
			{
				panOffset = idealPanOffset;
			}
			targetQuation = Quaternion.Euler(pitch, yaw, 0f);
			mCameraRootTransform.rotation = targetQuation;
			Vector3 vector = mPlayerTransform.position + panOffset;
			Vector3 vector2 = vector - distance * (targetQuation * Vector3.forward);
			targetPosition = vector2;
			mCameraRootTransform.position = targetPosition;
		}
	}

	private void Init30Camera(bool isForce = true)
	{
		IdealDistance30 = Mathf.Lerp(maxDistance30, minDistance30, mScale);
		distance = IdealDistance30;
		yaw = IdealYaw;
		pitch = IdealPitch;
		panOffset = idealPanOffset;
		if (!(Singleton<ObjManager>.Instance.MainPlayer == null))
		{
			targetQuation = Quaternion.Euler(pitch, yaw, 0f);
			Vector3 vector = mPlayerTransform.position + panOffset;
			Vector3 vector2 = vector - distance * (targetQuation * Vector3.forward);
			targetPosition = vector2;
			if (isForce)
			{
				mCameraRootTransform.position = targetPosition;
				mCameraRootTransform.rotation = targetQuation;
			}
		}
	}

	private void Update3_0_Camera()
	{
		if (!(Singleton<ObjManager>.Instance.MainPlayer == null))
		{
			IdealDistance30 = Mathf.Lerp(maxDistance30, minDistance30, mScale);
			if (smoothMotion)
			{
				distance = Mathf.Lerp(distance, IdealDistance30, Time.deltaTime * smoothZoomSpeed);
				yaw = Mathf.LerpAngle(yaw, IdealYaw, Time.deltaTime * smoothOrbitSpeed);
				pitch = Mathf.LerpAngle(pitch, IdealPitch, Time.deltaTime * smoothOrbitSpeed);
			}
			else
			{
				distance = IdealDistance30;
				yaw = IdealYaw;
				pitch = IdealPitch;
			}
			if (smoothPanning)
			{
				panOffset = Vector3.Lerp(panOffset, idealPanOffset, Time.deltaTime * smoothPanningSpeed);
			}
			else
			{
				panOffset = idealPanOffset;
			}
			targetQuation = Quaternion.Euler(pitch, yaw, 0f);
			mCameraRootTransform.rotation = targetQuation;
			Vector3 vector = mPlayerTransform.position + panOffset;
			Vector3 vector2 = vector - distance * (targetQuation * Vector3.forward);
			targetPosition = vector2;
			mCameraRootTransform.position = targetPosition;
		}
	}

	private void UpdateFixed3DCamera()
	{
		if (!(Singleton<ObjManager>.Instance.MainPlayer == null))
		{
			if (smoothMotion)
			{
				distance = Mathf.Lerp(distance, fixed3DDistance, Time.deltaTime * smoothZoomSpeed);
				yaw = Mathf.LerpAngle(yaw, IdealYaw, Time.deltaTime * smoothOrbitSpeed);
				pitch = Mathf.LerpAngle(pitch, IdealPitch, Time.deltaTime * smoothOrbitSpeed);
			}
			else
			{
				distance = IdealDistance30;
				yaw = IdealYaw;
				pitch = IdealPitch;
			}
			if (smoothPanning)
			{
				panOffset = Vector3.Lerp(panOffset, idealPanOffset, Time.deltaTime * smoothPanningSpeed);
			}
			else
			{
				panOffset = idealPanOffset;
			}
			targetQuation = Quaternion.Euler(pitch, yaw, 0f);
			mCameraRootTransform.rotation = targetQuation;
			Vector3 vector = mPlayerTransform.position + panOffset;
			Vector3 vector2 = vector - distance * (targetQuation * Vector3.forward);
			targetPosition = vector2;
			mCameraRootTransform.position = targetPosition;
		}
	}

	public void OnPinchCamera(PinchGesture gesture)
	{
		if (!SingletonUnity<JoyStickLogic>.Exists || !isCamCanUse || SingletonUnity<JoyStickLogic>.Instance.JoyStickUse || mCurrentViewState == CAMERAVIEWSTATE.FIXED || mCurrentViewState == CAMERAVIEWSTATE.FIXED_3D)
		{
			return;
		}
		if (gesture.Phase == ContinuousGesturePhase.Updated)
		{
			float num = Mathf.Abs(gesture.Delta);
			if (num > mPinchMax)
			{
				num = mPinchMax;
			}
			float num2 = num * gesture.Delta / Mathf.Abs(gesture.Delta);
			mScale += num2 * (0.5f + 0.5f * mScale) / mPinchSpeed;
			if (mScale > 1f)
			{
				mScale = 1f;
			}
			if (mScale < 0f)
			{
				mScale = 0f;
			}
		}
		else if (gesture.Phase == ContinuousGesturePhase.Ended)
		{
			mLastViewDistance = mScale;
			LocalDataSaveManager.SetLastViewDistance(mLastViewDistance);
		}
	}

	public void OnDragCamera(DragGesture gesture)
	{
		if (isCamCanUse && base.enabled)
		{
			IdealYaw += gesture.DeltaMove.x.Centimeters() * yawSensitivity;
			IdealPitch -= gesture.DeltaMove.y.Centimeters() * pitchSensitivity;
		}
	}

	public void OnDragCamera(Vector2 delta)
	{
		if (isCamCanUse)
		{
			IdealYaw += delta.x.Centimeters() * yawSensitivity;
			IdealPitch -= delta.y.Centimeters() * pitchSensitivity;
		}
	}

	public void StartDeathEffect()
	{
	}

	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	public void ClearByRockId(string rockId)
	{
		for (int i = 0; i < mCamRockInfoList.Count; i++)
		{
			CamRockInfo camRockInfo = mCamRockInfoList[i];
			if (camRockInfo.CamRockId == rockId)
			{
				camRockInfo.Init();
				mCamRockInfoList[i] = camRockInfo;
			}
		}
	}

	public void AddCamRock(string rockId)
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Screen_Vibrating != 0f)
		{
			CamRockData camRockDataByID = DataManager.GetCamRockDataByID(rockId);
			if (Random.Range(0, 100) <= camRockDataByID.RockRate && camRockDataByID != null)
			{
				camRockDataByID.Init();
				CamRockInfo camRockInfo = new CamRockInfo();
				camRockInfo.Init();
				camRockInfo.CamRockId = rockId;
				camRockInfo.NeedRockTime = camRockDataByID.NeedRockTimeSecond;
				camRockInfo.DelayTime = camRockDataByID.DelayTimeSecond;
				camRockInfo.XPosCurve = camRockDataByID.XPosCurve;
				camRockInfo.YPosCurve = camRockDataByID.YPosCurve;
				camRockInfo.ZPosCurve = camRockDataByID.ZPosCurve;
				camRockInfo.XRotCurve = camRockDataByID.XRotCurve;
				camRockInfo.YRotCurve = camRockDataByID.YRotCurve;
				camRockInfo.ZRotCurve = camRockDataByID.ZRotCurve;
				camRockInfo.WRotCurve = camRockDataByID.WRotCurve;
				mCamRockInfoList.Add(camRockInfo);
			}
		}
	}

	private bool CheckRock()
	{
		bool result = false;
		if (mCamRockInfoList.Count > 0)
		{
			mTempRockPos = Vector3.zero;
			mTempRockRot = Quaternion.identity;
			CamRockInfo camRockInfo = null;
			for (int i = 0; i < mCamRockInfoList.Count; i++)
			{
				camRockInfo = mCamRockInfoList[i];
				if (camRockInfo.IsValid())
				{
					if (camRockInfo.DelayTime > 0f)
					{
						camRockInfo.DelayTime -= Time.deltaTime;
						continue;
					}
					if (camRockInfo.RockTime > camRockInfo.NeedRockTime)
					{
						camRockInfo.Init();
						continue;
					}
					camRockInfo.RockTime += Time.deltaTime;
					RockCam(camRockInfo);
					result = true;
				}
			}
			mCameraTransform.localPosition = mTempRockPos;
			mCameraTransform.localRotation = mTempRockRot;
			for (int num = mCamRockInfoList.Count - 1; num >= 0; num--)
			{
				if (!mCamRockInfoList[num].IsValid())
				{
					mCamRockInfoList.RemoveAt(num);
				}
			}
		}
		return result;
	}

	private void RockCam(CamRockInfo rockInfo)
	{
		float x = rockInfo.XPosCurve.Evaluate(rockInfo.RockTime);
		float y = rockInfo.YPosCurve.Evaluate(rockInfo.RockTime);
		float z = rockInfo.ZPosCurve.Evaluate(rockInfo.RockTime);
		mTempRockPos += new Vector3(x, y, z);
		float x2 = rockInfo.XRotCurve.Evaluate(rockInfo.RockTime);
		float y2 = rockInfo.YRotCurve.Evaluate(rockInfo.RockTime);
		float z2 = rockInfo.ZRotCurve.Evaluate(rockInfo.RockTime);
		float w = rockInfo.WRotCurve.Evaluate(rockInfo.RockTime);
		mTempRockRot *= new Quaternion(x2, y2, z2, w);
	}

	public void ClearCamRock()
	{
		mCamRockInfoList.Clear();
		mTempRockPos = Vector3.zero;
		mTempRockRot = Quaternion.identity;
		mCameraTransform.localPosition = Vector3.zero;
		mCameraTransform.localRotation = Quaternion.identity;
	}

	public void DisableCamera()
	{
		base.enabled = false;
	}
}
