using UnityEngine;

public class GuildApplyListItemLogic : MonoBehaviour
{
	public UISprite Icon;

	public UILabel Name;

	public UILabel Level;

	public UILabel ComboValue;

	public GuildMember curMember;

	public void InitApplyListItem(GuildMember info)
	{
		curMember = info;
		Icon.spriteName = GameDefine.Player_Icon_Small_Pic[(int)info.Profession];
		Name.text = info.MemberName;
		Level.text = $"Lv.{info.Level}";
		ComboValue.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100421}"), info.ComboValue);
	}

	public void Agree()
	{
		if (curMember != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.AgreeJoinGuild(curMember.ServerId);
		}
	}

	public void DisAgree()
	{
		if (curMember != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.DisAgreeJoinGuild(curMember.ServerId);
		}
	}
}
