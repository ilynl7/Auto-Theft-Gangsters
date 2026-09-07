using System.Collections.Generic;

public class EquipStrengthenUIRootLogic : SingletonUnity<EquipStrengthenUIRootLogic>
{
	public enum EQUIP_STRENGTHEN_PAGE
	{
		NOTHING = -1,
		ENHANCE,
		REFINE,
		BADGE,
		INHERT,
		SKILL,
		TITLE
	}

	public enum OPENTYPE
	{
		NOTHINTG,
		EQUIP,
		BADGE
	}

	private EQUIP_STRENGTHEN_PAGE mcurPageType = EQUIP_STRENGTHEN_PAGE.NOTHING;

	private OPENTYPE mCurOpenType;

	public EQUIP_STRENGTHEN_PAGE CurPageType => mcurPageType;

	public void ClearBackAction()
	{
		mCurOpenType = OPENTYPE.NOTHINTG;
	}

	private GameItem GetPlayerWeapon()
	{
		ItemContainer equipPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.EquipPack;
		return equipPack.getWeapon();
	}

	private GameItem GetFirstEquip()
	{
		ItemContainer equipPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.EquipPack;
		return equipPack.GetFirstNoEmptyItem();
	}

	private List<GameItem> GetEquipListInEquipPack()
	{
		ItemContainer equipPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.EquipPack;
		return equipPack.ItemList;
	}

