using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
	public static bool initFlag = false;

	public static bool initDoneFlag = false;

	private static bool reImportFlag = false;

	private static Dictionary<string, List<SceneComponentData>> mSceneComponentDataDict = new Dictionary<string, List<SceneComponentData>>();

	private static Dictionary<string, SkillData> mSkillDataDict = new Dictionary<string, SkillData>();

	private static Dictionary<string, EffInfoData> mEffInfoDataDict = new Dictionary<string, EffInfoData>();

	private static Dictionary<string, ActionData> mActionDataDict = new Dictionary<string, ActionData>();

	private static Dictionary<string, CharacterModelData> mCharacterModelDataDict = new Dictionary<string, CharacterModelData>();

	private static Dictionary<string, ModelData> mModeDataDict = new Dictionary<string, ModelData>();

	private static Dictionary<string, BuffInfoData> mBuffInfoDataDict = new Dictionary<string, BuffInfoData>();

	private static Dictionary<string, NpcData> mNpcDataDict = new Dictionary<string, NpcData>();

	private static Dictionary<MAPTYPE, List<MapInfoData>> mMapInfoTypeDataDict = new Dictionary<MAPTYPE, List<MapInfoData>>();

	private static Dictionary<string, MapInfoData> mMapInfoDataDict = new Dictionary<string, MapInfoData>();

	private static Dictionary<string, List<FxEffInfoData>> mFxEffInfoDataDict = new Dictionary<string, List<FxEffInfoData>>();

	private static Dictionary<string, List<NPCPathData>> mNPCPathDataDic = new Dictionary<string, List<NPCPathData>>();

	private static Dictionary<string, List<MonsterData>> mMonsterDataDic = new Dictionary<string, List<MonsterData>>();

	private static Dictionary<string, ItemData> mItemDataDict = new Dictionary<string, ItemData>();

	private static Dictionary<int, List<string>> mLvAutoAcceptMissionDic = new Dictionary<int, List<string>>();

	private static Dictionary<string, MissionData> mMissionDataDict = new Dictionary<string, MissionData>();

	private static Dictionary<string, List<MissionData>> mMapMissionDataDic = new Dictionary<string, List<MissionData>>();

	private static List<MissionData> mSideMissionList = new List<MissionData>();

	private static Dictionary<string, NPCDialogData> mNPCDialogDataDict = new Dictionary<string, NPCDialogData>();

	private static Dictionary<string, MissionRequireData> mMissionRequireDataDict = new Dictionary<string, MissionRequireData>();

	private static Dictionary<string, NpcOptionDialogData> mNpcOptionDialogDataDict = new Dictionary<string, NpcOptionDialogData>();

	private static Dictionary<string, MapConnectInfoData> mMapConnectInfoDataDic = new Dictionary<string, MapConnectInfoData>();

	private static Dictionary<string, EquipData> mEquipDataDic = new Dictionary<string, EquipData>();

	private static Dictionary<int, SkillupgradeData> mSkillupgradeDataDic = new Dictionary<int, SkillupgradeData>();

	private static Dictionary<string, CopySceneData> mCopySceneDataDic = new Dictionary<string, CopySceneData>();

	private static List<CopySceneData> mCopySceneDataList = new List<CopySceneData>();

	public static Dictionary<string, LadderRewardData> mLadderRewardDataDic = new Dictionary<string, LadderRewardData>();

	private static List<LadderRewardData> mLadderRewardDataList = new List<LadderRewardData>();

	private static Dictionary<int, BaseLvData> mBaseLvDataDic = new Dictionary<int, BaseLvData>();

	private static Dictionary<string, TitleData> mTitleDateDic = new Dictionary<string, TitleData>();

	private static List<TitleData> mTitleList = new List<TitleData>();

	private static Dictionary<string, List<StoryData>> mStoryDataDic = new Dictionary<string, List<StoryData>>();

	private static Dictionary<int, List<EquipmentUpgradeData>> mEquipmentUpgradeDataDic = new Dictionary<int, List<EquipmentUpgradeData>>();

	private static Dictionary<int, DamageBoardTypeData> mDamageBoardTypeDataDic = new Dictionary<int, DamageBoardTypeData>();

	private static Dictionary<string, List<CamRockCurveData>> mCamRockCurveDataDic = new Dictionary<string, List<CamRockCurveData>>();

	private static Dictionary<string, CamRockData> mCamRockDataDic = new Dictionary<string, CamRockData>();

	private static Dictionary<string, SurveyMissionData> mSurveyMissionDataDic = new Dictionary<string, SurveyMissionData>();

	private static List<SurveyMissionData> mSurveyMissionDataList = new List<SurveyMissionData>();

	private static Dictionary<string, List<MultiDeliveryMissionData>> mMultiDeliveryMissionDataDic = new Dictionary<string, List<MultiDeliveryMissionData>>();

	private static Dictionary<string, DailyMissionData> mDailyMissionDataDic = new Dictionary<string, DailyMissionData>();

	private static Dictionary<int, List<RefineData>> mRefineDataDic = new Dictionary<int, List<RefineData>>();

	private static Dictionary<int, RefineLevelData> mRefineLevelDataDic = new Dictionary<int, RefineLevelData>();

	private static Dictionary<string, List<CarMissionBlockData>> mCarMissionBlockDataDic = new Dictionary<string, List<CarMissionBlockData>>();

	private static Dictionary<int, List<ConsignBuyTabData>> mConsignBuyTabDataDic = new Dictionary<int, List<ConsignBuyTabData>>();

	private static Dictionary<string, List<SneakingMissionData>> mSneakingMissionDataDic = new Dictionary<string, List<SneakingMissionData>>();

	private static Dictionary<string, BadgeData> mBadgeDataDic = new Dictionary<string, BadgeData>();

	private static List<BadgeData> BadgeDataList = null;

	private static Dictionary<int, TowerData> mTowerDataDic = new Dictionary<int, TowerData>();

	private static Dictionary<string, AIData> mAIDataDic = new Dictionary<string, AIData>();

	private static Dictionary<string, WildBossData> mWildBossDataDic = new Dictionary<string, WildBossData>();

	private static Dictionary<string, GuildBossData> mGuildBossDataDic = new Dictionary<string, GuildBossData>();

	private static Dictionary<string, BarFightCopyData> mBarFightCopyDataDic = new Dictionary<string, BarFightCopyData>();

	public static Dictionary<string, GuildDonateData> mGuildDonateDataDic = new Dictionary<string, GuildDonateData>();

	public static List<GuildLevelData> mGuildLevelDataList = new List<GuildLevelData>();

	private static Dictionary<int, List<GuildSkillData>> mGuildSkillDataDic = new Dictionary<int, List<GuildSkillData>>();

	private static Dictionary<string, TeamData> mTeamDataDic = new Dictionary<string, TeamData>();

	private static Dictionary<string, ShowRewardData> mShowRewardDataDic = new Dictionary<string, ShowRewardData>();

	private static Dictionary<int, AdaptData> mAdaptDataDic = new Dictionary<int, AdaptData>();

	private static Dictionary<string, FunctionData> mFunctionDataDic = new Dictionary<string, FunctionData>();

	private static Dictionary<int, SoundData> mSoundDataDic = new Dictionary<int, SoundData>();

	private static Dictionary<string, List<MapAreaInfoData>> mMapAreaInfoDataDic = new Dictionary<string, List<MapAreaInfoData>>();

	private static Dictionary<string, EscortData> mEscortDataDic = new Dictionary<string, EscortData>();

	private static Dictionary<string, CityDanceData> mCityDanceDataDic = new Dictionary<string, CityDanceData>();

	private static Dictionary<string, MountData> mMountDataDic = new Dictionary<string, MountData>();

	private static Dictionary<string, ColorData> mColorDataDic = new Dictionary<string, ColorData>();

	private static Dictionary<string, DanceData> mDanceDataDic = new Dictionary<string, DanceData>();

	private static Dictionary<string, SlotIconData> mSlotIconDataDic = new Dictionary<string, SlotIconData>();

	private static List<SlotIconData> mSlotIconList = new List<SlotIconData>();

	private static Dictionary<string, SlotAutoData> mSlotAutoDataDic = new Dictionary<string, SlotAutoData>();

	private static List<SlotAutoData> mSlotAutoList = new List<SlotAutoData>();

	private static Dictionary<string, SurviveBattleData> mSurviveBattleDataDic = new Dictionary<string, SurviveBattleData>();

	private static Dictionary<string, ScuffleData> mScuffleDataDic = new Dictionary<string, ScuffleData>();

	private static Dictionary<int, SignInMonthData> mSignInMonthDataDic = new Dictionary<int, SignInMonthData>();

	private static List<SignInMonthData> mSignInMonthDataList = new List<SignInMonthData>();

	private static Dictionary<string, DailyBuyData> mDailyBuyDataDic = new Dictionary<string, DailyBuyData>();

	private static Dictionary<string, InvestData> mInvestDataDic = new Dictionary<string, InvestData>();

	private static Dictionary<string, LevelPackageData> mLevelPackageDataDic = new Dictionary<string, LevelPackageData>();

	private static List<LevelPackageData> mLevelPackageDataList = new List<LevelPackageData>();

	private static Dictionary<string, RetrieveData> mRetrieveDataDic = new Dictionary<string, RetrieveData>();

	private static Dictionary<int, SignInWeekData> mSignInWeekDataDic = new Dictionary<int, SignInWeekData>();

	private static List<SignInWeekData> mSignInWeekDataList = new List<SignInWeekData>();

	private static Dictionary<string, DailyActiveData> mDailyActiveDataDic = new Dictionary<string, DailyActiveData>();

	private static List<DailyActiveData> mDailyActiveDataList = new List<DailyActiveData>();

	private static Dictionary<string, DailyActiveRewardData> mDailyActiveRewardDataDic = new Dictionary<string, DailyActiveRewardData>();

	private static List<DailyActiveRewardData> mDailyActiveRewardDataList = new List<DailyActiveRewardData>();

	private static Dictionary<string, FirstBuyData> mFirstBuyDataDic = new Dictionary<string, FirstBuyData>();

	private static Dictionary<string, BigPackageData> mBigPackageDataDic = new Dictionary<string, BigPackageData>();

	private static Dictionary<string, BigPackageTimeListData> mBigPackageTimeListDataDic = new Dictionary<string, BigPackageTimeListData>();

	private static Dictionary<string, PurchaseData> mPurchaseDataDic = new Dictionary<string, PurchaseData>();

	private static Dictionary<string, ModelPartData> mModelPartDataDic = new Dictionary<string, ModelPartData>();

	public static List<ModelPartData> mModelPartDataList = new List<ModelPartData>();

	private static Dictionary<string, DownloadRewardData> mDownloadRewardDataDic = new Dictionary<string, DownloadRewardData>();

	private static Dictionary<string, ShowModelData> mShowModelDataDic = new Dictionary<string, ShowModelData>();

	private static Dictionary<string, SocialDanceData> mSocialDanceDataDic = new Dictionary<string, SocialDanceData>();

	private static List<AnnounceData> mAnnounceDataList = new List<AnnounceData>();

	private static Dictionary<string, SexMiniData> mSexMiniDataDic = new Dictionary<string, SexMiniData>();

	private static Dictionary<string, ShopData> mShopDataDic = new Dictionary<string, ShopData>();

	public static List<ShopData> mShopDataList = new List<ShopData>();

	private static Dictionary<string, StrongerData> mStrongerDataDic = new Dictionary<string, StrongerData>();

	public static List<StrongerData> mStrongerDataList = new List<StrongerData>();

	private static Dictionary<string, LoadingUIData> mLoadingUIDataDic = new Dictionary<string, LoadingUIData>();

	public static List<LoadingUIData> mLoadingUIDataList = new List<LoadingUIData>();

	private static Dictionary<string, TimerActivityTipsData> mTimerActivityTipsDataDic = new Dictionary<string, TimerActivityTipsData>();

	private static List<TimerActivityTipsData> mTimerActivityTipsDataList = new List<TimerActivityTipsData>();

	private static Dictionary<string, TimerActivityData> mTimerActivityDataDic = new Dictionary<string, TimerActivityData>();

	private static Dictionary<string, ActivityBossData> mActivityBossDataDic = new Dictionary<string, ActivityBossData>();

	private static Dictionary<string, NotifyData> mNotifyDataDic = new Dictionary<string, NotifyData>();

	private static List<NotifyData> mNotifyDataList = new List<NotifyData>();

	private static Dictionary<string, LevelRewardData> mLevelRewardDataDic = new Dictionary<string, LevelRewardData>();

	private static List<LevelRewardData> mLevelRewardDataList = new List<LevelRewardData>();

	private static Dictionary<string, GuildBattleData> mGuildBattleDataDic = new Dictionary<string, GuildBattleData>();

	private static Dictionary<string, ConfigData> mConfigDataDic = new Dictionary<string, ConfigData>();

	private static List<ConfigData> mConfigDataStarScoreList = new List<ConfigData>();

	private static List<ConfigData> mConfigDataQualityScoreList = new List<ConfigData>();

	private static Dictionary<string, GuildStarData> mGuildStarDataDic = new Dictionary<string, GuildStarData>();

	private static List<GuildStarData> mGuildStarDataList = new List<GuildStarData>();

	private static Dictionary<string, ActivityMapData> mActivityMapDataDic = new Dictionary<string, ActivityMapData>();

	private static Dictionary<string, List<ActivityMapData>> mActivityMapDataByMapIdDic = new Dictionary<string, List<ActivityMapData>>();

	private static Dictionary<int, Dictionary<int, List<ActivityMapData>>> mActivityMapDataByTypeDic = new Dictionary<int, Dictionary<int, List<ActivityMapData>>>();

	private static Dictionary<string, MonthlyCardData> mMonthlyCardDataDic = new Dictionary<string, MonthlyCardData>();

	private static Dictionary<string, MoveTargetMissionData> mMoveTargetMissionDataDic = new Dictionary<string, MoveTargetMissionData>();

	private static Dictionary<string, PoliceLevelData> mPoliceLevelDataDic = new Dictionary<string, PoliceLevelData>();

	private static Dictionary<string, List<QualityData>> mQualityDataDic = new Dictionary<string, List<QualityData>>();

	private static Dictionary<int, SingleMapLockData> mSingleMapLockDataDic = new Dictionary<int, SingleMapLockData>();

	private static Dictionary<string, KillTargetMissionData> mKillTargetMissionDataDic = new Dictionary<string, KillTargetMissionData>();

	private static Dictionary<string, TargetCarMissionData> mTargetCarMissionDataDic = new Dictionary<string, TargetCarMissionData>();

	private static Dictionary<string, DailyExpData> mDailyExpDataDic = new Dictionary<string, DailyExpData>();

	private static Dictionary<string, OnlineMissionData> mOnlineMissionDataDict = new Dictionary<string, OnlineMissionData>();

	private static Dictionary<string, DominData> mDominDataDict = new Dictionary<string, DominData>();

	private static Dictionary<string, LevelSealData> mLevelSealDataDict = new Dictionary<string, LevelSealData>();

	private static Dictionary<string, TimeLimitMissionData> mTimeLimitMissionDataDict = new Dictionary<string, TimeLimitMissionData>();

	private static Dictionary<string, SkillLabelData> mSkillLabelDataDict = new Dictionary<string, SkillLabelData>();

	private static Dictionary<string, GuildCaptureData> mGuildCaptureDataDict = new Dictionary<string, GuildCaptureData>();

	public static bool DataInitFinishFlag => initDoneFlag && AnimationManager.initStreamDoneFlag && AnimationManager.initDownloadDoneFlag;

	public static Dictionary<string, NpcData> NpcDataDict => mNpcDataDict;

	public static Dictionary<int, TowerData> TowerDataDic
	{
		get
		{
			if (mTowerDataDic.Count == 0)
			{
				mTowerDataDic = DataReader.LoadTable<int, TowerData>("TowerData", "FloorID");
			}
			return mTowerDataDic;
		}
	}

	public static Dictionary<int, SingleMapLockData> SingleMapLockDataDic
	{
		get
		{
			if (mSingleMapLockDataDic == null || mSingleMapLockDataDic.Count == 0)
			{
				mSingleMapLockDataDic = DataReader.LoadTable<int, SingleMapLockData>("SingleMapLockData", "ID");
			}
			return mSingleMapLockDataDic;
		}
	}

	public static void InitData(MonoBehaviour mono)
	{
		if (!initFlag)
		{
			initDoneFlag = false;
			initFlag = true;
			if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
			{
				mono.StartCoroutine(BundleManager.LoadData(OnLoadBundleFinished));
			}
		}
	}

	public static void ReImportData(MonoBehaviour mono)
	{
		initDoneFlag = false;
		reImportFlag = true;
		if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
		{
			mono.StartCoroutine(BundleManager.LoadData(OnLoadBundleFinished));
		}
	}

	public static void LoadBaseLocalization()
	{
		LocalizationManager.ResourceLoadDictionary();
	}

	private static void OnLoadBundleFinished()
	{
		ClearAllList();
		mSkillDataDict = DataReader.LoadTable<string, SkillData>("SkillData", "ID");
		foreach (KeyValuePair<string, SkillData> item in mSkillDataDict)
		{
			item.Value.Init();
		}
		mEffInfoDataDict = DataReader.LoadTable<string, EffInfoData>("EffInfoData", "ID");
		foreach (KeyValuePair<string, EffInfoData> item2 in mEffInfoDataDict)
		{
			item2.Value.Init();
		}
		mActionDataDict = DataReader.LoadTable<string, ActionData>("ActionData", "ID");
		mBuffInfoDataDict = DataReader.LoadTable<string, BuffInfoData>("BuffInfoData", "ID");
		mNpcDataDict = DataReader.LoadTable<string, NpcData>("NpcData", "ID");
		mMapInfoDataDict = DataReader.LoadTable<string, MapInfoData>("MapInfoData", "ID");
		mCharacterModelDataDict = DataReader.LoadTable<string, CharacterModelData>("CharacterModelData", "ID");
		foreach (KeyValuePair<string, CharacterModelData> item3 in mCharacterModelDataDict)
		{
			item3.Value.Init();
		}
		mFxEffInfoDataDict = DataReader.LoadTableList<string, FxEffInfoData>("FxEffectData", "ID");
		mItemDataDict = DataReader.LoadTable<string, ItemData>("ItemData", "ID");
		mMissionDataDict = DataReader.LoadMissionDataTable("MissionData", "ID", ref mLvAutoAcceptMissionDic);
		mNPCDialogDataDict = DataReader.LoadTable<string, NPCDialogData>("NPCDialogData", "ID");
		mMissionRequireDataDict = DataReader.LoadTable<string, MissionRequireData>("MissionRequireData", "ID");
		mNpcOptionDialogDataDict = DataReader.LoadTable<string, NpcOptionDialogData>("NPCOptionDialogData", "ID");
		mEquipDataDic = DataReader.LoadTable<string, EquipData>("EquipData", "ID");
		if (GameManager.IsSupportCurDataVersion184())
		{
			mSkillupgradeDataDic = DataReader.LoadTable<int, SkillupgradeData>("SkillupgradeData", "level");
		}
		mCopySceneDataDic = DataReader.LoadTable<string, CopySceneData>("CopySceneData", "ID");
		mBaseLvDataDic = DataReader.LoadTable<int, BaseLvData>("BaseLvData", "Lv");
		mTitleDateDic = DataReader.LoadTable<string, TitleData>("TitleData", "ID");
		mStoryDataDic = DataReader.LoadTableList<string, StoryData>("StoryData", "ID");
		mEquipmentUpgradeDataDic = DataReader.LoadTableList<int, EquipmentUpgradeData>("EquipmentUpgradeData", "PartID");
		mModeDataDict = DataReader.LoadTable<string, ModelData>("ModelData", "ID");
		mMapConnectInfoDataDic = DataReader.LoadTable<string, MapConnectInfoData>("MapConnectInfoData", "ID");
		mDamageBoardTypeDataDic = DataReader.LoadTable<int, DamageBoardTypeData>("DamageBoardTypeData", "ID");
		mCamRockCurveDataDic = DataReader.LoadTableList<string, CamRockCurveData>("CamRockCurveData", "AnimaClipName");
		mCamRockDataDic = DataReader.LoadTable<string, CamRockData>("CamRockData", "ID");
		mSurveyMissionDataDic = DataReader.LoadTable<string, SurveyMissionData>("SurveyMissionData", "ID");
		mDailyMissionDataDic = DataReader.LoadTable<string, DailyMissionData>("DailyMissionData", "ID");
		mMultiDeliveryMissionDataDic = DataReader.LoadTableList<string, MultiDeliveryMissionData>("MultiDeliveryMissionData", "ID");
		mRefineDataDic = DataReader.LoadTableList<int, RefineData>("RefineData", "Part");
		mRefineLevelDataDic = DataReader.LoadTable<int, RefineLevelData>("RefineLevelData", "ID");
		mCarMissionBlockDataDic = DataReader.LoadTableList<string, CarMissionBlockData>("CarMissionBlockData", "SceneID");
		mConsignBuyTabDataDic = DataReader.LoadTableList<int, ConsignBuyTabData>("ConsignBuyTabData", "TopTabId");
		mSneakingMissionDataDic = DataReader.LoadTableList<string, SneakingMissionData>("SneakingMissionData", "MapID");
		mBadgeDataDic = DataReader.LoadTable<string, BadgeData>("BadgeData", "ID");
		mTowerDataDic = DataReader.LoadTable<int, TowerData>("TowerData", "FloorID");
		mAIDataDic = DataReader.LoadTable<string, AIData>("AIData", "ID");
		mWildBossDataDic = DataReader.LoadTable<string, WildBossData>("WildBossData", "ID");
		mBarFightCopyDataDic = DataReader.LoadTable<string, BarFightCopyData>("BarFightCopyData", "ID");
		mTeamDataDic = DataReader.LoadTable<string, TeamData>("TeamData", "ID");
		mAdaptDataDic = DataReader.LoadDicTable<int, AdaptData>("AdaptData", "ID", "DorpKeyDic");
		LocalizationManager.BundleLoadDictionary();
		mMonsterDataDic = DataReader.LoadTableList<string, MonsterData>("MonsterData", "MapID");
		mNPCPathDataDic = DataReader.LoadTableList<string, NPCPathData>("NPCPathData", "ID");
		mGuildBossDataDic = DataReader.LoadTable<string, GuildBossData>("GuildBossData", "ID");
		mGuildDonateDataDic = DataReader.LoadTable<string, GuildDonateData>("GuildDonateData", "ID");
		mGuildSkillDataDic = DataReader.LoadTableList<int, GuildSkillData>("GuildSkillData", "GuildSkillType");
		mGuildLevelDataList = DataReader.LoadImportData<GuildLevelData>("GuildLevelData");
		mShowRewardDataDic = DataReader.LoadTable<string, ShowRewardData>("ShowRewardData", "ID");
		mFunctionDataDic = DataReader.LoadTable<string, FunctionData>("FunctionData", "ID");
		mSoundDataDic = DataReader.LoadTable<int, SoundData>("SoundData", "Id");
		mMapAreaInfoDataDic = DataReader.LoadTableList<string, MapAreaInfoData>("MapAreaInfoData", "ID");
		foreach (KeyValuePair<string, List<MapAreaInfoData>> item4 in mMapAreaInfoDataDic)
		{
			for (int i = 0; i < item4.Value.Count; i++)
			{
				item4.Value[i].Init();
			}
		}
		mEscortDataDic = DataReader.LoadTable<string, EscortData>("EscortData", "ID");
		mCityDanceDataDic = DataReader.LoadTable<string, CityDanceData>("CityDanceData", "ID");
		mDanceDataDic = DataReader.LoadTable<string, DanceData>("DanceData", "ID");
		mLadderRewardDataDic = DataReader.LoadTable<string, LadderRewardData>("LadderRewardData", "ID");
		mMountDataDic = DataReader.LoadTable<string, MountData>("MountData", "ID");
		mColorDataDic = DataReader.LoadTable<string, ColorData>("ColorData", "ID");
		mSlotIconDataDic = DataReader.LoadTable<string, SlotIconData>("SlotIconData", "ID");
		mSlotAutoDataDic = DataReader.LoadTable<string, SlotAutoData>("SlotAutoData", "ID");
		mSurviveBattleDataDic = DataReader.LoadTable<string, SurviveBattleData>("SurviveBattleData", "ID");
		mSignInMonthDataDic = DataReader.LoadTable<int, SignInMonthData>("SignInMonthData", "Day");
		mDailyBuyDataDic = DataReader.LoadTable<string, DailyBuyData>("DailyBuyData", "ID");
		mInvestDataDic = DataReader.LoadTable<string, InvestData>("InvestData", "ID");
		mLevelPackageDataDic = DataReader.LoadTable<string, LevelPackageData>("LevelPackageData", "ID");
		mRetrieveDataDic = DataReader.LoadTable<string, RetrieveData>("RetrieveData", "ID");
		mSignInWeekDataDic = DataReader.LoadTable<int, SignInWeekData>("SignInWeekData", "Day");
		mDailyActiveDataDic = DataReader.LoadTable<string, DailyActiveData>("DailyActiveData", "ID");
		mDailyActiveRewardDataDic = DataReader.LoadTable<string, DailyActiveRewardData>("DailyActiveRewardData", "ID");
		mFirstBuyDataDic = DataReader.LoadTable<string, FirstBuyData>("FirstBuyData", "ID");
		mBigPackageDataDic = DataReader.LoadTable<string, BigPackageData>("BigPackageData", "ID");
		mPurchaseDataDic = DataReader.LoadTable<string, PurchaseData>("PurchaseData", "ProductId");
		mModelPartDataDic = DataReader.LoadTable<string, ModelPartData>("ModelPartData", "ID");
		mShowModelDataDic = DataReader.LoadTable<string, ShowModelData>("ShowModelData", "ID");
		mScuffleDataDic = DataReader.LoadTable<string, ScuffleData>("ScuffleData", "ID");
		mSocialDanceDataDic = DataReader.LoadTable<string, SocialDanceData>("SocialDanceData", "ID");
		mAnnounceDataList = DataReader.LoadImportData<AnnounceData>("AnnounceData");
		mDownloadRewardDataDic = DataReader.LoadTable<string, DownloadRewardData>("DownloadRewardData", "ID");
		mSexMiniDataDic = DataReader.LoadTable<string, SexMiniData>("SexMiniData", "ID");
		mShopDataDic = DataReader.LoadTable<string, ShopData>("ShopData", "ID");
		mStrongerDataDic = DataReader.LoadTable<string, StrongerData>("StrongerData", "ID");
		mLoadingUIDataDic = DataReader.LoadTable<string, LoadingUIData>("LoadingUIData", "ID");
		mSceneComponentDataDict = DataReader.LoadTableList<string, SceneComponentData>("SceneComponentData", "MapID");
		mTimerActivityTipsDataDic = DataReader.LoadTable<string, TimerActivityTipsData>("TimerActivityTipsData", "ID");
		mTimerActivityDataDic = DataReader.LoadTable<string, TimerActivityData>("TimerActivityData", "ID");
		mActivityBossDataDic = DataReader.LoadTable<string, ActivityBossData>("ActivityBossData", "Key");
		mNotifyDataDic = DataReader.LoadTable<string, NotifyData>("NotifyData", "ID");
		mLevelRewardDataDic = DataReader.LoadTable<string, LevelRewardData>("LevelRewardData", "ID");
		mGuildStarDataDic = DataReader.LoadTable<string, GuildStarData>("GuildStarData", "ID");
		mGuildBattleDataDic = DataReader.LoadTable<string, GuildBattleData>("GuildBattleData", "ID");
		mActivityMapDataDic = DataReader.LoadTable<string, ActivityMapData>("ActivityMapData", "ID");
		mConfigDataDic = DataReader.LoadTable<string, ConfigData>("ConfigData", "Key");
		mMonthlyCardDataDic = DataReader.LoadTable<string, MonthlyCardData>("MonthlyCardData", "ID");
		mMoveTargetMissionDataDic = DataReader.LoadTable<string, MoveTargetMissionData>("MoveTargetMissionData", "ID");
		mPoliceLevelDataDic = DataReader.LoadTable<string, PoliceLevelData>("PoliceLevelData", "ID");
		mSingleMapLockDataDic = DataReader.LoadTable<int, SingleMapLockData>("SingleMapLockData", "ID");
		mKillTargetMissionDataDic = DataReader.LoadTable<string, KillTargetMissionData>("KillTargetMissionData", "ID");
		mDailyExpDataDic = DataReader.LoadTable<string, DailyExpData>("DailyExpData", "ID");
		mOnlineMissionDataDict = DataReader.LoadTable<string, OnlineMissionData>("OnlineMissionData", "ID");
		mDominDataDict = DataReader.LoadTable<string, DominData>("DominData", "ID");
		mLevelSealDataDict = DataReader.LoadTable<string, LevelSealData>("LevelSealData", "ID");
		mTargetCarMissionDataDic = DataReader.LoadTable<string, TargetCarMissionData>("TargetCarMissionData", "ID");
		mBigPackageTimeListDataDic = DataReader.LoadTable<string, BigPackageTimeListData>("BigPackageTimeListData", "ID");
		mTimeLimitMissionDataDict = DataReader.LoadTable<string, TimeLimitMissionData>("TimeLimitMissionData", "ID");
		mSkillLabelDataDict = DataReader.LoadTable<string, SkillLabelData>("SkillLabelData", "ID");
		mGuildCaptureDataDict = DataReader.LoadTable<string, GuildCaptureData>("GuildCaptureData", "ID");
		mQualityDataDic = DataReader.LoadTableList<string, QualityData>("QualityData", "ID");
		initDoneFlag = true;
		BundleManager.UnloadDataBundle();
		if (!reImportFlag)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ResetPlayerData();
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.ClearData();
			if (SingletonUnity<MenuSceneController>.Exists)
			{
				SingletonUnity<MenuSceneController>.Instance.DataLoadFinish();
			}
		}
		else
		{
			reImportFlag = false;
		}
	}

	private static void ClearAllList()
	{
		mLadderRewardDataList.Clear();
		mCopySceneDataList.Clear();
		mTitleList.Clear();
		mSurveyMissionDataList.Clear();
		BadgeDataList = null;
		mGuildLevelDataList.Clear();
		mSlotIconList.Clear();
		mSlotAutoList.Clear();
		mSignInMonthDataList.Clear();
		mLevelPackageDataList.Clear();
		mSignInWeekDataList.Clear();
		mDailyActiveDataList.Clear();
		mDailyActiveRewardDataList.Clear();
		mModelPartDataList.Clear();
		mAnnounceDataList.Clear();
		mShopDataList.Clear();
		mStrongerDataList.Clear();
		mLoadingUIDataList.Clear();
		mTimerActivityTipsDataList.Clear();
		mNotifyDataList.Clear();
		mLevelRewardDataList.Clear();
		mMapMissionDataDic.Clear();
		mActivityMapDataByMapIdDic.Clear();
		mActivityMapDataByTypeDic.Clear();
		mSideMissionList.Clear();
		if (mConfigDataDic != null)
		{
			mConfigDataDic.Clear();
		}
	}

	public static List<SceneComponentData> GetSceneComponentDataListById(string mapId)
	{
		if (mSceneComponentDataDict != null && mSceneComponentDataDict.Count == 0)
		{
			mSceneComponentDataDict = DataReader.LoadTableList<string, SceneComponentData>("SceneComponentData", "MapID");
		}
		return DataReader.GetTableList(mSceneComponentDataDict, mapId);
	}

	public static SkillData GetSkillDataById(string id)
	{
		if (mSkillDataDict.Count == 0)
		{
			mSkillDataDict = DataReader.LoadTable<string, SkillData>("SkillData", "ID");
		}
		return DataReader.GetTableRow(mSkillDataDict, id);
	}

	public static EffInfoData GetEffInfoDataById(string id)
	{
		if (mEffInfoDataDict.Count == 0)
		{
			mEffInfoDataDict = DataReader.LoadTable<string, EffInfoData>("EffInfoData", "ID");
		}
		return DataReader.GetTableRow(mEffInfoDataDict, id);
	}

	public static ActionData GetActionDataByName(string name)
	{
		if (mActionDataDict.Count == 0)
		{
			mActionDataDict = DataReader.LoadTable<string, ActionData>("ActionData", "ID");
		}
		return DataReader.GetTableRow(mActionDataDict, name);
	}

	public static CharacterModelData GetCharacterModelDataByID(string id)
	{
		if (mCharacterModelDataDict.Count == 0)
		{
			mCharacterModelDataDict = DataReader.LoadTable<string, CharacterModelData>("CharacterModelData", "ID");
		}
		return DataReader.GetTableRow(mCharacterModelDataDict, id);
	}

	public static ModelData GetModeDataByID(string id)
	{
		if (mModeDataDict.Count == 0)
		{
			mModeDataDict = DataReader.LoadTable<string, ModelData>("ModelData", "ID");
		}
		return DataReader.GetTableRow(mModeDataDict, id);
	}

	public static BuffInfoData GetBuffInfoDataByID(string id)
	{
		if (mBuffInfoDataDict.Count == 0)
		{
			mBuffInfoDataDict = DataReader.LoadTable<string, BuffInfoData>("BuffInfoData", "ID");
		}
		return DataReader.GetTableRow(mBuffInfoDataDict, id);
	}

	public static NpcData GetNpcDataByID(string id)
	{
		if (mNpcDataDict.Count == 0)
		{
			mNpcDataDict = DataReader.LoadTable<string, NpcData>("NpcData", "ID");
		}
		return DataReader.GetTableRow(mNpcDataDict, id);
	}

	public static List<MapInfoData> GetMapInfoDataListByType(MAPTYPE type)
	{
		if (mMapInfoTypeDataDict.ContainsKey(type))
		{
			return mMapInfoTypeDataDict[type];
		}
		Dictionary<string, MapInfoData> allMapInfo = GetAllMapInfo();
		List<MapInfoData> list = new List<MapInfoData>();
		foreach (KeyValuePair<string, MapInfoData> item in allMapInfo)
		{
			if (item.Value.MapType == type)
			{
				list.Add(item.Value);
			}
		}
		if (list.Count > 0)
		{
			mMapInfoTypeDataDict.Add(type, list);
			return list;
		}
		return null;
	}

	public static MapInfoData GetMapInfoDataByID(string id)
	{
		if (mMapInfoDataDict.Count == 0)
		{
			mMapInfoDataDict = DataReader.LoadTable<string, MapInfoData>("MapInfoData", "ID");
		}
		return DataReader.GetTableRow(mMapInfoDataDict, id);
	}

	public static MapInfoData GetMapInfoDataByID(GameDefine.SCENE_DEFINE sceneId)
	{
		int num = (int)sceneId;
		return GetMapInfoDataByID(num.ToString());
	}

	public static Dictionary<string, MapInfoData> GetAllMapInfo()
	{
		if (mMapInfoDataDict.Count == 0)
		{
			mMapInfoDataDict = DataReader.LoadTable<string, MapInfoData>("MapInfoData", "ID");
		}
		return mMapInfoDataDict;
	}

	public static List<FxEffInfoData> GetFxEffInfoDataListById(string id)
	{
		if (mFxEffInfoDataDict.Count == 0)
		{
			mFxEffInfoDataDict = DataReader.LoadTableList<string, FxEffInfoData>("FxEffectData", "ID");
		}
		return DataReader.GetTableList(mFxEffInfoDataDict, id);
	}

	public static List<NPCPathData> GetNPCPathDataListById(string id)
	{
		if (mNPCPathDataDic.Count == 0)
		{
			mNPCPathDataDic = DataReader.LoadTableList<string, NPCPathData>("NPCPathData", "ID");
		}
		return DataReader.GetTableList(mNPCPathDataDic, id);
	}

	public static List<MonsterData> GetMonsterDataListByMapId(string mapId)
	{
		if (mMonsterDataDic.Count == 0)
		{
			mMonsterDataDic = DataReader.LoadTableList<string, MonsterData>("MonsterData", "MapID");
		}
		return DataReader.GetTableList(mMonsterDataDic, mapId);
	}

	public static Vector3 GetNPCPosInMonsterData(string mapId, string npcId)
	{
		List<MonsterData> monsterDataListByMapId = GetMonsterDataListByMapId(mapId);
		for (int i = 0; i < monsterDataListByMapId.Count; i++)
		{
			if (monsterDataListByMapId[i].NpcID.Equals(npcId))
			{
				return monsterDataListByMapId[i].GetNpcPos();
			}
		}
		return Vector3.zero;
	}

	public static bool GetNPCPosInMonsterData2(string mapId, string npcId, out Vector3 pos)
	{
		pos = Vector3.zero;
		List<MonsterData> monsterDataListByMapId = GetMonsterDataListByMapId(mapId);
		for (int i = 0; i < monsterDataListByMapId.Count; i++)
		{
			if (monsterDataListByMapId[i].NpcID.Equals(npcId))
			{
				pos = monsterDataListByMapId[i].GetNpcPos();
				return true;
			}
		}
		return false;
	}

	public static ItemData GetItemDataByID(string id)
	{
		if (mItemDataDict.Count == 0)
		{
			mItemDataDict = DataReader.LoadTable<string, ItemData>("ItemData", "ID");
		}
		return DataReader.GetTableRow(mItemDataDict, id);
	}

	public static List<string> GetLvAutoAcceptMissionListByLevel(int lv)
	{
		if (mLvAutoAcceptMissionDic.ContainsKey(lv))
		{
			return mLvAutoAcceptMissionDic[lv];
		}
		return null;
	}

	public static MissionData GetMissionDataByID(string id)
	{
		if (mMissionDataDict.Count == 0)
		{
			mMissionDataDict = DataReader.LoadTable<string, MissionData>("MissionData", "ID");
		}
		return DataReader.GetTableRow(mMissionDataDict, id);
	}

	public static List<MissionData> GetAcceptMissionDataByMapId(string mapId)
	{
		if (mMapMissionDataDic.Count == 0)
		{
			List<MissionData> list = new List<MissionData>(mMissionDataDict.Values);
			for (int i = 0; i < list.Count; i++)
			{
				if (mMapMissionDataDic.ContainsKey(list[i].AcceptMapId))
				{
					mMapMissionDataDic[list[i].AcceptMapId].Add(list[i]);
					continue;
				}
				mMapMissionDataDic.Add(list[i].AcceptMapId, new List<MissionData>());
				mMapMissionDataDic[list[i].AcceptMapId].Add(list[i]);
			}
		}
		if (mMapMissionDataDic.ContainsKey(mapId))
		{
			return mMapMissionDataDic[mapId];
		}
		return null;
	}

	public static List<MissionData> GetAllSideMissionList()
	{
		if (mSideMissionList.Count == 0)
		{
			List<MissionData> list = new List<MissionData>(mMissionDataDict.Values);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Class == 2 || list[i].Class == 8)
				{
					mSideMissionList.Add(list[i]);
				}
			}
		}
		return mSideMissionList;
	}

	public static NPCDialogData GetNPCDialogDataByID(string id)
	{
		if (mNPCDialogDataDict.Count == 0)
		{
			mNPCDialogDataDict = DataReader.LoadTable<string, NPCDialogData>("NPCDialogData", "ID");
		}
		return DataReader.GetTableRow(mNPCDialogDataDict, id);
	}

	public static MissionRequireData GetMissionRequireDataByID(string id)
	{
		if (mMissionRequireDataDict.Count == 0)
		{
			mMissionRequireDataDict = DataReader.LoadTable<string, MissionRequireData>("MissionRequireData", "ID");
		}
		return DataReader.GetTableRow(mMissionRequireDataDict, id);
	}

	public static NpcOptionDialogData GetNpcOptionDialogDataByID(string id)
	{
		if (mNpcOptionDialogDataDict.Count == 0)
		{
			mNpcOptionDialogDataDict = DataReader.LoadTable<string, NpcOptionDialogData>("NpcOptionDialogData", "ID");
		}
		return DataReader.GetTableRow(mNpcOptionDialogDataDict, id);
	}

	public static List<MapConnectInfoData> GetMapConnectData()
	{
		if (mMapConnectInfoDataDic.Count == 0)
		{
			mMapConnectInfoDataDic = DataReader.LoadTable<string, MapConnectInfoData>("MapConnectInfoData", "ID");
		}
		return new List<MapConnectInfoData>(mMapConnectInfoDataDic.Values);
	}

	public static EquipData GetEquipDataById(string id)
	{
		if (mEquipDataDic.Count == 0)
		{
			mEquipDataDic = DataReader.LoadTable<string, EquipData>("EquipData", "ID");
		}
		return DataReader.GetTableRow(mEquipDataDic, id);
	}

	public static SkillupgradeData GetSkillupgradeDataByLevel(int level)
	{
		if (!GameManager.IsSupportCurDataVersion184())
		{
			return null;
		}
		if (mSkillupgradeDataDic.Count == 0)
		{
			mSkillupgradeDataDic = DataReader.LoadTable<int, SkillupgradeData>("SkillupgradeData", "level");
		}
		return DataReader.GetTableRow(mSkillupgradeDataDic, level);
	}

	public static CopySceneData GetCopySceneDataById(string id)
	{
		if (mCopySceneDataDic.Count == 0)
		{
			mCopySceneDataDic = DataReader.LoadTable<string, CopySceneData>("CopySceneData", "ID");
		}
		return DataReader.GetTableRow(mCopySceneDataDic, id);
	}

	public static List<CopySceneData> GetCopySceneDataByType(int type)
	{
		if (mCopySceneDataDic.Count == 0)
		{
			mCopySceneDataDic = DataReader.LoadTable<string, CopySceneData>("CopySceneData", "ID");
		}
		if (mCopySceneDataList.Count == 0)
		{
			mCopySceneDataList = new List<CopySceneData>(mCopySceneDataDic.Values);
		}
		List<CopySceneData> list = new List<CopySceneData>();
		for (int i = 0; i < mCopySceneDataList.Count; i++)
		{
			if (mCopySceneDataList[i].SubType == type)
			{
				list.Add(mCopySceneDataList[i]);
			}
		}
		list.Sort((CopySceneData x, CopySceneData y) => x.ID.CompareTo(y.ID));
		return list;
	}

	public static List<KeyValuePair<string, CopySceneData>> GetCopySceneDataList()
	{
		if (mCopySceneDataDic.Count == 0)
		{
			mCopySceneDataDic = DataReader.LoadTable<string, CopySceneData>("CopySceneData", "ID");
		}
		return new List<KeyValuePair<string, CopySceneData>>(mCopySceneDataDic);
	}

	public static LadderRewardData GetLadderRewardDataByRank(int rank)
	{
		if (mLadderRewardDataDic.Count == 0)
		{
			mLadderRewardDataDic = DataReader.LoadTable<string, LadderRewardData>("LadderRewardData", "ID");
		}
		if (mLadderRewardDataList.Count == 0)
		{
			mLadderRewardDataList = new List<LadderRewardData>(mLadderRewardDataDic.Values);
		}
		for (int i = 0; i < mLadderRewardDataList.Count; i++)
		{
			if (mLadderRewardDataList[i].Rankdown <= rank && rank <= mLadderRewardDataList[i].Rankup)
			{
				return mLadderRewardDataList[i];
			}
		}
		return null;
	}

	public static List<LadderRewardData> GetLadderRewardDataList()
	{
		if (mLadderRewardDataDic.Count == 0)
		{
			mLadderRewardDataDic = DataReader.LoadTable<string, LadderRewardData>("LadderRewardData", "ID");
		}
		if (mLadderRewardDataList.Count == 0)
		{
			mLadderRewardDataList = new List<LadderRewardData>(mLadderRewardDataDic.Values);
		}
		return mLadderRewardDataList;
	}

	public static BaseLvData GetLevelDataByLevel(int level)
	{
		if (mBaseLvDataDic.Count == 0)
		{
			mBaseLvDataDic = DataReader.LoadTable<int, BaseLvData>("BaseLvData", "Lv");
		}
		return DataReader.GetTableRow(mBaseLvDataDic, level);
	}

	public static TitleData GetTitleDateById(string id)
	{
		if (mTitleDateDic.Count == 0)
		{
			mTitleDateDic = DataReader.LoadTable<string, TitleData>("TitleData", "ID");
		}
		if (mTitleList.Count == 0)
		{
			mTitleList = new List<TitleData>(mTitleDateDic.Values);
		}
		return DataReader.GetTableRow(mTitleDateDic, id);
	}

	public static List<TitleData> GetTitleList()
	{
		if (mTitleDateDic.Count == 0)
		{
			mTitleDateDic = DataReader.LoadTable<string, TitleData>("TitleData", "ID");
		}
		if (mTitleList.Count == 0)
		{
			mTitleList = new List<TitleData>(mTitleDateDic.Values);
		}
		return mTitleList;
	}

	public static List<StoryData> GetStoryDataListById(string id)
	{
		if (mStoryDataDic.Count == 0)
		{
			mStoryDataDic = DataReader.LoadTableList<string, StoryData>("StoryData", "ID");
		}
		return DataReader.GetTableList(mStoryDataDic, id);
	}

	public static List<EquipmentUpgradeData> GetEquipmentUpgradeDataListByPartID(int pos)
	{
		if (mEquipmentUpgradeDataDic.Count == 0)
		{
			mEquipmentUpgradeDataDic = DataReader.LoadTableList<int, EquipmentUpgradeData>("EquipmentUpgradeData", "PartID");
		}
		return DataReader.GetTableList(mEquipmentUpgradeDataDic, pos);
	}

	public static DamageBoardTypeData GetDamageBoardTypeDataById(int id)
	{
		if (mDamageBoardTypeDataDic.Count == 0)
		{
			mDamageBoardTypeDataDic = DataReader.LoadTable<int, DamageBoardTypeData>("DamageBoardTypeData", "ID");
		}
		return DataReader.GetTableRow(mDamageBoardTypeDataDic, id);
	}

	public static List<CamRockCurveData> GetCamRockCurveDataListByName(string name)
	{
		if (mCamRockCurveDataDic.Count == 0)
		{
			mCamRockCurveDataDic = DataReader.LoadTableList<string, CamRockCurveData>("CamRockCurveData", "AnimaClipName");
		}
		return DataReader.GetTableList(mCamRockCurveDataDic, name);
	}

	public static CamRockData GetCamRockDataByID(string id)
	{
		if (mCamRockDataDic.Count == 0)
		{
			mCamRockDataDic = DataReader.LoadTable<string, CamRockData>("CamRockData", "ID");
		}
		return DataReader.GetTableRow(mCamRockDataDic, id);
	}

	public static List<SurveyMissionData> GetSurveyMissionDataBySceneId(string sceneId)
	{
		if (mSurveyMissionDataDic.Count == 0)
		{
			mSurveyMissionDataDic = DataReader.LoadTable<string, SurveyMissionData>("SurveyMissionData", "ID");
		}
		if (mSurveyMissionDataList.Count == 0)
		{
			mSurveyMissionDataList = new List<SurveyMissionData>(mSurveyMissionDataDic.Values);
		}
		List<SurveyMissionData> list = new List<SurveyMissionData>();
		for (int i = 0; i < mSurveyMissionDataList.Count; i++)
		{
			if (mSurveyMissionDataList[i].SceneID.Equals(sceneId))
			{
				list.Add(mSurveyMissionDataList[i]);
			}
		}
		return list;
	}

	public static SurveyMissionData GetSurveyMissionDataById(string id)
	{
		if (mSurveyMissionDataDic.Count == 0)
		{
			mSurveyMissionDataDic = DataReader.LoadTable<string, SurveyMissionData>("SurveyMissionData", "ID");
		}
		return DataReader.GetTableRow(mSurveyMissionDataDic, id);
	}

	public static List<MultiDeliveryMissionData> GetMultiDeliveryMissionDataListById(string id)
	{
		if (mMultiDeliveryMissionDataDic.Count == 0)
		{
			mMultiDeliveryMissionDataDic = DataReader.LoadTableList<string, MultiDeliveryMissionData>("MultiDeliveryMissionData", "ID");
		}
		return DataReader.GetTableList(mMultiDeliveryMissionDataDic, id);
	}

	public static DailyMissionData GetDailyMissionDataById(string id)
	{
		if (mDailyMissionDataDic.Count == 0)
		{
			mDailyMissionDataDic = DataReader.LoadTable<string, DailyMissionData>("DailyMissionData", "ID");
		}
		return DataReader.GetTableRow(mDailyMissionDataDic, id);
	}

	public static List<RefineData> GetRefineDataListByPart(int partId)
	{
		if (mRefineDataDic.Count == 0)
		{
			mRefineDataDic = DataReader.LoadTableList<int, RefineData>("RefineData", "Part");
		}
		return DataReader.GetTableList(mRefineDataDic, partId);
	}

	public static RefineData GetRefineDataByPartLevelPRO(int part, int level, int profession)
	{
		List<RefineData> refineDataListByPart = GetRefineDataListByPart(part);
		for (int i = 0; i < refineDataListByPart.Count; i++)
		{
			if (refineDataListByPart[i].Job == profession && refineDataListByPart[i].Lv == level)
			{
				return refineDataListByPart[i];
			}
		}
		return null;
	}

	public static RefineLevelData GetRefineLevelDataById(int id)
	{
		if (mRefineLevelDataDic.Count == 0)
		{
			mRefineLevelDataDic = DataReader.LoadTable<int, RefineLevelData>("RefineLevelData", "ID");
		}
		return DataReader.GetTableRow(mRefineLevelDataDic, id);
	}

	public static List<CarMissionBlockData> GetCarMissionBlockDataListBySceneId(string sceneid)
	{
		if (mCarMissionBlockDataDic.Count == 0)
		{
			mCarMissionBlockDataDic = DataReader.LoadTableList<string, CarMissionBlockData>("CarMissionBlockData", "SceneID");
		}
		return DataReader.GetTableList(mCarMissionBlockDataDic, sceneid);
	}

	public static List<ConsignBuyTabData> GetConsignBuyTabDataListById(int id)
	{
		if (mConsignBuyTabDataDic.Count == 0)
		{
			mConsignBuyTabDataDic = DataReader.LoadTableList<int, ConsignBuyTabData>("ConsignBuyTabData", "TopTabId");
		}
		return DataReader.GetTableList(mConsignBuyTabDataDic, id);
	}

	public static List<List<ConsignBuyTabData>> GetConsignBuyTabData()
	{
		if (mConsignBuyTabDataDic.Count == 0)
		{
			mConsignBuyTabDataDic = DataReader.LoadTableList<int, ConsignBuyTabData>("ConsignBuyTabData", "TopTabId");
		}
		List<List<ConsignBuyTabData>> list = new List<List<ConsignBuyTabData>>();
		foreach (List<ConsignBuyTabData> value in mConsignBuyTabDataDic.Values)
		{
			list.Add(value);
		}
		return list;
	}

	public static List<SneakingMissionData> GetSneakingMissionDataListByMapId(string mapId)
	{
		if (mSneakingMissionDataDic.Count == 0)
		{
			mSneakingMissionDataDic = DataReader.LoadTableList<string, SneakingMissionData>("SneakingMissionData", "MapID");
		}
		return DataReader.GetTableList(mSneakingMissionDataDic, mapId);
	}

	public static BadgeData GetBadgeDataById(string id)
	{
		if (mBadgeDataDic.Count == 0)
		{
			mBadgeDataDic = DataReader.LoadTable<string, BadgeData>("BadgeData", "ID");
		}
		return DataReader.GetTableRow(mBadgeDataDic, id);
	}

	public static void GetBadgeTypeByColor(int color, List<int> typeList, List<string> nameList)
	{
		if (BadgeDataList == null)
		{
			BadgeDataList = new List<BadgeData>(mBadgeDataDic.Values);
		}
		for (int i = 0; i < BadgeDataList.Count; i++)
		{
			if (BadgeDataList[i].Color == color && !typeList.Contains(BadgeDataList[i].BadgeType))
			{
				typeList.Add(BadgeDataList[i].BadgeType);
				nameList.Add(GetItemDataByID(BadgeDataList[i].ID).Name);
			}
		}
	}

	public static TowerData GetTowerDataByFloorID(int id)
	{
		if (mTowerDataDic.Count == 0)
		{
			mTowerDataDic = DataReader.LoadTable<int, TowerData>("TowerData", "FloorID");
		}
		return DataReader.GetTableRow(mTowerDataDic, id);
	}

	public static AIData GetAIDataByID(string id)
	{
		if (mAIDataDic.Count == 0)
		{
			mAIDataDic = DataReader.LoadTable<string, AIData>("AIData", "ID");
		}
		return DataReader.GetTableRow(mAIDataDic, id);
	}

	public static WildBossData GetWildBossDataByID(string id)
	{
		if (mWildBossDataDic.Count == 0)
		{
			mWildBossDataDic = DataReader.LoadTable<string, WildBossData>("WildBossData", "ID");
		}
		return DataReader.GetTableRow(mWildBossDataDic, id);
	}

	public static GuildBossData GetGuildBossDataByID(string id)
	{
		if (mGuildBossDataDic.Count == 0)
		{
			mGuildBossDataDic = DataReader.LoadTable<string, GuildBossData>("GuildBossData", "ID");
		}
		return DataReader.GetTableRow(mGuildBossDataDic, id);
	}

	public static BarFightCopyData GetBarFightCopyDataByID(string id)
	{
		if (mBarFightCopyDataDic.Count == 0)
		{
			mBarFightCopyDataDic = DataReader.LoadTable<string, BarFightCopyData>("BarFightCopyData", "ID");
		}
		return DataReader.GetTableRow(mBarFightCopyDataDic, id);
	}

	public static GuildDonateData GetGuildDonateDataByID(string id)
	{
		if (mGuildDonateDataDic.Count == 0)
		{
			mGuildDonateDataDic = DataReader.LoadTable<string, GuildDonateData>("GuildDonateData", "ID");
		}
		return DataReader.GetTableRow(mGuildDonateDataDic, id);
	}

	public static List<GuildLevelData> GetGuildLevelDataList()
	{
		if (mGuildLevelDataList.Count == 0)
		{
			mGuildLevelDataList = DataReader.LoadImportData<GuildLevelData>("GuildLevelData");
		}
		return mGuildLevelDataList;
	}

	public static GuildLevelData GetGuildLevelDataByLevel(int level)
	{
		if (mGuildLevelDataList.Count == 0)
		{
			mGuildLevelDataList = DataReader.LoadImportData<GuildLevelData>("GuildLevelData");
		}
		if (mGuildLevelDataList.Count > level)
		{
			return mGuildLevelDataList[level];
		}
		return null;
	}

	public static List<GuildSkillData> GetGuildSkillDataListByType(int type)
	{
		if (mGuildSkillDataDic.Count == 0)
		{
			mGuildSkillDataDic = DataReader.LoadTableList<int, GuildSkillData>("GuildSkillData", "GuildSkillType");
		}
		return DataReader.GetTableList(mGuildSkillDataDic, type);
	}

	public static GuildSkillData GetGuildSkillDataByTypeLevel(int type, int level)
	{
		List<GuildSkillData> guildSkillDataListByType = GetGuildSkillDataListByType(type);
		if (level < guildSkillDataListByType.Count)
		{
			return guildSkillDataListByType[level];
		}
		return null;
	}

	public static TeamData GetTeamDataDataByID(string id)
	{
		if (mTeamDataDic.Count == 0)
		{
			mTeamDataDic = DataReader.LoadTable<string, TeamData>("TeamData", "ID");
		}
		return DataReader.GetTableRow(mTeamDataDic, id);
	}

	public static List<TeamData> GetTeamDataList()
	{
		if (mTeamDataDic.Count == 0)
		{
			mTeamDataDic = DataReader.LoadTable<string, TeamData>("TeamData", "ID");
		}
		return new List<TeamData>(mTeamDataDic.Values);
	}

	public static ShowRewardData GetShowRewardDataByID(string id)
	{
		if (mShowRewardDataDic.Count == 0)
		{
			mShowRewardDataDic = DataReader.LoadTable<string, ShowRewardData>("ShowRewardData", "ID");
		}
		return DataReader.GetTableRow(mShowRewardDataDic, id);
	}

	public static AdaptData GetAdaptDataByID(int id)
	{
		if (mAdaptDataDic.Count == 0)
		{
			mAdaptDataDic = DataReader.LoadDicTable<int, AdaptData>("AdaptData", "ID", "DorpKeyDic");
		}
		return DataReader.GetTableRow(mAdaptDataDic, id);
	}

	public static FunctionData GetFunctionDataById(string id)
	{
		if (mFunctionDataDic.Count == 0)
		{
			mFunctionDataDic = DataReader.LoadTable<string, FunctionData>("FunctionData", "ID");
		}
		return DataReader.GetTableRow(mFunctionDataDic, id);
	}

	public static List<FunctionData> GetFunctionDataList()
	{
		if (mFunctionDataDic.Count == 0)
		{
			mFunctionDataDic = DataReader.LoadTable<string, FunctionData>("FunctionData", "ID");
		}
		return new List<FunctionData>(mFunctionDataDic.Values);
	}

	public static SoundData GetSoundDataById(int id)
	{
		if (mSoundDataDic == null || mSoundDataDic.Count == 0)
		{
			mSoundDataDic = DataReader.LoadTable<int, SoundData>("SoundData", "Id");
		}
		return DataReader.GetTableRow(mSoundDataDic, id);
	}

	public static List<MapAreaInfoData> GetMapAreaInfoDataListById(string id)
	{
		if (mMapAreaInfoDataDic.Count == 0)
		{
			mMapAreaInfoDataDic = DataReader.LoadTableList<string, MapAreaInfoData>("MapAreaInfoData", "ID");
		}
		return DataReader.GetTableList(mMapAreaInfoDataDic, id);
	}

	public static EscortData GetEscortDataById(string id)
	{
		if (mEscortDataDic.Count == 0)
		{
			mEscortDataDic = DataReader.LoadTable<string, EscortData>("EscortData", "ID");
		}
		return DataReader.GetTableRow(mEscortDataDic, id);
	}

	public static CityDanceData GetCityDanceDataById(string id)
	{
		if (mCityDanceDataDic == null || mCityDanceDataDic.Count == 0)
		{
			mCityDanceDataDic = DataReader.LoadTable<string, CityDanceData>("CityDanceData", "ID");
		}
		return DataReader.GetTableRow(mCityDanceDataDic, id);
	}

	public static List<CityDanceData> GetCityDanceDataList()
	{
		if (mCityDanceDataDic.Count == 0)
		{
			mCityDanceDataDic = DataReader.LoadTable<string, CityDanceData>("CityDanceData", "ID");
		}
		return new List<CityDanceData>(mCityDanceDataDic.Values);
	}

	public static MountData GetMountDataById(string id)
	{
		if (mMountDataDic.Count == 0)
		{
			mMountDataDic = DataReader.LoadTable<string, MountData>("MountData", "ID");
		}
		return DataReader.GetTableRow(mMountDataDic, id);
	}

	public static ColorData GetColorDataById(string id)
	{
		if (mColorDataDic.Count == 0)
		{
			mColorDataDic = DataReader.LoadTable<string, ColorData>("ColorData", "ID");
		}
		return DataReader.GetTableRow(mColorDataDic, id);
	}

	public static DanceData GetDanceDataById(string id)
	{
		if (mDanceDataDic.Count == 0)
		{
			mDanceDataDic = DataReader.LoadTable<string, DanceData>("DanceData", "ID");
		}
		return DataReader.GetTableRow(mDanceDataDic, id);
	}

	public static SlotIconData GetSlotIconDataById(string id)
	{
		if (mSlotIconDataDic.Count == 0)
		{
			mSlotIconDataDic = DataReader.LoadTable<string, SlotIconData>("SlotIconData", "ID");
		}
		if (mSlotIconList.Count == 0)
		{
			mSlotIconList = new List<SlotIconData>(mSlotIconDataDic.Values);
		}
		return DataReader.GetTableRow(mSlotIconDataDic, id);
	}

	public static List<SlotIconData> GetSlotIconDataList()
	{
		if (mSlotIconDataDic.Count == 0)
		{
			mSlotIconDataDic = DataReader.LoadTable<string, SlotIconData>("SlotIconData", "ID");
		}
		if (mSlotIconList.Count == 0)
		{
			mSlotIconList = new List<SlotIconData>(mSlotIconDataDic.Values);
		}
		return mSlotIconList;
	}

	public static SlotAutoData GetSlotAutoDataById(string id)
	{
		if (mSlotAutoDataDic.Count == 0)
		{
			mSlotAutoDataDic = DataReader.LoadTable<string, SlotAutoData>("SlotAutoData", "ID");
		}
		if (mSlotAutoList.Count == 0)
		{
			mSlotAutoList = new List<SlotAutoData>(mSlotAutoDataDic.Values);
		}
		return DataReader.GetTableRow(mSlotAutoDataDic, id);
	}

	public static List<SlotAutoData> GetSlotAutoDataList()
	{
		if (mSlotAutoDataDic.Count == 0)
		{
			mSlotAutoDataDic = DataReader.LoadTable<string, SlotAutoData>("SlotAutoData", "ID");
		}
		if (mSlotAutoList.Count == 0)
		{
			mSlotAutoList = new List<SlotAutoData>(mSlotAutoDataDic.Values);
		}
		return mSlotAutoList;
	}

	public static SurviveBattleData GetSurviveBattleDataById(string id)
	{
		if (mSurviveBattleDataDic.Count == 0)
		{
			mSurviveBattleDataDic = DataReader.LoadTable<string, SurviveBattleData>("SurviveBattleData", "ID");
		}
		return DataReader.GetTableRow(mSurviveBattleDataDic, id);
	}

	public static ScuffleData GetScuffleDataById(string id)
	{
		if (mScuffleDataDic.Count == 0)
		{
			mScuffleDataDic = DataReader.LoadTable<string, ScuffleData>("ScuffleData", "ID");
		}
		return DataReader.GetTableRow(mScuffleDataDic, id);
	}

	public static SignInMonthData GetSignInMonthDataById(int id)
	{
		if (mSignInMonthDataDic.Count == 0)
		{
			mSignInMonthDataDic = DataReader.LoadTable<int, SignInMonthData>("SignInMonthData", "Day");
		}
		if (mSignInMonthDataList.Count == 0)
		{
			mSignInMonthDataList = new List<SignInMonthData>(mSignInMonthDataDic.Values);
			mSignInMonthDataList.Sort((SignInMonthData x, SignInMonthData y) => x.Day - y.Day);
		}
		return DataReader.GetTableRow(mSignInMonthDataDic, id);
	}

	public static List<SignInMonthData> GetSignInMonthDataList()
	{
		if (mSignInMonthDataDic.Count == 0)
		{
			mSignInMonthDataDic = DataReader.LoadTable<int, SignInMonthData>("SignInMonthData", "Day");
		}
		if (mSignInMonthDataList.Count == 0)
		{
			mSignInMonthDataList = new List<SignInMonthData>(mSignInMonthDataDic.Values);
			mSignInMonthDataList.Sort((SignInMonthData x, SignInMonthData y) => x.Day - y.Day);
		}
		return mSignInMonthDataList;
	}

	public static DailyBuyData GetDailyBuyDataBuyId(string id)
	{
		if (mDailyBuyDataDic.Count == 0)
		{
			mDailyBuyDataDic = DataReader.LoadTable<string, DailyBuyData>("DailyBuyData", "ID");
		}
		return DataReader.GetTableRow(mDailyBuyDataDic, id);
	}

	public static InvestData GetInvestDataBuyId(string id)
	{
		if (mInvestDataDic.Count == 0)
		{
			mInvestDataDic = DataReader.LoadTable<string, InvestData>("InvestData", "ID");
		}
		return DataReader.GetTableRow(mInvestDataDic, id);
	}

	public static LevelPackageData GetLevelPackageDataBuyId(string id)
	{
		if (mLevelPackageDataDic.Count == 0)
		{
			mLevelPackageDataDic = DataReader.LoadTable<string, LevelPackageData>("LevelPackageData", "ID");
		}
		return DataReader.GetTableRow(mLevelPackageDataDic, id);
	}

	public static List<LevelPackageData> GetLevelPackageDataList()
	{
		if (mLevelPackageDataDic.Count == 0)
		{
			mLevelPackageDataDic = DataReader.LoadTable<string, LevelPackageData>("LevelPackageData", "ID");
		}
		if (mLevelPackageDataList.Count == 0)
		{
			mLevelPackageDataList = new List<LevelPackageData>(mLevelPackageDataDic.Values);
		}
		return mLevelPackageDataList;
	}

	public static RetrieveData GetRetrieveDataBuyId(string id)
	{
		if (mRetrieveDataDic.Count == 0)
		{
			mRetrieveDataDic = DataReader.LoadTable<string, RetrieveData>("RetrieveData", "ID");
		}
		return DataReader.GetTableRow(mRetrieveDataDic, id);
	}

	public static List<SignInWeekData> GetSignInWeekDataList()
	{
		if (mSignInWeekDataDic.Count == 0)
		{
			mSignInWeekDataDic = DataReader.LoadTable<int, SignInWeekData>("SignInWeekData", "Day");
		}
		if (mSignInWeekDataList.Count == 0)
		{
			mSignInWeekDataList = new List<SignInWeekData>(mSignInWeekDataDic.Values);
			mSignInWeekDataList.Sort((SignInWeekData x, SignInWeekData y) => x.Day - y.Day);
		}
		return mSignInWeekDataList;
	}

	public static DailyActiveData GetDailyActiveDataById(string id)
	{
		if (mDailyActiveDataDic.Count == 0)
		{
			mDailyActiveDataDic = DataReader.LoadTable<string, DailyActiveData>("DailyActiveData", "ID");
		}
		return DataReader.GetTableRow(mDailyActiveDataDic, id);
	}

	public static List<DailyActiveData> GetDailyActiveDataList()
	{
		if (mDailyActiveDataDic.Count == 0)
		{
			mDailyActiveDataDic = DataReader.LoadTable<string, DailyActiveData>("DailyActiveData", "ID");
		}
		if (mDailyActiveDataList.Count == 0)
		{
			mDailyActiveDataList = new List<DailyActiveData>(mDailyActiveDataDic.Values);
			mDailyActiveDataList.Sort((DailyActiveData x, DailyActiveData y) => int.Parse(x.ID) - int.Parse(y.ID));
		}
		return mDailyActiveDataList;
	}

	public static DailyActiveRewardData GetDailyActiveRewardDataById(string id)
	{
		if (mDailyActiveRewardDataDic.Count == 0)
		{
			mDailyActiveRewardDataDic = DataReader.LoadTable<string, DailyActiveRewardData>("DailyActiveRewardData", "ID");
		}
		return DataReader.GetTableRow(mDailyActiveRewardDataDic, id);
	}

	public static List<DailyActiveRewardData> GetDailyActiveRewardDataList()
	{
		if (mDailyActiveRewardDataDic.Count == 0)
		{
			mDailyActiveRewardDataDic = DataReader.LoadTable<string, DailyActiveRewardData>("DailyActiveRewardData", "ID");
		}
		if (mDailyActiveRewardDataList.Count == 0)
		{
			mDailyActiveRewardDataList = new List<DailyActiveRewardData>(mDailyActiveRewardDataDic.Values);
			mDailyActiveRewardDataList.Sort((DailyActiveRewardData x, DailyActiveRewardData y) => int.Parse(x.ID) - int.Parse(y.ID));
		}
		return mDailyActiveRewardDataList;
	}

	public static FirstBuyData GetFirstBuyDataById(string id)
	{
		if (mFirstBuyDataDic.Count == 0)
		{
			mFirstBuyDataDic = DataReader.LoadTable<string, FirstBuyData>("FirstBuyData", "ID");
		}
		return DataReader.GetTableRow(mFirstBuyDataDic, id);
	}

	public static BigPackageData GetBigPackageDataById(string id)
	{
		if (mBigPackageDataDic.Count == 0)
		{
			mBigPackageDataDic = DataReader.LoadTable<string, BigPackageData>("BigPackageData", "ID");
		}
		return DataReader.GetTableRow(mBigPackageDataDic, id);
	}

	public static List<BigPackageTimeListData> GetBigPackageTimeListDataListById(string[] ids)
	{
		if (mBigPackageTimeListDataDic.Count == 0)
		{
			mBigPackageTimeListDataDic = DataReader.LoadTable<string, BigPackageTimeListData>("BigPackageTimeListData", "ID");
		}
		List<BigPackageTimeListData> list = new List<BigPackageTimeListData>();
		for (int i = 0; i < ids.Length; i++)
		{
			if (mBigPackageTimeListDataDic.ContainsKey(ids[i]))
			{
				list.Add(mBigPackageTimeListDataDic[ids[i]]);
			}
		}
		return list;
	}

	public static PurchaseData GetPurchaseDataBuyId(string id)
	{
		if (mPurchaseDataDic.Count == 0)
		{
			mPurchaseDataDic = DataReader.LoadTable<string, PurchaseData>("PurchaseData", "ProductId");
		}
		return DataReader.GetTableRow(mPurchaseDataDic, id);
	}

	public static Dictionary<string, PurchaseData> GetPurchaseData()
	{
		if (mPurchaseDataDic.Count == 0)
		{
			mPurchaseDataDic = DataReader.LoadTable<string, PurchaseData>("PurchaseData", "ProductId");
		}
		return mPurchaseDataDic;
	}

	public static List<ModelPartData> GetModelPartDataList()
	{
		if (mModelPartDataDic.Count == 0)
		{
			mModelPartDataDic = DataReader.LoadTable<string, ModelPartData>("ModelPartData", "ID");
		}
		if (mModelPartDataList.Count == 0)
		{
			mModelPartDataList = new List<ModelPartData>(mModelPartDataDic.Values);
		}
		return mModelPartDataList;
	}

	public static DownloadRewardData GetDownloadRewardDataBuyId(string id)
	{
		if (mDownloadRewardDataDic.Count == 0)
		{
			mDownloadRewardDataDic = DataReader.LoadTable<string, DownloadRewardData>("DownloadRewardData", "ID");
		}
		return DataReader.GetTableRow(mDownloadRewardDataDic, id);
	}

	public static ShowModelData GetShowModelDataById(string id)
	{
		if (mShowModelDataDic.Count == 0)
		{
			mShowModelDataDic = DataReader.LoadTable<string, ShowModelData>("ShowModelData", "ID");
		}
		return DataReader.GetTableRow(mShowModelDataDic, id);
	}

	public static SocialDanceData GetSocialDanceDataById(string id)
	{
		if (mSocialDanceDataDic.Count == 0)
		{
			mSocialDanceDataDic = DataReader.LoadTable<string, SocialDanceData>("SocialDanceData", "ID");
		}
		return DataReader.GetTableRow(mSocialDanceDataDic, id);
	}

	public static Dictionary<string, SocialDanceData> GetAllSocialDanceData()
	{
		if (mSocialDanceDataDic.Count == 0)
		{
			mSocialDanceDataDic = DataReader.LoadTable<string, SocialDanceData>("SocialDanceData", "ID");
		}
		return mSocialDanceDataDic;
	}

	public static List<AnnounceData> GetAnnounceDataList()
	{
		if (mAnnounceDataList.Count == 0)
		{
			mAnnounceDataList = DataReader.LoadImportData<AnnounceData>("AnnounceData");
		}
		return mAnnounceDataList;
	}

	public static SexMiniData GetSexMiniDataById(string id)
	{
		if (mSexMiniDataDic == null || mSexMiniDataDic.Count == 0)
		{
			mSexMiniDataDic = DataReader.LoadTable<string, SexMiniData>("SexMiniData", "ID");
		}
		return DataReader.GetTableRow(mSexMiniDataDic, id);
	}

	public static ShopData GetShopDataByID(string id)
	{
		if (mShopDataDic.Count == 0)
		{
			mShopDataDic = DataReader.LoadTable<string, ShopData>("ShopData", "ID");
		}
		return DataReader.GetTableRow(mShopDataDic, id);
	}

	public static List<ShopData> GetShopDataList()
	{
		if (mShopDataDic.Count == 0)
		{
			mShopDataDic = DataReader.LoadTable<string, ShopData>("ShopData", "ID");
		}
		if (mShopDataList.Count == 0)
		{
			mShopDataList = new List<ShopData>(mShopDataDic.Values);
		}
		return mShopDataList;
	}

	public static ShopData GetPlayerShopWeapon()
	{
		if (mShopDataDic.Count == 0)
		{
			mShopDataDic = DataReader.LoadTable<string, ShopData>("ShopData", "ID");
		}
		if (mShopDataList.Count == 0)
		{
			mShopDataList = new List<ShopData>(mShopDataDic.Values);
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		List<ShopData> list = new List<ShopData>();
		for (int i = 0; i < mShopDataList.Count; i++)
		{
			if (mShopDataList[i].ProfessionType == (int)playerData.Profession && mShopDataList[i].Class == 6)
			{
				ItemData itemDataByID = GetItemDataByID(mShopDataList[i].ItemID);
				if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP && itemDataByID.SubType == 0)
				{
					list.Add(mShopDataList[i]);
				}
			}
		}
		list.Sort(delegate(ShopData x, ShopData y)
		{
			ItemData itemDataByID3 = GetItemDataByID(x.ItemID);
			ItemData itemDataByID4 = GetItemDataByID(y.ItemID);
			return itemDataByID4.Level - itemDataByID3.Level;
		});
		for (int j = 0; j < list.Count; j++)
		{
			ItemData itemDataByID2 = GetItemDataByID(list[j].ItemID);
			if (playerData.CheckLevel(itemDataByID2.Level))
			{
				return list[j];
			}
		}
		return null;
	}

	public static List<StrongerData> GetStrongerDataList()
	{
		if (mStrongerDataDic.Count == 0)
		{
			mStrongerDataDic = DataReader.LoadTable<string, StrongerData>("StrongerData", "ID");
		}
		if (mStrongerDataList.Count == 0)
		{
			mStrongerDataList = new List<StrongerData>(mStrongerDataDic.Values);
		}
		return mStrongerDataList;
	}

	public static List<LoadingUIData> GetLoadingUIDataList()
	{
		if (mLoadingUIDataDic.Count == 0)
		{
			mLoadingUIDataDic = DataReader.LoadTable<string, LoadingUIData>("LoadingUIData", "ID");
		}
		if (mLoadingUIDataList.Count == 0)
		{
			mLoadingUIDataList = new List<LoadingUIData>(mLoadingUIDataDic.Values);
			mLoadingUIDataList.Sort((LoadingUIData x, LoadingUIData y) => (x.ID.Length != y.ID.Length) ? (x.ID.Length - y.ID.Length) : x.ID.CompareTo(y.ID));
		}
		return mLoadingUIDataList;
	}

	public static List<TimerActivityTipsData> GetTimerActivityTipsDataList()
	{
		if (mTimerActivityTipsDataDic == null || mTimerActivityTipsDataDic.Count == 0)
		{
			mTimerActivityTipsDataDic = DataReader.LoadTable<string, TimerActivityTipsData>("TimerActivityTipsData", "ID");
		}
		mTimerActivityTipsDataList.Clear();
		if (mTimerActivityTipsDataDic != null && mTimerActivityTipsDataList.Count == 0)
		{
			mTimerActivityTipsDataList = new List<TimerActivityTipsData>(mTimerActivityTipsDataDic.Values);
			for (int num = mTimerActivityTipsDataList.Count - 1; num >= 0; num--)
			{
				if (!TimeTools.IsTimeRange(mTimerActivityTipsDataList[num].Starttimes, mTimerActivityTipsDataList[num].EndTimes))
				{
					mTimerActivityTipsDataList.RemoveAt(num);
				}
			}
			mTimerActivityTipsDataList.Sort((TimerActivityTipsData x, TimerActivityTipsData y) => x.Weight - y.Weight);
		}
		return mTimerActivityTipsDataList;
	}

	public static TimerActivityData GetTimerActivityDataById(string id)
	{
		if (mTimerActivityDataDic == null || mTimerActivityDataDic.Count == 0)
		{
			mTimerActivityDataDic = DataReader.LoadTable<string, TimerActivityData>("TimerActivityData", "ID");
		}
		return DataReader.GetTableRow(mTimerActivityDataDic, id);
	}

	public static List<TimerActivityData> GetEnableTimerActivityList()
	{
		if (mTimerActivityDataDic == null || mTimerActivityDataDic.Count == 0)
		{
			mTimerActivityDataDic = DataReader.LoadTable<string, TimerActivityData>("TimerActivityData", "ID");
		}
		if (mTimerActivityDataDic != null)
		{
			List<TimerActivityData> list = new List<TimerActivityData>(mTimerActivityDataDic.Values);
			for (int num = list.Count - 1; num >= 0; num--)
			{
				if (!TimeTools.IsTimeRange(list[num].StartTimeList, list[num].EndTimeList))
				{
					list.RemoveAt(num);
				}
			}
			return list;
		}
		return null;
	}

	public static ActivityBossData GetActivityBossDataById(string id)
	{
		if (mActivityBossDataDic == null || mActivityBossDataDic.Count == 0)
		{
			mActivityBossDataDic = DataReader.LoadTable<string, ActivityBossData>("ActivityBossData", "Key");
		}
		return DataReader.GetTableRow(mActivityBossDataDic, id);
	}

	public static ActivityBossData GetActivityBossDataByActivityIdMapId(string activityId, string mapId)
	{
		if (mActivityBossDataDic == null || mActivityBossDataDic.Count == 0)
		{
			mActivityBossDataDic = DataReader.LoadTable<string, ActivityBossData>("ActivityBossData", "Key");
		}
		if (mActivityBossDataDic != null)
		{
			List<ActivityBossData> list = new List<ActivityBossData>(mActivityBossDataDic.Values);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].ID.Equals(activityId) && list[i].MapID.Equals(mapId))
				{
					return list[i];
				}
			}
		}
		return null;
	}

	public static List<NotifyData> GetNotifyDataList()
	{
		if (mNotifyDataDic == null || mNotifyDataDic.Count == 0)
		{
			mNotifyDataDic = DataReader.LoadTable<string, NotifyData>("NotifyData", "ID");
		}
		mNotifyDataList.Clear();
		if (mNotifyDataDic != null && mNotifyDataList.Count == 0)
		{
			mNotifyDataList = new List<NotifyData>(mNotifyDataDic.Values);
		}
		return mNotifyDataList;
	}

	public static LevelRewardData GetLevelRewardDataByID(string id)
	{
		if (mLevelRewardDataDic.Count == 0)
		{
			mLevelRewardDataDic = DataReader.LoadTable<string, LevelRewardData>("LevelRewardData", "ID");
		}
		return DataReader.GetTableRow(mLevelRewardDataDic, id);
	}

	public static List<LevelRewardData> GetLevelRewardDataList()
	{
		if (mLevelRewardDataDic.Count == 0)
		{
			mLevelRewardDataDic = DataReader.LoadTable<string, LevelRewardData>("LevelRewardData", "ID");
		}
		if (mLevelRewardDataDic != null && mLevelRewardDataList.Count == 0)
		{
			mLevelRewardDataList = new List<LevelRewardData>(mLevelRewardDataDic.Values);
		}
		return mLevelRewardDataList;
	}

	public static GuildBattleData GetGuildBattleDataById(string id)
	{
		if (mGuildBattleDataDic == null || mGuildBattleDataDic.Count == 0)
		{
			mGuildBattleDataDic = DataReader.LoadTable<string, GuildBattleData>("GuildBattleData", "ID");
		}
		return DataReader.GetTableRow(mGuildBattleDataDic, id);
	}

	public static ConfigData GetConfigDataByKey(string key)
	{
		if (mConfigDataDic == null || mConfigDataDic.Count == 0)
		{
			mConfigDataDic = DataReader.LoadTable<string, ConfigData>("ConfigData", "Key");
		}
		return DataReader.GetTableRow(mConfigDataDic, key);
	}

	public static List<ConfigData> GetConfigStarScoreList()
	{
		mConfigDataStarScoreList.Clear();
		for (int i = 0; i < GameDefine.EquipStarScoreName.Length; i++)
		{
			mConfigDataStarScoreList.Add(GetConfigDataByKey(GameDefine.EquipStarScoreName[i]));
		}
		return mConfigDataStarScoreList;
	}

	public static List<ConfigData> GetConfigQualityScoreList()
	{
		mConfigDataQualityScoreList.Clear();
		for (int i = 0; i < GameDefine.EquipQualityScoreName.Length; i++)
		{
			mConfigDataQualityScoreList.Add(GetConfigDataByKey(GameDefine.EquipQualityScoreName[i]));
		}
		return mConfigDataQualityScoreList;
	}

	public static GuildStarData GetGuildStarDataById(string id)
	{
		if (mGuildStarDataDic == null || mGuildStarDataDic.Count == 0)
		{
			mGuildStarDataDic = DataReader.LoadTable<string, GuildStarData>("GuildStarData", "ID");
		}
		return DataReader.GetTableRow(mGuildStarDataDic, id);
	}

	public static List<GuildStarData> GetGuildStarDataListByMapID(int mapid)
	{
		if (mGuildStarDataDic == null || mGuildStarDataDic.Count == 0)
		{
			mGuildStarDataDic = DataReader.LoadTable<string, GuildStarData>("GuildStarData", "ID");
		}
		mGuildStarDataList.Clear();
		if (mGuildStarDataDic != null && mGuildStarDataList.Count == 0)
		{
			mGuildStarDataList = new List<GuildStarData>(mGuildStarDataDic.Values);
		}
		List<GuildStarData> list = new List<GuildStarData>();
		for (int i = 0; i < mGuildStarDataList.Count; i++)
		{
			if (mGuildStarDataList[i].MapID == mapid)
			{
				list.Add(mGuildStarDataList[i]);
			}
		}
		return list;
	}

	public static ActivityMapData GetActivityMapDataById(string id)
	{
		if (mActivityMapDataDic == null || mActivityMapDataDic.Count == 0)
		{
			mActivityMapDataDic = DataReader.LoadTable<string, ActivityMapData>("ActivityMapData", "ID");
		}
		return DataReader.GetTableRow(mActivityMapDataDic, id);
	}

	public static ActivityMapData GetActivityMapDataByActID(int type, string actid)
	{
		if (mActivityMapDataDic == null || mActivityMapDataDic.Count == 0)
		{
			mActivityMapDataDic = DataReader.LoadTable<string, ActivityMapData>("ActivityMapData", "ID");
		}
		List<ActivityMapData> list = new List<ActivityMapData>(mActivityMapDataDic.Values);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].ActivityID.Equals(actid) && list[i].Type == type)
			{
				return list[i];
			}
		}
		return null;
	}

	public static List<ActivityMapData> GetAcitvityMapDataByMapId(string mapId)
	{
		if (mActivityMapDataDic == null || mActivityMapDataDic.Count == 0)
		{
			mActivityMapDataDic = DataReader.LoadTable<string, ActivityMapData>("ActivityMapData", "ID");
		}
		if (mActivityMapDataDic == null)
		{
			return new List<ActivityMapData>();
		}
		if (mActivityMapDataByMapIdDic.Count == 0)
		{
			List<ActivityMapData> list = new List<ActivityMapData>(mActivityMapDataDic.Values);
			for (int i = 0; i < list.Count; i++)
			{
				if (mActivityMapDataByMapIdDic.ContainsKey(list[i].MapId))
				{
					mActivityMapDataByMapIdDic[list[i].MapId].Add(list[i]);
					continue;
				}
				mActivityMapDataByMapIdDic.Add(list[i].MapId, new List<ActivityMapData>());
				mActivityMapDataByMapIdDic[list[i].MapId].Add(list[i]);
			}
		}
		if (mActivityMapDataByMapIdDic.ContainsKey(mapId))
		{
			return mActivityMapDataByMapIdDic[mapId];
		}
		return new List<ActivityMapData>();
	}

	private static void InitActivityMapDataByTypeDic()
	{
		if (mActivityMapDataDic == null || mActivityMapDataDic.Count == 0 || mActivityMapDataByTypeDic.Count != 0)
		{
			return;
		}
		List<ActivityMapData> list = new List<ActivityMapData>(mActivityMapDataDic.Values);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < list.Count; i++)
		{
			num = list[i].Type;
			num2 = list[i].SubType;
			if (mActivityMapDataByTypeDic.ContainsKey(num))
			{
				if (!mActivityMapDataByTypeDic[num].ContainsKey(num2))
				{
					mActivityMapDataByTypeDic[num].Add(num2, new List<ActivityMapData>());
				}
				mActivityMapDataByTypeDic[num][num2].Add(list[i]);
			}
			else
			{
				mActivityMapDataByTypeDic.Add(num, new Dictionary<int, List<ActivityMapData>>());
				mActivityMapDataByTypeDic[num].Add(num2, new List<ActivityMapData>());
				mActivityMapDataByTypeDic[num][num2].Add(list[i]);
			}
		}
	}

	public static List<ActivityMapData> GetActivityMapDataByType(int type, int subType)
	{
		InitActivityMapDataByTypeDic();
		if (mActivityMapDataByTypeDic.ContainsKey(type) && mActivityMapDataByTypeDic[type].ContainsKey(subType))
		{
			return mActivityMapDataByTypeDic[type][subType];
		}
		return null;
	}

	public static MonthlyCardData GetMonthlyCardDataById(string id)
	{
		if (mMonthlyCardDataDic == null || mMonthlyCardDataDic.Count == 0)
		{
			mMonthlyCardDataDic = DataReader.LoadTable<string, MonthlyCardData>("MonthlyCardData", "ID");
		}
		return DataReader.GetTableRow(mMonthlyCardDataDic, id);
	}

	public static MoveTargetMissionData GetMoveTargetMissionDataById(string id)
	{
		if (mMoveTargetMissionDataDic == null || mMoveTargetMissionDataDic.Count == 0)
		{
			mMoveTargetMissionDataDic = DataReader.LoadTable<string, MoveTargetMissionData>("MoveTargetMissionData", "ID");
		}
		return DataReader.GetTableRow(mMoveTargetMissionDataDic, id);
	}

	public static PoliceLevelData GetPoliceLevelDataById(string id)
	{
		if (mPoliceLevelDataDic == null || mPoliceLevelDataDic.Count == 0)
		{
			mPoliceLevelDataDic = DataReader.LoadTable<string, PoliceLevelData>("PoliceLevelData", "ID");
		}
		return DataReader.GetTableRow(mPoliceLevelDataDic, id);
	}

	public static List<QualityData> GetQualityDataListByID(string id)
	{
		if (mQualityDataDic.Count == 0)
		{
			mQualityDataDic = DataReader.LoadTableList<string, QualityData>("QualityData", "ID");
		}
		return DataReader.GetTableList(mQualityDataDic, id);
	}

	public static SingleMapLockData GetSingleMapLockDataById(int id)
	{
		if (mSingleMapLockDataDic == null || mSingleMapLockDataDic.Count == 0)
		{
			mSingleMapLockDataDic = DataReader.LoadTable<int, SingleMapLockData>("SingleMapLockData", "ID");
		}
		return DataReader.GetTableRow(mSingleMapLockDataDic, id);
	}

	public static KillTargetMissionData GetKillTargetMissionDataById(string id)
	{
		if (mKillTargetMissionDataDic == null || mKillTargetMissionDataDic.Count == 0)
		{
			mKillTargetMissionDataDic = DataReader.LoadTable<string, KillTargetMissionData>("KillTargetMissionData", "ID");
		}
		return DataReader.GetTableRow(mKillTargetMissionDataDic, id);
	}

	public static TargetCarMissionData GetTargetCarMissionDataById(string id)
	{
		if (mTargetCarMissionDataDic == null || mTargetCarMissionDataDic.Count == 0)
		{
			mTargetCarMissionDataDic = DataReader.LoadTable<string, TargetCarMissionData>("TargetCarMissionData", "ID");
		}
		return DataReader.GetTableRow(mTargetCarMissionDataDic, id);
	}

	public static DailyExpData GetDailyExpDataById(string id)
	{
		if (mDailyExpDataDic == null || mDailyExpDataDic.Count == 0)
		{
			mDailyExpDataDic = DataReader.LoadTable<string, DailyExpData>("DailyExpData", "ID");
		}
		return DataReader.GetTableRow(mDailyExpDataDic, id);
	}

	public static OnlineMissionData GetOnlineMissionDataByID(string id)
	{
		if (mOnlineMissionDataDict.Count == 0)
		{
			mOnlineMissionDataDict = DataReader.LoadTable<string, OnlineMissionData>("OnlineMissionData", "ID");
		}
		return DataReader.GetTableRow(mOnlineMissionDataDict, id);
	}

	public static DominData GetDominDataByID(string id)
	{
		if (mDominDataDict.Count == 0)
		{
			mDominDataDict = DataReader.LoadTable<string, DominData>("DominData", "ID");
		}
		return DataReader.GetTableRow(mDominDataDict, id);
	}

	public static LevelSealData GetLevelSealDataByID(string id)
	{
		if (mLevelSealDataDict == null || mLevelSealDataDict.Count == 0)
		{
			mLevelSealDataDict = DataReader.LoadTable<string, LevelSealData>("LevelSealData", "ID");
		}
		return DataReader.GetTableRow(mLevelSealDataDict, id);
	}

	public static TimeLimitMissionData GetTimeLimitMissionDataByID(string id)
	{
		if (mTimeLimitMissionDataDict == null || mTimeLimitMissionDataDict.Count == 0)
		{
			mTimeLimitMissionDataDict = DataReader.LoadTable<string, TimeLimitMissionData>("TimeLimitMissionData", "ID");
		}
		return DataReader.GetTableRow(mTimeLimitMissionDataDict, id);
	}

	public static SkillLabelData GetSkillLabelDataByID(string id)
	{
		if (mSkillLabelDataDict == null || mSkillLabelDataDict.Count == 0)
		{
			mSkillLabelDataDict = DataReader.LoadTable<string, SkillLabelData>("SkillLabelData", "ID");
		}
		return DataReader.GetTableRow(mSkillLabelDataDict, id);
	}

	public static GuildCaptureData GetGuildCaptureDataByID(string id)
	{
		if (mGuildCaptureDataDict == null || mGuildCaptureDataDict.Count == 0)
		{
			mGuildCaptureDataDict = DataReader.LoadTable<string, GuildCaptureData>("GuildCaptureData", "ID");
		}
		return DataReader.GetTableRow(mGuildCaptureDataDict, id);
	}
}
