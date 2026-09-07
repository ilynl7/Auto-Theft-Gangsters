using System;
using UnityEngine;

public class CarControllerRootLogic : SingletonUnity<CarControllerRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UIEventListener AccelBtnListener;

	public UIEventListener BrakeBtnListener;

	public UIEventListener LeftBtnListener;

	public UIEventListener RightBtnListener;

	public Transform LeftAnchorRoot;

	public Transform RightAnchorRoot;

	public GameObject ResetBtnRoot;

	private SceneManager mCarSceneManager;

	private ObjPlayerCar mPlayerCar;

	private bool mIsUsingGravity;

	private float lastClickTime;

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

	private new void Awake()
	{
		base.Awake();
		UIEventListener accelBtnListener = AccelBtnListener;
		accelBtnListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(accelBtnListener.onPress, new UIEventListener.BoolDelegate(OnPressAccBtn));
		UIEventListener brakeBtnListener = BrakeBtnListener;
		brakeBtnListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(brakeBtnListener.onPress, new UIEventListener.BoolDelegate(OnPressBrakeBtn));
		UIEventListener leftBtnListener = LeftBtnListener;
		leftBtnListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(leftBtnListener.onPress, new UIEventListener.BoolDelegate(OnPressLeftBtn));
		UIEventListener rightBtnListener = RightBtnListener;
		rightBtnListener.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(rightBtnListener.onPress, new UIEventListener.BoolDelegate(OnPressRightBtn));
		mCarSceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
	}

	private void OnEnable()
	{
		if (mCarSceneManager.IsTutorialScene())
		{
			mPlayerCar = Singleton<ObjManager>.Instance.MainPlayer.CurPlayerCar;
		}
		else
		{
			mPlayerCar = Singleton<ObjManager>.Instance.MainPlayerCar;
		}
	}

	public void Reset()
	{
		mIsUsingGravity = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.StreetRacingMode == 0;
		if (mIsUsingGravity)
		{
			NGUITools.SetActive(LeftBtnListener.gameObject, state: false);
			NGUITools.SetActive(RightBtnListener.gameObject, state: false);
			AccelBtnListener.transform.localPosition = new Vector3(-105f, 95f, 0f);
			BrakeBtnListener.transform.parent = LeftAnchorRoot;
			BrakeBtnListener.transform.localPosition = new Vector3(105f, 95f, 0f);
		}
		else
		{
			NGUITools.SetActive(LeftBtnListener.gameObject, state: true);
			NGUITools.SetActive(RightBtnListener.gameObject, state: true);
			AccelBtnListener.transform.localPosition = new Vector3(-64.99997f, 141.79f, 0f);
			BrakeBtnListener.transform.parent = RightAnchorRoot;
			BrakeBtnListener.transform.localPosition = new Vector3(-185f, 60f, 0f);
		}
	}

	public void OnPressLeftBtn(GameObject obj, bool isPress)
	{
		if (!mCarSceneManager.IsMissionStart)
		{
			return;
		}
		if (mPlayerCar == null)
		{
			if (mCarSceneManager.IsTutorialScene())
			{
				mPlayerCar = Singleton<ObjManager>.Instance.MainPlayer.CurPlayerCar;
			}
			else
			{
				mPlayerCar = Singleton<ObjManager>.Instance.MainPlayerCar;
			}
			if (mPlayerCar == null)
			{
				return;
			}
		}
		mPlayerCar.OnPressLeftBtn(isPress);
	}

	public void OnPressRightBtn(GameObject obj, bool isPress)
	{
		if (!mCarSceneManager.IsMissionStart)
		{
			return;
		}
		if (mPlayerCar == null)
		{
			if (mCarSceneManager.IsTutorialScene())
			{
				mPlayerCar = Singleton<ObjManager>.Instance.MainPlayer.CurPlayerCar;
			}
			else
			{
				mPlayerCar = Singleton<ObjManager>.Instance.MainPlayerCar;
			}
			if (mPlayerCar == null)
			{
				return;
			}
		}
		mPlayerCar.OnPressRightBtn(isPress);
	}

	public void OnPressAccBtn(GameObject obj, bool isPress)
	{
		if (!mCarSceneManager.IsMissionStart)
		{
			return;
		}
		if (mPlayerCar == null)
		{
			if (mCarSceneManager.IsTutorialScene())
			{
				mPlayerCar = Singleton<ObjManager>.Instance.MainPlayer.CurPlayerCar;
			}
			else
			{
				mPlayerCar = Singleton<ObjManager>.Instance.MainPlayerCar;
			}
			if (mPlayerCar == null)
			{
				return;
			}
		}
		mPlayerCar.OnPressAccelBtn(isPress);
	}

	public void OnPressBrakeBtn(GameObject obj, bool isPress)
	{
		if (!mCarSceneManager.IsMissionStart)
		{
			return;
		}
		if (mPlayerCar == null)
		{
			if (mCarSceneManager.IsTutorialScene())
			{
				mPlayerCar = Singleton<ObjManager>.Instance.MainPlayer.CurPlayerCar;
			}
			else
			{
				mPlayerCar = Singleton<ObjManager>.Instance.MainPlayerCar;
			}
			if (mPlayerCar == null)
			{
				return;
			}
		}
		mPlayerCar.OnPressBrakeBtn(isPress);
	}

	public void OnClickResetBtn()
	{
		if (mCarSceneManager.IsMissionStart && !(Time.time - lastClickTime < 1f))
		{
			lastClickTime = Time.time;
			if (mCarSceneManager.CurrentMapInofData.MapType == MAPTYPE.CAR_CHASE_COPY)
			{
				(mCarSceneManager as CarChaseSceneManager).ResetPlayerCarToLastPoint();
			}
			else if (mCarSceneManager.IsTutorialScene())
			{
				ResetPlayerCarPos();
			}
		}
	}

	public void ResetPlayerCarPos()
	{
		if (mPlayerCar == null)
		{
			if (mCarSceneManager.IsTutorialScene())
			{
				mPlayerCar = Singleton<ObjManager>.Instance.MainPlayer.CurPlayerCar;
			}
			else
			{
				mPlayerCar = Singleton<ObjManager>.Instance.MainPlayerCar;
			}
			if (mPlayerCar == null)
			{
				return;
			}
		}
		mPlayerCar.rigidbody.velocity = Vector3.zero;
		mPlayerCar.rigidbody.angularVelocity = Vector3.zero;
		bool isHit = false;
		float hitHeight = SceneManager.GetHitHeight(mPlayerCar.transform.position, out isHit);
		if (isHit)
		{
			mPlayerCar.transform.position = new Vector3(mPlayerCar.transform.position.x, hitHeight, mPlayerCar.transform.position.z) + Vector3.up;
		}
		else
		{
			Vector3 birthPosVector = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.BirthPosVector3;
			mPlayerCar.transform.position = new Vector3(birthPosVector.x, SceneManager.GetHitHeight(birthPosVector), birthPosVector.z) + Vector3.up;
		}
		mPlayerCar.transform.eulerAngles = new Vector3(0f, mPlayerCar.transform.eulerAngles.y, 0f);
	}

	public void TutorialShowAllBtn()
	{
		NGUITools.SetActive(ResetBtnRoot, state: true);
		NGUITools.SetActive(LeftBtnListener.gameObject, state: true);
		NGUITools.SetActive(RightBtnListener.gameObject, state: true);
		NGUITools.SetActive(BrakeBtnListener.gameObject, state: true);
	}
}
