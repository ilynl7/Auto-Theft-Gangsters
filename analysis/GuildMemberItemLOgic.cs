using UnityEngine;

public class GuildMemberItemLOgic : MonoBehaviour
{
	public GuildMember CurMember;

	public UILabel NameLab;

	public UILabel DuitesLab;

	public UILabel LevelLab;

	public UILabel ContributeLab;

	public UILabel AllContributeLab;

	public UILabel StateLab;

	public UISprite Icon;

	public UISprite BkSprite;

	public int CellHeight = 45;

	public void InitGuildMemberInfo(GuildMember info)
	{
		CurMember = info;
		NameLab.text = info.MemberName;
		DuitesLab.text = StrDictionary.GetDictionaryString(GameDefine.GuildJobStr[(int)info.Job]);
		LevelLab.text = $"{info.Level}";
		Icon.spriteName = GameDefine.Profession_PicName[(int)info.Profession];
		ContributeLab.text = info.ComboValue.ToString();
		AllContributeLab.text = info.AllContribute.ToString();
		if (info.State == 1)
		{
			StateLab.text = StrDictionary.GetDictionaryString("#{100714}");
			BkSprite.spriteName = "CZ_huaDongBG";
			StateLab.color = Color.green;
		}
		else
		{
			StateLab.color = Color.white;
			SetStateTime();
			BkSprite.spriteName = "CZ_huaDongBG_2";
		}
	}

	public void OnClickGuildItem()
	{
		if (CurMember.ServerId != Singleton<ObjManager>.Instance.MainPlayer.ServerId)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			TargetBasicInfo selectTargetBasicInfo = playerData.SelectTargetBasicInfo;
			selectTargetBasicInfo.ResetInfo(CurMember.ServerId, CurMember.Level, CurMember.ComboValue, CurMember.MemberName, CurMember.Profession, CurMember.State, playerData.PlayerGuild.ServerId, playerData.PlayerGuild.GuilName, UICamera.currentTouch.pos);
			HitOtherPLayerLogic.ShowMenu(HitType.HitGuildMember, selectTargetBasicInfo);
		}
	}

	private void SetStateTime()
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		int num = (int)((playerCommonData.GetCurServerTime() - CurMember.LastLogout) / 3600);
		if (num <= 1)
		{
			StateLab.text = "<1H";
		}
		else if (num <= 24)
		{
			StateLab.text = $"{num}H";
		}
		else
		{
			StateLab.text = $"{num / 24}D";
		}
	}
}
