using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class AppraiseRootLogic : MonoBehaviour
{
	private GameItem CurItem;

	public UISprite ItemIcon;

	public UISprite ItemQualityIcon;

	public UILabel ItemNameLabel;

	public UILabel LevelLabel;

	public UILabel LevelNameLabel;

	public UILabel ProfessionNameLabel;

	public UILabel ProfessionLabel;

	public UILabel DescLabel;

	public UILabel AppraisePriceLabel;

	public UILabel RecycleLabel;

	public GameObject AppraiseBtnObj;

	public GameObject recycleBtn;

	public GameObject BaseRoot;

	public UIGrid BaseAttributeGrid;

	public List<InfoLineItemLogic> BaseList;

	private int AppraisePrice;

	public void UpdateInfo(GameItem item, ITEM_SHOW_TYPE showType)
	{
		CurItem = item;
		ItemData itemData = CurItem.ItemData;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ItemIcon.spriteName = itemData.BackPackIcon;
		ItemQualityIcon.spriteName = CurItem.GetItemQuality().ToString();
		ItemNameLabel.text = itemData.MName;
		ItemNameLabel.color = GameDefine.GetColorByQuality(CurItem.GetItemQuality());
		EquipData equipDataById = DataManager.GetEquipDataById(CurItem.ItemId);
		if (CurItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			LevelNameLabel.text = StrDictionary.GetDictionaryString("#{100682}");
			LevelLabel.text = equipDataById.Class.ToString();
			SetLabelWarning(LevelLabel, istrue: false);
		}
		else
		{
			LevelNameLabel.text = StrDictionary.GetDictionaryString("#{100606}");
			LevelLabel.text = itemData.Level.ToString();
			SetLabelWarning(LevelLabel, playerData.Level < itemData.Level);
		}
		if (CurItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP && CurItem.ItemData.SubType == 0)
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
		ShowBaseAtt(CurItem);
		if (itemData.CanSell())
		{
			RecycleLabel.text = GameMoneyHelper.GetMoneyValStr(itemData.GetSellPrice(CurItem.GetItemQuality()), itemData.PriceType);
			UnityVersionUtil.SetActiveRecursive(RecycleLabel.gameObject, state: true);
		}
		else
		{
			RecycleLabel.text = string.Empty;
			UnityVersionUtil.SetActiveRecursive(RecycleLabel.gameObject, state: false);
		}
		DescLabel.text = StrDictionary.GetDictionaryString("#{100666}");
		if (showType == ITEM_SHOW_TYPE.BACKPACK)
		{
			NGUITools.SetActive(AppraiseBtnObj, state: true);
			AppraisePrice = equipDataById.GetAppraisePrice(CurItem.GetItemQuality());
			if (AppraisePrice != 0)
			{
				AppraisePriceLabel.text = GameMoneyHelper.GetMoneyValStr(AppraisePrice, GameDefine.MONEY_TYPE.GOLD);
			}
			else
			{
				AppraisePriceLabel.text = string.Empty;
			}
			NGUITools.SetActive(recycleBtn, state: true);
		}
		else
		{
			NGUITools.SetActive(AppraiseBtnObj, state: false);
			NGUITools.SetActive(recycleBtn, state: false);
		}
	}

	public void ShowBaseAtt(GameItem mCurItem)
	{
		EquipData equipDataById = DataManager.GetEquipDataById(mCurItem.ItemId);
		int num = equipDataById.GetBaseAttCount() - BaseList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(BaseList[0].gameObject) as GameObject;
				InfoLineItemLogic component = gameObject.GetComponent<InfoLineItemLogic>();
				gameObject.name = $"ItemInfoNameLabel{BaseList.Count:D2}";
				gameObject.transform.parent = BaseAttributeGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				BaseList.Add(component);
			}
		}
		for (int j = 0; j < BaseList.Count; j++)
		{
			if (j < equipDataById.GetBaseAttCount())
			{
				UnityVersionUtil.SetActiveRecursive(BaseList[j].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(BaseList[j].gameObject, state: false);
			}
		}
		bool flag = true;
		if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			flag = false;
		}
		else if (mCurItem.ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			flag = true;
		}
		int[] array = new int[4] { -1, -1, -1, -1 };
		if (equipDataById.BaseStatusType != 0)
		{
			string attributeIcon = GameDefine.GetAttributeIcon((int)equipDataById.BaseStatusType);
			string attributeName_S = GameDefine.GetAttributeName_S((int)equipDataById.BaseStatusType);
			array[0] = equipDataById.GetAttrValueByQuality(0, (int)mCurItem.GetItemQuality());
			string attributeValueStr = GameDefine.GetAttributeValueStr((int)equipDataById.BaseStatusType, array[0]);
			string enhancestr = string.Empty;
			if (flag)
			{
				enhancestr = string.Format("({0} +{1})", StrDictionary.GetDictionaryString("#{100934}"), equipDataById.GetAttrEnhanceVal(0, (int)mCurItem.GetItemQuality(), mCurItem.ItemLevel));
			}
			BaseList[0].ResetBase(attributeIcon, attributeName_S, attributeValueStr, enhancestr);
		}
		if (equipDataById.Status1 != 0)
		{
			string attributeIcon2 = GameDefine.GetAttributeIcon(equipDataById.Status1);
			string attributeName_S2 = GameDefine.GetAttributeName_S(equipDataById.Status1);
			array[1] = equipDataById.GetAttrValueByQuality(1, (int)mCurItem.GetItemQuality());
			string attributeValueStr2 = GameDefine.GetAttributeValueStr(equipDataById.Status1, array[1]);
			string enhancestr2 = string.Empty;
			if (flag)
			{
				enhancestr2 = string.Format("({0} +{1})", StrDictionary.GetDictionaryString("#{100934}"), equipDataById.GetAttrEnhanceVal(1, (int)mCurItem.GetItemQuality(), mCurItem.ItemLevel));
			}
			BaseList[1].ResetBase(attributeIcon2, attributeName_S2, attributeValueStr2, enhancestr2);
		}
		if (equipDataById.Status2 != 0)
		{
			string attributeIcon3 = GameDefine.GetAttributeIcon(equipDataById.Status2);
			string attributeName_S3 = GameDefine.GetAttributeName_S(equipDataById.Status2);
			array[2] = equipDataById.GetAttrValueByQuality(2, (int)mCurItem.GetItemQuality());
			string attributeValueStr3 = GameDefine.GetAttributeValueStr(equipDataById.Status2, array[2]);
			string enhancestr3 = string.Empty;
			if (flag)
			{
				enhancestr3 = string.Format("({0} +{1})", StrDictionary.GetDictionaryString("#{100934}"), equipDataById.GetAttrEnhanceVal(2, (int)mCurItem.GetItemQuality(), mCurItem.ItemLevel));
			}
			BaseList[2].ResetBase(attributeIcon3, attributeName_S3, attributeValueStr3, enhancestr3);
		}
		if (equipDataById.ExStatus != 0)
		{
			string attributeIcon4 = GameDefine.GetAttributeIcon(equipDataById.ExStatus);
			string attributeName_S4 = GameDefine.GetAttributeName_S(equipDataById.ExStatus);
			array[3] = equipDataById.GetAttrValueByQuality(3, (int)mCurItem.GetItemQuality());
			string attributeValueStr4 = GameDefine.GetAttributeValueStr(equipDataById.ExStatus, array[3]);
			string enhancestr4 = string.Empty;
			if (flag)
			{
				enhancestr4 = string.Format("({0} +{1})", StrDictionary.GetDictionaryString("#{100934}"), equipDataById.GetAttrEnhanceVal(3, (int)mCurItem.GetItemQuality(), mCurItem.ItemLevel));
			}
			BaseList[3].ResetBase(attributeIcon4, attributeName_S4, attributeValueStr4, enhancestr4);
		}
		BaseAttributeGrid.Reposition();
	}

	public void OnClickRecycleBtn()
	{
		int num = (int)((float)(CurItem.ItemData.GetSellPrice(CurItem.GetItemQuality()) * CurItem.StackNum) * 1f);
		MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{100636}", num), StrDictionary.GetDictionaryString("#{100127}"), delegate
		{
			if (CurItem.ItemData.Type != GameDefine.ITEM_TYPE.BADGE)
			{
				sell_item.request rpcReq = new sell_item.request
				{
					indexId = CurItem.IndexId,
					itemCount = CurItem.StackNum,
					type = (long)CurItem.ContainerType
				};
				NetLogic.GetInstance().Send<Protocol.sell_item>(rpcReq);
			}
		});
		SingletonUnity<ItemInfoRootLogicNew>.Instance.OnClickCloseBtn();
	}

	public void OnClickAppraiseBtn()
	{
		EquipData equipDataById = DataManager.GetEquipDataById(CurItem.ItemId);
		if (GameMoneyHelper.BeforeCheckBuy(GameDefine.MONEY_TYPE.GOLD, AppraisePrice))
		{
			equip_appraise.request request = new equip_appraise.request();
			request.index = CurItem.IndexId;
			NetLogic.GetInstance().Send<Protocol.equip_appraise>(request);
		}
		SingletonUnity<ItemInfoRootLogicNew>.Instance.OnClickCloseBtn();
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
