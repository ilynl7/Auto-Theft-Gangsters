using System;
using System.Collections.Generic;
using Sproto;
using SprotoType;
using UnityEngine;

public class HitOtherPLayerLogic : SingletonUnity<HitOtherPLayerLogic>
{
	private Camera mMainCamera;

	public GameObject mPopMenuOffset;

	public UIGrid mMenuItemGrid;

	private int mMenuItemNum;

	private GameObject mResMenuItem;

	private long TargetServerId;

	private static TargetBasicInfo mTargetBasicInfo;

	private static Vector3 targetPos;

	public UILabel Name;

	public UILabel Level;

	public UILabel CombolLabel;

	public UILabel GuildNameLabel;

	public UISprite Icon;

	public GameObject ErJiJieMian;

	public GameObject JiBenGongNeng;

	public UIGrid ErJiGrid;

	public UISprite BottomPic;

	public UISprite RightPic;

	private HitType currHitType;

	private List<PopMenuItemLogic> PopMenuItemLogicListEnable = new List<PopMenuItemLogic>();

	private List<PopMenuItemLogic> PopMenuItemLogicListDisEnable = new List<PopMenuItemLogic>();

	private static List<object> initParams = new List<object>();

	private bool ScreenFitInitFlag;

	private float mHalfMenuWidth;

	private float mHalfMenuHeight;

	private bool FristInit;

	protected override void Awake()
	{
		base.Awake();
		Init();
	}

