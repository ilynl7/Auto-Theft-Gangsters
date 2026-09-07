public class RetrieveData
{
	public string ID;

	public int Type;

	public string Name;

	public string ShowRewardID;

	public string DropID;

	public int PriceType1;

	public int PriceCost1;

	public int AddCost1;

	public int MaxCost1;

	public int PriceType2;

	public int PriceCost2;

	public int AddCost2;

	public int MaxCost2;

	public int Scale1;

	public int Scale2;

	public int UnlockLevel;

	public bool isDanceOrExp => Type == 2 || Type == 4;

	public bool isGuildDance => Type == 9;
}
