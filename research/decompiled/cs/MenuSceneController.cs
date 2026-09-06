using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SprotoType;
using UnityEngine;

public class MenuSceneController : SingletonUnity<MenuSceneController>
{
	public delegate void OnLoadPartFinisheDel(GameObject obj);

	public List<game_server> GameServerList = new List<game_server>();

	public List<game_server> UserServerList = new List<game_server>();

	public string user_Str = string.Empty;

	private FakeObjLogic[] mNpcObjList = new FakeObjLogic[2];

	private string[] mNpcObjModelNameList = new string[2] { "NPC_Nv_004", "NPC_Nv_005" };

	protected override void Awake()
	{
		base.Awake();
	}

	private void Start()
	{
		CheckLoadUIState();
	}

	private IEnumerator NextFrame()
	{
		while (!DataManager.initDoneFlag)
		{
			yield return null;
		}
		SingletonDontDestoryUnity<GameManager>.Instance.HideFakeLoading();
		LoadingWindow.LoadScene(3001);
	}

	private void CheckLoadUIState()
	{
		if (GameSettingData.IsLowPhone)
		{
			DataManager.InitData(SingletonDontDestoryUnity<GameManager>.Instance);
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.LoadingUIRoot);
			if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(NextFrame());
			}
		}
		else
		{
			SingletonDontDestoryUnity<GameManager>.Instance.EnterLoginScene();
			PlayerData.CurLoginServerData = new ServerInfoData();
			if (GameSettingData.IsLocalTestServer)
			{
				SetTestServer();
			}
			else
			{
				PlayerData.CurLoginServerData.LoginIP = "gangsterlogin.galaxyaura.com";
				PlayerData.CurLoginServerData.LoginPort = 9777;
				PlayerData.CurLoginServerData.IsUseDns = true;
			}
			PlayerData.LocalDataVersion = GetDataVersion();
			InitUI();
			SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurLoginServerData.LoginIP, PlayerData.CurLoginServerData.LoginPort, ConnectSuccess, ConnectLost);
		}
		showAD();
		phoneStateFlurry(GameSettingData.IsLowPhone);
	}

	private void showAD()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.LocalFirstEnter)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.LocalFirstEnter = false;
			switch (LocalDataSaveManager.GetADFreeFlag())
			{
			case 0:
				SingletonDontDestoryUnity<GameManager>.Instance.ShowFullScreenSmall();
				break;
			case -1:
				LocalDataSaveManager.SetADFreeFlag(0);
				break;
			}
		}
	}

	private void phoneStateFlurry(bool islowphone)
	{
		if (LocalDataSaveManager.GetPhoneStateFlag() == 0)
		{
			if (islowphone)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("PhoneState", "state", "local");
			}
			else
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("PhoneState", "state", "connect");
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("PhoneState", "timezone", $"time{TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours}");
			}
			LocalDataSaveManager.SetPhoneStateFlag();
		}
	}

	private void ConnectLost()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TopMessageBoxUI, delegate
		{
			SingletonUnity<TopMessageBoxLogic>.Instance.OpenOKCancelBox("#{100142}", "#{100127}", OnYesClick, OnCancleClick, "#{100143}", "#{100144}");
		});
	}

	private void ConnectSuccess(bool isSuccess)
	{
		WaitResponseUIRootLogic.CloseBox();
		if (isSuccess)
		{
			if (SingletonUnity<AccountVersionCheckRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AccountVersionCheckRootLogic>.Instance.gameObject))
			{
				SingletonUnity<AccountVersionCheckRootLogic>.Instance.StartCheckVerifyAccount();
			}
			else
			{
				Debug.LogError("SingletonUnity<AccountVersionCheckRootLogic>.Exists == false!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
			}
		}
		else
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TopMessageBoxUI, delegate
			{
				SingletonUnity<TopMessageBoxLogic>.Instance.OpenOKCancelBox("#{100142}", "#{100127}", OnYesClick, OnCancleClick, "#{100143}", "#{100144}");
			});
			NoticeLogic.AddNotifyData("#{200065}");
		}
	}

	private void OnCancleClick()
	{
		Application.Quit();
	}

	private void OnYesClick()
	{
		WaitResponseUIRootLogic.OpenWaitBox(4, -1f, 0f);
		SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(PlayerData.CurLoginServerData.LoginIP, PlayerData.CurLoginServerData.LoginPort, ConnectSuccess, ConnectLost);
	}

	private void InitUI()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NotifyRootUI);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.AccountVersionCheckRootUI);
		if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(WaitFrame());
		}
	}

	private IEnumerator WaitFrame()
	{
		yield return null;
		SingletonDontDestoryUnity<GameManager>.Instance.HideFakeLoading();
	}

	public void ShowChooseServer()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.LeaveGame();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.LoginRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CreateRoleRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ChooseRoleRootUI);
	}

	public void ShowCreateRole(bool isNewAccount)
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LoginRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ChooseRoleRootUI);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.CreateRoleRootUI, delegate
		{
			SingletonUnity<CreateRoleRootLogic>.Instance.Reset(isNewAccount);
		});
	}

	public void ShowChooseRole(character_list.response chaList)
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LoginRootUI);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ChooseRoleRootUI, delegate
		{
			SingletonUnity<ChooseRoleRootLogic>.Instance.Reset(chaList);
		});
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CreateRoleRootUI);
	}

	private void OnClickExitGame()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.LeaveGame();
		MessageBoxLogic.CloseBox();
		Application.Quit();
	}

	private void OnClickNo()
	{
		MessageBoxLogic.CloseBox();
	}

	private void ExitGame()
	{
		if (SingletonUnity<ExitGameRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ExitGameRoot>.Instance.gameObject))
		{
			SingletonUnity<ExitGameRoot>.Instance.OnClickNoBtn();
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExitGameRoot, delegate
		{
			SingletonUnity<ExitGameRoot>.Instance.Reset();
		});
	}

	private void Update()
	{
		BundleManager.LoadModelListUpdate(this);
		if (Input.GetKeyUp(KeyCode.Escape) && !SingletonUnity<UIManager>.Instance.BackUIFun())
		{
			ExitGame();
		}
	}

	public GameObject CreatePlayerModelRoot(PROFESSION_TYPE profession)
	{
		return ResourcesManager.LoadAndInstantiate("TestModel/Model/" + profession.ToString() + "/ModelRoot") as GameObject;
	}

	public void CreatePlayerModelVisual(GameObject modelRoot, string weaponId, string headId, string bodyId, string legId, OnLoadPartFinisheDel func)
	{
		LoadPlayerVisual(modelRoot, weaponId, headId, bodyId, legId, func);
	}

	private void LoadPlayerVisual(GameObject playerRootObj, string partWeaponId, string partHeadId, string partBodyId, string partLegId, OnLoadPartFinisheDel func)
	{
		ModelData modelData = null;
		modelData = DataManager.GetModeDataByID(partLegId);
		if (modelData != null)
		{
			BundleManager.LoadModelInList(modelData.Name, isNeedUnload: true, isDoNotCache: false, OnLoadPlayerPartFinished, func, playerRootObj, modelData.ModelPath);
		}
		modelData = null;
		modelData = DataManager.GetModeDataByID(partWeaponId);
		if (modelData != null)
		{
			BundleManager.LoadModelInList(modelData.Name, isNeedUnload: true, isDoNotCache: false, OnLoadPlayerPartFinished, func, playerRootObj, modelData.ModelPath);
		}
		modelData = null;
		modelData = DataManager.GetModeDataByID(partHeadId);
		if (modelData != null)
		{
			BundleManager.LoadModelInList(modelData.Name, isNeedUnload: true, isDoNotCache: false, OnLoadPlayerPartFinished, func, playerRootObj, modelData.ModelPath);
		}
		modelData = null;
		modelData = DataManager.GetModeDataByID(partBodyId);
		if (modelData != null)
		{
			BundleManager.LoadModelInList(modelData.Name, isNeedUnload: true, isDoNotCache: false, OnLoadPlayerPartFinished, func, playerRootObj, modelData.ModelPath);
		}
		modelData = null;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	private void OnLoadPlayerPartFinished(object objBundle, object param1 = null, object param2 = null)
	{
		OnLoadPartFinisheDel onLoadPartFinisheDel = param1 as OnLoadPartFinisheDel;
		GameObject gameObject = param2 as GameObject;
		GameObject gameObject2 = objBundle as GameObject;
		BundleManager.ResetShader(gameObject2.transform);
		gameObject2.layer = LayerMask.NameToLayer("ShadowCaster");
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		UnityVersionUtil.SetActiveRecursive(gameObject2.gameObject, state: false);
		BundleManager.RebuildBones(gameObject, gameObject2);
		onLoadPartFinisheDel?.Invoke(gameObject);
	}

	public static int GetDataVersion()
	{
		string path = $"{FileUpdateHelper.GetLocalVersionPath()}/{FileUpdateHelper.VersionFileName}";
		string path2 = $"{Application.streamingAssetsPath}{FileUpdateHelper.ApkVersionFolderName}/{FileUpdateHelper.VersionFileName}";
		string result = string.Empty;
		if (File.Exists(path) && !MyFileUtil.GetStringFromFile(path, ref result))
		{
			Log.ERROR_MSG("parse version fail");
		}
		string result2 = string.Empty;
		if (File.Exists(path2) && !MyFileUtil.GetStringFromFile(path2, ref result2))
		{
			Log.ERROR_MSG("parse version fail");
		}
		if (string.Compare(result, result2) > 0)
		{
			return int.Parse(result);
		}
		if (string.IsNullOrEmpty(result2))
		{
			return 0;
		}
		return int.Parse(result2);
	}

	public void SaveServerList(verfiy.response response)
	{
		GameServerList.Clear();
		UserServerList.Clear();
		user_Str = string.Empty;
		if (response.HasGame_server)
		{
			GameServerList = response.game_server;
		}
		if (GameSettingData.IsLocalTestServer && (!GameSettingData.IsLocalTestServer || GameSettingData.LocalTestServerID != 0))
		{
			return;
		}
		if (response.HasUser_server)
		{
			SaveUseServer(response.user_server);
			if (UserServerList.Count > 0)
			{
				PlayerData.CurGameServerData = new game_server();
				PlayerData.CurGameServerData.serverId = UserServerList[0].serverId;
				PlayerData.CurGameServerData.serverIP = UserServerList[0].serverIP;
				PlayerData.CurGameServerData.serverPort = UserServerList[0].serverPort;
				PlayerData.CurGameServerData.serverName = UserServerList[0].serverName;
				PlayerData.CurGameServerData.serverState = UserServerList[0].serverState;
				if (UserServerList[0].HasNewServer)
				{
					PlayerData.CurGameServerData.newServer = UserServerList[0].newServer;
				}
			}
			else
			{
				PlayerData.CurGameServerData = null;
			}
		}
		else
		{
			DelTestServer(ishave: false);
			PlayerData.CurGameServerData = null;
		}
	}

	public void UpdateServerList(List<game_server> serverlist)
	{
		GameServerList.Clear();
		GameServerList = serverlist;
		if (GameSettingData.IsLocalTestServer && (!GameSettingData.IsLocalTestServer || GameSettingData.LocalTestServerID != 0))
		{
			return;
		}
		updateUserServer();
		if (PlayerData.CurGameServerData == null)
		{
			return;
		}
		for (int i = 0; i < GameServerList.Count; i++)
		{
			if (PlayerData.CurGameServerData.serverId == GameServerList[i].serverId)
			{
				PlayerData.CurGameServerData.serverId = GameServerList[i].serverId;
				PlayerData.CurGameServerData.serverIP = GameServerList[i].serverIP;
				PlayerData.CurGameServerData.serverPort = GameServerList[i].serverPort;
				PlayerData.CurGameServerData.serverName = GameServerList[i].serverName;
				PlayerData.CurGameServerData.serverState = GameServerList[i].serverState;
				if (GameServerList[i].HasNewServer)
				{
					PlayerData.CurGameServerData.newServer = GameServerList[i].newServer;
				}
				break;
			}
		}
	}

	public void ChooseRecommendServer()
	{
		string serverAreaName = SingletonDontDestoryUnity<GameManager>.Instance.ServerAreaName;
		int num = 0;
		if (!string.IsNullOrEmpty(serverAreaName))
		{
			switch (serverAreaName)
			{
			case "Asia":
			case "Australia":
				num = 2;
				break;
			case "Europe":
			case "Africa":
				num = 1;
				break;
			case "North America":
			case "South America":
			case "Antarctica":
				num = 0;
				break;
			}
		}
		else
		{
			Debug.Log("AreaName: is null or empty");
		}
		List<game_server> nearTimezoneServer = getNearTimezoneServer();
		if (nearTimezoneServer.Count > 0)
		{
			nearTimezoneServer.Sort((game_server x, game_server y) => (x.serverRank == y.serverRank) ? ((int)x.serverId - (int)y.serverId) : ((int)x.serverRank - (int)y.serverRank));
			List<game_server> list = new List<game_server>();
			for (int num2 = nearTimezoneServer.Count - 1; num2 >= 0; num2--)
			{
				if ((int)nearTimezoneServer[num2].serverArea == num)
				{
					list.Add(nearTimezoneServer[num2]);
					break;
				}
			}
			game_server game_server2 = null;
			game_server2 = ((list == null || list.Count <= 0) ? GetRecommendServer(nearTimezoneServer) : GetRecommendServer(list));
			if (game_server2 != null)
			{
				PlayerData.CurGameServerData = new game_server();
				PlayerData.CurGameServerData.serverId = game_server2.serverId;
				PlayerData.CurGameServerData.serverIP = game_server2.serverIP;
				PlayerData.CurGameServerData.serverPort = game_server2.serverPort;
				PlayerData.CurGameServerData.serverName = game_server2.serverName;
				PlayerData.CurGameServerData.serverState = game_server2.serverState;
				if (game_server2.HasNewServer)
				{
					PlayerData.CurGameServerData.newServer = game_server2.newServer;
				}
			}
			else
			{
				PlayerData.CurGameServerData = null;
			}
		}
		else
		{
			PlayerData.CurGameServerData = null;
		}
	}

	public game_server GetRecommendServer(List<game_server> uselist)
	{
		game_server game_server2 = null;
		List<game_server> list = new List<game_server>();
		int num = -1;
		for (int i = 0; i < uselist.Count; i++)
		{
			if (num < uselist[i].serverRank)
			{
				num = (int)uselist[i].serverRank;
			}
		}
		int num2 = 0;
		for (int j = 0; j < uselist.Count; j++)
		{
			if (num == (int)uselist[j].serverRank)
			{
				list.Add(uselist[j]);
				num2 += (int)uselist[j].serverWeight;
			}
		}
		if (list == null || list.Count == 0)
		{
			return null;
		}
		if (num2 == 0)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			game_server2 = list[index];
		}
		else
		{
			int num3 = UnityEngine.Random.Range(0, num2);
			game_server2 = list[list.Count - 1];
			for (int k = 0; k < list.Count; k++)
			{
				if (num3 < (int)list[k].serverWeight)
				{
					game_server2 = list[k];
					break;
				}
				num3 -= (int)list[k].serverWeight;
			}
		}
		return game_server2;
	}

	public List<game_server> getNearTimezoneServer()
	{
		List<game_server> list = new List<game_server>();
		for (int i = 0; i < GameServerList.Count; i++)
		{
			if (GameServerList[i].serverState != 3)
			{
				list.Add(GameServerList[i]);
			}
		}
		int LocalTimezone = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
		list.Sort(delegate(game_server x, game_server y)
		{
			int num2 = Mathf.Abs((int)x.serverTimeZone - LocalTimezone);
			int num3 = Mathf.Abs((int)y.serverTimeZone - LocalTimezone);
			return num2 - num3;
		});
		for (int num = list.Count - 1; num > 0; num--)
		{
			if (Mathf.Abs(list[num].serverTimeZone - LocalTimezone) != Mathf.Abs(list[0].serverTimeZone - LocalTimezone))
			{
				list.RemoveAt(num);
			}
		}
		return list;
	}

	public void SaveUseServer(string use_server)
	{
		bool ishave = false;
		if (!string.IsNullOrEmpty(use_server))
		{
			string[] array = use_server.Split('#');
			for (int i = 0; i < array.Length; i++)
			{
				if (int.Parse(array[i]) == 2)
				{
					ishave = true;
					break;
				}
			}
		}
		DelTestServer(ishave);
		if (string.IsNullOrEmpty(use_server))
		{
			return;
		}
		user_Str = use_server;
		string[] array2 = use_server.Split('#');
		for (int j = 0; j < array2.Length; j++)
		{
			for (int k = 0; k < GameServerList.Count; k++)
			{
				if (GameServerList[k].serverId == int.Parse(array2[j]))
				{
					UserServerList.Add(GameServerList[k]);
					break;
				}
			}
		}
	}

	public void updateUserServer()
	{
		bool ishave = false;
		if (!string.IsNullOrEmpty(user_Str))
		{
			string[] array = user_Str.Split('#');
			for (int i = 0; i < array.Length; i++)
			{
				if (int.Parse(array[i]) == 2)
				{
					ishave = true;
					break;
				}
			}
		}
		DelTestServer(ishave);
		for (int j = 0; j < UserServerList.Count; j++)
		{
			for (int k = 0; k < GameServerList.Count; k++)
			{
				if (GameServerList[k].serverId == UserServerList[j].serverId)
				{
					UserServerList[j] = GameServerList[k];
					break;
				}
			}
		}
	}

	private void DelTestServer(bool ishave)
	{
		if (ishave || GameServerList.Count <= 1)
		{
			return;
		}
		for (int num = GameServerList.Count - 1; num >= 0; num--)
		{
			if (GameServerList[num].serverId == 2)
			{
				GameServerList.RemoveAt(num);
			}
		}
	}

	public List<game_server> GetAreaServerList(int areaid)
	{
		List<game_server> list = new List<game_server>();
		if (areaid == -1)
		{
			list = new List<game_server>(UserServerList);
		}
		else
		{
			for (int i = 0; i < GameServerList.Count; i++)
			{
				if ((int)GameServerList[i].serverArea == areaid)
				{
					list.Add(GameServerList[i]);
				}
			}
			list.Sort((game_server x, game_server y) => (int)x.serverId - (int)y.serverId);
		}
		return list;
	}

	public void DataLoadFinish()
	{
		MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(GameDefine.SCENE_DEFINE.SCENE_MAIN_CITY);
		SoundData soundDataById = DataManager.GetSoundDataById(mapInfoDataByID.GetAudioID());
		if (soundDataById != null)
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.PlayBGMusic(soundDataById.Id, soundDataById.FadeOutTime, soundDataById.FadeInTime);
		}
		Transform transform = null;
		for (int i = 0; i < mNpcObjModelNameList.Length; i++)
		{
			transform = GameObject.Find($"DSJ_shangYeZhongXin_1/animtion/{mNpcObjModelNameList[i]}").transform;
			if (mNpcObjList[i] == null)
			{
				mNpcObjList[i] = new FakeObjLogic();
			}
			if (mNpcObjList[i].FakeObj == null)
			{
				mNpcObjList[i].InitAnimaFakeNpcObj(mNpcObjModelNameList[i], transform, "tiaowu");
			}
		}
	}

	public void AddServerList()
	{
	}

	public void SetTestServer()
	{
		switch (GameSettingData.LocalTestServerID)
		{
		case 0:
			PlayerData.CurLoginServerData.LoginIP = "ec2-52-91-30-19.compute-1.amazonaws.com";
			PlayerData.CurLoginServerData.LoginPort = 9777;
			PlayerData.CurLoginServerData.IsUseDns = true;
			break;
		case 1:
			PlayerData.CurLoginServerData.LoginIP = "192.168.1.220";
			PlayerData.CurLoginServerData.LoginPort = 9777;
			PlayerData.CurLoginServerData.IsUseDns = false;
			PlayerData.CurGameServerData = new game_server();
			PlayerData.CurGameServerData.serverIP = "192.168.1.220";
			PlayerData.CurGameServerData.serverPort = 9555L;
			PlayerData.CurGameServerData.serverName = "Liu";
			PlayerData.CurGameServerData.serverState = 0L;
			PlayerData.CurGameServerData.serverId = 1L;
			PlayerData.CurGameServerData.newServer = 1L;
			break;
		case 2:
			PlayerData.CurLoginServerData.LoginIP = "192.168.1.70";
			PlayerData.CurLoginServerData.LoginPort = 9777;
			PlayerData.CurLoginServerData.IsUseDns = false;
			PlayerData.CurGameServerData = new game_server();
			PlayerData.CurGameServerData.serverIP = "192.168.1.70";
			PlayerData.CurGameServerData.serverPort = 9555L;
			PlayerData.CurGameServerData.serverName = "Li";
			PlayerData.CurGameServerData.serverState = 0L;
			PlayerData.CurGameServerData.serverId = 1L;
			PlayerData.CurGameServerData.newServer = 0L;
			break;
		case 3:
			PlayerData.CurLoginServerData.LoginIP = "192.168.1.71";
			PlayerData.CurLoginServerData.LoginPort = 9777;
			PlayerData.CurLoginServerData.IsUseDns = false;
			PlayerData.CurGameServerData = new game_server();
			PlayerData.CurGameServerData.serverIP = "192.168.1.71";
			PlayerData.CurGameServerData.serverPort = 9555L;
			PlayerData.CurGameServerData.serverName = "Local Server";
			PlayerData.CurGameServerData.serverState = 0L;
			PlayerData.CurGameServerData.serverId = 1L;
			PlayerData.CurGameServerData.newServer = 0L;
			break;
		}
	}
}
