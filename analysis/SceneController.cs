using UnityEngine;

public class SceneController : MonoBehaviour
{
	private void InitObj()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Exists)
		{
			ResourcesManager.LoadAndInstantiate("Manager/GameManager");
		}
		ResourcesManager.LoadAndInstantiate("FingerGestures/FingerGestures");
		base.gameObject.AddComponent<InputController>();
		base.gameObject.AddComponent<AIManager>();
		base.gameObject.AddComponent<MyEvent>();
		SingletonDontDestoryUnity<GameManager>.Instance.LoadScenneManager();
		UpdateCamera();
	}

	private void PlayBgmMusic()
	{
		MapInfoData currentMapInofData = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData;
		SoundData soundDataById = DataManager.GetSoundDataById(currentMapInofData.GetAudioID());
		if (soundDataById != null)
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.PlayBGMusic(soundDataById.Id, soundDataById.FadeOutTime, soundDataById.FadeInTime);
		}
	}

	private void UpdateCamera()
	{
	}

	private void InitUI()
	{
		ResourcesManager.LoadAndInstantiate("UIRoot/UIRoot");
	}

	private void InitSceneEffect()
	{
	}

	private void Awake()
	{
		InitUI();
		InitObj();
		NetLogic.GetInstance().CanProcessPack = true;
		InitSceneEffect();
		SingletonDontDestoryUnity<NetManager>.Instance.CheckConnectLost();
		PlayBgmMusic();
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
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (!sceneManager.IsBigWorld() && !sceneManager.IsLowPhoneManager() && !sceneManager.IsTutorialScene())
		{
			return;
		}
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
		if (Input.GetKeyUp(KeyCode.Escape) && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager != null && !SingletonUnity<UIManager>.Instance.BackUIFun())
		{
			if (SingletonUnity<CopyFunctionRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CopyFunctionRootLogic>.Instance.gameObject))
			{
				SingletonUnity<CopyFunctionRootLogic>.Instance.OnClickLeaveCopyBtn();
			}
			else if (SingletonUnity<SexGameUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SexGameUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<SexGameUIRootLogic>.Instance.OnClickExitBtn();
			}
			else
			{
				ExitGame();
			}
		}
	}

	private void OnCreateTestNPC(ObjNPC npc)
	{
		npc.DefaultDialogID = string.Empty;
	}

	private void AddEquipMent()
	{
		ItemContainer equipPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.EquipPack;
		GameItem gameItem = new GameItem();
		gameItem.ItemId = "2";
		gameItem.StackNum = 1;
		equipPack.AddItem(gameItem);
		gameItem = new GameItem();
		gameItem.ItemId = "3";
		gameItem.StackNum = 1;
		equipPack.AddItem(gameItem);
		gameItem = new GameItem();
		gameItem.ItemId = "4";
		gameItem.StackNum = 1;
		equipPack.AddItem(gameItem);
		gameItem = new GameItem();
		gameItem.ItemId = "5";
		gameItem.StackNum = 1;
		equipPack.AddItem(gameItem);
		gameItem = new GameItem();
		gameItem.ItemId = "6";
		gameItem.StackNum = 1;
		equipPack.AddItem(gameItem);
		gameItem = new GameItem();
		gameItem.ItemId = "7";
		gameItem.StackNum = 1;
		equipPack.AddItem(gameItem);
		gameItem = new GameItem();
		gameItem.ItemId = "8";
		gameItem.StackNum = 1;
		equipPack.AddItem(gameItem);
		gameItem = new GameItem();
		gameItem.ItemId = "9";
		gameItem.StackNum = 1;
		equipPack.AddItem(gameItem);
		gameItem = new GameItem();
		gameItem.ItemId = "10";
		gameItem.StackNum = 1;
		equipPack.AddItem(gameItem);
		gameItem = new GameItem();
		gameItem.ItemId = "11";
		gameItem.StackNum = 1;
		equipPack.AddItem(gameItem);
		gameItem = new GameItem();
		gameItem.ItemId = "12";
		gameItem.StackNum = 1;
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack.AddItem(gameItem);
	}

	public void TestInitMissionData()
	{
	}

	public void TestOtherPlayer()
	{
		ObjInitPlayerData objInitPlayerData = new ObjInitPlayerData();
		objInitPlayerData.mServerID = UUID.GenUUID();
		objInitPlayerData.mPos = new Vector3(21f, 0f, 20f);
		Singleton<ObjManager>.Instance.CreateOtherPlayer(objInitPlayerData);
	}

	public void TestRecycleOtherPlayer()
	{
		Singleton<ObjManager>.Instance.RecycleOtherPlayer(Singleton<ObjManager>.Instance.FindObjInScene(9L) as ObjOtherPlayer);
	}

	public void TestReCreateOtherPlayer()
	{
		ObjInitPlayerData objInitPlayerData = new ObjInitPlayerData();
		objInitPlayerData.mServerID = UUID.GenUUID();
		objInitPlayerData.mPos = new Vector3(21f, 0f, 25f);
		Singleton<ObjManager>.Instance.CreateOtherPlayer(objInitPlayerData);
	}

	public void CreateMissionNPC()
	{
		ObjInitNpcData objInitNpcData = new ObjInitNpcData();
		NpcData npcDataByID = DataManager.GetNpcDataByID("1001");
		objInitNpcData.npcInfoData = npcDataByID;
		objInitNpcData.mServerID = UUID.GenUUID();
		objInitNpcData.mPos = new VectorXZ(16f, 30f);
		Singleton<ObjManager>.Instance.CreateNPC(objInitNpcData);
		ObjInitNpcData objInitNpcData2 = new ObjInitNpcData();
		NpcData npcDataByID2 = DataManager.GetNpcDataByID("1002");
		objInitNpcData2.npcInfoData = npcDataByID2;
		objInitNpcData2.mServerID = UUID.GenUUID();
		objInitNpcData2.mPos = new VectorXZ(24f, 20f);
		Singleton<ObjManager>.Instance.CreateNPC(objInitNpcData2);
		ObjInitNpcData objInitNpcData3 = new ObjInitNpcData();
		NpcData npcDataByID3 = DataManager.GetNpcDataByID("1003");
		objInitNpcData3.npcInfoData = npcDataByID3;
		objInitNpcData3.mServerID = UUID.GenUUID();
		objInitNpcData3.mPos = new VectorXZ(16f, 20f);
		Singleton<ObjManager>.Instance.CreateNPC(objInitNpcData3);
	}
}
