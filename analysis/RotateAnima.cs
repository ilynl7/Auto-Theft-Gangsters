using UnityEngine;

public class RotateAnima : MonoBehaviour
{
	private Transform mTra;

	private float RotateSpeed = 10f;

	private bool isPlay = true;

	private void Awake()
	{
		mTra = base.transform;
		isPlay = false;
	}

	private void Update()
	{
		if (isPlay)
		{
			mTra.localEulerAngles -= new Vector3(0f, RotateSpeed * Time.deltaTime, 0f);
		}
	}

	public void PlayAnima()
	{
		isPlay = true;
	}

	public void StopAnima()
	{
		isPlay = false;
	}
}
