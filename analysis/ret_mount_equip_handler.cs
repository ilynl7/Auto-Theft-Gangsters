using Sproto;
using SprotoType;

public class ret_mount_equip_handler
{
	public static SprotoTypeBase ret_mount_equip_request(SprotoTypeBase req)
	{
		if (req is ret_mount_equip.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (SingletonUnity<PlayerCarRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PlayerCarRootLogic>.Instance.gameObject))
			{
				SingletonUnity<PlayerCarRootLogic>.Instance.UpdateCarPage(request);
				if (TutorialManager.CurStep == TUTORIAL_STEP.CAR_CLICK_EQUIP)
				{
					SingletonUnity<PlayerCarRootLogic>.Instance.CheckTutorialEvent();
				}
			}
			SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			if (sceneManager.CurrentMapInofData.MapType == MAPTYPE.TUTORIAL_CAR)
			{
				PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
				if (request.mount_info.ContainsKey(request.ID) && request.mount_info[request.ID].state == 2)
				{
					playerData.MountId = request.ID;
				}
				if (SingletonUnity<CitySimController>.Exists)
				{
					SingletonUnity<CitySimController>.Instance.UpdateSpecialCarState();
				}
			}
		}
		return null;
	}
}
