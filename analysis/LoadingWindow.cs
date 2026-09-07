using System;
using System.Collections;
using UnityEngine;

public class LoadingWindow : MonoBehaviour
{
	public static float LoadingProgress = 0f;

	public static float LoadingSpeed = 0.02f;

	public static bool BlackLoading = false;

	private static string nextSceneName = string.Empty;

	public static string preSceneId = string.Empty;

	public static bool isSendMapReady = false;

	private AsyncOperation mLoadAsync;

	private float mLoadProgress;

	private float mUILoadProgress;

	public static void LoadScene(int sceneDefine)
	{
		isSendMapReady = false;
		if (SingletonUnity<UIManager>.Exists)
		{
			SingletonUnity<UIManager>.Instance.UICheckChangeScene();
		}
		NetLogic.GetInstance().FinishReconnecting();
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager != null)
		{
			preSceneId = SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr;
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CloseActivityObj();
			MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(preSceneId);
			if (mapInfoDataByID.MapType == MAPTYPE.TUTORIAL_CAR && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager is NewTutorialSceneManager @object)
			{
				UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(@object.CheckLockArea));
			}
		}
		nextSceneName = DataManager.GetMapInfoDataByID(sceneDefine.ToString()).SceneName;
		if (sceneDefine == 11 && !GameManager.IsSupportCurDataVersion200())
		{
			nextSceneName = "DSJ_GTA";
		}
		SingletonDontDestoryUnity<GameManager>.Instance.RunningMapId = sceneDefine;
		ChangeSceneClearAction();
		BundleManager.UnloadCacheActObj();
		BundleManager.IsCanUnloadBundle = false;
		Application.LoadLevel("LoadingScene");
	}

	public static void ChangeSceneClearAction()
	{
		vp_Timer.DestroyAll();
		UnloadResource();
		Singleton<ObjManager>.Instance.ClearAll();
	}

	private static void UnloadResource()
	{
		EffectLogic.Clear();
	}

	private void Start()
	{
		BundleManager.IsCanUnloadBundle = true;
		mLoadProgress = 0f;
		mUILoadProgress = 0f;
		BundleManager.UnloadCacheActObj();
		BundleManager.ClearCacheTexture();
		if (SingletonDontDestoryUnity<GameManager>.Instance.RunningMapId == 0)
		{
			BundleManager.ClearCacheModelBundle(isClearAll: true);
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.ResetPlayerData();
			instance.PlayerCommonData.ClearData();
			instance.ClearServerRefresh();
			DataManager.initFlag = false;
			AnimationManager.initDownloadFlag = false;
			SingletonDontDestoryUnity<NetManager>.Instance.ClearLastCheckNeedQuitUpdateVersion();
			mLoadAsync = Application.LoadLevelAsync(nextSceneName);
		}
		else
		{
			BundleManager.ClearCacheModelBundle();
			MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr);
			if (mapInfoDataByID.DirectLoad == 1)
			{
				mLoadAsync = Application.LoadLevelAsync(nextSceneName);
			}
			else if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(LoadScene(nextSceneName));
			}
		}
	}

	private void Update()
	{
		if (mLoadAsync != null)
		{
			mLoadProgress = mLoadAsync.progress;
		}
		if (mUILoadProgress < mLoadProgress)
		{
			mUILoadProgress += LoadingSpeed;
			LoadingProgress = mUILoadProgress;
		}
		if (SingletonUnity<LoadingUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<LoadingUIRoot>.Instance.gameObject))
		{
			SingletonUnity<LoadingUIRoot>.Instance.SetProgress(mUILoadProgress);
		}
	}

	private IEnumerator LoadScene(string nextScene)
	{
		yield return StartCoroutine(BundleManager.LoadScene(nextScene, OnLoadSceneFinish));
	}

	private void OnLoadSceneFinish(bool isSuccess, string sceneName, AssetBundle sceneBundle)
	{
		if (!isSuccess)
		{
			LoadScene(preSceneId);
		}
		else
		{
			mLoadAsync = Application.LoadLevelAsync(sceneName);
		}
	}
}
