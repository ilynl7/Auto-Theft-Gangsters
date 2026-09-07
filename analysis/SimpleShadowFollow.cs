using UnityEngine;

public class SimpleShadowFollow : MonoBehaviour
{
	private GameObject mBindObj;

	private float mDeltaHeight;

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

	private void OnEnable()
	{
		base.transform.rotation = Quaternion.identity;
		base.transform.position = new Vector3(0f, -20f, 0f);
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
		if (null != mBindObj && null == mBindObjTrans)
		{
			mBindObjTrans = mBindObj.transform;
		}
		if ((bool)mBindObj && null != mTransform && null != mBindObjTrans)
		{
			mTransform.position = mBindObjTrans.position;
			mTransform.position += mPosition;
			mTransform.rotation = mBindObjTrans.rotation;
		}
	}
}
