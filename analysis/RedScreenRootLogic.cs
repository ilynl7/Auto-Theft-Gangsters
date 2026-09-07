using UnityEngine;

public class RedScreenRootLogic : SingletonUnity<RedScreenRootLogic>
{
	private static float startTime;

	private ObjMainPlayer mMainPlayer;

	public static void EnableRedScreen()
	{
		if (!SingletonUnity<RedScreenRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<RedScreenRootLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.RedScreenRoot, delegate
			{
				SingletonUnity<RedScreenRootLogic>.Instance.Reset();
			});
		}
		if (GameManager.IsSupportCurDataVersion() && TutorialManager.IsTutorialCanShow() && SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsTutorialCanShow(FUNCTION_TYPE.LOW_HP_USE_DRUG_TIP))
		{
			ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			if (!mainPlayer.IsOpenAutoCombat && Time.time > startTime)
			{
				TutorialManager.ShowTutorial(TUTORIAL_STEP.DRUG_USE_TIP_START);
				startTime = Time.time + 5f;
			}
		}
	}

	public static void DisableRedScreen()
	{
		if (SingletonUnity<RedScreenRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<RedScreenRootLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RedScreenRoot);
		}
	}

	public void Reset()
	{
		if (mMainPlayer == null)
		{
			mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (mMainPlayer == null || mMainPlayer.IsDie)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RedScreenRoot);
		}
	}
}
