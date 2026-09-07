using Sproto;
using SprotoType;

public class ret_mount_info_handler
{
	public static SprotoTypeBase ret_mount_info_request(SprotoTypeBase req)
	{
		if (req is ret_mount_info.request request)
		{
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.playerMountData.UpdateMountInfo(request);
			if (SingletonUnity<WaitResponseUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<WaitResponseUIRootLogic>.Instance.gameObject))
			{
				WaitResponseUIRootLogic.CloseBox();
			}
			if (SingletonUnity<PlayerCarRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PlayerCarRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PlayerCarRootLogic>.Instance.Reset(request);
				if (TutorialManager.CurStep == TUTORIAL_STEP.CAR_START)
				{
					SingletonUnity<FunctionBtnRootLogic>.Instance.CheckTutorialEvent();
				}
			}
		}
		return null;
	}
}
