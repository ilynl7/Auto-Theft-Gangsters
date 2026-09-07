using UnityEngine;

public class InputController : SingletonUnity<InputController>
{
	private PinchRecognizer mPinchRecognizer;

	private DragRecognizer mDragRecognizer;

	private LongPressRecognizer mLongPressRecognizer;

	private SwipeRecognizer mSwipeRecognizer;

	private TapRecognizer mTapRecognizer;

	private TwistRecognizer mTwistRecognizer;

	private FingerDownDetector mFingerDownDetector;

	private FingerHoverDetector mFingerHoverDetector;

	private FingerMotionDetector mFingerMotionDetector;

	private FingerUpDetector mFingerUpDetector;

	private ScreenRaycaster mScreenRaycaster;

	private ObjMainPlayer mMainPlayer;

	private Camera mainCamera;

	private int dragFingerIndex = -1;

	public PinchRecognizer PinchRecognizer => mPinchRecognizer;

	public DragRecognizer DragRecognizer => mDragRecognizer;

	public LongPressRecognizer LongPressRecognizer => mLongPressRecognizer;

	public SwipeRecognizer SwipeRecognizer => mSwipeRecognizer;

	public TapRecognizer TapRecognizer => mTapRecognizer;

	public TwistRecognizer TwistRecognizer => mTwistRecognizer;

	public FingerDownDetector FingerDownDetector => mFingerDownDetector;

	public FingerHoverDetector FingerHoverDetector => mFingerHoverDetector;

	public FingerMotionDetector FingerMotionDetector => mFingerMotionDetector;

	public FingerUpDetector FingerUpDetector => mFingerUpDetector;

	public ScreenRaycaster ScreenRaycaster => mScreenRaycaster;

	public ObjMainPlayer MainPlayer
	{
		get
		{
			if (mMainPlayer == null)
			{
				mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			}
			return mMainPlayer;
		}
	}

	private new void Awake()
	{
		base.Awake();
		Init();
	}

	private void Init()
	{
		if (mPinchRecognizer == null)
		{
			mPinchRecognizer = base.gameObject.AddComponent<PinchRecognizer>();
			mPinchRecognizer.OnGesture += OnPinch;
			mPinchRecognizer.UseSendMessage = false;
		}
		if (mDragRecognizer == null)
		{
			mDragRecognizer = base.gameObject.AddComponent<DragRecognizer>();
			mDragRecognizer.OnGesture += OnDrag;
			mDragRecognizer.UseSendMessage = false;
			mDragRecognizer.MaxSimultaneousGestures = 2;
		}
		if (mLongPressRecognizer == null)
		{
			mLongPressRecognizer = base.gameObject.AddComponent<LongPressRecognizer>();
			mLongPressRecognizer.OnGesture += OnLongPress;
		}
		if (mSwipeRecognizer == null)
		{
			mSwipeRecognizer = base.gameObject.AddComponent<SwipeRecognizer>();
			mSwipeRecognizer.OnGesture += OnSwipe;
		}
		if (mTapRecognizer == null)
		{
			mTapRecognizer = base.gameObject.AddComponent<TapRecognizer>();
			mTapRecognizer.OnGesture += OnTap;
		}
		if (mTwistRecognizer == null)
		{
			mTwistRecognizer = base.gameObject.AddComponent<TwistRecognizer>();
			mTwistRecognizer.OnGesture += OnTwist;
		}
		if (mFingerDownDetector == null)
		{
			mFingerDownDetector = base.gameObject.AddComponent<FingerDownDetector>();
			mFingerDownDetector.OnFingerDown += OnFingerDown;
		}
		if (mFingerHoverDetector == null)
		{
			mFingerHoverDetector = base.gameObject.AddComponent<FingerHoverDetector>();
			mFingerHoverDetector.OnFingerHover += OnFingerHover;
		}
		if (mFingerMotionDetector == null)
		{
			mFingerMotionDetector = base.gameObject.AddComponent<FingerMotionDetector>();
			mFingerMotionDetector.OnFingerMove += OnFingerMove;
			mFingerMotionDetector.OnFingerStationary += OnFingerStationary;
		}
		if (mFingerUpDetector == null)
		{
			mFingerUpDetector = base.gameObject.AddComponent<FingerUpDetector>();
			mFingerUpDetector.OnFingerUp += OnFingerUp;
		}
	}

