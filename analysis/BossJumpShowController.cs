using UnityEngine;

public class BossJumpShowController : MonoBehaviour
{
	public delegate void OnBossJumpOverDelegate();

	private OnBossJumpOverDelegate OnBossJumpOver;

	private Animation mAnimation;

	private Transform mTransform;

	private string mJumpUpAnimaName = "jump up";

	private string mJumpDownAnimaName = "jump down";

	private string mJumpLoopAnimaName = "jump loop";

	private string mModelName;

	private string mModelTypeName;

	private float mJumpUpLength;

	private float mJumpDownLength;

	private Vector3 mStartPos;

	private Vector3 mTargetPos;

	private float mDuration;

	private float mMoveDuration;

	private float mUseTime;

	private int mJumpStep;

	private float mJumpUpStartTime = 0.8f;

	private float mJumpDownEndTime = 0.8f;

	private float mHpos;

	private float mParamB;

	private float mStartAngle;

	private float mTargetAngle;

	private float gravity = 9.81f;

	private void Awake()
	{
		mAnimation = base.gameObject.GetComponentInChildren<Animation>();
		mTransform = base.transform;
	}

	private void Update()
	{
		mUseTime += Time.deltaTime;
		if (mUseTime >= mDuration)
		{
			base.enabled = false;
			if (OnBossJumpOver != null)
			{
				OnBossJumpOver();
			}
			Object.Destroy(this);
		}
		else
		{
			CheckPos();
			CheckAnima();
		}
	}

	public void Play(float hPos, Vector3 startPos, Vector3 targetPos, string modelName, string modelTypeName, OnBossJumpOverDelegate func = null)
	{
		OnBossJumpOver = func;
		mParamB = Mathf.Sqrt(hPos / gravity);
		mHpos = hPos;
		float num = targetPos.y - startPos.y;
		mMoveDuration = Mathf.Sqrt((hPos - num) / gravity) + mParamB;
		mDuration = mMoveDuration + mJumpUpStartTime + mJumpDownEndTime;
		mStartPos = startPos;
		mTargetPos = targetPos;
		mModelName = modelName;
		mModelTypeName = modelTypeName;
		mUseTime = 0f;
		mJumpStep = 0;
		mTransform.position = mStartPos;
		mStartAngle = MathUtil.Heading(mTransform.forward);
		mTargetAngle = MathUtil.Heading(new Vector3(mTargetPos.x - mStartPos.x, 0f, mTargetPos.z - mStartPos.z));
		if (mAnimation[mJumpUpAnimaName] == null)
		{
			LoadAnim(mJumpUpAnimaName);
		}
		if (mAnimation[mJumpLoopAnimaName] == null)
		{
			LoadAnim(mJumpLoopAnimaName);
		}
		if (mAnimation[mJumpDownAnimaName] == null)
		{
			LoadAnim(mJumpDownAnimaName);
		}
		mJumpUpLength = mAnimation[mJumpUpAnimaName].length;
		mJumpDownLength = mAnimation[mJumpDownAnimaName].length;
		mAnimation.Play(mJumpUpAnimaName);
	}

	private void CheckPos()
	{
		float num = mUseTime - mJumpUpStartTime;
		if (num < 0f)
		{
			mTransform.forward = MathUtil.HeadingToVector3(Mathf.LerpAngle(mStartAngle, mTargetAngle, Mathf.Clamp01(mUseTime / mJumpUpStartTime)));
			return;
		}
		if (num > mMoveDuration)
		{
			mTransform.position = mTargetPos;
			return;
		}
		float num2 = (0f - gravity) * (num - mParamB) * (num - mParamB) + mHpos;
		mTransform.position = VectorXZ.Lerp(mStartPos, mTargetPos, Mathf.Clamp01(num / mMoveDuration)) + Vector3.up * (num2 + mStartPos.y);
	}

	private void CheckAnima()
	{
		if (mJumpStep == 0)
		{
			if (mUseTime > mJumpUpLength - 0.2f)
			{
				mAnimation.CrossFade(mJumpLoopAnimaName);
				mJumpStep = 1;
			}
		}
		else if (mJumpStep == 1 && mUseTime > mDuration - mJumpDownLength + 0.3f)
		{
			mAnimation.CrossFade(mJumpDownAnimaName);
			mJumpStep = 2;
		}
	}

	public bool LoadAnim(string animaName)
	{
		AnimationClip animationClip = AnimationManager.LoadAnimation(mModelName, animaName) as AnimationClip;
		if (animationClip != null)
		{
			mAnimation.AddClip(animationClip, animaName);
			return true;
		}
		animationClip = AnimationManager.LoadAnimation(mModelTypeName, animaName) as AnimationClip;
		if (animationClip != null)
		{
			mAnimation.AddClip(animationClip, animaName);
			return true;
		}
		return false;
	}
}
