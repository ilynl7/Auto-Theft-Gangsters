using SprotoType;

public class EquipInhertRootLogic : SingletonUnity<EquipInhertRootLogic>
{
	public EquipInertItemLogic leftInhert;

	public EquipInertItemLogic rightInhert;

	public UILabel PriceLabel;

	public GameItem leftitem;

	public GameItem rightitem;

	public bool IsWeaponFlag;

	private int NeedPrice;

	public UILabel LeftInfoLabel;

	public UILabel RightInfoLabel;

	public void ShowInfo(GameItem gameItem, GameItem equipedItem)
	{
		leftitem = equipedItem;
		rightitem = gameItem;
		if (gameItem.ItemData.SubType == 0)
		{
			IsWeaponFlag = true;
			LeftInfoLabel.text = StrDictionary.GetDictionaryString("#{100672}");
			RightInfoLabel.text = StrDictionary.GetDictionaryString("#{100673}");
		}
		else
		{
			IsWeaponFlag = false;
			LeftInfoLabel.text = StrDictionary.GetDictionaryString("#{100679}");
			RightInfoLabel.text = StrDictionary.GetDictionaryString("#{100680}");
		}
		leftInhert.Reset(equipedItem, isEquiped: true, null, AutoSelect);
		rightInhert.Reset(gameItem, isEquiped: false, UpdatePrice, AutoSelect);
		PriceLabel.text = GameMoneyHelper.GetMoneyValStr(0, GameDefine.MONEY_TYPE.GOLD);
		NeedPrice = 0;
	}

	public void OnClickInhertBtn()
	{
		if (leftInhert.CurSelectIndex != -1 && rightInhert.CurSelectIndex != -1 && GameMoneyHelper.BeforeCheckBuy(GameDefine.MONEY_TYPE.GOLD, NeedPrice))
		{
			if (IsWeaponFlag)
			{
				weapon_inhert.request request = new weapon_inhert.request();
				request.index1 = leftitem.IndexId;
				request.index2 = rightitem.IndexId;
				request.attribute_index1 = leftInhert.CurSelectIndex + 1;
				request.attribute_index2 = rightInhert.CurSelectIndex + 1;
				NetLogic.GetInstance().Send<Protocol.weapon_inhert>(request);
			}
			else
			{
				attribute_inhert.request request2 = new attribute_inhert.request();
				request2.index1 = leftitem.IndexId;
				request2.index2 = rightitem.IndexId;
				request2.attribute_index1 = leftInhert.CurSelectIndex + 1;
				request2.attribute_index2 = rightInhert.CurSelectIndex + 1;
				NetLogic.GetInstance().Send<Protocol.attribute_inhert>(request2);
			}
		}
	}

	public void AutoSelect(bool isequiped, string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			if (isequiped)
			{
				if (!string.IsNullOrEmpty(rightInhert.GetSelectKey) && leftInhert.IsHaveKey(rightInhert.GetSelectKey))
				{
					rightInhert.ClearSelect();
				}
			}
			else if (!string.IsNullOrEmpty(leftInhert.GetSelectKey) && rightInhert.IsHaveKey(leftInhert.GetSelectKey))
			{
				leftInhert.ClearSelect();
			}
		}
		else if (isequiped)
		{
			if (!string.IsNullOrEmpty(rightInhert.GetSelectKey) && leftInhert.IsHaveKey(rightInhert.GetSelectKey) && !key.Equals(rightInhert.GetSelectKey))
			{
				rightInhert.ClearSelect();
			}
			rightInhert.SelectNeedKey(key);
		}
		else
		{
			leftInhert.SelectNeedKey(key, needselect: true);
		}
	}

	public void UpdateInhertItem(GameItem newitem)
	{
		if (newitem != null && leftitem.IndexId == newitem.IndexId)
		{
			leftitem = newitem;
			leftInhert.Reset(leftitem, isEquiped: true, null, AutoSelect);
		}
		if (newitem != null && rightitem.IndexId == newitem.IndexId)
		{
			rightitem = newitem;
			rightInhert.Reset(rightitem, isEquiped: false, UpdatePrice, AutoSelect);
		}
	}

	public void UpdatePrice(int quality)
	{
		if (GameManager.IsSupportCurDataVersion167())
		{
			NeedPrice = rightitem.GetInhertPrice(quality, IsWeaponFlag);
			PriceLabel.text = GameMoneyHelper.GetMoneyValStr(NeedPrice, GameDefine.MONEY_TYPE.GOLD);
		}
		else if (IsWeaponFlag)
		{
			if (GameDefine.WeaponInherited.ContainsKey(quality))
			{
				ConfigData configDataByKey = DataManager.GetConfigDataByKey(GameDefine.WeaponInherited[quality]);
				if (configDataByKey != null)
				{
					NeedPrice = configDataByKey.Valuei;
					PriceLabel.text = GameMoneyHelper.GetMoneyValStr(NeedPrice, GameDefine.MONEY_TYPE.GOLD);
				}
			}
		}
		else if (GameDefine.EquipInherited.ContainsKey(quality))
		{
			ConfigData configDataByKey2 = DataManager.GetConfigDataByKey(GameDefine.EquipInherited[quality]);
			if (configDataByKey2 != null)
			{
				NeedPrice = configDataByKey2.Valuei;
				PriceLabel.text = GameMoneyHelper.GetMoneyValStr(NeedPrice, GameDefine.MONEY_TYPE.GOLD);
			}
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EquipInhertRoot);
	}
}
