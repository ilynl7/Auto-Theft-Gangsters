using System.Text;
using UnityEngine;

public class DamageBoard : MonoBehaviour
{
	private UILabel mDamageBoardLabel;

	private UITweener[] TweenArray;

	private float mShowTime;

	private Transform mDamageBoadTransform;

	private int mType;

	private Transform mCameraTransform;

	private StringBuilder sb = new StringBuilder(512);

	private GameObject parent;

	public float ShowTime
	{
		get
		{
			return mShowTime;
		}
		set
		{
			mShowTime = value;
		}
	}

	private void Init()
	{
		if (mDamageBoadTransform == null)
		{
			mDamageBoadTransform = base.transform;
		}
		if (mDamageBoardLabel == null)
		{
			mDamageBoardLabel = base.gameObject.GetComponent<UILabel>();
			if (mDamageBoardLabel == null)
			{
				mDamageBoardLabel = base.gameObject.AddComponent<UILabel>();
			}
		}
		if (mCameraTransform == null)
		{
			mCameraTransform = Camera.mainCamera.transform;
		}
	}

	public void Reuse()
	{
	}

	public void ShowDamageBoard_01(int nType, string strValue, Vector3 pos)
	{
		DamageBoardTypeData damageBoardTypeDataById = DataManager.GetDamageBoardTypeDataById(nType);
		if (damageBoardTypeDataById == null)
		{
			damageBoardTypeDataById = DataManager.GetDamageBoardTypeDataById(1);
		}
		mType = nType;
	}

	public void ShowDamgeBoard(int nType, string strValue, Vector3 pos)
	{
		DamageBoardTypeData damageBoardTypeDataById = DataManager.GetDamageBoardTypeDataById(nType);
		if (damageBoardTypeDataById == null)
		{
			damageBoardTypeDataById = DataManager.GetDamageBoardTypeDataById(1);
		}
		Init();
		if (TweenArray == null)
		{
			TweenArray = base.gameObject.GetComponents<UITweener>();
		}
		pos.y += 2f;
		if (parent == null)
		{
			parent = new GameObject();
			parent.transform.parent = mDamageBoadTransform.parent;
		}
		parent.transform.localScale = Vector3.one;
		parent.transform.localEulerAngles = Vector3.zero;
		parent.transform.localPosition = Vector3.zero;
		mDamageBoadTransform.parent = parent.transform;
		mDamageBoadTransform.localPosition = Vector3.zero;
		mDamageBoadTransform.localScale = Vector3.one;
		mDamageBoadTransform.localEulerAngles = Vector3.zero;
		parent.transform.position = pos;
		for (int i = 0; i < TweenArray.Length; i++)
		{
			TweenArray[i].ResetToBeginning();
		}
		for (int j = 0; j < TweenArray.Length; j++)
		{
			TweenArray[j].enabled = true;
			TweenArray[j].Play();
		}
		parent.transform.localRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
		Vector3 worldUp = mCameraTransform.rotation * Vector3.up;
		parent.transform.LookAt(mDamageBoadTransform.position + mCameraTransform.rotation * Vector3.back, worldUp);
		parent.transform.Rotate(Vector3.up * 180f);
		sb.Length = 0;
		sb.AppendFormat("{0}", strValue);
		mDamageBoardLabel.text = sb.ToString();
		mDamageBoardLabel.alpha = 1f;
		mShowTime = Time.time + damageBoardTypeDataById.ShowTime;
	}

	private void Update()
	{
		if (parent != null && mDamageBoardLabel.alpha > 0.1f)
		{
			parent.transform.localRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
			Vector3 worldUp = mCameraTransform.rotation * Vector3.up;
			parent.transform.LookAt(mDamageBoadTransform.position + mCameraTransform.rotation * Vector3.back, worldUp);
			parent.transform.Rotate(Vector3.up * 180f);
			float num = Vector3.Distance(base.transform.position, mCameraTransform.position);
			parent.transform.localScale = Vector3.Lerp(new Vector3(0.2f, 0.2f, 0.2f), Vector3.one, num / 10f);
		}
	}

	private void Awake()
	{
		Init();
	}
}
