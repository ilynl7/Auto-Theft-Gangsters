using System;
using Sproto;
using SprotoType;
using UnityEngine;

public class NetManager : SingletonDontDestoryUnity<NetManager>
{
	private const int MAX_HEART_BEAT_SECOND = 15;

	private int connectGameServer = -1;

	private float curTime = 15f;

	private int netTime = -1;

	private long sendTime2;

	private long sendTime1;

	private static float checkTime = -1f;

	private string lastCheckVersion = string.Empty;

	public int NetDelayTime => netTime;

	protected override void Awake()
	{
		base.Awake();
		if (SingletonDontDestoryUnity<NetManager>.Exists && base.IsInit)
		{
			NetLogic.GetInstance();
		}
	}

	public void LeaveGame()
	{
		NetLogic.GetInstance().Send<Protocol.leave_game>();
		NetLogic.GetInstance().SendLast();
		NetLogic.GetInstance().DisconnectServer();
		connectGameServer = -1;
	}

	private void OnApplicationQuit()
	{
		LeaveGame();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public void ConnectToServer(string ip, int nport, NetLogic.ConnectDelegate delConnect = null, NetLogic.ConnectLostDelegate delConnectLost = null)
	{
		if (delConnect != null)
		{
			NetLogic.SetConnectDelegate(delConnect);
		}
		else
		{
			NetLogic.SetConnectDelegate(ConnectSuccess);
		}
		if (delConnectLost != null)
		{
			NetLogic.SetConnectLostDelegate(delConnectLost);
		}
		else
		{
			NetLogic.SetConnectLostDelegate(ConnectLost);
		}
		NetLogic.GetInstance().ConnectToServer(ip, nport, 500);
	}

	private void ConnectSuccess(bool isSuccess)
	{
		if (isSuccess)
		{
			sendTime2 = 0L;
			LoginRequest(PlayerData.loginType);
		}
		else
		{
			WaitResponseUIRootLogic.CloseBox();
			NoticeLogic.AddNotifyData("#{200065}");
		}
	}

	public void QueueFinish()
	{
		LoginRequest(PlayerData.loginType);
	}

	private void LoginRequest(long logintype)
	{
		login.request request = new login.request();
		request.session = PlayerData.session;
		request.id = PlayerData.GetPlayerAccountId();
		request.logintype = logintype;
		request.version = GameSettingData.GameVersion;
		request.time = (int)(Time.realtimeSinceStartup * 100f);
		if (PlayerData.CurGameServerData != null)
		{
			request.serverId = PlayerData.CurGameServerData.serverId;
		}
		request.unityVersion = GameSettingData.UnityVersion;
		NetLogic.GetInstance().Send<Protocol.login>(request, LoginResponse);
		NetLogic.GetInstance().StartReconnecting();
	}

	public void PickResponse(SprotoTypeBase req)
	{
		character_pick.response response = req as character_pick.response;
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		if (response == null || !response.HasErrno)
		{
			return;
		}
		if (response.errno == 0L)
		{
			WaitResponseUIRootLogic.CloseBox();
			NoticeLogic.AddNotifyData("#{100153}");
			LeaveGame();
			if (!Application.loadedLevelName.Equals(GameDefine.LOGIN_SCENE_NAME))
			{
				LoadingWindow.LoadScene(0);
			}
		}
		else if (response.errno == 2)
		{
			WaitResponseUIRootLogic.CloseBox();
			NoticeLogic.AddNotifyData("#{100174}");
			LeaveGame();
			if (!Application.loadedLevelName.Equals(GameDefine.LOGIN_SCENE_NAME))
			{
				LoadingWindow.LoadScene(0);
			}
		}
	}

	private void LoginResponse(SprotoTypeBase req)
	{
		NetLogic.GetInstance().FinishReconnecting();
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		login.response response = req as login.response;
		bool isFinishDownload = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload;
		if (response == null)
		{
			return;
		}
		if (response.type > 1)
		{
			GameManager.OnLineState = true;
			connectGameServer = 0;
			instance.PlayerCommonData.ServerLevel = (int)response.serverLevel;
			if (Application.loadedLevelName.Equals(GameDefine.LOGIN_SCENE_NAME))
			{
				if (SingletonUnity<CreateRoleRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CreateRoleRootLogic>.Instance.gameObject))
				{
					WaitResponseUIRootLogic.CloseBox();
				}
				else
				{
					if (response.HasVersionCode && IsNeedQuitUpdate(response.versionCode, delegate
					{
						if (PlayerData.downLoadFlag == 1 && int.Parse(response.dataVersionCode) != PlayerData.LocalDataVersion)
						{
							WaitResponseUIRootLogic.CloseBox();
							SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LoginRootUI);
							if (!int.TryParse(response.dataVersionCode, out PlayerData.ServerDataVersion))
							{
								PlayerData.ServerDataVersion = -1;
							}
							SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.AccountVersionCheckRootUI, delegate
							{
								SingletonUnity<AccountVersionCheckRootLogic>.Instance.ResetDownLoadPage(isVerifyDownload: false);
							});
						}
						else
						{
							if (!int.TryParse(response.dataVersionCode, out PlayerData.ServerDataVersion))
							{
								PlayerData.ServerDataVersion = -1;
							}
							CharacterListRequest();
						}
					}))
					{
						return;
					}
					if (PlayerData.downLoadFlag == 1 && int.Parse(response.dataVersionCode) != PlayerData.LocalDataVersion)
					{
						WaitResponseUIRootLogic.CloseBox();
						SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LoginRootUI);
						if (!int.TryParse(response.dataVersionCode, out PlayerData.ServerDataVersion))
						{
							PlayerData.ServerDataVersion = -1;
						}
						SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.AccountVersionCheckRootUI, delegate
						{
							SingletonUnity<AccountVersionCheckRootLogic>.Instance.ResetDownLoadPage(isVerifyDownload: false);
						});
					}
					else
					{
						if (!int.TryParse(response.dataVersionCode, out PlayerData.ServerDataVersion))
						{
							PlayerData.ServerDataVersion = -1;
						}
						CharacterListRequest();
					}
				}
				return;
			}
			WaitResponseUIRootLogic.CloseBox();
			if (isFinishDownload && !IsVersionSame(response))
			{
				NoticeLogic.AddNotifyData("#{200064}");
				LeaveGame();
				LoadingWindow.LoadScene(0);
				connectGameServer = -1;
				return;
			}
			SingletonDontDestoryUnity<GameManager>.Instance.QueryInventory();
			SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.OnReconnectSuccess();
			NoticeLogic.AddNotifyData("#{100146}");
			if (!SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsSingleCopyScene())
			{
				Singleton<ObjManager>.Instance.RecycleAllNPC();
				Singleton<ObjManager>.Instance.RecycleAllOtherPlayer();
				if (SingletonUnity<SurveyProgressLineLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SurveyProgressLineLogic>.Instance.gameObject))
				{
					SingletonUnity<SurveyProgressLineLogic>.Instance.StopSurveyItem();
				}
			}
			refresh_online_state.request request = new refresh_online_state.request();
			request.id = PlayerData.MainPlayerServerId;
			if (Singleton<ObjManager>.Instance.MainPlayer == null)
			{
				request.type = 1L;
				if (!LoadingWindow.isSendMapReady)
				{
					NetLogic.GetInstance().Send<Protocol.map_ready>();
					LoadingWindow.isSendMapReady = true;
				}
			}
			else
			{
				request.type = 0L;
				if (!LoadingWindow.isSendMapReady)
				{
					NetLogic.GetInstance().Send<Protocol.map_ready>();
					LoadingWindow.isSendMapReady = true;
				}
			}
			request.mapId = SingletonDontDestoryUnity<GameManager>.Instance.RunningMapIdStr;
			NetLogic.GetInstance().Send<Protocol.refresh_online_state>(request);
		}
		else if (response.type > 0)
		{
			connectGameServer = 0;
			instance.PlayerCommonData.ServerLevel = (int)response.serverLevel;
			GameManager.OnLineState = true;
			if (Application.loadedLevelName.Equals(GameDefine.LOGIN_SCENE_NAME))
			{
				if (SingletonUnity<CreateRoleRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CreateRoleRootLogic>.Instance.gameObject))
				{
					WaitResponseUIRootLogic.CloseBox();
				}
				else
				{
					if (response.HasVersionCode && IsNeedQuitUpdate(response.versionCode, delegate
					{
						if (PlayerData.downLoadFlag == 1 && int.Parse(response.dataVersionCode) != PlayerData.LocalDataVersion)
						{
							WaitResponseUIRootLogic.CloseBox();
							SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LoginRootUI);
							if (!int.TryParse(response.dataVersionCode, out PlayerData.ServerDataVersion))
							{
								PlayerData.ServerDataVersion = -1;
							}
							SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.AccountVersionCheckRootUI, delegate
							{
								SingletonUnity<AccountVersionCheckRootLogic>.Instance.ResetDownLoadPage(isVerifyDownload: false);
							});
						}
						else
						{
							if (!int.TryParse(response.dataVersionCode, out PlayerData.ServerDataVersion))
							{
								PlayerData.ServerDataVersion = -1;
							}
							CharacterListRequest();
						}
					}))
					{
						return;
					}
					if (PlayerData.downLoadFlag == 1 && int.Parse(response.dataVersionCode) != PlayerData.LocalDataVersion)
					{
						WaitResponseUIRootLogic.CloseBox();
						SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LoginRootUI);
						if (!int.TryParse(response.dataVersionCode, out PlayerData.ServerDataVersion))
						{
							PlayerData.ServerDataVersion = -1;
						}
						SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.AccountVersionCheckRootUI, delegate
						{
							SingletonUnity<AccountVersionCheckRootLogic>.Instance.ResetDownLoadPage(isVerifyDownload: false);
						});
					}
					else
					{
						if (!int.TryParse(response.dataVersionCode, out PlayerData.ServerDataVersion))
						{
							PlayerData.ServerDataVersion = -1;
						}
						CharacterListRequest();
					}
				}
			}
			else if (isFinishDownload && !IsVersionSame(response))
			{
				NoticeLogic.AddNotifyData("#{200064}");
				LeaveGame();
				LoadingWindow.LoadScene(0);
				connectGameServer = -1;
			}
			else
			{
				SingletonDontDestoryUnity<GameManager>.Instance.QueryInventory();
				bool isTutorialFinish = instance.PlayerData.IsTutorialFinish;
				instance.PlayerData.ResetPlayerData();
				instance.PlayerCommonData.ClearData();
				character_pick.request request2 = new character_pick.request();
				request2.id = PlayerData.MainPlayerServerId;
				NetLogic.GetInstance().Send<Protocol.character_pick>(request2, PickResponse);
				instance.FirstEnterGame = true;
				instance.IsShowMainMissionTip = true;
				NetLogic.GetInstance().StartReconnecting();
				instance.PlayerData.IsTutorialFinish = isTutorialFinish;
			}
		}
		else
		{
			WaitResponseUIRootLogic.CloseBox();
			NoticeLogic.AddNotifyData("#{200064}");
			LeaveGame();
			LoadingWindow.LoadScene(0);
			connectGameServer = -1;
		}
	}

	private bool IsVersionSame(login.response response)
	{
		string[] array = response.versionCode.Split('.');
		string[] array2 = GameSettingData.GameVersion.Split('.');
		int[] array3 = new int[array.Length];
		int[] array4 = new int[array2.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array3[i] = int.Parse(array[i]);
			array4[i] = int.Parse(array2[i]);
		}
		for (int j = 0; j < 2; j++)
		{
			if (array3[j] > array4[j])
			{
				return false;
			}
		}
		if (array3[2] > array4[2])
		{
			return false;
		}
		if (int.Parse(response.dataVersionCode) > PlayerData.LocalDataVersion)
		{
			return false;
		}
		return true;
	}

	public void CharacterListRequest()
	{
		NetLogic.GetInstance().Send<Protocol.character_list>(null, CharacterListResponse);
	}

	public void GameCheck()
	{
		NetLogic.GetInstance().Send<Protocol.game_check>();
	}

	private void CharacterListResponse(SprotoTypeBase req)
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.AccountVersionCheckRootUI);
		character_list.response response = req as character_list.response;
		WaitResponseUIRootLogic.CloseBox();
		if (response != null)
		{
			if (response.character.Count == 0)
			{
				SingletonUnity<MenuSceneController>.Instance.ShowCreateRole(isNewAccount: true);
			}
			else
			{
				SingletonUnity<MenuSceneController>.Instance.ShowChooseRole(response);
			}
		}
	}

	public void CheckConnectLost()
	{
		if (NetLogic.GetInstance().ConnectStatus == NetLogic.CONNECT_STATUS.DISCONNECTED && SingletonUnity<UIManager>.Exists)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TopMessageBoxUI, delegate
			{
				SingletonUnity<TopMessageBoxLogic>.Instance.OpenOKCancelBox("#{100142}", "#{100127}", OnYesClick, OnCancleClick, "#{100143}", "#{100144}");
			});
		}
	}

	private void CheckConnectTimeOut()
	{
		if (checkTime < 0f)
		{
			return;
		}
		if (!NetLogic.GetInstance().CanProcessPack)
		{
			checkTime = 0f;
			return;
		}
		checkTime += Time.deltaTime;
		if (checkTime <= 12f)
		{
			return;
		}
		if (NetLogic.GetInstance().ReConnectingFlag)
		{
			NetLogic.GetInstance().FinishReconnecting();
			if (!Application.loadedLevelName.Equals(GameDefine.LOGIN_SCENE_NAME))
			{
				NoticeLogic.AddNotifyData("#{200064}");
				LeaveGame();
				LoadingWindow.LoadScene(0);
				connectGameServer = -1;
				return;
			}
		}
		if ((!SingletonUnity<TopMessageBoxLogic>.Exists || !UnityVersionUtil.IsActive(SingletonUnity<TopMessageBoxLogic>.Instance.gameObject)) && SingletonUnity<UIManager>.Exists)
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TopMessageBoxUI, delegate
			{
				SingletonUnity<TopMessageBoxLogic>.Instance.OpenOKCancelBox("#{100142}", "#{100127}", OnYesClick, OnCancleTimeClick, "#{100143}", "#{100144}");
			});
		}
	}

	public void OnCancleTimeClick()
	{
		checkTime = -1f;
		SingletonDontDestoryUnity<NetManager>.Instance.LeaveGame();
		LoadingWindow.LoadScene(0);
	}

	private void OnYesTimeClick()
	{
		checkTime = -1f;
		WaitResponseUIRootLogic.OpenWaitBox(0, 60f, 0f);
		ReConnectToServer(ReConnectSuccess);
	}

	public void ConnectLost()
	{
		GameManager.OnLineState = false;
		connectGameServer = -1;
		if (NetLogic.GetInstance().ReConnectingFlag)
		{
			NetLogic.GetInstance().FinishReconnecting();
			if (!Application.loadedLevelName.Equals(GameDefine.LOGIN_SCENE_NAME))
			{
				NoticeLogic.AddNotifyData("#{200064}");
				LeaveGame();
				LoadingWindow.LoadScene(0);
				connectGameServer = -1;
				return;
			}
		}
		if (SingletonUnity<UIManager>.Exists)
		{
			TutorialManager.CloseTutorial();
			WaitResponseUIRootLogic.CloseBox();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TopMessageBoxUI, delegate
			{
				SingletonUnity<TopMessageBoxLogic>.Instance.OpenOKCancelBox("#{100142}", "#{100127}", OnYesClick, OnCancleClick, "#{100143}", "#{100144}");
			});
		}
	}

	private void OnYesClick()
	{
		if (Application.loadedLevelName.Equals(GameDefine.LOGIN_SCENE_NAME) && ((SingletonUnity<CreateRoleRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CreateRoleRootLogic>.Instance.gameObject)) || (SingletonUnity<ChooseRoleRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ChooseRoleRootLogic>.Instance.gameObject))))
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonUnity<MenuSceneController>.Instance.ShowChooseServer();
		}
		else
		{
			WaitResponseUIRootLogic.OpenWaitBox(0, -1f, 0f);
			ReConnectToServer(ReConnectSuccess);
		}
	}

	private void ReConnectSuccess(bool isSuccess)
	{
		if (isSuccess)
		{
			netTime = 0;
			sendTime2 = 0L;
			sendTime1 = DateTime.UtcNow.Ticks;
			checkTime = -1f;
			LoginRequest(0L);
		}
		else
		{
			WaitResponseUIRootLogic.CloseBox();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TopMessageBoxUI, delegate
			{
				SingletonUnity<TopMessageBoxLogic>.Instance.OpenOKCancelBox("#{100142}", "#{100127}", OnYesClick, OnCancleClick, "#{100143}", "#{100144}");
			});
		}
	}

	private void OnCancleClick()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.LeaveGame();
		LoadingWindow.LoadScene(0);
	}

	public void ReConnectToServer(NetLogic.ConnectDelegate delConnect = null, NetLogic.ConnectLostDelegate delConnectLost = null)
	{
		if (delConnect != null)
		{
			NetLogic.SetConnectDelegate(delConnect);
		}
		else
		{
			NetLogic.SetConnectDelegate(ConnectSuccess);
		}
		if (delConnectLost != null)
		{
			NetLogic.SetConnectLostDelegate(delConnectLost);
		}
		else
		{
			NetLogic.SetConnectLostDelegate(ConnectLost);
		}
		NetLogic.GetInstance().ReConnectToServer();
		connectGameServer = -1;
	}

	private void HeaerBeat()
	{
		heart_beat.request request = new heart_beat.request();
		sendTime2 = DateTime.UtcNow.Ticks;
		request.time = sendTime2;
		sendTime1 = sendTime2;
		checkTime = 0f;
		request.time2 = (int)(Time.realtimeSinceStartup * 100f);
		NetLogic.GetInstance().Send<Protocol.heart_beat>(request, HeaerBeatResponse);
	}

	private void HeaerBeatResponse(SprotoTypeBase req)
	{
		heart_beat.response response = req as heart_beat.response;
		sendTime2 = 0L;
		checkTime = -1f;
		if (response != null)
		{
			netTime = (int)((DateTime.UtcNow.Ticks - response.time) / 100000);
		}
		if (response.HasServerTime)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.ServerTime = response.serverTime;
		}
	}

	private void Update()
	{
		NetLogic.GetInstance().Update();
		if (NetLogic.GetInstance().ConnectStatus == NetLogic.CONNECT_STATUS.CONNRCTED && connectGameServer > -1)
		{
			curTime += Time.deltaTime;
			if (curTime > 15f)
			{
				curTime = 0f;
				HeaerBeat();
			}
			if (sendTime2 > 0)
			{
				netTime = (int)((DateTime.UtcNow.Ticks - sendTime1) / 100000);
			}
			CheckConnectTimeOut();
		}
	}

	private void OnApplicationFocus(bool isfocus)
	{
		checkTime = -1f;
	}

	public bool IsNeedQuitUpdate(string serverVersionCode, MessageBoxLogic.OnCancelClick onContinueGame)
	{
		if (!string.IsNullOrEmpty(serverVersionCode) && !lastCheckVersion.Equals(serverVersionCode))
		{
			lastCheckVersion = serverVersionCode;
			string[] array = serverVersionCode.Split('.');
			string[] array2 = GameSettingData.GameVersion.Split('.');
			int[] array3 = new int[array.Length];
			int[] array4 = new int[array2.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array3[i] = int.Parse(array[i]);
				array4[i] = int.Parse(array2[i]);
			}
			for (int j = 0; j < 2; j++)
			{
				if (array3[j] > array4[j])
				{
					WaitResponseUIRootLogic.CloseBox();
					MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{100179}"), StrDictionary.GetDictionaryString("#{100127}"), delegate
					{
						SingletonDontDestoryUnity<GameManager>.Instance.Rating();
						Application.Quit();
					});
					return true;
				}
			}
			if (array3[2] > array4[2])
			{
				WaitResponseUIRootLogic.CloseBox();
				MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{100178}"), StrDictionary.GetDictionaryString("#{100127}"), delegate
				{
					SingletonDontDestoryUnity<GameManager>.Instance.Rating();
					Application.Quit();
				}, onContinueGame);
				return true;
			}
		}
		return false;
	}

	public void ClearLastCheckNeedQuitUpdateVersion()
	{
		lastCheckVersion = string.Empty;
	}
}
