using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class MultiRankSmallRootLogic : SingletonUnity<MultiRankSmallRootLogic>
{
	public List<DamageRankItemLogic> rankItemList;

	public List<damage_list> DamageList;

	public UIGrid RankGrid;

	public UILabel TitleLabel;

	public UILabel ScoreLabel;

	public UILabel selfRankLabel;

	public UILabel selfNameLabel;

	public UILabel selfDamageLabel;

	private battle_info curBattleInfo;

	public TweenRotation hidebtnAnima;

	public TweenPosition infoAnima;

	private bool hideflag;

	public UIAnchor curAnchor;

	public GameObject InfoObj;

	public GameObject TeamObj;

	public List<TeamTipMemberLineLogic> TeamMemberLineList;

	public UISprite InfoSelectSp;

	public UISprite teamSelectSp;

	private int CurPage;

	public void EnableReset()
	{
		CurPage = 0;
		OnClickInfoBtn();
	}

	public void OnClickInfoBtn()
	{
		if (CurPage != 1)
		{
			NGUITools.SetActive(InfoObj.gameObject, state: true);
			NGUITools.SetActive(TeamObj.gameObject, state: false);
			CurPage = 1;
			InfoSelectSp.enabled = true;
			teamSelectSp.enabled = false;
		}
	}

	public void OnClickTeamBtn()
	{
		if (CurPage != 2)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (!playerData.IsHaveTeam())
			{
				NoticeLogic.AddNotifyData("#{103207}");
				return;
			}
			NGUITools.SetActive(InfoObj.gameObject, state: false);
			NGUITools.SetActive(TeamObj.gameObject, state: true);
			CurPage = 2;
			InfoSelectSp.enabled = false;
			teamSelectSp.enabled = true;
			ResetTeam();
		}
	}

	public void ResetTeam()
	{
		if (CurPage == 2)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (playerData.IsHaveTeam())
			{
				ResetTeamMember();
			}
		}
	}

	public void ResetTeamMember()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsTeamLeader())
		{
			for (int i = 0; i < playerData.TeamInfo.TeamMembers.Length; i++)
			{
				TeamMemberLineList[i].Reset(playerData.TeamInfo.TeamMembers[i]);
			}
			return;
		}
		int num = 0;
		TeamMemberLineList[num].Reset(playerData.TeamInfo.TeamLeader);
		num++;
		for (int j = 0; j < playerData.TeamInfo.TeamMembers.Length; j++)
		{
			if (playerData.TeamInfo.TeamMembers[j].ServerId != PlayerData.MainPlayerServerId)
			{
				TeamMemberLineList[num].Reset(playerData.TeamInfo.TeamMembers[j]);
				num++;
			}
		}
	}

	private TeamTipMemberLineLogic GetMemberLineById(long serverId)
	{
		for (int i = 0; i < TeamMemberLineList.Count; i++)
		{
			if (TeamMemberLineList[i].CurMember != null && TeamMemberLineList[i].CurMember.IsValid() && TeamMemberLineList[i].CurMember.ServerId == serverId)
			{
				return TeamMemberLineList[i];
			}
		}
		return null;
	}

	public void UpdateMemberInfo(TeamMember member)
	{
		if (CurPage == 2)
		{
			TeamTipMemberLineLogic memberLineById = GetMemberLineById(member.ServerId);
			if (memberLineById != null)
			{
				memberLineById.Reset(member);
			}
		}
	}

	public void Reset(battle_info battleinfo)
	{
		if (CurPage != 1)
		{
			return;
		}
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100772}");
		ScoreLabel.text = StrDictionary.GetDictionaryString("#{100756}");
		curBattleInfo = battleinfo;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		DamageList = curBattleInfo.damage_list;
		int num = DamageList.Count - rankItemList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(rankItemList[0].gameObject) as GameObject;
				gameObject.transform.parent = RankGrid.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				DamageRankItemLogic component = gameObject.GetComponent<DamageRankItemLogic>();
				rankItemList.Add(component);
				if (rankItemList.Count < 10)
				{
					gameObject.gameObject.name = "0" + rankItemList.Count;
				}
				else
				{
					gameObject.gameObject.name = rankItemList.Count.ToString();
				}
			}
			RankGrid.Reposition();
		}
		for (int j = 0; j < rankItemList.Count; j++)
		{
			if (j < DamageList.Count)
			{
				NGUITools.SetActive(rankItemList[j].gameObject, state: true);
				rankItemList[j].updateItem(j + 1, DamageList[j]);
			}
			else
			{
				NGUITools.SetActive(rankItemList[j].gameObject, state: false);
			}
		}
		selfRankLabel.text = $"NO.{curBattleInfo.my_rank}";
		selfNameLabel.text = playerData.MainPlayerAttrData.Name;
		selfDamageLabel.text = $"{curBattleInfo.my_damage}";
		if (!UnityVersionUtil.IsActive(base.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
			hidebtnAnima.ResetToBeginning();
			infoAnima.ResetToBeginning();
			hideflag = true;
			OnClickHideBtn();
			curAnchor.enabled = true;
			NGUITools.SetActive(InfoObj.gameObject, state: true);
			NGUITools.SetActive(TeamObj.gameObject, state: false);
		}
	}

	public void Reset(battle_info battleinfo, GameDefine.ACTIVITY_TYPE type)
	{
		if (CurPage != 1 || type != GameDefine.ACTIVITY_TYPE.BAR_FIGHT)
		{
			return;
		}
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100772}");
		ScoreLabel.text = StrDictionary.GetDictionaryString("#{100756}");
		curBattleInfo = battleinfo;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		DamageList = curBattleInfo.damage_list;
		DamageList.Sort((damage_list x, damage_list y) => (int)y.damage - (int)x.damage);
		for (int num = DamageList.Count - 1; num >= 6; num--)
		{
			DamageList.RemoveAt(num);
		}
		int num2 = -1;
		int num3 = -1;
		for (int i = 0; i < DamageList.Count; i++)
		{
			if (DamageList[i].id == PlayerData.MainPlayerServerId)
			{
				num3 = i + 1;
				num2 = (int)DamageList[i].damage;
				break;
			}
		}
		int num4 = DamageList.Count - rankItemList.Count;
		if (num4 > 0)
		{
			for (int j = 0; j < num4; j++)
			{
				GameObject gameObject = Object.Instantiate(rankItemList[0].gameObject) as GameObject;
				gameObject.transform.parent = RankGrid.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				DamageRankItemLogic component = gameObject.GetComponent<DamageRankItemLogic>();
				rankItemList.Add(component);
				if (rankItemList.Count < 10)
				{
					gameObject.gameObject.name = "0" + rankItemList.Count;
				}
				else
				{
					gameObject.gameObject.name = rankItemList.Count.ToString();
				}
			}
			RankGrid.Reposition();
		}
		for (int k = 0; k < rankItemList.Count; k++)
		{
			if (k < DamageList.Count)
			{
				NGUITools.SetActive(rankItemList[k].gameObject, state: true);
				rankItemList[k].updateItem(k + 1, DamageList[k]);
			}
			else
			{
				NGUITools.SetActive(rankItemList[k].gameObject, state: false);
			}
		}
		selfRankLabel.text = $"NO.{num3}";
		selfNameLabel.text = playerData.MainPlayerAttrData.Name;
		selfDamageLabel.text = $"{num2}";
		if (!UnityVersionUtil.IsActive(base.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
			hidebtnAnima.ResetToBeginning();
			infoAnima.ResetToBeginning();
			hideflag = true;
			OnClickHideBtn();
			curAnchor.enabled = true;
			NGUITools.SetActive(InfoObj.gameObject, state: true);
			NGUITools.SetActive(TeamObj.gameObject, state: false);
		}
	}

	public void Reset(ret_request_survive_top.request request)
	{
		if (CurPage != 1)
		{
			return;
		}
		TitleLabel.text = StrDictionary.GetDictionaryString("#{101596}");
		ScoreLabel.text = StrDictionary.GetDictionaryString("#{101597}");
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		int num = request.score_infos.Count - rankItemList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(rankItemList[0].gameObject) as GameObject;
				gameObject.transform.parent = RankGrid.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				DamageRankItemLogic component = gameObject.GetComponent<DamageRankItemLogic>();
				rankItemList.Add(component);
				if (rankItemList.Count < 10)
				{
					gameObject.gameObject.name = "0" + rankItemList.Count;
				}
				else
				{
					gameObject.gameObject.name = rankItemList.Count.ToString();
				}
			}
			RankGrid.Reposition();
		}
		for (int j = 0; j < rankItemList.Count; j++)
		{
			if (j < request.score_infos.Count)
			{
				NGUITools.SetActive(rankItemList[j].gameObject, state: true);
				rankItemList[j].updateItem(j + 1, request.score_infos[j]);
			}
			else
			{
				NGUITools.SetActive(rankItemList[j].gameObject, state: false);
			}
		}
		selfRankLabel.text = $"NO.{request.my_rank}";
		selfNameLabel.text = playerData.MainPlayerAttrData.Name;
		selfDamageLabel.text = $"{request.my_score}";
		if (!UnityVersionUtil.IsActive(base.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(base.gameObject, state: true);
			hidebtnAnima.ResetToBeginning();
			infoAnima.ResetToBeginning();
			hideflag = true;
			OnClickHideBtn();
			curAnchor.enabled = true;
			NGUITools.SetActive(InfoObj.gameObject, state: true);
			NGUITools.SetActive(TeamObj.gameObject, state: false);
		}
	}

	public void OnClickHideBtn()
	{
		if (hideflag)
		{
			hidebtnAnima.PlayForward();
			infoAnima.PlayForward();
			hideflag = false;
		}
		else
		{
			hidebtnAnima.PlayReverse();
			infoAnima.PlayReverse();
			hideflag = true;
		}
	}
}
