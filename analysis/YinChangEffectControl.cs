using UnityEngine;

public class YinChangEffectControl : MonoBehaviour
{
	public delegate void onFinishedDelegate();

	public string effectId = string.Empty;

	private float mEndTime;

	private MeshFilter meshFilter;

	private float speed;

	private float progress;

	private Material mMaterial;

	private onFinishedDelegate onFinished;

	public void InitEffect(EffInfoData effInfoData, float duration, Vector3 pos, Quaternion rotation, onFinishedDelegate dele = null)
	{
		effectId = effInfoData.ID;
		speed = 1f / duration;
		mEndTime = Time.time + duration;
		progress = 0f;
		mMaterial = GetComponent<Renderer>().sharedMaterial;
		meshFilter = GetComponent<MeshFilter>();
		mMaterial.SetFloat("_Progress", 0f);
		if (effInfoData != null)
		{
			MeshCreate meshCreate = null;
			if (effInfoData.AreaType == 1)
			{
				meshCreate = new SectorMeshCreate();
				float distance = (float)effInfoData.Param1 / 100f;
				meshFilter.sharedMesh = meshCreate.Create(distance, 360f);
			}
			else if (effInfoData.AreaType == 2)
			{
				meshCreate = new RectangleCreate();
				float distance2 = (float)effInfoData.Param1 / 100f;
				float parm = (float)effInfoData.Param2 / 100f;
				meshFilter.sharedMesh = meshCreate.Create(distance2, parm);
			}
			else if (effInfoData.AreaType == 3)
			{
				meshCreate = new SectorMeshCreate();
				float distance3 = (float)effInfoData.Param1 / 100f;
				float parm2 = effInfoData.Param2;
				meshFilter.sharedMesh = meshCreate.Create(distance3, parm2);
			}
		}
		base.transform.position = pos;
		base.transform.rotation = rotation;
		onFinished = dele;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (!(mEndTime > 0f))
		{
			return;
		}
		progress += Time.deltaTime * speed;
		if (progress > 1f)
		{
			progress = 1f;
		}
		mMaterial.SetFloat("_Progress", progress);
		if (progress >= 1f)
		{
			if (onFinished != null)
			{
				onFinished();
			}
			Object.Destroy(base.gameObject);
		}
	}
}
