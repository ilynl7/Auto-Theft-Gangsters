using SprotoType;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
	public int TeleportID = -1;

	public int ActiveRadius = 3;

	private bool mbValid = true;

	private float mfLastInvaildTime;

	private Transform mMainPlayerTransform;

	private Transform mTeleportTransform;

	public GameDefine.SCENE_DEFINE nextSceneId = GameDefine.SCENE_DEFINE.SCENE_MAIN_CITY;

	private void Start()
	{
		mTeleportTransform = base.transform;
	}

	private void GoNextScene()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
			enter_new_map.request request = new enter_new_map.request();
			int num = (int)nextSceneId;
			request.mapInfoId = num.ToString();
			NetLogic.GetInstance().Send<Protocol.enter_new_map>(request);
			mfLastInvaildTime = Time.time;
			mbValid = false;
		}
	}

	private void FixedUpdate()
	{
		if (!mbValid)
		{
			if (Time.time - mfLastInvaildTime < 3f)
			{
				return;
			}
			mbValid = true;
		}
		if (null == mMainPlayerTransform)
		{
			if (null != Singleton<ObjManager>.Instance.MainPlayer)
			{
				mMainPlayerTransform = Singleton<ObjManager>.Instance.MainPlayer.transform;
			}
			if (null == mMainPlayerTransform)
			{
				return;
			}
		}
		if (null != Singleton<ObjManager>.Instance.MainPlayer && Vector3.Distance(mMainPlayerTransform.position, mTeleportTransform.position) <= (float)ActiveRadius)
		{
			GoNextScene();
		}
	}
}
