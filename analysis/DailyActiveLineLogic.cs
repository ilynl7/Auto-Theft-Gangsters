using SprotoType;
using UnityEngine;

public class DailyActiveLineLogic : MonoBehaviour
{
	public UILabel TimesLabel;

	public UILabel TitleLabel;

	public UILabel InfoLabel;

	public UILabel ScoreLabel;

	public UILabel btnLabel;

	public UISprite btnsp;

	private daily_active curActive;

	private DailyActiveData curData;

	public void UpdateInfo(daily_active curinfo)
	{
		curActive = curinfo;
		curData = DataManager.GetDailyActiveDataById(curinfo.ID);
		TitleLabel.text = StrDictionary.GetDictionaryString(curData.Title);
		InfoLabel.text = StrDictionary.GetDictionaryString(curData.Desc, curData.Count);
		if (curActive.count < curData.Count)
		{
			TimesLabel.text = $"{curActive.count}/{curData.Count}";
		}
		else
		{
			TimesLabel.text = $"{curData.Count}/{curData.Count}";
		}
		ScoreLabel.text = $"\ufffd\ufffd{curData.Score * curData.Count}";
		if (curActive.count >= curData.Count)
		{
			btnsp.spriteName = GameDefine.BtnIconNew[1];
			btnLabel.text = StrDictionary.GetDictionaryString("#{300802}");
		}
		else
		{
			btnsp.spriteName = GameDefine.BtnIconNew[0];
			btnLabel.text = StrDictionary.GetDictionaryString("#{300801}");
		}
	}

	public void OnClickGoBtn()
	{
		if (curActive.count >= curData.Count)
		{
			return;
		}
		switch ((GameDefine.DAILY_ACTIVE_TYPE)curData.Type)
		{
		case GameDefine.DAILY_ACTIVE_TYPE.EQUIP_COPY:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.EQUIP_COPY);
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.EQUIP_COPY);
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.EXP_COPY:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.EXP_DAILY_COPY);
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.EXP_DAILY_COPY);
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.CASH_COPY:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.CASH_DAILY_COPY);
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.CASH_DAILY_COPY);
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.CAR_COPY:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.CAR_CHASE_COPY);
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.CAR_CHASE_COPY);
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.ATTACK_ESCORT:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT);
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT);
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.TOWER_COPY:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_CHALLENGE))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.INVALID, null, null, GameDefine.ACTIVITY_TYPE.TOWER);
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.INVALID, null, null, GameDefine.ACTIVITY_TYPE.TOWER);
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.WILD_BOSS:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_WORLDBOSS))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.WILD_BOSS);
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.WILD_BOSS);
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.RANK_PVP:
			if (!CheckUnlockFunction(FUNCTION_TYPE.RANK_PVP))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickRankBtn();
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickRankBtn();
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.EQUIP_UPGRADE:
			if (CheckUnlockFunction(FUNCTION_TYPE.ENHANCE_EQUIP))
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
				{
					SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipEnhance();
				});
			}
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.REFINE_UPGRADE:
			if (CheckUnlockFunction(FUNCTION_TYPE.ENHANCE_STAR))
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
				{
					SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipRefine();
				});
			}
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.SKILL_UPGRADE:
			if (CheckUnlockFunction(FUNCTION_TYPE.SKILL))
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
				{
					SingletonUnity<EquipStrengthenUIRootLogic>.Instance.OnClickSkillBtn();
				});
			}
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.GOLD_BUY:
			if (CheckUnlockFunction(FUNCTION_TYPE.SHOP_EQUIP))
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
				{
					SingletonUnity<ShopUIRootLogic>.Instance.OnClickEquipBtn();
				});
			}
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.DIAMOND_BUY:
			if (CheckUnlockFunction(FUNCTION_TYPE.SHOP_TOOL))
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
				{
					SingletonUnity<ShopUIRootLogic>.Instance.OnClickToolsBtn();
				});
			}
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.DAILY_BUY:
			if (CheckUnlockFunction(FUNCTION_TYPE.GIFT_DAILY))
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CommercialUIRoot, delegate
				{
					SingletonUnity<CommercialUIRootLogic>.Instance.OnClickDailyBuyBtn();
				});
			}
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.ESCORT:
			if (CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
			{
				SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.ESCORT);
				});
			}
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.DOMIN:
			if (!CheckUnlockFunction(FUNCTION_TYPE.DOMIN))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDominBtn();
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDominBtn();
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.SCUFFLE_AREA:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.SCUFFLE_AREA_1);
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.SCUFFLE_AREA_1);
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.SINGLE_DANCE:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.SINGLE_DANCE);
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.SINGLE_DANCE);
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.SURVIVE:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE);
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE);
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.GUILD_BOSS:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_BOSS);
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_BOSS);
			});
			break;
		case GameDefine.DAILY_ACTIVE_TYPE.GUILD_DANCE:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
			{
				break;
			}
			if (SingletonUnity<NewActivityUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewActivityUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_DANCE);
				break;
			}
			SingletonUnity<MenuBaseRootLogic>.Instance.OnClickBackBtn();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_DANCE);
			});
			break;
		}
	}

	public bool CheckUnlockFunction(FUNCTION_TYPE curFunction)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData.IsFunctionUnlock(curFunction))
		{
			return true;
		}
		NoticeLogic.AddNotifyData("#{101539}");
		return false;
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActiveRewardRoot);
	}
}
