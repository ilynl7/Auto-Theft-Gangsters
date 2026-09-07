using UnityEngine;

public class GuildListLineItemLogic : MonoBehaviour
{
	public GuildInfo curGuildInfo;

	public UILabel NameLab;

	public UILabel IdLab;

	public UILabel BossNameLab;

	public UILabel LvLabLab;

	public UILabel MemberNumLab;

	public UISprite GuildIcon;

	public bool IsNeedApply;

	public UILabel ApplyBtnLabel;

	public UILabel DkpLabel;

	public UILabel ComboLabel;

	public UIButton ApplyBtn;

	private bool CanApply = true;

	public void InitGuildInfo(GuildInfo info, int index)
	{
		curGuildInfo = info;
		NameLab.text = info.GuilName;
		GuildIcon.spriteName = GameDefine.GuildIcon[info.GuildIcon];
		BossNameLab.text = info.GuildChiefName;
		LvLabLab.text = info.GuilLevel.ToString();
		MemberNumLab.text = $"{info.CurMemberNum}/{info.MaxMemberNum}";
		ComboLabel.text = info.ComboValue.ToString();
		DkpLabel.text = info.Exp.ToString();
		if (Singleton<ObjManager>.Instance.MainPlayer.ApplyGuildIDList.Contains(curGuildInfo.ServerId))
		{
			ApplyBtnLabel.text = StrDictionary.GetDictionaryString("#{100744}");
			CanApply = false;
		}
		else
		{
			ApplyBtnLabel.text = StrDictionary.GetDictionaryString("#{100770}");
			CanApply = true;
		}
	}

	public void OnClickApplyBtn()
	{
		if (CanApply && Singleton<ObjManager>.Instance.MainPlayer.JoinGuild(curGuildInfo.ServerId))
		{
			ApplyBtnLabel.text = StrDictionary.GetDictionaryString("#{100744}");
		}
	}
}
