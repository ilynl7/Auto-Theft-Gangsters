using UnityEngine;

public class TestViewChange : MonoBehaviour
{
	private bool open;

	public void Click()
	{
		if (open)
		{
			Singleton<ObjManager>.Instance.MainPlayer.CameraController.ChangeCameraView(CameraController.CAMERAVIEWSTATE.FREE);
		}
		else
		{
			Singleton<ObjManager>.Instance.MainPlayer.CameraController.ChangeCameraView(CameraController.CAMERAVIEWSTATE.FIXED);
		}
		open = !open;
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
