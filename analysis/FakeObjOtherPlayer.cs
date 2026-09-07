using UnityEngine;

public class FakeObjOtherPlayer : SingletonUnity<FakeObjOtherPlayer>
{
	public Transform MeshRoot;

	public Camera objCam;

	public RenderTexture ModelPic;

	protected override void Awake()
	{
		base.Awake();
		if (ModelPic == null)
		{
			ModelPic = new RenderTexture(332, 512, 16, RenderTextureFormat.ARGB32);
		}
		ModelPic.useMipMap = false;
		objCam.targetTexture = ModelPic;
		objCam.ResetAspect();
	}

	public void EnableFakeObjRoot()
	{
		objCam.cullingMask = 2097152;
		base.gameObject.layer = 21;
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
	}

	public void DisableFakeObjRoot()
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
	}

	protected override void OnDestroy()
	{
		if (ModelPic.IsCreated())
		{
			ModelPic.Release();
		}
		base.OnDestroy();
	}

	public void resetTexture(int width, int height, int depth)
	{
		if (ModelPic.IsCreated())
		{
			ModelPic.width = width;
			ModelPic.height = height;
			ModelPic.depth = depth;
		}
	}
}
