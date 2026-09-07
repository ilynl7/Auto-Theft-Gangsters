using Sproto;
using SprotoType;

public class random_select_ok_handler
{
	public static SprotoTypeBase random_select_ok_request(SprotoTypeBase req)
	{
		if (req is random_select_ok.request && (!SingletonUnity<TeamUIRootNewLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<TeamUIRootNewLogic>.Instance.gameObject)) && !UIManager.IsUnlockTutorialEnable() && SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamRootNew);
		}
		return null;
	}
}
