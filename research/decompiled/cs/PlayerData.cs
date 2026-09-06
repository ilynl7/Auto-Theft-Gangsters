using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class PlayerData
{
	private const string PLAYER_ACCOUNT_ID = "player_account_id";

	private const string PLAYER_ACCOUNT_KEY = "player_account_key";

	public const string LoginIP = "gangsterlogin.galaxyaura.com";

	public const string LoginIpClassic = "gangsterlogin2.galaxyaura.com";

	public const int loginPort = 9777;

	public const int gamePort = 9555;

	public const int MAX_STR_COUNT = 1000;

	private const float DragCDTime = 20f;

	public static ServerInfoData CurLoginServerData;

	public static game_server CurGameServerData;

	public static int LocalDataVersion;

	public static int ServerDataVersion;

	public bool IsFinishDownload = true;

	public static string FaceBookId;

	public static string FacekBookToken;

	public Vector3 MainPlayerStartPos = Vector3.zero;

	public Vector3 MainPlayerStartDir = Vector3.zero;

	private static long mMainPlayerServerId = -1L;

	private bool mIsTutorialFinish;

	private bool mAutoComabat;

	private float mBreakAutoCombatTime;

	private bool mIsOpenAutoCombat;

	private bool mAutoUseDrag = true;

	private float mAutoUseDragThreshold = 0.5f;

	private bool mAutoUseSort = true;

	public static long session = 0L;

	public static long session2 = 0L;

	public static int loginType = 1;

	public static int downLoadFlag = 0;

	public static long FaceBookBind = -1L;

	public static string FaceBookBind2 = string.Empty;

	private float mMusicDragValue = 1f;

	private float mSoundDragValue = 1f;

	private int mGraphicsQua = 1;

	private int mStreetRacingMode = 1;

	private int mScreen_Vibrating;

	private int mNotify = 1;

	private int mChannelValue;

	private int mChatShow = 1;

	private int mNonMissionTarget = 1;

	private int mVehicleTarget = 1;

	private float mfCopySceneChange;

	public int SkillIndex;

	private List<CharacterSkillData> mMainPlayerSkillDataList = new List<CharacterSkillData>();

	private List<string> mMainPlayerSkillIDList = new List<string>();

	private FriendInfo mFriendInfo;

	private long mVideoTimes;

	private long mVideoMaxTimes;

	private long mVideoDiamond;

	private float mDragCdTime = -1f;

	private PROFESSION_TYPE mProfession;

	private CharacterAttributeData mMainPlayerAttrData = new CharacterAttributeData();

	private CharacterAttributeData mOtherPlayerAttriData = new CharacterAttributeData();

	private PlayerRankPVPData mRankPVPData = new PlayerRankPVPData();

	private TowerInfoData mTowerData = new TowerInfoData();

	private TargetBasicInfo mSelectTargetBasicInfo = new TargetBasicInfo();

	private CameraController.CAMERAVIEWSTATE mViewType = CameraController.CAMERAVIEWSTATE.FREE;

	private string mCharacterModelId;

	private CharacterModelData mCharacterModelData;

	private string mPartWeaponId = string.Empty;

	private string mPartHeadId = string.Empty;

	private string mPartBodyId = string.Empty;

	private string mPartLegId = string.Empty;

	private string mFashionHeadId = string.Empty;

	private string mFashionBodyId = string.Empty;

	private string mFashionLegId = string.Empty;

	private string mFashionWeaponId = string.Empty;

	private string mWeaponItemId = string.Empty;

	private string mFashionItemId = string.Empty;

	private bool mIsShowFashion;

	private bool mIsServerRidingMount;

	private string mMountId = string.Empty;

	private string mMountColor = string.Empty;

	private long mCash;

	private long mGold;

	private long mDiamond;

	private long mBattleCoin;

	private long mActivityCoin;

	private ItemContainer mItemBackPack;

	private ItemContainer mEquipPack;

	private ItemContainer mBadgeEquipPack;

	private ItemContainer mBadgeBackPack;

	private ItemContainer mEquipBackPack;

	private ItemContainer mFashionBackPack;

	private ItemContainer mFashionEquipPack;

	public Dictionary<string, domin_info> Domin_InfoDic = new Dictionary<string, domin_info>();

	public Dictionary<long, character_look> Domin_CharacterDic = new Dictionary<long, character_look>();

	private Team mTeam;

	private static float TEAM_INVITE_INTERVAL = 10f;

	private Dictionary<long, float> mPreTeamInviteDic = new Dictionary<long, float>();

	public Dictionary<string, shop_item> ShopDict;

	public GameDefine.SHOP_TYPE ShopType;

	private CopyData mCopyInfoData = new CopyData();

	private ActivityData mActivityData = new ActivityData();

	private PlayerSlotData mPlayerSlotData = new PlayerSlotData();

	private WelfareData mWelfareData = new WelfareData();

	private PlayerMountData mPlayerMountData = new PlayerMountData();

	private int mCurLineIndex;

	private int mLineCount;

	private List<long> lineStates;

	private long mCurSelectPotionIndex = -1L;

	private GameDefine.CAMP_TYPE mPlayerCamp;

	private PlayerChatHistory mChatHistory;

	private GameDefine.CHAT_CHANNEL_TYPE mChoosedChannelType = GameDefine.CHAT_CHANNEL_TYPE.WORLD;

	private bool mIsInitTranslation;

	private bool mIsNeedTranslation;

	private RecentSpeakerRecord mRecentSpeakers;

	private List<consign_item> mSaleNowList;

	private List<consign_item> mBuyList;

	private Guild mPlayerGuild;

	private long mGuildContribute;

	private bool mIsGuildBattleRedTeam;

	private List<string> mMainPlayerPartBundleIdList = new List<string>();

	private PlayerDanceData mPlayerDanceData;

	public static long MainPlayerServerId
	{
		get
		{
			return mMainPlayerServerId;
		}
		set
		{
			mMainPlayerServerId = value;
		}
	}

	public bool IsTutorialFinish
	{
		get
		{
			return mIsTutorialFinish;
		}
		set
		{
			mIsTutorialFinish = value;
		}
	}

	public int SystemMusic
	{
		get
		{
			return LocalDataSaveManager.GetSystemMusic();
		}
		set
		{
			LocalDataSaveManager.SetSystemMusic(value);
		}
	}

	public int SystemSoundEffect
	{
		get
		{
			return LocalDataSaveManager.GetSystemSoundEffect();
		}
		set
		{
			LocalDataSaveManager.SetSystemSoundEffect(value);
		}
	}

	public float MusicDragValue
	{
		get
		{
			return mMusicDragValue;
		}
		set
		{
			mMusicDragValue = value;
		}
	}

	public float SoundDragValue
	{
		get
		{
			return mSoundDragValue;
		}
		set
		{
			mSoundDragValue = value;
		}
	}

	public int GraphicsQua => mGraphicsQua;

	public float Screen_Vibrating => mScreen_Vibrating;

	public int StreetRacingMode => mStreetRacingMode;

	public float Notify => mNotify;

	public int GetChannelValue => mChannelValue;

	public int GetChatShow
	{
		get
		{
			mChatShow = LocalDataSaveManager.GetChatShow();
			return mChatShow;
		}
	}

	public int NonMissionTarget => mNonMissionTarget;

	public int VehicleTarget => mVehicleTarget;

	public bool CopySceneChange
	{
		get
		{
			if (Time.realtimeSinceStartup - mfCopySceneChange > 10f)
			{
				return false;
			}
			return true;
		}
		set
		{
			if (value)
			{
				mfCopySceneChange = Time.realtimeSinceStartup;
			}
			else
			{
				mfCopySceneChange = 0f;
			}
		}
	}

	public List<CharacterSkillData> MainPlayerSkillDataList
	{
		get
		{
			return mMainPlayerSkillDataList;
		}
		set
		{
			mMainPlayerSkillDataList = value;
		}
	}

	public List<string> MainPlayerSkillIDList
	{
		get
		{
			return mMainPlayerSkillIDList;
		}
		set
		{
			mMainPlayerSkillIDList = value;
		}
	}

	public FriendInfo FriendInfo => mFriendInfo;

	public long VideoTimes => mVideoTimes;

	public long VideoMaxTimes => mVideoMaxTimes;

	public long VideoDiamond => mVideoDiamond;

	public PROFESSION_TYPE Profession
	{
		get
		{
			return mProfession;
		}
		set
		{
			mProfession = value;
		}
	}

	public CharacterAttributeData MainPlayerAttrData
	{
		get
		{
			return mMainPlayerAttrData;
		}
		set
		{
			mMainPlayerAttrData = value;
		}
	}

	public CharacterAttributeData OtherPlayerAttriData
	{
		get
		{
			return mOtherPlayerAttriData;
		}
		set
		{
			mOtherPlayerAttriData = value;
		}
	}

	public int Level
	{
		get
		{
			return mMainPlayerAttrData.Level;
		}
		set
		{
			mMainPlayerAttrData.Level = value;
		}
	}

	public PlayerRankPVPData RankPVPData => mRankPVPData;

	public TowerInfoData TowerData => mTowerData;

	public TargetBasicInfo SelectTargetBasicInfo
	{
		get
		{
			return mSelectTargetBasicInfo;
		}
		set
		{
			mSelectTargetBasicInfo = value;
		}
	}

	public CameraController.CAMERAVIEWSTATE ViewType
	{
		get
		{
			return mViewType;
		}
		set
		{
			mViewType = value;
		}
	}

	public string CharacterModelId
	{
		get
		{
			return mCharacterModelId;
		}
		set
		{
			mCharacterModelId = value;
		}
	}

	public CharacterModelData CharacterModelData
	{
		get
		{
			return mCharacterModelData;
		}
		set
		{
			mCharacterModelData = value;
		}
	}

	public string PartWeaponId
	{
		get
		{
			return mPartWeaponId;
		}
		set
		{
			mPartWeaponId = value;
		}
	}

	public string PartHeadId
	{
		get
		{
			return mPartHeadId;
		}
		set
		{
			mPartHeadId = value;
		}
	}

	public string PartBodyId
	{
		get
		{
			return mPartBodyId;
		}
		set
		{
			mPartBodyId = value;
		}
	}

	public string PartLegId
	{
		get
		{
			return mPartLegId;
		}
		set
		{
			mPartLegId = value;
		}
	}

	public string FashionHeadId
	{
		get
		{
			return mFashionHeadId;
		}
		set
		{
			mFashionHeadId = value;
		}
	}

	public string FashionBodyId
	{
		get
		{
			return mFashionBodyId;
		}
		set
		{
			mFashionBodyId = value;
		}
	}

	public string FashionLegId
	{
		get
		{
			return mFashionLegId;
		}
		set
		{
			mFashionLegId = value;
		}
	}

	public string FashionWeaponId
	{
		get
		{
			return mFashionWeaponId;
		}
		set
		{
			mFashionWeaponId = value;
		}
	}

	public string WeaponItemId
	{
		get
		{
			return mWeaponItemId;
		}
		set
		{
			mWeaponItemId = value;
		}
	}

	public string FashionItemId
	{
		get
		{
			return mFashionItemId;
		}
		set
		{
			mFashionItemId = value;
		}
	}

	public bool IsShowFashion
	{
		get
		{
			return mIsShowFashion;
		}
		set
		{
			mIsShowFashion = value;
		}
	}

	public bool IsServerRidingMount
	{
		get
		{
			return mIsServerRidingMount;
		}
		set
		{
			mIsServerRidingMount = value;
		}
	}

	public string MountId
	{
		get
		{
			return mMountId;
		}
		set
		{
			mMountId = value;
			if (Singleton<ObjManager>.Instance.MainPlayer != null)
			{
				UpdateMainPlayerPartBundleIdList(Singleton<ObjManager>.Instance.MainPlayer.TargetPartObjId);
			}
		}
	}

	public string MountColor
	{
		get
		{
			return mMountColor;
		}
		set
		{
			mMountColor = value;
		}
	}

	public long Cash => mCash;

	public long Gold => mGold;

	public long Diamond => mDiamond;

	public long BattleCoin => mBattleCoin;

	public long ActivityCoin => mActivityCoin;

	public ItemContainer ItemBackPack
	{
		get
		{
			return mItemBackPack;
		}
		set
		{
			mItemBackPack = value;
		}
	}

	public ItemContainer EquipPack
	{
		get
		{
			return mEquipPack;
		}
		set
		{
			mEquipPack = value;
		}
	}

	public ItemContainer BadgeEquipPack
	{
		get
		{
			return mBadgeEquipPack;
		}
		set
		{
			mBadgeEquipPack = value;
		}
	}

	public ItemContainer BadgeBackPack
	{
		get
		{
			return mBadgeBackPack;
		}
		set
		{
			mBadgeBackPack = value;
		}
	}

	public ItemContainer EquipBackPack
	{
		get
		{
			return mEquipBackPack;
		}
		set
		{
			mEquipBackPack = value;
		}
	}

	public ItemContainer FashionBackPack
	{
		get
		{
			return mFashionBackPack;
		}
		set
		{
			mFashionBackPack = value;
		}
	}

	public ItemContainer FashionEquipPack
	{
		get
		{
			return mFashionEquipPack;
		}
		set
		{
			mFashionEquipPack = value;
		}
	}

	public Team TeamInfo
	{
		get
		{
			return mTeam;
		}
		set
		{
			mTeam = value;
		}
	}

	public Dictionary<long, float> PreTeamInviteDic => mPreTeamInviteDic;

	public CopyData CopyInfoData => mCopyInfoData;

	public ActivityData ActivityData
	{
		get
		{
			return mActivityData;
		}
		set
		{
			mActivityData = value;
		}
	}

	public PlayerSlotData playerSlotData
	{
		get
		{
			return mPlayerSlotData;
		}
		set
		{
			mPlayerSlotData = value;
		}
	}

	public WelfareData welfareData
	{
		get
		{
			return mWelfareData;
		}
		set
		{
			mWelfareData = value;
		}
	}

	public PlayerMountData playerMountData
	{
		get
		{
			return mPlayerMountData;
		}
		set
		{
			mPlayerMountData = value;
		}
	}

	public int CurLineIndex
	{
		get
		{
			return mCurLineIndex;
		}
		set
		{
			mCurLineIndex = value;
		}
	}

	public int LineCount
	{
		get
		{
			return mLineCount;
		}
		set
		{
			lineStates = null;
			mLineCount = value;
		}
	}

	public List<long> LineStates
	{
		get
		{
			return lineStates;
		}
		set
		{
			lineStates = value;
		}
	}

	public long CurSelectPotionIndex
	{
		get
		{
			return mCurSelectPotionIndex;
		}
		set
		{
			mCurSelectPotionIndex = value;
		}
	}

	public GameDefine.CAMP_TYPE PlayerCamp
	{
		get
		{
			return mMainPlayerAttrData.Camp;
		}
		set
		{
			mMainPlayerAttrData.Camp = value;
		}
	}

	public int PlayerPkMode
	{
		get
		{
			return mMainPlayerAttrData.PkMode;
		}
		set
		{
			mMainPlayerAttrData.PkMode = value;
		}
	}

	public PlayerChatHistory ChatHistory
	{
		get
		{
			return mChatHistory;
		}
		set
		{
			mChatHistory = value;
		}
	}

	public GameDefine.CHAT_CHANNEL_TYPE ChoosedChannelType
	{
		get
		{
			return mChoosedChannelType;
		}
		set
		{
			mChoosedChannelType = value;
		}
	}

	public List<PlayerChatHistoryInfo> CurChannelChatHistoryList => mChatHistory.ChannelChatHistoryList[(int)mChoosedChannelType];

	public bool IsNeedTranslation
	{
		get
		{
			if (mIsInitTranslation)
			{
				return mIsNeedTranslation;
			}
			mIsNeedTranslation = LocalDataSaveManager.GetTranslationFlag();
			mIsInitTranslation = true;
			return mIsNeedTranslation;
		}
		set
		{
			mIsNeedTranslation = value;
		}
	}

	public RecentSpeakerRecord RecentSpeakers
	{
		get
		{
			return mRecentSpeakers;
		}
		set
		{
			mRecentSpeakers = value;
		}
	}

	public List<consign_item> SaleNowList
	{
		get
		{
			return mSaleNowList;
		}
		set
		{
			mSaleNowList = value;
		}
	}

	public List<consign_item> BuyList
	{
		get
		{
			return mBuyList;
		}
		set
		{
			mBuyList = value;
		}
	}

	public Guild PlayerGuild
	{
		get
		{
			return mPlayerGuild;
		}
		set
		{
			mPlayerGuild = value;
		}
	}

	public long GuildContribute
	{
		get
		{
			return mGuildContribute;
		}
		set
		{
			mGuildContribute = value;
		}
	}

	public bool IsGuildBattleRedTeam
	{
		get
		{
			return mIsGuildBattleRedTeam;
		}
		set
		{
			mIsGuildBattleRedTeam = value;
		}
	}

	public bool AutoUseDrag
	{
		get
		{
			return mAutoUseDrag;
		}
		set
		{
			mAutoUseDrag = value;
		}
	}

	public float AutoUseDragThreshold
	{
		get
		{
			return mAutoUseDragThreshold;
		}
		set
		{
			mAutoUseDragThreshold = value;
		}
	}

	public bool AutoUseSort
	{
		get
		{
			return mAutoUseSort;
		}
		set
		{
			mAutoUseSort = value;
		}
	}

	public bool AutoComabat
	{
		get
		{
			return mAutoComabat;
		}
		set
		{
			mAutoComabat = value;
		}
	}

	public bool IsOpenAutoCombat
	{
		get
		{
			return mIsOpenAutoCombat;
		}
		set
		{
			mIsOpenAutoCombat = value;
		}
	}

	public float BreakAutoCombatTime
	{
		get
		{
			return mBreakAutoCombatTime;
		}
		set
		{
			mBreakAutoCombatTime = value;
		}
	}

	public PlayerDanceData PlayerDanceData
	{
		get
		{
			return mPlayerDanceData;
		}
		set
		{
			mPlayerDanceData = value;
		}
	}

	public PlayerData()
	{
		Init();
	}

	private void Init()
	{
		mEquipPack = new ItemContainer(ItemContainer.EQUIPPACK_SIZE, ITEM_CONTAINER_TYPE.EQUIPPACK);
		mEquipBackPack = new ItemContainer(ItemContainer.EQUIP_BACKPACK_SIZE, ITEM_CONTAINER_TYPE.EQUIP_BACKPACK);
		mItemBackPack = new ItemContainer(ItemContainer.ITEM_BACKPACK_SIZE, ITEM_CONTAINER_TYPE.ITEM_BACKPACK);
		mBadgeBackPack = new ItemContainer(ItemContainer.BADGE_BACKPACK_SIZE, ITEM_CONTAINER_TYPE.BADGE_BACKPACK);
		mBadgeEquipPack = new ItemContainer(ItemContainer.BADGE_EQUIPPACK_SIZE, ITEM_CONTAINER_TYPE.BADGE_EQUIPPACK);
		mFashionBackPack = new ItemContainer(ItemContainer.FASHION_BACKPACK_SIZE, ITEM_CONTAINER_TYPE.FASHION_BACKPACK);
		mFashionEquipPack = new ItemContainer(ItemContainer.FASHION_EQUIPPACK_SIZE, ITEM_CONTAINER_TYPE.FASHION_EQUIPPACK);
		mTeam = new Team();
		mFriendInfo = new FriendInfo();
		mChatHistory = new PlayerChatHistory();
		mRecentSpeakers = new RecentSpeakerRecord();
		mPlayerGuild = new Guild();
		mPlayerDanceData = new PlayerDanceData();
		InitDragValue();
		InitOptionValue();
	}

	public static void SavePlayerAccountId(string id)
	{
		if (GameSettingData.IsLocalTestServer)
		{
			PlayerPrefs.SetString("player_account_id" + GameSettingData.LocalTestServerID + "unity4", id);
		}
		else
		{
			PlayerPrefs.SetString("player_account_id", id);
		}
	}

	public static void SavePlayerAccountKey(string key)
	{
		if (GameSettingData.IsLocalTestServer)
		{
			PlayerPrefs.SetString("player_account_key" + GameSettingData.LocalTestServerID + "unity4", key);
		}
		else
		{
			PlayerPrefs.SetString("player_account_key", key);
		}
	}

	public static void ClearAccount()
	{
		if (GameSettingData.IsLocalTestServer)
		{
			PlayerPrefs.DeleteKey("player_account_id" + GameSettingData.LocalTestServerID + "unity4");
			PlayerPrefs.DeleteKey("player_account_key" + GameSettingData.LocalTestServerID + "unity4");
		}
		else
		{
			PlayerPrefs.DeleteKey("player_account_id");
			PlayerPrefs.DeleteKey("player_account_key");
		}
	}

	public static string GetPlayerAccountId()
	{
		if (GameSettingData.IsLocalTestServer)
		{
			return PlayerPrefs.GetString("player_account_id" + GameSettingData.LocalTestServerID + "unity4", string.Empty);
		}
		return PlayerPrefs.GetString("player_account_id", string.Empty);
	}

	public static string GetPlayerAccountKey()
	{
		if (GameSettingData.IsLocalTestServer)
		{
			return PlayerPrefs.GetString("player_account_key" + GameSettingData.LocalTestServerID + "unity4", string.Empty);
		}
		return PlayerPrefs.GetString("player_account_key", string.Empty);
	}

	public void SetDragValue(float value)
	{
		value = Mathf.Clamp01(value);
		mAutoUseDragThreshold = value;
		LocalDataSaveManager.SetkeySystemUseDrag(value);
	}

	public void SetMusicDragValue(float value)
	{
		value = Mathf.Clamp01(value);
		mMusicDragValue = value;
		LocalDataSaveManager.SetmusicDrag(value);
	}

	public void SetSoundDragValue(float value)
	{
		value = Mathf.Clamp01(value);
		mSoundDragValue = value;
		LocalDataSaveManager.SetsoundDrag(value);
	}

	public void InitDragValue()
	{
		mAutoUseDragThreshold = LocalDataSaveManager.GetkeySystemUseDrag();
		mMusicDragValue = LocalDataSaveManager.GetmusicDrag();
		mSoundDragValue = LocalDataSaveManager.GetsoundDrag();
	}

	public void SetGraphics(int val)
	{
		mGraphicsQua = val;
		LocalDataSaveManager.SetGraphics(val);
		GameSettingData.PhoneClass = val;
		if (Singleton<ObjManager>.Exists)
		{
			Singleton<ObjManager>.Instance.ResetPhoneClass();
		}
	}

	public void SetStreetModel(int value)
	{
		mStreetRacingMode = value;
		LocalDataSaveManager.SetStreetModel(value);
	}

	public void SetScreenVibrat(int value)
	{
		mScreen_Vibrating = value;
		LocalDataSaveManager.SetScreenVibrat(value);
	}

	public void SetNotify(int value)
	{
		mNotify = value;
		LocalDataSaveManager.Setnotyfition(value);
	}

	public void SetChannel(int value)
	{
		if (mChannelValue != value)
		{
			mChannelValue = value;
			LocalDataSaveManager.SetChannelShow(mChannelValue);
			if (SingletonUnity<ChatBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ChatBaseRootLogic>.Instance.gameObject))
			{
				SingletonUnity<ChatBaseRootLogic>.Instance.UpdateMessage();
			}
			if (SingletonUnity<FunctionBtnRootLogic>.Exists)
			{
				SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateMessage();
			}
		}
	}

	public void SetChatShow(int value)
	{
		if (mChatShow != value)
		{
			mChatShow = value;
			LocalDataSaveManager.SetChatShow(value);
		}
	}

	public void SetNonMissionTarget(int value)
	{
		if (mNonMissionTarget != value)
		{
			mNonMissionTarget = value;
			LocalDataSaveManager.SetNonMissionTarget(value);
		}
	}

	public void SetVehicleTarget(int value)
	{
		if (mVehicleTarget != value)
		{
			mVehicleTarget = value;
			LocalDataSaveManager.SetVehicleTarget(value);
		}
	}

	public void InitOptionValue()
	{
		int num = GameSettingData.InitGraphic();
		int num2 = 0;
		num2 = ((num != 0) ? LocalDataSaveManager.GetGraphics(num) : num);
		mStreetRacingMode = LocalDataSaveManager.GetStreetModel();
		mScreen_Vibrating = LocalDataSaveManager.GetScreenVibrat();
		mNotify = LocalDataSaveManager.Getnotyfition();
		mChannelValue = LocalDataSaveManager.GetChannelShow();
		mNonMissionTarget = LocalDataSaveManager.GetNonMissionTarget();
		mVehicleTarget = LocalDataSaveManager.GetVehicleTarget();
		SetGraphics(num2);
	}

	public bool IsAcceptChannel(GameDefine.CHAT_CHANNEL_TYPE channeltype)
	{
		int num = -1;
		switch (channeltype)
		{
		case GameDefine.CHAT_CHANNEL_TYPE.SYSTEM:
			num = 0;
			break;
		case GameDefine.CHAT_CHANNEL_TYPE.WORLD:
			num = 1;
			break;
		case GameDefine.CHAT_CHANNEL_TYPE.NORMAL:
			num = 2;
			break;
		case GameDefine.CHAT_CHANNEL_TYPE.TEAM:
			num = 3;
			break;
		case GameDefine.CHAT_CHANNEL_TYPE.GUILD:
			num = 4;
			break;
		case GameDefine.CHAT_CHANNEL_TYPE.PRIVATE:
			num = 5;
			break;
		}
		if (num == -1)
		{
			return true;
		}
		if ((mChannelValue & (1 << num)) != 0)
		{
			return true;
		}
		return false;
	}

	public void CleanUpSkillDataList()
	{
		mMainPlayerSkillDataList.Clear();
		mMainPlayerSkillIDList.Clear();
	}

	public void SetVideoTimes(long times)
	{
		mVideoTimes = times;
	}

	public void SetVideoMaxTimes(long maxtimes)
	{
		mVideoMaxTimes = maxtimes;
	}

	public void SetVideoDiamond(long diamondnum)
	{
		mVideoDiamond = diamondnum;
	}

	public bool CheckLevel(int limitlevel)
	{
		return Level >= limitlevel;
	}

	public bool CheckLevel(int minlevel, int maxlevel)
	{
		return Level >= minlevel && Level <= maxlevel;
	}

	public int CheckIsLevelHeigh(int minlevel, int maxlevel)
	{
		if (Level < minlevel)
		{
			return 7;
		}
		if (Level > maxlevel)
		{
			return 6;
		}
		return 0;
	}

	public float GetDragTime01()
	{
		if (mDragCdTime > 0f)
		{
			return 1f - Mathf.Clamp01((Time.time - mDragCdTime) / 20f);
		}
		return 0f;
	}

	public bool CanUseDrag()
	{
		if (mDragCdTime <= 0f || mDragCdTime + 20f <= Time.time)
		{
			return true;
		}
		return false;
	}

	public void UpdateDragCD()
	{
		mDragCdTime = Time.time;
	}

	public bool CheckWeaponIsSame()
	{
		if (!string.IsNullOrEmpty(mWeaponItemId) && !string.IsNullOrEmpty(mFashionItemId))
		{
			EquipData equipDataById = DataManager.GetEquipDataById(mWeaponItemId);
			EquipData equipDataById2 = DataManager.GetEquipDataById(mFashionItemId);
			return equipDataById.WeaponType == equipDataById2.WeaponType;
		}
		if (!string.IsNullOrEmpty(PartWeaponId) && !string.IsNullOrEmpty(FashionWeaponId))
		{
			if (GameDefine.GetWeaponName(PartWeaponId).Equals(GameDefine.GetWeaponName(FashionWeaponId)))
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public void SetCash(long val)
	{
		mCash = val;
	}

	public void SetGold(long val)
	{
		mGold = val;
	}

	public void SetDiamond(long val)
	{
		mDiamond = val;
	}

	public void SetBattleCoin(long val)
	{
		mBattleCoin = val;
	}

	public void SetActivityCoin(long val)
	{
		mActivityCoin = val;
	}

	public int GetEquipCombatVal(EQUIP_PACK_TYPE packType, EQUIP_BACKPACK_TYPE targetPart)
	{
		return packType switch
		{
			EQUIP_PACK_TYPE.BACKPACK => mEquipPack.GetEquipByEquipType(targetPart)?.GetItemCombatVal() ?? 0, 
			EQUIP_PACK_TYPE.FASHION => mFashionBackPack.GetEquipByEquipType(targetPart)?.GetItemCombatVal() ?? 0, 
			_ => 0, 
		};
	}

	public ItemContainer GetItemContainer(ITEM_CONTAINER_TYPE type)
	{
		return type switch
		{
			ITEM_CONTAINER_TYPE.ITEM_BACKPACK => mItemBackPack, 
			ITEM_CONTAINER_TYPE.EQUIPPACK => mEquipPack, 
			ITEM_CONTAINER_TYPE.BADGE_EQUIPPACK => mBadgeEquipPack, 
			ITEM_CONTAINER_TYPE.BADGE_BACKPACK => mBadgeBackPack, 
			ITEM_CONTAINER_TYPE.EQUIP_BACKPACK => mEquipBackPack, 
			ITEM_CONTAINER_TYPE.FASHION_BACKPACK => mFashionBackPack, 
			ITEM_CONTAINER_TYPE.FASHION_EQUIPPACK => mFashionEquipPack, 
			_ => null, 
		};
	}

	public void UpdateEquipsTips()
	{
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.CheckTips(IsHaveBackPackTips(), GameDefine.TIPS_TYPE.CHARACTER);
			SingletonUnity<FunctionBtnRootLogic>.Instance.CheckTips(IsHaveItemTips(), GameDefine.TIPS_TYPE.ITEMS);
		}
		if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.RefershTips();
		}
	}

	public void UpdateEnhanceTips()
	{
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateEnhanceTipsFlag();
		}
		if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.RefershTips();
		}
	}

	public bool IsHaveEquipTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.CHARACTER_EQUIP))
		{
			return false;
		}
		for (int i = 0; i < mEquipBackPack.ContainerSize; i++)
		{
			if (!mEquipBackPack.ItemList[i].IsEmpty() && mEquipBackPack.ItemList[i].Parm[5] == 0)
			{
				return true;
			}
		}
		List<GameItem> itemList = mEquipPack.ItemList;
		List<GameItem> itemList2 = mEquipBackPack.ItemList;
		for (int j = 0; j < itemList.Count; j++)
		{
			GameItem gameItem = itemList[j];
			if (gameItem.IsEmpty())
			{
				continue;
			}
			ItemData itemData = gameItem.ItemData;
			ItemData itemData2 = null;
			for (int k = 0; k < itemList2.Count; k++)
			{
				GameItem gameItem2 = itemList2[k];
				if (!gameItem2.IsEmpty())
				{
					itemData2 = gameItem2.ItemData;
					if (itemData2.SubType == itemData.SubType && gameItem.GetItemQuality() < gameItem2.GetItemQuality() && gameItem2.Parm[5] == 0)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool IsHaveFashionEquipTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.CHARACTER_FASHION))
		{
			return false;
		}
		for (int i = 0; i < mFashionBackPack.ContainerSize; i++)
		{
			if (!mFashionBackPack.ItemList[i].IsEmpty() && mFashionBackPack.ItemList[i].Parm[5] == 0)
			{
				return true;
			}
		}
		List<GameItem> itemList = mFashionEquipPack.ItemList;
		List<GameItem> itemList2 = mFashionBackPack.ItemList;
		for (int j = 0; j < itemList.Count; j++)
		{
			GameItem gameItem = itemList[j];
			if (gameItem.IsEmpty())
			{
				continue;
			}
			ItemData itemData = gameItem.ItemData;
			ItemData itemData2 = null;
			for (int k = 0; k < itemList2.Count; k++)
			{
				GameItem gameItem2 = itemList2[k];
				if (!gameItem2.IsEmpty())
				{
					itemData2 = gameItem2.ItemData;
					if (itemData2.SubType == itemData.SubType && gameItem.GetItemCombatVal() < gameItem2.GetItemCombatVal() && gameItem2.Parm[5] == 0)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool IsHaveBadgeTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.CHARACTER_BADGE))
		{
			return false;
		}
		for (int i = 0; i < mBadgeBackPack.ContainerSize; i++)
		{
			if (!mBadgeBackPack.ItemList[i].IsEmpty() && mBadgeBackPack.ItemList[i].Parm[5] == 0)
			{
				return true;
			}
		}
		List<GameItem> itemList = mBadgeEquipPack.ItemList;
		List<GameItem> itemList2 = mBadgeBackPack.ItemList;
		for (int j = 0; j < itemList.Count; j++)
		{
			GameItem gameItem = itemList[j];
			if (gameItem.IsEmpty())
			{
				continue;
			}
			BadgeData badgeDataById = DataManager.GetBadgeDataById(gameItem.ItemId);
			BadgeData badgeData = null;
			for (int k = 0; k < itemList2.Count; k++)
			{
				GameItem gameItem2 = itemList2[j];
				if (!gameItem2.IsEmpty())
				{
					badgeData = DataManager.GetBadgeDataById(gameItem2.ItemId);
					if (gameItem.ItemData.SubType == gameItem2.ItemData.SubType && badgeDataById.Lv < badgeData.Lv && gameItem2.Parm[5] == 0)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool IsHaveItemTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.CHARACTER_ITEM))
		{
			return false;
		}
		for (int i = 0; i < mItemBackPack.ContainerSize; i++)
		{
			if (!mItemBackPack.ItemList[i].IsEmpty() && mItemBackPack.ItemList[i].Parm[5] == 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsHaveBackPackTips()
	{
		return IsHaveEquipTips() || IsHaveFashionEquipTips() || IsHaveBadgeTips() || IsHaveItemTips();
	}

	public bool IsHaveEnhanceandRefineTips()
	{
		return IsHaveEnhanceTips() || IsHaveRefineTips();
	}

	public bool IsHaveEnhanceTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ENHANCE_EQUIP))
		{
			return false;
		}
		for (int i = 0; i < mEquipPack.ContainerSize; i++)
		{
			if (!mEquipPack.ItemList[i].IsEmpty())
			{
				GameItem gameItem = mEquipPack.ItemList[i];
				EquipData equipDataById = DataManager.GetEquipDataById(gameItem.ItemId);
				long cash = GameMoneyHelper.GetCash();
				int upgradeMoneyByQualityAndLevel = equipDataById.GetUpgradeMoneyByQualityAndLevel((int)gameItem.GetItemQuality(), gameItem.ItemLevel);
				GameItem enhanceItem = ItemBackPack.GetEnhanceItem();
				int upgradeExpValByLevel = equipDataById.GetUpgradeExpValByLevel(gameItem.ItemLevel);
				if (enhanceItem == null)
				{
					return false;
				}
				if (mEquipPack.ItemList[i].ItemLevel < Level && cash >= upgradeMoneyByQualityAndLevel && enhanceItem.StackNum >= upgradeExpValByLevel)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsCanUpgradeWeapon()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ENHANCE_EQUIP))
		{
			return false;
		}
		for (int i = 0; i < mEquipPack.ContainerSize; i++)
		{
			if (!mEquipPack.ItemList[i].IsEmpty() && mEquipPack.ItemList[i].ItemData.SubType == 0)
			{
				GameItem gameItem = mEquipPack.ItemList[i];
				EquipData equipDataById = DataManager.GetEquipDataById(gameItem.ItemId);
				long cash = GameMoneyHelper.GetCash();
				int upgradeMoneyByQualityAndLevel = equipDataById.GetUpgradeMoneyByQualityAndLevel((int)gameItem.GetItemQuality(), gameItem.ItemLevel);
				GameItem enhanceItem = ItemBackPack.GetEnhanceItem();
				int upgradeExpValByLevel = equipDataById.GetUpgradeExpValByLevel(gameItem.ItemLevel);
				if (enhanceItem == null)
				{
					return false;
				}
				if (mEquipPack.ItemList[i].ItemLevel < Level && cash >= upgradeMoneyByQualityAndLevel && enhanceItem.StackNum >= upgradeExpValByLevel)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsHaveRefineTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ENHANCE_STAR))
		{
			return false;
		}
		RefineData[] array = new RefineData[5];
		for (int i = 1; i < 5; i++)
		{
			array[i] = DataManager.GetRefineDataByPartLevelPRO(i, MainPlayerAttrData.GetTargetRefinePartLevel((REFINE_PART)i), (int)Profession);
		}
		for (int j = 1; j < 5; j++)
		{
			RefineData refineData = array[j];
			if (refineData.Lv >= GameDefine.MAX_REFINE_LEVEL)
			{
				continue;
			}
			int itemStackNumById = ItemBackPack.GetItemStackNumById(refineData.CostId1);
			if (itemStackNumById < refineData.Cost1)
			{
				continue;
			}
			if (!string.IsNullOrEmpty(refineData.CostId2))
			{
				int itemStackNumById2 = ItemBackPack.GetItemStackNumById(refineData.CostId2);
				if (itemStackNumById2 < refineData.Cost2)
				{
					continue;
				}
			}
			if (refineData.MoneyType == 0)
			{
				if (GameMoneyHelper.GetCash() < refineData.MoneyCost)
				{
					continue;
				}
			}
			else if (GameMoneyHelper.GetGold() < refineData.MoneyCost)
			{
				continue;
			}
			return true;
		}
		return false;
	}

	public bool IsHaveCanInherit()
	{
		ItemContainer equipBackPack = EquipBackPack;
		ItemContainer equipPack = EquipPack;
		PROFESSION_TYPE profession = Profession;
		List<GameItem> equipItemList = ItemContainerTool.GetEquipItemList(equipPack);
		GameItem gameItem = equipItemList[ItemContainerTool.ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE.WEAPON)];
		GameItem gameItem2 = equipItemList[ItemContainerTool.ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE.HEAD)];
		GameItem gameItem3 = equipItemList[ItemContainerTool.ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE.BODY)];
		GameItem gameItem4 = equipItemList[ItemContainerTool.ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE.LEG)];
		GameItem gameItem5 = equipItemList[ItemContainerTool.ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE.BELT)];
		GameItem gameItem6 = equipItemList[ItemContainerTool.ChangeEquipTypeToIndex(EQUIP_BACKPACK_TYPE.NECKLACE)];
		if (gameItem != null && !gameItem.IsEmpty())
		{
			List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(equipBackPack, 0, profession, gameItem);
			for (int i = 0; i < targetTypeItem.Count; i++)
			{
				if (targetTypeItem[i].IsAppraise && targetTypeItem[i].IsHaveRandomAtt)
				{
					return true;
				}
			}
		}
		if (gameItem2 != null && !gameItem2.IsEmpty())
		{
			List<GameItem> targetTypeItem2 = ItemContainerTool.GetTargetTypeItem(equipBackPack, 1, profession);
			for (int j = 0; j < targetTypeItem2.Count; j++)
			{
				if (targetTypeItem2[j].IsAppraise && targetTypeItem2[j].IsHaveRandomAtt)
				{
					return true;
				}
			}
		}
		if (gameItem3 != null && !gameItem3.IsEmpty())
		{
			List<GameItem> targetTypeItem3 = ItemContainerTool.GetTargetTypeItem(equipBackPack, 2, profession);
			for (int k = 0; k < targetTypeItem3.Count; k++)
			{
				if (targetTypeItem3[k].IsAppraise && targetTypeItem3[k].IsHaveRandomAtt)
				{
					return true;
				}
			}
		}
		if (gameItem4 != null && !gameItem4.IsEmpty())
		{
			List<GameItem> targetTypeItem4 = ItemContainerTool.GetTargetTypeItem(equipBackPack, 3, profession);
			for (int l = 0; l < targetTypeItem4.Count; l++)
			{
				if (targetTypeItem4[l].IsAppraise && targetTypeItem4[l].IsHaveRandomAtt)
				{
					return true;
				}
			}
		}
		if (gameItem5 != null && !gameItem5.IsEmpty())
		{
			List<GameItem> targetTypeItem5 = ItemContainerTool.GetTargetTypeItem(equipBackPack, 4, profession);
			for (int m = 0; m < targetTypeItem5.Count; m++)
			{
				if (targetTypeItem5[m].IsAppraise && targetTypeItem5[m].IsHaveRandomAtt)
				{
					return true;
				}
			}
		}
		if (gameItem6 != null && !gameItem6.IsEmpty())
		{
			List<GameItem> targetTypeItem6 = ItemContainerTool.GetTargetTypeItem(equipBackPack, 5, profession);
			for (int n = 0; n < targetTypeItem6.Count; n++)
			{
				if (targetTypeItem6[n].IsAppraise && targetTypeItem6[n].IsHaveRandomAtt)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void UpdateDominInfo(ret_domin_info.request request)
	{
		if (request.HasDomin_infos)
		{
			Domin_InfoDic = request.domin_infos;
		}
		if (request.HasCharacters)
		{
			Domin_CharacterDic = request.characters;
		}
		FlashDominRootPage();
		UpdateDominInfoPage();
	}

	public void UpdateDominInfoPage()
	{
		if (SingletonUnity<DominInfoRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<DominInfoRootLogic>.Instance.gameObject))
		{
			SingletonUnity<DominInfoRootLogic>.Instance.FlashPage();
		}
	}

	public void FlashDominRootPage()
	{
		if (SingletonUnity<DominRootLogic>.Exists && SingletonUnity<DominRootLogic>.Instance.gameObject.active)
		{
			SingletonUnity<DominRootLogic>.Instance.Reset(Domin_InfoDic, Domin_CharacterDic);
		}
		if (SingletonUnity<MiniMap>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MiniMap>.Instance.gameObject))
		{
			SingletonUnity<MiniMap>.Instance.UpdateActionMapObj();
		}
	}

	public bool IsHaveTeam()
	{
		return mTeam != null && mTeam.TeamID != -1;
	}

	public bool IsTeamMember()
	{
		if (IsHaveTeam() && MainPlayerServerId != mTeam.TeamLeader.ServerId)
		{
			return true;
		}
		return false;
	}

	public bool IsTeamLeader()
	{
		if (IsHaveTeam() && MainPlayerServerId == mTeam.TeamLeader.ServerId)
		{
			return true;
		}
		return false;
	}

	public bool IsSingleTeam()
	{
		if (IsHaveTeam())
		{
			for (int i = 0; i < mTeam.TeamMembers.Length; i++)
			{
				if (mTeam.TeamMembers[i] != null && mTeam.TeamMembers[i].IsValid())
				{
					return false;
				}
			}
		}
		return true;
	}

	public bool IsCanInviteTeam(long teamId)
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.TEAM))
		{
			return false;
		}
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager == null || sceneManager.IsCarScene() || sceneManager.IsShowTeamScene())
		{
			return false;
		}
		if (mPreTeamInviteDic.ContainsKey(teamId))
		{
			if (Time.time - mPreTeamInviteDic[teamId] > TEAM_INVITE_INTERVAL)
			{
				mPreTeamInviteDic[teamId] = Time.time;
				return true;
			}
			return false;
		}
		if (mPreTeamInviteDic.Count > 10)
		{
			List<long> list = new List<long>(mPreTeamInviteDic.Keys);
			float num = Time.time - TEAM_INVITE_INTERVAL;
			for (int i = 0; i < list.Count; i++)
			{
				if (mPreTeamInviteDic[list[i]] < num)
				{
					mPreTeamInviteDic.Remove(list[i]);
				}
			}
		}
		mPreTeamInviteDic.Add(teamId, Time.time);
		return true;
	}

	public GameItem GetPotionItem()
	{
		return ItemBackPack.GetItemNoEmptyByIndexId(mCurSelectPotionIndex);
	}

	public void SetPKModeState(int pkMode)
	{
		if (pkMode != mMainPlayerAttrData.PkMode)
		{
			Singleton<ObjManager>.Instance.MainPlayer.SelectTarget(null);
		}
		mMainPlayerAttrData.PkMode = pkMode;
		if (SingletonUnity<TouXiangKuangLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TouXiangKuangLogic>.Instance.gameObject))
		{
			SingletonUnity<TouXiangKuangLogic>.Instance.UpdateStateBtn();
		}
	}

	public void CancelSuccess(long id)
	{
		if (mSaleNowList == null)
		{
			return;
		}
		for (int i = 0; i < mSaleNowList.Count; i++)
		{
			if (mSaleNowList[i].id == id)
			{
				mSaleNowList.RemoveAt(i);
				break;
			}
		}
	}

	public bool IsHaveGuild()
	{
		return mPlayerGuild != null && mPlayerGuild.IsGuildValid();
	}

	public bool isGuildChief()
	{
		return MainPlayerServerId == mPlayerGuild.GuildChiefId;
	}

	public float GetGuildSkillProgress()
	{
		float num = 0f;
		Dictionary<long, guild_skill> guildSkills = PlayerGuild.GuildSkills;
		List<guild_skill> list = new List<guild_skill>(guildSkills.Values);
		for (int i = 0; i < list.Count; i++)
		{
			num += (float)list[i].level;
		}
		return num / (float)(list.Count * 10);
	}

	public bool IsInMainPlayerPartBundleIdList(string id)
	{
		if (mMainPlayerPartBundleIdList.Contains(id))
		{
			return true;
		}
		return false;
	}

	public void UpdateMainPlayerPartBundleIdList(string[] curModelIdList)
	{
		mMainPlayerPartBundleIdList.Clear();
		ModelData modelData = null;
		for (int i = 0; i < curModelIdList.Length; i++)
		{
			if (string.IsNullOrEmpty(curModelIdList[i]))
			{
				continue;
			}
			modelData = DataManager.GetModeDataByID(curModelIdList[i]);
			if (modelData != null)
			{
				if (!mMainPlayerPartBundleIdList.Contains(modelData.Name))
				{
					mMainPlayerPartBundleIdList.Add(modelData.Name);
				}
				if (!string.IsNullOrEmpty(modelData.ModelPath) && !mMainPlayerPartBundleIdList.Contains(modelData.ModelPath))
				{
					mMainPlayerPartBundleIdList.Add(modelData.ModelPath);
				}
			}
		}
		if (!string.IsNullOrEmpty(MountId))
		{
			MountData mountDataById = DataManager.GetMountDataById(MountId);
			ModelData modeDataByID = DataManager.GetModeDataByID(mountDataById.ModelId);
			if (!mMainPlayerPartBundleIdList.Contains(modeDataByID.Name))
			{
				mMainPlayerPartBundleIdList.Add(modeDataByID.Name);
			}
			if (!string.IsNullOrEmpty(modeDataByID.ModelPath) && !mMainPlayerPartBundleIdList.Contains(modeDataByID.ModelPath))
			{
				mMainPlayerPartBundleIdList.Add(modeDataByID.ModelPath);
			}
		}
	}

	public void ResetPlayerData()
	{
		mItemBackPack.ClearContainer();
		mEquipPack.ClearContainer();
		mBadgeBackPack.ClearContainer();
		mBadgeEquipPack.ClearContainer();
		mEquipBackPack.ClearContainer();
		mFashionBackPack.ClearContainer();
		mFashionEquipPack.ClearContainer();
		CleanUpSkillDataList();
		mChatHistory.Reset();
		mRecentSpeakers.Reset();
		mFriendInfo.Init();
		mTeam.Reset();
		mPlayerDanceData.Reset();
		mMainPlayerPartBundleIdList.Clear();
		mPlayerGuild.ResetGuild();
		InitOptionValue();
		mAutoComabat = false;
		mIsOpenAutoCombat = false;
		mAutoUseDrag = true;
		IsFinishDownload = false;
		IsTutorialFinish = false;
		mMountId = string.Empty;
		mActivityData.Reset();
		mWelfareData.Reset();
		mRankPVPData.Reset();
		mCopyInfoData.Reset();
		mTowerData.Reset();
		UIManager.Reset();
		mPreTeamInviteDic.Clear();
		Domin_InfoDic.Clear();
		Domin_CharacterDic.Clear();
		MissionTeamTipLogic.mCurPage = -1;
	}
}
