using UnityEngine;

public class SurveyItemObj : MonoBehaviour
{
	public SurveyMissionData SurveyMissionData;

	private BundleManager.LoadModelData mLoadingModelData;

	private long mLoadingModelDataId;

	private GameObject mMeshRoot;

	private GameObject mEffectRoot;

	private int mRestNum;

	private Transform mMainPlayerTransform;

	private float sqrDis;

	private float SqrActiveRadius = 3f;

	private float ExitSqrActiveRadius = 3.5f;

	private Transform mTransform;

	private bool mInCircleFlag;

	public BundleManager.LoadModelData LoadingModelData
	{
		get
		{
			return mLoadingModelData;
		}
		set
		{
			mLoadingModelData = value;
		}
	}

	public long LoadingModelDataId
	{
		get
		{
			return mLoadingModelDataId;
		}
		set
		{
			mLoadingModelDataId = value;
		}
	}

	public GameObject MeshRoot
	{
		get
		{
			return mMeshRoot;
		}
		set
		{
			mMeshRoot = value;
		}
	}

	public GameObject EffectRoot
	{
		get
		{
			return mEffectRoot;
		}
		set
		{
			mEffectRoot = value;
		}
	}

	public bool IsEnable()
	{
		if (mRestNum > 0)
		{
			return true;
		}
		return false;
	}

	public void Reset(SurveyMissionData surveyMissionData)
	{
		SurveyMissionData = surveyMissionData;
		if (surveyMissionData.SceneID.Equals("11"))
		{
			SqrActiveRadius = 8f;
			ExitSqrActiveRadius = 10f;
		}
		else
		{
			SqrActiveRadius = 3f;
			ExitSqrActiveRadius = 3.5f;
		}
		base.transform.position = new Vector3(surveyMissionData.PosX, SceneManager.GetHitHeight(surveyMissionData.PosX, surveyMissionData.PosZ), surveyMissionData.PosZ);
		base.transform.forward = MathUtil.HeadingToVector3(surveyMissionData.PosO);
		mRestNum = SurveyMissionData.Count;
		mTransform = base.transform;
	}

	public void CollectOne()
	{
		mRestNum--;
	}

	public void Refresh()
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
		mRestNum = SurveyMissionData.Count;
	}

	public void PlayEffect()
	{
		UnityVersionUtil.SetActiveRecursive(EffectRoot, state: false);
		Animation animation = MeshRoot.GetComponent<Animation>();
		if (animation == null)
		{
			animation = MeshRoot.GetComponentInChildren<Animation>();
		}
		if (animation != null)
		{
			animation.Play();
		}
		ParticleSystem[] componentsInChildren = MeshRoot.GetComponentsInChildren<ParticleSystem>();
		if (componentsInChildren != null && componentsInChildren.Length > 0)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].Play();
			}
		}
	}

	private void Update()
	{
		if (mRestNum <= 0)
		{
			if (mInCircleFlag)
			{
				if (SingletonUnity<SurveyItemBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SurveyItemBtnRootLogic>.Instance.gameObject))
				{
					SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SurveyItemBtnRoot);
				}
				mInCircleFlag = false;
			}
			return;
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
		sqrDis = (mMainPlayerTransform.position - mTransform.position).sqrMagnitude;
		if (sqrDis <= SqrActiveRadius)
		{
			if (!mInCircleFlag)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SurveyItemBtnRoot, delegate
				{
					SingletonUnity<SurveyItemBtnRootLogic>.Instance.Reset(this);
				});
				mInCircleFlag = true;
			}
		}
		else if (sqrDis > ExitSqrActiveRadius && mInCircleFlag)
		{
			if (SingletonUnity<SurveyItemBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SurveyItemBtnRootLogic>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SurveyItemBtnRoot);
			}
			mInCircleFlag = false;
		}
	}

	private void OnDisable()
	{
		if (mInCircleFlag && SingletonUnity<SurveyItemBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SurveyItemBtnRootLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SurveyItemBtnRoot);
		}
	}
}
