using System.Collections.Generic;
using UnityEngine;

public class EquipData
{
	public string ID = string.Empty;

	public string NAME = string.Empty;

	public int Lv = 1;

	public int Job;

	public int Position;

	public int Basestatus;

	public int BSValue;

	public int Status1;

	public int ES1V;

	public int Status2;

	public int ES2V;

	public int ExStatus;

	public int ExV;

	public string ModelId = string.Empty;

	public int UpgradeMaxLevel;

	public string DropId1;

	public string DropId2;

	public string DropId3;

	public string DropId4;

	public string DropId5;

	public string DropId6;

	public string identify = string.Empty;

	public int LevelScale = 10000;

	public string BaseSkills;

	public int WeaponType;

	public string QualityID = string.Empty;

	public int Class;

	private int[] mPrice;

	private List<EquipmentUpgradeData> mCacheUpgradeDataList;

	public PROFESSION_TYPE profession => (PROFESSION_TYPE)Job;

	public ATTRIBUTE_TYPE BaseStatusType => (ATTRIBUTE_TYPE)Basestatus;

	public ATTRIBUTE_TYPE ExtraStatus1Type => (ATTRIBUTE_TYPE)Status1;

	public ATTRIBUTE_TYPE ExtraStatus2Type => (ATTRIBUTE_TYPE)Status2;

	public ATTRIBUTE_TYPE ExStatusType => (ATTRIBUTE_TYPE)ExStatus;

	public EQUIP_BACKPACK_TYPE EquipType => (EQUIP_BACKPACK_TYPE)Position;

	public int GetAppraisePrice(EQUIP_QUALITY quality)
	{
		if (mPrice == null && !string.IsNullOrEmpty(identify))
		{
			string[] array = identify.Split('#');
			mPrice = new int[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				mPrice[i] = int.Parse(array[i]);
			}
		}
		if (mPrice != null)
		{
			if ((int)quality < mPrice.Length)
			{
				return mPrice[(int)quality];
			}
		}
		return 0;
	}

	public int GetAttrIDByQuality(int qualityValue)
	{
		return qualityValue switch
		{
			0 => Basestatus, 
			1 => Status1, 
			2 => Status2, 
			3 => ExStatus, 
			_ => 0, 
		};
	}

	public int GetAttrValueByQuality(int targetQualityValue, int itemQuality)
	{
		ItemData itemDataByID = DataManager.GetItemDataByID(ID);
		if (itemDataByID.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			return targetQualityValue switch
			{
				0 => Mathf.FloorToInt(BSValue), 
				1 => Mathf.FloorToInt(ES1V), 
				2 => Mathf.FloorToInt(ES2V), 
				3 => Mathf.FloorToInt(ExV), 
				_ => -1, 
			};
		}
		float num = 1f;
		if (GameManager.IsSupportCurDataVersion167())
		{
			List<QualityData> qualityDataListByID = DataManager.GetQualityDataListByID(QualityID);
			if (qualityDataListByID != null && qualityDataListByID.Count > itemQuality)
			{
				num = qualityDataListByID[itemQuality].ModulusVal;
			}
		}
		else if (GameDefine.EquipQualityValAddName.ContainsKey(itemQuality))
		{
			ConfigData configDataByKey = DataManager.GetConfigDataByKey(GameDefine.EquipQualityValAddName[itemQuality]);
			if (configDataByKey != null)
			{
				num = configDataByKey.Valuef;
			}
		}
		return targetQualityValue switch
		{
			0 => Mathf.FloorToInt((float)BSValue * num), 
			1 => Mathf.FloorToInt((float)ES1V * num), 
			2 => Mathf.FloorToInt((float)ES2V * num), 
			3 => Mathf.FloorToInt((float)ExV * num), 
			_ => -1, 
		};
	}

	public int GetAttrValByQualityAndLevel(int targetQualityValue, int itemQuality, int itemLevel)
	{
		int attrValueByQuality = GetAttrValueByQuality(targetQualityValue, itemQuality);
		if (mCacheUpgradeDataList == null)
		{
			mCacheUpgradeDataList = DataManager.GetEquipmentUpgradeDataListByPartID(Position);
			if (mCacheUpgradeDataList == null)
			{
				Debug.LogError("EquipmentUpgradeData error no have postion");
			}
		}
		if (mCacheUpgradeDataList != null && mCacheUpgradeDataList.Count >= itemLevel && itemLevel > 0)
		{
			EquipmentUpgradeData equipmentUpgradeData = mCacheUpgradeDataList[itemLevel - 1];
			if (equipmentUpgradeData != null)
			{
				int num = Job;
				if (num == -1)
				{
					num = WeaponType;
				}
				float equipKByQuality = equipmentUpgradeData.GetEquipKByQuality(targetQualityValue, num);
				return Mathf.FloorToInt((float)attrValueByQuality + equipKByQuality * (float)itemLevel);
			}
		}
		return attrValueByQuality;
	}

