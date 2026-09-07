public class BadgeData
{
	public string ID = string.Empty;

	public int Color = -1;

	public int BadgeType = -1;

	public int Lv = -1;

	public int Status1 = -1;

	public int Value1 = -1;

	public int Status2 = -1;

	public int Value2 = -1;

	public int MoneyType = -1;

	public int UpgradeCost;

	public int UpgradeCount = 4;

	public int GetBaseAttCount()
	{
		int num = 0;
		if (Status1 != -1)
		{
			num++;
		}
		if (Status2 != -1)
		{
			num++;
		}
		return num;
	}
}
