using Sproto;
using SprotoType;
using UnityEngine;

public class LoginRootLogic : SingletonUnity<LoginRootLogic>
{
	public UILabel ServerLabel;

	public UISprite ServerStatePic;

	public UILabel VersionLabel;

	public GameObject ChooseServerRoot;

	public UITexture BottomPic;

	private string mNoticeStr;

	private string mNoticeVersion;

	private float lastUpdateServerTime;

	private float UpdateDeltaTime = 10f;

	private new void Awake()
	{
		base.Awake();
	}

	public void Reset(string noticeStr, string noticeVersion)
	{
		lastUpdateServerTime = Time.time;
		if (PlayerData.CurGameServerData == null)
		{
			SingletonUnity<MenuSceneController>.Instance.ChooseRecommendServer();
		}
		if (PlayerData.CurGameServerData != null)
		{
			ServerLabel.text = PlayerData.CurGameServerData.serverName;
			if (PlayerData.CurGameServerData.HasNewServer && PlayerData.CurGameServerData.newServer == 1 && PlayerData.CurGameServerData.serverState != 3)
			{
				ServerStatePic.color = Color.red;
			}
			else
			{
				ServerStatePic.color = GameDefine.SERVER_STATE_COLOR[(int)PlayerData.CurGameServerData.serverState];
			}
			ServerLabel.color = Color.white;
			ServerStatePic.enabled = true;
		}
		else
		{
			ServerLabel.text = StrDictionary.GetDictionaryString("#{200074}");
			ServerLabel.color = Color.grey;
			ServerStatePic.enabled = false;
		}
		VersionLabel.text = $"Version:{GameSettingData.GameVersion}";
		bool flag = true;
		if (!string.IsNullOrEmpty(noticeStr))
		{
			mNoticeStr = noticeStr;
			if (!string.IsNullOrEmpty(noticeVersion))
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.LoginNoticeRootUI, delegate
				{
					SingletonUnity<LoginNoticeRootLogic>.Instance.Reset(mNoticeStr);
				});
				flag = false;
			}
		}
		if (flag)
		{
			AccountVersionCheckRootLogic.CheckFaceBookBindTips(1);
		}
	}

	private void UpdateGameServer()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurLoginServerData.LoginIP, PlayerData.CurLoginServerData.LoginPort, delegate
		{
			NetLogic.GetInstance().Send<Protocol.update_game_server>(null, UpdateGameServerResponse);
		});
	}

	public void UpdateGameServerResponse(SprotoTypeBase req)
	{
		if (!(req is update_game_server.response { HasGame_server: not false } response))
		{
			return;
		}
		SingletonUnity<MenuSceneController>.Instance.UpdateServerList(response.game_server);
		if (PlayerData.CurGameServerData != null)
		{
			ServerLabel.text = PlayerData.CurGameServerData.serverName;
			if (PlayerData.CurGameServerData.HasNewServer && PlayerData.CurGameServerData.newServer == 1 && PlayerData.CurGameServerData.serverState != 3)
			{
				ServerStatePic.color = Color.red;
			}
			else
			{
				ServerStatePic.color = GameDefine.SERVER_STATE_COLOR[(int)PlayerData.CurGameServerData.serverState];
			}
			ServerLabel.color = Color.white;
			ServerStatePic.enabled = true;
		}
		if (SingletonUnity<ChooseServerRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ChooseServerRootLogic>.Instance.gameObject))
		{
			SingletonUnity<ChooseServerRootLogic>.Instance.RefershServerList();
		}
	}

	public void OnClickLoginBtn()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.CanSendToServer(4))
		{
			NoticeLogic.AddNotifyData("#{200066}");
		}
		else if (PlayerData.CurGameServerData == null)
		{
			OnClickChooseServerBtn();
		}
		else if (PlayerData.CurGameServerData.HasServerState && PlayerData.CurGameServerData.serverState == 3)
		{
			MessageBoxLogic.OpenOKBox("#{200128}", "#{100127}");
			if (Time.time - lastUpdateServerTime > UpdateDeltaTime)
			{
				lastUpdateServerTime = Time.time;
				UpdateGameServer();
			}
		}
		else
		{
			WaitResponseUIRootLogic.OpenWaitBox(4, 0f, 0f);
			if (GameSettingData.IsLocalTestServer)
			{
				SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurGameServerData.serverIP, (int)PlayerData.CurGameServerData.serverPort);
			}
			else
			{
				SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurGameServerData.serverIP, (int)PlayerData.CurGameServerData.serverPort);
			}
		}
	}

	private void ConnectLost()
	{
	}

	private void ConnectSuccess(bool isSuccess)
	{
		if (isSuccess)
		{
			string playerAccountId = PlayerData.GetPlayerAccountId();
			if (string.IsNullOrEmpty(playerAccountId))
			{
				VisitorRequest();
			}
			else
			{
				VerifyRequest();
			}
		}
		else
		{
			WaitResponseUIRootLogic.CloseBox();
			NoticeLogic.AddNotifyData("#{200065}");
		}
	}

	private void VisitorRequest()
	{
		visitor.request rpcReq = new visitor.request();
		NetLogic.GetInstance().Send<Protocol.visitor>(rpcReq, VisitorResponse);
	}

	private void VisitorResponse(SprotoTypeBase rep)
	{
		if (rep is visitor.response { state: 0L } response)
		{
			PlayerData.SavePlayerAccountId(response.id);
			PlayerData.SavePlayerAccountKey(response.key);
			VerifyRequest();
		}
		else
		{
			Debug.LogError("create  erro!");
			NetLogic.GetInstance().DisconnectServer();
		}
	}

	private void VerifyRequest()
	{
		verfiy.request request = new verfiy.request();
		request.id = PlayerData.GetPlayerAccountId();
		request.key = PlayerData.GetPlayerAccountKey();
		NetLogic.GetInstance().Send<Protocol.verfiy>(request, VerifyResponse);
	}

	private void VerifyResponse(SprotoTypeBase req)
	{
		if (req is verfiy.response response)
		{
			if (response.state == 0L)
			{
				PlayerData.session = response.session;
				PlayerData.loginType = 1;
				SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurGameServerData.serverIP, (int)PlayerData.CurGameServerData.serverPort);
			}
			else if (response.state == 2)
			{
				NoticeLogic.AddNotifyData("#{100147}");
				PlayerData.session = response.session;
				PlayerData.loginType = 2;
				SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurGameServerData.serverIP, (int)PlayerData.CurGameServerData.serverPort);
			}
			else
			{
				VisitorRequest();
			}
		}
	}

	public void OnClickChooseServerBtn()
	{
		if (PlayerData.loginType == 2 && PlayerData.CurGameServerData != null)
		{
			MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{200073}", PlayerData.CurGameServerData.serverName), "#{100127}", OnClickLoginBtn);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ChooseServerRootUI, delegate
		{
			SingletonUnity<ChooseServerRootLogic>.Instance.Reset();
			if (Time.time - lastUpdateServerTime > UpdateDeltaTime)
			{
				lastUpdateServerTime = Time.time;
				UpdateGameServer();
			}
		});
	}

	public void OnClickreLogin()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.AccountVersionCheckRootUI, delegate
		{
			SingletonUnity<AccountVersionCheckRootLogic>.Instance.ReLoginVerfiyAccount();
		});
	}

	public void OnClickNoticeBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.LoginNoticeRootUI, delegate
		{
			SingletonUnity<LoginNoticeRootLogic>.Instance.Reset(mNoticeStr);
		});
	}
}
