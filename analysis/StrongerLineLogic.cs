using System.Collections.Generic;
using UnityEngine;

public class StrongerLineLogic : MonoBehaviour
{
	public UISprite btnSp;

	public UISprite IconFlag;

	public UILabel InfoLabel;

	public GameObject sliderObj;

	public UISlider ProgressValue;

	public UILabel StateLabel;

	public UISprite sliderFore;

	public GameObject starObj;

	public UISprite[] starList;

	private StrongerData curData;

	public UISprite TipsFlag;

	public void Reset(StrongerData curstrongerdata)
	{
		curData = curstrongerdata;
		btnSp.spriteName = GameDefine.BtnIconNew[0];
		IconFlag.spriteName = curstrongerdata.Icon;
		IconFlag.SetDimensions(curData.IconWidth, curData.IconHeight);
		TipsFlag.enabled = false;
		InfoLabel.text = StrDictionary.GetDictionaryString(curstrongerdata.Desc);
		if (curData.Star == 0)
		{
			UnityVersionUtil.SetActiveRecursive(starObj, state: false);
			UnityVersionUtil.SetActiveRecursive(sliderObj, state: true);
			StateLabel.enabled = true;
			switch ((GameDefine.STRONGER_ACTIVITY)curData.Type)
			{
			case GameDefine.STRONGER_ACTIVITY.SKILL:
				ProgressValue.value = GetSkillProgress();
				break;
			case GameDefine.STRONGER_ACTIVITY.ENHANCE_CUS:
				ProgressValue.value = GetEnhanceCusProgress();
				break;
			case GameDefine.STRONGER_ACTIVITY.HONOR:
				ProgressValue.value = GetHonorProgress();
				break;
			case GameDefine.STRONGER_ACTIVITY.ENHANCE_STR:
				ProgressValue.value = GetEnhanceStarProgress();
				break;
			case GameDefine.STRONGER_ACTIVITY.ENHANCE_FUSE:
				ProgressValue.value = GetBadgeProgress();
				break;
			case GameDefine.STRONGER_ACTIVITY.BIGSALE:
			{
				PlayerCommonData playerCommonData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
				if (!playerCommonData2.Big_PackFlag)
				{
					ProgressValue.value = 1f;
				}
				else
				{
					ProgressValue.value = 0f;
				}
				break;
			}
			case GameDefine.STRONGER_ACTIVITY.FIRSTBUY:
			{
				PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
				if (!playerCommonData.First_PackFlag)
				{
					ProgressValue.value = 1f;
				}
				else
				{
					ProgressValue.value = 0f;
				}
				break;
			}
			case GameDefine.STRONGER_ACTIVITY.GUILDSKILL:
				ProgressValue.value = GetGuildSkillProgress();
				break;
			case GameDefine.STRONGER_ACTIVITY.EQUIPMORE:
				ProgressValue.value = GetEquipLevelProgress();
				break;
			case GameDefine.STRONGER_ACTIVITY.EQUIPBEST:
				ProgressValue.value = GetEquipLevelProgress();
				break;
			}
			if (ProgressValue.value < 0.5f)
			{
				StateLabel.text = StrDictionary.GetDictionaryString("#{800105}");
			}
			else if (ProgressValue.value < 0.75f)
			{
				StateLabel.text = StrDictionary.GetDictionaryString("#{800106}");
			}
			else if (ProgressValue.value < 0.9f)
			{
				StateLabel.text = StrDictionary.GetDictionaryString("#{800107}");
			}
			else
			{
				StateLabel.text = StrDictionary.GetDictionaryString("#{800108}");
			}
			if (ProgressValue.value < float.Epsilon)
			{
				sliderFore.enabled = false;
			}
			else
			{
				sliderFore.enabled = true;
			}
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(starObj, state: true);
			UnityVersionUtil.SetActiveRecursive(sliderObj, state: false);
			StateLabel.enabled = false;
			for (int i = 0; i < starList.Length; i++)
			{
				if (i < curData.Star)
				{
					starList[i].enabled = true;
				}
				else
				{
					starList[i].enabled = false;
				}
			}
			switch ((GameDefine.STRONGER_ACTIVITY)curData.Type)
			{
			case GameDefine.STRONGER_ACTIVITY.MAIN_LINE:
				if (SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetCurMissionByClassType(MISSION_CLASS_TYPE.MAIN) == null)
				{
					btnSp.spriteName = GameDefine.BtnIconNew[1];
				}
				break;
			case GameDefine.STRONGER_ACTIVITY.DAILY_LINE:
				if (!SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.IsHaveDailyMission())
				{
					btnSp.spriteName = GameDefine.BtnIconNew[1];
				}
				break;
			}
		}
		SetTipsInfo();
	}

