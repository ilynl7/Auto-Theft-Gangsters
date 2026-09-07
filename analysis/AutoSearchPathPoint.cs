using UnityEngine;

public class AutoSearchPathPoint
{
	private string mSceneId;

	private float mPosX;

	private float mPosY;

	private float mPosZ;

	public string SceneId
	{
		get
		{
			return mSceneId;
		}
		set
		{
			mSceneId = value;
		}
	}

	public float PosX
	{
		get
		{
			return mPosX;
		}
		set
		{
			mPosX = value;
		}
	}

	public float PosY
	{
		get
		{
			return mPosY;
		}
		set
		{
			mPosY = value;
		}
	}

	public float PosZ
	{
		get
		{
			return mPosZ;
		}
		set
		{
			mPosZ = value;
		}
	}

	public AutoSearchPathPoint(string sceneId, float posX, float posY, float posZ)
	{
		mSceneId = sceneId;
		mPosX = posX;
		mPosY = posY;
		mPosZ = posZ;
	}

	public AutoSearchPathPoint(string sceneId, Vector3 pos)
	{
		mSceneId = sceneId;
		mPosX = pos.x;
		mPosY = pos.y;
		mPosZ = pos.z;
	}

	public static AutoSearchPathPoint CreatePoint(GameObject obj)
	{
		if (obj == null)
		{
			return null;
		}
		return new AutoSearchPathPoint(SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr, obj.transform.position.x, obj.transform.position.y, obj.transform.position.z);
	}

	public bool Equal(AutoSearchPathPoint point)
	{
		if (mSceneId.Equals(point.SceneId) && Mathf.Abs(mPosX - point.PosX) < 0.01f && Mathf.Abs(mPosZ - point.PosZ) < 0.01f)
		{
			return true;
		}
		return false;
	}

	public void Reset()
	{
		mSceneId = "-1";
		mPosX = 0f;
		mPosZ = 0f;
	}
}
