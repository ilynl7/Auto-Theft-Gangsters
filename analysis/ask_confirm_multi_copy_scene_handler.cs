using Sproto;
using SprotoType;

public class ask_confirm_multi_copy_scene_handler
{
	private static long mCurCopySession;

	private static TUTORIAL_STEP preTutorialStep;

	public static SprotoTypeBase ask_confirm_multi_copy_scene_request(SprotoTypeBase req)
	{
		if (req is ask_confirm_multi_copy_scene.request request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			CopySceneData copySceneData = null;
			if (request.type1 == 1)
			{
				copySceneData = DataManager.GetCopySceneDataById(request.id);
			}
			mCurCopySession = request.session;
			preTutorialStep = TUTORIAL_STEP.INVALID;
			if (UIManager.IsUnlockTutorialEnable())
			{
				if (TutorialManager.CurStep != TUTORIAL_STEP.JOIN_TEAM_START && TutorialManager.CurStep != TUTORIAL_STEP.JOIN_TEAM_SHOW_1 && TutorialManager.CurStep != TUTORIAL_STEP.JOIN_TEAM_CLICK_URGE && TutorialManager.CurStep != TUTORIAL_STEP.JOIN_TEAM_FINISH)
				{
					OnClickCancelBtn();
					return null;
				}
				preTutorialStep = TutorialManager.CurStep;
				TutorialManager.CloseTutorial();
			}
			Team teamInfo = playerData.TeamInfo;
			teamInfo.IsCheckingEnterCopy = true;
			teamInfo.ClearMemberEnterCopyState();
			SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			if (sceneManager != null && !sceneManager.IsShowTeamScene() && copySceneData != null)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamPreparationRoot, delegate
				{
					SingletonUnity<TeamPreparationRootLogic>.Instance.Reset(mCurCopySession, refreshTime: true);
				});
			}
			if (SingletonUnity<TeamUIRootNewLogic>.Exists)
			{
				SingletonUnity<TeamUIRootNewLogic>.Instance.Reset();
			}
		}
		return null;
	}

	private static void OnClickOkBtn()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveTeam())
		{
			ret_ask_confirm_multi_copy_scene.request request = new ret_ask_confirm_multi_copy_scene.request();
			request.state = 1L;
			request.session = mCurCopySession;
			NetLogic.GetInstance().Send<Protocol.ret_ask_confirm_multi_copy_scene>(request);
		}
		else
		{
			ret_ask_confirm_multi_copy_scene.request request2 = new ret_ask_confirm_multi_copy_scene.request();
			request2.state = 0L;
			request2.session = mCurCopySession;
			NetLogic.GetInstance().Send<Protocol.ret_ask_confirm_multi_copy_scene>(request2);
			Team teamInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo;
			teamInfo.SelfMember.IsReadyEnterCopy = true;
			if (SingletonUnity<TeamUIRootNewLogic>.Exists)
			{
				SingletonUnity<TeamUIRootNewLogic>.Instance.Reset();
			}
		}
		if (preTutorialStep != TUTORIAL_STEP.INVALID)
		{
			TutorialManager.ShowTutorial(preTutorialStep);
		}
	}

	private static void OnClickCancelBtn()
	{
		ret_ask_confirm_multi_copy_scene.request request = new ret_ask_confirm_multi_copy_scene.request();
		request.state = 1L;
		request.session = mCurCopySession;
		NetLogic.GetInstance().Send<Protocol.ret_ask_confirm_multi_copy_scene>(request);
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveTeam())
		{
			Team teamInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo;
			teamInfo.SelfMember.IsReadyEnterCopy = false;
			teamInfo.IsCheckingEnterCopy = false;
			if (SingletonUnity<TeamUIRootNewLogic>.Exists)
			{
				SingletonUnity<TeamUIRootNewLogic>.Instance.Reset();
			}
		}
		if (preTutorialStep != TUTORIAL_STEP.INVALID)
		{
			TutorialManager.ShowTutorial(preTutorialStep);
		}
	}
}