	public void SetTipsInfo()
	{
		TipsFlag.enabled = false;
		if (curData == null || curData.StrongerType != 2)
		{
			return;
		}
		switch (curData.Type)
		{
		case 6:
			if (SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.IsHaveDailyMission())
			{
				TipsFlag.enabled = true;
			}
			break;
		case 7:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.IsDailyCopyCanPlay(MAPTYPE.EQUIP_COPY))
			{
				TipsFlag.enabled = true;
			}
			break;
		case 8:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.IsDailyCopyCanPlay(MAPTYPE.EXP_DAILY_COPY))
			{
				TipsFlag.enabled = true;
			}
			break;
		case 9:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.ESCORT) || SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT))
			{
				TipsFlag.enabled = true;
			}
			break;
		case 10:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.CITY_DANCE))
			{
				TipsFlag.enabled = true;
			}
			break;
		case 11:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.BAR_FIGHT))
			{
				TipsFlag.enabled = true;
			}
			break;
		case 12:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.IsDailyCopyCanPlay(MAPTYPE.SCUFFLE_AREA_1))
			{
				TipsFlag.enabled = true;
			}
			break;
		case 34:
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopyInfoData.IsDailyCopyCanPlay(MAPTYPE.BIG_WORLD))
			{
				TipsFlag.enabled = true;
			}
			break;
		}
	}

	public float GetGuildSkillProgress()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetGuildSkillProgress();
	}

	public float GetEquipLevelProgress()
	{
		float num = 0f;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ItemContainer equipPack = playerData.EquipPack;
		for (int i = 0; i < equipPack.ContainerSize; i++)
		{
			if (!equipPack.ItemList[i].IsEmpty() && playerData.Level - equipPack.ItemList[i].ItemData.Level < 10)
			{
				num += 1f;
			}
		}
		return num / 6f;
	}

	public float GetBadgeProgress()
	{
		float num = 0f;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		RefineData[] array = new RefineData[5];
		List<GameItem> itemList = playerData.BadgeEquipPack.ItemList;
		for (int i = 0; i < itemList.Count; i++)
		{
			GameItem gameItem = itemList[i];
			if (!gameItem.IsEmpty())
			{
				ItemData itemData = gameItem.ItemData;
				BadgeData badgeDataById = DataManager.GetBadgeDataById(gameItem.ItemId);
				num += (float)badgeDataById.Lv;
			}
		}
		return num / 40f;
	}

	public float GetEnhanceStarProgress()
	{
		float num = 0f;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		RefineData[] array = new RefineData[5];
		for (int i = 1; i < 5; i++)
		{
			array[i] = DataManager.GetRefineDataByPartLevelPRO(i, playerData.MainPlayerAttrData.GetTargetRefinePartLevel((REFINE_PART)i), (int)playerData.Profession);
			num += (float)array[i].Lv;
		}
		return num / 40f;
	}

	public float GetHonorProgress()
	{
		float num = 0f;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		int curTitleLevel = playerData.MainPlayerAttrData.CurTitleLevel;
		return (float)curTitleLevel / 10f;
	}

	public float GetEnhanceCusProgress()
	{
		float num = 0f;
		ItemContainer equipPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.EquipPack;
		for (int i = 0; i < equipPack.ContainerSize; i++)
		{
			if (!equipPack.ItemList[i].IsEmpty())
			{
				num += (float)equipPack.ItemList[i].ItemLevel;
			}
		}
		return num / (float)(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level * 6);
	}

	public float GetSkillProgress()
	{
		float num = 0f;
		int num2 = 0;
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		for (int i = 0; i < mainPlayer.CharacterSkillData.Count; i++)
		{
			CharacterSkillData characterSkillData = mainPlayer.CharacterSkillData[i];
			if (characterSkillData == null || !SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(characterSkillData.UnlockLevel))
			{
				continue;
			}
			int index = characterSkillData.Index;
			if (index >= 4 && index <= 6)
			{
				SkillData skillDataById = DataManager.GetSkillDataById(characterSkillData.ID);
				if (skillDataById != null && skillDataById.IsUpgrade == 1)
				{
					num2 += SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
					num += (float)(characterSkillData.Level + 1);
				}
			}
		}
		return num / (float)num2;
	}

	public void OnClickGoToBtn()
	{
		switch ((GameDefine.STRONGER_ACTIVITY)curData.Type)
		{
		case GameDefine.STRONGER_ACTIVITY.SKILL:
			if (!CheckUnlockFunction(FUNCTION_TYPE.SKILL))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
			{
				SingletonUnity<EquipStrengthenUIRootLogic>.Instance.OnClickSkillBtn();
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.ENHANCE_CUS:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ENHANCE_EQUIP))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
			{
				SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipEnhance();
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.HONOR:
			if (!CheckUnlockFunction(FUNCTION_TYPE.TITLE))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
			{
				SingletonUnity<EquipStrengthenUIRootLogic>.Instance.OnClickPlayerTitleBtn();
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.ENHANCE_STR:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ENHANCE_STAR))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
			{
				SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowEquipRefine();
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.ENHANCE_FUSE:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ENHANCE_BADGE))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EquipStrengthenUIRootLogic, delegate
			{
				SingletonUnity<EquipStrengthenUIRootLogic>.Instance.ShowBadgeMerge();
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.MAIN_LINE:
			if (!CheckUnlockFunction(FUNCTION_TYPE.MAIN_MISSION))
			{
				return;
			}
			SingletonUnity<MissionTeamTipLogic>.Instance.ResetMissionTip();
			if (!SingletonUnity<MissionTeamTipLogic>.Instance.MissionTipRoot.IsHaveMainLineMission())
			{
				NoticeLogic.AddNotifyData("#{800501}");
			}
			else
			{
				TutorialManager.ShowTutorial(TUTORIAL_STEP.MAIN_MISSION_CLICK_START);
			}
			break;
		case GameDefine.STRONGER_ACTIVITY.DAILY_LINE:
			if (!CheckUnlockFunction(FUNCTION_TYPE.DAILY_MISSION))
			{
				return;
			}
			SingletonUnity<MissionTeamTipLogic>.Instance.ResetMissionTip();
			if (!SingletonUnity<MissionTeamTipLogic>.Instance.MissionTipRoot.IsHaveDailyMission())
			{
				NoticeLogic.AddNotifyData("#{800502}");
			}
			else
			{
				TutorialManager.ShowTutorial(TUTORIAL_STEP.DAILY_MISSION_CLICK_START);
			}
			break;
		case GameDefine.STRONGER_ACTIVITY.EQUIP_COPY:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.EQUIP_COPY);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.EXP_COPY:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.EXP_DAILY_COPY);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.ESCORT:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.ESCORT))
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.ESCORT);
				}
				else if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.IsTimeActivityCanPlay(GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT))
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.ATTACK_ESCORT);
				}
				else
				{
					SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.ESCORT);
				}
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.DANCE:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.CITY_DANCE);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.BARFIGHT:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.BAR_FIGHT);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.SCUFFLE_AREA:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.SCUFFLE_AREA_1);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.CASH_COPY:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.CASH_DAILY_COPY);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.CAR_COPY:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.CAR_CHASE_COPY);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.TOWER:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_CHALLENGE))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.INVALID, null, null, GameDefine.ACTIVITY_TYPE.TOWER);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.WILD_BOSS:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_WORLDBOSS))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.WILD_BOSS);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.PVP:
			if (!CheckUnlockFunction(FUNCTION_TYPE.RANK_PVP))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickRankBtn();
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.DIAMOND_BUY:
			if (!CheckUnlockFunction(FUNCTION_TYPE.SHOP_BUY))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
			{
				SingletonUnity<ShopUIRootLogic>.Instance.OnClickBuyDiamondBtn();
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.SURVIVE_BATTLE:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.SURVIVE_BATTLE);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.GUILD_SHOP:
			if (!CheckUnlockFunction(FUNCTION_TYPE.SHOP_GUILD))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
			{
				SingletonUnity<ShopUIRootLogic>.Instance.Reset();
				SingletonUnity<ShopUIRootLogic>.Instance.OnClickGuildBtn((GameDefine.SHOP_TAB_TYPE)curData.SubType, GameDefine.UIBACKTYPE.NOTHINTG, curData.shopitemid);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.DIAMOND_SHOP:
			if (!CheckUnlockFunction(FUNCTION_TYPE.SHOP_TOOL))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
			{
				SingletonUnity<ShopUIRootLogic>.Instance.OnClickToolsBtn((GameDefine.SHOP_TAB_TYPE)curData.SubType, GameDefine.UIBACKTYPE.NOTHINTG, curData.shopitemid);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.GOLD_SHOP:
			if (!CheckUnlockFunction(FUNCTION_TYPE.SHOP_EQUIP))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
			{
				SingletonUnity<ShopUIRootLogic>.Instance.OnClickEquipBtn((GameDefine.SHOP_TAB_TYPE)curData.SubType, GameDefine.UIBACKTYPE.NOTHINTG, curData.shopitemid);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.SLOT:
			if (!CheckUnlockFunction(FUNCTION_TYPE.LOTTO))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SlotUIRoot, delegate
			{
				SingletonUnity<SlotUIRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(242, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_slot_info>();
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.GUILD_DONATE:
			if (!CheckUnlockFunction(FUNCTION_TYPE.GUILD))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewGuildUIRootLogic, delegate
			{
				SingletonUnity<NewGuildUIRootLogic>.Instance.targetTabType = 0;
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.GUILD_BOSS:
			if (!CheckUnlockFunction(FUNCTION_TYPE.GUILD_ACTIVITY))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.Reset();
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.GUILD_BOSS);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.CASH_SHOP:
			if (!CheckUnlockFunction(FUNCTION_TYPE.SHOP_BIGSALE))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShopRoot, delegate
			{
				SingletonUnity<ShopUIRootLogic>.Instance.OnClickBigSaleBtn((GameDefine.SHOP_TAB_TYPE)curData.SubType, GameDefine.UIBACKTYPE.NOTHINTG, curData.shopitemid);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.SEX_GAME:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.SEX_MINI);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.BIGSALE:
		{
			if (!CheckUnlockFunction(FUNCTION_TYPE.BIGSALES))
			{
				return;
			}
			PlayerCommonData playerCommonData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			if (!playerCommonData2.Big_PackFlag)
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BigPackRoot, delegate
			{
				SingletonUnity<BigPackRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(260, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_big_pack>();
			});
			break;
		}
		case GameDefine.STRONGER_ACTIVITY.FIRSTBUY:
		{
			if (!CheckUnlockFunction(FUNCTION_TYPE.BIGSALES))
			{
				return;
			}
			PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			if (!playerCommonData.First_PackFlag)
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.FirstBuyRoot, delegate
			{
				SingletonUnity<FirstBuyRootLogic>.Instance.EnableReset();
				WaitResponseUIRootLogic.OpenWaitBox(259, 10f, 0f);
				NetLogic.GetInstance().Send<Protocol.request_first_buy>();
			});
			break;
		}
		case GameDefine.STRONGER_ACTIVITY.GUILDSKILL:
			if (!CheckUnlockFunction(FUNCTION_TYPE.GUILD_SKILL))
			{
				return;
			}
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewGuildUIRootLogic, delegate
				{
					SingletonUnity<NewGuildUIRootLogic>.Instance.targetTabType = 2;
				});
			}
			else
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewGuildUIRootLogic);
			}
			break;
		case GameDefine.STRONGER_ACTIVITY.EQUIPMORE:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.EQUIP_COPY);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.EQUIPBEST:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_TIME))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickTimeBtn(GameDefine.ACTIVITY_TYPE.BAR_FIGHT);
			});
			break;
		case GameDefine.STRONGER_ACTIVITY.PKMAP:
			if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_DAILY))
			{
				return;
			}
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn();
			});
			break;
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("BeStronger", "BeStronger", $"clicktype_{curData.Type}");
	}

	public bool CheckUnlockFunction(FUNCTION_TYPE curFunction)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData.IsFunctionUnlock(curFunction))
		{
			OnClickCloseBtn();
			return true;
		}
		NoticeLogic.AddNotifyData("#{101539}");
		return false;
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.StrongerRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
	}
}
