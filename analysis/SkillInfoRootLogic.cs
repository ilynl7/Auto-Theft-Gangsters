using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SkillInfoRootLogic : SingletonUnity<SkillInfoRootLogic>
{
	private int mClickUpgradeBtnCount;

	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	private vp_Timer.Handle clickHandle;

	public TweenAlpha tweenAlpha;

	public TweenPosition tweenPosition;

	private Vector3 ModelPos = new Vector3(-0.6819441f, 0.2052425f, -2.390722f);

	private Vector3 ModelAngle = new Vector3(30.60997f, 13.9f, 0f);

	public UISprite SelectSprite;

	private int curSelect;

	private GameObject Selectobj;

	public List<SkillInfoBtnLogic> SkillBtnLogicList = new List<SkillInfoBtnLogic>();

	public List<SkillInfoBtnLogic> SkillBtnUseLogicList = new List<SkillInfoBtnLogic>();

	public UILabel SkillNameLabel;

	public UILabel SkillTypeLabel;

	public UILabel SkillCDLabel;

	public UILabel SkillDisLabel;

	public UILabel SkillTargetLabel;

	public UILabel CurLevelLabel;

	public UILabel SkillCurLevelDamageLabel;

	public UILabel NextLevelLabel;

	public UILabel SkillNextLevelDamageLabel;

	public UILabel SkillDescLabel;

	public List<UILabel> SkillLabelLabelList;

	public List<UILabel> SkillScoresLabelList;

	public List<UISprite> SkillLabelPicList;

	public UILabel ScoresLabel;

	public UILabel UpgradeSkillCostInfoLabel;

	public UISprite Plan1BtnPic;

	public UISprite Plan2BtnPic;

	private string PlanEnablePicName = "CZ_huaDongBG_1";

	private string PlanDisablePicName = "CZ_huaDongBG";

	public UISprite UpgradeBtnRoot;

	public UISprite UpgradeAllBtnRoot;

	public GameObject PlaySkillBtn;

	private CharacterSkillData mCurChaSkillData;

	private int mPlayerLevel;

	public GameObject levelinfoFlag;

	public UILabel LevelInfoLabel;

	public UITexture PlayerModelPic;

	public GameObject AnimationBtnRoot;

	private PlayerData playerData;

	private int mCurLevel;

	public SkillupgradeData skillNextUpgradeData;

	private FakeObjLogic mPlayerModelVisual;

	private string[] UnlockLevelStr = new string[4]
	{
		string.Empty,
		"#{101163}",
		"#{101164}",
		"#{101165}"
	};

	private int curSkillIndex;

	public FakeObjLogic PlayerModelVisual
	{
		get
		{
			return mPlayerModelVisual;
		}
		set
		{
			mPlayerModelVisual = value;
		}
	}

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mClickUpgradeBtnCount = 0;
		mOnClickTutorialBtn = tutorialEvent;
		if (TutorialManager.CurStep == TUTORIAL_STEP.SKILL_UPGRADE_CLICK)
		{
			clickHandle = new vp_Timer.Handle();
		}
	}

	private void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			TutorialManager.OnClickTutorialBtn onClickTutorialBtn = mOnClickTutorialBtn;
			mOnClickTutorialBtn = null;
			onClickTutorialBtn();
			onClickTutorialBtn = null;
		}
	}

	private void UpgradeBtnTutorialCheck()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.SKILL_UPGRADE_CLICK)
		{
			mClickUpgradeBtnCount++;
			NGUITools.SetActive(SingletonUnity<TutorialUIRootLogic>.Instance.HandTipTweenS.gameObject, state: false);
			NGUITools.SetActive(SingletonUnity<TutorialUIRootLogic>.Instance.TipCircleSprite.gameObject, state: false);
			if (clickHandle != null)
			{
				clickHandle.Cancel();
			}
			vp_Timer.In(0.5f, delegate
			{
				NGUITools.SetActive(SingletonUnity<TutorialUIRootLogic>.Instance.HandTipTweenS.gameObject, state: true);
				NGUITools.SetActive(SingletonUnity<TutorialUIRootLogic>.Instance.TipCircleSprite.gameObject, state: true);
			}, clickHandle);
			if (mClickUpgradeBtnCount >= 3 && TutorialManager.CurStep == TUTORIAL_STEP.SKILL_UPGRADE_CLICK)
			{
				CheckTutorialEvent();
			}
		}
	}

	private void CloseTutorial()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn = null;
			TutorialManager.CloseTutorial();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.SetTutorialShowFinish(FUNCTION_TYPE.SKILL);
		}
	}

	public void PlaySkillLevelAnim(int level)
	{
		if (mCurLevel < level)
		{
			tweenAlpha.ResetToBeginning();
			tweenAlpha.PlayForward();
			tweenPosition.ResetToBeginning();
			tweenPosition.PlayForward();
		}
	}

	public void ResetModelVisual()
	{
		playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ItemContainer fashionEquipPack = playerData.FashionEquipPack;
		ItemContainer equipPack = playerData.EquipPack;
		string empty = string.Empty;
		string empty2 = string.Empty;
		string empty3 = string.Empty;
		string empty4 = string.Empty;
		if (playerData.IsShowFashion)
		{
			empty = fashionEquipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.HEAD, playerData.Profession, isFashion: true);
			if (string.IsNullOrEmpty(empty))
			{
				empty = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.HEAD, playerData.Profession);
			}
			if (CheckWeaponIsSame())
			{
				empty2 = fashionEquipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.WEAPON, playerData.Profession, isFashion: true);
				if (string.IsNullOrEmpty(empty2))
				{
					empty2 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.WEAPON, playerData.Profession);
				}
			}
			else
			{
				empty2 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.WEAPON, playerData.Profession);
			}
			empty3 = fashionEquipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.BODY, playerData.Profession, isFashion: true);
			if (string.IsNullOrEmpty(empty3))
			{
				empty3 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.BODY, playerData.Profession);
			}
			empty4 = fashionEquipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.LEG, playerData.Profession, isFashion: true);
			if (string.IsNullOrEmpty(empty4))
			{
				empty4 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.LEG, playerData.Profession);
			}
		}
		else
		{
			empty = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.HEAD, playerData.Profession);
			empty2 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.WEAPON, playerData.Profession);
			empty3 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.BODY, playerData.Profession);
			empty4 = equipPack.GetEquipModelIdByEquipType(EQUIP_BACKPACK_TYPE.LEG, playerData.Profession);
		}
		if (mPlayerModelVisual == null)
		{
			mPlayerModelVisual = new FakeObjLogic();
			mPlayerModelVisual.InitFakeObject(empty2, empty, empty3, empty4, playerData.Profession, SingletonUnity<FakeObjRootLogic>.Instance.MeshRoot);
		}
		else
		{
			mPlayerModelVisual.CheckFakeObject(empty2, empty, empty3, empty4);
			mPlayerModelVisual.PlayAnim("idle", playerData.CharacterModelData.ModelFirstType);
		}
	}

	public bool CheckWeaponIsSame()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ItemContainer fashionEquipPack = playerData.FashionEquipPack;
		ItemContainer equipPack = playerData.EquipPack;
		EquipData equipWeaponData = equipPack.GetEquipWeaponData(EQUIP_BACKPACK_TYPE.WEAPON);
		EquipData equipWeaponData2 = fashionEquipPack.GetEquipWeaponData(EQUIP_BACKPACK_TYPE.WEAPON);
		if (equipWeaponData != null && equipWeaponData2 != null)
		{
			return equipWeaponData.WeaponType == equipWeaponData2.WeaponType;
		}
		return true;
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GameMenuSkillInfoRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		UnLoadFakeObj();
	}

	public void UnLoadFakeObj()
	{
		if (mPlayerModelVisual != null)
		{
			mPlayerModelVisual.DestroyFakeObj();
			mPlayerModelVisual = null;
		}
		if (SingletonUnity<FakeObjRootLogic>.Instance != null)
		{
			SingletonUnity<FakeObjRootLogic>.Instance.DisableFakeObjRoot();
		}
	}

	private void OnEnable()
	{
		FakeObjRootLogic instance = SingletonUnity<FakeObjRootLogic>.Instance;
		if (instance == null)
		{
			ResourcesManager.LoadAndInstantiate("UIRoot/FakeObjRoot");
			instance = SingletonUnity<FakeObjRootLogic>.Instance;
		}
		SingletonUnity<FakeObjRootLogic>.Instance.SetPicValue(0.8f);
		SingletonUnity<FakeObjRootLogic>.Instance.EnableFakeObjRoot();
		ResetModelVisual();
		instance.objCam.targetTexture.Release();
		instance.objCam.targetTexture = null;
		instance.objCam.targetTexture = SingletonUnity<FakeObjRootLogic>.Instance.ModelPic;
		instance.objCam.ResetAspect();
		mPlayerModelVisual.FakeObj.transform.localPosition = ModelPos;
		mPlayerModelVisual.FakeObj.transform.localEulerAngles = ModelAngle;
		PlayerModelPic.mainTexture = instance.ModelPic;
		NGUITools.SetActive(PlayerModelPic.gameObject, state: false);
		curSelect = 0;
		ResetBtn(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SkillIndex);
		SkillBtnLogicList[curSelect].OnClickBtn();
	}

	private void OnDisable()
	{
		if (SingletonUnity<FakeObjRootLogic>.Exists)
		{
			if (mPlayerModelVisual.FakeObj != null)
			{
				mPlayerModelVisual.FakeObj.transform.localPosition = Vector3.zero;
				mPlayerModelVisual.FakeObj.transform.localEulerAngles = Vector3.zero;
			}
			SingletonUnity<FakeObjRootLogic>.Instance.objCam.targetTexture.Release();
			SingletonUnity<FakeObjRootLogic>.Instance.objCam.targetTexture = null;
			SingletonUnity<FakeObjRootLogic>.Instance.ModelPic.height = 512;
			SingletonUnity<FakeObjRootLogic>.Instance.objCam.targetTexture = SingletonUnity<FakeObjRootLogic>.Instance.ModelPic;
			UnLoadFakeObj();
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.SKILL_DRAG_MOVE)
		{
			CheckTutorialEvent();
		}
	}

	public void OnClickUpgradeBtn()
	{
		if (!GameMoneyHelper.BeforeCheckBuy(skillNextUpgradeData.PriceType, skillNextUpgradeData.PriceValue))
		{
			CloseTutorial();
		}
		else if (mCurChaSkillData.Level + 1 < mPlayerLevel)
		{
			skill_level_up.request request = new skill_level_up.request();
			request.skillId = mCurChaSkillData.ID;
			request.curLevel = mCurChaSkillData.Level;
			NetLogic.GetInstance().Send<Protocol.skill_level_up>(request);
			if (TutorialManager.CurStep == TUTORIAL_STEP.SKILL_UPGRADE_CLICK)
			{
				CheckTutorialEvent();
			}
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Skill", "click_updateone", "times");
		}
		else
		{
			CloseTutorial();
			NoticeLogic.AddNotifyData("#{101143}");
		}
	}

	public void OnClickUpgradeALLBtn()
	{
		if (!GameMoneyHelper.BeforeCheckBuy(skillNextUpgradeData.PriceType, skillNextUpgradeData.PriceValue))
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.SKILL_UPGRADE_CLICK_ALL)
			{
				CloseTutorial();
			}
			return;
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.SKILL_UPGRADE_CLICK_ALL)
		{
			CheckTutorialEvent();
		}
		if (mCurChaSkillData.Level + 1 < mPlayerLevel)
		{
			skill_level_up.request request = new skill_level_up.request();
			request.skillId = mCurChaSkillData.ID;
			request.curLevel = mCurChaSkillData.Level;
			request.all = 1L;
			NetLogic.GetInstance().Send<Protocol.skill_level_up>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Skill", "click_updateall", "times");
		}
		else
		{
			NoticeLogic.AddNotifyData("#{101143}");
		}
	}

	private void OnClickOkUpgradeBtn()
	{
	}

	public void OnClickSkillBtn(SkillData skillData, CharacterSkillData curChaSkillData, GameObject obj)
	{
		Selectobj = obj;
		SelectSprite.transform.position = obj.transform.position;
		ResetSkillInfo(skillData, curChaSkillData);
		mCurChaSkillData = curChaSkillData;
	}

	public void ResetSkillInfo(SkillData skillData, CharacterSkillData chaSkillData)
	{
		int num = 0;
		int sKILL_MAX_LEVEL = GameDefine.SKILL_MAX_LEVEL;
		NGUITools.SetActive(PlayerModelPic.gameObject, state: false);
		if (chaSkillData != null)
		{
			num = chaSkillData.Level;
			skillNextUpgradeData = DataManager.GetSkillupgradeDataByLevel(chaSkillData.Level + 1);
			CurLevelLabel.text = $"Lv.{num + 1}";
			NGUITools.SetActive(UpgradeBtnRoot.gameObject, state: true);
			NGUITools.SetActive(UpgradeAllBtnRoot.gameObject, state: true);
			if (skillData != null)
			{
				NGUITools.SetActive(AnimationBtnRoot, state: true);
				SkillNameLabel.text = skillData.MName;
				SkillDescLabel.text = skillData.MDescription;
				SkillTypeLabel.text = StrDictionary.GetDictionaryString("#{101156}", "Instant");
				SkillCDLabel.text = StrDictionary.GetDictionaryString("#{101157}", skillData.CDSecond);
				SkillDisLabel.text = StrDictionary.GetDictionaryString("#{101158}", skillData.TraceDistanceMeter);
				SkillTargetLabel.text = StrDictionary.GetDictionaryString("#{101159}", skillData.MaxAttackCount);
				if (skillData.IsUpgrade == 1)
				{
					NGUITools.SetActive(SkillCurLevelDamageLabel.gameObject, state: true);
					NGUITools.SetActive(SkillNextLevelDamageLabel.gameObject, state: true);
					if (chaSkillData.Level > 0 && skillNextUpgradeData != null)
					{
						SkillCurLevelDamageLabel.text = StrDictionary.GetDictionaryString("#{101102}", skillData.GetSkillDamageMultiVal(chaSkillData.Level) * 100f, skillData.GetSkillDamageVal(chaSkillData.Level));
					}
					else if (skillData.IsUpgrade == 0)
					{
						if (skillData.Job == 0)
						{
							SkillCurLevelDamageLabel.text = StrDictionary.GetDictionaryString("#{101147}");
						}
						else if (skillData.Job == 1)
						{
							SkillCurLevelDamageLabel.text = StrDictionary.GetDictionaryString("#{101148}");
						}
						else
						{
							SkillCurLevelDamageLabel.text = StrDictionary.GetDictionaryString("#{101149}");
						}
					}
					else
					{
						SkillCurLevelDamageLabel.text = StrDictionary.GetDictionaryString("#{101102}", skillData.GetSkillDamageMultiVal(chaSkillData.Level) * 100f, skillData.GetSkillDamageVal(chaSkillData.Level));
					}
					if (skillNextUpgradeData != null)
					{
						SkillNextLevelDamageLabel.text = StrDictionary.GetDictionaryString("#{101102}", skillData.GetSkillDamageMultiVal(chaSkillData.Level + 1) * 100f, skillData.GetSkillDamageVal(chaSkillData.Level + 1));
					}
				}
				else
				{
					NGUITools.SetActive(SkillCurLevelDamageLabel.gameObject, state: false);
					NGUITools.SetActive(SkillNextLevelDamageLabel.gameObject, state: false);
				}
			}
			else
			{
				NGUITools.SetActive(SkillCurLevelDamageLabel.gameObject, state: false);
				NGUITools.SetActive(SkillNextLevelDamageLabel.gameObject, state: false);
				SkillNameLabel.text = string.Empty;
				SkillDescLabel.text = string.Empty;
				SkillTypeLabel.text = StrDictionary.GetDictionaryString("#{101156}", string.Empty);
				SkillCDLabel.text = StrDictionary.GetDictionaryString("#{101157}", 0);
				SkillDisLabel.text = StrDictionary.GetDictionaryString("#{101158}", 0);
				SkillTargetLabel.text = StrDictionary.GetDictionaryString("#{101159}", 0);
				NGUITools.SetActive(AnimationBtnRoot, state: false);
			}
			if (skillNextUpgradeData != null)
			{
				UnityVersionUtil.SetActiveRecursive(UpgradeSkillCostInfoLabel.gameObject, state: true);
				NGUITools.SetActive(UpgradeBtnRoot.gameObject, state: true);
				NGUITools.SetActive(UpgradeAllBtnRoot.gameObject, state: true);
				NextLevelLabel.text = $"Lv.{num + 2}";
				UpgradeSkillCostInfoLabel.text = GameMoneyHelper.GetMoneyValStr(skillNextUpgradeData.PriceValue, skillNextUpgradeData.PriceType);
				if (chaSkillData.Level + 1 < mPlayerLevel)
				{
					UpgradeBtnRoot.spriteName = GameDefine.BtnIcon[1];
					UpgradeAllBtnRoot.spriteName = GameDefine.BtnIcon[1];
				}
				else
				{
					UpgradeBtnRoot.spriteName = GameDefine.BtnIcon[2];
					UpgradeAllBtnRoot.spriteName = GameDefine.BtnIcon[2];
				}
			}
			else
			{
				NGUITools.SetActive(SkillNextLevelDamageLabel.gameObject, state: false);
				UnityVersionUtil.SetActiveRecursive(UpgradeSkillCostInfoLabel.gameObject, state: false);
				NGUITools.SetActive(UpgradeBtnRoot.gameObject, state: false);
				NGUITools.SetActive(UpgradeAllBtnRoot.gameObject, state: false);
				UpgradeBtnRoot.spriteName = GameDefine.BtnIcon[2];
				UpgradeAllBtnRoot.spriteName = GameDefine.BtnIcon[2];
			}
			if (chaSkillData.UnlockLevel > mPlayerLevel)
			{
				NGUITools.SetActive(UpgradeBtnRoot.gameObject, state: false);
				NGUITools.SetActive(UpgradeAllBtnRoot.gameObject, state: false);
			}
			mCurLevel = num;
			NextLevelLabel.color = new Color(0.7058824f, 0.8392157f, 0.9098039f, 1f);
			NGUITools.SetActive(NextLevelLabel.gameObject, state: true);
			if (sKILL_MAX_LEVEL > 0)
			{
				if (num + 1 < sKILL_MAX_LEVEL)
				{
					if (num + 1 < mPlayerLevel)
					{
						LevelInfoLabel.text = string.Empty;
					}
					else
					{
						LevelInfoLabel.text = StrDictionary.GetDictionaryString("#{101166}", mPlayerLevel + 1);
						NextLevelLabel.color = Color.red;
						NGUITools.SetActive(SkillNextLevelDamageLabel.gameObject, state: false);
					}
					UnityVersionUtil.SetActiveRecursive(levelinfoFlag, state: true);
				}
				else
				{
					LevelInfoLabel.text = string.Empty;
					UnityVersionUtil.SetActiveRecursive(levelinfoFlag, state: false);
					NGUITools.SetActive(SkillNextLevelDamageLabel.gameObject, state: false);
					NGUITools.SetActive(NextLevelLabel.gameObject, state: false);
					CurLevelLabel.text = "Max";
				}
			}
			else
			{
				LevelInfoLabel.text = string.Empty;
				LevelInfoLabel.text = string.Empty;
				UnityVersionUtil.SetActiveRecursive(levelinfoFlag, state: false);
				NGUITools.SetActive(SkillNextLevelDamageLabel.gameObject, state: false);
			}
			int num2 = 0;
			for (int i = 0; i < SkillLabelLabelList.Count; i++)
			{
				if (skillData != null)
				{
					if (i < skillData.LabelIdList.Count)
					{
						SkillLabelData skillLabelDataByID = DataManager.GetSkillLabelDataByID(skillData.LabelIdList[i]);
						if (skillLabelDataByID != null)
						{
							SkillLabelLabelList[i].text = StrDictionary.GetDictionaryString(skillLabelDataByID.localization, skillLabelDataByID.ValueArray);
							SkillScoresLabelList[i].text = $"{skillLabelDataByID.score}";
							SkillLabelPicList[i].color = skillLabelDataByID.LabelColor;
							num2 += skillLabelDataByID.score;
						}
						else
						{
							SkillLabelPicList[i].color = Color.white;
							SkillLabelLabelList[i].text = StrDictionary.GetDictionaryString(UnlockLevelStr[i]);
							SkillScoresLabelList[i].text = string.Empty;
						}
					}
					else
					{
						SkillLabelPicList[i].color = Color.white;
						SkillLabelLabelList[i].text = StrDictionary.GetDictionaryString(UnlockLevelStr[i]);
						SkillScoresLabelList[i].text = string.Empty;
					}
				}
				else
				{
					SkillLabelPicList[i].color = Color.white;
					SkillLabelLabelList[i].text = string.Empty;
					SkillScoresLabelList[i].text = string.Empty;
				}
			}
			ScoresLabel.text = $"{num2}";
		}
		else
		{
			Debug.Log("chaSkillData==null!!!!!!!!!!");
			NGUITools.SetActive(UpgradeBtnRoot.gameObject, state: false);
			NGUITools.SetActive(UpgradeAllBtnRoot.gameObject, state: false);
		}
	}

	private void ResetBtn(int skillIndex)
	{
		curSkillIndex = skillIndex;
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		for (int i = 0; i < SkillBtnLogicList.Count; i++)
		{
			SkillBtnLogicList[i].Reset(null, isTop: true);
		}
		for (int j = 0; j < SkillBtnUseLogicList.Count; j++)
		{
			SkillBtnUseLogicList[j].Reset(null);
		}
		for (int k = 0; k < mainPlayer.CharacterSkillData.Count; k++)
		{
			int index = mainPlayer.CharacterSkillData[k].Index;
			if (index == 0)
			{
				SkillBtnUseLogicList[index].Reset(mainPlayer.CharacterSkillData[k]);
			}
			if (skillIndex == 0)
			{
				if (index >= 4 && index <= 6)
				{
					SkillBtnUseLogicList[index - 3].Reset(mainPlayer.CharacterSkillData[k]);
				}
				Plan1BtnPic.spriteName = PlanEnablePicName;
				Plan2BtnPic.spriteName = PlanDisablePicName;
			}
			else
			{
				if (index >= 7 && index <= 9)
				{
					SkillBtnUseLogicList[index - 6].Reset(mainPlayer.CharacterSkillData[k]);
				}
				Plan1BtnPic.spriteName = PlanDisablePicName;
				Plan2BtnPic.spriteName = PlanEnablePicName;
			}
			if (index > 3)
			{
				SkillBtnLogicList[mainPlayer.CharacterSkillData[k].Index2 - 5].Reset(mainPlayer.CharacterSkillData[k], isTop: true);
			}
		}
		for (int l = 1; l < SkillBtnUseLogicList.Count; l++)
		{
			if (skillIndex == 0)
			{
				SkillBtnUseLogicList[l].UpdateDrageSurface(l + 3);
			}
			else
			{
				SkillBtnUseLogicList[l].UpdateDrageSurface(l + 6);
			}
		}
		mPlayerLevel = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
	}

	public void SelectSkill(string id, int newIndex)
	{
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer != null)
		{
			mainPlayer.ChangeSkillPosition(id, newIndex);
		}
		ResetBtn(curSkillIndex);
		if (SingletonUnity<JueseJiNengQuLogic>.Exists)
		{
			SingletonUnity<JueseJiNengQuLogic>.Instance.UpdateIcon();
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.SKILL_DRAG_MOVE)
		{
			CheckTutorialEvent();
		}
	}

	public void RemoveSkill(string id)
	{
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer != null)
		{
			mainPlayer.RemoveSkillPosition(id);
		}
		ResetBtn(curSkillIndex);
		if (SingletonUnity<JueseJiNengQuLogic>.Exists)
		{
			SingletonUnity<JueseJiNengQuLogic>.Instance.UpdateIcon();
		}
	}

	public void Reset()
	{
		ResetBtn(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SkillIndex);
		SkillBtnLogicList[curSelect].OnClickBtn();
	}

	public void SyncPage()
	{
		if (!UnityVersionUtil.IsActive(base.gameObject))
		{
			return;
		}
		ResetBtn(curSkillIndex);
		for (int i = 0; i < SkillBtnLogicList.Count; i++)
		{
			if (SkillBtnLogicList[i].CharacterSkillData != null && SkillBtnLogicList[i].SkillId.Equals(mCurChaSkillData.ID))
			{
				PlaySkillLevelAnim(SkillBtnLogicList[i].CharacterSkillData.Level);
				SkillBtnLogicList[i].OnClickBtn();
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Skill", $"skill_{mCurChaSkillData.ID}", $"skilllevel_{mCurChaSkillData.Level + 1}");
				break;
			}
		}
	}

	public void OnClickPlaySkillBtn()
	{
		if (mCurChaSkillData != null)
		{
			NGUITools.SetActive(PlayerModelPic.gameObject, state: true);
			mPlayerModelVisual.UseSkill(mCurChaSkillData.ID, delegate
			{
				NGUITools.SetActive(PlayerModelPic.gameObject, state: false);
			});
		}
	}

	public void OnClickCloseAnimaBtn()
	{
		NGUITools.SetActive(PlayerModelPic.gameObject, state: false);
	}

	public void OnClickPlan1Btn()
	{
		ResetBtn(0);
	}

	public void OnClickPlan2Btn()
	{
		ResetBtn(1);
	}
}