	public void InitEquipStrengtheUI()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
			MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickSkillBtn, isIcon: true, "CZ_left_Skill", StrDictionary.GetDictionaryString("#{100118}"), FUNCTION_TYPE.SKILL, CheckSkillUpdateTips);
			MenuTabBtnInfo item2 = new MenuTabBtnInfo(OnClickEquipEnhanceBtn, isIcon: true, "CZ_left_Enhance", StrDictionary.GetDictionaryString("#{100618}"), FUNCTION_TYPE.ENHANCE_EQUIP, playerData.IsHaveEnhanceTips);
			MenuTabBtnInfo item3 = new MenuTabBtnInfo(OnClickEquipRefineBtn, isIcon: true, "CZ_left_Upgrade", StrDictionary.GetDictionaryString("#{100619}"), FUNCTION_TYPE.ENHANCE_STAR, playerData.IsHaveRefineTips);
			MenuTabBtnInfo item4 = new MenuTabBtnInfo(OnClickBadgeMergeBtn, isIcon: true, "CZ_left_BadgeUp", StrDictionary.GetDictionaryString("#{100616}"), FUNCTION_TYPE.ENHANCE_BADGE);
			MenuTabBtnInfo item5 = new MenuTabBtnInfo(OnClickPlayerTitleBtn, isIcon: true, "CZ_left_Character", StrDictionary.GetDictionaryString("#{101701}"), FUNCTION_TYPE.TITLE_TITLE, CheckTitleTips);
			list.Add(item);
			list.Add(item2);
			list.Add(item3);
			list.Add(item4);
			list.Add(item5);
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(list, OnClickCloseBtn);
			mcurPageType = EQUIP_STRENGTHEN_PAGE.NOTHING;
		});
	}

	public void Reset()
	{
		if (!SingletonUnity<MenuBaseRootLogic>.Instance.AutoClickTipsTap())
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ENHANCE_EQUIP))
			{
				ShowEquipEnhance();
			}
			else
			{
				OnClickSkillBtn();
			}
		}
	}

	public void OnClickSkillBtn()
	{
		ShowSkillInfo();
	}

	public void ShowSkillInfo(OPENTYPE type = OPENTYPE.NOTHINTG)
	{
		if (mcurPageType != EQUIP_STRENGTHEN_PAGE.SKILL)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RefineUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BadgeMergeRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EnhanceUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GameMenuShengWangRootUI);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GameMenuSkillInfoRootUI, delegate
			{
				SingletonUnity<SkillInfoRootLogic>.Instance.Reset();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(0);
			mcurPageType = EQUIP_STRENGTHEN_PAGE.SKILL;
			mCurOpenType = type;
		}
	}

	public void ShowEquipEnhance(GameItem item = null, OPENTYPE type = OPENTYPE.NOTHINTG)
	{
		if (item == null)
		{
			item = GetPlayerWeapon();
		}
		if (item == null)
		{
			item = GetFirstEquip();
		}
		if (item != null)
		{
			if (!SingletonUnity<EnhanceUIRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<EnhanceUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RefineUIRootLogic);
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BadgeMergeRoot);
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GameMenuSkillInfoRootUI);
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GameMenuShengWangRootUI);
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EnhanceUIRootLogic);
			}
			SingletonUnity<EnhanceUIRootLogic>.Instance.Show(item);
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(1);
		}
		mCurOpenType = type;
	}

	public void OnClickEquipEnhanceBtn()
	{
		if (mcurPageType != 0)
		{
			ShowEquipEnhance();
			mcurPageType = EQUIP_STRENGTHEN_PAGE.ENHANCE;
		}
	}

	public void ShowEquipRefine()
	{
		if (!SingletonUnity<RefineUIRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<RefineUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EnhanceUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BadgeMergeRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GameMenuSkillInfoRootUI);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GameMenuShengWangRootUI);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.RefineUIRootLogic);
		}
		SingletonUnity<RefineUIRootLogic>.Instance.Show();
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(2);
	}

	public void OnClickEquipRefineBtn()
	{
		if (mcurPageType != EQUIP_STRENGTHEN_PAGE.REFINE)
		{
			ShowEquipRefine();
			mcurPageType = EQUIP_STRENGTHEN_PAGE.REFINE;
		}
	}

	public void ShowBadgeMerge(GameItem item = null, OPENTYPE type = OPENTYPE.NOTHINTG)
	{
		if (!SingletonUnity<BadgeMergeRootLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<BadgeMergeRootLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RefineUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EnhanceUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GameMenuSkillInfoRootUI);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GameMenuShengWangRootUI);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BadgeMergeRoot, delegate
			{
				SingletonUnity<BadgeMergeRootLogic>.Instance.Show(item);
			});
		}
		SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(3);
		mCurOpenType = type;
	}

	public void OnClickBadgeMergeBtn()
	{
		if (mcurPageType != EQUIP_STRENGTHEN_PAGE.BADGE)
		{
			ShowBadgeMerge();
			mcurPageType = EQUIP_STRENGTHEN_PAGE.BADGE;
		}
	}

	public void OnClickPlayerTitleBtn()
	{
		if (mcurPageType != EQUIP_STRENGTHEN_PAGE.TITLE)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RefineUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BadgeMergeRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EnhanceUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GameMenuSkillInfoRootUI);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GameMenuShengWangRootUI, delegate
			{
				SingletonUnity<JSShengWangLogic>.Instance.Reset();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(4);
			mcurPageType = EQUIP_STRENGTHEN_PAGE.TITLE;
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RefineUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EnhanceUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EquipStrengthenUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GameMenuSkillInfoRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BadgeMergeRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GameMenuShengWangRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		if (mCurOpenType == OPENTYPE.BADGE)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate
			{
				SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickBadgeBtn();
			});
		}
		else if (mCurOpenType == OPENTYPE.EQUIP)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate
			{
				SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickEquipBackPackBtn();
			});
		}
	}

	private void OnEnable()
	{
		InitEquipStrengtheUI();
	}

	public bool CheckSkillUpdateTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SKILL))
		{
			return false;
		}
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		for (int i = 0; i < mainPlayer.CharacterSkillData.Count; i++)
		{
			SkillupgradeData skillupgradeData = null;
			CharacterSkillData characterSkillData = mainPlayer.CharacterSkillData[i];
			if (characterSkillData == null || !SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(characterSkillData.UnlockLevel))
			{
				continue;
			}
			int index = characterSkillData.Index;
			if (index >= 4 && index <= 6)
			{
				SkillData skillDataById = DataManager.GetSkillDataById(characterSkillData.ID);
				skillupgradeData = DataManager.GetSkillupgradeDataByLevel(characterSkillData.Level + 1);
				if (skillDataById != null && skillDataById.IsUpgrade != 0 && skillupgradeData != null && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level > characterSkillData.Level + 1 && GameMoneyHelper.GetMoneyNum(skillupgradeData.PriceType) >= skillupgradeData.PriceValue)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool CheckTitleTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.TITLE_TITLE))
		{
			return false;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		int curTitleExp = playerData.MainPlayerAttrData.CurTitleExp;
		int curTitleLevel = playerData.MainPlayerAttrData.CurTitleLevel;
		if (curTitleLevel < 10 && curTitleLevel >= 0)
		{
			TitleData titleDateById = DataManager.GetTitleDateById(curTitleLevel.ToString());
			if (curTitleExp >= titleDateById.EXP)
			{
				return true;
			}
			return false;
		}
		return false;
	}
}
