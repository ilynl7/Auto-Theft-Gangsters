using Sproto;
using SprotoType;

public class start_enter_game_handler
{
	public static SprotoTypeBase start_enter_game_request(SprotoTypeBase req)
	{
		if (req is start_enter_game.request request && request.state == 1)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (SingletonUnity<DownLoadResRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DownLoadResRootLogic>.Instance.gameObject))
			{
				SingletonUnity<DownLoadResRootLogic>.Instance.OnClickCloseBtn();
			}
			if (SingletonUnity<DownloadTipRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DownloadTipRootLogic>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DownLoadTipRoot);
			}
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload = true;
			if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager != null && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsCanUsePotion() && !Singleton<ObjManager>.Instance.MainPlayer.IsLocalDrivingCar)
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PotionObjRoot, delegate
				{
					SingletonUnity<PotionLogic>.Instance.Reset();
				});
			}
			if (SingletonUnity<FunctionBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionBtnRootLogic>.Instance.gameObject))
			{
				SingletonUnity<FunctionBtnRootLogic>.Instance.refershBtn();
				SingletonUnity<FunctionBtnRootLogic>.Instance.ResetRightBtn();
			}
		}
		return null;
	}
}
