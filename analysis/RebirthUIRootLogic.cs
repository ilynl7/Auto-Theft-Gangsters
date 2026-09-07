using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class RebirthUIRootLogic : SingletonUnity<RebirthUIRootLogic>
{
	public UILabel TimeLabel;

	public UILabel ItemNumLabel;

	public UISprite IconPic;

	public UISprite IconQuality;

	public UILabel InPlaceBtnLabel;

	public UILabel ReturnBtnLabel;

	private REBIRTH_TYPE mCurRebirthType;

	private int mCurItemCost;

	private string mCurItemId;

	private int mCurItemNum;

	private int mWaitTime = 10;

	private int mCurTimeCount;

	private float mStartTime;

	private PlayerData mPlayerData;

	private bool IsBtnEnable;

	public UISprite BackCityBtn;

	public Transform BackTra;

	public Transform relifeTra;

	public Transform AddFoeTra;

	public UILabel InfoLabel;

	public GameObject ShopBtn;

	public GameObject BigSaleBtn;

	public GameObject SlotBtn;

	public GameObject WelfareBtn;

	public GameObject MysteryBtn;

	public GameObject SkillBtn;

	public GameObject CustomizeBtn;

	public GameObject StrengthBtn;

	public GameObject InhertBtn;

	public GameObject TopObj;

	public GameObject DownObj;

	public UIGrid TopGrid;

	public UIGrid DownGrid;

	private long KillID;

	private int tempTime;

	public void Reset(notice_relife_player.request request)
	{
		TutorialManager.CloseTutorial();
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		mCurRebirthType = (REBIRTH_TYPE)request.type;
		mCurItemCost = (int)request.cost;
		mCurItemId = request.itemId;
		mStartTime = Time.time;
		if (mCurRebirthType == REBIRTH_TYPE.MAIN_CITY_REBIRTH)
		{
			ReturnBtnLabel.text = StrDictionary.GetDictionaryString("#{100502}");
		}
		else
		{
			ReturnBtnLabel.text = StrDictionary.GetDictionaryString("#{100503}");
		}
		ItemData itemDataByID = DataManager.GetItemDataByID(mCurItemId);
		IconPic.spriteName = itemDataByID.BackPackIcon;
		IconQuality.spriteName = itemDataByID.QualityType.ToString();
		bool flag = false;
		KillID = -1L;
		if (request.HasCharacterid && !SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo.IsAlreadyEnemy(request.characterid))
		{
			KillID = request.characterid;
			flag = true;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP))
		{
			refershItemInfo();
			if (flag)
			{
				BackTra.transform.localPosition = new Vector3(-170f, -77f, 0f);
				UnityVersionUtil.SetActiveRecursive(relifeTra.transform.gameObject, state: true);
				relifeTra.transform.localPosition = new Vector3(0f, -77f, 0f);
				UnityVersionUtil.SetActiveRecursive(AddFoeTra.transform.gameObject, state: true);
				InfoLabel.text = StrDictionary.GetDictionaryString("#{103307}", request.name, request.name);
				AddFoeTra.transform.localPosition = new Vector3(170f, -77f, 0f);
			}
			else
			{
				BackTra.transform.localPosition = new Vector3(-99f, -77f, 0f);
				UnityVersionUtil.SetActiveRecursive(relifeTra.transform.gameObject, state: true);
				relifeTra.transform.localPosition = new Vector3(125f, -77f, 0f);
				UnityVersionUtil.SetActiveRecursive(AddFoeTra.transform.gameObject, state: false);
				InfoLabel.text = string.Empty;
			}
			mCurTimeCount = mWaitTime;
			TimeLabel.text = $"{mCurTimeCount}s";
			IsBtnEnable = false;
			BackCityBtn.spriteName = GameDefine.BtnIcon[2];
		}
		else
		{
			List<GameItem> itemByItemId = mPlayerData.ItemBackPack.GetItemByItemId(mCurItemId);
			mCurItemNum = 0;
			for (int i = 0; i < itemByItemId.Count; i++)
			{
				mCurItemNum += itemByItemId[i].StackNum;
			}
			ItemNumLabel.text = $"{mCurItemCost}/{mCurItemNum}";
			if (mCurItemCost <= mCurItemNum)
			{
				ItemNumLabel.color = Color.white;
				if (flag)
				{
					BackTra.transform.localPosition = new Vector3(-170f, -77f, 0f);
					UnityVersionUtil.SetActiveRecursive(relifeTra.transform.gameObject, state: true);
					relifeTra.transform.localPosition = new Vector3(0f, -77f, 0f);
					UnityVersionUtil.SetActiveRecursive(AddFoeTra.transform.gameObject, state: true);
					InfoLabel.text = StrDictionary.GetDictionaryString("#{103307}", request.name, request.name);
					AddFoeTra.transform.localPosition = new Vector3(170f, -77f, 0f);
				}
				else
				{
					BackTra.transform.localPosition = new Vector3(-99f, -77f, 0f);
					UnityVersionUtil.SetActiveRecursive(relifeTra.transform.gameObject, state: true);
					relifeTra.transform.localPosition = new Vector3(125f, -77f, 0f);
					UnityVersionUtil.SetActiveRecursive(AddFoeTra.transform.gameObject, state: false);
					InfoLabel.text = string.Empty;
				}
				mCurTimeCount = mWaitTime;
				TimeLabel.text = $"{mCurTimeCount}s";
				IsBtnEnable = false;
				BackCityBtn.spriteName = GameDefine.BtnIcon[2];
			}
			else
			{
				mCurTimeCount = 0;
				TimeLabel.text = string.Empty;
				IsBtnEnable = true;
				BackCityBtn.spriteName = GameDefine.BtnIcon[0];
				if (flag)
				{
					BackTra.transform.localPosition = new Vector3(-99f, -77f, 0f);
					UnityVersionUtil.SetActiveRecursive(relifeTra.transform.gameObject, state: false);
					UnityVersionUtil.SetActiveRecursive(AddFoeTra.transform.gameObject, state: true);
					InfoLabel.text = StrDictionary.GetDictionaryString("#{103307}", request.name, request.name);
					AddFoeTra.transform.localPosition = new Vector3(99f, -77f, 0f);
				}
				else
				{
					ItemNumLabel.color = Color.red;
					BackTra.transform.localPosition = new Vector3(0f, -63f, 0f);
					UnityVersionUtil.SetActiveRecursive(relifeTra.transform.gameObject, state: false);
					UnityVersionUtil.SetActiveRecursive(AddFoeTra.transform.gameObject, state: false);
					InfoLabel.text = string.Empty;
				}
			}
		}
		LocalDataSaveManager.SetDiedFlag(1);
		UpdateBtnInfo();
	}

	public void OnClickInPlaceBtn()
	{
		if (mCurItemNum >= mCurItemCost)
		{
			relife_player.request request = new relife_player.request();
			request.isInplace = true;
			NetLogic.GetInstance().Send<Protocol.relife_player>(request);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RebirthUIRoot);
		}
		else
		{
			GameMoneyHelper.ShowItemProductTop(mCurItemId, GameDefine.SHOP_TYPE.EQUIP_SHOP);
		}
	}

	public void OnClickBackRebirthBtn()
	{
		if (IsBtnEnable)
		{
			relife_player.request request = new relife_player.request();
			request.isInplace = false;
			NetLogic.GetInstance().Send<Protocol.relife_player>(request);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RebirthUIRoot);
		}
	}

	public void OnClickAddFoeBtn()
	{
		if (KillID != -1)
		{
			FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
			if (friendInfo.IsCanAddEnemy() && !friendInfo.IsAlreadyEnemy(KillID))
			{
				NoticeLogic.AddNotifyData("#{103320}");
				add_friend.request request = new add_friend.request();
				request.characterId = KillID;
				request.type = 1L;
				NetLogic.GetInstance().Send<Protocol.add_friend>(request);
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Social", "Enemy", "add_times");
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100265}");
			}
		}
	}

	public void refershItemInfo()
	{
		List<GameItem> itemByItemId = mPlayerData.ItemBackPack.GetItemByItemId(mCurItemId);
		mCurItemNum = 0;
		for (int i = 0; i < itemByItemId.Count; i++)
		{
			mCurItemNum += itemByItemId[i].StackNum;
		}
		ItemNumLabel.text = $"{mCurItemCost}/{mCurItemNum}";
		if (mCurItemCost <= mCurItemNum)
		{
			ItemNumLabel.color = Color.white;
		}
		else
		{
			ItemNumLabel.color = Color.red;
		}
	}

	private void Update()
	{
		if (IsBtnEnable)
		{
			return;
		}
		tempTime = (int)(Time.time - mStartTime);
		if (mCurTimeCount != mWaitTime - tempTime)
		{
			mCurTimeCount = mWaitTime - tempTime;
			if (mCurTimeCount < 0)
			{
				IsBtnEnable = true;
				BackCityBtn.spriteName = GameDefine.BtnIcon[0];
				TimeLabel.text = string.Empty;
			}
			else
			{
				TimeLabel.text = $"{mCurTimeCount}s";
			}
		}
	}

	private void BackToRebirth()
	{
		relife_player.request request = new relife_player.request();
		request.isInplace = false;
		NetLogic.GetInstance().Send<Protocol.relife_player>(request);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RebirthUIRoot);
	}

	public void UpdateBtnInfo()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		NGUITools.SetActive(TopObj.gameObject, state: true);
		NGUITools.SetActive(DownObj.gameObject, state: true);
		bool flag = false;
		bool flag2 = false;
		if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SHOP))
		{
			NGUITools.SetActive(ShopBtn.gameObject, state: true);
			flag = true;
		}
		else
		{
			NGUITools.SetActive(ShopBtn.gameObject, state: false);
		}
		if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.BIGSALES) && playerCommonData.Big_PackFlag)
		{
			NGUITools.SetActive(BigSaleBtn.gameObject, state: true);
			flag = true;
		}
		else
		{
			NGUITools.SetActive(BigSaleBtn.gameObject, state: false);
		}
		if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.LOTTO))
		{
			NGUITools.SetActive(SlotBtn.gameObject, state: true);
			flag = true;
		}
		else
		{
			NGUITools.SetActive(SlotBtn.gameObject, state: false);
		}
		if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GIFT))
		{
			NGUITools.SetActive(WelfareBtn.gameObject, state: true);
			flag = true;
		}
		else
		{
			NGUITools.SetActive(WelfareBtn.gameObject, state: false);
		}
		if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.MYSTERYSHOP))
		{
			if (playerCommonData.Push != -1 && (playerCommonData.Push & 0x10) == 0L)
			{
				NGUITools.SetActive(MysteryBtn.gameObject, state: false);
			}
			else
			{
				NGUITools.SetActive(MysteryBtn.gameObject, state: true);
				flag = true;
			}
		}
		else
		{
			NGUITools.SetActive(MysteryBtn.gameObject, state: false);
		}
		if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SKILL) && CheckSkillUpdateTips())
		{
			NGUITools.SetActive(SkillBtn.gameObject, state: true);
			flag2 = true;
		}
		else
		{
			NGUITools.SetActive(SkillBtn.gameObject, state: false);
		}
		if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ENHANCE_EQUIP) && playerData.IsHaveEnhanceTips())
		{
			NGUITools.SetActive(CustomizeBtn.gameObject, state: true);
			flag2 = true;
		}
		else
		{
			NGUITools.SetActive(CustomizeBtn.gameObject, state: false);
		}
		if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ENHANCE_STAR) && playerData.IsHaveRefineTips())
		{
			NGUITools.SetActive(StrengthBtn.gameObject, state: true);
			flag2 = true;
		}
		else
		{
			NGUITools.SetActive(StrengthBtn.gameObject, state: false);
		}
		if (playerCommonData.IsFunctionUnlock(FUNCTION_TYPE.CHARACTER_EQUIP) && playerData.IsHaveCanInherit())
		{
			NGUITools.SetActive(InhertBtn.gameObject, state: true);
			flag2 = true;
		}
		else
		{
			NGUITools.SetActive(InhertBtn.gameObject, state: false);
		}
		TopGrid.Reposition();
		DownGrid.Reposition();
		if (!flag)
		{
			NGUITools.SetActive(TopObj.gameObject, state: false);
		}
		if (!flag2)
		{
			NGUITools.SetActive(DownObj.gameObject, state: false);
		}
	}

	public void OnClickBigSaleBtn()
	{
		UIManager.SetSpecialType(UIManager.SHOW_TYPE.BIGSALE);
		BackToRebirth();
	}

	public void OnClickShopBtn()
	{
		UIManager.SetSpecialType(UIManager.SHOW_TYPE.SHOP);
		BackToRebirth();
	}

	public void OnClickSlotBtn()
	{
		UIManager.SetSpecialType(UIManager.SHOW_TYPE.SLOT);
		BackToRebirth();
	}

	public void OnClickWelfareBtn()
	{
		UIManager.SetSpecialType(UIManager.SHOW_TYPE.WELFARE);
		BackToRebirth();
	}

	public void OnClickMysteryBtn()
	{
		UIManager.SetSpecialType(UIManager.SHOW_TYPE.MYSTERY);
		BackToRebirth();
	}

	public void OnClickSkillBtn()
	{
		UIManager.SetSpecialType(UIManager.SHOW_TYPE.SKILL);
		BackToRebirth();
	}

	public void OnClickCustomizeBtn()
	{
		UIManager.SetSpecialType(UIManager.SHOW_TYPE.ENHANCE_EQUIP);
		BackToRebirth();
	}

	public void OnClickStrengthBtn()
	{
		UIManager.SetSpecialType(UIManager.SHOW_TYPE.ENHANCE_STAR);
		BackToRebirth();
	}

	public void OnClickInhertBtn()
	{
		UIManager.SetSpecialType(UIManager.SHOW_TYPE.INHERT);
		BackToRebirth();
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
			CharacterSkillData characterSkillData = mainPlayer.CharacterSkillData[i];
			if (characterSkillData == null || !SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(characterSkillData.UnlockLevel))
			{
				continue;
			}
			int index = characterSkillData.Index;
			if (index >= 4 && index <= 6)
			{
				SkillData skillDataById = DataManager.GetSkillDataById(characterSkillData.ID);
				SkillupgradeData skillupgradeDataByLevel = DataManager.GetSkillupgradeDataByLevel(characterSkillData.Level + 1);
				if (skillDataById != null && skillupgradeDataByLevel != null && skillDataById.IsUpgrade != 0 && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level > characterSkillData.Level + 1 && GameMoneyHelper.GetMoneyNum(skillupgradeDataByLevel.PriceType) > skillupgradeDataByLevel.PriceValue)
				{
					return true;
				}
			}
		}
		return false;
	}
}
