using DG.Tweening;
using UnityEngine;

public class DragScreenHelpRoot : SingletonUnity<DragScreenHelpRoot>
{
	public GameObject HandObj;

	private GameObject mTargetObj;

	private Camera mMainCamera;

	private int screenWidth;

	private static Tweener mTutorialTweener;

	private SCREEN_DIRECTION mCurDirection;

	private void Start()
	{
		screenWidth = Mathf.RoundToInt(480f * ((float)Screen.width / (float)Screen.height));
	}

	public void SetTargetObj(GameObject target)
	{
		mTargetObj = target;
		NGUITools.SetActive(HandObj, state: false);
		mMainCamera = Camera.main;
	}

	private void Update()
	{
		if (!(mTargetObj == null))
		{
			Vector3 vector = mMainCamera.WorldToViewportPoint(mTargetObj.transform.position + Vector3.up * 0.5f);
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
			if (zero.x >= (float)(screenWidth - 30))
			{
				DragRight();
			}
			else if (zero.x <= 30f)
			{
				DragLeft();
			}
			else
			{
				DisableTip();
			}
		}
	}

	private void DragRight()
	{
		if (!UnityVersionUtil.IsActive(HandObj))
		{
			NGUITools.SetActive(HandObj, state: true);
		}
		if (mCurDirection != SCREEN_DIRECTION.RIGHT)
		{
			mCurDirection = SCREEN_DIRECTION.RIGHT;
			mTutorialTweener.Kill();
			HandObj.transform.localPosition = new Vector3(0f, 50f, 0f);
			mTutorialTweener = HandObj.transform.DOLocalMove(new Vector3(200f, 50f, 0f), 1f).SetLoops(int.MaxValue);
		}
	}

	private void DragLeft()
	{
		if (!UnityVersionUtil.IsActive(HandObj))
		{
			NGUITools.SetActive(HandObj, state: true);
		}
		if (mCurDirection != SCREEN_DIRECTION.LEFT)
		{
			mCurDirection = SCREEN_DIRECTION.LEFT;
			mTutorialTweener.Kill();
			HandObj.transform.localPosition = new Vector3(0f, 50f, 0f);
			mTutorialTweener = HandObj.transform.DOLocalMove(new Vector3(-200f, 50f, 0f), 1f).SetLoops(int.MaxValue);
		}
	}

	private void DisableTip()
	{
		if (UnityVersionUtil.IsActive(HandObj))
		{
			NGUITools.SetActive(HandObj, state: false);
		}
	}
}
