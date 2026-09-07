using SprotoType;
using UnityEngine;

public class GuildBattleInfoLine : MonoBehaviour
{
	public UILabel ranklabel;

	public UILabel namelabel;

	public UILabel infolabel;

	private Color blueColor = new Color(31f / 85f, 0.7058824f, 1f);

	private Color redColor = Color.red;

	public void updateItem(guild_battle_item_info curinfo, int ranknum, bool isScore, bool isBlue)
	{
		ranklabel.text = $"NO.{ranknum + 1}";
		namelabel.text = curinfo.name;
		if (isScore)
		{
			infolabel.text = curinfo.score.ToString();
		}
		else
		{
			infolabel.text = curinfo.killNum.ToString();
		}
		if (isBlue)
		{
			ranklabel.color = blueColor;
			namelabel.color = blueColor;
			infolabel.color = blueColor;
		}
		else
		{
			ranklabel.color = redColor;
			namelabel.color = redColor;
			infolabel.color = redColor;
		}
	}
}
