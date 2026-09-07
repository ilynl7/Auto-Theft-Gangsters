public class GuildStrengthenRootLogic : SingletonUnity<GuildStrengthenRootLogic>
{
	public UISprite SkillBtn;

	public UISprite StarBtn;

	private int CurPage = -1;

	public void Reset()
	{
		CurPage = -1;
		OnClickSkillBtn();
	}

	public void OnClickSkillBtn()
	{
		if (CurPage != 1)
		{
			CurPage = 1;
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildStarRoot);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildSkillRootLogic, delegate
			{
				Singleton<ObjManager>.Instance.MainPlayer.ReqGuildSkill();
				WaitResponseUIRootLogic.OpenWaitBox(212, 10f, 0f);
			});
			SetCurSelect();
		}
	}

	public void OnClickStarBtn()
	{
		if (CurPage != 2)
		{
			CurPage = 2;
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildSkillRootLogic);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildStarRoot, delegate
			{
				SingletonUnity<GuildStarRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(293, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.req_guild_star>();
			});
			SetCurSelect();
		}
	}

	public void SetCurSelect()
	{
		if (CurPage == 1)
		{
			SkillBtn.spriteName = "CZ_huaDongBG_1";
			StarBtn.spriteName = "CZ_huaDongBG";
		}
		else if (CurPage == 2)
		{
			SkillBtn.spriteName = "CZ_huaDongBG";
			StarBtn.spriteName = "CZ_huaDongBG_1";
		}
	}
}
