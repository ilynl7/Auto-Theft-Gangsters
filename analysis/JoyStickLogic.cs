using UnityEngine;

public class JoyStickLogic : SingletonUnity<JoyStickLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	private Vector3 mCenterPosition = Vector3.zero;

	public float MaxRadius = 100f;

	private Vector3 mTouchOffset = Vector3.zero;

	public GameObject insideCircle;

	public GameObject outSideCircle;

	private int mTouchID = -1;

	private bool mPressed;

	private Plane mPlane;

	public UISprite IconSprite;

	private TweenPosition tweenPosition;

	private ThirdPersonController thirdPersonController;

	public int TouchID => mTouchID;

	public bool JoyStickUse => mPressed;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	private void Start()
	{
	}

	private void OnEnable()
	{
		HidePic();
		mPressed = false;
		base.transform.localPosition = Vector3.zero;
	}

	public void HidePic()
	{
		int limitlevel = 10;
		FunctionData functionDataById = DataManager.GetFunctionDataById(4086.ToString());
		if (functionDataById != null)
		{
			limitlevel = functionDataById.Condition;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(limitlevel))
		{
			UnityVersionUtil.SetActiveRecursive(insideCircle.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(outSideCircle.gameObject, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(insideCircle.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(outSideCircle.gameObject, state: true);
			outSideCircle.transform.localPosition = Vector3.zero;
		}
	}

	public void ShowPic()
	{
		UnityVersionUtil.SetActiveRecursive(insideCircle.gameObject, state: true);
		UnityVersionUtil.SetActiveRecursive(outSideCircle.gameObject, state: true);
	}

	private void Update()
	{
		UpdateControler();
	}

	private void StartMove()
	{
		if (tweenPosition == null)
		{
			tweenPosition = GetComponent<TweenPosition>();
		}
		tweenPosition.enabled = false;
		IconSprite.alpha = 1f;
		if (TutorialManager.CurStep == TUTORIAL_STEP.JOYSTICK_START)
		{
			CheckTutorialEvent();
		}
	}

	private void EndMove()
	{
		if (tweenPosition == null)
		{
			tweenPosition = GetComponent<TweenPosition>();
		}
		tweenPosition.from = base.transform.localPosition;
		tweenPosition.ResetToBeginning();
		tweenPosition.PlayForward();
		IconSprite.alpha = 0.5f;
	}

	private void OnPress(bool pressed)
	{
		if (thirdPersonController == null)
		{
			return;
		}
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
		if (!base.enabled || !NGUITools.GetActive(base.gameObject))
		{
			return;
		}
		if (pressed)
		{
			if (!mPressed)
			{
				mTouchID = UICamera.currentTouchID;
				mPressed = true;
				Transform transform = UICamera.currentCamera.transform;
				mPlane = new Plane(transform.rotation * Vector3.back, UICamera.lastHit.point);
				Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
				float enter = 0f;
				if (mPlane.Raycast(ray, out enter))
				{
					Vector3 point = ray.GetPoint(enter);
					base.transform.position = point;
					outSideCircle.transform.position = point;
					mCenterPosition = base.transform.localPosition;
					mTouchOffset = point - base.transform.position;
					StartMove();
				}
				thirdPersonController.IsJoyStickPress = true;
				ShowPic();
			}
		}
		else if (mPressed && mTouchID == UICamera.currentTouchID)
		{
			thirdPersonController.IsJoyStickPress = false;
			mPressed = false;
			mTouchID = -1;
			EndMove();
			HidePic();
		}
	}

	private void OnDrag(Vector2 delta)
	{
		if (mPressed && mTouchID == UICamera.currentTouchID && base.enabled && NGUITools.GetActive(base.gameObject))
		{
			UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
			Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
			float enter = 0f;
			if (mPlane.Raycast(ray, out enter))
			{
				Vector3 point = ray.GetPoint(enter);
				UpdatePosition(point);
			}
		}
	}

	private void UpdatePosition(Vector3 pos)
	{
		Vector3 v = pos - mTouchOffset;
		Vector3 vector = base.transform.parent.transform.worldToLocalMatrix.MultiplyPoint(v);
		float num = Vector3.SqrMagnitude(vector - mCenterPosition);
		if (num > MaxRadius * MaxRadius)
		{
			float num2 = Mathf.Sqrt(num);
			vector = Vector3.Lerp(mCenterPosition, vector, MaxRadius / num2);
		}
		base.transform.localPosition = vector;
	}

	private void UpdateControler()
	{
		if (thirdPersonController == null)
		{
			if (!(Singleton<ObjManager>.Instance.MainPlayer != null))
			{
				return;
			}
			thirdPersonController = Singleton<ObjManager>.Instance.MainPlayer.ThirdPersonController;
		}
		if (thirdPersonController.IsJoyStickPress)
		{
			thirdPersonController.HorizonRaw = (base.transform.localPosition.y - mCenterPosition.y) / MaxRadius;
			thirdPersonController.VerticalRaw = (base.transform.localPosition.x - mCenterPosition.x) / MaxRadius;
		}
	}

	private void OnDisable()
	{
		if (thirdPersonController != null)
		{
			thirdPersonController.IsJoyStickPress = false;
			thirdPersonController.HorizonRaw = 0f;
			thirdPersonController.VerticalRaw = 0f;
		}
	}

	public void MoveOutScreen()
	{
		base.transform.localPosition = Vector3.zero;
		if (thirdPersonController != null)
		{
			thirdPersonController.IsJoyStickPress = false;
			thirdPersonController.IsMoving = false;
			thirdPersonController.HorizonRaw = 0f;
			thirdPersonController.VerticalRaw = 0f;
		}
		mPressed = false;
		mTouchID = -1;
		HidePic();
	}
}
