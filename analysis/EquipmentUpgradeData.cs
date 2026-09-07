public class EquipmentUpgradeData
{
	public int PartID = -1;

	public int Level;

	public int ParameterK1 = 1;

	public int ParameterK2 = 1;

	public int ParameterK3 = 1;

	public int ParameterK4 = 1;

	public int QJParameterK1 = 1;

	public int QJParameterK2 = 1;

	public int QJParameterK3 = 1;

	public int QJParameterK4 = 1;

	public int NQParameterK1 = 1;

	public int NQParameterK2 = 1;

	public int NQParameterK3 = 1;

	public int NQParameterK4 = 1;

	public int WeaponParameterK = 1;

	public int MoneyParameterK1;

	public int MoneyParameterB1 = 1;

	public int MoneyParameterK2;

	public int MoneyParameterB2 = 1;

	public int MoneyParameterK3;

	public int MoneyParameterB3 = 1;

	public int MoneyParameterK4;

	public int MoneyParameterB4 = 1;

	public int LevelupParameterK = 1;

	public int LevelupParameterB;

	public float EquipeWhiteK => (float)ParameterK1 / 100f;

	public float EquipeGreenK => (float)ParameterK2 / 100f;

	public float EquipeBlueK => (float)ParameterK3 / 100f;

	public float EquipePurpleK => (float)ParameterK4 / 100f;

	public float MoneyWhitK => (float)MoneyParameterK1 / 100f;

	public float MoneyWhitB => MoneyParameterB1;

	public float MoneyGreenK => (float)MoneyParameterK2 / 100f;

	public float MoneyGreenB => MoneyParameterB2;

	public float MoneyBlueK => (float)MoneyParameterK3 / 100f;

	public float MoneyBlueB => MoneyParameterB3;

	public float MoneyPurpleK => (float)MoneyParameterK4 / 100f;

	public float MoneyPurpleB => MoneyParameterB4;

	public float LevelK => (float)LevelupParameterK / 100f;

	public float LevelB => LevelupParameterB;

	public float GetEquipKByQuality(int quality, int job)
	{
		if (job == 0 || !GameManager.IsSupportCurDataVersion145())
		{
			return quality switch
			{
				0 => EquipeWhiteK, 
				1 => EquipeGreenK, 
				2 => EquipeBlueK, 
				3 => EquipePurpleK, 
				_ => EquipeWhiteK, 
			};
		}
		if (job == 1)
		{
			return quality switch
			{
				0 => (float)QJParameterK1 / 100f, 
				1 => (float)QJParameterK2 / 100f, 
				2 => (float)QJParameterK3 / 100f, 
				3 => (float)QJParameterK4 / 100f, 
				_ => (float)QJParameterK1 / 100f, 
			};
		}
		return quality switch
		{
			0 => (float)NQParameterK1 / 100f, 
			1 => (float)NQParameterK2 / 100f, 
			2 => (float)NQParameterK3 / 100f, 
			3 => (float)NQParameterK4 / 100f, 
			_ => (float)NQParameterK1 / 100f, 
		};
	}

	public float GetMoneyKByQuality(int quality)
	{
		return MoneyWhitK;
	}

	public float GetMoneyBByQuality(int quality)
	{
		return MoneyWhitB;
	}
}
