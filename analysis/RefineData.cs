using UnityEngine;

public class RefineData
{
	public string ID = string.Empty;

	public int Job = -1;

	public int Part = -1;

	public int Lv;

	public int Stat1;

	public int Value1;

	public int Stat2;

	public int Value2;

	public int Stat3;

	public int Value3;

	public int Stat4;

	public int Value4;

	public string CostId1 = string.Empty;

	public int Cost1;

	public string CostId2 = string.Empty;

	public int Cost2;

	public int MoneyType;

	public int MoneyCost;

	public string SaftyId = string.Empty;

	public int SaftyCost;

	public int Chance;

	public string ICON = string.Empty;

	public int GetCombatValue()
	{
		float num = 0f;
		num += (float)Value1 * GameDefine.GET_ATTRIBUTE_COMBAT_VAL(Stat1);
		if (Stat2 != 0)
		{
			num += (float)Value2 * GameDefine.GET_ATTRIBUTE_COMBAT_VAL(Stat2);
		}
		if (Stat3 != 0)
		{
			num += (float)Value3 * GameDefine.GET_ATTRIBUTE_COMBAT_VAL(Stat3);
		}
		if (Stat4 != 0)
		{
			num += (float)Value4 * GameDefine.GET_ATTRIBUTE_COMBAT_VAL(Stat4);
		}
		return Mathf.FloorToInt(num);
	}
}
