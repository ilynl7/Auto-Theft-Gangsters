using System;
using System.Collections.Generic;
using UnityEngine;

public class GuildMemberRootLogic : SingletonUnity<GuildMemberRootLogic>
{
	public List<GuildMemberItemLOgic> GuildMemberItemList = new List<GuildMemberItemLOgic>();

	private List<GuildMember> GuildMemberList = new List<GuildMember>();

	public GameObject LogBtn;

	public GameObject ApplyListBtn;

	public GuildApplyListLogic ApplyListLogic;

	public GameObject NeedAppro;

	public GameObject ApproBtn;

	public GameObject RecruitBtn;

	public GameObject Tips;

	public List<GuildMember> ApplyList = new List<GuildMember>();

	public UIScrollView ScrollView;

	public UIWrapContentNew Grid;

	public UIWidget BottomWidget;

	protected override void Awake()
	{
		base.Awake();
		UIWrapContentNew grid = Grid;
		grid.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(grid.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeItem));
	}

	public void EnableReset()
	{
		for (int i = 0; i < GuildMemberItemList.Count; i++)
		{
			NGUITools.SetActive(GuildMemberItemList[i].gameObject, state: false);
		}
		UnityVersionUtil.SetActiveRecursive(Tips, state: false);
		UnityVersionUtil.SetActiveRecursive(ApplyListBtn, state: false);
		UnityVersionUtil.SetActiveRecursive(RecruitBtn, state: false);
		UnityVersionUtil.SetActiveRecursive(NeedAppro, state: false);
		UnityVersionUtil.SetActiveRecursive(ApproBtn, state: false);
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		GuildMemberItemLOgic item = GuildMemberItemList[index];
		Reset(item, Mathf.Abs(realIndex), GuildMemberList.Count);
	}

	private void Reset(GuildMemberItemLOgic item, int realIndex, int maxLine)
	{
		if (realIndex >= GuildMemberList.Count)
		{
			realIndex = GuildMemberList.Count - 1;
		}
		if (realIndex >= 0)
		{
			item.InitGuildMemberInfo(GuildMemberList[realIndex]);
		}
	}

	private void UpdateBtn()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		UnityVersionUtil.SetActiveRecursive(ApplyListBtn, playerData.PlayerGuild.CanApprove());
		UnityVersionUtil.SetActiveRecursive(RecruitBtn, playerData.PlayerGuild.CanApprove());
		UnityVersionUtil.SetActiveRecursive(NeedAppro, playerData.PlayerGuild.CanSetApprove());
		if (playerData.PlayerGuild.CanSetApprove())
		{
			UnityVersionUtil.SetActiveRecursive(ApproBtn, !playerData.PlayerGuild.IsNeedAppro);
		}
		if (ApplyList.Count > 0 && playerData.PlayerGuild.CanApprove())
		{
			UnityVersionUtil.SetActiveRecursive(Tips, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(Tips, state: false);
		}
	}

	public void UpdateGuildMemberItemList(Dictionary<long, GuildMember> list)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		ApplyList.Clear();
		GuildMemberList.Clear();
		List<GuildMember> list2 = new List<GuildMember>(list.Values);
		list2.Sort((GuildMember x, GuildMember y) => (x.mState == y.mState) ? (x.Job - y.Job) : (y.State - x.mState));
		int num = list.Count - GuildMemberItemList.Count;
		int num2 = 0;
		for (int i = 0; i < list2.Count; i++)
		{
			GuildMember guildMember = list2[i];
			if (guildMember.Job != Guild_JOB.JOB_Candidate)
			{
				if (num2 < GuildMemberItemList.Count)
				{
					GuildMemberItemList[num2++].InitGuildMemberInfo(guildMember);
				}
				GuildMemberList.Add(guildMember);
			}
			else
			{
				ApplyList.Add(guildMember);
			}
		}
		UpdateBtn();
		for (int j = 0; j < GuildMemberItemList.Count; j++)
		{
			NGUITools.SetActive(GuildMemberItemList[j].gameObject, j < num2);
		}
		Grid.maxIndex = 0;
		Grid.minIndex = -1 * (GuildMemberList.Count - 1);
		BottomWidget.height = GuildMemberList.Count * GuildMemberItemList[0].CellHeight;
		Grid.SortBasedOnScrollMovement();
		ScrollView.ResetPosition();
		UpdateApplyList();
	}

	public void UpdateApplyList()
	{
		if (SingletonUnity<GuildApplyListLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildApplyListLogic>.Instance.gameObject))
		{
			SingletonUnity<GuildApplyListLogic>.Instance.UpdataApplyList(ApplyList);
		}
	}

	public void OnClickApplyListBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildApplyListLogicRoot, delegate
		{
			SingletonUnity<GuildApplyListLogic>.Instance.UpdataApplyList(ApplyList);
		});
	}

	public void OnClickRecruitMember()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		SingletonUnity<NewGuildUIRootLogic>.Instance.OnClickCloseBtn();
		ChatUIRootLogic.ResetLinkChat(GameDefine.CHAT_LINK_TYPE.GUILD, GameDefine.CHAT_CHANNEL_TYPE.WORLD, playerData.PlayerGuild);
	}

	public void OnClickQuit()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (!playerData.PlayerGuild.CanLeaveGuild())
		{
			MessageBoxLogic.OpenOKCancelBox("#{200111}", "#{100127}", delegate
			{
				Singleton<ObjManager>.Instance.MainPlayer.LeaveGuild();
			});
		}
		else
		{
			MessageBoxLogic.OpenOKCancelBox("#{200114}", "#{100127}", delegate
			{
				Singleton<ObjManager>.Instance.MainPlayer.LeaveGuild();
			});
		}
	}

	public void ShowGuildLogs(List<string> lists)
	{
		if (SingletonUnity<GuildLogUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildLogUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<GuildLogUIRootLogic>.Instance.UpdataLogList(lists);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildLogUIRootLogic, delegate
		{
			SingletonUnity<GuildLogUIRootLogic>.Instance.UpdataLogList(lists);
		});
	}

	public void ChangeGuildNeedAppro()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		bool flag = !playerData.PlayerGuild.IsNeedAppro;
		playerData.PlayerGuild.IsNeedAppro = flag;
		UnityVersionUtil.SetActiveRecursive(ApproBtn, !flag);
		Singleton<ObjManager>.Instance.MainPlayer.SetGuildNeedAppro(flag);
	}

	public void OnClickLogBtn()
	{
		Singleton<ObjManager>.Instance.MainPlayer.LookAtLog();
	}
}
