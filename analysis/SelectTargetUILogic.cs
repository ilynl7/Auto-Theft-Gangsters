using UnityEngine;

public class SelectTargetUILogic : SingletonUnity<SelectTargetUILogic>
{
	public UISprite targetPic;

	private int screenWidth;

	private Camera mMainCamera;

	private ObjMainPlayer mMainPlayer;

	private void Start()
	{
		screenWidth = Mathf.RoundToInt(480f * ((float)Screen.width / (float)Screen.height));
		mMainCamera = Camera.main;
	}

	private void Update()
	{
		if (mMainPlayer == null)
		{
			mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			if (mMainPlayer == null)
			{
				return;
			}
		}
		if (!UnityVersionUtil.IsActive(targetPic.gameObject))
		{
			if (mMainPlayer.SelectedTarget != null && !mMainPlayer.SelectedTarget.IsDie && UnityVersionUtil.IsActive(mMainPlayer.SelectedTarget.gameObject) && TargetDistance(mMainPlayer, mMainPlayer.SelectedTarget))
			{
				UnityVersionUtil.SetActiveRecursive(targetPic.gameObject, state: true);
			}
			if (mMainPlayer.SelectedTarget == null || mMainPlayer.SelectedTarget.IsDie || !UnityVersionUtil.IsActive(mMainPlayer.SelectedTarget.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(targetPic.gameObject, state: false);
				return;
			}
			Vector3 vector = mMainCamera.WorldToViewportPoint(mMainPlayer.SelectedTarget.Position + Vector3.up * 1.3f);
			if (vector.z < 0f)
			{
				UnityVersionUtil.SetActiveRecursive(targetPic.gameObject, state: false);
				return;
			}
			Vector3 localPosition = new Vector3(vector.x * (float)screenWidth, vector.y * 480f, 0f);
			targetPic.transform.localPosition = localPosition;
			if (targetPic.transform.localPosition.x < -20f || targetPic.transform.localPosition.x > (float)(screenWidth + 20) || targetPic.transform.localPosition.y < -10f || targetPic.transform.localPosition.y > 490f)
			{
				mMainPlayer.SelectTarget(null);
			}
			return;
		}
		if (mMainPlayer.SelectedTarget == null || mMainPlayer.SelectedTarget.IsDie || !UnityVersionUtil.IsActive(mMainPlayer.SelectedTarget.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(targetPic.gameObject, state: false);
			return;
		}
		Vector3 vector2 = mMainCamera.WorldToViewportPoint(mMainPlayer.SelectedTarget.Position + Vector3.up * 1.3f);
		if (vector2.z < 0f)
		{
			UnityVersionUtil.SetActiveRecursive(targetPic.gameObject, state: false);
			return;
		}
		Vector3 localPosition2 = new Vector3(vector2.x * (float)screenWidth, vector2.y * 480f, 0f);
		targetPic.transform.localPosition = localPosition2;
		if (targetPic.transform.localPosition.x < -20f || targetPic.transform.localPosition.x > (float)(screenWidth + 20) || targetPic.transform.localPosition.y < -10f || targetPic.transform.localPosition.y > 490f)
		{
			mMainPlayer.SelectTarget(null);
		}
	}

	private bool TargetDistance(ObjMainPlayer mainPlayer, ObjCharacter obj)
	{
		if (Vector3.SqrMagnitude(mainPlayer.Position - obj.Position) < 100f)
		{
			return true;
		}
		return false;
	}
}
