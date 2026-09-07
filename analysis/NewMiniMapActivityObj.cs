using UnityEngine;

public class NewMiniMapActivityObj : MonoBehaviour
{
	public UISprite IconPic;

	public UISprite MissionStatePic;

	private Transform parentTrans;

	private Transform cacheTrans;

	public void Reset(string IconName, string missionStatePicName, bool isMission)
	{
		cacheTrans = base.transform;
		parentTrans = cacheTrans.parent;
		IconPic.spriteName = IconName;
		IconPic.MakePixelPerfect();
		IconPic.width = (int)((float)IconPic.width * 0.6f);
		IconPic.height = (int)((float)IconPic.height * 0.6f);
		if (string.IsNullOrEmpty(missionStatePicName))
		{
			MissionStatePic.enabled = false;
		}
		else
		{
			MissionStatePic.enabled = true;
			MissionStatePic.spriteName = missionStatePicName;
			MissionStatePic.MakePixelPerfect();
			MissionStatePic.width /= 3;
			MissionStatePic.height /= 3;
		}
		if (isMission)
		{
			MissionStatePic.pivot = UIWidget.Pivot.Left;
			MissionStatePic.transform.localPosition = new Vector3(6f, 0f, 0f);
			MissionStatePic.color = Color.white;
		}
		else
		{
			MissionStatePic.pivot = UIWidget.Pivot.Center;
			MissionStatePic.transform.localPosition = Vector3.zero;
			MissionStatePic.color = Color.red;
		}
	}

	private void Update()
	{
		if (parentTrans != null)
		{
			cacheTrans.localEulerAngles = new Vector3(0f, 0f, 0f - parentTrans.localEulerAngles.z);
		}
	}
}
