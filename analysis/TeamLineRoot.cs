using SprotoType;
using UnityEngine;

public class TeamLineRoot : MonoBehaviour
{
	public UISprite PlayerProfessionPic;

	public UILabel PlayerLevelLabel;

	public UILabel PlayerNameLabel;

	public UILabel PlayerComboValLabel;

	public UILabel TeamLimitLevelLabel;

	public UILabel TeamMemberNumLabel;

	public UILabel ApplyBtnLabel;

	public UIButton ApplyBtn;

	public BoxCollider ApplyBtnCollider;

	private bool mIsApplied;

	private team mCurTeamData;

	private PlayerData playerData;

	public void Reset(team tData, bool isApplied)
	{
		mCurTeamData = tData;
		PlayerProfessionPic.spriteName = GameDefine.Player_Profession_Pic[(int)mCurTeamData.teamleader.profession];
		PlayerLevelLabel.text = $"Lv.{mCurTeamData.teamleader.level}";
		PlayerNameLabel.text = mCurTeamData.teamleader.name;
		PlayerComboValLabel.text = $"{mCurTeamData.teamleader.combValue}";
		TeamLimitLevelLabel.text = $"Lv.{mCurTeamData.minLevel}~{mCurTeamData.maxLevel}";
		TeamMemberNumLabel.text = $"{((mCurTeamData.teammembers == null) ? 1 : (mCurTeamData.teammembers.Count + 1))}/{4}";
		mIsApplied = isApplied;
		playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (mIsApplied)
		{
			ApplyBtnLabel.text = StrDictionary.GetDictionaryString("#{100744}");
			ApplyBtn.SetState(UIButtonColor.State.Disabled, immediate: true);
			ApplyBtnCollider.enabled = false;
		}
		else if (playerData.MainPlayerAttrData.Level >= mCurTeamData.minLevel && playerData.MainPlayerAttrData.Level <= mCurTeamData.maxLevel)
		{
			ApplyBtnLabel.text = StrDictionary.GetDictionaryString("#{100812}");
			ApplyBtn.SetState(UIButtonColor.State.Normal, immediate: true);
			ApplyBtnCollider.enabled = true;
		}
		else
		{
			ApplyBtnLabel.text = StrDictionary.GetDictionaryString("#{100812}");
			ApplyBtn.SetState(UIButtonColor.State.Disabled, immediate: true);
			ApplyBtnCollider.enabled = false;
		}
	}

	public void OnClickApplyBtn()
	{
		if (!mIsApplied)
		{
			mIsApplied = true;
			ApplyBtnLabel.text = StrDictionary.GetDictionaryString("#{100744}");
			ApplyBtn.SetState(UIButtonColor.State.Disabled, immediate: true);
			ApplyBtnCollider.enabled = false;
			req_join_team.request request = new req_join_team.request();
			request.teamid = mCurTeamData.id;
			request.isapply = false;
			NetLogic.GetInstance().Send<Protocol.req_join_team>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.AddApplyTeam(mCurTeamData);
		}
	}
}
