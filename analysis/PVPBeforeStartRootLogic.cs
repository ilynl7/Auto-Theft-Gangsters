using SprotoType;

public class PVPBeforeStartRootLogic : SingletonUnity<PVPBeforeStartRootLogic>
{
	public UILabel LeftNameLabel;

	public UILabel RightNameLabel;

	public UISprite LeftPlayerPic;

	public UISprite RightPlayerPic;

	public UILabel TitleLabel;

	public UILabel LeftCombolValueLabel;

	public UILabel RightCombolValueLabel;

	public void ResetRankPVPPage(character character)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		LeftNameLabel.text = playerData.MainPlayerAttrData.Name;
		RightNameLabel.text = character.general.name;
		LeftCombolValueLabel.text = playerData.MainPlayerAttrData.ComboValue.ToString();
		LeftPlayerPic.spriteName = GameDefine.Player_Icon_Pic[(int)playerData.Profession];
		RightPlayerPic.spriteName = GameDefine.Player_Icon_Pic[character.general.profession];
		RightCombolValueLabel.text = character.attribute_other.combValue.ToString();
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.MapType == MAPTYPE.DOMIN_MAP)
		{
			TitleLabel.text = string.Empty;
		}
		else
		{
			TitleLabel.text = "RankPVP";
		}
	}
}
