using System.Collections.Generic;
using UnityEngine;

public class FakeObjRootLogic : SingletonUnity<FakeObjRootLogic>
{
	public Transform MeshRoot;

	public GameObject mTopLeftObj;

	public GameObject mBottomRightObj;

	public Camera objCam;

	public RenderTexture ModelPic;

	private float curRate = -1f;

	private Vector3 BeginPos = new Vector3(0f, -1f, 2.7f);

	private Vector3 BeginRot = new Vector3(0f, 175f, 0f);

	private Vector3 TargetPos;

	private Vector3 TargetRot;

	private bool MoveFlag;

	private List<ModelPartData> PartDataList = new List<ModelPartData>();

	private new void Awake()
	{
		base.Awake();
		base.transform.position = new Vector3(0f, 8f, 0f);
	}

	private void CreateModelPic(float rate)
	{
		MoveFlag = false;
		MeshRoot.localPosition = BeginPos;
		MeshRoot.localRotation = Quaternion.Euler(BeginRot);
		ModelPic = new RenderTexture((int)(512f * rate), 512, 16, RenderTextureFormat.ARGB32);
		ModelPic.useMipMap = false;
		objCam.targetTexture = ModelPic;
		objCam.ResetAspect();
	}

	public void SetPicValue(float rate = 0.5f)
	{
		MoveFlag = false;
		MeshRoot.localPosition = BeginPos;
		MeshRoot.localRotation = Quaternion.Euler(BeginRot);
		if (ModelPic == null || !ModelPic.IsCreated())
		{
			CreateModelPic(rate);
		}
		else
		{
			if (curRate == rate && ModelPic.IsCreated())
			{
				return;
			}
			if (ModelPic.IsCreated())
			{
				ModelPic.Release();
			}
			CreateModelPic(rate);
		}
		curRate = rate;
	}

	public void MoveShowPart(int profession, int curpart)
	{
		if (PartDataList.Count == 0)
		{
			PartDataList = DataManager.GetModelPartDataList();
		}
		MoveFlag = true;
		MeshRoot.localPosition = BeginPos;
		MeshRoot.localRotation = Quaternion.Euler(BeginRot);
		TargetPos = BeginPos;
		TargetRot = BeginRot;
		for (int i = 0; i < PartDataList.Count; i++)
		{
			if (profession == PartDataList[i].Profession && curpart == PartDataList[i].PartType)
			{
				TargetPos = new Vector3(PartDataList[i].CamPosX, PartDataList[i].CamPosY, PartDataList[i].CamPosZ);
				if (curpart == 0)
				{
					TargetRot = new Vector3(0f, PartDataList[i].RotationY, 0f);
				}
				break;
			}
		}
	}

	public void EnableFakeObjRoot()
	{
		objCam.cullingMask = 4194304;
		base.gameObject.layer = 22;
		MeshRoot.gameObject.layer = 22;
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

	private void Update()
	{
		if (MoveFlag)
		{
			MeshRoot.localPosition = Vector3.Lerp(MeshRoot.localPosition, TargetPos, Time.deltaTime * 1f);
			MeshRoot.localRotation = Quaternion.Slerp(MeshRoot.localRotation, Quaternion.Euler(TargetRot), Time.deltaTime * 1f);
			if (Vector3.Distance(MeshRoot.localPosition, TargetPos) < 0.05f)
			{
				MoveFlag = false;
			}
		}
	}
}
