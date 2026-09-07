using SprotoType;
using UnityEngine;

public class GuildBattleScoreLine : MonoBehaviour
{
	public UILabel Ranklabel;

	public UISprite TopSp;

	public UILabel PlayerName;

	public UILabel GuildName;

	public UILabel killLabel;

	public UILabel MaxKillLabel;

	public UILabel ScoreLabel;

	public void UpdateInfo(guild_battle_item_info curInfo, int rank)
	{
		Ranklabel.text = $"NO.{rank + 1}";
		if (rank < 3)
		{
			TopSp.spriteName = GameDefine.RANK_PICNAME[rank];
			TopSp.enabled = true;
		}
		else
		{
			TopSp.enabled = false;
		}
		PlayerName.text = curInfo.name;
		GuildName.text = curInfo.guildName;
		killLabel.text = curInfo.killNum.ToString();
		MaxKillLabel.text = curInfo.continueKill.ToString();
		ScoreLabel.text = curInfo.score.ToString();
	}
}
