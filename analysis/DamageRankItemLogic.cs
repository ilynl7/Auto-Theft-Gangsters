using SprotoType;
using UnityEngine;

public class DamageRankItemLogic : MonoBehaviour
{
	public UILabel ranklabel;

	public UILabel namelabel;

	public UILabel damagelabel;

	public void updateItem(int rankindex, damage_list curinfo)
	{
		ranklabel.text = $"NO.{rankindex}";
		namelabel.text = curinfo.name;
		damagelabel.text = $"{curinfo.damage}";
	}

	public void updateItem(int rankindex, score_info curinfo)
	{
		ranklabel.text = $"NO.{rankindex}";
		namelabel.text = curinfo.name;
		damagelabel.text = $"{curinfo.value}";
	}
}
