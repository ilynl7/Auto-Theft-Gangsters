using UnityEngine;

public class ObjPlayerMountCar : MonoBehaviour
{
	public MountCarMeshRoot MeshRoot;

	private float mAngleSpeed;

	private Transform mRootObj;

	private RideMountData mCurRideMountData;

	private BundleManager.LoadModelData mLoadingMeshData;

	private long mLoadingModelDataId;

	public bool ResetFlag;

	private float lastSpeed = -1f;

	private float preAngleY;

	private float steerAngle;

	private float wheelAngle;

	private float[] steerList = new float[6];

	private int curIndex;

	public RideMountData CurRideMountData
	{
		get
		{
			return mCurRideMountData;
		}
		set
		{
			mCurRideMountData = value;
		}
	}

	public BundleManager.LoadModelData LoadingMeshData
	{
		get
		{
			return mLoadingMeshData;
		}
		set
		{
			mLoadingMeshData = value;
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

	public void Reset(RideMountData data)
	{
		if (ResetFlag)
		{
			return;
		}
		mCurRideMountData = data;
		if (MeshRoot != null)
		{
			ResetFlag = true;
			ChangeColor(data.mColorData);
			if (mCurRideMountData.Player != null)
			{
				NGUITools.SetLayer(MeshRoot.gameObject, mCurRideMountData.Player.gameObject.layer);
				mCurRideMountData.Player.AnimationLogic.AnimaObj.transform.parent = MeshRoot.PlayerRoot;
				mCurRideMountData.Player.AnimationLogic.AnimaObj.transform.localPosition = Vector3.zero;
				mCurRideMountData.Player.AnimationLogic.AnimaObj.transform.localRotation = Quaternion.identity;
				mRootObj = mCurRideMountData.Player.CacheTransform;
				mAngleSpeed = mCurRideMountData.Player.NavMeshAgent.speed * 57.29578f / MeshRoot.WheelRadius / 2f;
				mCurRideMountData.Player.CurAnimationState = GameDefine.ANIMATIONSTATE.DRIVING;
				mCurRideMountData.Player.CurPlayerState = PLAYER_STATE.DRIVING;
				mCurRideMountData.Player.LoadingMountFlag = false;
			}
			if (mCurRideMountData.PlayerCar != null)
			{
				OnPlayerCarLoadDone();
			}
		}
	}

	public void UpdateSpeed()
	{
		if (mCurRideMountData != null && mCurRideMountData.Player != null && MeshRoot != null && Mathf.Abs(lastSpeed - mCurRideMountData.Player.NavMeshAgent.speed) > float.Epsilon)
		{
			mAngleSpeed = mCurRideMountData.Player.NavMeshAgent.speed * 57.29578f / MeshRoot.WheelRadius / 2f;
			lastSpeed = mCurRideMountData.Player.NavMeshAgent.speed;
		}
	}

	public void UpdateSetSpeed(float speed)
	{
		if (mCurRideMountData != null && mCurRideMountData.Player != null && MeshRoot != null && Mathf.Abs(lastSpeed - speed) > float.Epsilon)
		{
			mAngleSpeed = speed * 57.29578f / MeshRoot.WheelRadius / 2f;
			lastSpeed = speed;
		}
	}

	private void OnPlayerCarLoadDone()
	{
		mCurRideMountData.PlayerCar.OnMeshLoadDone(MeshRoot);
		mCurRideMountData.Player.CacheTransform.parent = MeshRoot.PlayerRoot;
		mCurRideMountData.Player.CacheTransform.localPosition = Vector3.zero;
		mCurRideMountData.Player.CacheTransform.localRotation = Quaternion.identity;
	}

	public void OnMeshLoadDone(MountCarMeshRoot meshRoot, RideMountData data)
	{
		MeshRoot = meshRoot;
		MeshRoot.transform.parent = base.transform;
		MeshRoot.transform.localPosition = Vector3.zero;
		MeshRoot.transform.localRotation = Quaternion.identity;
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(meshRoot.gameObject, state: true);
			Reset(data);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(meshRoot.gameObject, state: false);
		}
	}

	public void ChangeColor(ColorData data)
	{
		if (data != null && MeshRoot != null)
		{
			MeshRoot.ChangeColor(data);
		}
	}

	private void Update()
	{
		if (MeshRoot != null)
		{
			if (mCurRideMountData != null && mCurRideMountData.Player != null && mCurRideMountData.Player.IsMoving)
			{
				steerAngle = Mathf.Lerp(steerAngle, Mathf.Clamp((0f - Mathf.DeltaAngle(mRootObj.eulerAngles.y, preAngleY)) / Time.deltaTime / 4f, -50f, 50f), 0.8f);
				steerList[curIndex] = steerAngle;
				steerAngle = GetAVE();
				curIndex = (curIndex + 1) % steerList.Length;
				wheelAngle += mAngleSpeed * Time.deltaTime;
				MeshRoot.QLWheel.localRotation = Quaternion.Euler(new Vector3(wheelAngle, steerAngle, 0f));
				MeshRoot.QRWheel.localRotation = Quaternion.Euler(new Vector3(0f - wheelAngle, steerAngle + 180f, 0f));
				MeshRoot.HLWheel.localRotation = Quaternion.Euler(new Vector3(wheelAngle, 0f, 0f));
				MeshRoot.HRWheel.localRotation = Quaternion.Euler(new Vector3(0f - wheelAngle, 180f, 0f));
				preAngleY = mRootObj.eulerAngles.y;
			}
			else
			{
				steerList[curIndex] = 0f;
				steerAngle = GetAVE();
				curIndex = (curIndex + 1) % steerList.Length;
				MeshRoot.QLWheel.localRotation = Quaternion.Euler(new Vector3(wheelAngle, steerAngle, 0f));
				MeshRoot.QRWheel.localRotation = Quaternion.Euler(new Vector3(wheelAngle, steerAngle + 180f, 0f));
			}
		}
	}

	private float GetAVE()
	{
		float num = 0f;
		for (int i = 0; i < steerList.Length; i++)
		{
			num += steerList[i];
		}
		return num / (float)steerList.Length;
	}
}
