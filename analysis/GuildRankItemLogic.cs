using SprotoType;
using UnityEngine;

public class GuildRankItemLogic : MonoBehaviour
{
	public UILabel RankLabel;

	public UISprite RankPic;

	public UILabel nameLabel;

	public UILabel chairmanlabel;

	public UILabel lvlabel;

	public UILabel infoLabel;

	public void ResetInfo(guild_info curinfo, int rankindex)
	{
		RankLabel.text = $"NO.{rankindex + 1}";
		if (rankindex < 3)
		{
			RankPic.spriteName = GameDefine.RANK_PICNAME[rankindex];
			RankPic.enabled = true;
		}
		else
		{
			RankPic.enabled = false;
		}
		nameLabel.text = curinfo.guildName;
		chairmanlabel.text = curinfo.guildChiefName;
		lvlabel.text = curinfo.guildLevel.ToString();
		infoLabel.text = curinfo.guildCombo.ToString();
	}
}
