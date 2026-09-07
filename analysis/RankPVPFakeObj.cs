using UnityEngine;

public class RankPVPFakeObj : MonoBehaviour
{
	public Transform MeshRoot;

	public Camera objCam;

	public RenderTexture ModelPic;

	public int layer = 22;

	protected void Awake()
	{
		objCam.targetTexture = null;
		if (ModelPic == null)
		{
			ModelPic = new RenderTexture(375, 420, 16, RenderTextureFormat.ARGB32);
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
