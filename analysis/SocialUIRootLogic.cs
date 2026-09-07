using System.Collections.Generic;
using SprotoType;

public class SocialUIRootLogic : SingletonUnity<SocialUIRootLogic>
{
	private int curPageIndex = -1;

	private int state;

	public void InitSocialUI()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
			List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
			MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickMailBtn, isIcon: true, "CZ_left_Mail", StrDictionary.GetDictionaryString("#{100215}"), FUNCTION_TYPE.SOCIAL_MAIL, friendInfo.IsHaveMailTips);
			MenuTabBtnInfo item2 = new MenuTabBtnInfo(OnClickFriendBtn, isIcon: true, "CZ_left_Friend", StrDictionary.GetDictionaryString("#{100212}"), FUNCTION_TYPE.SOCIAL_FRIEND);
			MenuTabBtnInfo item3 = new MenuTabBtnInfo(OnClickEnemyBtn, isIcon: true, "CZ_left_chouRen", StrDictionary.GetDictionaryString("#{103301}"), FUNCTION_TYPE.SOCIAL_ENEMY);
			MenuTabBtnInfo item4 = new MenuTabBtnInfo(OnClickFriendListBtn, isIcon: true, "CZ_left_Request", StrDictionary.GetDictionaryString("#{100214}"), FUNCTION_TYPE.SOCIAL_APPLY, friendInfo.IshavefriendApply);
			list.Add(item);
			list.Add(item2);
			list.Add(item3);
			list.Add(item4);
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(list, OnClickCloseBtn);
			curPageIndex = -1;
		});
	}

	public void SelectFriendInfobtn()
	{
		FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
		if (friendInfo.IshavefriendApply())
		{
			OnClickFriendListBtn();
		}
		else
		{
			OnClickFriendBtn();
		}
	}

	public void OnClickFriendBtn()
	{
		if (curPageIndex != 1)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EnemyUIRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FriendApplyUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MailUIRootLogic);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.FriendUIRootLogic, delegate
			{
				SingletonUnity<FriendUIRootLogic>.Instance.EnableReset();
				SingletonUnity<FriendUIRootLogic>.Instance.UpdateFriendList();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(1);
			curPageIndex = 1;
		}
	}

	public void OnClickEnemyBtn()
	{
		if (curPageIndex != 2)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FriendUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FriendApplyUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MailUIRootLogic);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.EnemyUIRoot, delegate
			{
				WaitResponseUIRootLogic.OpenWaitBox(126, 10f, 0f);
				request_update_friend_useinfo.request rpcReq = new request_update_friend_useinfo.request
				{
					type = 1L
				};
				NetLogic.GetInstance().Send<Protocol.request_update_friend_useinfo>(rpcReq);
				SingletonUnity<EnemyUIRootLogic>.Instance.EnableReset();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(2);
			curPageIndex = 2;
		}
	}

	public void OnClickFriendListBtn()
	{
		if (curPageIndex != 3)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FriendUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EnemyUIRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MailUIRootLogic);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.FriendApplyUIRootLogic, delegate
			{
				SingletonUnity<FriendApplyUIRootLogic>.Instance.UpdateFriendList();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(3);
			curPageIndex = 3;
		}
	}

	public void OnClickMailBtn()
	{
		if (curPageIndex != 0)
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FriendUIRootLogic);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EnemyUIRoot);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FriendApplyUIRootLogic);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MailUIRootLogic, delegate
			{
				SingletonUnity<MailUIRootLogic>.Instance.Reset();
			});
			SingletonUnity<MenuBaseRootLogic>.Instance.SetTargetBtnToggleEnable(0);
			curPageIndex = 0;
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FriendUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.EnemyUIRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.FriendApplyUIRootLogic);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MailUIRootLogic);
		CloseSocialUI();
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
	}

	public void ShowSocialUI()
	{
		if (state == 0)
		{
			InitSocialUI();
			state = 1;
		}
	}

	public void CloseSocialUI()
	{
		state = 0;
	}

	public void UpdateSocialInfo()
	{
		if (SingletonUnity<FriendAddUILogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FriendAddUILogic>.Instance.gameObject))
		{
			SingletonUnity<FriendAddUILogic>.Instance.UpdateFriendList();
		}
		if (SingletonUnity<FriendUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FriendUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<FriendUIRootLogic>.Instance.UpdateFriendList();
		}
		if (SingletonUnity<MailUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MailUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<MailUIRootLogic>.Instance.UpdateMailList();
		}
		if (SingletonUnity<FriendApplyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FriendApplyUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<FriendApplyUIRootLogic>.Instance.UpdateFriendList();
		}
		if (SingletonUnity<EnemyUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<EnemyUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<EnemyUIRootLogic>.Instance.UpdateEnemyList();
		}
	}
}
