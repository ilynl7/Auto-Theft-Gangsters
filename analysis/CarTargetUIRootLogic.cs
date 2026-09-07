using System;
using System.Text;
using UnityEngine;

public class CarTargetUIRootLogic : SingletonUnity<CarTargetUIRootLogic>
{
	public UISprite targetPic;

	public UISprite ArrowPic;

	public UILabel DisLabel;

	public GameObject mTargetObj;

	private int screenWidth;

	private Camera mMainCamera;

	private GameObject mSourceObj;

	private Vector3 tartgetPos;

	private bool isNeedUpdate;

	private Transform targetTransform;

	private Transform sourcesTransform;

	private StringBuilder sb = new StringBuilder();

	private new void Awake()
	{
		base.Awake();
		screenWidth = Mathf.RoundToInt(480f * ((float)Screen.width / (float)Screen.height));
		mMainCamera = Camera.main;
		targetTransform = targetPic.transform;
	}

	private void Update()
	{
		if (mSourceObj == null)
		{
			return;
		}
		if (UnityVersionUtil.IsActive(mTargetObj))
		{
			if (!UnityVersionUtil.IsActive(targetPic.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(targetPic.gameObject, state: true);
				return;
			}
			Vector3 vector = mMainCamera.WorldToViewportPoint(tartgetPos + Vector3.up * 0.5f);
			Vector3 zero = Vector3.zero;
			if (vector.z > 0f)
			{
				zero = new Vector3(Mathf.Clamp(vector.x * (float)screenWidth, 30f, screenWidth - 30), Mathf.Clamp(vector.y * 480f, 10f, 470f), 0f);
			}
			else
			{
				vector.x = 1f - vector.x;
				zero = new Vector3(Mathf.Clamp(vector.x * (float)screenWidth, 30f, screenWidth - 30), 10f, 0f);
			}
			ArrowPic.transform.localPosition = zero;
			if (zero.x >= (float)(screenWidth - 30))
			{
				ArrowPic.transform.localEulerAngles = Vector3.zero;
				targetPic.transform.localPosition = zero + Vector3.left * 45f;
				DisLabel.transform.localPosition = targetTransform.localPosition + Vector3.up * 30f;
			}
			else if (zero.x <= 30f)
			{
				ArrowPic.transform.localEulerAngles = new Vector3(0f, 0f, 180f);
				targetPic.transform.localPosition = zero + Vector3.right * 45f;
				DisLabel.transform.localPosition = targetTransform.localPosition + Vector3.up * 30f;
			}
			else if (zero.y >= 410f)
			{
				ArrowPic.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
				targetPic.transform.localPosition = zero + Vector3.down * 45f;
				DisLabel.transform.localPosition = targetTransform.localPosition + Vector3.down * 30f;
			}
			else
			{
				ArrowPic.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
				targetPic.transform.localPosition = zero + Vector3.up * 45f;
				DisLabel.transform.localPosition = targetTransform.localPosition + Vector3.up * 30f;
			}
			sb.Length = 0;
			sb.AppendFormat("{0}m", (int)Vector3.Distance(sourcesTransform.position, tartgetPos));
			DisLabel.text = sb.ToString();
		}
		else if (UnityVersionUtil.IsActive(targetPic.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(targetPic.gameObject, state: false);
		}
	}

	public void Reset(GameObject targetObj, GameObject sourceObj)
	{
		mTargetObj = targetObj;
		tartgetPos = targetObj.transform.position;
		mSourceObj = sourceObj;
		if (mSourceObj != null)
		{
			sourcesTransform = sourceObj.transform;
		}
		Update();
	}

	private void OnEnable()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(onScreenResize));
	}

	private void OnDisable()
	{
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(onScreenResize));
	}

	private void onScreenResize()
	{
		screenWidth = Mathf.RoundToInt(480f * ((float)Screen.width / (float)Screen.height));
	}
}
