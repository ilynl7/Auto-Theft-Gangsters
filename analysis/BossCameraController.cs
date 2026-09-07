using UnityEngine;

public class BossCameraController : MonoBehaviour
{
	private ObjCharacter mObjCharacter;

	private bool isStart;

	private float distance;

	private Vector3 mLookOffset;

	private float mLookYaw;

	private float mCurrentYaw;

	private float durationTime;

	private float velocity;

	public void Init(ObjCharacter objCharacter)
	{
		mObjCharacter = objCharacter;
		isStart = false;
	}

	private void CaluParm()
	{
		mLookOffset = mObjCharacter.transform.position - base.transform.position;
		mLookYaw = MathUtil.WrapDegrees(Quaternion.LookRotation(mLookOffset).eulerAngles.y);
		mCurrentYaw = 0f;
		isStart = true;
	}

	private void UpdatePosRot()
	{
		Vector3 position = mObjCharacter.transform.position;
		mCurrentYaw += Time.deltaTime * 60f * 6f;
		Quaternion quaternion = Quaternion.Euler(0f, 0f - mCurrentYaw, 0f);
		Vector3 vector = position - quaternion * mLookOffset;
		Quaternion rotation = Quaternion.LookRotation(position - vector);
		base.transform.position = vector;
		base.transform.rotation = rotation;
		if (mCurrentYaw >= 270f)
		{
			Singleton<ObjManager>.Instance.MainPlayer.CameraController.LerpBackToPlayer(2f);
			Object.Destroy(this);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (isStart)
		{
			UpdatePosRot();
		}
	}

	public void CameraArrived()
	{
		CaluParm();
	}
}
