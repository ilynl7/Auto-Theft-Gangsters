using Sproto;
using SprotoType;

public class ret_title_req_level_up_handler
{
	public static SprotoTypeBase ret_title_req_level_up_request(SprotoTypeBase req)
	{
		if (req is ret_title_req_level_up.request { HasTitle_exp: not false, HasTitle_level: not false } request && Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.AttributeData.CurTitleExp = (int)request.title_exp;
			Singleton<ObjManager>.Instance.MainPlayer.AttributeData.CurTitleLevel = (int)request.title_level;
			if (SingletonUnity<JSShengWangLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<JSShengWangLogic>.Instance.gameObject))
			{
				WaitResponseUIRootLogic.CloseBox();
				SingletonUnity<JSShengWangLogic>.Instance.Reset();
			}
			PlayerHeadInfoLogic playerHeadInfoLogic = Singleton<ObjManager>.Instance.MainPlayer.HeadInfoLogic as PlayerHeadInfoLogic;
			playerHeadInfoLogic.ChangePic(Singleton<ObjManager>.Instance.MainPlayer.AttributeData.CurTitleLevel);
			if (SingletonUnity<FunctionBtnRootLogic>.Exists)
			{
				SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateTitleTips();
			}
			if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.RefershTips();
			}
			NoticeLogic.AddNotifyData("#{101719}");
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Achieve", "Title", $"title_{request.title_level}");
		}
		return null;
	}
}
