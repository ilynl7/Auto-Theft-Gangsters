using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class BundleManager
{
	public class LoadModelData
	{
		public enum BundleType
		{
			MODEL,
			EFFECT
		}

		public string ModelName = string.Empty;

		public OnLoadModelFinish OnLoadFinished;

		public object param1;

		public object param2;

		public string SubPath = string.Empty;

		public bool DoNotCache;

		public BundleType bundleType;

		public bool IsNeedUnload = true;

		public long ID;

		private string mLoadURL = string.Empty;

		public string LoadURL
		{
			get
			{
				if (string.IsNullOrEmpty(mLoadURL))
				{
					mLoadURL = GetLoadUrl();
				}
				return mLoadURL;
			}
		}

		public LoadModelData(string modelName, bool isNeedUnload, BundleType bType = BundleType.MODEL, bool isDoNotCache = false, OnLoadModelFinish onFinished = null, object paramData1 = null, object paramData2 = null, string sPath = null)
		{
			ModelName = modelName;
			OnLoadFinished = onFinished;
			param1 = paramData1;
			param2 = paramData2;
			SubPath = sPath;
			DoNotCache = isDoNotCache;
			bundleType = bType;
			IsNeedUnload = isNeedUnload;
			ID = UUID.GenUUID();
		}

		private string GetLoadUrl()
		{
			sb.Length = 0;
			sb1.Length = 0;
			string fileName = sb.AppendFormat("{0}{1}", ModelName, ".bundle").ToString();
			if (bundleType == BundleType.MODEL)
			{
				return GetLocalUrl(sb1.AppendFormat("{0}{1}{2}", BundleModelRootPath, "/", SubPath).ToString(), fileName);
			}
			return GetLocalUrl(BundleEffectRootPath, fileName);
		}

		public static string GetLoadUrl(string modelName, string subPath)
		{
			sb.Length = 0;
			sb1.Length = 0;
			string fileName = sb.AppendFormat("{0}{1}", modelName, ".bundle").ToString();
			return GetLocalUrl(sb1.AppendFormat("{0}{1}{2}", BundleModelRootPath, "/", subPath).ToString(), fileName);
		}
	}

	public class TextureInfo
	{
		public string name;

		public Texture mTexture;

		public int useTimes;

		public float m_LastActiveTime;

		public TextureInfo(string Name, Texture curTex, int use = 0, float time = 0f)
		{
			name = Name;
			mTexture = curTex;
			useTimes = use;
			m_LastActiveTime = time;
		}
	}

	public class LoadTextureData
	{
		public string TextureName = string.Empty;

		public LoadTextureFinish OnLoadFinished;

		public LoadTextureData(string texname, LoadTextureFinish onFinished = null)
		{
			TextureName = texname;
			OnLoadFinished = onFinished;
		}
	}

	public delegate void OnLoadModelFinish(object objBundle, object param1 = null, object param2 = null);

	public delegate void LoadUIBundleFinish(UIPathData uiData, GameObject retObj, UIManager.OnOpenUIDelegate param1, object param2);

	public delegate void OnLoadSceneFinish(bool isSuccess, string sceneName, AssetBundle sceneBundle);

	public delegate void OnLoadActObjFinish(string name, SceneComponentData data, UnityEngine.Object obj);

	public delegate void OnLoadAnimationFinishedDelegate();

	public delegate void OnLoadDataFinishedDelegate();

	public delegate void LoadSoundFinish(string modelName, AudioClip audioClip, object param1, object param2, object param3 = null);

	public delegate void LoadTextureFinish(string name, Texture textureObj);

	public delegate void LoadTextureDicFinish(Dictionary<string, Texture> DicTex);

	public delegate void LoaditemsFinish(string name, UnityEngine.Object obj, object param1 = null, object param2 = null);

	protected const string Separator = "/";

	protected const string BundleSuffix = ".bundle";

	protected const string CommonShader = "CommonShader.bundle";

	protected const string FilePrefix = "file:///";

	protected const string SceneRoot = "/Scene";

	protected const string SceneAnimaRoot = "/StartSceneAnima";

	private const string EffectCommonShader = "EffectCommonShader.bundle";

	private const string CommonPic = "CommonPic.bundle";

	public static bool IsCanUnloadBundle = true;

	public static string BundleRoot = "/Bundle";

	public static string BundleModelRootPath = BundleRoot + "/Model";

	public static StringBuilder sb = new StringBuilder(512);

	public static StringBuilder sb1 = new StringBuilder(512);

	public static bool ModelLoadingShaderFlag = false;

	private static AssetBundle mModelCommonShaderBundle = null;

	private static List<LoadModelData> mLoadModelList = new List<LoadModelData>();

	private static List<string> mLoadingBundle = new List<string>();

	private static Dictionary<string, List<LoadModelData>> mWaitingBundleDic = new Dictionary<string, List<LoadModelData>>();

	private static Dictionary<string, AssetBundleData> mModelBundleCacheDic = new Dictionary<string, AssetBundleData>();

	public static string FontUIName = "Font.bundle";

	public static string CommonUIName = "Common.bundle";

	public static string UIPathRoot = "/UI";

	public static string CommonUIPath = "/UI/Common";

	public static string GameUIPath = "/UI/Common/GameUI";

	public static string MenuUIPath = "/UI/Common/MenuUI";

	private static AssetBundle mFontUIBundle = null;

	private static AssetBundle mCommonUIBundle = null;

	private static List<string> mGameUIBundleList = new List<string>();

	private static List<string> mMenuUIBundleList = new List<string>();

	private static List<string> mCommonUIBundleList = new List<string>();

	private static Dictionary<string, AssetBundleData> mUICacheBundleDic = new Dictionary<string, AssetBundleData>();

	private static List<string> loadingUIBundleList = new List<string>();

	private static Dictionary<string, int> waitingUIBundleNum = new Dictionary<string, int>();

	private static Dictionary<string, AssetBundle> mSceneBundleCacheDic = new Dictionary<string, AssetBundle>();

	private static List<string> mCacheSceneList = new List<string>();

	private static AssetBundle mCommonSceneBundle = null;

	public static string CommonRootPath = BundleRoot + "/Activity/CommonObj";

	public static string ActObjRootPath = BundleRoot + "/Activity/Obj";

	private static Dictionary<string, AssetBundle> mActivityBundleCacheDic = new Dictionary<string, AssetBundle>();

	private static List<string> mCacheActObjList = new List<string>();

	private static AssetBundle mCommonActObjBundle = null;

	private static string BundleEffectRootPath = BundleRoot + "/Effect";

	private static bool effectCommonFileLoadingFlag = false;

	private static AssetBundle mEffectCommonShaderBundle = null;

	private static AssetBundle mEffectCommonPicBundle = null;

	private static string AnimationRootPath = BundleRoot + "/Animation";

	private static List<string> AnimationStreamFileNameList = new List<string> { "baiRen_XD", "heiRen_QJ", "nvRen_QS" };

	private static List<string> AnimationFileNameList = new List<string> { "baiRen_QJ", "baiRen_QS", "heiRen_XD", "heiRen_QS", "nvRen_XD", "nvRen_QJ" };

	private static Dictionary<string, AssetBundle> mAnimationBundleDic = new Dictionary<string, AssetBundle>();

	private static bool DownloadAnimationFlag = false;

	private static bool StreamAnimationFlag = false;

	private static string DataRootPath = BundleRoot + "/Data";

	private static string DataFileName = "Data.bundle";

	private static AssetBundle mDataBundle = null;

	private static bool mLoadingDataBundleFlag = false;

	private static bool mWaittingLoadDataFlag = false;

	private static string SoundRootPath = BundleRoot + "/";

	private static string TextureRootPath = BundleRoot + "/Items/Texture";

	public static int MaxTextureNum = 20;

	public static Dictionary<string, TextureInfo> mTextureDic = new Dictionary<string, TextureInfo>();

	public static List<string> mCurLoadingTextureList = new List<string>();

	public static List<string> mLoadTextureBundle = new List<string>();

	public static Dictionary<string, List<LoadTextureData>> mWaitTextureDic = new Dictionary<string, List<LoadTextureData>>();

	public static Texture NextLoadingTexture = null;

	private static string ItemsRootPath = BundleRoot + "/Items/ShowModel";

	public static Dictionary<string, AssetBundleData> ModelBundleCacheDic => mModelBundleCacheDic;

	public static AssetBundle DataBundle => mDataBundle;

	public static bool UnloadBundle(AssetBundle curBundle, bool flag)
	{
		if (IsCanUnloadBundle)
		{
			if (curBundle != null)
			{
				curBundle.Unload(flag);
			}
			return true;
		}
		return false;
	}

	public static void CleanmLoadModelList()
	{
		mLoadModelList.Clear();
	}

	public static LoadModelData LoadModelInList(string modelName, bool isNeedUnload, bool isDoNotCache = false, OnLoadModelFinish onFinished = null, object paramData1 = null, object paramData2 = null, string subPath = null)
	{
		object obj = ResourcesManager.LoadAndInstantiate("TestModel/" + modelName);
		if (obj != null)
		{
			onFinished?.Invoke(obj, paramData1, paramData2);
			return null;
		}
		LoadModelData loadModelData = new LoadModelData(modelName, isNeedUnload, LoadModelData.BundleType.MODEL, isDoNotCache, onFinished, paramData1, paramData2, subPath);
		if (mModelBundleCacheDic.ContainsKey(loadModelData.ModelName) && mModelBundleCacheDic[loadModelData.ModelName].IsBundleValid())
		{
			if (onFinished != null)
			{
				mModelBundleCacheDic[loadModelData.ModelName].AddBundleUsingCount();
				obj = UnityEngine.Object.Instantiate(mModelBundleCacheDic[loadModelData.ModelName].GetBundle().mainAsset);
				onFinished(obj, paramData1, paramData2);
			}
			return null;
		}
		if (!string.IsNullOrEmpty(loadModelData.SubPath) && !mModelBundleCacheDic.ContainsKey(loadModelData.SubPath))
		{
			mModelBundleCacheDic.Add(loadModelData.SubPath, new AssetBundleData(null, 0, loadModelData.SubPath, string.Empty, isDepend: true));
		}
		if (!mModelBundleCacheDic.ContainsKey(loadModelData.ModelName))
		{
			mModelBundleCacheDic.Add(loadModelData.ModelName, new AssetBundleData(null, 0, loadModelData.ModelName, loadModelData.SubPath, isDepend: false));
		}
		mModelBundleCacheDic[loadModelData.ModelName].AddBundleUsingCount();
		mLoadModelList.Add(loadModelData);
		return loadModelData;
	}

	public static void LoadModelListUpdate(MonoBehaviour mono)
	{
		if (mono == null || mLoadModelList.Count <= 0)
		{
			return;
		}
		if (mLoadModelList[0].bundleType == LoadModelData.BundleType.MODEL)
		{
			if (mModelCommonShaderBundle == null)
			{
				if (!ModelLoadingShaderFlag && UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
				{
					mono.StartCoroutine(LoadCommonShader());
				}
				return;
			}
			LoadModelData loadModelData = mLoadModelList[0];
			mLoadModelList.RemoveAt(0);
			if (!string.IsNullOrEmpty(loadModelData.SubPath))
			{
				if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
				{
					mono.StartCoroutine(LoadDependPicBundle(loadModelData, mono));
				}
			}
			else if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
			{
				mono.StartCoroutine(LoadModelFromList(loadModelData));
			}
		}
		else if (mEffectCommonPicBundle == null)
		{
			if (!effectCommonFileLoadingFlag && UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
			{
				mono.StartCoroutine(LoadEffectCommonFile());
			}
		}
		else
		{
			LoadModelData curData = mLoadModelList[0];
			mLoadModelList.RemoveAt(0);
			if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
			{
				mono.StartCoroutine(LoadModelFromList(curData));
			}
		}
	}

	private static IEnumerator LoadModelFromList(LoadModelData curData)
	{
		string loadUrl = curData.LoadURL;
		if (LoadModelFromCache(curData))
		{
			yield break;
		}
		if (!mLoadingBundle.Contains(loadUrl))
		{
			mLoadingBundle.Add(loadUrl);
			WWW www = new WWW(loadUrl);
			yield return www;
			if (www.assetBundle != null)
			{
				mModelBundleCacheDic[curData.ModelName].SetBundle(www.assetBundle);
				if (curData.OnLoadFinished != null)
				{
					if (mWaitingBundleDic.ContainsKey(loadUrl))
					{
						for (int i = 0; i < mWaitingBundleDic[loadUrl].Count; i++)
						{
							LoadModelFromCache(mWaitingBundleDic[loadUrl][i]);
						}
						mWaitingBundleDic.Remove(loadUrl);
					}
					curData.OnLoadFinished(UnityEngine.Object.Instantiate(www.assetBundle.mainAsset), curData.param1, curData.param2);
				}
				else
				{
					if (mWaitingBundleDic.ContainsKey(loadUrl) && mWaitingBundleDic.ContainsKey(loadUrl))
					{
						for (int j = 0; j < mWaitingBundleDic[loadUrl].Count; j++)
						{
							LoadModelFromCache(mWaitingBundleDic[loadUrl][j]);
						}
						mWaitingBundleDic.Remove(loadUrl);
					}
					if (mModelBundleCacheDic[curData.ModelName].UnLoadBundle())
					{
						mModelBundleCacheDic.Remove(curData.ModelName);
					}
				}
				if (mLoadingBundle.Contains(loadUrl))
				{
					mLoadingBundle.Remove(loadUrl);
				}
			}
			else
			{
				Log.ERROR_MSG("load bundle fail : " + curData.LoadURL);
			}
		}
		else
		{
			if (!mWaitingBundleDic.ContainsKey(loadUrl))
			{
				mWaitingBundleDic.Add(loadUrl, new List<LoadModelData>());
			}
			mWaitingBundleDic[loadUrl].Add(curData);
		}
	}

	private static bool LoadModelFromCache(LoadModelData curData)
	{
		if (mModelBundleCacheDic[curData.ModelName].IsBundleValid())
		{
			if (curData.OnLoadFinished != null)
			{
				UnityEngine.Object objBundle = UnityEngine.Object.Instantiate(mModelBundleCacheDic[curData.ModelName].GetBundle().mainAsset);
				curData.OnLoadFinished(objBundle, curData.param1, curData.param2);
				return true;
			}
			if (mModelBundleCacheDic[curData.ModelName].UnLoadBundle())
			{
				mModelBundleCacheDic.Remove(curData.ModelName);
			}
			return true;
		}
		return false;
	}

	private static IEnumerator LoadCommonShader()
	{
		ModelLoadingShaderFlag = true;
		WWW wwwCommonShader = new WWW(GetLocalUrl(BundleModelRootPath, "CommonShader.bundle"));
		yield return wwwCommonShader;
		if (wwwCommonShader.assetBundle != null)
		{
			wwwCommonShader.assetBundle.LoadAll();
			mModelCommonShaderBundle = wwwCommonShader.assetBundle;
		}
		else
		{
			Debug.Log("no shader LoadModelFromList");
		}
	}

	private static IEnumerator LoadDependPicBundle(LoadModelData curData, MonoBehaviour mono)
	{
		if (!mModelBundleCacheDic[curData.SubPath].IsBundleValid())
		{
			if (!mLoadingBundle.Contains(curData.SubPath))
			{
				mLoadingBundle.Add(curData.SubPath);
				sb.Length = 0;
				sb1.Length = 0;
				sb.AppendFormat("{0}{1}{2}", BundleModelRootPath, "/", curData.SubPath);
				sb1.AppendFormat("{0}{1}", curData.SubPath, ".bundle");
				string str = GetLocalUrl(sb.ToString(), sb1.ToString());
				WWW wwwDependPic = new WWW(str);
				yield return wwwDependPic;
				if (wwwDependPic.assetBundle != null)
				{
					wwwDependPic.assetBundle.LoadAll();
					mModelBundleCacheDic[curData.SubPath].SetBundle(wwwDependPic.assetBundle);
					mLoadingBundle.Remove(curData.SubPath);
					if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
					{
						mono.StartCoroutine(LoadModelFromList(curData));
					}
					if (!mWaitingBundleDic.ContainsKey(curData.SubPath))
					{
						yield break;
					}
					List<LoadModelData> waitingList = mWaitingBundleDic[curData.SubPath];
					for (int i = 0; i < waitingList.Count; i++)
					{
						if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
						{
							mono.StartCoroutine(LoadModelFromList(waitingList[i]));
						}
					}
					mWaitingBundleDic.Remove(curData.SubPath);
				}
				else
				{
					Debug.Log("wwwDependPic.assetBundle == null");
				}
			}
			else
			{
				if (!mWaitingBundleDic.ContainsKey(curData.SubPath))
				{
					mWaitingBundleDic.Add(curData.SubPath, new List<LoadModelData>());
				}
				mWaitingBundleDic[curData.SubPath].Add(curData);
			}
		}
		else if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
		{
			mono.StartCoroutine(LoadModelFromList(curData));
		}
	}

	public static void UnloadModel(string modelId, long curLoadingDataId, bool isMainPlayerUnload)
	{
		if (!string.IsNullOrEmpty(modelId))
		{
			ModelData modeDataByID = DataManager.GetModeDataByID(modelId);
			if (modeDataByID != null)
			{
				UnloadModel(modeDataByID.Name, modeDataByID.ModelPath, curLoadingDataId, isMainPlayerUnload);
				return;
			}
			CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(modelId);
			UnloadModel(characterModelDataByID.Name, string.Empty, curLoadingDataId, isMainPlayerPart: false);
		}
	}

	public static void UnloadModel(string modelName, string subPath, long loadingDataId, bool isMainPlayerPart)
	{
		if (mModelBundleCacheDic.ContainsKey(modelName) && mModelBundleCacheDic[modelName].IsBundleValid() && mModelBundleCacheDic[modelName].UnLoadBundle())
		{
			mModelBundleCacheDic.Remove(modelName);
		}
	}

	public static void RemoveFromLoadModelList(long loadingDataId)
	{
		if (loadingDataId == -1)
		{
			return;
		}
		for (int i = 0; i < mLoadModelList.Count; i++)
		{
			if (mLoadModelList[i].ID == loadingDataId)
			{
				LoadModelData loadModelData = mLoadModelList[i];
				mLoadModelList.RemoveAt(i);
				if (mModelBundleCacheDic.ContainsKey(loadModelData.ModelName) && mModelBundleCacheDic[loadModelData.ModelName].UnLoadBundle())
				{
					mModelBundleCacheDic.Remove(loadModelData.ModelName);
				}
				break;
			}
		}
	}

	public static void ClearCacheModelBundle(bool isClearAll = false)
	{
		List<KeyValuePair<string, AssetBundleData>> list = new List<KeyValuePair<string, AssetBundleData>>(mModelBundleCacheDic);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Value.Clear(isClearAll))
			{
				mModelBundleCacheDic.Remove(list[i].Key);
			}
		}
		mLoadModelList.Clear();
		mLoadingBundle.Clear();
		mWaitingBundleDic.Clear();
		Resources.UnloadUnusedAssets();
		GC.Collect();
	}

	public static void ClearMainPlayerBundleFlag(string weaponId, string headId, string bodyId, string legId, string carId)
	{
		List<string> list = new List<string>();
		if (!string.IsNullOrEmpty(weaponId))
		{
			ModelData modeDataByID = DataManager.GetModeDataByID(weaponId);
			if (modeDataByID != null)
			{
				list.Add(modeDataByID.Name);
				if (!string.IsNullOrEmpty(modeDataByID.ModelPath))
				{
					list.Add(modeDataByID.ModelPath);
				}
			}
		}
		if (!string.IsNullOrEmpty(headId))
		{
			ModelData modeDataByID2 = DataManager.GetModeDataByID(headId);
			list.Add(modeDataByID2.Name);
			if (!string.IsNullOrEmpty(modeDataByID2.ModelPath) && !list.Contains(modeDataByID2.ModelPath))
			{
				list.Add(modeDataByID2.ModelPath);
			}
		}
		if (!string.IsNullOrEmpty(bodyId))
		{
			ModelData modeDataByID3 = DataManager.GetModeDataByID(bodyId);
			list.Add(modeDataByID3.Name);
			if (!string.IsNullOrEmpty(modeDataByID3.ModelPath) && !list.Contains(modeDataByID3.ModelPath))
			{
				list.Add(modeDataByID3.ModelPath);
			}
		}
		if (!string.IsNullOrEmpty(legId))
		{
			ModelData modeDataByID4 = DataManager.GetModeDataByID(legId);
			list.Add(modeDataByID4.Name);
			if (!string.IsNullOrEmpty(modeDataByID4.ModelPath) && !list.Contains(modeDataByID4.ModelPath))
			{
				list.Add(modeDataByID4.ModelPath);
			}
		}
		if (!string.IsNullOrEmpty(carId))
		{
			ModelData modeDataByID5 = DataManager.GetModeDataByID(DataManager.GetMountDataById(carId).ModelId);
			list.Add(modeDataByID5.Name);
			if (!string.IsNullOrEmpty(modeDataByID5.ModelPath) && !list.Contains(modeDataByID5.ModelPath))
			{
				list.Add(modeDataByID5.ModelPath);
			}
		}
		List<string> list2 = new List<string>(mModelBundleCacheDic.Keys);
		for (int num = list2.Count - 1; num >= 0; num--)
		{
			if (!list.Contains(list2[num]) && mModelBundleCacheDic.ContainsKey(list2[num]) && !mModelBundleCacheDic[list2[num]].IsDependObj && mModelBundleCacheDic[list2[num]].UsingCount <= 0 && mModelBundleCacheDic[list2[num]].UnLoadBundle())
			{
				mModelBundleCacheDic.Remove(list2[num]);
			}
		}
	}

	public static void UnloadCommonShaderBundle()
	{
		if (UnloadBundle(mModelCommonShaderBundle, flag: true))
		{
			mModelCommonShaderBundle = null;
		}
	}

	public static void AddOutLineMaterial(GameObject obj)
	{
		SkinnedMeshRenderer component = obj.GetComponent<SkinnedMeshRenderer>();
		Material[] array = new Material[component.materials.Length + 1];
		for (int i = 0; i < component.materials.Length; i++)
		{
			if (component.materials[i].name.Contains("XRay"))
			{
				return;
			}
			array[i] = component.materials[i];
		}
		UnityEngine.Object @object = ResourcesManager.Load("Material/XRay");
		if (null != @object)
		{
			array[component.materials.Length] = UnityEngine.Object.Instantiate(@object) as Material;
		}
		component.materials = array;
	}

	public static void ResetParticleShader(Transform obj)
	{
		ParticleSystem component = obj.GetComponent<ParticleSystem>();
		if (component != null)
		{
			Material material = component.renderer.material;
			string name = material.shader.name;
			if (!string.IsNullOrEmpty(name))
			{
				Shader shader = Shader.Find(name);
				if (shader != null)
				{
					material.shader = shader;
				}
				else
				{
					Debug.Log("unable to refresh shader: " + name + " in material " + material.name);
				}
			}
		}
		for (int i = 0; i < obj.childCount; i++)
		{
			ResetParticleShader(obj.transform.GetChild(i));
		}
	}

	public static void ResetAllShader(Transform obj)
	{
		if (obj.renderer != null && obj.renderer.material != null)
		{
			Material material = obj.renderer.material;
			string text = material.shader.name;
			if (!string.IsNullOrEmpty(text))
			{
				if (GameSettingData.IsLowPhone && text.Contains("Outline_"))
				{
					text = "Mobile/Diffuse";
				}
				Shader shader = Shader.Find(text);
				if (shader != null)
				{
					material.shader = shader;
				}
				else
				{
					Debug.Log("unable to refresh shader: " + text + " in material " + material.name);
				}
			}
		}
		for (int i = 0; i < obj.childCount; i++)
		{
			ResetAllShader(obj.transform.GetChild(i));
		}
	}

	public static bool ResetShader(Transform obj)
	{
		if (obj.renderer != null && obj.renderer.material != null)
		{
			Material material = obj.renderer.material;
			string text = material.shader.name;
			if (!string.IsNullOrEmpty(text))
			{
				if (GameSettingData.IsLowPhone && text.Contains("Outline_"))
				{
					text = "Mobile/Diffuse";
				}
				Shader shader = Shader.Find(text);
				if (shader != null)
				{
					material.shader = shader;
				}
				else
				{
					Debug.Log("unable to refresh shader: " + text + " in material " + material.name);
				}
			}
			return true;
		}
		for (int i = 0; i < obj.childCount; i++)
		{
			if (ResetShader(obj.transform.GetChild(i)))
			{
				return true;
			}
		}
		return false;
	}

	public static bool ResetShader(Transform obj, out Material sm)
	{
		if (obj.renderer != null && obj.renderer.material != null)
		{
			sm = obj.renderer.material;
			string text = sm.shader.name;
			if (!string.IsNullOrEmpty(text))
			{
				if (GameSettingData.IsLowPhone && text.Contains("Outline_"))
				{
					text = "Mobile/Diffuse";
				}
				Shader shader = Shader.Find(text);
				if (shader != null)
				{
					sm.shader = shader;
				}
				else
				{
					Debug.Log("unable to refresh shader: " + text + " in material " + sm.name);
				}
			}
			return true;
		}
		for (int i = 0; i < obj.childCount; i++)
		{
			if (ResetShader(obj.transform.GetChild(i), out sm))
			{
				return true;
			}
		}
		sm = null;
		return false;
	}

	public static void RebuildBones(ObjCharacter parent, GameObject skineObject)
	{
		BoneSave component = skineObject.GetComponent<BoneSave>();
		List<string> list = component.list;
		List<Transform> list2 = new List<Transform>();
		if (parent.BonesTrsDict == null)
		{
			parent.BonesTrsDict = parent.GetComponentInChildren<TransDicts>().TransDict;
		}
		for (int i = 0; i < list.Count; i++)
		{
			Transform transform = parent.BonesTrsDict[list[i]];
			if (transform != null)
			{
				list2.Add(transform);
			}
			else
			{
				Log.DEBUG_MSG("Find bone erro=" + list[i]);
			}
		}
		SkinnedMeshRenderer component2 = skineObject.GetComponent<SkinnedMeshRenderer>();
		component2.bones = list2.ToArray();
		component2.enabled = true;
		component2.updateWhenOffscreen = true;
		parent.AnimationLogic.AnimaObj.cullingType = AnimationCullingType.AlwaysAnimate;
	}

	public static void RebuildBones(GameObject parent, GameObject skineObject)
	{
		BoneSave component = skineObject.GetComponent<BoneSave>();
		List<string> list = component.list;
		List<Transform> list2 = new List<Transform>();
		Dictionary<string, Transform> dictionary = null;
		if (!parent.gameObject.activeSelf)
		{
			UnityVersionUtil.SetActiveRecursive(parent.gameObject, state: true);
			TransDicts transDicts = parent.GetComponent<TransDicts>();
			if (transDicts == null)
			{
				transDicts = parent.GetComponentInChildren<TransDicts>();
			}
			dictionary = transDicts.TransDict;
			UnityVersionUtil.SetActiveRecursive(parent.gameObject, state: false);
		}
		else
		{
			TransDicts transDicts2 = parent.GetComponent<TransDicts>();
			if (transDicts2 == null)
			{
				transDicts2 = parent.GetComponentInChildren<TransDicts>();
			}
			dictionary = transDicts2.TransDict;
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (dictionary.ContainsKey(list[i]))
			{
				Transform transform = dictionary[list[i]];
				if (transform != null)
				{
					list2.Add(transform);
				}
				else
				{
					Log.DEBUG_MSG("Find bone erro=" + list[i]);
				}
			}
		}
		SkinnedMeshRenderer component2 = skineObject.GetComponent<SkinnedMeshRenderer>();
		component2.bones = list2.ToArray();
		component2.enabled = true;
		parent.animation.cullingType = AnimationCullingType.AlwaysAnimate;
	}

	public static string GetDataLocalUrl(string folderPath, string fileName)
	{
		if (folderPath.EndsWith("/"))
		{
			folderPath = folderPath.Substring(0, folderPath.Length - 1);
		}
		sb.Length = 0;
		sb1.Length = 0;
		sb.AppendFormat("{0}{1}{2}{3}", FileUpdateHelper.GetLocalPathRoot(), folderPath, "/", fileName);
		string text = sb.ToString();
		if (PlayerData.downLoadFlag == 1 && File.Exists(text))
		{
			sb.Length = 0;
			return sb.AppendFormat("{0}{1}", "file:///", text).ToString();
		}
		sb.Length = 0;
		return sb.AppendFormat("{0}{1}{2}{3}", Application.streamingAssetsPath, folderPath, "/", fileName).ToString();
	}

	public static string GetLocalUrl(string folderPath, string fileName)
	{
		if (folderPath.EndsWith("/"))
		{
			folderPath = folderPath.Substring(0, folderPath.Length - 1);
		}
		sb.Length = 0;
		sb1.Length = 0;
		sb.AppendFormat("{0}{1}{2}{3}", FileUpdateHelper.GetLocalPathRoot(), folderPath, "/", fileName);
		string text = sb.ToString();
		if (File.Exists(text))
		{
			sb.Length = 0;
			return sb.AppendFormat("{0}{1}", "file:///", text).ToString();
		}
		sb.Length = 0;
		return sb.AppendFormat("{0}{1}{2}{3}", Application.streamingAssetsPath, folderPath, "/", fileName).ToString();
	}

	public static string GetLocalCheckPath(string folderPath, string fileName)
	{
		if (folderPath.EndsWith("/"))
		{
			folderPath = folderPath.Substring(0, folderPath.Length - 1);
		}
		sb.Length = 0;
		sb1.Length = 0;
		sb.AppendFormat("{0}{1}{2}{3}", FileUpdateHelper.GetLocalPathRoot(), folderPath, "/", fileName);
		return sb.ToString();
	}

	public static string GetLocalStreamUrl(string folderPath, string fileName)
	{
		if (folderPath.EndsWith("/"))
		{
			folderPath = folderPath.Substring(0, folderPath.Length - 1);
		}
		sb.Length = 0;
		sb1.Length = 0;
		sb.AppendFormat("{0}{1}{2}{3}", FileUpdateHelper.GetLocalPathRoot(), folderPath, "/", fileName);
		string text = sb.ToString();
		sb.Length = 0;
		return sb.AppendFormat("{0}{1}{2}{3}", Application.streamingAssetsPath, folderPath, "/", fileName).ToString();
	}

	public static IEnumerator LoadScene(string sceneName, OnLoadSceneFinish onLoadFinish)
	{
		if (mCommonSceneBundle == null)
		{
			sb.Length = 0;
			sb1.Length = 0;
			string commonLoadPath = GetLocalUrl(sb.AppendFormat("{0}{1}", BundleRoot, "/Scene").ToString(), sb1.AppendFormat("CommonPrefab{0}", ".bundle").ToString());
			WWW commonWWW = new WWW(commonLoadPath);
			yield return commonWWW;
			mCommonSceneBundle = commonWWW.assetBundle;
		}
		if (mSceneBundleCacheDic.ContainsKey(sceneName))
		{
			mCacheSceneList.Remove(sceneName);
			mCacheSceneList.Add(sceneName);
			mCommonSceneBundle.LoadAll();
			mSceneBundleCacheDic[sceneName].LoadAll();
			onLoadFinish?.Invoke(isSuccess: true, sceneName, mSceneBundleCacheDic[sceneName]);
			yield break;
		}
		sb.Length = 0;
		sb1.Length = 0;
		string loadPath = GetLocalUrl(sb.AppendFormat("{0}{1}", BundleRoot, "/Scene").ToString(), sb1.AppendFormat("{0}{1}", sceneName, ".bundle").ToString());
		WWW www = new WWW(loadPath);
		yield return www;
		bool isSuccess = false;
		AssetBundle assetBundle = null;
		if (string.IsNullOrEmpty(www.error))
		{
			if (mCacheSceneList.Count >= GameSettingData.MaxSceneCache[GameSettingData.GetPhoneClass()])
			{
				UnloadBundle(mSceneBundleCacheDic[mCacheSceneList[0]], flag: true);
				mSceneBundleCacheDic.Remove(mCacheSceneList[0]);
				mCacheSceneList.RemoveAt(0);
			}
			mCommonSceneBundle.LoadAll();
			assetBundle = www.assetBundle;
			assetBundle.LoadAll();
			mCacheSceneList.Add(sceneName);
			mSceneBundleCacheDic.Add(sceneName, assetBundle);
			isSuccess = true;
		}
		else
		{
			Debug.Log(www.error);
		}
		onLoadFinish?.Invoke(isSuccess, sceneName, assetBundle);
	}

	public static IEnumerator LoadSceneActivityObj(string ActObjName, SceneComponentData data, OnLoadActObjFinish onLoadFinish)
	{
		if (mCommonActObjBundle == null)
		{
			sb.Length = 0;
			sb1.Length = 0;
			string commonLoadPath = GetLocalUrl(CommonRootPath, "CommonobjPrefab.bundle");
			WWW commonWWW = new WWW(commonLoadPath);
			yield return commonWWW;
			mCommonActObjBundle = commonWWW.assetBundle;
		}
		if (mActivityBundleCacheDic.ContainsKey(ActObjName))
		{
			mCacheActObjList.Remove(ActObjName);
			mCacheActObjList.Add(ActObjName);
			mCommonActObjBundle.LoadAll();
			mActivityBundleCacheDic[ActObjName].LoadAll();
			onLoadFinish?.Invoke(ActObjName, data, mActivityBundleCacheDic[ActObjName].mainAsset);
			yield break;
		}
		sb.Length = 0;
		sb1.Length = 0;
		string loadPath = GetLocalUrl(ActObjRootPath, ActObjName + ".bundle");
		WWW www = new WWW(loadPath);
		yield return www;
		AssetBundle assetBundle2 = null;
		if (string.IsNullOrEmpty(www.error))
		{
			mCommonActObjBundle.LoadAll();
			assetBundle2 = www.assetBundle;
			assetBundle2.LoadAll();
			mCacheActObjList.Add(ActObjName);
			mActivityBundleCacheDic.Add(ActObjName, assetBundle2);
			onLoadFinish?.Invoke(ActObjName, data, assetBundle2.mainAsset);
		}
		else
		{
			Debug.Log(www.error);
		}
	}

	public static void UnloadCacheActObj()
	{
		for (int num = mCacheActObjList.Count - 1; num >= 0; num--)
		{
			UnloadBundle(mActivityBundleCacheDic[mCacheActObjList[num]], flag: true);
			mActivityBundleCacheDic.Remove(mCacheActObjList[num]);
			mCacheActObjList.RemoveAt(num);
		}
		if (UnloadBundle(mCommonActObjBundle, flag: true))
		{
			mCommonActObjBundle = null;
		}
	}

	public static IEnumerator LoadSceneAnima(string sceneAnimaName, OnLoadSceneFinish onLoadFinish)
	{
		if (mCommonSceneBundle == null)
		{
			sb.Length = 0;
			sb1.Length = 0;
			string commonLoadPath = GetLocalUrl(sb.AppendFormat("{0}{1}", BundleRoot, "/Scene").ToString(), sb1.AppendFormat("CommonPrefab{0}", ".bundle").ToString());
			WWW commonWWW = new WWW(commonLoadPath);
			yield return commonWWW;
			mCommonSceneBundle = commonWWW.assetBundle;
		}
		sb.Length = 0;
		sb1.Length = 0;
		string loadPath = GetLocalUrl(sb.AppendFormat("{0}{1}", BundleRoot, "/StartSceneAnima").ToString(), sb1.AppendFormat("{0}{1}", sceneAnimaName, ".bundle").ToString());
		WWW www = new WWW(loadPath);
		yield return www;
		bool isSuccess2 = false;
		AssetBundle assetBundle2 = null;
		if (string.IsNullOrEmpty(www.error))
		{
			mCommonSceneBundle.LoadAll();
			assetBundle2 = www.assetBundle;
			assetBundle2.LoadAll();
			isSuccess2 = true;
			onLoadFinish?.Invoke(isSuccess2, sceneAnimaName, assetBundle2);
			assetBundle2.Unload(unloadAllLoadedObjects: false);
		}
		else
		{
			onLoadFinish?.Invoke(isSuccess2, sceneAnimaName, assetBundle2);
		}
	}

	public static LoadModelData LoadEffectInList(string effectName, bool isNeedUnload, bool isDoNotCache = false, OnLoadModelFinish onFinished = null, object paramData1 = null, object paramData2 = null)
	{
		LoadModelData loadModelData = new LoadModelData(effectName, isNeedUnload, LoadModelData.BundleType.EFFECT, isDoNotCache, onFinished, paramData1, paramData2);
		if (mModelBundleCacheDic.ContainsKey(loadModelData.ModelName) && mModelBundleCacheDic[loadModelData.ModelName].IsBundleValid())
		{
			if (onFinished != null)
			{
				mModelBundleCacheDic[loadModelData.ModelName].AddBundleUsingCount();
				UnityEngine.Object objBundle = UnityEngine.Object.Instantiate(mModelBundleCacheDic[loadModelData.ModelName].GetBundle().mainAsset);
				onFinished(objBundle, paramData1, paramData2);
			}
			return null;
		}
		if (!string.IsNullOrEmpty(loadModelData.SubPath) && !mModelBundleCacheDic.ContainsKey(loadModelData.SubPath))
		{
			mModelBundleCacheDic.Add(loadModelData.SubPath, new AssetBundleData(null, 0, loadModelData.SubPath, string.Empty, isDepend: true));
		}
		if (!mModelBundleCacheDic.ContainsKey(loadModelData.ModelName))
		{
			mModelBundleCacheDic.Add(loadModelData.ModelName, new AssetBundleData(null, 0, loadModelData.ModelName, loadModelData.SubPath, isDepend: false));
		}
		mModelBundleCacheDic[loadModelData.ModelName].AddBundleUsingCount();
		mLoadModelList.Add(loadModelData);
		return loadModelData;
	}

	private static IEnumerator LoadEffectCommonFile()
	{
		effectCommonFileLoadingFlag = true;
		WWW wwwEffectCommonShader = new WWW(GetLocalUrl(BundleEffectRootPath, "EffectCommonShader.bundle"));
		yield return wwwEffectCommonShader;
		if (wwwEffectCommonShader.assetBundle != null)
		{
			wwwEffectCommonShader.assetBundle.LoadAll();
			mEffectCommonShaderBundle = wwwEffectCommonShader.assetBundle;
		}
		else
		{
			Debug.Log("no shader LoadModelFromList");
		}
		WWW wwwCommonPic = new WWW(GetLocalUrl(BundleEffectRootPath, "CommonPic.bundle"));
		yield return wwwCommonPic;
		if (wwwCommonPic.assetBundle != null)
		{
			wwwCommonPic.assetBundle.LoadAll();
			mEffectCommonPicBundle = wwwCommonPic.assetBundle;
		}
		else
		{
			Debug.Log("no shader LoadModelFromList");
		}
	}

	public static IEnumerator LoadDownloadAnimationBundle(OnLoadAnimationFinishedDelegate finishfun)
	{
		if (DownloadAnimationFlag)
		{
			yield break;
		}
		DownloadAnimationFlag = true;
		for (int i = 0; i < AnimationFileNameList.Count; i++)
		{
			if (mAnimationBundleDic.ContainsKey(AnimationFileNameList[i]) && UnloadBundle(mAnimationBundleDic[AnimationFileNameList[i]], flag: false))
			{
				mAnimationBundleDic.Remove(AnimationFileNameList[i]);
			}
			if (mAnimationBundleDic.ContainsKey(AnimationFileNameList[i]))
			{
				continue;
			}
			WWW wwwAnima = new WWW(GetLocalUrl(AnimationRootPath, AnimationFileNameList[i] + ".Bundle"));
			yield return wwwAnima;
			if (string.IsNullOrEmpty(wwwAnima.error))
			{
				if (wwwAnima.assetBundle != null)
				{
					mAnimationBundleDic.Add(AnimationFileNameList[i], wwwAnima.assetBundle);
				}
				else
				{
					Debug.Log("no animation bundle" + AnimationFileNameList[i]);
				}
			}
			else
			{
				Debug.Log(wwwAnima.error);
			}
		}
		finishfun?.Invoke();
		DownloadAnimationFlag = false;
	}

	public static IEnumerator LoadStreamAnimationBundle(OnLoadAnimationFinishedDelegate finishfun)
	{
		if (StreamAnimationFlag)
		{
			yield break;
		}
		StreamAnimationFlag = true;
		for (int i = 0; i < AnimationStreamFileNameList.Count; i++)
		{
			if (mAnimationBundleDic.ContainsKey(AnimationStreamFileNameList[i]) && UnloadBundle(mAnimationBundleDic[AnimationStreamFileNameList[i]], flag: false))
			{
				mAnimationBundleDic.Remove(AnimationStreamFileNameList[i]);
			}
			if (mAnimationBundleDic.ContainsKey(AnimationStreamFileNameList[i]))
			{
				continue;
			}
			WWW wwwAnima = new WWW(GetLocalUrl(AnimationRootPath, AnimationStreamFileNameList[i] + ".bundle"));
			yield return wwwAnima;
			if (string.IsNullOrEmpty(wwwAnima.error))
			{
				if (wwwAnima.assetBundle != null)
				{
					mAnimationBundleDic.Add(AnimationStreamFileNameList[i], wwwAnima.assetBundle);
				}
				else
				{
					Debug.Log("no animation bundle" + AnimationStreamFileNameList[i]);
				}
			}
			else
			{
				Debug.Log(wwwAnima.error);
			}
		}
		finishfun?.Invoke();
		StreamAnimationFlag = false;
	}

	public static UnityEngine.Object LoadAnimation(string BundleName, string AnimationName)
	{
		if (mAnimationBundleDic.ContainsKey(BundleName) && mAnimationBundleDic[BundleName] != null)
		{
			UnityEngine.Object @object = null;
			return mAnimationBundleDic[BundleName].Load(AnimationName, typeof(UnityEngine.Object));
		}
		return null;
	}

	public static IEnumerator LoadData(OnLoadDataFinishedDelegate onFinished)
	{
		mWaittingLoadDataFlag = false;
		if (mLoadingDataBundleFlag)
		{
			mWaittingLoadDataFlag = true;
			yield break;
		}
		mLoadingDataBundleFlag = true;
		WWW wwwData = new WWW(GetDataLocalUrl(DataRootPath, DataFileName));
		yield return wwwData;
		if (mWaittingLoadDataFlag)
		{
			mWaittingLoadDataFlag = false;
		}
		mLoadingDataBundleFlag = false;
		if (wwwData.assetBundle != null)
		{
			mDataBundle = wwwData.assetBundle;
			onFinished?.Invoke();
		}
		else
		{
			Debug.Log("no shader LoadModelFromList");
		}
	}

	public static object LoadTable(string fileName)
	{
		if (mDataBundle == null)
		{
			Debug.Log("mDataBundle == null :: " + fileName);
			return null;
		}
		return mDataBundle.Load(fileName, typeof(object));
	}

	public static void UnloadDataBundle()
	{
		if (UnloadBundle(mDataBundle, flag: true))
		{
			mDataBundle = null;
		}
	}

	public static IEnumerator LoadSound(string soundPath, string name, LoadSoundFinish delFinish, object param1 = null, object param2 = null, object param3 = null)
	{
		WWW wwwSound = new WWW(GetLocalUrl(SoundRootPath + soundPath, name + ".bundle"));
		yield return wwwSound;
		AudioClip retObj2 = null;
		if (string.IsNullOrEmpty(wwwSound.error))
		{
			if (wwwSound.assetBundle != null)
			{
				retObj2 = wwwSound.assetBundle.mainAsset as AudioClip;
				delFinish?.Invoke(soundPath + name, retObj2, param1, param2, param3);
				UnloadBundle(wwwSound.assetBundle, flag: false);
			}
			else
			{
				delFinish?.Invoke(soundPath + name, null, param1, param2, param3);
			}
		}
		else
		{
			Debug.Log(wwwSound.error);
		}
	}

	public static IEnumerator LoadTexture(string name, LoadTextureFinish delFinish)
	{
		if (mTextureDic.ContainsKey(name))
		{
			mTextureDic[name].useTimes++;
			mTextureDic[name].m_LastActiveTime = Time.realtimeSinceStartup;
			delFinish?.Invoke(name, mTextureDic[name].mTexture);
		}
		else
		{
			if (mCurLoadingTextureList.Contains(name))
			{
				yield break;
			}
			mCurLoadingTextureList.Add(name);
			WWW wwwTexture = new WWW(GetLocalUrl(TextureRootPath, name + ".bundle"));
			yield return wwwTexture;
			mCurLoadingTextureList.Remove(name);
			Texture retObj = null;
			if (string.IsNullOrEmpty(wwwTexture.error))
			{
				if (wwwTexture.assetBundle != null)
				{
					retObj = wwwTexture.assetBundle.mainAsset as Texture;
					TextureInfo newtexture = new TextureInfo(name, retObj, 1, Time.realtimeSinceStartup);
					mTextureDic.Add(name, newtexture);
					UnloadBundle(wwwTexture.assetBundle, flag: false);
					CheckTextureCache();
				}
				else
				{
					Debug.Log("bundle no texture! name:" + name);
				}
			}
			else
			{
				Debug.Log(wwwTexture.error);
			}
			delFinish?.Invoke(name, retObj);
		}
	}

	public static IEnumerator LoadTexture(List<string> namelist, LoadTextureDicFinish delFun)
	{
		Dictionary<string, Texture> retDic = new Dictionary<string, Texture>();
		for (int i = 0; i < namelist.Count; i++)
		{
			if (mTextureDic.ContainsKey(namelist[i]))
			{
				mTextureDic[namelist[i]].useTimes++;
				mTextureDic[namelist[i]].m_LastActiveTime = Time.realtimeSinceStartup;
				retDic.Add(namelist[i], mTextureDic[namelist[i]].mTexture);
				continue;
			}
			WWW wwwTexture = new WWW(GetLocalUrl(TextureRootPath, namelist[i] + ".bundle"));
			yield return wwwTexture;
			Texture retObj2 = null;
			if (string.IsNullOrEmpty(wwwTexture.error))
			{
				if (wwwTexture.assetBundle != null)
				{
					retObj2 = wwwTexture.assetBundle.mainAsset as Texture;
					TextureInfo newtexture = new TextureInfo(namelist[i], retObj2, 1, Time.realtimeSinceStartup);
					mTextureDic.Add(namelist[i], newtexture);
					retDic.Add(namelist[i], retObj2);
					UnloadBundle(wwwTexture.assetBundle, flag: false);
					CheckTextureCache();
				}
				else
				{
					Debug.Log("bundle no texture! name:" + namelist[i]);
				}
			}
			else
			{
				Debug.Log(wwwTexture.error);
			}
		}
		delFun?.Invoke(retDic);
	}

	public static IEnumerator LoadWaitTexture(List<string> namelist, LoadTextureFinish delFun)
	{
		for (int i = 0; i < namelist.Count; i++)
		{
			LoadTextureData curtexturedata = new LoadTextureData(namelist[i], delFun);
			if (mTextureDic.ContainsKey(namelist[i]))
			{
				mTextureDic[namelist[i]].useTimes++;
				mTextureDic[namelist[i]].m_LastActiveTime = Time.realtimeSinceStartup;
				delFun?.Invoke(namelist[i], mTextureDic[namelist[i]].mTexture);
			}
			else if (!mLoadTextureBundle.Contains(namelist[i]))
			{
				mLoadTextureBundle.Add(namelist[i]);
				WWW wwwTexture = new WWW(GetLocalUrl(TextureRootPath, namelist[i] + ".bundle"));
				yield return wwwTexture;
				Texture retObj2 = null;
				if (string.IsNullOrEmpty(wwwTexture.error))
				{
					if (wwwTexture.assetBundle != null)
					{
						retObj2 = wwwTexture.assetBundle.mainAsset as Texture;
						TextureInfo newtexture = new TextureInfo(namelist[i], retObj2, 1, Time.realtimeSinceStartup);
						mTextureDic.Add(namelist[i], newtexture);
						if (curtexturedata.OnLoadFinished != null)
						{
							if (mWaitTextureDic.ContainsKey(namelist[i]))
							{
								for (int j = 0; j < mWaitTextureDic[namelist[i]].Count; j++)
								{
									LoadTextureFromCache(mWaitTextureDic[namelist[i]][j]);
								}
								mWaitTextureDic.Remove(namelist[i]);
							}
							curtexturedata.OnLoadFinished(namelist[i], retObj2);
						}
						else if (mWaitTextureDic.ContainsKey(namelist[i]))
						{
							for (int k = 0; k < mWaitTextureDic[namelist[i]].Count; k++)
							{
								LoadTextureFromCache(mWaitTextureDic[namelist[i]][k]);
							}
							mWaitTextureDic.Remove(namelist[i]);
						}
						UnloadBundle(wwwTexture.assetBundle, flag: false);
						CheckTextureCache();
						if (mLoadTextureBundle.Contains(namelist[i]))
						{
							mLoadTextureBundle.Remove(namelist[i]);
						}
					}
					else
					{
						Debug.Log("bundle no texture! name:" + namelist[i]);
					}
				}
				else
				{
					Debug.Log(wwwTexture.error);
				}
			}
			else
			{
				if (!mWaitTextureDic.ContainsKey(namelist[i]))
				{
					mWaitTextureDic.Add(namelist[i], new List<LoadTextureData>());
				}
				mWaitTextureDic[namelist[i]].Add(curtexturedata);
			}
		}
	}

	private static void LoadTextureFromCache(LoadTextureData infodata)
	{
		if (mTextureDic.ContainsKey(infodata.TextureName))
		{
			mTextureDic[infodata.TextureName].useTimes++;
			mTextureDic[infodata.TextureName].m_LastActiveTime = Time.realtimeSinceStartup;
			if (infodata.OnLoadFinished != null)
			{
				infodata.OnLoadFinished(infodata.TextureName, mTextureDic[infodata.TextureName].mTexture);
			}
		}
	}

	private static void CheckTextureCache()
	{
		if (mTextureDic.Count < MaxTextureNum)
		{
			return;
		}
		float num = 100000000f;
		string text = string.Empty;
		foreach (TextureInfo value in mTextureDic.Values)
		{
			if (num > value.m_LastActiveTime)
			{
				text = value.name;
				num = value.m_LastActiveTime;
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			mTextureDic.Remove(text);
		}
	}

	public static void ClearCacheTexture()
	{
		mCurLoadingTextureList.Clear();
		mTextureDic.Clear();
		mWaitTextureDic.Clear();
		mLoadTextureBundle.Clear();
	}

	public static void UnloadTexture(string name)
	{
		if (mTextureDic.ContainsKey(name))
		{
			mTextureDic.Remove(name);
		}
	}

	public static void UnloadTexture(List<string> names)
	{
		for (int i = 0; i < names.Count; i++)
		{
			UnloadTexture(names[i]);
		}
	}

	public static void StartLoadingUItexture(MonoBehaviour mono, string name)
	{
		if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
		{
			mono.StartCoroutine(LoadLoadingTexture(name));
		}
	}

	public static IEnumerator LoadLoadingTexture(string name)
	{
		if (NextLoadingTexture != null && NextLoadingTexture.name.Equals(name))
		{
			yield break;
		}
		string check_path = GetLocalCheckPath(TextureRootPath, name + ".bundle");
		if (File.Exists(check_path))
		{
			string urlstr = GetLocalUrl(TextureRootPath, name + ".bundle");
			WWW wwwTexture = new WWW(urlstr);
			yield return wwwTexture;
			Texture retObj2 = null;
			if (string.IsNullOrEmpty(wwwTexture.error))
			{
				if (wwwTexture.assetBundle != null)
				{
					retObj2 = wwwTexture.assetBundle.mainAsset as Texture;
					NextLoadingTexture = retObj2;
					UnloadBundle(wwwTexture.assetBundle, flag: false);
				}
				else
				{
					Debug.Log("bundle no texture! name:" + name);
				}
			}
			else
			{
				Debug.Log(wwwTexture.error);
			}
		}
		else
		{
			Debug.Log("bundle no exit texture! name:" + name);
		}
	}

	public static Texture GetNextLoadingTexture()
	{
		return NextLoadingTexture;
	}

	public static IEnumerator LoadItem(string name, LoaditemsFinish delFinish, object param1 = null, object param2 = null)
	{
		WWW wwwTexture = new WWW(GetLocalUrl(ItemsRootPath, name + ".bundle"));
		yield return wwwTexture;
		UnityEngine.Object retObj2 = null;
		if (string.IsNullOrEmpty(wwwTexture.error))
		{
			if (wwwTexture.assetBundle != null)
			{
				retObj2 = wwwTexture.assetBundle.mainAsset;
				delFinish?.Invoke(name, retObj2, param1, param2);
				UnloadBundle(wwwTexture.assetBundle, flag: false);
			}
			else
			{
				delFinish?.Invoke(name, null, param1, param2);
			}
		}
		else
		{
			Debug.Log(wwwTexture.error);
		}
	}

	public static void PrintCurCacheDic()
	{
		Debug.Log("================================");
		List<AssetBundleData> list = new List<AssetBundleData>(mModelBundleCacheDic.Values);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].IsBundleValid())
			{
				if (list[i].Bundle.mainAsset != null)
				{
					Debug.Log("name :: " + list[i].Bundle.mainAsset.name + " :: count :: " + list[i].UsingCount);
				}
				else
				{
					Debug.Log(list[i].SelfURL + " :: " + list[i].UsingCount);
				}
			}
			else
			{
				Debug.Log("Bundle Not Valid :: " + list[i].SelfURL + " :: " + list[i].UsingCount);
			}
		}
		Debug.Log("==============end===============");
	}
}