	public int GetAttrEnhanceVal(int targetQualityValue, int itemQuality, int itemLevel)
	{
		if (mCacheUpgradeDataList == null)
		{
			mCacheUpgradeDataList = DataManager.GetEquipmentUpgradeDataListByPartID(Position);
			if (mCacheUpgradeDataList == null)
			{
				Debug.LogError("EquipmentUpgradeData error no have postion");
			}
		}
		if (mCacheUpgradeDataList != null && mCacheUpgradeDataList.Count >= itemLevel && itemLevel > 0)
		{
			EquipmentUpgradeData equipmentUpgradeData = mCacheUpgradeDataList[itemLevel - 1];
			if (equipmentUpgradeData != null)
			{
				int num = Job;
				if (num == -1)
				{
					num = WeaponType;
				}
				float equipKByQuality = equipmentUpgradeData.GetEquipKByQuality(targetQualityValue, num);
				return Mathf.FloorToInt(equipKByQuality * (float)itemLevel);
			}
		}
		return 0;
	}

	public int GetUpgradeLevelByExp(int curLevel, int exp, out int use)
	{
		if (mCacheUpgradeDataList == null)
		{
			mCacheUpgradeDataList = DataManager.GetEquipmentUpgradeDataListByPartID(Position);
			if (mCacheUpgradeDataList == null)
			{
				Debug.LogError("EquipmentUpgradeData error no have postion");
			}
		}
		use = 0;
		if (mCacheUpgradeDataList == null)
		{
			return curLevel;
		}
		int num = exp;
		int num2 = curLevel;
		while (num > 0 && mCacheUpgradeDataList.Count > num2)
		{
			EquipmentUpgradeData equipmentUpgradeData = null;
			equipmentUpgradeData = mCacheUpgradeDataList[num2];
			if (equipmentUpgradeData != null)
			{
				int num3 = Mathf.FloorToInt(equipmentUpgradeData.LevelB + equipmentUpgradeData.LevelK * (float)num2 * (float)num2 * 3f);
				if (num < num3)
				{
					break;
				}
				num -= num3;
				use += num3;
				num2++;
			}
		}
		return num2;
	}

	public int GetUpgradeExpValByLevel(int itemLevel)
	{
		if (mCacheUpgradeDataList == null)
		{
			mCacheUpgradeDataList = DataManager.GetEquipmentUpgradeDataListByPartID(Position);
			if (mCacheUpgradeDataList == null)
			{
				Debug.LogError("EquipmentUpgradeData error no have postion");
			}
		}
		if (mCacheUpgradeDataList != null && mCacheUpgradeDataList.Count > itemLevel)
		{
			EquipmentUpgradeData equipmentUpgradeData = mCacheUpgradeDataList[itemLevel];
			if (equipmentUpgradeData != null)
			{
				return Mathf.FloorToInt(equipmentUpgradeData.LevelB + equipmentUpgradeData.LevelK * (float)itemLevel * (float)itemLevel * 3f);
			}
		}
		return 0;
	}

	public int GetUpgradeMoneyByQualityAndLevel(int itemQuality, int itemLevel)
	{
		if (mCacheUpgradeDataList == null)
		{
			mCacheUpgradeDataList = DataManager.GetEquipmentUpgradeDataListByPartID(Position);
			if (mCacheUpgradeDataList == null)
			{
				Debug.LogError("EquipmentUpgradeData error no have postion");
			}
		}
		if (mCacheUpgradeDataList != null && mCacheUpgradeDataList.Count > itemLevel)
		{
			EquipmentUpgradeData equipmentUpgradeData = mCacheUpgradeDataList[itemLevel];
			if (equipmentUpgradeData != null)
			{
				float moneyKByQuality = equipmentUpgradeData.GetMoneyKByQuality(itemQuality);
				float moneyBByQuality = equipmentUpgradeData.GetMoneyBByQuality(itemQuality);
				return Mathf.FloorToInt(moneyBByQuality + moneyKByQuality * (float)itemLevel);
			}
		}
		return 0;
	}

	public int GetBaseAttCount()
	{
		int num = 0;
		if (Basestatus != 0)
		{
			num++;
		}
		if (Status1 != 0)
		{
			num++;
		}
		if (Status2 != 0)
		{
			num++;
		}
		if (ExStatus != 0)
		{
			num++;
		}
		return num;
	}
}
