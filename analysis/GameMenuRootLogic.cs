using UnityEngine;

public class GameMenuRootLogic : SingletonUnity<GameMenuRootLogic>
{
	public UIGrid VerticalGrid;

	public UIGrid HorizontalGrid;

	public UISprite CharacterBtnIcon;

	public UISprite BagBtnIcon;

	public UISprite SocialBtnIcon;

	public UISprite GuildBtnIcon;

	public UISprite UnKnowBtnIcon;

	public UISprite CarBtnIcon;

	public UISprite TitleBtnIcon;

	public UISprite EnhanceBtnIcon;

	public UISprite SkillBtnIcon;

	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

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

	public void Reset()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		NGUITools.SetActive(CarBtnIcon.gameObject, playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.CAR));
		NGUITools.SetActive(TitleBtnIcon.gameObject, playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.TITLE));
		NGUITools.SetActive(EnhanceBtnIcon.gameObject, playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ENHANCE));
		NGUITools.SetActive(SkillBtnIcon.gameObject, playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SKILL));
		NGUITools.SetActive(CharacterBtnIcon.gameObject, playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.CHARACTER));
		NGUITools.SetActive(BagBtnIcon.gameObject, playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.BAG));
		NGUITools.SetActive(SocialBtnIcon.gameObject, playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SOCIAL));
		NGUITools.SetActive(GuildBtnIcon.gameObject, playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GUILD));
		VerticalGrid.Reposition();
		HorizontalGrid.Reposition();
	}

	public void OnClickGangBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewGuildUIRootLogic);
	}

	public void OnClickSkillBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GameMenuSkillInfoRootUI);
	}

	public void OnClickCharacterBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate(bool isSuccess, object param)
		{
			if (isSuccess)
			{
				Debug.Log("!!!!!!!!!!!!!!!!!!!!!!! :: " + TutorialManager.CurStep);
				CheckTutorialEvent();
			}
			SingletonUnity<PlayerInfoMenuRootLogic>.Instance.Reset();
		});
	}

	public void OnClickBagBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate(bool isSuccess, object param)
		{
			if (isSuccess)
			{
				Debug.Log("!!!!!!!!!!!!!!!!!!!!!!! :: " + TutorialManager.CurStep);
				CheckTutorialEvent();
			}
			SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickItemBackPackBtn();
		});
	}

	public void OnClickSocialBtn()
	{
		SingletonUnity<SocialUIRootLogic>.Instance.ShowSocialUI();
		SingletonUnity<SocialUIRootLogic>.Instance.OnClickMailBtn();
	}

	public void OnClickTitleBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TitleUIRootLogic);
	}

	public void OnClickEnhaceBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
		{
			SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipEnhance();
		});
	}

	public void OnClickCarBtn()
	{
	}

	public UISprite GetBtnIconByFunctionType(FUNCTION_TYPE type)
	{
		return type switch
		{
			FUNCTION_TYPE.CHARACTER => CharacterBtnIcon, 
			FUNCTION_TYPE.BAG => BagBtnIcon, 
			FUNCTION_TYPE.SOCIAL => SocialBtnIcon, 
			FUNCTION_TYPE.GUILD => GuildBtnIcon, 
			FUNCTION_TYPE.CAR => CarBtnIcon, 
			FUNCTION_TYPE.TITLE => TitleBtnIcon, 
			FUNCTION_TYPE.ENHANCE => EnhanceBtnIcon, 
			FUNCTION_TYPE.SKILL => SkillBtnIcon, 
			_ => null, 
		};
	}
}
