using System;
using System.Collections.Generic;

[Serializable]
public class SkillData
{
	public string ID = string.Empty;

	public string Name = string.Empty;

	public string Icon = string.Empty;

	public string Description = string.Empty;

	public string ActionName = string.Empty;

	public int CastTime;

	public string CastAction = string.Empty;

	public int CD;

	public int TraceDistance;

	public int ComboValidTime;

	public string ComboStart = string.Empty;

	public string NextSkill = string.Empty;

	public int HoldTime;

	public int AutoMoveFlag;

	public int MoveTime;

	public int MoveDistance;

	public int MoveAngle;

	public string EffId_0 = string.Empty;

	public int EffTime_0;

	public string EffId_1 = string.Empty;

	public int EffTime_1;

	public string EffId_2 = string.Empty;

	public int EffTime_2;

	public int CanBeBreak;

	public int MaxAttackCount = 1;

	public int PriorityAutoCombat;

	public int AutoAttackDistance;

	public string Template = string.Empty;

	public int Locklevel = 1;

	public int IsUpgrade;

	public int fvalue;

	public int fvalueAdd;

	public int Job;

	public int TeamID;

	public string CamRockID = string.Empty;

	private string[] mCamRockIDList;

	private XorFloat cdx = new XorFloat();

	private XorFloat hdx = new XorFloat();

	private XorFloat Edx01 = new XorFloat();

	private XorFloat Edx02 = new XorFloat();

	private XorFloat Edx03 = new XorFloat();

	private XorFloat traceX = new XorFloat();

	private XorFloat movx = new XorFloat();

	private XorFloat autox = new XorFloat();

	private XorFloat movtx = new XorFloat();

	public string LabelID;

	private List<string> mLabelIdList;

	public string MName => StrDictionary.GetDictionaryString(Name);

	public string MDescription => StrDictionary.GetDictionaryString(Description);

	public string[] CamRockIDList
	{
		get
		{
			if (mCamRockIDList == null && !string.IsNullOrEmpty(CamRockID))
			{
				mCamRockIDList = CamRockID.Split('#');
			}
			return mCamRockIDList;
		}
	}

	public float CastTimeSecond => (float)CastTime / 1000f;

	public float MoveTimeSecond => movtx.value;

	public float CDSecond => cdx.value;

	public float TraceDistanceMeter => traceX.value;

	public float AutoAttackDistanceMeter => autox.value;

	public float ComboValidTimeSecond => (float)ComboValidTime / 1000f;

	public float HoldTimeSecond => hdx.value;

	public float MoveDistanceMeter => movx.value;

	public float EffTime_0Second => Edx01.value;

	public float EffTime_1Second => Edx02.value;

	public float EffTime_2Second => Edx03.value;

	public List<string> LabelIdList
	{
		get
		{
			if (mLabelIdList == null)
			{
				mLabelIdList = new List<string>();
				if (!string.IsNullOrEmpty(LabelID))
				{
					string[] array = LabelID.Split('#');
					for (int i = 0; i < array.Length; i++)
					{
						mLabelIdList.Add(array[i]);
					}
				}
			}
			return mLabelIdList;
		}
	}

	public void Init()
	{
		cdx.value = (float)CD / 1000f;
		hdx.value = (float)HoldTime / 1000f;
		Edx01.value = (float)EffTime_0 / 1000f;
		Edx02.value = (float)EffTime_1 / 1000f;
		Edx03.value = (float)EffTime_2 / 1000f;
		traceX.value = (float)TraceDistance / 100f;
		movx.value = (float)MoveDistance / 100f;
		autox.value = (float)AutoAttackDistance / 100f;
		movtx.value = (float)MoveTime / 1000f;
	}

	public bool IsFirstComboSkill()
	{
		if (string.IsNullOrEmpty(ComboStart))
		{
			return false;
		}
		return ID.Equals(ComboStart);
	}

	public bool IsComboSkill()
	{
		return !string.IsNullOrEmpty(NextSkill);
	}

	public bool IsComboLastSkill()
	{
		if (string.IsNullOrEmpty(ComboStart))
		{
			return false;
		}
		return NextSkill.Equals(ComboStart);
	}

	public int GetSkillDamageVal(int level)
	{
		if (level < 0)
		{
			level = 0;
		}
		if (IsUpgrade == 0)
		{
			level = 0;
		}
		int num = 0;
		if (!string.IsNullOrEmpty(EffId_0))
		{
			EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(EffId_0);
			num += effInfoDataById.Damage + effInfoDataById.DamageAdd * level;
		}
		if (!string.IsNullOrEmpty(EffId_1))
		{
			EffInfoData effInfoDataById2 = DataManager.GetEffInfoDataById(EffId_1);
			num += effInfoDataById2.Damage + effInfoDataById2.DamageAdd * level;
		}
		if (!string.IsNullOrEmpty(EffId_1))
		{
			EffInfoData effInfoDataById3 = DataManager.GetEffInfoDataById(EffId_1);
			num += effInfoDataById3.Damage + effInfoDataById3.DamageAdd * level;
		}
		return num;
	}

	public float GetSkillDamageMultiVal(int level)
	{
		if (level < 0)
		{
			level = 0;
		}
		if (IsUpgrade == 0)
		{
			level = 0;
		}
		int num = 0;
		if (!string.IsNullOrEmpty(EffId_0))
		{
			EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(EffId_0);
			num += effInfoDataById.DamageMulti + effInfoDataById.DamageMultiAdd * level;
		}
		if (!string.IsNullOrEmpty(EffId_1))
		{
			EffInfoData effInfoDataById2 = DataManager.GetEffInfoDataById(EffId_1);
			num += effInfoDataById2.DamageMulti + effInfoDataById2.DamageMultiAdd * level;
		}
		if (!string.IsNullOrEmpty(EffId_1))
		{
			EffInfoData effInfoDataById3 = DataManager.GetEffInfoDataById(EffId_1);
			num += effInfoDataById3.DamageMulti + effInfoDataById3.DamageMultiAdd * level;
		}
		return (float)num / 10000f;
	}
}
