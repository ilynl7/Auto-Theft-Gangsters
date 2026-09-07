using System.Collections;
using UnityEngine;

public class LoadingUIRoot : SingletonUnity<LoadingUIRoot>
{
	public UILabel TipsLabel;

	public UISprite ProgressLine;

	public UISprite BottomLine;

	private float mCurProgress;

	private float mTargetProgress;

	public UISprite BlackTopPic;

	public GameObject PicLoadingRoot;

	public static int TipsIndex = -1;

	public UITexture LoadTexture;

	private bool mMapReadyFlag;

	public GameObject ProressLineAnimaObj;

	private void OnEnable()
	{
		Texture nextLoadingTexture = BundleManager.GetNextLoadingTexture();
		if (nextLoadingTexture != null && !LoadTexture.mainTexture.name.Equals(nextLoadingTexture.name))
		{
			LoadTexture.mainTexture = nextLoadingTexture;
		}
	}

	private void SetProgressLineWidth(int width)
	{
		ProgressLine.width = width;
		ProressLineAnimaObj.transform.localPosition = new Vector3(width, 0f, 0f);
	}

	private void Start()
	{
		if (LoadingWindow.BlackLoading)
		{
			UnityVersionUtil.SetActiveRecursive(BlackTopPic.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(PicLoadingRoot.gameObject, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(BlackTopPic.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(PicLoadingRoot.gameObject, state: true);
		}
		ProgressLine.transform.localPosition = new Vector3(-BottomLine.width / 2, 0f, 0f);
		mTargetProgress = 1f;
		mCurProgress = LoadingWindow.LoadingProgress;
		SetTipStr();
		mMapReadyFlag = false;
	}

	private void Update()
	{
		if (!(mCurProgress <= mTargetProgress))
		{
			return;
		}
		mCurProgress += LoadingWindow.LoadingSpeed;
		if (!GameManager.IsSceneReady)
		{
			if (mCurProgress > 0.9f)
			{
				mCurProgress = 0.9f;
			}
		}
		else if (mCurProgress > 0.9f && !mMapReadyFlag && SingletonUnity<MyEvent>.Exists)
		{
			mMapReadyFlag = true;
			if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(OnLoadingOver());
			}
		}
		if (LoadingWindow.BlackLoading)
		{
			BlackTopPic.alpha = 1f - mCurProgress;
		}
		else
		{
			SetProgressLineWidth((int)((float)BottomLine.width * mCurProgress));
		}
		if (mCurProgress >= 1f)
		{
			TipsIndex = -1;
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LoadingUIRoot);
			if (LoadingWindow.BlackLoading)
			{
				LoadingWindow.BlackLoading = false;
			}
			SingletonDontDestoryUnity<GameManager>.Instance.LoadNextLoadingTexture();
		}
	}

	private IEnumerator OnLoadingOver()
	{
		yield return null;
		SingletonUnity<MyEvent>.Instance.Fire("OnLoadingOver");
	}

	public void SetProgress(float progress)
	{
		mTargetProgress = progress;
	}

	public void SetTipStr()
	{
		if (!GameSettingData.IsLowPhone)
		{
			if (TipsIndex == -1)
			{
				TipsIndex = Random.Range(0, GameDefine.LoadingTips.Length);
			}
			string keystr = GameDefine.LoadingTips[TipsIndex];
			TipsLabel.text = StrDictionary.GetDictionaryString(keystr);
		}
	}
}
