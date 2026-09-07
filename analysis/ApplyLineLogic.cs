using SprotoType;
using UnityEngine;

public class ApplyLineLogic : MonoBehaviour
{
	public UISprite Icon;

	public UILabel Name;

	public UILabel Level;

	public UILabel ComboValue;

	public teammember curMember;

	public void InitApplyListItem(teammember info)
	{
		curMember = info;
		Icon.spriteName = GameDefine.Game_Player_Icon_pic[(int)info.profession];
		Name.text = info.name;
		Level.text = $"Lv.{info.level}";
		ComboValue.text = $"{info.combValue}";
	}

	public void OnClickAcceptBtn()
	{
		Debug.Log("OnClickAcceptBtn");
		apply_join_result.request request = new apply_join_result.request();
		request.characterId = curMember.id;
		request.isAgree = 1L;
		NetLogic.GetInstance().Send<Protocol.apply_join_result>(request);
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.RemoveApplyMember(curMember.id);
	}

	public void OnClickRejectBtn()
	{
		apply_join_result.request request = new apply_join_result.request();
		request.characterId = curMember.id;
		request.isAgree = 0L;
		NetLogic.GetInstance().Send<Protocol.apply_join_result>(request);
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.RemoveApplyMember(curMember.id);
	}
}
