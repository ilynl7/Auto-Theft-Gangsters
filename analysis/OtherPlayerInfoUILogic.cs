using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class OtherPlayerInfoUILogic : SingletonUnity<OtherPlayerInfoUILogic>
{
	public List<RewardItem> ItemShowList;

	public JSSXKuangUILogic jssInfo;

	private FakeObjLogic mCurFakeObj;

	public UIEventListener RotateModelBtnListener;

	public UITexture ModelPic;

	public UILabel NameLabel;

	public UILabel FightLabel;

	private CharacterAttributeData targetAttribute;

	private List<GameItem> mCurItemDataList = new List<GameItem>();

	private PROFESSION_TYPE mCurProfessionType;

	private string mModelName;

	private character_look CurCharacterLook;

	public GameObject EquipRoot;

	public GameObject FashionRoot;

	public GameObject BadgeRoot;

	public GameObject RefineRoot;

	public GameObject modelViewRoot;

	public List<RewardItem> FashionItemList;

	public List<RewardItem> BadgeItemList;

	public List<UILabel> RefineItemList;

	private List<GameItem> CurFashionItemList = new List<GameItem>();

	private List<GameItem> CurBadgeItemList = new List<GameItem>();

	public UITexture[] ItemEffects;

	public List<UILabel> BadgeAttributeName;

	public List<UISprite> BadgeAttributeICON;

	public List<UILabel> BadgeAttributeValue;

	public List<GameObject> BadgeAttributeObj;

	public UILabel BottomLabel;

	public UIGrid BadgeGrid;

	public UISprite[] ItemBtns;

	public UISprite[] SelectDir;

	public UIPlayTween leftEffect;

	private int curSelect = -1;

	protected List<CharacterSkillData> mCharacterSkillData = new List<CharacterSkillData>();

	private List<string> mPlayerSkillIDList;

	public List<CharacterSkillData> CharacterSkillData
	{
		get
		{
			return mCharacterSkillData;
		}
		set
		{
			mCharacterSkillData = value;
		}
	}

	public List<string> PlayerSkillIDList
	{
		get
		{
			return mPlayerSkillIDList;
		}
		set
		{
			mPlayerSkillIDList = value;
		}
	}

	public void ResetEnable()
	{
		leftEffect.resetOnPlay = true;
		leftEffect.Play(forward: true);
	}

	private void OnEnable()
	{
		curSelect = -1;
		ResetFakeObjRoot();
	}

	public void EnableReset()
	{
		UnityVersionUtil.SetActiveRecursive(FashionRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(BadgeRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(RefineRoot.gameObject, state: false);
	}

	public void ResetOtherPlayerInfo(List<GameItem> list, PROFESSION_TYPE Type, string ModelName, CharacterAttributeData data, character_look look)
	{
		targetAttribute = data;
		mCurItemDataList.Clear();
		mCurItemDataList = ItemContainerTool.GetEquipItemList(list);
		mCurProfessionType = Type;
		CurCharacterLook = look;
		mModelName = ModelName;
		CurFashionItemList.Clear();
		if (look.HasFashion_equip)
		{
			List<GameItem> gameItemList = GetGameItemList(new List<gameitem>(look.fashion_equip.Values));
			CurFashionItemList = ItemContainerTool.GetFashionEquipItemList(gameItemList);
		}
		CurBadgeItemList.Clear();
		if (look.HasBadge_equip)
		{
			List<GameItem> gameItemList2 = GetGameItemList(new List<gameitem>(look.badge_equip.Values));
			CurBadgeItemList = ItemContainerTool.GetBadgeEquipItemList(gameItemList2);
		}
		NameLabel.text = $"Lv:{targetAttribute.Level}  {targetAttribute.Name}";
		FightLabel.text = targetAttribute.ComboValue.ToString();
		jssInfo.Show(targetAttribute);
		OnClickEquipBtn();
		ResetModelVisual();
		if (CurCharacterLook.HasSkills)
		{
			UpdateSkillList(CurCharacterLook.skills);
		}
	}

	public void UpdateSkillList(Dictionary<string, skill_info> skills)
	{
		CharacterSkillData.Clear();
		if (skills == null)
		{
			return;
		}
		int count = skills.Count;
		foreach (KeyValuePair<string, skill_info> skill in skills)
		{
			CharacterSkillData.Add(new CharacterSkillData(skill.Value.skillId, (int)skill.Value.skillLevel, (int)skill.Value.indexPos, (int)skill.Value.indexPos2, skill.Value.disable));
		}
	}

	public int GetPlayerSkillLevelByPos(int skillpos)
	{
		for (int i = 0; i < CharacterSkillData.Count; i++)
		{
			if (CharacterSkillData[i].Index2 == skillpos)
			{
				return CharacterSkillData[i].Level;
			}
		}
		return 0;
	}

	private void UpdateSelectItemBtn(int selectid)
	{
		curSelect = selectid;
		for (int i = 0; i < ItemBtns.Length; i++)
		{
			if (selectid == i)
			{
				ItemBtns[i].alpha = 1f;
				SelectDir[i].enabled = true;
			}
			else
			{
				ItemBtns[i].alpha = 10f / 51f;
				SelectDir[i].enabled = false;
			}
		}
	}

	public void OnClickEquipBtn()
	{
		if (curSelect == 0)
		{
			return;
		}
		UnityVersionUtil.SetActiveRecursive(FashionRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(BadgeRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(RefineRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(EquipRoot.gameObject, state: true);
		UnityVersionUtil.SetActiveRecursive(modelViewRoot.gameObject, state: true);
		UpdateSelectItemBtn(0);
		for (int i = 0; i < ItemShowList.Count; i++)
		{
			ItemShowList[i].SetItemEmpty();
			if (i < mCurItemDataList.Count && mCurItemDataList[i] != null && !mCurItemDataList[i].IsEmpty())
			{
				int level = 0;
				if (mCurItemDataList[i].ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					level = targetAttribute.GetEquipEnhanceLevel(mCurItemDataList[i].ItemData.SubType);
				}
				ItemShowList[i].UpdateItem(mCurItemDataList[i], level, isEquipTips: true);
			}
		}
	}

	public void OnClickFashionBtn()
	{
		if (curSelect == 1)
		{
			return;
		}
		UnityVersionUtil.SetActiveRecursive(EquipRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(BadgeRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(RefineRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(FashionRoot.gameObject, state: true);
		UnityVersionUtil.SetActiveRecursive(modelViewRoot.gameObject, state: true);
		UpdateSelectItemBtn(1);
		for (int i = 0; i < FashionItemList.Count; i++)
		{
			FashionItemList[i].SetItemEmpty();
			if (i < CurFashionItemList.Count && CurFashionItemList[i] != null)
			{
				FashionItemList[i].UpdateItem(CurFashionItemList[i], 0);
			}
		}
	}

	public void OnClickBadgeBtn()
	{
		if (curSelect == 2)
		{
			return;
		}
		UnityVersionUtil.SetActiveRecursive(EquipRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(FashionRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(modelViewRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(RefineRoot.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(BadgeRoot.gameObject, state: true);
		UpdateSelectItemBtn(2);
		for (int i = 0; i < BadgeItemList.Count; i++)
		{
			BadgeItemList[i].SetItemEmpty();
			if (i < CurBadgeItemList.Count && CurBadgeItemList[i] != null)
			{
				BadgeItemList[CurBadgeItemList[i].Parm[0]].UpdateItem(CurBadgeItemList[i], 0);
			}
		}
		UpdateBageItem();
	}

	public void OnClickRefineBtn()
	{
		if (curSelect != 3)
		{
			UnityVersionUtil.SetActiveRecursive(EquipRoot.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(BadgeRoot.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(FashionRoot.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(modelViewRoot.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(RefineRoot.gameObject, state: true);
			RefineItemList[0].text = CurCharacterLook.attribute_other.refineNeckLevel.ToString();
			RefineItemList[1].text = CurCharacterLook.attribute_other.refineRing1Level.ToString();
			RefineItemList[2].text = CurCharacterLook.attribute_other.refineRing2Level.ToString();
			RefineItemList[3].text = CurCharacterLook.attribute_other.refineBeltLevel.ToString();
			UpdateSelectItemBtn(3);
			ResetEnable();
		}
	}

	private void UpdateBageItem()
	{
		int[] array = new int[3];
		int[] array2 = new int[3];
		int num = 1;
		int num2 = 0;
		for (int i = 0; i < CurBadgeItemList.Count; i++)
		{
			GameItem gameItem = CurBadgeItemList[i];
			if (gameItem != null && !gameItem.IsEmpty())
			{
				ItemData itemData = gameItem.ItemData;
				BadgeData badgeDataById = DataManager.GetBadgeDataById(gameItem.ItemId);
				array[badgeDataById.Color]++;
				array2[badgeDataById.Color] += badgeDataById.Lv;
				BadgeAttributeName[num].text = GameDefine.GetAttributeName_S(badgeDataById.Status1);
				BadgeAttributeICON[num].spriteName = GameDefine.GetAttributeIcon(badgeDataById.Status1);
				BadgeAttributeValue[num].text = GameDefine.GetAttributeValueStr(badgeDataById.Status1, badgeDataById.Value1);
				num2 += CurBadgeItemList[i].GetItemCombatVal();
				num++;
			}
		}
		int num3 = num;
		for (int j = 0; j < BadgeAttributeObj.Count; j++)
		{
			NGUITools.SetActive(BadgeAttributeObj[j], j < num3);
		}
		int num4 = -1;
		int num5 = 0;
		for (int k = 0; k < array.Length; k++)
		{
			if (array[k] >= 3)
			{
				num4 = k;
				num5 = array2[k];
				break;
			}
		}
		List<int> list = new List<int>();
		if (num4 > -1)
		{
			for (int l = 0; l < CurBadgeItemList.Count; l++)
			{
				GameItem gameItem2 = CurBadgeItemList[l];
				if (gameItem2 != null && !gameItem2.IsEmpty())
				{
					ItemData itemData2 = gameItem2.ItemData;
					BadgeData badgeDataById2 = DataManager.GetBadgeDataById(gameItem2.ItemId);
					if (badgeDataById2.Color == num4)
					{
						list.Add(l);
					}
				}
			}
		}
		for (int m = 0; m < ItemEffects.Length; m++)
		{
			if (list.Contains(m))
			{
				ItemEffects[m].alpha = 1f;
			}
			else
			{
				ItemEffects[m].alpha = 0f;
			}
		}
		NGUITools.SetActive(BadgeAttributeObj[0], num4 > -1);
		switch (num4)
		{
		case 1:
		{
			ConfigData configDataByKey5 = DataManager.GetConfigDataByKey("badge_parm_2");
			ConfigData configDataByKey6 = DataManager.GetConfigDataByKey("badge_attribute_2");
			if (configDataByKey5 != null)
			{
				int value3 = Mathf.FloorToInt((float)num5 / configDataByKey5.Valuef);
				BadgeAttributeName[0].text = GameDefine.GetAttributeName_S(configDataByKey6.Valuei);
				BadgeAttributeICON[0].spriteName = GameDefine.GetAttributeIcon(configDataByKey6.Valuei);
				BadgeAttributeValue[0].text = GameDefine.GetAttributeValueStr(configDataByKey6.Valuei, value3);
			}
			else
			{
				float num8 = Mathf.Pow(num5, 1.25f) / 3.94822f / 1000f;
				BadgeAttributeName[0].text = GameDefine.GetAttributeName_S(1010);
				BadgeAttributeICON[0].spriteName = GameDefine.GetAttributeIcon(1010);
				BadgeAttributeValue[0].text = $"+{num8:P1}";
			}
			break;
		}
		case 0:
		{
			ConfigData configDataByKey3 = DataManager.GetConfigDataByKey("badge_parm_1");
			ConfigData configDataByKey4 = DataManager.GetConfigDataByKey("badge_attribute_1");
			if (configDataByKey3 != null)
			{
				int value2 = Mathf.FloorToInt((float)num5 / configDataByKey3.Valuef);
				BadgeAttributeName[0].text = GameDefine.GetAttributeName_S(configDataByKey4.Valuei);
				BadgeAttributeICON[0].spriteName = GameDefine.GetAttributeIcon(configDataByKey4.Valuei);
				BadgeAttributeValue[0].text = GameDefine.GetAttributeValueStr(configDataByKey4.Valuei, value2);
			}
			else
			{
				float num7 = Mathf.Pow(num5, 0.5555f) / 1.8411f / 100f;
				BadgeAttributeName[0].text = GameDefine.GetAttributeName_S(1012);
				BadgeAttributeICON[0].spriteName = GameDefine.GetAttributeIcon(1012);
				BadgeAttributeValue[0].text = $"+{num7:P1}";
			}
			break;
		}
		case 2:
		{
			ConfigData configDataByKey = DataManager.GetConfigDataByKey("badge_parm_3");
			ConfigData configDataByKey2 = DataManager.GetConfigDataByKey("badge_attribute_3");
			if (configDataByKey != null)
			{
				int value = Mathf.FloorToInt((float)num5 / configDataByKey.Valuef);
				BadgeAttributeName[0].text = GameDefine.GetAttributeName_S(configDataByKey2.Valuei);
				BadgeAttributeICON[0].spriteName = GameDefine.GetAttributeIcon(configDataByKey2.Valuei);
				BadgeAttributeValue[0].text = GameDefine.GetAttributeValueStr(configDataByKey2.Valuei, value);
			}
			else
			{
				float num6 = Mathf.Pow(num5, 0.5f) / 1.73205f * 0.5f;
				BadgeAttributeName[0].text = GameDefine.GetAttributeName_S(1011);
				BadgeAttributeICON[0].spriteName = GameDefine.GetAttributeIcon(1011);
				BadgeAttributeValue[0].text = $"+{num6:F1}";
			}
			break;
		}
		}
		BadgeGrid.Reposition();
	}

	private List<GameItem> GetGameItemList(List<gameitem> list)
	{
		List<GameItem> list2 = new List<GameItem>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != null)
			{
				GameItem gameItem = ServerToClientTools.ServerGameItemToClientGameItem(list[i]);
				if (gameItem != null)
				{
					list2.Add(gameItem);
				}
			}
		}
		return list2;
	}

	private void ResetFakeObjRoot()
	{
		RotateModelBtnListener.onDrag = OnDragModelBtn;
		FakeObjOtherPlayer instance = SingletonUnity<FakeObjOtherPlayer>.Instance;
		if (instance == null)
		{
			ResourcesManager.LoadAndInstantiate("UIRoot/FakeObjOtherPlayerRoot");
			instance = SingletonUnity<FakeObjOtherPlayer>.Instance;
		}
		SingletonUnity<FakeObjOtherPlayer>.Instance.EnableFakeObjRoot();
		ModelPic.mainTexture = instance.ModelPic;
	}

	private void ResetModelVisual()
	{
		if (mCurFakeObj != null)
		{
			mCurFakeObj.DestroyFakeObj();
		}
		if (mCurFakeObj == null || mCurFakeObj.FakeObj == null)
		{
			mCurFakeObj = new FakeObjLogic();
			mCurFakeObj.InitFakeObject(CurCharacterLook.visual, mCurProfessionType.ToString(), mModelName, SingletonUnity<FakeObjOtherPlayer>.Instance.MeshRoot, null, "FakeObj2");
		}
		else
		{
			mCurFakeObj.CheckFakeObject(CurCharacterLook.visual);
			mCurFakeObj.PlayAnim("idle", mModelName);
		}
	}

	public void UnLoadFakeObj()
	{
		if (mCurFakeObj != null)
		{
			mCurFakeObj.DestroyFakeObj();
			mCurFakeObj = null;
		}
		if (SingletonUnity<FakeObjOtherPlayer>.Instance != null)
		{
			SingletonUnity<FakeObjOtherPlayer>.Instance.DisableFakeObjRoot();
		}
	}

	private void OnDragModelBtn(GameObject btn, Vector2 delta)
	{
		if (mCurFakeObj != null && mCurFakeObj.FakeObj != null)
		{
			mCurFakeObj.FakeObj.transform.localEulerAngles -= delta.x * Vector3.up;
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PlayerInfoMenuPlayerInfoRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.OtherPlayerInfoUILogicRoot);
		UnLoadFakeObj();
	}

	private void OnDisable()
	{
		UnLoadFakeObj();
	}
}
