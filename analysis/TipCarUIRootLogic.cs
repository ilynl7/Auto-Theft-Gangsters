using UnityEngine;

public class TipCarUIRootLogic : SingletonUnity<TipCarUIRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UITexture CarModelPic;

	public UILabel TextLabel;

	private Color ambientLight;

	public UIWidget YesBtn;

	private string curMountId = string.Empty;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	private void ResetFakeCarObjRoot()
	{
		FakeCarObjRootLogic instance = SingletonUnity<FakeCarObjRootLogic>.Instance;
		if (instance == null)
		{
			ResourcesManager.LoadAndInstantiate("UIRoot/FakeCarObjRoot");
			instance = SingletonUnity<FakeCarObjRootLogic>.Instance;
		}
		SingletonUnity<FakeCarObjRootLogic>.Instance.EnableFakeObjRoot();
		CarModelPic.mainTexture = instance.ModelPic;
	}

	private void ResetCarModelVisual(MountData mountData, ColorData colorda)
	{
		SingletonUnity<FakeCarObjRootLogic>.Instance.InitFakeCarObj(mountData, colorda);
	}

	public void Reset(string mountId, string messageid = "#{101425}")
	{
		ResetFakeCarObjRoot();
		MountData mountDataById = DataManager.GetMountDataById(mountId);
		ResetCarModelVisual(mountDataById, DataManager.GetColorDataById(mountDataById.DefaultColorId));
		TextLabel.text = StrDictionary.GetDictionaryString(messageid, mountDataById.MCarName);
		curMountId = mountId;
	}

	public void OnClickCloseBtn()
	{
		ClosePage();
		if (TutorialManager.CurStep == TUTORIAL_STEP.CAR_SHOW_NEXT)
		{
			mOnClickTutorialBtn = null;
			TutorialManager.CloseTutorial();
		}
	}

	private void ClosePage()
	{
		if ((!SingletonUnity<PlayerCarRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<PlayerCarRootLogic>.Instance.gameObject)) && SingletonUnity<FakeCarObjRootLogic>.Exists && SingletonUnity<FakeCarObjRootLogic>.Instance != null)
		{
			SingletonUnity<FakeCarObjRootLogic>.Instance.DisableFakeObjRoot();
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TipCarUIRoot);
	}

	public void OnClickYesBtn()
	{
		ClosePage();
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.IsGetSignWeekCar())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerCarRoot, delegate
			{
				SingletonUnity<PlayerCarRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(235, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_mount_info>();
			});
		}
	}
}