	private void Start()
	{
		if (mMainPlayer == null)
		{
			mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		mainCamera = Camera.main;
	}

	public void OnPinch(PinchGesture gesture)
	{
		if (MainPlayer != null)
		{
			MainPlayer.CameraController.OnPinchCamera(gesture);
		}
	}

	public void OnDrag(DragGesture gesture)
	{
		if (gesture.Phase == ContinuousGesturePhase.Started && dragFingerIndex == -1)
		{
			if (MainPlayer != null && MainPlayer.CameraController.IsCamCanUse)
			{
				dragFingerIndex = gesture.Fingers[0].Index;
			}
		}
		else
		{
			if (gesture.Fingers[0].Index != dragFingerIndex)
			{
				return;
			}
			if (gesture.Phase == ContinuousGesturePhase.Updated)
			{
				if (MainPlayer != null)
				{
					MainPlayer.CameraController.OnDragCamera(gesture);
				}
			}
			else
			{
				dragFingerIndex = -1;
			}
		}
	}

	public void OnLongPress(LongPressGesture gesture)
	{
	}

	public void OnSwipe(SwipeGesture gesture)
	{
	}

	public void OnTap(TapGesture gesture)
	{
		if (mainCamera == null)
		{
			mainCamera = Camera.main;
			if (mainCamera == null)
			{
				return;
			}
		}
		if (mMainPlayer == null)
		{
			mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			if (mMainPlayer == null)
			{
				return;
			}
		}
		if (mMainPlayer.CameraController.IsCamCanUse)
		{
			Ray ray = mainCamera.ScreenPointToRay(gesture.Position);
			CastScreenRay(ray, gesture);
		}
	}

	public static bool IsMouseOverUI(Vector2 pos)
	{
		RaycastHit hit = default(RaycastHit);
		Vector3 inPos = UICamera.currentCamera.ScreenToWorldPoint(pos);
		GameObject gameObject = ((!UICamera.Raycast(inPos, out hit)) ? null : UICamera.lastHit.collider.gameObject);
		if (gameObject != null)
		{
			return true;
		}
		return false;
	}

	private void CastScreenRay(Ray ray, TapGesture gesture)
	{
		int num = 67108864;
		num = ~num;
		RaycastHit hitInfo = default(RaycastHit);
		if (!Physics.Raycast(ray, out hitInfo, 50f, num))
		{
			return;
		}
		GameObject root = NGUITools.GetRoot(hitInfo.collider.gameObject);
		ObjCharacter component = root.GetComponent<ObjCharacter>();
		if (!(component != null))
		{
			return;
		}
		if (component.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			Ray ray2 = new Ray(hitInfo.point + ray.direction * 0.01f, ray.direction);
			CastScreenRay(ray2, gesture);
			return;
		}
		mMainPlayer.SelectTarget(component);
		if (component.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
		{
			ObjOtherPlayer objOtherPlayer = component as ObjOtherPlayer;
			TargetBasicInfo selectTargetBasicInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SelectTargetBasicInfo;
			selectTargetBasicInfo.ResetInfo(objOtherPlayer.ServerId, objOtherPlayer.AttributeData.Level, objOtherPlayer.AttributeData.ComboValue, objOtherPlayer.AttributeData.Name, objOtherPlayer.Profession, 1, objOtherPlayer.AttributeData.GuildId, objOtherPlayer.AttributeData.GuildName, gesture.Position);
			HitOtherPLayerLogic.ShowMenu(HitType.HitOtherPlayer, selectTargetBasicInfo);
			CampTool.TipsCanSelectAttackPlayer(mMainPlayer, objOtherPlayer);
		}
	}

	public void OnTwist(TwistGesture gesture)
	{
	}

	public void OnFingerDown(FingerDownEvent e)
	{
	}

	public void OnFingerHover(FingerHoverEvent e)
	{
	}

	public void OnFingerMove(FingerMotionEvent e)
	{
	}

	public void OnFingerStationary(FingerMotionEvent e)
	{
	}

	public void OnFingerUp(FingerUpEvent e)
	{
	}
}
