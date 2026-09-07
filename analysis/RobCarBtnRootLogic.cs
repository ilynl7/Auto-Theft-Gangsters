public class RobCarBtnRootLogic : SingletonUnity<RobCarBtnRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UITexture RobCarBtn;

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

	public void Reset(ObjFakeAICar mCurNearestCar)
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsTutorialCanShow(FUNCTION_TYPE.ROB_CAR_TIP))
		{
			TutorialManager.ShowTutorial(TUTORIAL_STEP.ROB_CAR_START);
			return;
		}
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager != null && sceneManager.CheckCarIsMissionCar(mCurNearestCar))
		{
			OnClickRobCarBtn();
		}
	}

	public void OnClickRobCarBtn()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.ROB_CAR_START)
		{
			CheckTutorialEvent();
		}
		SingletonUnity<CitySimController>.Instance.RobCar();
	}
}
