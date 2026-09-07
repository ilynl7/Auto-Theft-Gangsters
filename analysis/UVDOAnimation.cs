using UnityEngine;

public class UVDOAnimation : MonoBehaviour
{
	public enum Style
	{
		Once,
		Loop,
		PingPong
	}

	public Style mCurStyle;

	public AnimationCurve mAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

	public float MaxValue = 1f;

	public bool isYanimation;

	private Material CurMaterial;

	public bool isSharedMaterial;

	public float Speed = 1f;

	private float mFactor;

	public bool StartAnimation;

	private void Start()
	{
		mFactor = 0f;
		if (isSharedMaterial)
		{
			CurMaterial = base.gameObject.renderer.sharedMaterial;
		}
		else
		{
			CurMaterial = base.gameObject.renderer.material;
		}
		if (CurMaterial == null)
		{
			Debug.LogError("Material is NULL!");
		}
	}

	private void Update()
	{
		if (!(CurMaterial != null) || !StartAnimation)
		{
			return;
		}
		mFactor += Speed * Time.deltaTime;
		if (mCurStyle == Style.Loop)
		{
			if (Speed < 0f)
			{
				Speed = 0f - Speed;
			}
			if (mFactor > 1f)
			{
				mFactor -= Mathf.Floor(mFactor);
			}
		}
		else if (mCurStyle == Style.PingPong)
		{
			if (mFactor > 1f)
			{
				mFactor = 1f - (mFactor - Mathf.Floor(mFactor));
				Speed = 0f - Speed;
			}
			else if (mFactor < 0f)
			{
				mFactor = 0f - mFactor;
				Speed = 0f - Speed;
			}
		}
		else
		{
			if (Speed < 0f)
			{
				Speed = 0f - Speed;
			}
			if (mFactor >= 1f)
			{
				mFactor = 0f;
				StartAnimation = false;
			}
		}
		if (isYanimation)
		{
			CurMaterial.mainTextureOffset = new Vector2(0f, mAnimationCurve.Evaluate(mFactor) * MaxValue);
		}
		else
		{
			CurMaterial.mainTextureOffset = new Vector2(mAnimationCurve.Evaluate(mFactor) * MaxValue, 0f);
		}
	}

	private void OnDisable()
	{
		if (CurMaterial != null)
		{
			CurMaterial.mainTextureOffset = Vector2.zero;
		}
	}
}
