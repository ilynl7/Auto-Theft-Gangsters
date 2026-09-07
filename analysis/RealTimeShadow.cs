using System.Collections.Generic;
using UnityEngine;

public class RealTimeShadow : SingletonUnity<RealTimeShadow>
{
	private static string shadowMatString = "Shader \"Hidden/ShadowMat\" {\n\tProperties {\n\t\t_ShadowLightness (\"_ShadowLightness\", Color) = (0,0,0,0)\n\t}\n\tSubShader {\n\t\tTags { \"RenderType\" = \"RealTimeShadow\"}\n\t\tPass {\n\t\t\tCull Off ZWrite Off\n\t\t\tColor [_ShadowLightness]\n\t\t}\n\t}\n\tFallback off\n   }";

	private Material m_ShadowMaterial;

	private static string projectorMatString = "\tShader \"Hidden/ShadowProjectorMultiply\" { \n\tProperties {\n\t\t_ShadowTex (\"Cookie\", 2D) = \"white\" { TexGen ObjectLinear }\n\t\t_FalloffTex (\"FallOff\", 2D) = \"white\" { TexGen ObjectLinear\t}\n\t}\n\tSubshader {\n\t\tPass {\n\t\t\tZWrite off\n\t\t\tOffset -1, -1\n\t\t\tColorMask RGB\n\t\t\tBlend DstColor Zero\n\t\t\tSetTexture [_ShadowTex] {\n\t\t\t\tcombine texture\n\t\t\t\tMatrix [_Projector]\n\t\t\t}\n\t\t\tSetTexture [_FalloffTex] {\n\t\t\t\tconstantColor (1,1,1,0)\n\t\t\t\tcombine previous lerp (texture) constant\n\t\t\t\tMatrix [_ProjectorClip]\n\t\t\t}\n\t\t}\n\t}\n}";

	private Material m_ProjectorMaterial;

	private static string projectorMatNoFallOffString = "\tShader \"Hidden/ShadowProjectorMultiply\" { \n\tProperties {\n\t\t_ShadowTex (\"Cookie\", 2D) = \"white\" { TexGen ObjectLinear }\n\t\t_FalloffTex (\"FallOff\", 2D) = \"white\" { TexGen ObjectLinear\t}\n\t}\n\tSubshader {\n\t\tPass {\n\t\t\tZWrite off\n\t\t\tOffset -1, -1\n\t\t\tFog { Color (1, 1, 1) }\n\t\t\tColorMask RGB\n\t\t\tBlend DstColor Zero\n\t\t\tSetTexture [_ShadowTex] {\n\t\t\t\tcombine texture, ONE - texture\n\t\t\t\tMatrix [_Projector]\n\t\t\t}\n\t\t}\n\t}\n}";

	private Material m_ProjectorNoFallOffMaterial;

	public GameObject TargetRenderObj;

	public LayerMask ShadowReceiverLayer = 8388608;

	public LayerMask ShadowCasterLayer = int.MinValue;

	public float CastingDistance = 10f;

	public Texture FalloffTex;

	private int ShadowPicSizeW = 256;

	private int ShadowPicSizeH = 256;

	private Vector3 ShadowAngle = new Vector3(55f, 70f, 0f);

	private RenderTexture mShadowPic;

	private Transform mShadowProjectorTrans;

	private Shader mShadowShader;

	private Camera mShadowCamera;

	private Projector mShadowProjector;

	private List<Renderer> mRenderList;

	private float mLightNess = 0.7f;

	private bool initFlag;

	private float extraWidth = 0.6f;

	private Material shadowMaterial
	{
		get
		{
			if (m_ShadowMaterial == null)
			{
				m_ShadowMaterial = new Material(shadowMatString);
			}
			return m_ShadowMaterial;
		}
	}

	private Material projectorMaterial
	{
		get
		{
			if (m_ProjectorMaterial == null)
			{
				m_ProjectorMaterial = new Material(projectorMatString);
			}
			return m_ProjectorMaterial;
		}
	}

	private Material projectorNoFallOffMaterial
	{
		get
		{
			if (m_ProjectorNoFallOffMaterial == null)
			{
				m_ProjectorNoFallOffMaterial = new Material(projectorMatNoFallOffString);
			}
			return m_ProjectorNoFallOffMaterial;
		}
	}

