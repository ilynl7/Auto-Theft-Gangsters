using UnityEngine;

public class FxControl : MonoBehaviour
{
	private long mOwnerId = -1L;

	public GameDefine.OBJ_TYPE mOwnerType;

	private float mGenerateTime;

	private ParticleSystem[] arraySystem;

	private string mEffectName;

	private string mEffectId;

	private float mDuration;

	private Transform mCacheTransform;

	private EffectLogic mCurEffectLogicHandle;

	private AutoMoveFx mAutoMoveFx;

	public long OwnerId => mOwnerId;

	public GameDefine.OBJ_TYPE OwnerType => mOwnerType;

	public float GenerateTime => mGenerateTime;

	public string EffectName => mEffectName;

	public string EffectId => mEffectId;

	public float Duration => mDuration;

	public Transform CacheTransform
	{
		get
		{
			if (mCacheTransform == null)
			{
				mCacheTransform = base.transform;
			}
			return mCacheTransform;
		}
	}

	private void OnEnable()
	{
		for (int i = 0; i < arraySystem.Length; i++)
		{
			arraySystem[i].Clear();
		}
	}

	private void OnDisable()
	{
		for (int i = 0; i < arraySystem.Length; i++)
		{
			if (arraySystem[i] != null)
			{
				arraySystem[i].Clear();
			}
		}
	}

	private void PlayParticleSystem()
	{
		for (int i = 0; i < arraySystem.Length; i++)
		{
			arraySystem[i].Play();
		}
	}

	public virtual void Play(Transform target)
	{
		Play();
		if (target != null)
		{
			if (mAutoMoveFx == null)
			{
				mAutoMoveFx = base.gameObject.AddComponent<AutoMoveFx>();
			}
			mAutoMoveFx.Reset(mDuration, target);
		}
	}

	public virtual void Play(Vector3 target)
	{
		Play();
		if (mAutoMoveFx == null)
		{
			mAutoMoveFx = base.gameObject.AddComponent<AutoMoveFx>();
		}
		mAutoMoveFx.Reset(mDuration, target);
	}

	public virtual void Play()
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		PlayParticleSystem();
	}

	public virtual void Reset(EffectLogic effectLogic, FxEffInfoData fxEffInfoData, float duration, long ownerId, GameDefine.OBJ_TYPE ownerType)
	{
		mDuration = duration;
		mEffectName = $"{fxEffInfoData.EffName}";
		mEffectId = fxEffInfoData.ID;
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
		mGenerateTime = Time.time;
		mOwnerId = ownerId;
		mOwnerType = ownerType;
	}

	private void Awake()
	{
		arraySystem = GetComponentsInChildren<ParticleSystem>();
	}

	private void Update()
	{
		mDuration -= Time.deltaTime;
		if (mDuration <= 0f)
		{
			mDuration = 2f;
			EffectLogic.RecyleEffect(this);
		}
	}
}
