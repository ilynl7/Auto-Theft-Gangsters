using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GameItem
{
	private long mIndexId = -1L;

	private ITEM_CONTAINER_TYPE mType = ITEM_CONTAINER_TYPE.INVALID;

	private EQUIP_BACKPACK_TYPE mEquipType = EQUIP_BACKPACK_TYPE.COUNT;

	private string mItemId = string.Empty;

	private ItemData mItemData;

	private bool mBindFlag;

	private int mStackNum = -1;

	private EQUIP_QUALITY mQuality = EQUIP_QUALITY.INVALID;

	private int mAppraise;

	private Dictionary<long, random_attri> mRandom_AttriDic;

	private Dictionary<long, inlay> mInlayDic;

	private List<QualityData> mQualityDataList = new List<QualityData>();

	private int mItemLevel;

	private int[] parm = new int[8];

	public long IndexId
	{
		get
		{
			return mIndexId;
		}
		set
		{
			mIndexId = value;
		}
	}

	public ITEM_CONTAINER_TYPE ContainerType
	{
		get
		{
			return mType;
		}
		set
		{
			mType = value;
		}
	}

	public EQUIP_BACKPACK_TYPE EquipType
	{
		get
		{
			return mEquipType;
		}
		set
		{
			mEquipType = value;
		}
	}

	public string ItemId
	{
		get
		{
			return mItemId;
		}
		set
		{
			if (mItemId.Equals(value))
			{
				return;
			}
			mItemData = DataManager.GetItemDataByID(value);
			if (mItemData != null)
			{
				mItemId = value;
				if (mItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
				{
					mEquipType = (EQUIP_BACKPACK_TYPE)mItemData.SubType;
				}
				else
				{
					mEquipType = EQUIP_BACKPACK_TYPE.COUNT;
				}
			}
			else
			{
				mEquipType = EQUIP_BACKPACK_TYPE.COUNT;
			}
		}
	}

	public ItemData ItemData
	{
		get
		{
			return mItemData;
		}
		set
		{
			mItemData = value;
		}
	}

	public bool BindFlag
	{
		get
		{
			return mBindFlag;
		}
		set
		{
			mBindFlag = value;
		}
	}

	public int StackNum
	{
		get
		{
			return mStackNum;
		}
		set
		{
			mStackNum = value;
		}
	}

	public EQUIP_QUALITY Quality
	{
		get
		{
			return mQuality;
		}
		set
		{
			mQuality = value;
		}
	}

	public int Appraise
	{
		get
		{
			return mAppraise;
		}
		set
		{
			mAppraise = value;
		}
	}

	public bool IsAppraise => mAppraise == 1;

	public Dictionary<long, random_attri> Random_AttriDic
	{
		get
		{
			return mRandom_AttriDic;
		}
		set
		{
			mRandom_AttriDic = value;
		}
	}

	public bool IsHaveRandomAtt
	{
		get
		{
			if (mRandom_AttriDic != null && mRandom_AttriDic.Count > 0)
			{
				return true;
			}
			return false;
		}
	}

	public Dictionary<long, inlay> InlayDic
	{
		get
		{
			return mInlayDic;
		}
		set
		{
			mInlayDic = value;
		}
	}

	public bool IsHaveInlay
	{
		get
		{
			if (mInlayDic != null && mInlayDic.Count > 0)
			{
				return true;
			}
			return false;
		}
	}

	public List<QualityData> QualityDataList
	{
		get
		{
			if (!string.IsNullOrEmpty(ItemId))
			{
				EquipData equipDataById = DataManager.GetEquipDataById(ItemId);
				mQualityDataList = DataManager.GetQualityDataListByID(equipDataById.QualityID);
			}
			return mQualityDataList;
		}
	}

	public int ItemLevel
	{
		get
		{
			return mItemLevel;
		}
		set
		{
			mItemLevel = value;
		}
	}

	public int[] Parm => parm;

	public GameItem()
	{
	}

	public GameItem(string id, EQUIP_QUALITY quality, int count)
	{
		mItemId = id;
		mQuality = quality;
		mStackNum = count;
		mItemData = DataManager.GetItemDataByID(id);
		if (mItemData.Type != GameDefine.ITEM_TYPE.EQUIP)
		{
			mQuality = mItemData.QualityType;
		}
	}

	public GameItem(long indexId, ITEM_CONTAINER_TYPE type, string itemId, bool bindFlag, int stackNum, EQUIP_QUALITY quality)
	{
		mIndexId = indexId;
		mType = type;
		ItemId = itemId;
		mBindFlag = bindFlag;
		mStackNum = stackNum;
		mQuality = quality;
	}

	public void SetAttInfo(int appraiseinfo, Dictionary<long, random_attri> randomatt, Dictionary<long, inlay> inlaydic)
	{
		Appraise = appraiseinfo;
		Random_AttriDic = randomatt;
		InlayDic = inlaydic;
	}

	public void SetAddItem()
	{
		mItemId = GameDefine.EmptyAddItemID;
		mQuality = EQUIP_QUALITY.KUANG_WHITE;
		mStackNum = 1;
		mItemData = DataManager.GetItemDataByID(mItemId);
		Parm[5] = 1;
	}

	public void SetParm(List<long> parms)
	{
		for (int i = 0; i < parms.Count; i++)
		{
			parm[i] = (int)parms[i];
		}
	}

	public void Reset()
	{
		mItemId = string.Empty;
		mStackNum = -1;
		mIndexId = -1L;
	}

	public bool IsEmpty()
	{
		if (!string.IsNullOrEmpty(mItemId) && mItemData != null)
		{
			return false;
		}
		return true;
	}

	public GameItem ShallowCopy()
	{
		return (GameItem)MemberwiseClone();
	}

	public bool IsFull()
	{
		if (!string.IsNullOrEmpty(mItemId))
		{
			ItemData itemDataByID = DataManager.GetItemDataByID(mItemId);
			if (itemDataByID != null && itemDataByID.Stack > 1 && itemDataByID.Stack > mStackNum)
			{
				return false;
			}
		}
		return true;
	}

	public int GetItemLeftSpace()
	{
		if (!string.IsNullOrEmpty(mItemId))
		{
			ItemData itemDataByID = DataManager.GetItemDataByID(mItemId);
			if (itemDataByID != null)
			{
				return itemDataByID.Stack - mStackNum;
			}
		}
		return -1;
	}

	public void SetItem(GameItem item)
	{
		mIndexId = item.IndexId;
		mItemId = item.ItemId;
		BindFlag = item.BindFlag;
		mStackNum = item.StackNum;
	}

	public void UpdateItem(gameitem netItem)
	{
		ItemId = netItem.itemId;
		if (netItem.HasParm)
		{
			SetParm(netItem.parm);
		}
		if (netItem.HasBindflag)
		{
			BindFlag = netItem.bindflag;
		}
		else
		{
			BindFlag = false;
		}
		if (netItem.HasStack)
		{
			StackNum = (int)netItem.stack;
		}
		else
		{
			StackNum = 1;
		}
		if (netItem.HasIndexId)
		{
			IndexId = netItem.indexId;
		}
		else
		{
			IndexId = -1L;
		}
		if (netItem.HasQuality)
		{
			Quality = (EQUIP_QUALITY)netItem.quality;
		}
		else
		{
			Quality = EQUIP_QUALITY.INVALID;
		}
		if (netItem.HasLevel)
		{
			ItemLevel = (int)netItem.level;
		}
		else
		{
			ItemLevel = 0;
		}
		if (netItem.HasAppraise)
		{
			Appraise = (int)netItem.appraise;
		}
		else
		{
			Appraise = 0;
		}
		if (netItem.HasRandom_attri)
		{
			Random_AttriDic = netItem.random_attri;
		}
		else
		{
			Random_AttriDic = null;
		}
		if (netItem.HasInlay)
		{
			InlayDic = netItem.inlay;
		}
		else
		{
			InlayDic = null;
		}
	}

	public int GetItemScore()
	{
		if (mItemData != null)
		{
			return mItemData.GetScore(mQuality);
		}
		return 0;
	}

	public int GetItemCombatVal()
	{
		if (mItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			EquipData equipDataById = DataManager.GetEquipDataById(mItemData.ID);
			float num = 0f;
			for (int i = 0; i < equipDataById.GetBaseAttCount(); i++)
			{
				int attrIDByQuality = equipDataById.GetAttrIDByQuality(i);
				if (attrIDByQuality != 0)
				{
					num += (float)equipDataById.GetAttrValByQualityAndLevel(i, (int)GetItemQuality(), mItemLevel) * GameDefine.GET_ATTRIBUTE_COMBAT_VAL(attrIDByQuality);
				}
			}
			if (IsHaveRandomAtt && mItemData.SubType != 0)
			{
				foreach (random_attri value in Random_AttriDic.Values)
				{
					int attid = (int)value.id;
					num += (float)(int)value.value * GameDefine.GET_ATTRIBUTE_COMBAT_VAL(attid);
				}
			}
			return Mathf.FloorToInt(num);
		}
		if (mItemData.Type == GameDefine.ITEM_TYPE.FASHION_EQUIP)
		{
			EquipData equipDataById2 = DataManager.GetEquipDataById(mItemData.ID);
			float num2 = 0f;
			for (int j = 0; j < equipDataById2.GetBaseAttCount(); j++)
			{
				int attrIDByQuality2 = equipDataById2.GetAttrIDByQuality(j);
				if (attrIDByQuality2 != 0)
				{
					num2 += (float)equipDataById2.GetAttrValByQualityAndLevel(j, (int)GetItemQuality(), mItemLevel) * GameDefine.GET_ATTRIBUTE_COMBAT_VAL(attrIDByQuality2);
				}
			}
			return Mathf.FloorToInt(num2);
		}
		if (mItemData.Type == GameDefine.ITEM_TYPE.BADGE)
		{
			BadgeData badgeDataById = DataManager.GetBadgeDataById(mItemData.ID);
			float num3 = 0f;
			num3 += (float)badgeDataById.Value1 * GameDefine.GET_ATTRIBUTE_COMBAT_VAL(badgeDataById.Status1);
			if (badgeDataById.Status2 != -1)
			{
				num3 += (float)badgeDataById.Value2 * GameDefine.GET_ATTRIBUTE_COMBAT_VAL(badgeDataById.Status2);
			}
			return Mathf.FloorToInt(num3);
		}
		return 0;
	}

	public EQUIP_QUALITY GetItemQuality()
	{
		if (ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			if (IsAppraise)
			{
				return (EQUIP_QUALITY)GetQualityByScore();
			}
			return mQuality;
		}
		return mQuality;
	}

	public int GetQualityByScore()
	{
		int randomAttScore = GetRandomAttScore();
		if (GameManager.IsSupportCurDataVersion167())
		{
			if (QualityDataList != null && QualityDataList.Count > 0)
			{
				for (int i = 0; i < QualityDataList.Count; i++)
				{
					if (randomAttScore < QualityDataList[i].EquipQualityScore)
					{
						if (i == 0)
						{
							return i;
						}
						return i - 1;
					}
					if (i == QualityDataList.Count - 1)
					{
						return i;
					}
				}
			}
		}
		else
		{
			List<ConfigData> configQualityScoreList = DataManager.GetConfigQualityScoreList();
			if (configQualityScoreList != null && configQualityScoreList.Count > 0)
			{
				for (int j = 0; j < configQualityScoreList.Count; j++)
				{
					if (randomAttScore < configQualityScoreList[j].Valuei)
					{
						if (j == 0)
						{
							return j;
						}
						return j - 1;
					}
					if (j == configQualityScoreList.Count - 1)
					{
						return j;
					}
				}
			}
		}
		return 0;
	}

	public int GetStarByScore()
	{
		if (ItemData.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			EquipData equipDataById = DataManager.GetEquipDataById(ItemData.ID);
			if (equipDataById.Class >= 1)
			{
				return equipDataById.Class - 1;
			}
			return 0;
		}
		int randomAttScore = GetRandomAttScore();
		if (GameManager.IsSupportCurDataVersion167())
		{
			int itemQuality = (int)GetItemQuality();
			if (QualityDataList != null && QualityDataList.Count > itemQuality)
			{
				QualityData qualityData = QualityDataList[itemQuality];
				List<int> equipStarList = qualityData.EquipStarList;
				if (equipStarList != null && equipStarList.Count > 0)
				{
					for (int i = 0; i < equipStarList.Count; i++)
					{
						if (randomAttScore <= equipStarList[i])
						{
							return i;
						}
						if (i == equipStarList.Count - 1)
						{
							return i;
						}
					}
				}
			}
		}
		else
		{
			List<ConfigData> configStarScoreList = DataManager.GetConfigStarScoreList();
			if (configStarScoreList.Count > 0)
			{
				for (int j = 0; j < configStarScoreList.Count; j++)
				{
					if (randomAttScore < configStarScoreList[j].Valuei)
					{
						return j;
					}
					if (j == configStarScoreList.Count - 1)
					{
						return j + 1;
					}
				}
			}
		}
		return 0;
	}

	public int GetRandomAttScore()
	{
		int num = 0;
		if (IsHaveRandomAtt)
		{
			if (GameManager.IsSupportCurDataVersion167())
			{
				foreach (random_attri value in Random_AttriDic.Values)
				{
					if (!value.HasQualityId)
					{
						EquipData equipDataById = DataManager.GetEquipDataById(mItemData.ID);
						value.qualityId = equipDataById.QualityID;
					}
					num += GetAttScoreByQuality((int)value.quality, value.qualityId);
				}
			}
			else
			{
				foreach (random_attri value2 in Random_AttriDic.Values)
				{
					num += GetAttScoreByQuality((int)value2.quality);
				}
			}
		}
		return num;
	}

	public void ShowPrintInfo()
	{
		if (!IsHaveRandomAtt)
		{
			return;
		}
		foreach (random_attri value in Random_AttriDic.Values)
		{
			if (!value.HasQualityId)
			{
				EquipData equipDataById = DataManager.GetEquipDataById(mItemData.ID);
				value.qualityId = equipDataById.QualityID;
			}
		}
	}

	private int GetAttScoreByQuality(int quaval)
	{
		if (GameDefine.StarAttIntegral.ContainsKey(quaval))
		{
			ConfigData configDataByKey = DataManager.GetConfigDataByKey(GameDefine.StarAttIntegral[quaval]);
			if (configDataByKey != null)
			{
				return configDataByKey.Valuei;
			}
		}
		return 0;
	}

	private int GetAttScoreByQuality(int quaval, string qualityid)
	{
		List<QualityData> qualityDataListByID = DataManager.GetQualityDataListByID(qualityid);
		if (qualityDataListByID != null && qualityDataListByID.Count > quaval)
		{
			return qualityDataListByID[quaval].AttInitialScore;
		}
		return 0;
	}

	public int GetEquipAttQuality(int quaval, string qualityid)
	{
		if (GameManager.IsSupportCurDataVersion167())
		{
			int attScoreByQuality = GetAttScoreByQuality(quaval, qualityid);
			if (QualityDataList != null && QualityDataList.Count > 0)
			{
				for (int i = 0; i < QualityDataList.Count; i++)
				{
					if (attScoreByQuality < QualityDataList[i].AttQualityScore)
					{
						if (i == 0)
						{
							return i;
						}
						return i - 1;
					}
					if (i == QualityDataList.Count - 1)
					{
						return i;
					}
				}
			}
			return 0;
		}
		return quaval;
	}

	public int GetInhertPrice(int quality, bool isweapon)
	{
		if (QualityDataList != null && QualityDataList.Count > quality)
		{
			if (isweapon)
			{
				return QualityDataList[quality].WeaponInherit;
			}
			return QualityDataList[quality].EquipInherit;
		}
		return 0;
	}
}