	public void InitShadow()
	{
		mShadowPic = new RenderTexture(ShadowPicSizeW, ShadowPicSizeH, 0);
		if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB1555))
		{
			mShadowPic.format = RenderTextureFormat.ARGB1555;
		}
		mShadowPic.useMipMap = false;
		GameObject gameObject = new GameObject("ShadowProjecter");
		mShadowProjectorTrans = gameObject.transform;
		mShadowProjectorTrans.rotation = Quaternion.Euler(ShadowAngle);
		mShadowShader = shadowMaterial.shader;
		mShadowCamera = gameObject.AddComponent<Camera>();
		mShadowCamera.clearFlags = CameraClearFlags.Color;
		mShadowCamera.backgroundColor = Color.white;
		mShadowCamera.cullingMask = ShadowCasterLayer;
		mShadowCamera.orthographic = true;
		mShadowCamera.aspect = 1f;
		mShadowCamera.targetTexture = mShadowPic;
		mShadowCamera.SetReplacementShader(mShadowShader, "RenderType");
		Material material = ((!(FalloffTex == null)) ? projectorMaterial : projectorNoFallOffMaterial);
		material.SetTexture("_ShadowTex", mShadowPic);
		if (FalloffTex != null)
		{
			material.SetTexture("_FalloffTex", FalloffTex);
		}
		mShadowProjector = gameObject.AddComponent<Projector>();
		mShadowProjector.orthographic = true;
		mShadowProjector.ignoreLayers = ~(int)ShadowReceiverLayer;
		mShadowProjector.material = material;
		Shader.SetGlobalColor("_ShadowLightness", new Color(mLightNess, mLightNess, mLightNess, 0f));
		mShadowProjector.enabled = false;
		mShadowCamera.enabled = false;
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager != null && sceneManager.CurrentMapInofData != null)
		{
			mShadowProjector.transform.eulerAngles = sceneManager.CurrentMapInofData.ShadowDir;
		}
	}

	private new void Awake()
	{
		base.Awake();
		mRenderList = new List<Renderer>();
		InitShadow();
	}

	public void Reset(GameObject targetObj)
	{
		TargetRenderObj = targetObj;
		mRenderList.Clear();
		Renderer[] componentsInChildren = TargetRenderObj.transform.GetChild(0).GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Debug.Log("renderMeshs :: " + i + " :: " + componentsInChildren[i].gameObject.name);
			mRenderList.Add(componentsInChildren[i]);
		}
		Debug.Log("renderMeshs :: " + componentsInChildren.Length);
		GetAllRendereBounds(out var _, out var extents);
		float num = extents.x;
		if (extents.y > num)
		{
			num = extents.y;
		}
		if (extents.z > num)
		{
			num = extents.z;
		}
		mShadowCamera.orthographicSize = num;
		mShadowCamera.nearClipPlane = 0f - num;
		mShadowCamera.farClipPlane = num;
		mShadowProjector.orthoGraphicSize = num;
		mShadowProjector.nearClipPlane = (0f - num) * 0.18f;
		mShadowProjector.farClipPlane = mShadowProjector.nearClipPlane + CastingDistance;
		EnableRealTimeShadow();
	}

	public void Reset(List<GameObject> targetRenderObjList)
	{
		mRenderList.Clear();
		for (int i = 0; i < targetRenderObjList.Count; i++)
		{
			mRenderList.Add(targetRenderObjList[i].GetComponent<Renderer>());
		}
		GetAllRendereBounds(out var _, out var extents);
		float num = extents.x;
		if (extents.y > num)
		{
			num = extents.y;
		}
		if (extents.z > num)
		{
			num = extents.z;
		}
		mShadowCamera.orthographicSize = num;
		mShadowCamera.nearClipPlane = 0f - num;
		mShadowCamera.farClipPlane = num;
		mShadowProjector.orthoGraphicSize = num;
		mShadowProjector.nearClipPlane = (0f - num) * 0.18f;
		mShadowProjector.farClipPlane = mShadowProjector.nearClipPlane + CastingDistance;
		EnableRealTimeShadow();
	}

	public void EnableRealTimeShadow()
	{
		base.enabled = true;
		if (!mShadowProjector.enabled)
		{
			mShadowProjector.enabled = true;
		}
		if (!mShadowCamera.enabled)
		{
			mShadowCamera.enabled = true;
		}
	}

	private void LateUpdate()
	{
		if (GetAllRendereBounds(out var center, out var _))
		{
			mShadowProjectorTrans.position = center;
		}
	}

	private bool GetAllRendereBounds(out Vector3 center, out Vector3 extents)
	{
		int count = mRenderList.Count;
		bool result = false;
		Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		for (int i = 0; i < count; i++)
		{
			Renderer renderer = mRenderList[i];
			if (renderer != null)
			{
				Bounds bounds = renderer.bounds;
				Vector3 center2 = bounds.center;
				Vector3 extents2 = bounds.extents;
				float num = center2.x - extents2.x - extraWidth;
				if (num < vector.x)
				{
					vector.x = num;
				}
				float num2 = center2.x + extents2.x + extraWidth;
				if (num2 > vector2.x)
				{
					vector2.x = num2;
				}
				float num3 = center2.y - extents2.y - extraWidth;
				if (num3 < vector.y)
				{
					vector.y = num3;
				}
				float num4 = center2.y + extents2.y + extraWidth;
				if (num4 > vector2.y)
				{
					vector2.y = num4;
				}
				float num5 = center2.z + extents2.z + extraWidth;
				if (num5 > vector2.z)
				{
					vector2.z = num5;
				}
				float num6 = center2.z - extents2.z - extraWidth;
				if (num6 < vector.z)
				{
					vector.z = num6;
				}
				result = true;
			}
		}
		center.x = 0.5f * (vector2.x + vector.x);
		center.y = 0.5f * (vector2.y + vector.y);
		center.z = 0.5f * (vector2.z + vector.z);
		extents.x = 0.7f * (vector2.x - vector.x);
		extents.y = 0.7f * (vector2.y - vector.y);
		extents.z = 0.7f * (vector2.z - vector.z);
		return result;
	}

	public void DisableRealTimeShadow()
	{
		base.enabled = false;
		if (mShadowProjector.enabled)
		{
			mShadowProjector.enabled = false;
		}
		if (mShadowCamera.enabled)
		{
			mShadowCamera.enabled = false;
		}
	}
}
