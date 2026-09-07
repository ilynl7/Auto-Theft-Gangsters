using UnityEngine;

public class TeamTipMemberLineLogic : MonoBehaviour
{
	public UILabel NameLabel;

	public UISprite PlayerIconSprite;

	public UISlider HpSlider;

	public UILabel LevelLabel;

	public GameObject TeamLeaderPicObj;

	public GameObject EnableRoot;

	public GameObject DisableRoot;

	private TeamMember mCurMember;

	public TeamMember CurMember => mCurMember;

	public void Reset(TeamMember member)
	{
		mCurMember = member;
		if (member != null && member.IsValid())
		{
			UnityVersionUtil.SetActiveRecursive(DisableRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(EnableRoot, state: true);
			NameLabel.text = mCurMember.Name;
			PlayerIconSprite.spriteName = GameDefine.Player_Icon_Small_Pic[(int)mCurMember.Profession];
			LevelLabel.text = $"{member.Level}";
			UpdateHP((float)mCurMember.HP / (float)mCurMember.MaxHP);
			if (mCurMember.TeamJob == 0)
			{
				NGUITools.SetActive(TeamLeaderPicObj, state: true);
			}
			else
			{
				NGUITools.SetActive(TeamLeaderPicObj, state: false);
			}
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(EnableRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(DisableRoot, state: true);
		}
	}

	public void UpdateHP(float percent)
	{
		HpSlider.value = percent;
	}

	public void OnClickLine()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamRootNew);
	}

	public void OnClickDisableLine()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamRootNew);
	}
}
