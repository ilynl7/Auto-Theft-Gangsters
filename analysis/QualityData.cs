using System.Collections.Generic;

public class QualityData
{
	public string ID;

	public int EquipQualityScore;

	public int AttQualityScore;

	public int AttInitialScore;

	public int Modulus;

	public int EquipInherit;

	public int WeaponInherit;

	public int EquipStar0;

	public int EquipStar1;

	public int EquipStar2;

	public int EquipStar3;

	public int EquipStar4;

	public int EquipStar5;

	public int EquipStar6;

	public int EquipStar7;

	private List<int> starlist = new List<int>();

	public List<int> EquipStarList
	{
		get
		{
			if (starlist == null || starlist.Count == 0)
			{
				starlist.Add(EquipStar0);
				starlist.Add(EquipStar1);
				starlist.Add(EquipStar2);
				starlist.Add(EquipStar3);
				starlist.Add(EquipStar4);
				starlist.Add(EquipStar5);
				starlist.Add(EquipStar6);
				starlist.Add(EquipStar7);
			}
			return starlist;
		}
	}

	public float ModulusVal => (float)Modulus / 10000f;
}
