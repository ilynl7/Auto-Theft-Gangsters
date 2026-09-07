using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class TeamInviteRootLogic : SingletonUnity<TeamInviteRootLogic>
{
	public Transform ChooseObjTrans;

	public Transform GuildBtnTrans;

	public Transform NearByBtnTrans;

	public Transform FriendBtnTrans;

	public Transform RootOffSet;

	private int mCurPage = -1;

	private List<friend_info> mCurNearByList = new List<friend_info>();

	private List<GuildMember> mCurGuildMamberList = new List<GuildMember>();

	private List<friend_info> mCurFriendList = new List<friend_info>();

	private PlayerData mPlayerData;

	private TwoColumnPlayerInfoPage mPlayerInfoPage;

	private new void Awake()
	{
		base.Awake();
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.TwoColumPlayerInfoPage, OnLoadPlayerInfoPage);
		mCurPage = -1;
		OnClickGuildBtn();
	}

	private void OnLoadPlayerInfoPage(GameObject newObj, object param)
	{
		GameObject gameObject = Object.Instantiate(newObj) as GameObject;
		gameObject.transform.parent = RootOffSet;
		gameObject.transform.localPosition = new Vector3(0f, 15.81504f, 0f);
		gameObject.transform.localScale = Vector3.one;
		mPlayerInfoPage = gameObject.GetComponent<TwoColumnPlayerInfoPage>();
	}

	public void ResetPage(int pageIndex)
	{
		List<PlayerInfoItemData> list = new List<PlayerInfoItemData>();
		mCurPage = pageIndex;
		if (mCurPage == 0)
		{
			if (mCurGuildMamberList.Count > 0)
			{
				for (int i = 0; i < mCurGuildMamberList.Count; i++)
				{
					if (mCurGuildMamberList[i].State != 0 && mCurGuildMamberList[i].ServerId != PlayerData.MainPlayerServerId)
					{
						PlayerInfoItemData playerInfoItemData = new PlayerInfoItemData();
						playerInfoItemData.Profession = (int)mCurGuildMamberList[i].Profession;
						playerInfoItemData.Name = mCurGuildMamberList[i].MemberName;
						playerInfoItemData.Level = mCurGuildMamberList[i].Level;
						playerInfoItemData.ComboVal = mCurGuildMamberList[i].ComboValue;
						playerInfoItemData.Key = mCurGuildMamberList[i].ServerId;
						playerInfoItemData.IsEnable = !mPlayerData.TeamInfo.InvitedPlyaer.Contains(mCurGuildMamberList[i].ServerId);
						playerInfoItemData.GuildId = mPlayerData.PlayerGuild.ServerId;
						playerInfoItemData.GuildName = mPlayerData.PlayerGuild.GuilName;
						list.Add(playerInfoItemData);
					}
				}
			}
			ChooseObjTrans.localPosition = GuildBtnTrans.localPosition - Vector3.up * 5f;
		}
		else if (mCurPage == 1)
		{
			if (mCurNearByList.Count > 0)
			{
				for (int j = 0; j < mCurNearByList.Count; j++)
				{
					if (!mPlayerData.TeamInfo.isTeamMemberById(mCurNearByList[j].characterId))
					{
						PlayerInfoItemData playerInfoItemData2 = new PlayerInfoItemData();
						playerInfoItemData2.Profession = (int)mCurNearByList[j].profession;
						playerInfoItemData2.Name = mCurNearByList[j].name;
						playerInfoItemData2.Level = (int)mCurNearByList[j].level;
						playerInfoItemData2.ComboVal = (int)mCurNearByList[j].combValue;
						playerInfoItemData2.Key = mCurNearByList[j].characterId;
						playerInfoItemData2.IsEnable = !mPlayerData.TeamInfo.InvitedPlyaer.Contains(mCurNearByList[j].characterId);
						playerInfoItemData2.GuildId = mCurNearByList[j].guildId;
						playerInfoItemData2.GuildName = mCurNearByList[j].guildName;
						list.Add(playerInfoItemData2);
					}
				}
			}
			ChooseObjTrans.localPosition = NearByBtnTrans.localPosition - Vector3.up * 5f;
		}
		else if (mCurPage == 2)
		{
			if (mCurFriendList.Count > 0)
			{
				for (int k = 0; k < mCurFriendList.Count; k++)
				{
					if (mCurFriendList[k].state != 0L)
					{
						PlayerInfoItemData playerInfoItemData3 = new PlayerInfoItemData();
						playerInfoItemData3.Profession = (int)mCurFriendList[k].profession;
						playerInfoItemData3.Name = mCurFriendList[k].name;
						playerInfoItemData3.Level = (int)mCurFriendList[k].level;
						playerInfoItemData3.ComboVal = (int)mCurFriendList[k].combValue;
						playerInfoItemData3.Key = mCurFriendList[k].friendId;
						playerInfoItemData3.IsEnable = !mPlayerData.TeamInfo.InvitedPlyaer.Contains(mCurFriendList[k].friendId);
						playerInfoItemData3.GuildId = mCurFriendList[k].guildId;
						playerInfoItemData3.GuildName = mCurFriendList[k].guildName;
						list.Add(playerInfoItemData3);
					}
				}
			}
			ChooseObjTrans.localPosition = FriendBtnTrans.localPosition - Vector3.up * 5f;
		}
		mPlayerInfoPage.Reset(list, StrDictionary.GetDictionaryString("#{100815}"), StrDictionary.GetDictionaryString("#{100814}"), OnClickInviteBtn);
	}

	private void OnClickInviteBtn(long key)
	{
		req_invite_team.request request = new req_invite_team.request();
		request.characterid = key;
		request.goalId = mPlayerData.TeamInfo.TeamGoalData.ID;
		request.minLevel = mPlayerData.TeamInfo.MinLimitLevel;
		request.maxLevel = mPlayerData.TeamInfo.MaxLimitLevel;
		NetLogic.GetInstance().Send<Protocol.req_invite_team>(request);
		mPlayerData.TeamInfo.InvitedPlyaer.Add(request.characterid);
	}

	public void UpdateNearByPlayerList()
	{
		WaitResponseUIRootLogic.CloseBox();
		mCurNearByList = new List<friend_info>(mPlayerData.FriendInfo.MainPlayerRandomFriendDic.Values);
		ResetPage(1);
	}

	public void UpdateGuildMemberInfo()
	{
		mCurGuildMamberList = new List<GuildMember>(mPlayerData.PlayerGuild.GuildMemberList.Values);
		ResetPage(0);
	}

	public void UpdateFriendListInfo()
	{
		WaitResponseUIRootLogic.CloseBox();
		mCurFriendList = new List<friend_info>(mPlayerData.FriendInfo.MainPlayerFriendDic.Values);
		ResetPage(2);
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TeamInviteRoot);
	}

	public void OnClickGuildBtn()
	{
		if (mPlayerData.IsHaveGuild())
		{
			if (mPlayerData.PlayerGuild.GuildMemberList != null && mPlayerData.PlayerGuild.GuildMemberList.Count > 0)
			{
				UpdateGuildMemberInfo();
			}
			Singleton<ObjManager>.Instance.MainPlayer.ApplyUpdataGuildMemberList();
			WaitResponseUIRootLogic.OpenWaitBox(170, 10f, 0f);
		}
		else
		{
			ResetPage(0);
		}
	}

	public void OnClickNearByBtn()
	{
		WaitResponseUIRootLogic.OpenWaitBox(159, 10f, 0f);
		req_random_online_character_list.request request = new req_random_online_character_list.request();
		request.characterId = PlayerData.MainPlayerServerId;
		NetLogic.GetInstance().Send<Protocol.req_random_online_character_list>(request);
	}

	public void OnClickFriendBtn()
	{
		WaitResponseUIRootLogic.OpenWaitBox(126, 10f, 0f);
		request_update_friend_useinfo.request request = new request_update_friend_useinfo.request();
		request.characterId = PlayerData.MainPlayerServerId;
		NetLogic.GetInstance().Send<Protocol.request_update_friend_useinfo>(request);
	}
}
