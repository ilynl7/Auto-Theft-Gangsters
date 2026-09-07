using System.Collections.Generic;
using UnityEngine;

public class ObjCar : ObjCharacter
{
	public float MaxSpeed = 40f;

	public float MaxSteerAngle = 10f;

	public float MaxAcceleration = 10f;

	public float BrakeAcceleration = 40f;

	protected CarControl mCarControl;

	public GameObject MeshRoot;

	public Transform CarBodyRoot;

	private string mModelId;

	public WheelSuspension FLWheel;

	public WheelSuspension FRWheel;

	public WheelSuspension BLWheel;

	public WheelSuspension BRWheel;

	protected Dictionary<GameObject, ParticleSystem> mStrikeDic = new Dictionary<GameObject, ParticleSystem>();

	public ObjCharacter InsidePlayer;

	protected CarPath mPath;

	protected int mCurPathIndex;

	private SceneManager mCurSceneManager;

	public override float ModelRadius => 3f;

	public override float ModelHeight => 1.5f;

	public CarControl CarControl => mCarControl;

	public new string ModelId
	{
		get
		{
			return mModelId;
		}
		set
		{
			mModelId = value;
		}
	}

	public float CurSpeed => mCarControl.CurSpeed;

	private SceneManager CurSceneManager
	{
		get
		{
			if (mCurSceneManager == null)
			{
				mCurSceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			}
			return mCurSceneManager;
		}
	}

	protected new void Init()
	{
		if (mCarControl == null)
		{
			mCarControl = base.gameObject.GetComponent<CarControl>();
			if (mCarControl == null)
			{
				mCarControl = base.gameObject.AddComponent<CarControl>();
			}
		}
		FLWheel = base.transform.FindChild("MeshRoot/FLWheel").gameObject.GetComponent<WheelSuspension>();
		FRWheel = base.transform.FindChild("MeshRoot/FRWheel").gameObject.GetComponent<WheelSuspension>();
		BLWheel = base.transform.FindChild("MeshRoot/BLWheel").gameObject.GetComponent<WheelSuspension>();
		BRWheel = base.transform.FindChild("MeshRoot/BRWheel").gameObject.GetComponent<WheelSuspension>();
		CarBodyRoot = base.transform.FindChild("MeshRoot/cheshen");
	}

	public virtual void DisableCar()
	{
		if (!base.rigidbody.isKinematic)
		{
			base.rigidbody.velocity = Vector3.zero;
			base.rigidbody.angularVelocity = Vector3.zero;
		}
		base.rigidbody.useGravity = false;
		base.rigidbody.isKinematic = true;
		base.rigidbody.drag = 0.6f;
		base.rigidbody.angularDrag = 0.6f;
		mCarControl.enabled = false;
		FLWheel.enabled = false;
		FRWheel.enabled = false;
		BLWheel.enabled = false;
		BRWheel.enabled = false;
		InsidePlayer = null;
	}

	public virtual void EnableCar(ObjCharacter insidePlayer)
	{
		base.rigidbody.isKinematic = false;
		base.rigidbody.useGravity = true;
		mCarControl.enabled = true;
		base.rigidbody.drag = 0f;
		base.rigidbody.angularDrag = 0f;
		FLWheel.enabled = true;
		FRWheel.enabled = true;
		BLWheel.enabled = true;
		BRWheel.enabled = true;
		InsidePlayer = insidePlayer;
	}

	protected new void Reset()
	{
	}

	public new void FaceToPub(Vector3 pos)
	{
		Vector3 vector = pos - base.Position;
		vector.y = 0f;
		if (vector != Vector3.zero)
		{
			base.CacheTransform.rotation = Quaternion.LookRotation(vector);
		}
	}

	private void SetCarPath(CarPath path)
	{
		mPath = path;
		mCurPathIndex = 0;
	}

	public CarPathPoint GetCurPathPoint()
	{
		if (mCurPathIndex < mPath.PathPointList.Count - 1)
		{
			for (int i = mCurPathIndex; i < mPath.PathPointList.Count; i++)
			{
				if (mPath.PathPointList[i].transform.InverseTransformPoint(base.Position).z * mPath.PathPointList[i + 1].transform.InverseTransformPoint(base.Position).z < 0f)
				{
					mCurPathIndex = i;
					return mPath.PathPointList[i];
				}
			}
			return null;
		}
		return mPath.PathPointList[mCurPathIndex];
	}

	public int GetCurPathIndex()
	{
		if (mCurPathIndex < mPath.PathPointList.Count - 1)
		{
			for (int i = Mathf.Max(mCurPathIndex - 1, 0); i < mPath.PathPointList.Count; i++)
			{
				if (mPath.PathPointList[i].transform.InverseTransformPoint(base.Position).z * mPath.PathPointList[i + 1].transform.InverseTransformPoint(base.Position).z < 0f)
				{
					mCurPathIndex = i + 1;
					return mCurPathIndex;
				}
			}
			return mCurPathIndex;
		}
		return mCurPathIndex;
	}

	protected void FixedUpdate()
	{
		mCarControl.OnTheGroundFlag = FLWheel.OnGround && FRWheel.OnGround && BLWheel.OnGround && BRWheel.OnGround;
	}

	protected void Update()
	{
		if (mCarControl.CurSpeed < 10f && mStrikeDic.Count > 0)
		{
			List<GameObject> list = new List<GameObject>(mStrikeDic.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				DisableStrike(list[i]);
			}
		}
	}

	protected void EnableStrike(GameObject obj, Vector3 pos)
	{
		if (!GameSettingData.IsCarCopyEffectEnable[GameSettingData.GetPhoneClass()])
		{
			return;
		}
		GameObject root = NGUITools.GetRoot(obj);
		if (!mStrikeDic.ContainsKey(root))
		{
			ParticleSystem strike = CurSceneManager.GetStrike();
			if (strike != null)
			{
				UnityVersionUtil.SetActiveRecursive(strike.gameObject, state: true);
				strike.transform.parent = MeshRoot.transform;
				strike.transform.position = pos;
				mStrikeDic.Add(root, strike);
				strike.Play();
			}
			else
			{
				Debug.Log("Strike Out Of Pool Count!!!!!!!!!!!!!!!!!");
			}
		}
	}

	protected void DisableStrike(GameObject obj)
	{
		GameObject root = NGUITools.GetRoot(obj);
		if (mStrikeDic.ContainsKey(root))
		{
			ParticleSystem particleSystem = mStrikeDic[root];
			UnityVersionUtil.SetActiveRecursive(particleSystem.gameObject, state: false);
			mStrikeDic.Remove(root);
			particleSystem.transform.parent = null;
			CurSceneManager.RecycleStrike(particleSystem);
		}
	}

	protected void ClearStrike()
	{
		List<ParticleSystem> list = new List<ParticleSystem>(mStrikeDic.Values);
		for (int i = 0; i < list.Count; i++)
		{
			UnityVersionUtil.SetActiveRecursive(list[i].gameObject, state: false);
			list[i].transform.parent = null;
			CurSceneManager.RecycleStrike(list[i]);
		}
		mStrikeDic.Clear();
	}
}
