using System.Collections.Generic;
using UnityEngine;

public class UIFlowLightCtl : MonoBehaviour
{
	public float mUvSpeed = 0.02f;

	public float mDuration = 3f;

	public float mPower = 2f;

	private float mUvAdd;

	private bool mIsPlaying;

	private Material picMat;

	public static Dictionary<int, Material> ShareGroudMat = new Dictionary<int, Material>();

	public static Dictionary<int, int> ReferenceCount = new Dictionary<int, int>();

	public int ShareGroup = -1;

	private float widthRate;

	private float heightRate;

	private float xOffsetRate;

	private float yOffsetRate;

	private UISpriteFlowLight sp;

	public AnimationCurve curve;

	private float timeCount;

	private float powerVal;

	private void Awake()
	{
		sp = base.gameObject.GetComponent<UISpriteFlowLight>();
		if (ShareGroup > -1)
		{
			if (ShareGroudMat.ContainsKey(ShareGroup))
			{
				picMat = ShareGroudMat[ShareGroup];
				Dictionary<int, int> referenceCount;
				Dictionary<int, int> dictionary = (referenceCount = ReferenceCount);
				int shareGroup;
				int key = (shareGroup = ShareGroup);
				shareGroup = referenceCount[shareGroup];
				dictionary[key] = shareGroup + 1;
			}
			else
			{
				picMat = Object.Instantiate(sp.material) as Material;
				ShareGroudMat.Add(ShareGroup, picMat);
				ReferenceCount.Add(ShareGroup, 1);
			}
			sp.mTestMat = picMat;
		}
		else
		{
			picMat = Object.Instantiate(sp.material) as Material;
			sp.mTestMat = picMat;
		}
		widthRate = (float)sp.GetAtlasSprite().width * 1f / (float)sp.atlas.spriteMaterial.mainTexture.width;
		heightRate = (float)sp.GetAtlasSprite().height * 1f / (float)sp.atlas.spriteMaterial.mainTexture.height;
		xOffsetRate = (float)sp.GetAtlasSprite().x * 1f / (float)sp.atlas.spriteMaterial.mainTexture.width;
		yOffsetRate = (float)(sp.atlas.spriteMaterial.mainTexture.height - (sp.GetAtlasSprite().y + sp.GetAtlasSprite().height)) * 1f / (float)sp.atlas.spriteMaterial.mainTexture.height;
		sp.mTestMat = picMat;
		mUvAdd = 0f;
		mIsPlaying = true;
		sp.onRender = UpdateMaterial;
	}

	private void UpdateReference()
	{
		if (ReferenceCount.ContainsKey(ShareGroup))
		{
			Dictionary<int, int> referenceCount;
			Dictionary<int, int> dictionary = (referenceCount = ReferenceCount);
			int shareGroup;
			int key = (shareGroup = ShareGroup);
			shareGroup = referenceCount[shareGroup];
			dictionary[key] = shareGroup - 1;
			if (ReferenceCount[ShareGroup] <= 0)
			{
				ShareGroudMat.Remove(ShareGroup);
				ReferenceCount.Remove(ShareGroup);
			}
		}
	}

	private void Start()
	{
		picMat.SetFloat("_WidthRate", widthRate);
		picMat.SetFloat("_HeightRate", heightRate);
		picMat.SetFloat("_XOffset", xOffsetRate);
		picMat.SetFloat("_YOffset", yOffsetRate);
	}

	private void OnDestroy()
	{
		UpdateReference();
	}

	private void Update()
	{
		timeCount += Time.deltaTime;
		mUvAdd += mUvSpeed;
		if (mUvAdd >= 1f)
		{
			mUvAdd -= 1f;
		}
		if (timeCount >= mDuration)
		{
			timeCount -= mDuration;
		}
		powerVal = curve.Evaluate(timeCount / mDuration) * mPower;
	}

	private void UpdateMaterial(Material mat)
	{
		mat.SetFloat("_FlowLightOffset", mUvAdd);
		mat.SetFloat("_FlowLightPower", powerVal);
	}

	private void PlayOnceAgain()
	{
		mUvAdd = 0f;
		timeCount = 0f;
		mIsPlaying = true;
	}
}
