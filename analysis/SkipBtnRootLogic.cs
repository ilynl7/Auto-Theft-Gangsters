public class SkipBtnRootLogic : SingletonUnity<SkipBtnRootLogic>
{
	public void OnClickSkipBtn()
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		switch (sceneManager.CurrentMapInofData.MapType)
		{
		case MAPTYPE.TUTORIAL_CAR:
			(sceneManager as NewTutorialSceneManager).curAnimaCtl.SkipAnima();
			break;
		case MAPTYPE.SINGLE_RUN_POINT_COPY:
			(sceneManager as SingleRunPointSceneManager).CurAnimaCtl.SkipAnima();
			break;
		case MAPTYPE.CASH_DAILY_COPY:
		case MAPTYPE.EXP_DAILY_COPY:
			break;
		}
	}
}
