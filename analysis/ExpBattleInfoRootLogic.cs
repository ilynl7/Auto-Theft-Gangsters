using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ExpBattleInfoRootLogic : SingletonUnity<ExpBattleInfoRootLogic>
{
	public UILabel TimeLabel;

	public UILabel WaveLabel;

	public UILabel EnemyLabel;

	public UILabel AllKillLabel;

	private DailyExpData CurExpData;

	private float reamainTime = -1f;

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

	private bool isEquipCopy;

	public void EnableReset()
	{
		CurPage = 0;
		isEquipCopy = false;
		OnClickInfoBtn();
	}

	public void ResetToTeam()
	{
		isEquipCopy = true;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (!playerData.IsHaveTeam())
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ExpBattleInfoRoot);
			return;
		}
		NGUITools.SetActive(InfoObj.gameObject, state: false);
		NGUITools.SetActive(TeamObj.gameObject, state: true);
		CurPage = 2;
		InfoSelectSp.enabled = false;
		teamSelectSp.enabled = true;
		ResetTeam();
		infoAnima.ResetToBeginning();
		hideflag = true;
		OnClickHideBtn();
		curAnchor.enabled = true;
	}

	public void OnClickInfoBtn()
	{
		if (!isEquipCopy && CurPage != 1)
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

	public void UpdateInfo(notice_copy_scene_info.request request)
	{
		if (CurPage == 1)
		{
			if (CurExpData == null || !CurExpData.ID.Equals(request.id))
			{
				CurExpData = DataManager.GetDailyExpDataById(request.id);
			}
			PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			if (playerCommonData != null && request.HasTime)
			{
				reamainTime = request.time - playerCommonData.GetCurServerTime();
			}
			if (reamainTime <= 0f)
			{
				TimeLabel.text = "00:00:00";
			}
			WaveLabel.text = $"{request.parm1}/{CurExpData.GroupCount}";
			EnemyLabel.text = $"{request.parm2}/{CurExpData.GetWaveEnemyNum((int)request.index) * CurExpData.WaveCount}";
			int num = CurExpData.GetWaveEnemyNum((int)request.index) * CurExpData.WaveCount * CurExpData.GroupCount;
			int num2 = 0;
			if (request.HasParm3)
			{
				num2 = (int)request.parm3;
			}
			AllKillLabel.text = $"{num2}/{num}";
			if (!UnityVersionUtil.IsActive(base.gameObject))
			{
				NGUITools.SetActive(base.gameObject, state: true);
				hidebtnAnima.ResetToBeginning();
				infoAnima.ResetToBeginning();
				hideflag = true;
				OnClickHideBtn();
				curAnchor.enabled = true;
				NGUITools.SetActive(InfoObj.gameObject, state: true);
				NGUITools.SetActive(TeamObj.gameObject, state: false);
			}
		}
	}

	private void Update()
	{
		if (reamainTime > 0f)
		{
			reamainTime -= Time.deltaTime;
			if (reamainTime < 0f)
			{
				reamainTime = 0f;
			}
			if (reamainTime <= 10f)
			{
				TimeLabel.color = Color.red;
			}
			else
			{
				TimeLabel.color = Color.white;
			}
			TimeLabel.text = $"{new TimeSpan(0, 0, (int)reamainTime)}";
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
