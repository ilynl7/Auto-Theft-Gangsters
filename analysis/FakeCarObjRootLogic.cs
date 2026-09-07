using UnityEngine;

public class FakeCarObjRootLogic : SingletonUnity<FakeCarObjRootLogic>
{
	public Transform MeshRoot;

	public GameObject mTopLeftObj;

	public GameObject mBottomRightObj;

	public Camera objCam;

	public RenderTexture ModelPic;

	private ObjPlayerMountCar CurCarMesh;

	private MountData curMountdata;

	private ColorData curColordata;

	public RotateAnima rotateAnima;

	protected override void Awake()
	{
		base.Awake();
		if (ModelPic == null)
		{
			ModelPic = new RenderTexture(400, 400, 16, RenderTextureFormat.ARGB32);
		}
		ModelPic.useMipMap = false;
		objCam.targetTexture = ModelPic;
		objCam.ResetAspect();
		objCam.cullingMask = 4194304;
		base.gameObject.layer = 22;
	}

	public void InitFakeCarObj(MountData mountData, ColorData colordata)
	{
		if (CurCarMesh != null && (curMountdata == null || !curMountdata.ID.Equals(mountData.ID) || curColordata == null || !curColordata.ID.Equals(colordata.ID)))
		{
			UnityVersionUtil.SetActiveRecursive(CurCarMesh.gameObject, state: false);
			CurCarMesh.transform.parent = null;
			Singleton<ObjManager>.Instance.RecycleMountCar(CurCarMesh);
			CurCarMesh = null;
		}
		if (CurCarMesh == null)
		{
			RideMountData rideMountData = new RideMountData();
			rideMountData.Player = null;
			rideMountData.MountData = mountData;
			rideMountData.mColorData = colordata;
			CurCarMesh = Singleton<ObjManager>.Instance.GetMountCar(rideMountData);
		}
		UnityVersionUtil.SetActiveRecursive(CurCarMesh.gameObject, state: true);
		NGUITools.SetLayer(CurCarMesh.gameObject, 22);
		MeshRoot.transform.localPosition = mountData.ModelPos;
		MeshRoot.transform.localRotation = Quaternion.identity;
		CurCarMesh.transform.parent = MeshRoot.transform;
		CurCarMesh.transform.localPosition = Vector3.zero;
		CurCarMesh.transform.localRotation = Quaternion.identity;
		curMountdata = mountData;
		curColordata = colordata;
	}

	public void ChangeColor(ColorData colordata)
	{
		if (CurCarMesh != null)
		{
			CurCarMesh.ChangeColor(colordata);
		}
		else
		{
			Debug.Log("not find mesh !!!!!");
		}
	}

	public void EnableFakeObjRoot()
	{
		StopRotate();
		curMountdata = null;
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
	}

	public void DisableFakeObjRoot()
	{
		if (CurCarMesh != null)
		{
			UnityVersionUtil.SetActiveRecursive(CurCarMesh.gameObject, state: false);
			CurCarMesh.transform.parent = null;
			Singleton<ObjManager>.Instance.RecycleMountCar(CurCarMesh);
			CurCarMesh = null;
		}
		StopRotate();
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

	public void PlayRotate()
	{
		rotateAnima.PlayAnima();
	}

	public void StopRotate()
	{
		rotateAnima.StopAnima();
	}
}
