using UnityEngine;

public class TeamFakeObjPicRootLogic : MonoBehaviour
{
	public Transform MeshRoot;

	public Camera objCam;

	public RenderTexture ModelPic;

	public int type;

	public int layer = 22;

	protected void Awake()
	{
		objCam.targetTexture = null;
		if (ModelPic == null)
		{
			if (type == 0)
			{
				ModelPic = new RenderTexture(180, 250, 16, RenderTextureFormat.ARGB32);
			}
			else if (type == 1)
			{
				ModelPic = new RenderTexture(375, 474, 16, RenderTextureFormat.ARGB32);
			}
			else if (type == 2)
			{
				ModelPic = new RenderTexture(400, 520, 16, RenderTextureFormat.ARGB32);
			}
			ModelPic.isPowerOfTwo = false;
			ModelPic.useMipMap = false;
		}
		objCam.targetTexture = ModelPic;
		objCam.ResetAspect();
	}

	public void CreateModelPic()
	{
		objCam.targetTexture = null;
		if (ModelPic == null)
		{
			if (type == 0)
			{
				ModelPic = new RenderTexture(180, 250, 16, RenderTextureFormat.ARGB32);
			}
			else if (type == 1)
			{
				ModelPic = new RenderTexture(375, 474, 16, RenderTextureFormat.ARGB32);
			}
			else if (type == 2)
			{
				ModelPic = new RenderTexture(400, 520, 16, RenderTextureFormat.ARGB32);
			}
			ModelPic.isPowerOfTwo = false;
			ModelPic.useMipMap = false;
		}
		objCam.targetTexture = ModelPic;
		objCam.ResetAspect();
	}

	public void EnableFakeObjRoot()
	{
		objCam.cullingMask = 1 << layer;
		base.gameObject.layer = layer;
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
	}

	public void DisableFakeObjRoot()
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
	}

	protected void OnDestroy()
	{
		if (ModelPic.IsCreated())
		{
			ModelPic.Release();
		}
	}

	public void resetTexture(int width, int height, int depth)
	{
		if (ModelPic.IsCreated())
		{
			ModelPic.width = width;
			ModelPic.height = height;
			ModelPic.depth = depth;
			objCam.targetTexture = ModelPic;
			objCam.ResetAspect();
		}
	}
}
