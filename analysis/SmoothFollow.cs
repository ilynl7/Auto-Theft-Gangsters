using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
	public float distance = 10f;

	private float height = 5f;

	private float heightDamping = 2f;

	private float rotationDamping = 3f;

	private Transform targetTransform;

	private Transform mainCameraTransform;

	public void UpdateFollow()
	{
		if (targetTransform == null)
		{
			if (Singleton<ObjManager>.Instance.MainPlayer == null)
			{
				return;
			}
			targetTransform = Singleton<ObjManager>.Instance.MainPlayer.transform;
		}
		if (mainCameraTransform == null)
		{
			mainCameraTransform = Camera.mainCamera.transform;
		}
		float y = targetTransform.eulerAngles.y;
		float to = targetTransform.position.y + height;
		float y2 = mainCameraTransform.eulerAngles.y;
		float y3 = mainCameraTransform.position.y;
		y2 = Mathf.LerpAngle(y2, y, rotationDamping * Time.deltaTime);
		y3 = Mathf.Lerp(y3, to, heightDamping * Time.deltaTime);
		Quaternion quaternion = Quaternion.Euler(0f, y2, 0f);
		mainCameraTransform.position = targetTransform.position;
		mainCameraTransform.position -= quaternion * Vector3.forward * distance;
		Vector3 position = mainCameraTransform.position;
		position.y = y3;
		mainCameraTransform.position = position;
		mainCameraTransform.LookAt(targetTransform);
	}
}