	private void Init()
	{
		if (FristInit)
		{
			return;
		}
		FristInit = true;
		foreach (Transform item in ErJiGrid.transform)
		{
			UIEventListener uIEventListener = UIEventListener.Get(item.gameObject);
			uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnClickErJiJieMianItem));
		}
	}

	private void Start()
	{
		mMenuItemNum = 0;
		mMainCamera = Camera.main;
	}

	public static void ShowMenu(HitType hittype, TargetBasicInfo target)
	{
		initParams.Clear();
		initParams.Add(hittype);
		initParams.Add(target);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.HitOtherPlayerRoot, ShowUIOver, initParams);
	}

	private static void ShowUIOver(bool bSucces, object param)
	{
		if (bSucces)
		{
			List<object> list = param as List<object>;
			if (SingletonUnity<HitOtherPLayerLogic>.Exists && list != null)
			{
				SingletonUnity<HitOtherPLayerLogic>.Instance.ShowPopMenu((HitType)(int)list[0], (TargetBasicInfo)list[1]);
			}
		}
	}

	private void ShowPopMenu(HitType strMenuName, TargetBasicInfo target)
	{
		mTargetBasicInfo = target;
		TargetServerId = target.ServerId;
		mMenuItemNum = 0;
		currHitType = strMenuName;
		if (mMainCamera == null)
		{
			mMainCamera = Camera.main;
		}
		SetOffsetPos();
		SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.PopMenuItem, LoadItemOver, strMenuName);
	}

	private void SetOffsetPos()
	{
		if (!ScreenFitInitFlag)
		{
			ScreenFitInitFlag = true;
			mHalfMenuWidth = mPopMenuOffset.transform.localScale.x * (float)(BottomPic.width / 2 + RightPic.width / 2);
			mHalfMenuHeight = mPopMenuOffset.transform.localScale.y * (float)BottomPic.height / 2f;
		}
		Vector2 vector = new Vector2(mTargetBasicInfo.MousePos.x * UIController.ScreenWidthScale, mTargetBasicInfo.MousePos.y * UIController.ScreenHeightScale);
		float x = ((!(vector.x < UIController.ScreenWidth / 2f)) ? (0f - mHalfMenuWidth) : mHalfMenuWidth);
		float y = ((!(vector.y < UIController.ScreenHeight / 2f)) ? (0f - mHalfMenuHeight) : mHalfMenuHeight);
		Vector2 vector2 = new Vector2(x, y);
		vector += vector2;
		vector = new Vector2(Mathf.Clamp(vector.x, 50f, UIController.ScreenWidth - 50f), Mathf.Clamp(vector.y, 50f, UIController.ScreenHeight - 50f));
		mPopMenuOffset.transform.localPosition = new Vector3(vector.x, vector.y, 0f);
	}

	private void LoadItemOver(GameObject resobj, object param)
	{
		if (!(resobj == null))
		{
			mResMenuItem = resobj;
			ShowTargetBasicInfo();
			for (int i = 0; i < PopMenuItemLogicListDisEnable.Count; i++)
			{
				NGUITools.SetActive(PopMenuItemLogicListDisEnable[i].gameObject, state: false);
			}
			if (mMenuItemNum > 0)
			{
				mMenuItemGrid.Reposition();
			}
		}
	}

	private void ShowTargetBasicInfo()
	{
		if (mTargetBasicInfo == null)
		{
			return;
		}
		UnityVersionUtil.SetActiveRecursive(ErJiJieMian, state: false);
		Icon.spriteName = GameDefine.Player_Icon_Small_Pic[(int)mTargetBasicInfo.profession];
		Name.text = mTargetBasicInfo.Name;
		Level.text = $"Lv.{mTargetBasicInfo.Level.ToString()}";
		CombolLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{100421}"), mTargetBasicInfo.ComboValue.ToString());
		GuildNameLabel.text = mTargetBasicInfo.GuildName;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		AddMenuItem(0, StrDictionary.GetDictionaryString("#{100203}"), PopMenuView);
		if (mTargetBasicInfo.OnlineState == 1)
		{
			AddMenuItem(2, StrDictionary.GetDictionaryString("#{100218}"), PopMenuChat);
			if ((playerData.IsHaveTeam() & !playerData.TeamInfo.isTeamMemberById(mTargetBasicInfo.ServerId)) && !playerData.TeamInfo.IsFull())
			{
				AddMenuItem(3, StrDictionary.GetDictionaryString("#{100204}"), PopMenuTeam);
			}
			else if (!playerData.IsHaveTeam())
			{
				AddMenuItem(3, StrDictionary.GetDictionaryString("#{100256}"), PopMenuJoinTeam);
			}
			if (playerData.IsHaveGuild() && !mTargetBasicInfo.isHaveGuild())
			{
				AddMenuItem(4, StrDictionary.GetDictionaryString("#{100205}"), PopMenuInviteJoinGuild);
			}
		}
		if (!playerData.IsHaveGuild() && mTargetBasicInfo.isHaveGuild())
		{
			AddMenuItem(4, StrDictionary.GetDictionaryString("#{100257}"), PopMenuJoinGuild);
		}
		friend_info friendById = playerData.FriendInfo.GetFriendById(mTargetBasicInfo.ServerId);
		if (friendById == null)
		{
			AddMenuItem(5, StrDictionary.GetDictionaryString("#{100228}"), PopMenuAddFriend);
		}
		else
		{
			AddMenuItem(5, StrDictionary.GetDictionaryString("#{100207}"), PopMenuDel);
		}
		if (currHitType == HitType.HitTeamMemberIcon && playerData.IsHaveTeam() && playerData.IsTeamLeader())
		{
			AddMenuItem(6, StrDictionary.GetDictionaryString("#{100825}"), PopRemoveTeamMember);
		}
		if (currHitType == HitType.HitGuildMember)
		{
			if (playerData.PlayerGuild.CanKickedMember(TargetServerId))
			{
				AddMenuItem(6, StrDictionary.GetDictionaryString("#{100729}"), PopRemoveGuildMember);
			}
			if (playerData.PlayerGuild.CanChangeMemberJob(TargetServerId))
			{
				AddMenuItem(7, StrDictionary.GetDictionaryString("#{100728}"), PopChangeDuate);
			}
		}
		friend_info enemyById = playerData.FriendInfo.GetEnemyById(mTargetBasicInfo.ServerId);
		if (enemyById == null)
		{
			AddMenuItem(8, StrDictionary.GetDictionaryString("#{103305}"), PopMenuAddEnemy);
		}
		else
		{
			AddMenuItem(8, StrDictionary.GetDictionaryString("#{103306}"), PopMenuDelEnemy);
		}
	}

	private void AddMenuItem(int Itemid, string strLabel, PopMenuItemLogic.MenuItemOnClicked func)
	{
		if (!(null == mResMenuItem))
		{
			PopMenuItemLogic popMenuItemLogic = null;
			if (PopMenuItemLogicListDisEnable.Count <= 0)
			{
				popMenuItemLogic = SetParent(mResMenuItem, mMenuItemGrid, Itemid);
			}
			else
			{
				popMenuItemLogic = PopMenuItemLogicListDisEnable[PopMenuItemLogicListDisEnable.Count - 1];
				PopMenuItemLogicListDisEnable.RemoveAt(PopMenuItemLogicListDisEnable.Count - 1);
			}
			if (popMenuItemLogic != null)
			{
				popMenuItemLogic.InitMenuItem(strLabel, func);
				PopMenuItemLogicListEnable.Add(popMenuItemLogic);
				mMenuItemNum++;
			}
		}
	}

	private PopMenuItemLogic SetParent(GameObject child, UIGrid parent, int Id)
	{
		if (child == null || parent == null)
		{
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(child) as GameObject;
		parent.AddChild(gameObject.transform);
		gameObject.transform.localScale = Vector3.one;
		if (Id != -1)
		{
			gameObject.name = $"MenuItem{Id.ToString()}";
		}
		parent.Reposition();
		return gameObject.GetComponent<PopMenuItemLogic>();
	}

	private void PopRemoveTeamMember()
	{
		Close();
		team_kick.request request = new team_kick.request();
		request.characterId = mTargetBasicInfo.ServerId;
		NetLogic.GetInstance().Send<Protocol.team_kick>(request);
	}

	private void PopRemoveGuildMember()
	{
		Close();
		MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{200112}", mTargetBasicInfo.Name), StrDictionary.GetDictionaryString("#{100127}"), OnRemove);
	}

	private void OnRemove()
	{
		if (mTargetBasicInfo != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.KickGuildMember(TargetServerId);
		}
		Close();
	}

	private void PopChangeDuate()
	{
		UnityVersionUtil.SetActiveRecursive(ErJiJieMian, state: true);
	}

	public void OnClickCloseErjiJieMian()
	{
		if (UnityVersionUtil.IsActive(ErJiJieMian.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(ErJiJieMian, state: false);
		}
		else
		{
			Close();
		}
	}

	private void SetErJiJIeMianBtn(Guild_JOB job)
	{
		ErJiGrid.Reposition();
	}

	private void PopMenuAddFriend()
	{
		if (mTargetBasicInfo != null)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SOCIAL_APPLY))
			{
				FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
				if (friendInfo.IsCanAddFriend())
				{
					add_friend.request request = new add_friend.request();
					request.characterId = TargetServerId;
					NetLogic.GetInstance().Send<Protocol.add_friend>(request);
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Social", "Friend", "apply_times");
				}
				else
				{
					NoticeLogic.AddNotifyData("#{100265}");
				}
			}
			else
			{
				int condition = DataManager.GetFunctionDataById(4062.ToString()).Condition;
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", condition));
			}
		}
		Close();
	}

	private void PopMenuAddEnemy()
	{
		if (mTargetBasicInfo != null)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SOCIAL_APPLY))
			{
				FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
				if (friendInfo.IsCanAddEnemy())
				{
					add_friend.request request = new add_friend.request();
					request.characterId = TargetServerId;
					request.type = 1L;
					NetLogic.GetInstance().Send<Protocol.add_friend>(request);
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Social", "Enemy", "add_times");
				}
				else
				{
					NoticeLogic.AddNotifyData("#{100265}");
				}
			}
			else
			{
				int condition = DataManager.GetFunctionDataById(4062.ToString()).Condition;
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", condition));
			}
		}
		Close();
	}

	private void PopMenuTeam()
	{
		if (mTargetBasicInfo != null)
		{
			req_invite_team.request request = new req_invite_team.request();
			request.characterid = TargetServerId;
			NetLogic.GetInstance().Send<Protocol.req_invite_team>(request);
		}
		Close();
	}

	private void PopMenuJoinGuild()
	{
		if (mTargetBasicInfo != null)
		{
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.GUILD))
			{
				guild_join.request request = new guild_join.request();
				request.guildId = mTargetBasicInfo.GuildId;
				NetLogic.GetInstance().Send<Protocol.guild_join>(request);
			}
			else
			{
				int condition = DataManager.GetFunctionDataById(3017.ToString()).Condition;
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", condition));
			}
		}
		Close();
	}

	private void PopMenuInviteJoinGuild()
	{
		if (mTargetBasicInfo != null)
		{
			guild_invite.request request = new guild_invite.request();
			request.id = mTargetBasicInfo.ServerId;
			NetLogic.GetInstance().Send<Protocol.guild_invite>(request);
		}
		Close();
	}

	private void PopMenuJoinTeam()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.TEAM))
		{
			if (mTargetBasicInfo != null)
			{
				req_other_team.request request = new req_other_team.request();
				request.id = mTargetBasicInfo.ServerId;
				NetLogic.GetInstance().Send<Protocol.req_other_team>(request);
			}
			Close();
		}
		else
		{
			int condition = DataManager.GetFunctionDataById(3018.ToString()).Condition;
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{100154}", condition));
		}
	}

	private void PopMenuDel()
	{
		Close();
		if (mTargetBasicInfo != null)
		{
			MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{100247}", mTargetBasicInfo.Name), StrDictionary.GetDictionaryString("#{100244}"), OnConfirmClick);
		}
	}

	private void PopMenuDelEnemy()
	{
		Close();
		if (mTargetBasicInfo != null)
		{
			MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{103319}", mTargetBasicInfo.Name), StrDictionary.GetDictionaryString("#{100244}"), OnConfirmClickDelEnemy);
		}
	}

	private void OnConfirmClick()
	{
		if (mTargetBasicInfo != null)
		{
			del_friend.request request = new del_friend.request();
			request.characterId = TargetServerId;
			NetLogic.GetInstance().Send<Protocol.del_friend>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Social", "Friend", "del_times");
		}
		Close();
	}

	private void OnConfirmClickDelEnemy()
	{
		if (mTargetBasicInfo != null)
		{
			del_friend.request request = new del_friend.request();
			request.characterId = TargetServerId;
			request.type = 1L;
			NetLogic.GetInstance().Send<Protocol.del_friend>(request);
			FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
			friendInfo.RemoveEnemy(TargetServerId);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Social", "Enemy", "del_times");
		}
		Close();
	}

	private void PopMenuChat()
	{
		if (mTargetBasicInfo != null)
		{
			ChatUIRootLogic.ResetPrivateChat(TargetServerId, mTargetBasicInfo.Name, mTargetBasicInfo.profession);
		}
		Close();
		if (SingletonUnity<SocialUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SocialUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<SocialUIRootLogic>.Instance.OnClickCloseBtn();
		}
		if (SingletonUnity<NewGuildUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewGuildUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewGuildUIRootLogic>.Instance.OnClickCloseBtn();
		}
		if (SingletonUnity<TeamUIRootNewLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TeamUIRootNewLogic>.Instance.gameObject))
		{
			SingletonUnity<TeamUIRootNewLogic>.Instance.OnClickCloseBtn();
		}
	}

	private void OnChatRootShow(bool isSuccess, object param)
	{
		if (isSuccess)
		{
			GameDefine.CHAT_CHANNEL_TYPE choosedChannelType = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ChoosedChannelType;
			SingletonUnity<ChatUIRootLogic>.Instance.Reset(choosedChannelType);
		}
	}

	private void PopMenuView()
	{
		if (mTargetBasicInfo != null)
		{
			ask_character_info.request request = new ask_character_info.request();
			request.characterId = TargetServerId;
			NetLogic.GetInstance().Send<Protocol.ask_character_info>(request, Ret_Ask_Character_info);
		}
		Close();
	}

	private void Ret_Ask_Character_info(SprotoTypeBase req)
	{
		if (!UIManager.IsUnlockTutorialEnable() && req is ask_character_info.response { HasCharacter: not false } response)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OtherPlayerInfoUILogicRoot, OnOpenOtherPlayerInfoMenuRoot, response.character);
		}
	}

	private void OnOpenOtherPlayerInfoMenuRoot(bool success, object param)
	{
		character_look character_look = param as character_look;
		if (!success || !SingletonUnity<OtherPlayerInfoUILogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<OtherPlayerInfoUILogic>.Instance.gameObject))
		{
			return;
		}
		SingletonUnity<OtherPlayerInfoUILogic>.Instance.EnableReset();
		if (character_look != null)
		{
			List<GameItem> list = new List<GameItem>();
			if (character_look.HasEquip)
			{
				list = GetGameItemList(new List<gameitem>(character_look.equip.Values));
			}
			PROFESSION_TYPE type = (PROFESSION_TYPE)character_look.general.profession;
			string modeName = ServerToClientTools.GetModeName(character_look.visual.HeadId);
			CharacterAttributeData characterAttributeData = ServerToClientTools.attributeToCharacterAttributeData(character_look);
			characterAttributeData.Name = mTargetBasicInfo.Name;
			SingletonUnity<OtherPlayerInfoUILogic>.Instance.ResetOtherPlayerInfo(list, type, modeName, characterAttributeData, character_look);
		}
	}

	private List<GameItem> GetGameItemList(List<gameitem> list)
	{
		List<GameItem> list2 = new List<GameItem>();
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != null)
			{
				GameItem gameItem = ServerToClientTools.ServerGameItemToClientGameItem(list[i]);
				if (gameItem != null)
				{
					list2.Add(gameItem);
				}
			}
		}
		return list2;
	}

	private void PopMenuReport()
	{
		Close();
	}

	private void PopMenuFollow()
	{
		if (mTargetBasicInfo != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.FollowServerID = TargetServerId;
		}
		else
		{
			Singleton<ObjManager>.Instance.MainPlayer.FollowServerID = -1L;
		}
		Close();
	}

	private void OnClickErJiJieMianItem(GameObject obj)
	{
		Guild_JOB guild_JOB = Guild_JOB.NO_JOB;
		switch (obj.name)
		{
		case "01_BossBtn":
			MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{100787}", mTargetBasicInfo.Name), StrDictionary.GetDictionaryString("#{100127}"), OnSure);
			return;
		case "04_MemberBtn":
			guild_JOB = Guild_JOB.JOB_Member;
			break;
		case "03_ElderBtn":
			guild_JOB = Guild_JOB.JOB_Elder;
			break;
		case "02_ViceBtn":
			guild_JOB = Guild_JOB.JOB_VicePresident;
			break;
		}
		if (mTargetBasicInfo != null && guild_JOB != Guild_JOB.NO_JOB)
		{
			Singleton<ObjManager>.Instance.MainPlayer.ChangeMemberJob(TargetServerId, guild_JOB);
		}
		Close();
	}

	private void OnSure()
	{
		Close();
		if (mTargetBasicInfo != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.ChangeMemberJob(TargetServerId, Guild_JOB.JOB_Chief);
		}
	}

	public void Close()
	{
		for (int num = PopMenuItemLogicListEnable.Count - 1; num >= 0; num--)
		{
			PopMenuItemLogicListDisEnable.Add(PopMenuItemLogicListEnable[num]);
			PopMenuItemLogicListEnable.RemoveAt(num);
		}
		mMenuItemNum = 0;
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.HitOtherPlayerRoot);
	}

	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			Close();
		}
	}
}
