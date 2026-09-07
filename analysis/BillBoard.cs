using UnityEngine;

public class BillBoard : MonoBehaviour
{
	private GameObject mBindObj;

	private float mDeltaHeight = 2.25f;

	private Transform mCameraTransform;

	private Transform mTransform;

	private Vector3 mPosition;

	private Transform mBindObjTrans;

	public GameObject BindObj
	{
		get
		{
			return mBindObj;
		}
		set
		{
			mBindObj = value;
			if (mBindObj != null)
			{
				mBindObjTrans = mBindObj.transform;
			}
		}
	}

	public float DeltaHeight
	{
		get
		{
			return mDeltaHeight;
		}
		set
		{
			if (value != mDeltaHeight)
			{
				mDeltaHeight = value;
				mPosition = new Vector3(0f, mDeltaHeight, 0f);
			}
		}
	}

	private void Awake()
	{
		if (!(null == Camera.main))
		{
			mCameraTransform = Camera.main.transform;
			Vector3 worldUp = mCameraTransform.rotation * Vector3.up;
			base.transform.LookAt(base.transform.position + mCameraTransform.rotation * Vector3.back, worldUp);
		}
	}

	private void OnEnable()
	{
		base.transform.rotation = Quaternion.identity;
		if (!(null == Camera.main))
		{
			Vector3 worldUp = mCameraTransform.rotation * Vector3.up;
			base.transform.LookAt(base.transform.position + mCameraTransform.rotation * Vector3.back, worldUp);
			base.transform.position = new Vector3(0f, -20f, 0f);
		}
	}

	private void Start()
	{
		if (mBindObj != null)
		{
			mPosition = new Vector3(0f, mDeltaHeight, 0f);
			mBindObjTrans = mBindObj.transform;
		}
		mTransform = base.transform;
	}

	private void Update()
	{
		if (mCameraTransform == null)
		{
			if (Camera.main == null)
			{
				return;
			}
			mCameraTransform = Camera.main.transform;
			if (mCameraTransform == null)
			{
				return;
			}
		}
		if (null != mBindObj && null == mBindObjTrans)
		{
			mBindObjTrans = mBindObj.transform;
		}
		if ((bool)mBindObj && null != mTransform && null != mBindObjTrans)
		{
			mTransform.position = mBindObjTrans.position;
			mTransform.position += mPosition;
		}
		Vector3 worldUp = mCameraTransform.rotation * Vector3.up;
		base.transform.LookAt(base.transform.position + mCameraTransform.rotation * Vector3.back, worldUp);
		base.transform.Rotate(Vector3.up * 180f);
	}
}
