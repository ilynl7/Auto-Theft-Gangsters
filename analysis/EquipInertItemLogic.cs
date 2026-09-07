using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class EquipInertItemLogic : MonoBehaviour
{
	public delegate void AutoSelectDelegate(bool isequiped, string key);

	public UISprite ItemIcon;

	public UISprite ItemQualityIcon;

	public UILabel ItemNameLabel;

	public UILabel ItemEnhanceLabel;

	public UILabel LevelLabel;

	public UILabel ProfessionNameLabel;

	public UILabel ProfessionLabel;

	public UISprite IsEquipedSprite;

	public List<UISprite> StarList;

	public UIGrid AttParentGrid;

	public List<InfoLineSelectItemLogic> SelectItemList;

	public UIGrid SkillParentGrid;

	public List<InfoLineSelectItemLogic> SelectSkillItemList;

	public int CurSelectIndex = -1;

	private string CurSelectkey = string.Empty;

	public bool IsWeaponFlag;

	private bool IsEquipped;

	private DelegateDefine.OneIntParamDelegate UpdatePriceFun;

	private AutoSelectDelegate AutoSelectFun;

	public string GetSelectKey => CurSelectkey;

	public void Reset(GameItem mCurItem, bool isEquiped, DelegateDefine.OneIntParamDelegate updatefun = null, AutoSelectDelegate selectfun = null)
	{
		UpdatePriceFun = updatefun;
		AutoSelectFun = selectfun;
		IsEquipped = isEquiped;
		ItemData itemData = mCurItem.ItemData;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		CurSelectIndex = -1;
		CurSelectkey = string.Empty;
		ItemNameLabel.text = itemData.MName;
		ItemIcon.spriteName = itemData.BackPackIcon;
		LevelLabel.text = itemData.Level.ToString();
		SetLabelWarning(LevelLabel, playerData.Level < itemData.Level);
		ItemQualityIcon.spriteName = mCurItem.GetItemQuality().ToString();
		int itemCombatVal = mCurItem.GetItemCombatVal();
		ItemEnhanceLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100664}"), itemCombatVal);
		ItemNameLabel.color = GameDefine.GetColorByQuality(mCurItem.GetItemQuality());
		EquipData equipDataById = DataManager.GetEquipDataById(mCurItem.ItemId);
		if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP && mCurItem.ItemData.SubType == 0)
		{
			ProfessionLabel.text = StrDictionary.GetDictionaryString(GameDefine.WeaponName[equipDataById.WeaponType]);
			SetLabelWarning(ProfessionLabel, istrue: false);
			ProfessionNameLabel.text = StrDictionary.GetDictionaryString("#{101219}");
		}
		else
		{
			ProfessionLabel.text = StrDictionary.GetDictionaryString(GameDefine.ProfessionName[equipDataById.Job]);
			SetLabelWarning(ProfessionLabel, playerData.Profession != equipDataById.profession);
			ProfessionNameLabel.text = StrDictionary.GetDictionaryString("#{100607}");
		}
		if (IsEquipped)
		{
			IsEquipedSprite.alpha = 1f;
		}
		else
		{
			IsEquipedSprite.alpha = 0f;
		}
		if (mCurItem.ItemData.SubType == 0)
		{
			IsWeaponFlag = true;
			ShowSkillInfo(mCurItem, IsEquipped);
		}
		else
		{
			IsWeaponFlag = false;
			ShowAttInfo(mCurItem, IsEquipped);
		}
		ShowStar(mCurItem.GetStarByScore());
	}

	public void ShowAttInfo(GameItem mCurItem, bool isEquiped)
	{
		NGUITools.SetActive(AttParentGrid.gameObject, state: true);
		NGUITools.SetActive(SkillParentGrid.gameObject, state: false);
		if (isEquiped)
		{
			int num = 6 - SelectItemList.Count;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = Object.Instantiate(SelectItemList[0].gameObject) as GameObject;
					InfoLineSelectItemLogic component = gameObject.GetComponent<InfoLineSelectItemLogic>();
					gameObject.name = $"ItemInfoNameLabel{SelectItemList.Count:D2}";
					gameObject.transform.parent = AttParentGrid.transform;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					SelectItemList.Add(component);
				}
			}
			for (int j = 0; j < SelectItemList.Count; j++)
			{
				if (j < 6)
				{
					NGUITools.SetActive(SelectItemList[j].gameObject, state: true);
				}
				else
				{
					NGUITools.SetActive(SelectItemList[j].gameObject, state: false);
				}
			}
			for (int k = 0; k < SelectItemList.Count; k++)
			{
				SelectItemList[k].ResetAtt(GetindexAtt(mCurItem, k), k, mCurItem, OnClickSelectBtn);
			}
			AttParentGrid.Reposition();
		}
		else if (mCurItem.IsHaveRandomAtt)
		{
			int num2 = mCurItem.Random_AttriDic.Count - SelectItemList.Count;
			if (num2 > 0)
			{
				for (int l = 0; l < num2; l++)
				{
					GameObject gameObject2 = Object.Instantiate(SelectItemList[0].gameObject) as GameObject;
					InfoLineSelectItemLogic component2 = gameObject2.GetComponent<InfoLineSelectItemLogic>();
					gameObject2.name = $"ItemInfoNameLabel{SelectItemList.Count:D2}";
					gameObject2.transform.parent = AttParentGrid.transform;
					gameObject2.transform.localScale = Vector3.one;
					gameObject2.transform.localPosition = Vector3.zero;
					SelectItemList.Add(component2);
				}
			}
			List<random_attri> list = new List<random_attri>(mCurItem.Random_AttriDic.Values);
			for (int num3 = list.Count - 1; num3 >= 0; num3--)
			{
				if (list[num3].id == 0L)
				{
					list.RemoveAt(num3);
				}
			}
			list.Sort((random_attri x, random_attri y) => (int)x.index - (int)y.index);
			for (int m = 0; m < SelectItemList.Count; m++)
			{
				if (m < list.Count)
				{
					NGUITools.SetActive(SelectItemList[m].gameObject, state: true);
					SelectItemList[m].ResetAtt(list[m], (int)list[m].index - 1, mCurItem, OnClickSelectBtn);
				}
				else
				{
					NGUITools.SetActive(SelectItemList[m].gameObject, state: false);
				}
			}
			AttParentGrid.Reposition();
		}
		else
		{
			for (int n = 0; n < SelectItemList.Count; n++)
			{
				NGUITools.SetActive(SelectItemList[n].gameObject, state: false);
			}
		}
	}

	public void ShowSkillInfo(GameItem mCurItem, bool isEquiped)
	{
		NGUITools.SetActive(AttParentGrid.gameObject, state: false);
		NGUITools.SetActive(SkillParentGrid.gameObject, state: true);
		if (isEquiped)
		{
			int num = 6 - SelectSkillItemList.Count;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = Object.Instantiate(SelectSkillItemList[0].gameObject) as GameObject;
					InfoLineSelectItemLogic component = gameObject.GetComponent<InfoLineSelectItemLogic>();
					gameObject.name = $"ItemInfoNameLabel{SelectSkillItemList.Count:D2}";
					gameObject.transform.parent = SkillParentGrid.transform;
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					SelectSkillItemList.Add(component);
				}
			}
			for (int j = 0; j < SelectSkillItemList.Count; j++)
			{
				if (j < 6)
				{
					NGUITools.SetActive(SelectSkillItemList[j].gameObject, state: true);
				}
				else
				{
					NGUITools.SetActive(SelectSkillItemList[j].gameObject, state: false);
				}
			}
			for (int k = 0; k < SelectSkillItemList.Count; k++)
			{
				SelectSkillItemList[k].ResetSkill(GetindexAtt(mCurItem, k), k, mCurItem, OnClickSelectBtn);
			}
			SkillParentGrid.Reposition();
		}
		else if (mCurItem.IsHaveRandomAtt)
		{
			int num2 = mCurItem.Random_AttriDic.Count - SelectSkillItemList.Count;
			if (num2 > 0)
			{
				for (int l = 0; l < num2; l++)
				{
					GameObject gameObject2 = Object.Instantiate(SelectSkillItemList[0].gameObject) as GameObject;
					InfoLineSelectItemLogic component2 = gameObject2.GetComponent<InfoLineSelectItemLogic>();
					gameObject2.name = $"ItemInfoNameLabel{SelectSkillItemList.Count:D2}";
					gameObject2.transform.parent = SkillParentGrid.transform;
					gameObject2.transform.localScale = Vector3.one;
					gameObject2.transform.localPosition = Vector3.zero;
					SelectSkillItemList.Add(component2);
				}
			}
			List<random_attri> list = new List<random_attri>(mCurItem.Random_AttriDic.Values);
			for (int num3 = list.Count - 1; num3 >= 0; num3--)
			{
				if (string.IsNullOrEmpty(list[num3].skillId))
				{
					list.RemoveAt(num3);
				}
			}
			list.Sort((random_attri x, random_attri y) => (int)x.index - (int)y.index);
			for (int m = 0; m < SelectSkillItemList.Count; m++)
			{
				if (m < list.Count)
				{
					NGUITools.SetActive(SelectSkillItemList[m].gameObject, state: true);
					SelectSkillItemList[m].ResetSkill(list[m], (int)list[m].index - 1, mCurItem, OnClickSelectBtn);
				}
				else
				{
					NGUITools.SetActive(SelectSkillItemList[m].gameObject, state: false);
				}
			}
			SkillParentGrid.Reposition();
		}
		else
		{
			for (int n = 0; n < SelectSkillItemList.Count; n++)
			{
				NGUITools.SetActive(SelectSkillItemList[n].gameObject, state: false);
			}
		}
	}

	public void OnClickSelectBtn(int index, int quality, string selectkey, bool isAutoSelect)
	{
		if (CurSelectIndex != index)
		{
			CurSelectIndex = index;
			CurSelectkey = selectkey;
			UpdateSelectIndex();
			if (UpdatePriceFun != null)
			{
				UpdatePriceFun(quality);
			}
			if (isAutoSelect && AutoSelectFun != null)
			{
				AutoSelectFun(IsEquipped, selectkey);
			}
		}
	}

	public void ClearSelect()
	{
		CurSelectIndex = -1;
		CurSelectkey = string.Empty;
		UpdateSelectIndex();
	}

	public void SelectNeedKey(string key, bool needselect = false)
	{
		if (IsWeaponFlag)
		{
			if (needselect)
			{
				if (IsHaveKey(key))
				{
					for (int i = 0; i < SelectSkillItemList.Count && !SelectSkillItemList[i].AutoSelectKey(key); i++)
					{
					}
				}
				else if (CurSelectIndex == -1)
				{
					for (int j = 0; j < SelectSkillItemList.Count && !SelectSkillItemList[j].SelectEmpty(); j++)
					{
					}
				}
			}
			else
			{
				for (int k = 0; k < SelectSkillItemList.Count && !SelectSkillItemList[k].AutoSelectKey(key); k++)
				{
				}
			}
		}
		else if (needselect)
		{
			if (IsHaveKey(key))
			{
				for (int l = 0; l < SelectItemList.Count && !SelectItemList[l].AutoSelectKey(key); l++)
				{
				}
			}
			else if (CurSelectIndex == -1)
			{
				for (int m = 0; m < SelectItemList.Count && !SelectItemList[m].SelectEmpty(); m++)
				{
				}
			}
		}
		else
		{
			for (int n = 0; n < SelectItemList.Count && !SelectItemList[n].AutoSelectKey(key); n++)
			{
			}
		}
	}

	public bool IsHaveKey(string key)
	{
		if (IsWeaponFlag)
		{
			for (int i = 0; i < SelectSkillItemList.Count; i++)
			{
				if (SelectSkillItemList[i].IsEqualsKey(key))
				{
					return true;
				}
			}
		}
		else
		{
			for (int j = 0; j < SelectItemList.Count; j++)
			{
				if (SelectItemList[j].IsEqualsKey(key))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void UpdateSelectIndex()
	{
		if (IsWeaponFlag)
		{
			for (int i = 0; i < SelectSkillItemList.Count; i++)
			{
				SelectSkillItemList[i].UpdateSelect(CurSelectIndex);
			}
		}
		else
		{
			for (int j = 0; j < SelectItemList.Count; j++)
			{
				SelectItemList[j].UpdateSelect(CurSelectIndex);
			}
		}
	}

	public random_attri GetindexAtt(GameItem mCurItem, int index)
	{
		if (mCurItem.IsHaveRandomAtt)
		{
			List<random_attri> list = new List<random_attri>(mCurItem.Random_AttriDic.Values);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].index - 1 == index)
				{
					return list[i];
				}
			}
		}
		return null;
	}

	public void ShowStar(int star)
	{
		for (int i = 0; i < StarList.Count; i++)
		{
			if (i < star)
			{
				StarList[i].color = Color.white;
			}
			else
			{
				StarList[i].color = Color.black;
			}
		}
	}

	public void SetLabelWarning(UILabel curlabel, bool istrue)
	{
		if (istrue)
		{
			curlabel.color = Color.red;
		}
		else
		{
			curlabel.color = Color.white;
		}
	}
}
