using System;
using System.Collections.Generic;
using DG.Tweening;
using SprotoType;
using UnityEngine;

public class ObjManager : Singleton<ObjManager>
{
	public delegate void OnGetNPC(ObjNPC npc);

	private ObjMainPlayer mMainPlayer;

	private ObjPlayerCar mMainPlayerCar;

	private Dictionary<long, Obj> mObjDict = new Dictionary<long, Obj>();

	private Dictionary<string, GameObject> mOtherObjDic = new Dictionary<string, GameObject>();

	private List<ObjNPC> mOtherPlayerEscortNPCList = new List<ObjNPC>();

	private ObjNpcPoolGroup mObjNpcPoolGroup;

	private ObjOtherPlayerPool mOtherPlayerPool;

	private List<ObjCharacter>[] mCampList = new List<ObjCharacter>[7];

	private List<ObjCharacter>[] mCampTargetList = new List<ObjCharacter>[7];

	private List<ObjNPC> mMapShowObjList = new List<ObjNPC>();

	private List<int> mTargetCampList = new List<int>();

	private Dictionary<long, ObjInitPlayerData> mOtherPlayerNoLogicDataDic = new Dictionary<long, ObjInitPlayerData>();

	private List<ObjOtherPlayer> mObjOtherPlayerVisibleList = new List<ObjOtherPlayer>();

	private List<ObjOtherPlayer> mObjOtherPlayerInVisibleList = new List<ObjOtherPlayer>();

	private Dictionary<long, int> mPlayerMeshLoadNumDic = new Dictionary<long, int>();

	private Dictionary<long, int> mPlayerMeshLoadTargetNumDic = new Dictionary<long, int>();

	private SimplePoolGroup<ObjFakeAICar> mObjFakeAICarPoolGroup;

	private int tt;

	private SimplePoolGroup<ObjSimpleAICar> mObjSimpleAICarPoolGroup;

	private int mPoliceSoundId = 35;

	private SimplePoolGroup<ObjRagdollNPC> mObjRagdollNPCPoolGroup;

	private SimplePoolGroup<ObjPatrolNPC> mObjPatrolNPCPoolGroup;

	private SimplePool<BombSustainedRangeObj> mBombSustainedRangeObjPool;

	private SimplePoolGroup<ObjPlayerMountCar> mMountCarPoolGroup;

	public ObjMainPlayer MainPlayer => mMainPlayer;

	public ObjPlayerCar MainPlayerCar => mMainPlayerCar;

	public Dictionary<long, Obj> ObjDict
	{
		get
		{
			return mObjDict;
		}
		set
		{
			mObjDict = value;
		}
	}

	public Dictionary<string, GameObject> OtherObjDic
	{
		get
		{
			return mOtherObjDic;
		}
		set
		{
			mOtherObjDic = value;
		}
	}

	public List<ObjNPC> OtherPlayerEscortNPCList => mOtherPlayerEscortNPCList;

	public ObjOtherPlayerPool OtherPlayerPool => mOtherPlayerPool;

	public List<ObjCharacter>[] CampList => mCampList;

	public List<ObjCharacter>[] CampTargetList => mCampTargetList;

	public List<ObjNPC> MapShowObjList => mMapShowObjList;

	public Dictionary<long, ObjInitPlayerData> OtherPlayerNoLogicDataDic => mOtherPlayerNoLogicDataDic;

	public List<ObjOtherPlayer> ObjOtherPlayerVisibleList => mObjOtherPlayerVisibleList;

	public List<ObjOtherPlayer> ObjOtherPlayerInVisibleList => mObjOtherPlayerInVisibleList;

	public List<ObjFakeAICar> EnableFakeAICarList => mObjFakeAICarPoolGroup.EnableList;

	public List<ObjRagdollNPC> EnableRagdollNpcList => mObjRagdollNPCPoolGroup.EnableList;

	public ObjManager()
	{
		ResetObjManager();
	}

	public ObjNPC GetNearestEscortNPC()
	{
		float num = float.MaxValue;
		ObjNPC result = null;
		float num2 = 0f;
		for (int i = 0; i < mOtherPlayerEscortNPCList.Count; i++)
		{
			num2 = Vector3.SqrMagnitude(mMainPlayer.Position - mOtherPlayerEscortNPCList[i].Position);
			if (num2 < num)
			{
				result = mOtherPlayerEscortNPCList[i];
				num = num2;
			}
		}
		return result;
	}

	public void AddMapShowObj(Obj obj)
	{
		ObjNPC objNPC = obj as ObjNPC;
		if (objNPC != null && objNPC.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC && !objNPC.IsSoundBoxNpc())
		{
			mMapShowObjList.Add(objNPC);
			UpdateMapNpc();
		}
	}

	public void RemoveShowObj(ObjCharacter objCha)
	{
		if (mMapShowObjList.Remove(objCha as ObjNPC))
		{
			UpdateMapNpc();
		}
	}

	private void UpdateMapNpc()
	{
		if (SingletonUnity<NewMapUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMapUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewMapUIRootLogic>.Instance.UpdateNpcPos();
		}
	}

	public void AddToTargetCampList(ObjCharacter objCha)
	{
		CampTool.GetBeAttackedList(objCha.AttributeData.Camp, mTargetCampList);
		if (objCha.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsCanAttackOtherPlayerScene() && !mTargetCampList.Contains(0))
		{
			mTargetCampList.Add(0);
		}
		for (int i = 0; i < mTargetCampList.Count; i++)
		{
			mCampTargetList[mTargetCampList[i]].Add(objCha);
		}
		mCampList[(int)objCha.AttributeData.Camp].Add(objCha);
	}

	public void RemoveFromTargetCampList(ObjCharacter objCha)
	{
		CampTool.GetBeAttackedList(objCha.AttributeData.Camp, mTargetCampList);
		if (objCha.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsCanAttackOtherPlayerScene() && !mTargetCampList.Contains(0))
		{
			mTargetCampList.Add(0);
		}
		for (int i = 0; i < mTargetCampList.Count; i++)
		{
			mCampTargetList[mTargetCampList[i]].Remove(objCha);
		}
		mCampList[(int)objCha.AttributeData.Camp].Remove(objCha);
	}

	public void ChangeCamp(ObjCharacter objCha, GameDefine.CAMP_TYPE preCamp, GameDefine.CAMP_TYPE targetCamp)
	{
		mCampList[(int)preCamp].Remove(objCha);
		mCampList[(int)targetCamp].Add(objCha);
		CampTool.GetBeAttackedList(preCamp, mTargetCampList);
		for (int i = 0; i < mTargetCampList.Count; i++)
		{
			mCampTargetList[mTargetCampList[i]].Remove(objCha);
		}
		CampTool.GetBeAttackedList(targetCamp, mTargetCampList);
		for (int j = 0; j < mTargetCampList.Count; j++)
		{
			mCampTargetList[mTargetCampList[j]].Add(objCha);
		}
	}

	public ObjInitPlayerData GetNoLogicOtherPlayerData(long serverId)
	{
		if (mOtherPlayerNoLogicDataDic.ContainsKey(serverId))
		{
			return mOtherPlayerNoLogicDataDic[serverId];
		}
		return null;
	}

	public bool RemoveNoLogicOtherPlayerData(long serverId)
	{
		if (mOtherPlayerNoLogicDataDic.ContainsKey(serverId))
		{
			mOtherPlayerNoLogicDataDic.Remove(serverId);
			return true;
		}
		return false;
	}

	public void ResetObjManager()
	{
		mObjNpcPoolGroup = new ObjNpcPoolGroup();
		mObjNpcPoolGroup.Reset(GameSettingData.MaxNPCPoolGroupNum[GameSettingData.GetPhoneClass()], GameSettingData.MaxNPCPoolNum[GameSettingData.GetPhoneClass()]);
		mObjNpcPoolGroup.RegisterOnNpcRecycle(OnNpcRecycle);
		mOtherPlayerPool = new ObjOtherPlayerPool();
		mOtherPlayerPool.SetPoolMaxNum(GameSettingData.MaxOtherPlayerPoolNum[GameSettingData.GetPhoneClass()]);
		ResetObjSimpleAICarPoolGroup();
		ResetObjRagdollNPCPoolGroup();
		ResetObjPatrolNPCPoolGroup();
		ResetBombSustainedRangeObjPool();
		ResetMountCarPoolGroup();
		ResetObjFakeAICarPoolGroup();
	}

	public void ResetPhoneClass()
	{
		RefreshPoolNum();
		ResetPlayerFashionEffect();
		ResetMainPlayerXRay();
		ResetPlayerShadow();
	}

	public void ResetMainPlayerXRay()
	{
		if (!(mMainPlayer == null))
		{
			if (GameSettingData.IsShowPlayerShadow[GameSettingData.GetPhoneClass()])
			{
				mMainPlayer.AddXRayMat();
			}
			else
			{
				mMainPlayer.RemoveXRayMat();
			}
		}
	}

	public void ResetPlayerShadow()
	{
		if (mMainPlayer == null)
		{
			return;
		}
		if (!GameSettingData.IsShowPlayerShadow[GameSettingData.GetPhoneClass()])
		{
			if (SingletonUnity<RealTimeShadow>.Exists)
			{
				SingletonUnity<RealTimeShadow>.Instance.DisableRealTimeShadow();
			}
			if (mMainPlayer.SimpleShadow == null)
			{
				mMainPlayer.InitSimpleShadow();
			}
			return;
		}
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < mMainPlayer.PartObject.Length; i++)
		{
			if (mMainPlayer.PartObject[i] != null)
			{
				list.Add(mMainPlayer.PartObject[i]);
			}
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.MapType != MAPTYPE.CAR_CHASE_COPY)
		{
			if (!SingletonUnity<RealTimeShadow>.Exists)
			{
				GameObject gameObject = ResourcesManager.LoadAndInstantiate("Items/PlayerRealTimeShadow") as GameObject;
				RealTimeShadow component = gameObject.GetComponent<RealTimeShadow>();
				component.Reset(list);
			}
			else
			{
				SingletonUnity<RealTimeShadow>.Instance.EnableRealTimeShadow();
				SingletonUnity<RealTimeShadow>.Instance.Reset(list);
			}
		}
		if (mMainPlayer.SimpleShadow != null)
		{
			ResourcesManager.UnLoadSimpleShadowPrefab(mMainPlayer.SimpleShadow.gameObject);
			mMainPlayer.SimpleShadow = null;
		}
	}

	public void ResetPlayerFashionEffect()
	{
		if (GameSettingData.IsShowFashionEffect[GameSettingData.GetPhoneClass()])
		{
			for (int i = 0; i < mObjOtherPlayerVisibleList.Count; i++)
			{
				mObjOtherPlayerVisibleList[i].ReshowPartEffect();
			}
			if (mMainPlayer != null)
			{
				mMainPlayer.ReshowPartEffect();
			}
		}
		else
		{
			for (int j = 0; j < mObjOtherPlayerVisibleList.Count; j++)
			{
				mObjOtherPlayerVisibleList[j].ClearPartEffect();
			}
			if (mMainPlayer != null)
			{
				mMainPlayer.ClearPartEffect();
			}
		}
	}

	public void ClearPlayerFashionEffect()
	{
	}

	public void ReshowPlayerFashionEffect()
	{
	}

	public void RefreshPoolNum()
	{
		mObjNpcPoolGroup.RefreshPool(GameSettingData.MaxNPCPoolGroupNum[GameSettingData.GetPhoneClass()], GameSettingData.MaxNPCPoolNum[GameSettingData.GetPhoneClass()]);
		mOtherPlayerPool.SetPoolMaxNum(GameSettingData.MaxOtherPlayerPoolNum[GameSettingData.GetPhoneClass()]);
	}

	~ObjManager()
	{
		mObjNpcPoolGroup.DeRegisterOnNpcRecycle(OnNpcRecycle);
	}

	public bool IsMainPlayerPart(string bundleKey)
	{
		if (SingletonDontDestoryUnity<GameManager>.Exists)
		{
			return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsInMainPlayerPartBundleIdList(bundleKey);
		}
		return false;
	}

	public void CreateMainPlayer(ObjInitPlayerData initData)
	{
		if (mMainPlayer != null)
		{
			return;
		}
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		initData.mPos = new Vector3(initData.mPos.x, SceneManager.GetHitHeight(initData.mPos), initData.mPos.z);
		PlayerData playerData = instance.PlayerData;
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/PlayerRoot") as GameObject;
		gameObject.name = "MainPlayer_" + initData.Camp;
		CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(initData.mCharacterModelId);
		if (characterModelDataByID != null)
		{
			string path = $"TestModel/Model/{initData.Profession}/ModelRoot";
			ReloadModel(gameObject.transform, path);
		}
		if (gameObject != null)
		{
			mMainPlayer = gameObject.GetComponent<ObjMainPlayer>();
			if (mMainPlayer == null)
			{
				mMainPlayer = gameObject.AddComponent<ObjMainPlayer>();
			}
		}
		mMainPlayer.UpdateWeaponModeID(initData.GetWeaponID());
		mMainPlayer.UpdateWeaponItemID(initData.WeaponItemId);
		mMainPlayer.InitInfo(characterModelDataByID);
		mMainPlayer.Init();
		mMainPlayer.ResetMainPlayer(initData);
		if (playerData.IsShowFashion)
		{
			if (playerData.CheckWeaponIsSame())
			{
				LoadPlayerVisual(mMainPlayer, playerData.FashionWeaponId, playerData.FashionHeadId, playerData.FashionBodyId, playerData.FashionLegId);
				BundleManager.ClearMainPlayerBundleFlag(playerData.FashionWeaponId, playerData.FashionHeadId, playerData.FashionBodyId, playerData.FashionLegId, playerData.MountId);
			}
			else
			{
				LoadPlayerVisual(mMainPlayer, playerData.PartWeaponId, playerData.FashionHeadId, playerData.FashionBodyId, playerData.FashionLegId);
				BundleManager.ClearMainPlayerBundleFlag(playerData.PartWeaponId, playerData.FashionHeadId, playerData.FashionBodyId, playerData.FashionLegId, playerData.MountId);
			}
		}
		else
		{
			LoadPlayerVisual(mMainPlayer, playerData.PartWeaponId, playerData.PartHeadId, playerData.PartBodyId, playerData.PartLegId);
			BundleManager.ClearMainPlayerBundleFlag(playerData.PartWeaponId, playerData.PartHeadId, playerData.PartBodyId, playerData.PartLegId, playerData.MountId);
		}
		AddDict(mMainPlayer.ServerId, mMainPlayer);
		AddToTargetCampList(mMainPlayer);
		if (instance.AutoSearchPath.IsAutoMovingFlag)
		{
			instance.MissionManager.ContinueAutoMoveToMission();
		}
		MissionData missionData = null;
		instance.PlayerCommonData.CheckTitleLevelFunction();
		SingletonUnity<UIManager>.Instance.ShowBaseUI();
		GameManager.IsSceneReady = true;
		SingletonUnity<MyEvent>.Instance.Fire("OnMainPlayerCreate");
	}

	public void CreateMainPlayer(movement posInfo)
	{
		if (mMainPlayer != null)
		{
			return;
		}
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		PlayerData playerData = instance.PlayerData;
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/PlayerRoot") as GameObject;
		gameObject.name = "MainPlayer_" + playerData.PlayerCamp;
		CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(playerData.CharacterModelId);
		if (characterModelDataByID != null)
		{
			string path = $"TestModel/Model/{playerData.Profession}/ModelRoot";
			ReloadModel(gameObject.transform, path);
		}
		if (gameObject != null)
		{
			mMainPlayer = gameObject.GetComponent<ObjMainPlayer>();
			if (mMainPlayer == null)
			{
				mMainPlayer = gameObject.AddComponent<ObjMainPlayer>();
			}
		}
		mMainPlayer.InitInfo(characterModelDataByID);
		mMainPlayer.UpdateWeaponModeID(playerData.PartWeaponId);
		mMainPlayer.UpdateWeaponItemID(playerData.WeaponItemId);
		mMainPlayer.Init();
		mMainPlayer.ResetMainPlayer(posInfo);
		if (playerData.IsShowFashion)
		{
			if (playerData.CheckWeaponIsSame())
			{
				LoadPlayerVisual(mMainPlayer, playerData.FashionWeaponId, playerData.FashionHeadId, playerData.FashionBodyId, playerData.FashionLegId);
				BundleManager.ClearMainPlayerBundleFlag(playerData.FashionWeaponId, playerData.FashionHeadId, playerData.FashionBodyId, playerData.FashionLegId, playerData.MountId);
			}
			else
			{
				LoadPlayerVisual(mMainPlayer, playerData.PartWeaponId, playerData.FashionHeadId, playerData.FashionBodyId, playerData.FashionLegId);
				BundleManager.ClearMainPlayerBundleFlag(playerData.PartWeaponId, playerData.FashionHeadId, playerData.FashionBodyId, playerData.FashionLegId, playerData.MountId);
			}
		}
		else
		{
			LoadPlayerVisual(mMainPlayer, playerData.PartWeaponId, playerData.PartHeadId, playerData.PartBodyId, playerData.PartLegId);
			BundleManager.ClearMainPlayerBundleFlag(playerData.PartWeaponId, playerData.PartHeadId, playerData.PartBodyId, playerData.PartLegId, playerData.MountId);
		}
		AddDict(mMainPlayer.ServerId, mMainPlayer);
		AddToTargetCampList(mMainPlayer);
		if (instance.AutoSearchPath.IsAutoMovingFlag)
		{
			instance.MissionManager.ContinueAutoMoveToMission();
		}
		MissionData missionData = null;
		instance.PlayerCommonData.CheckTitleLevelFunction();
		SingletonUnity<UIManager>.Instance.ShowBaseUI();
		GameManager.IsSceneReady = true;
		SingletonUnity<MyEvent>.Instance.Fire("OnMainPlayerCreate");
	}

	public void LoadPlayerVisual(ObjOtherPlayer player, string partWeaponId, string partHeadId, string partBodyId, string partLegId)
	{
		if (mPlayerMeshLoadNumDic.ContainsKey(player.ServerId))
		{
			mPlayerMeshLoadNumDic[player.ServerId] = 0;
		}
		else
		{
			mPlayerMeshLoadNumDic.Add(player.ServerId, 0);
		}
		if (mPlayerMeshLoadTargetNumDic.ContainsKey(player.ServerId))
		{
			mPlayerMeshLoadTargetNumDic[player.ServerId] = 0;
		}
		else
		{
			mPlayerMeshLoadTargetNumDic.Add(player.ServerId, 0);
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (!partLegId.Equals(player.TargetPartObjId[3]))
		{
			player.SetTargetPartObjId(MODEL_TYPE.LEG, partLegId);
			if (!partLegId.Equals(player.PartObjId[3]))
			{
				Dictionary<long, int> dictionary;
				Dictionary<long, int> dictionary2 = (dictionary = mPlayerMeshLoadTargetNumDic);
				long serverId;
				long key = (serverId = player.ServerId);
				int num = dictionary[serverId];
				dictionary2[key] = num + 1;
				if (player.LoadingModelData[3] != null)
				{
					player.LoadingModelData[3].OnLoadFinished = null;
					player.LoadingModelData[3] = null;
				}
				flag4 = true;
			}
			else
			{
				if (player.LoadingModelData[3] != null)
				{
					player.LoadingModelData[3].OnLoadFinished = null;
					player.LoadingModelData[3] = null;
				}
				flag4 = false;
			}
		}
		else if (!partLegId.Equals(player.PartObjId[3]))
		{
			Dictionary<long, int> dictionary3;
			Dictionary<long, int> dictionary4 = (dictionary3 = mPlayerMeshLoadTargetNumDic);
			long serverId;
			long key2 = (serverId = player.ServerId);
			int num = dictionary3[serverId];
			dictionary4[key2] = num + 1;
			flag4 = player.LoadingModelData[3] == null;
		}
		else
		{
			flag4 = false;
		}
		if (!partHeadId.Equals(player.TargetPartObjId[1]))
		{
			player.SetTargetPartObjId(MODEL_TYPE.HEAD, partHeadId);
			if (!partHeadId.Equals(player.PartObjId[1]))
			{
				Dictionary<long, int> dictionary5;
				Dictionary<long, int> dictionary6 = (dictionary5 = mPlayerMeshLoadTargetNumDic);
				long serverId;
				long key3 = (serverId = player.ServerId);
				int num = dictionary5[serverId];
				dictionary6[key3] = num + 1;
				if (player.LoadingModelData[1] != null)
				{
					player.LoadingModelData[1].OnLoadFinished = null;
					player.LoadingModelData[1] = null;
				}
				flag2 = true;
			}
			else
			{
				if (player.LoadingModelData[1] != null)
				{
					player.LoadingModelData[1].OnLoadFinished = null;
					player.LoadingModelData[1] = null;
				}
				flag2 = false;
			}
		}
		else if (!partHeadId.Equals(player.PartObjId[1]))
		{
			Dictionary<long, int> dictionary7;
			Dictionary<long, int> dictionary8 = (dictionary7 = mPlayerMeshLoadTargetNumDic);
			long serverId;
			long key4 = (serverId = player.ServerId);
			int num = dictionary7[serverId];
			dictionary8[key4] = num + 1;
			flag2 = player.LoadingModelData[1] == null;
		}
		else
		{
			flag2 = false;
		}
		if (!partBodyId.Equals(player.TargetPartObjId[2]))
		{
			player.SetTargetPartObjId(MODEL_TYPE.BODY, partBodyId);
			if (!partBodyId.Equals(player.PartObjId[2]))
			{
				Dictionary<long, int> dictionary9;
				Dictionary<long, int> dictionary10 = (dictionary9 = mPlayerMeshLoadTargetNumDic);
				long serverId;
				long key5 = (serverId = player.ServerId);
				int num = dictionary9[serverId];
				dictionary10[key5] = num + 1;
				if (player.LoadingModelData[2] != null)
				{
					player.LoadingModelData[2].OnLoadFinished = null;
					player.LoadingModelData[2] = null;
				}
				flag3 = true;
			}
			else
			{
				if (player.LoadingModelData[2] != null)
				{
					player.LoadingModelData[2].OnLoadFinished = null;
					player.LoadingModelData[2] = null;
				}
				flag3 = false;
			}
		}
		else if (!partBodyId.Equals(player.PartObjId[2]))
		{
			Dictionary<long, int> dictionary11;
			Dictionary<long, int> dictionary12 = (dictionary11 = mPlayerMeshLoadTargetNumDic);
			long serverId;
			long key6 = (serverId = player.ServerId);
			int num = dictionary11[serverId];
			dictionary12[key6] = num + 1;
			flag3 = player.LoadingModelData[2] == null;
		}
		else
		{
			flag3 = false;
		}
		if (!partWeaponId.Equals(player.TargetPartObjId[0]))
		{
			player.SetTargetPartObjId(MODEL_TYPE.WEAPON, partWeaponId);
			if (!partWeaponId.Equals(player.PartObjId[0]))
			{
				Dictionary<long, int> dictionary13;
				Dictionary<long, int> dictionary14 = (dictionary13 = mPlayerMeshLoadTargetNumDic);
				long serverId;
				long key7 = (serverId = player.ServerId);
				int num = dictionary13[serverId];
				dictionary14[key7] = num + 1;
				if (player.LoadingModelData[0] != null)
				{
					player.LoadingModelData[0].OnLoadFinished = null;
					player.LoadingModelData[0] = null;
				}
				flag = true;
			}
			else
			{
				if (player.LoadingModelData[0] != null)
				{
					player.LoadingModelData[0].OnLoadFinished = null;
					player.LoadingModelData[0] = null;
				}
				flag = false;
			}
		}
		else if (!partWeaponId.Equals(player.PartObjId[0]))
		{
			Dictionary<long, int> dictionary15;
			Dictionary<long, int> dictionary16 = (dictionary15 = mPlayerMeshLoadTargetNumDic);
			long serverId;
			long key8 = (serverId = player.ServerId);
			int num = dictionary15[serverId];
			dictionary16[key8] = num + 1;
			flag = player.LoadingModelData[0] == null;
		}
		else
		{
			flag = false;
		}
		if (mPlayerMeshLoadTargetNumDic[player.ServerId] != 0)
		{
			player.AnimationLogic.AnimaObj.cullingType = AnimationCullingType.AlwaysAnimate;
		}
		bool isNeedUnload = player.ObjType != GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || GameSettingData.IsBundleNeedUnload[GameSettingData.GetPhoneClass()];
		ModelData modelData = null;
		modelData = DataManager.GetModeDataByID(partLegId);
		if (flag4)
		{
			if (modelData != null)
			{
				player.LoadingModelData[3] = BundleManager.LoadModelInList(modelData.Name, isNeedUnload, isDoNotCache: false, OnLoadPlayerPartFinished, modelData, player, modelData.ModelPath);
				CheckModelEffect(modelData, player);
				player.LoadingModelDataId[3] = ((player.LoadingModelData[3] != null) ? player.LoadingModelData[3].ID : (-1));
			}
		}
		else
		{
			CheckModelEffect(modelData, player);
		}
		modelData = null;
		modelData = DataManager.GetModeDataByID(partWeaponId);
		if (flag)
		{
			if (modelData != null)
			{
				player.LoadingModelData[0] = BundleManager.LoadModelInList(modelData.Name, isNeedUnload, isDoNotCache: false, OnLoadPlayerPartFinished, modelData, player, modelData.ModelPath);
				CheckModelEffect(modelData, player);
				player.LoadingModelDataId[0] = ((player.LoadingModelData[0] != null) ? player.LoadingModelData[0].ID : (-1));
			}
		}
		else
		{
			CheckModelEffect(modelData, player);
		}
		modelData = null;
		modelData = DataManager.GetModeDataByID(partHeadId);
		if (flag2)
		{
			if (modelData != null)
			{
				player.LoadingModelData[1] = BundleManager.LoadModelInList(modelData.Name, isNeedUnload, isDoNotCache: false, OnLoadPlayerPartFinished, modelData, player, modelData.ModelPath);
				CheckModelEffect(modelData, player);
				player.LoadingModelDataId[1] = ((player.LoadingModelData[1] != null) ? player.LoadingModelData[1].ID : (-1));
			}
		}
		else
		{
			CheckModelEffect(modelData, player);
		}
		modelData = null;
		modelData = DataManager.GetModeDataByID(partBodyId);
		if (flag3)
		{
			if (modelData != null)
			{
				player.LoadingModelData[2] = BundleManager.LoadModelInList(modelData.Name, isNeedUnload, isDoNotCache: false, OnLoadPlayerPartFinished, modelData, player, modelData.ModelPath);
				CheckModelEffect(modelData, player);
				player.LoadingModelDataId[2] = ((player.LoadingModelData[2] != null) ? player.LoadingModelData[2].ID : (-1));
			}
		}
		else
		{
			CheckModelEffect(modelData, player);
		}
		modelData = null;
	}

	public static string GetWeaponId(characterVisual visual)
	{
		if (visual.showType == 1 && visual.HasFashion_WeaponId)
		{
			if (CheckWeaponIsSame(visual))
			{
				return visual.Fashion_WeaponId;
			}
			return visual.WeaponId;
		}
		return visual.WeaponId;
	}

	public static bool CheckWeaponIsSame(characterVisual visual)
	{
		if (visual.HasWeaponItemId && visual.HasFashionItemId)
		{
			EquipData equipDataById = DataManager.GetEquipDataById(visual.WeaponItemId);
			EquipData equipDataById2 = DataManager.GetEquipDataById(visual.FashionItemId);
			return equipDataById.WeaponType == equipDataById2.WeaponType;
		}
		if (visual.HasWeaponId && visual.HasFashion_WeaponId)
		{
			if (GameDefine.GetWeaponName(visual.WeaponId).Equals(GameDefine.GetWeaponName(visual.Fashion_WeaponId)))
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public static string GetHeadId(characterVisual visual)
	{
		if (visual.showType == 1 && visual.HasFashion_HeadId)
		{
			return visual.Fashion_HeadId;
		}
		return visual.HeadId;
	}

	public static string GetBodyId(characterVisual visual)
	{
		if (visual.showType == 1 && visual.HasFashion_BodyId)
		{
			return visual.Fashion_BodyId;
		}
		return visual.BodyId;
	}

	public static string GetLegId(characterVisual visual)
	{
		if (visual.showType == 1 && visual.HasFashion_LegId)
		{
			return visual.Fashion_LegId;
		}
		return visual.LegId;
	}

	public void OnLoadPlayerPartFinished(object objBundle, object param1 = null, object param2 = null)
	{
		ObjOtherPlayer objOtherPlayer = param2 as ObjOtherPlayer;
		if (objOtherPlayer == null)
		{
			return;
		}
		ModelData modelData = param1 as ModelData;
		GameObject gameObject = objBundle as GameObject;
		Material sm = null;
		if (objOtherPlayer.PartObject[(int)modelData.MODELTYPE] != null)
		{
			UnityEngine.Object.Destroy(objOtherPlayer.PartObject[modelData.ModelType]);
			objOtherPlayer.PartObject[modelData.ModelType] = null;
			BundleManager.UnloadModel(objOtherPlayer.PartObjId[modelData.ModelType], -1L, objOtherPlayer.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER);
		}
		objOtherPlayer.PartObject[modelData.ModelType] = gameObject;
		objOtherPlayer.PartObjId[modelData.ModelType] = modelData.ID;
		objOtherPlayer.LoadingModelData[modelData.ModelType] = null;
		objOtherPlayer.LoadingModelDataId[modelData.ModelType] = -1L;
		gameObject.name = modelData.MODELTYPE.ToString();
		gameObject.transform.parent = objOtherPlayer.AnimationLogic.AnimaObj.transform;
		gameObject.transform.localPosition = Vector3.zero;
		BundleManager.RebuildBones(objOtherPlayer, gameObject);
		BundleManager.ResetShader(gameObject.transform, out sm);
		if (sm.HasProperty("_DefaultColor"))
		{
			Color color = sm.GetColor("_DefaultColor");
			sm.SetColor("_Color", new Color(color.r, color.g, color.b, 0f));
			sm.DOColor(color, 1.5f);
		}
		objOtherPlayer.PartMatList[modelData.ModelType] = sm;
		if (UnityVersionUtil.IsActive(objOtherPlayer.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(gameObject.gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(gameObject.gameObject, state: false);
		}
		if (objOtherPlayer.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			if (!SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsCarScene())
			{
				AddOutLineMaterial(gameObject, objOtherPlayer as ObjMainPlayer);
			}
			gameObject.layer = 31;
		}
		Dictionary<long, int> dictionary;
		Dictionary<long, int> dictionary2 = (dictionary = mPlayerMeshLoadNumDic);
		long serverId;
		long key = (serverId = objOtherPlayer.ServerId);
		int num = dictionary[serverId];
		dictionary2[key] = num + 1;
		if (mPlayerMeshLoadNumDic[objOtherPlayer.ServerId] != mPlayerMeshLoadTargetNumDic[objOtherPlayer.ServerId])
		{
			return;
		}
		if (objOtherPlayer.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			SingletonUnity<MyEvent>.Instance.Fire("OnMainPlayerMeshLoadDone");
			if (GameSettingData.IsShowPlayerShadow[GameSettingData.GetPhoneClass()])
			{
				List<GameObject> list = new List<GameObject>();
				for (int i = 0; i < objOtherPlayer.PartObject.Length; i++)
				{
					if (objOtherPlayer.PartObject[i] != null)
					{
						list.Add(objOtherPlayer.PartObject[i]);
					}
				}
				if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.MapType != MAPTYPE.CAR_CHASE_COPY)
				{
					if (!SingletonUnity<RealTimeShadow>.Exists)
					{
						GameObject gameObject2 = ResourcesManager.LoadAndInstantiate("Items/PlayerRealTimeShadow") as GameObject;
						RealTimeShadow component = gameObject2.GetComponent<RealTimeShadow>();
						component.Reset(list);
					}
					else
					{
						SingletonUnity<RealTimeShadow>.Instance.Reset(list);
					}
				}
			}
		}
		mPlayerMeshLoadNumDic.Remove(objOtherPlayer.ServerId);
		mPlayerMeshLoadTargetNumDic.Remove(objOtherPlayer.ServerId);
	}

	public static void AddOutLineMaterial(GameObject parentObj, ObjMainPlayer mainPlayer)
	{
		if (mainPlayer.IsServerRidingMount || mainPlayer.IsDrivingMount() || !GameSettingData.IsShowPlayerShadow[GameSettingData.GetPhoneClass()])
		{
			return;
		}
		SkinnedMeshRenderer[] componentsInChildren = parentObj.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
		{
			Material[] array = new Material[skinnedMeshRenderer.materials.Length + 1];
			for (int j = 0; j < skinnedMeshRenderer.materials.Length; j++)
			{
				if (skinnedMeshRenderer.materials[j].name.Contains("XRay"))
				{
					return;
				}
				array[j] = skinnedMeshRenderer.materials[j];
			}
			array[skinnedMeshRenderer.materials.Length] = mainPlayer.XRayMat;
			skinnedMeshRenderer.materials = array;
		}
	}

	public void CheckModelEffect(ModelData modelData, ObjOtherPlayer player)
	{
		if (!GameSettingData.IsShowFashionEffect[GameSettingData.GetPhoneClass()])
		{
			return;
		}
		if (!string.IsNullOrEmpty(modelData.EffectId))
		{
			List<FxEffInfoData> fxEffInfoDataListById = DataManager.GetFxEffInfoDataListById(modelData.EffectId);
			for (int i = 0; i < fxEffInfoDataListById.Count; i++)
			{
				ModelEffectData modelEffectData = new ModelEffectData(modelData, fxEffInfoDataListById[i], i == 0, string.Empty);
				string path = $"{fxEffInfoDataListById[i].EffFilePath}/{fxEffInfoDataListById[i].EffName}";
				GameObject gameObject = ResourcesManager.LoadAndInstantiate(path) as GameObject;
				List<GameObject> list = player.PartEffectList[modelData.ModelType];
				if (list == null)
				{
					list = new List<GameObject>();
					player.PartEffectList[modelData.ModelType] = list;
				}
				else if (i == 0)
				{
					for (int j = 0; j < list.Count; j++)
					{
						UnityEngine.Object.Destroy(list[j]);
					}
					list.Clear();
				}
				if (gameObject != null)
				{
					list.Add(gameObject);
					gameObject.transform.parent = TransformUtil.FindChildTransform(player.CacheTransform, modelEffectData.fxEffinfoData.EffLinkNode);
					gameObject.transform.localPosition = modelEffectData.fxEffinfoData.Position;
					gameObject.transform.localEulerAngles = modelEffectData.fxEffinfoData.Angel;
				}
				else
				{
					BundleManager.LoadEffectInList(fxEffInfoDataListById[i].EffName, isNeedUnload: false, isDoNotCache: false, OnLoadPlayerPartEffectFinished, modelEffectData, player);
				}
			}
			return;
		}
		List<GameObject> list2 = player.PartEffectList[modelData.ModelType];
		if (list2 != null)
		{
			for (int k = 0; k < list2.Count; k++)
			{
				UnityEngine.Object.Destroy(list2[k]);
			}
			list2.Clear();
		}
	}

	public void OnLoadPlayerPartEffectFinished(object objBundle, object param1 = null, object param2 = null)
	{
		ObjOtherPlayer objOtherPlayer = param2 as ObjOtherPlayer;
		if (objOtherPlayer == null)
		{
			return;
		}
		ModelEffectData modelEffectData = param1 as ModelEffectData;
		GameObject gameObject = objBundle as GameObject;
		if (objOtherPlayer.TargetPartObjId[modelEffectData.modelData.ModelType].Equals(modelEffectData.modelData.ID))
		{
			List<GameObject> list = objOtherPlayer.PartEffectList[modelEffectData.modelData.ModelType];
			list.Add(gameObject);
			gameObject.transform.parent = TransformUtil.FindChildTransform(objOtherPlayer.CacheTransform, modelEffectData.fxEffinfoData.EffLinkNode);
			gameObject.transform.localPosition = modelEffectData.fxEffinfoData.Position;
			gameObject.transform.localEulerAngles = modelEffectData.fxEffinfoData.Angel;
			if (UnityVersionUtil.IsActive(objOtherPlayer.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(gameObject.gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(gameObject.gameObject, state: false);
			}
		}
		else
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public void CreateZombiePlayer(ObjInitPlayerData initData)
	{
		ObjZombiePlayer objZombiePlayer = null;
		initData.mPos = new Vector3(initData.mPos.x, SceneManager.GetHitHeight(initData.mPos), initData.mPos.z);
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/PlayerRoot") as GameObject;
		gameObject.name = $"ZombiePlayer{initData.mServerID}";
		CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(initData.mCharacterModelId);
		if (characterModelDataByID != null)
		{
			string path = $"TestModel/Model/{initData.Profession}/ModelRoot";
			ReloadModel(gameObject.transform, path);
		}
		if (gameObject != null)
		{
			objZombiePlayer = gameObject.GetComponent<ObjZombiePlayer>();
			if (objZombiePlayer == null)
			{
				objZombiePlayer = gameObject.AddComponent<ObjZombiePlayer>();
			}
			objZombiePlayer.InitInfo(characterModelDataByID);
			objZombiePlayer.UpdateWeaponModeID(initData.GetWeaponID());
			objZombiePlayer.UpdateWeaponItemID(initData.WeaponItemId);
			objZombiePlayer.Init();
			objZombiePlayer.ResetZombiePlayer(initData);
			AddDict(objZombiePlayer.ServerId, objZombiePlayer);
			AddToTargetCampList(objZombiePlayer);
			if (CheckOtherPlayerVisiable(objZombiePlayer))
			{
				LoadPlayerVisual(objZombiePlayer, initData.GetWeaponID(), initData.HeadId, initData.BodyId, initData.LegId);
			}
		}
	}

	public void RecycleZombiePlayer(ObjZombiePlayer zombiePlayer)
	{
		RemoveObj(zombiePlayer);
		RemoveFromTargetCampList(zombiePlayer);
		if (mObjOtherPlayerVisibleList.Contains(zombiePlayer))
		{
			mObjOtherPlayerVisibleList.Remove(zombiePlayer);
		}
		if (mObjOtherPlayerInVisibleList.Contains(zombiePlayer))
		{
			mObjOtherPlayerInVisibleList.Remove(zombiePlayer);
		}
		UpdateInVisibleOtherPlayer();
		UnityEngine.Object.Destroy(zombiePlayer.gameObject);
	}

	public ObjZombieRagdollPlayer CreateZombieRagdollPlayer(ObjInitPlayerData initData)
	{
		ObjZombieRagdollPlayer objZombieRagdollPlayer = null;
		initData.mPos = new Vector3(initData.mPos.x, SceneManager.GetHitHeight(initData.mPos), initData.mPos.z);
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/PlayerRoot") as GameObject;
		gameObject.name = $"ZombiePlayer{initData.mServerID}";
		CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(initData.mCharacterModelId);
		if (characterModelDataByID != null)
		{
			string path = $"TestModel/Model/{initData.Profession}/ModelRoot";
			ReloadModel(gameObject.transform, path);
		}
		if (gameObject != null)
		{
			objZombieRagdollPlayer = gameObject.GetComponent<ObjZombieRagdollPlayer>();
			if (objZombieRagdollPlayer == null)
			{
				objZombieRagdollPlayer = gameObject.AddComponent<ObjZombieRagdollPlayer>();
			}
			objZombieRagdollPlayer.InitInfo(characterModelDataByID);
			objZombieRagdollPlayer.UpdateWeaponModeID(initData.GetWeaponID());
			objZombieRagdollPlayer.UpdateWeaponItemID(initData.WeaponItemId);
			objZombieRagdollPlayer.Init();
			objZombieRagdollPlayer.ResetObjZombieRagdollPlayer(initData);
			AddDict(objZombieRagdollPlayer.ServerId, objZombieRagdollPlayer);
			AddToTargetCampList(objZombieRagdollPlayer);
			if (CheckOtherPlayerVisiable(objZombieRagdollPlayer))
			{
				LoadPlayerVisual(objZombieRagdollPlayer, initData.GetWeaponID(), initData.HeadId, initData.BodyId, initData.LegId);
			}
			ObjZombieRagdollPlayer.InitRagdollObj(gameObject.transform.GetChild(0), objZombieRagdollPlayer);
			if (objZombieRagdollPlayer.IsMissionNpc)
			{
				GameObject gameObject2 = ResourcesManager.LoadAndInstantiate("Items/qiGan") as GameObject;
				gameObject2.transform.position = objZombieRagdollPlayer.transform.position;
				gameObject2.transform.rotation = objZombieRagdollPlayer.transform.rotation;
				gameObject2.transform.localScale = Vector3.one;
				objZombieRagdollPlayer.FlagObj = gameObject2;
			}
			return objZombieRagdollPlayer;
		}
		return null;
	}

	public void RecycleZombieRagdollPlayer(ObjZombieRagdollPlayer zombiePlayer)
	{
		if (SingletonUnity<CitySimController>.Exists)
		{
			SingletonUnity<CitySimController>.Instance.OnRecycleZombiePlayer(zombiePlayer);
		}
		RemoveObj(zombiePlayer);
		RemoveFromTargetCampList(zombiePlayer);
		if (mObjOtherPlayerVisibleList.Contains(zombiePlayer))
		{
			mObjOtherPlayerVisibleList.Remove(zombiePlayer);
		}
		if (mObjOtherPlayerInVisibleList.Contains(zombiePlayer))
		{
			mObjOtherPlayerInVisibleList.Remove(zombiePlayer);
		}
		UpdateInVisibleOtherPlayer();
		if (zombiePlayer.FlagObj != null)
		{
			UnityEngine.Object.Destroy(zombiePlayer.FlagObj);
		}
		UnityEngine.Object.Destroy(zombiePlayer.gameObject);
	}

	private int GetPlayerCount()
	{
		return mCampList[0].Count + mCampList[1].Count;
	}

	public void CreateOtherPlayer(ObjInitPlayerData initData)
	{
		initData.mPos = new Vector3(initData.mPos.x, SceneManager.GetHitHeight(initData.mPos), initData.mPos.z);
		if (GetPlayerCount() <= GameSettingData.MaxOtherPlayerLogicNum[GameSettingData.GetPhoneClass()])
		{
			ObjOtherPlayer otherPlayer = mOtherPlayerPool.GetOtherPlayer(initData);
			if (otherPlayer != null)
			{
				bool flag = CheckOtherPlayerVisiable(otherPlayer);
				initData.SetVisible(flag);
				UnityVersionUtil.SetActiveRecursive(otherPlayer.gameObject, state: true);
				otherPlayer.UpdateWeaponModeID(initData.GetWeaponID());
				otherPlayer.UpdateWeaponItemID(initData.WeaponItemId);
				otherPlayer.ResetOtherPlayer(initData);
				AddDict(otherPlayer.ServerId, otherPlayer);
				AddToTargetCampList(otherPlayer);
				if (flag)
				{
					LoadPlayerVisual(otherPlayer, initData.GetWeaponID(), initData.HeadId, initData.BodyId, initData.LegId);
				}
				return;
			}
			GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/OtherPlayerRoot") as GameObject;
			gameObject.name = $"OtherPlayer{initData.mServerID}";
			CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(initData.mCharacterModelId);
			if (characterModelDataByID != null)
			{
				string path = $"TestModel/Model/{initData.Profession}/ModelRoot";
				ReloadModel(gameObject.transform, path);
			}
			if (gameObject != null)
			{
				otherPlayer = gameObject.GetComponent<ObjOtherPlayer>();
				if (otherPlayer == null)
				{
					otherPlayer = gameObject.AddComponent<ObjOtherPlayer>();
				}
				bool flag2 = CheckOtherPlayerVisiable(otherPlayer);
				initData.SetVisible(flag2);
				otherPlayer.InitInfo(characterModelDataByID);
				otherPlayer.UpdateWeaponModeID(initData.GetWeaponID());
				otherPlayer.UpdateWeaponItemID(initData.WeaponItemId);
				otherPlayer.Init();
				otherPlayer.ResetOtherPlayer(initData);
				AddDict(otherPlayer.ServerId, otherPlayer);
				AddToTargetCampList(otherPlayer);
				if (flag2)
				{
					LoadPlayerVisual(otherPlayer, initData.GetWeaponID(), initData.HeadId, initData.BodyId, initData.LegId);
					return;
				}
				otherPlayer.SetTargetPartObjId(MODEL_TYPE.WEAPON, initData.GetWeaponID());
				otherPlayer.SetTargetPartObjId(MODEL_TYPE.HEAD, initData.HeadId);
				otherPlayer.SetTargetPartObjId(MODEL_TYPE.BODY, initData.BodyId);
				otherPlayer.SetTargetPartObjId(MODEL_TYPE.LEG, initData.LegId);
			}
			else
			{
				Debug.Log("Create Other Player Error");
			}
		}
		else
		{
			if (mOtherPlayerNoLogicDataDic.ContainsKey(initData.mServerID))
			{
				mOtherPlayerNoLogicDataDic[initData.mServerID] = initData;
			}
			else
			{
				mOtherPlayerNoLogicDataDic.Add(initData.mServerID, initData);
			}
			update_client_state.request request = new update_client_state.request();
			request.id = initData.mServerID;
			request.state = 0L;
			NetLogic.GetInstance().Send<Protocol.update_client_state>(request);
		}
	}

	public void RecycleOtherPlayer(ObjOtherPlayer otherPlayer)
	{
		otherPlayer.DisMountCar();
		otherPlayer.StopDance();
		RemoveObj(otherPlayer);
		RemoveFromTargetCampList(otherPlayer);
		otherPlayer.IsDie = true;
		if (mObjOtherPlayerVisibleList.Contains(otherPlayer))
		{
			mObjOtherPlayerVisibleList.Remove(otherPlayer);
		}
		if (mObjOtherPlayerInVisibleList.Contains(otherPlayer))
		{
			mObjOtherPlayerInVisibleList.Remove(otherPlayer);
		}
		UpdateInVisibleOtherPlayer();
		mOtherPlayerPool.RecycleOtherPlayer(otherPlayer);
		UpdateNoLogicOtherPlayer();
	}

	private void UpdateNoLogicOtherPlayer()
	{
		if (mOtherPlayerNoLogicDataDic.Count <= 0 || GetPlayerCount() > GameSettingData.MaxOtherPlayerLogicNum[GameSettingData.GetPhoneClass()])
		{
			return;
		}
		ObjInitPlayerData objInitPlayerData = null;
		List<ObjInitPlayerData> list = new List<ObjInitPlayerData>(mOtherPlayerNoLogicDataDic.Values);
		float num = float.MaxValue;
		float num2 = float.MaxValue;
		for (int i = 0; i < list.Count; i++)
		{
			num2 = VectorXZ.Distance(MainPlayer.Position, list[i].mPos);
			if (num2 < num)
			{
				objInitPlayerData = list[i];
			}
		}
		RemoveNoLogicOtherPlayerData(objInitPlayerData.mServerID);
		CreateOtherPlayer(objInitPlayerData);
	}

	public bool CheckOtherPlayerVisiable(ObjOtherPlayer otherPlayer)
	{
		if (mObjOtherPlayerVisibleList.Count >= GameSettingData.MaxOtherPlayerVisibleNum[GameSettingData.GetPhoneClass()])
		{
			mObjOtherPlayerInVisibleList.Add(otherPlayer);
			return false;
		}
		mObjOtherPlayerVisibleList.Add(otherPlayer);
		return true;
	}

	public void UpdateInVisibleOtherPlayer()
	{
		if (mObjOtherPlayerVisibleList.Count >= GameSettingData.MaxOtherPlayerVisibleNum[GameSettingData.GetPhoneClass()])
		{
			return;
		}
		int num = GameSettingData.MaxOtherPlayerVisibleNum[GameSettingData.GetPhoneClass()] - mObjOtherPlayerVisibleList.Count;
		num = ((num <= mObjOtherPlayerInVisibleList.Count) ? num : mObjOtherPlayerInVisibleList.Count);
		List<long> list = new List<long>();
		if (mMainPlayer != null && mMainPlayer.TeamId != -1)
		{
			Team teamInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo;
			for (int i = 0; i < teamInfo.TeamMembers.Length; i++)
			{
				if (teamInfo.TeamMembers[i].IsValid() && teamInfo.TeamMembers[i].ServerId != PlayerData.MainPlayerServerId)
				{
					list.Add(teamInfo.TeamMembers[i].ServerId);
				}
			}
		}
		for (int j = 0; j < num; j++)
		{
			ObjOtherPlayer objOtherPlayer = null;
			if (list.Count > 0)
			{
				for (int k = 0; k < mObjOtherPlayerInVisibleList.Count; k++)
				{
					if (list.Contains(mObjOtherPlayerInVisibleList[k].ServerId))
					{
						list.Remove(mObjOtherPlayerInVisibleList[k].ServerId);
						objOtherPlayer = mObjOtherPlayerInVisibleList[k];
						mObjOtherPlayerInVisibleList.Remove(objOtherPlayer);
						break;
					}
				}
			}
			if (objOtherPlayer == null)
			{
				objOtherPlayer = mObjOtherPlayerInVisibleList[0];
				mObjOtherPlayerInVisibleList.RemoveAt(0);
			}
			mObjOtherPlayerVisibleList.Add(objOtherPlayer);
			objOtherPlayer.UpdateWeaponModeID(objOtherPlayer.TargetPartObjId[0]);
			objOtherPlayer.SetVisible(visible: true);
			if (objOtherPlayer.PartObject[1] == null)
			{
				LoadPlayerVisual(objOtherPlayer, objOtherPlayer.TargetPartObjId[0], objOtherPlayer.TargetPartObjId[1], objOtherPlayer.TargetPartObjId[2], objOtherPlayer.TargetPartObjId[3]);
			}
		}
	}

	public GameObject ReloadModel(Transform root, string path)
	{
		GameObject gameObject = ResourcesManager.LoadAndInstantiate(path) as GameObject;
		if (gameObject == null)
		{
			return null;
		}
		gameObject.transform.parent = root;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.name = "ModelRoot";
		return gameObject;
	}

	public Obj FindObj(long id)
	{
		if (mObjDict.ContainsKey(id))
		{
			return mObjDict[id];
		}
		return null;
	}

	public ObjCharacter FindObjInScene(long id)
	{
		if (mObjDict.ContainsKey(id))
		{
			return mObjDict[id] as ObjCharacter;
		}
		return null;
	}

	public ObjNPC FindMissionNpcInScene(string npcId)
	{
		List<ObjNPC> enableNpcList = mObjNpcPoolGroup.EnableNpcList;
		for (int i = 0; i < enableNpcList.Count; i++)
		{
			if (enableNpcList[i].NPCDataID.Equals(npcId))
			{
				if (enableNpcList[i].AttributeData.Camp == GameDefine.CAMP_TYPE.FUNCTION_NPC)
				{
					return enableNpcList[i];
				}
				return null;
			}
		}
		List<ObjRagdollNPC> enableList = mObjRagdollNPCPoolGroup.EnableList;
		for (int j = 0; j < enableList.Count; j++)
		{
			if (enableList[j].NPCData.ID.Equals(npcId))
			{
				if (enableList[j].AttributeData.Camp == GameDefine.CAMP_TYPE.FUNCTION_NPC)
				{
					return enableList[j];
				}
				return null;
			}
		}
		return null;
	}

	public void Clear()
	{
		if (mObjDict != null)
		{
			mObjDict.Clear();
		}
		if (mOtherObjDic != null)
		{
			mOtherObjDic.Clear();
		}
		if (mOtherPlayerEscortNPCList != null)
		{
			mOtherPlayerEscortNPCList.Clear();
		}
	}

	public void AddDict(long id, Obj obj)
	{
		if (mObjDict.ContainsKey(id))
		{
			Log.ERROR_MSG("Erro AddDict SeverId=" + id);
			return;
		}
		mObjDict.Add(id, obj);
		AddMapShowObj(obj);
	}

	public void RemoveDict(long id)
	{
		if (mObjDict != null)
		{
			mObjDict.Remove(id);
		}
	}

	public void RemoveObj(ObjCharacter character)
	{
		character.Recyle();
		RemoveDict(character.ServerId);
		RemoveShowObj(character);
	}

	public void RemoveObj(long serverId)
	{
		Obj obj = FindObj(serverId);
		if (obj.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
		{
			ObjOtherPlayer otherPlayer = obj as ObjOtherPlayer;
			RecycleOtherPlayer(otherPlayer);
		}
		else if (obj.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
		{
			ObjNPC objNpc = obj as ObjNPC;
			RecycleNpc(objNpc);
		}
		else if (obj.ObjType == GameDefine.OBJ_TYPE.OBJ_DROP_ITEM)
		{
			ObjDropItem dropItem = obj as ObjDropItem;
			RecycleDropItem(dropItem);
		}
		else if (obj.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			RemoveDict(serverId);
			UnityEngine.Object.Destroy(obj);
		}
	}

	public void RecycleNpc(ObjNPC objNpc)
	{
		SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.OnRecycleNpc(objNpc);
		mObjNpcPoolGroup.RecycleNpc(objNpc);
	}

	public void CreateNPC(ObjInitNpcData initData, OnGetNPC func = null, OnGetNPC onLoadModelDone = null)
	{
		func = ((func == null) ? new OnGetNPC(OnNpcGet) : ((OnGetNPC)Delegate.Combine(func, new OnGetNPC(OnNpcGet))));
		initData.mPos = new Vector3(initData.mPos.x, SceneManager.GetHitHeight(initData.mPos), initData.mPos.z);
		mObjNpcPoolGroup.GetNpc(initData, func, onLoadModelDone);
	}

	public void DisableAllLocalNpcAction()
	{
		for (int i = 0; i < mObjNpcPoolGroup.EnableNpcList.Count; i++)
		{
			if (mObjNpcPoolGroup.EnableNpcList[i].AILogic != null)
			{
				mObjNpcPoolGroup.EnableNpcList[i].AILogic.DisableAIAction();
			}
		}
	}

	public void OnNpcGet(ObjNPC objNpc)
	{
		AddDict(objNpc.ServerId, objNpc);
		AddToTargetCampList(objNpc);
		if (objNpc.NPCData.Type == 4)
		{
			CurMission escortMission = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetEscortMission();
			if (escortMission == null || objNpc.ServerId != escortMission.GetParam(1))
			{
				mOtherPlayerEscortNPCList.Add(objNpc);
			}
		}
	}

	public bool IsMyEscortNpc(ObjNPC objNpc)
	{
		CurMission escortMission = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetEscortMission();
		if (escortMission != null && objNpc.ServerId == escortMission.GetParam(1))
		{
			return true;
		}
		return false;
	}

	public void OnNpcRecycle(ObjNPC objNpc)
	{
		RemoveObj(objNpc);
		RemoveFromTargetCampList(objNpc);
		if (objNpc.NPCData.Type == 4)
		{
			CurMission escortMission = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetEscortMission();
			if (escortMission == null || objNpc.ServerId != escortMission.GetParam(1))
			{
				mOtherPlayerEscortNPCList.Remove(objNpc);
			}
		}
	}

	public bool CheckNPCClear(ObjNPC objNpc)
	{
		return mObjNpcPoolGroup.CheckNpcClear(objNpc);
	}

	public void CreateDropItem(ObjInitDropItemData dropItemInitData)
	{
		if (dropItemInitData.ownerServerId == MainPlayer.ServerId)
		{
			if (dropItemInitData.ItemType == GameDefine.ITEM_TYPE.ADD_COIN)
			{
				ResourcesManager.LoadDropItemPrefab(UIInfo.DropMoney, "DropItem", OnCreateDropItem, dropItemInitData);
			}
			else
			{
				SimpleRewardRootLogic.AddReward(dropItemInitData.item);
			}
		}
	}

	public void OnCreateDropItem(GameObject newObj, ObjInitDropItemData initData)
	{
		if (newObj == null)
		{
			Debug.Log("create dropItem obj == null");
			return;
		}
		if (initData == null)
		{
			Debug.Log("dropItem InitData == null");
			return;
		}
		ObjDropItem objDropItem = newObj.GetComponent<ObjDropItem>();
		if (objDropItem == null)
		{
			objDropItem = newObj.AddComponent<ObjDropItem>();
		}
		objDropItem.Init(initData);
		AddDict(initData.ServerID, objDropItem);
	}

	public void RecycleDropItem(ObjDropItem dropItem)
	{
		RemoveDict(dropItem.ServerId);
		ResourcesManager.UnLoadDropItemPrefab(dropItem.gameObject);
	}

	public void CreateSurveyItem(SurveyMissionData surveyMissionData)
	{
		if (surveyMissionData == null)
		{
			return;
		}
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/SurveyItemRoot") as GameObject;
		gameObject.gameObject.name = surveyMissionData.GetSurveyItemName();
		SurveyItemObj surveyItemObj = gameObject.AddComponent<SurveyItemObj>();
		surveyItemObj.Reset(surveyMissionData);
		surveyItemObj.MeshRoot = null;
		surveyItemObj.EffectRoot = gameObject.transform.GetChild(0).gameObject;
		if (!string.IsNullOrEmpty(surveyMissionData.ModelName))
		{
			GameObject gameObject2 = ResourcesManager.LoadAndInstantiate("TestModel/" + surveyMissionData.ModelName) as GameObject;
			if (gameObject2 != null)
			{
				surveyItemObj.MeshRoot = gameObject2;
				gameObject2.transform.parent = surveyItemObj.gameObject.transform;
				gameObject2.transform.localPosition = Vector3.zero;
				gameObject2.transform.localRotation = Quaternion.identity;
			}
		}
		AddOtherObjDic(surveyMissionData.GetSurveyItemName(), gameObject);
	}

	private void OnLoadSurveyItemFinished(object objBundle, object param1, object param2)
	{
		SurveyItemObj surveyItemObj = param1 as SurveyItemObj;
		surveyItemObj.LoadingModelData = null;
		surveyItemObj.LoadingModelDataId = -1L;
		SurveyMissionData surveyMissionData = param2 as SurveyMissionData;
		GameObject gameObject = objBundle as GameObject;
		BundleManager.ResetShader(gameObject.transform);
		gameObject.transform.parent = surveyItemObj.transform;
		gameObject.transform.localPosition = Vector3.zero;
		UnityVersionUtil.SetActiveRecursive(gameObject.gameObject, state: true);
		surveyItemObj.MeshRoot = gameObject;
	}

	public void RecycleSurveyItem(SurveyItemObj surveyItem)
	{
		if (!(surveyItem == null) && !(surveyItem.gameObject == null))
		{
			if (surveyItem.LoadingModelData != null)
			{
				surveyItem.LoadingModelData.OnLoadFinished = null;
				surveyItem.LoadingModelData = null;
			}
			RemoveOtherObj($"SurveyItem{surveyItem.SurveyMissionData.ID}");
			UnityEngine.Object.Destroy(surveyItem.gameObject);
		}
	}

	private void AddOtherObjDic(string id, GameObject obj)
	{
		if (mOtherObjDic == null)
		{
			mOtherObjDic = new Dictionary<string, GameObject>();
		}
		if (!mOtherObjDic.ContainsKey(id))
		{
			mOtherObjDic.Add(id, obj);
		}
		else
		{
			Debug.Log(id + " Has Already In The Dic");
		}
	}

	public GameObject FindOtherObjInDic(string id)
	{
		if (mOtherObjDic != null && mOtherObjDic.ContainsKey(id))
		{
			return mOtherObjDic[id];
		}
		return null;
	}

	public bool RemoveOtherObj(string id)
	{
		if (mOtherObjDic.ContainsKey(id))
		{
			mOtherObjDic.Remove(id);
			return true;
		}
		return false;
	}

	public void ClearAll()
	{
		for (int i = 0; i < mCampTargetList.Length; i++)
		{
			if (mCampTargetList[i] == null)
			{
				mCampTargetList[i] = new List<ObjCharacter>();
			}
			mCampTargetList[i].Clear();
		}
		for (int j = 0; j < mCampList.Length; j++)
		{
			if (mCampList[j] == null)
			{
				mCampList[j] = new List<ObjCharacter>();
			}
			mCampList[j].Clear();
		}
		mMapShowObjList.Clear();
		ClearNPC();
		ClearOtherPlayer();
		mMainPlayer = null;
		mMainPlayerCar = null;
		Clear();
		ClearAICar();
		ClearRagdollNPCPool();
		ClearPatrolNPCPool();
		ClearMountCarPoolGroup();
		ClearBombPool();
		ClearFakeCar();
		mPlayerMeshLoadNumDic.Clear();
		mPlayerMeshLoadTargetNumDic.Clear();
	}

	public void RecycleAllNPC()
	{
		for (int num = mObjNpcPoolGroup.EnableNpcList.Count - 1; num >= 0; num--)
		{
			RecycleNpc(mObjNpcPoolGroup.EnableNpcList[num]);
		}
	}

	public void ClearNPC()
	{
		mObjNpcPoolGroup.ClearNPC();
	}

	public void RecycleAllOtherPlayer()
	{
		mOtherPlayerNoLogicDataDic.Clear();
		List<Obj> list = new List<Obj>(mObjDict.Values);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
			{
				RecycleOtherPlayer(list[i] as ObjOtherPlayer);
			}
		}
	}

	public void ClearOtherPlayer()
	{
		mOtherPlayerPool.Clear();
		mObjOtherPlayerVisibleList.Clear();
		mObjOtherPlayerInVisibleList.Clear();
		mOtherPlayerNoLogicDataDic.Clear();
	}

	public ObjNPC FindNearestFightNPCInScene(string npcID)
	{
		float num = float.MaxValue;
		ObjNPC result = null;
		for (int i = 0; i < mObjNpcPoolGroup.EnableNpcList.Count; i++)
		{
			ObjNPC objNPC = mObjNpcPoolGroup.EnableNpcList[i];
			if (!objNPC.IsMissionNpc() && objNPC.NPCDataID.Equals(npcID))
			{
				float num2 = VectorXZ.Distance(mMainPlayer.Position, objNPC.Position);
				if (num2 < num)
				{
					num = num2;
					result = objNPC;
				}
			}
		}
		for (int j = 0; j < EnableRagdollNpcList.Count; j++)
		{
			ObjNPC objNPC2 = EnableRagdollNpcList[j];
			if (!objNPC2.IsMissionNpc() && objNPC2.NPCDataID.Equals(npcID))
			{
				float num3 = VectorXZ.Distance(mMainPlayer.Position, objNPC2.Position);
				if (num3 < num)
				{
					num = num3;
					result = objNPC2;
				}
			}
		}
		return result;
	}

	public ObjNPC FindNearestFightNPCInScene()
	{
		float num = float.MaxValue;
		ObjNPC result = null;
		for (int i = 0; i < mObjNpcPoolGroup.EnableNpcList.Count; i++)
		{
			ObjNPC objNPC = mObjNpcPoolGroup.EnableNpcList[i];
			if (!objNPC.IsMissionNpc())
			{
				float num2 = VectorXZ.Distance(mMainPlayer.Position, objNPC.Position);
				if (num2 < num)
				{
					num = num2;
					result = objNPC;
				}
			}
		}
		for (int j = 0; j < EnableRagdollNpcList.Count; j++)
		{
			ObjNPC objNPC2 = EnableRagdollNpcList[j];
			if (!objNPC2.IsMissionNpc())
			{
				float num3 = VectorXZ.Distance(mMainPlayer.Position, objNPC2.Position);
				if (num3 < num)
				{
					num = num3;
					result = objNPC2;
				}
			}
		}
		return result;
	}

	public bool IsCanAttack(ObjCharacter curCharacter)
	{
		float num = 100f;
		if (curCharacter == null || curCharacter.IsDie || !UnityVersionUtil.IsActive(curCharacter.gameObject))
		{
			return false;
		}
		if (curCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
		{
			ObjNPC objNPC = curCharacter as ObjNPC;
			if (objNPC.NPCFunctionType == GameDefine.NPC_FUNCTION_TYPE.CITIZEN_NPC)
			{
				return false;
			}
			if (objNPC.IsMissionNpc())
			{
				return false;
			}
		}
		else if (curCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER && !CampTool.ISPlayerCanAttack(MainPlayer, curCharacter as ObjOtherPlayer))
		{
			return false;
		}
		float num2 = VectorXZ.Distance(mMainPlayer.Position, curCharacter.Position);
		if (num2 < num)
		{
			return true;
		}
		return false;
	}

	public ObjCharacter FindCanAttackCharacter(int camp, Vector3 pos)
	{
		float num = 5f;
		float num2 = 100f;
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager != null && sceneManager.CurrentMapInofData != null)
		{
			num2 = sceneManager.CurrentMapInofData.AutoFightDis;
		}
		ObjCharacter result = null;
		List<ObjCharacter> list = mCampTargetList[camp];
		for (int i = 0; i < list.Count; i++)
		{
			ObjCharacter objCharacter = null;
			if (CheckCanAutoAttack(list[i]))
			{
				objCharacter = list[i];
				float num3 = VectorXZ.Distance(mMainPlayer.Position, objCharacter.Position);
				if (num3 < num2)
				{
					num2 = num3;
					result = objCharacter;
				}
			}
		}
		return result;
	}

	public bool CheckCanAutoAttack(ObjCharacter curCharacter)
	{
		if (curCharacter.IsDie)
		{
			return false;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (curCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
		{
			ObjNPC objNPC = curCharacter as ObjNPC;
			if (objNPC.NPCFunctionType == GameDefine.NPC_FUNCTION_TYPE.CITIZEN_NPC)
			{
				return false;
			}
			if (objNPC.IsMissionNpc())
			{
				return false;
			}
			if (playerData.NonMissionTarget == 0 && (sceneManager.IsBigWorld() || sceneManager.IsTutorialScene()) && !missionManager.IsCurMissionNeedNpc(objNPC))
			{
				return false;
			}
		}
		else if (curCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
		{
			if (!CampTool.ISPlayerCanAttack(MainPlayer, curCharacter as ObjOtherPlayer))
			{
				return false;
			}
		}
		else if ((curCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC_CAR || curCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_PLAYER_CAR) && playerData.VehicleTarget == 0 && (sceneManager.IsBigWorld() || sceneManager.IsTutorialScene()) && !missionManager.IsHaveDestroyCarMission())
		{
			return false;
		}
		return true;
	}

	public ObjPlayerCar CreateMainPlayerCar(ObjCarInitData initData)
	{
		if (mMainPlayerCar != null)
		{
			return null;
		}
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/CarRoot") as GameObject;
		gameObject.name = $"PlayerCar";
		gameObject.tag = "PlayerCar";
		ObjPlayerCar objPlayerCar = gameObject.GetComponent<ObjPlayerCar>();
		if (objPlayerCar == null)
		{
			objPlayerCar = gameObject.AddComponent<ObjPlayerCar>();
			objPlayerCar.InitCar();
		}
		objPlayerCar.ResetPlayerCar(initData);
		mMainPlayerCar = objPlayerCar;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		RideMountData rideMountData = new RideMountData();
		rideMountData.MountData = DataManager.GetMountDataById(playerData.MountId);
		rideMountData.mColorData = DataManager.GetColorDataById(playerData.MountColor);
		rideMountData.Player = MainPlayer;
		rideMountData.PlayerCar = mMainPlayerCar;
		ObjPlayerMountCar mountCar = GetMountCar(rideMountData);
		if (mountCar == null)
		{
			Debug.Log("mountCar == null !!!!!!!!!!!!!!!!!!!!!!!!!");
		}
		if (objPlayerCar == null)
		{
			Debug.Log("playerCar == null !!!!!!!!!!!!!!!!!!!!!!!!!");
		}
		if (objPlayerCar.MeshRoot == null)
		{
			Debug.Log("playerCar.MeshRoot == null !!!!!!!!!!!!!!!!!!!!!!!!!");
		}
		mountCar.transform.parent = objPlayerCar.MeshRoot.transform;
		mountCar.transform.localPosition = Vector3.zero;
		mountCar.transform.localRotation = Quaternion.identity;
		return objPlayerCar;
	}

	public ObjPlayerCar CreateTutorialPlayerCar(ObjCarInitData initData)
	{
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/TutorialCarRoot") as GameObject;
		gameObject.name = $"PlayerCar";
		gameObject.tag = "PlayerCar";
		GameObject gameObject2 = null;
		CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(initData.CharacterModelId);
		if (characterModelDataByID != null)
		{
			string path = $"TestModel/CarModel/{characterModelDataByID.Name}";
			gameObject2 = ReloadModel(gameObject.transform, path);
			gameObject2.gameObject.name = $"MeshRoot";
		}
		ObjPlayerCar objPlayerCar = gameObject.GetComponent<ObjPlayerCar>();
		if (objPlayerCar == null)
		{
			objPlayerCar = gameObject.AddComponent<ObjPlayerCar>();
			objPlayerCar.InitCar();
		}
		objPlayerCar.MeshRoot = gameObject2;
		GameObject gameObject3 = gameObject2.transform.FindChild("cheshen").gameObject;
		gameObject3.layer = LayerMask.NameToLayer("PlayerCar");
		gameObject3.tag = "PlayerCar";
		BoxCollider component = gameObject3.GetComponent<BoxCollider>();
		GameObject gameObject4 = gameObject2.transform.FindChild("FrontCollision").gameObject;
		gameObject4.layer = gameObject3.layer;
		gameObject4.tag = gameObject3.tag;
		GameObject gameObject5 = new GameObject("PlayerCarCollider");
		gameObject5.transform.parent = gameObject3.transform.parent;
		gameObject5.transform.localPosition = gameObject3.transform.localPosition;
		gameObject5.transform.localRotation = gameObject3.transform.localRotation;
		BoxCollider boxCollider = gameObject5.AddComponent<BoxCollider>();
		boxCollider.center = component.center + Vector3.forward * 1.5f / 2f;
		boxCollider.size = component.size + Vector3.forward * 1.5f;
		boxCollider.isTrigger = true;
		gameObject5.layer = gameObject3.layer;
		gameObject5.tag = gameObject3.tag;
		objPlayerCar.ResetPlayerCar(initData);
		return objPlayerCar;
	}

	public ObjPlayerCar CreateTutorialAICar(ObjCarInitData initData)
	{
		GameObject gameObject = null;
		GameObject gameObject2 = null;
		ObjPlayerCar objPlayerCar = null;
		if (initData.CarMountData == null)
		{
			return null;
		}
		if (GameManager.IsSupportCurDataVersion167() || !SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
		{
			ModelData modeDataByID = DataManager.GetModeDataByID(initData.CarMountData.ModelId);
			if (modeDataByID == null)
			{
				return null;
			}
			if (initData.CarMountData.IsShowPlayer == 1)
			{
				gameObject = ResourcesManager.LoadAndInstantiate("TestModel/TutorialCarRoot") as GameObject;
				objPlayerCar = gameObject.GetComponent<ObjPlayerCar>();
				if (objPlayerCar == null)
				{
					objPlayerCar = gameObject.AddComponent<ObjPlayerCar>();
				}
				ModelData modeDataByID2 = DataManager.GetModeDataByID(initData.CarMountData.ModelId);
				BundleManager.LoadModelInList(modeDataByID2.Name, isNeedUnload: false, isDoNotCache: false, OnLoadTutorialPlayerCarFinish, initData, objPlayerCar, modeDataByID2.ModelPath);
				return objPlayerCar;
			}
			string path = $"TestModel/CarModel/{modeDataByID.Name}";
			gameObject2 = ResourcesManager.LoadAndInstantiate(path) as GameObject;
			if (gameObject2 == null)
			{
				if (GameManager.IsSupportCurDataVersion177())
				{
					gameObject = ResourcesManager.LoadAndInstantiate("TestModel/TutorialCarRoot") as GameObject;
					objPlayerCar = gameObject.GetComponent<ObjPlayerCar>();
					if (objPlayerCar == null)
					{
						objPlayerCar = gameObject.AddComponent<ObjPlayerCar>();
					}
					ModelData modeDataByID3 = DataManager.GetModeDataByID(initData.CarMountData.ModelId);
					SingletonDontDestoryUnity<GameManager>.Instance.StartCoroutine(BundleManager.LoadItem(modeDataByID3.Name, OnLoadTutorialPlayerCarFinishFromItem, initData, objPlayerCar));
					return objPlayerCar;
				}
				return null;
			}
			gameObject2.gameObject.name = $"MeshRoot";
		}
		else
		{
			CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(initData.CarMountData.ID);
			if (characterModelDataByID == null)
			{
				return null;
			}
			if (initData.CarMountData.IsShowPlayer == 1)
			{
				gameObject = ResourcesManager.LoadAndInstantiate("TestModel/TutorialCarRoot") as GameObject;
				objPlayerCar = gameObject.GetComponent<ObjPlayerCar>();
				if (objPlayerCar == null)
				{
					objPlayerCar = gameObject.AddComponent<ObjPlayerCar>();
				}
				ModelData modeDataByID4 = DataManager.GetModeDataByID(initData.CarMountData.ModelId);
				BundleManager.LoadModelInList(modeDataByID4.Name, isNeedUnload: false, isDoNotCache: false, OnLoadTutorialPlayerCarFinish, initData, objPlayerCar, modeDataByID4.ModelPath);
				return objPlayerCar;
			}
			string path2 = $"TestModel/CarModel/{characterModelDataByID.Name}";
			gameObject2 = ResourcesManager.LoadAndInstantiate(path2) as GameObject;
			if (gameObject2 == null)
			{
				if (GameManager.IsSupportCurDataVersion177())
				{
					gameObject = ResourcesManager.LoadAndInstantiate("TestModel/TutorialCarRoot") as GameObject;
					objPlayerCar = gameObject.GetComponent<ObjPlayerCar>();
					if (objPlayerCar == null)
					{
						objPlayerCar = gameObject.AddComponent<ObjPlayerCar>();
					}
					ModelData modeDataByID5 = DataManager.GetModeDataByID(initData.CarMountData.ModelId);
					SingletonDontDestoryUnity<GameManager>.Instance.StartCoroutine(BundleManager.LoadItem(modeDataByID5.Name, OnLoadTutorialPlayerCarFinishFromItem, initData, objPlayerCar));
					return objPlayerCar;
				}
				return null;
			}
			gameObject2.gameObject.name = $"MeshRoot";
		}
		gameObject = ResourcesManager.LoadAndInstantiate("TestModel/TutorialCarRoot") as GameObject;
		objPlayerCar = gameObject.GetComponent<ObjPlayerCar>();
		if (objPlayerCar == null)
		{
			objPlayerCar = gameObject.AddComponent<ObjPlayerCar>();
		}
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.transform.localScale = Vector3.one;
		objPlayerCar.MeshRoot = gameObject2;
		objPlayerCar.InitCar();
		GameObject gameObject3 = gameObject2.transform.FindChild("cheshen").gameObject;
		BoxCollider component = gameObject3.GetComponent<BoxCollider>();
		GameObject gameObject4 = gameObject2.transform.FindChild("FrontCollision").gameObject;
		gameObject4.layer = gameObject3.layer;
		gameObject4.tag = gameObject3.tag;
		GameObject gameObject5 = new GameObject("PlayerCarCollider");
		gameObject5.transform.parent = gameObject3.transform.parent;
		gameObject5.transform.localPosition = gameObject3.transform.localPosition;
		gameObject5.transform.localRotation = gameObject3.transform.localRotation;
		BoxCollider boxCollider = gameObject5.AddComponent<BoxCollider>();
		boxCollider.center = component.center + Vector3.forward * 1.5f / 2f;
		boxCollider.size = component.size + Vector3.forward * 1.5f;
		boxCollider.isTrigger = true;
		gameObject5.layer = gameObject3.layer;
		gameObject5.tag = gameObject3.tag;
		objPlayerCar.ResetPlayerCar(initData);
		return objPlayerCar;
	}

	private void OnLoadTutorialPlayerCarFinishFromItem(string name, UnityEngine.Object modelBundle, object param1 = null, object param2 = null)
	{
		ObjCarInitData objCarInitData = param1 as ObjCarInitData;
		ObjPlayerCar objPlayerCar = param2 as ObjPlayerCar;
		MountData carMountData = objCarInitData.CarMountData;
		GameObject gameObject = UnityEngine.Object.Instantiate(modelBundle) as GameObject;
		if (gameObject == null)
		{
			Log.ERROR_MSG("Instantiate Bundle Error");
			return;
		}
		BundleManager.ResetShader(gameObject.transform);
		UnityVersionUtil.SetActiveRecursive(gameObject, state: true);
		gameObject.gameObject.name = "MeshRoot";
		gameObject.transform.parent = objPlayerCar.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.animation.cullingType = AnimationCullingType.BasedOnRenderers;
		objPlayerCar.MeshRoot = gameObject;
		objPlayerCar.InitCar();
		GameObject gameObject2 = gameObject.transform.FindChild("cheshen").gameObject;
		BoxCollider component = gameObject2.GetComponent<BoxCollider>();
		GameObject gameObject3 = gameObject.transform.FindChild("FrontCollision").gameObject;
		gameObject3.layer = gameObject2.layer;
		gameObject3.tag = gameObject2.tag;
		GameObject gameObject4 = new GameObject("PlayerCarCollider");
		gameObject4.transform.parent = gameObject2.transform.parent;
		gameObject4.transform.localPosition = gameObject2.transform.localPosition;
		gameObject4.transform.localRotation = gameObject2.transform.localRotation;
		BoxCollider boxCollider = gameObject4.AddComponent<BoxCollider>();
		boxCollider.center = component.center + Vector3.forward * 1.5f / 2f;
		boxCollider.size = component.size + Vector3.forward * 1.5f;
		boxCollider.isTrigger = true;
		gameObject4.layer = gameObject2.layer;
		gameObject4.tag = gameObject2.tag;
		objPlayerCar.ResetPlayerCar(objCarInitData);
		if (carMountData.IsShowPlayer == 1)
		{
			objPlayerCar.UpdateColor();
		}
		ObjFakeAICar component2 = objPlayerCar.gameObject.GetComponent<ObjFakeAICar>();
		if (component2 != null)
		{
			component2.InitCar();
			component2.ResetStaticCar();
		}
	}

	private void OnLoadTutorialPlayerCarFinish(object modelBundle, object param1, object param2)
	{
		ObjCarInitData objCarInitData = param1 as ObjCarInitData;
		ObjPlayerCar objPlayerCar = param2 as ObjPlayerCar;
		MountData carMountData = objCarInitData.CarMountData;
		GameObject gameObject = modelBundle as GameObject;
		if (gameObject == null)
		{
			Log.ERROR_MSG("Instantiate Bundle Error");
			return;
		}
		BundleManager.ResetShader(gameObject.transform);
		UnityVersionUtil.SetActiveRecursive(gameObject, state: true);
		gameObject.gameObject.name = "MeshRoot";
		gameObject.transform.parent = objPlayerCar.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.animation.cullingType = AnimationCullingType.BasedOnRenderers;
		objPlayerCar.MeshRoot = gameObject;
		objPlayerCar.InitCar();
		GameObject gameObject2 = gameObject.transform.FindChild("cheshen").gameObject;
		BoxCollider component = gameObject2.GetComponent<BoxCollider>();
		GameObject gameObject3 = gameObject.transform.FindChild("FrontCollision").gameObject;
		gameObject3.layer = gameObject2.layer;
		gameObject3.tag = gameObject2.tag;
		GameObject gameObject4 = new GameObject("PlayerCarCollider");
		gameObject4.transform.parent = gameObject2.transform.parent;
		gameObject4.transform.localPosition = gameObject2.transform.localPosition;
		gameObject4.transform.localRotation = gameObject2.transform.localRotation;
		BoxCollider boxCollider = gameObject4.AddComponent<BoxCollider>();
		boxCollider.center = component.center + Vector3.forward * 1.5f / 2f;
		boxCollider.size = component.size + Vector3.forward * 1.5f;
		boxCollider.isTrigger = true;
		gameObject4.layer = gameObject2.layer;
		gameObject4.tag = gameObject2.tag;
		objPlayerCar.ResetPlayerCar(objCarInitData);
		if (carMountData.IsShowPlayer == 1)
		{
			objPlayerCar.UpdateColor();
		}
		ObjFakeAICar component2 = objPlayerCar.gameObject.GetComponent<ObjFakeAICar>();
		if (component2 != null)
		{
			component2.InitCar();
			component2.ResetStaticCar();
		}
	}

	private void ResetObjFakeAICarPoolGroup()
	{
		mObjFakeAICarPoolGroup = new SimplePoolGroup<ObjFakeAICar>();
		mObjFakeAICarPoolGroup.Reset(CreateObjFakeAICar, DestroyObjFakeAICar, 10);
	}

	public void ClearFakeCar()
	{
		for (int i = 0; i < mObjFakeAICarPoolGroup.EnableList.Count; i++)
		{
			RemoveDict(mObjFakeAICarPoolGroup.EnableList[i].PlayerCar.ServerId);
		}
		mObjFakeAICarPoolGroup.Clear();
	}

	private ObjFakeAICar CreateObjFakeAICar(object data)
	{
		ObjCarInitData objCarInitData = data as ObjCarInitData;
		ObjPlayerCar objPlayerCar = CreateTutorialAICar(objCarInitData);
		if (objPlayerCar == null)
		{
			return null;
		}
		objPlayerCar.gameObject.name = "FakeAICar" + tt++;
		ObjFakeAICar objFakeAICar = objPlayerCar.gameObject.GetComponent<ObjFakeAICar>();
		if (objFakeAICar == null)
		{
			objFakeAICar = objPlayerCar.gameObject.AddComponent<ObjFakeAICar>();
		}
		objFakeAICar.ModelId = objCarInitData.CharacterModelId;
		return objFakeAICar;
	}

	private void DestroyObjFakeAICar(ObjFakeAICar aiCar)
	{
		if (aiCar != null)
		{
			UnityEngine.Object.Destroy(aiCar.gameObject);
		}
	}

	public ObjFakeAICar GetFakeAICar(ObjCarInitData initData, bool isFriendCar = false)
	{
		ObjFakeAICar objFakeAICar = mObjFakeAICarPoolGroup.Get(initData.CharacterModelId, initData);
		if (objFakeAICar == null)
		{
			return null;
		}
		if (initData.CarMountData.IsShowPlayer == 0 && objFakeAICar.PlayerCar.MeshRoot == null)
		{
			UnityVersionUtil.SetActiveRecursive(objFakeAICar.gameObject, state: false);
			mObjFakeAICarPoolGroup.Recycle(objFakeAICar, initData.CharacterModelId);
			return null;
		}
		if (objFakeAICar.PlayerCar.MeshRoot != null)
		{
			objFakeAICar.PlayerCar.ResetPlayerCar(initData);
		}
		else
		{
			objFakeAICar.PlayerCar.BeforeLoadMeshReset(initData);
		}
		objFakeAICar.ServerId = initData.ServerID;
		UnityVersionUtil.SetActiveRecursive(objFakeAICar.gameObject, state: true);
		if (isFriendCar)
		{
			objFakeAICar.PlayerCar.AttributeData.Camp = GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC;
		}
		else
		{
			objFakeAICar.PlayerCar.AttributeData.Camp = GameDefine.CAMP_TYPE.NORMAL_NPC;
		}
		AddDict(initData.ServerID, objFakeAICar.PlayerCar);
		AddToTargetCampList(objFakeAICar.PlayerCar);
		return objFakeAICar;
	}

	public void RecycleFakeAICar(ObjFakeAICar aiCar)
	{
		if (SingletonUnity<CitySimController>.Exists)
		{
			SingletonUnity<CitySimController>.Instance.OnRecycleFakeAICar(aiCar);
		}
		SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.OnRecycleTargetCar(aiCar);
		RemoveDict(aiCar.PlayerCar.ServerId);
		RemoveFromTargetCampList(aiCar.PlayerCar);
		mObjFakeAICarPoolGroup.Recycle(aiCar, aiCar.ModelId);
	}

	private void ResetObjSimpleAICarPoolGroup()
	{
		mObjSimpleAICarPoolGroup = new SimplePoolGroup<ObjSimpleAICar>();
		mObjSimpleAICarPoolGroup.Reset(CreateObjSimpleAICar, DestroyObjSimpleAICar, 10);
	}

	public void ClearAICar()
	{
		mObjSimpleAICarPoolGroup.Clear();
	}

	private ObjSimpleAICar CreateObjSimpleAICar(object data)
	{
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/TutorialCarRoot") as GameObject;
		gameObject.name = $"AICar";
		gameObject.tag = "PoliceCar";
		ObjCarInitData objCarInitData = data as ObjCarInitData;
		GameObject gameObject2 = null;
		CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(objCarInitData.CharacterModelId);
		if (characterModelDataByID != null)
		{
			string path = $"TestModel/CarModel/{characterModelDataByID.Name}";
			gameObject2 = ReloadModel(gameObject.transform, path);
			gameObject2.gameObject.name = $"MeshRoot";
		}
		ObjSimpleAICar objSimpleAICar = gameObject.GetComponent<ObjSimpleAICar>();
		if (objSimpleAICar == null)
		{
			objSimpleAICar = gameObject.AddComponent<ObjSimpleAICar>();
		}
		objSimpleAICar.MeshRoot = gameObject2;
		GameObject gameObject3 = gameObject2.transform.FindChild("cheshen").gameObject;
		gameObject3.layer = LayerMask.NameToLayer("PlayerCar");
		gameObject3.tag = "PoliceCar";
		BoxCollider component = gameObject3.GetComponent<BoxCollider>();
		GameObject gameObject4 = gameObject2.transform.FindChild("FrontCollision").gameObject;
		gameObject4.layer = gameObject3.layer;
		gameObject4.tag = gameObject3.tag;
		GameObject gameObject5 = new GameObject("PlayerCarCollider");
		gameObject5.transform.parent = gameObject3.transform.parent;
		gameObject5.transform.localPosition = gameObject3.transform.localPosition;
		gameObject5.transform.localRotation = gameObject3.transform.localRotation;
		BoxCollider boxCollider = gameObject5.AddComponent<BoxCollider>();
		boxCollider.center = component.center + Vector3.forward * 1.5f / 2f;
		boxCollider.size = component.size + Vector3.forward * 1.5f;
		boxCollider.isTrigger = true;
		gameObject5.layer = gameObject3.layer;
		gameObject5.tag = gameObject3.tag;
		return objSimpleAICar;
	}

	private void DestroyObjSimpleAICar(ObjSimpleAICar aiCar)
	{
		UnityEngine.Object.Destroy(aiCar.gameObject);
	}

	public ObjSimpleAICar GetSimpleAICar(ObjCarInitData initData)
	{
		return mObjSimpleAICarPoolGroup.Get(initData.CharacterModelId, initData);
	}

	public void RecycleSimpleAICar(ObjSimpleAICar aiCar)
	{
		mObjSimpleAICarPoolGroup.Recycle(aiCar, aiCar.ModelId);
	}

	public void StopPoliceSound()
	{
		int num = 0;
		for (int i = 0; i < mObjSimpleAICarPoolGroup.EnableList.Count; i++)
		{
			if (!mObjSimpleAICarPoolGroup.EnableList[i].IsChaseDone)
			{
				num++;
			}
		}
		if (num <= 0 && SingletonDontDestoryUnity<SoundManager>.Exists)
		{
			SingletonDontDestoryUnity<SoundManager>.Instance.StopSoundEffect(mPoliceSoundId);
		}
	}

	public void StopAllPoliceSound()
	{
		SingletonDontDestoryUnity<SoundManager>.Instance.StopSoundEffect(mPoliceSoundId);
	}

	private void ResetObjRagdollNPCPoolGroup()
	{
		mObjRagdollNPCPoolGroup = new SimplePoolGroup<ObjRagdollNPC>();
		mObjRagdollNPCPoolGroup.Reset(CreateRagdollNPC, DestroyRagdollNPC, 20);
	}

	private void ClearRagdollNPCPool()
	{
		mObjRagdollNPCPoolGroup.Clear();
	}

	private ObjRagdollNPC CreateRagdollNPC(object data)
	{
		ObjInitNpcData objInitNpcData = data as ObjInitNpcData;
		CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(objInitNpcData.npcInfoData.Model);
		if (characterModelDataByID != null)
		{
			GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/NPCRoot") as GameObject;
			ObjRagdollNPC objRagdollNPC = null;
			if (gameObject != null)
			{
				gameObject.name = "NPC" + objInitNpcData.npcInfoData.ID + "_" + objInitNpcData.mServerID;
				objRagdollNPC = gameObject.GetComponent<ObjRagdollNPC>();
				if (objRagdollNPC == null)
				{
					objRagdollNPC = gameObject.AddComponent<ObjRagdollNPC>();
				}
				objRagdollNPC.InitInfo(characterModelDataByID);
				objRagdollNPC.Init();
				UnityVersionUtil.SetActiveRecursive(objRagdollNPC.gameObject, state: true);
			}
			objRagdollNPC.LoadingModelData = BundleManager.LoadModelInList(characterModelDataByID.Name, isNeedUnload: true, isDoNotCache: false, OnLoadRagdollNpcModelFinished, objInitNpcData, objRagdollNPC);
			objRagdollNPC.LoadingModelDataId = ((objRagdollNPC.LoadingModelData != null) ? objRagdollNPC.LoadingModelData.ID : (-1));
			return objRagdollNPC;
		}
		Debug.Log("characterModelData == null " + objInitNpcData.npcInfoData.Model);
		return null;
	}

	private void OnLoadRagdollNpcModelFinished(object modelBundle, object param1, object param2)
	{
		ObjInitNpcData objInitNpcData = param1 as ObjInitNpcData;
		ObjRagdollNPC objRagdollNPC = param2 as ObjRagdollNPC;
		GameObject gameObject = modelBundle as GameObject;
		if (gameObject == null)
		{
			Log.ERROR_MSG("Instantiate Bundle Error");
			return;
		}
		gameObject.gameObject.name = $"MeshRoot";
		Material sm = null;
		BundleManager.ResetShader(gameObject.transform, out sm);
		objRagdollNPC.MeshMat = sm;
		gameObject.transform.parent = objRagdollNPC.CacheTransform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		objRagdollNPC.LoadModelFinishInit();
		objRagdollNPC.LoadingModelData = null;
		objRagdollNPC.LoadingModelDataId = -1L;
		objRagdollNPC.MeshRoot = gameObject;
		gameObject.transform.localScale = Vector3.one * objInitNpcData.npcInfoData.ModelScale;
		if (objRagdollNPC.enabled)
		{
			UnityVersionUtil.SetActiveRecursive(gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(gameObject, state: false);
		}
		ObjRagdollNPC.InitRagdollObj(objRagdollNPC.AnimationLogic.AnimaObj.transform, objRagdollNPC);
	}

	private void DestroyRagdollNPC(ObjRagdollNPC objNpc)
	{
		if (objNpc.LoadingModelData != null)
		{
			objNpc.LoadingModelData.OnLoadFinished = null;
			objNpc.LoadingModelData = null;
			BundleManager.RemoveFromLoadModelList(objNpc.LoadingModelDataId);
			objNpc.LoadingModelDataId = -1L;
		}
		if (objNpc.MeshRoot != null)
		{
			BundleManager.UnloadModel(objNpc.CurrentCharacterModelData.ID, -1L, isMainPlayerUnload: false);
		}
		if (objNpc != null && objNpc.gameObject != null)
		{
			UnityEngine.Object.Destroy(objNpc.gameObject);
		}
		objNpc = null;
	}

	public ObjRagdollNPC GetRagdollNPC(ObjInitNpcData initData, OnGetNPC func)
	{
		ObjRagdollNPC objRagdollNPC = mObjRagdollNPCPoolGroup.Get(initData.mCharacterModelId, initData);
		if (objRagdollNPC != null)
		{
			objRagdollNPC.ResetNpc(initData);
			if (objRagdollNPC.LoadingModelData == null && objRagdollNPC.MeshRoot == null)
			{
				objRagdollNPC.LoadingModelData = BundleManager.LoadModelInList(objRagdollNPC.CurrentCharacterModelData.Name, isNeedUnload: true, isDoNotCache: false, OnLoadRagdollNpcModelFinished, initData, objRagdollNPC);
				objRagdollNPC.LoadingModelDataId = ((objRagdollNPC.LoadingModelData != null) ? objRagdollNPC.LoadingModelData.ID : (-1));
			}
		}
		else
		{
			SimplePool<ObjRagdollNPC> value = null;
			if (mObjRagdollNPCPoolGroup.ObjPoolDic.TryGetValue(initData.mCharacterModelId, out value) && value.EnableObjList.Count > 0)
			{
				value.EnableObjList[0].RecycleSelf();
				objRagdollNPC = mObjRagdollNPCPoolGroup.Get(initData.mCharacterModelId, initData);
				if (objRagdollNPC != null)
				{
					objRagdollNPC.ResetNpc(initData);
					if (objRagdollNPC.LoadingModelData == null && objRagdollNPC.MeshRoot == null)
					{
						objRagdollNPC.LoadingModelData = BundleManager.LoadModelInList(objRagdollNPC.CurrentCharacterModelData.Name, isNeedUnload: true, isDoNotCache: false, OnLoadRagdollNpcModelFinished, initData, objRagdollNPC);
						objRagdollNPC.LoadingModelDataId = ((objRagdollNPC.LoadingModelData != null) ? objRagdollNPC.LoadingModelData.ID : (-1));
					}
				}
			}
		}
		func = (OnGetNPC)Delegate.Combine(func, new OnGetNPC(OnNpcGet));
		if (objRagdollNPC != null)
		{
			func?.Invoke(objRagdollNPC);
		}
		return objRagdollNPC;
	}

	public void RecycleRagdollNPC(ObjRagdollNPC npc)
	{
		if (SingletonUnity<CitySimController>.Exists)
		{
			SingletonUnity<CitySimController>.Instance.OnRecycleNpc(npc);
		}
		SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.OnRecycleNpc(npc);
		OnNpcRecycle(npc);
		ResourcesManager.UnLoadSimpleShadowPrefab(npc.SimpleShadow);
		mObjRagdollNPCPoolGroup.Recycle(npc, npc.CurrentCharacterModelData.ID);
		if (npc != null && npc.LoadingModelData != null)
		{
			npc.LoadingModelData.OnLoadFinished = null;
			npc.LoadingModelData = null;
		}
	}

	private void ResetObjPatrolNPCPoolGroup()
	{
		mObjPatrolNPCPoolGroup = new SimplePoolGroup<ObjPatrolNPC>();
		mObjPatrolNPCPoolGroup.Reset(CreatePatrolNPC, DestroyPatrolNPC, 50);
	}

	private void ClearPatrolNPCPool()
	{
		mObjPatrolNPCPoolGroup.Clear();
	}

	private ObjPatrolNPC CreatePatrolNPC(object data)
	{
		ObjInitNpcData objInitNpcData = data as ObjInitNpcData;
		CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(objInitNpcData.npcInfoData.Model);
		if (characterModelDataByID != null)
		{
			GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/NPCRoot") as GameObject;
			ObjPatrolNPC objPatrolNPC = null;
			if (gameObject != null)
			{
				gameObject.name = "NPC" + objInitNpcData.mServerID;
				objPatrolNPC = gameObject.GetComponent<ObjPatrolNPC>();
				if (objPatrolNPC == null)
				{
					objPatrolNPC = gameObject.AddComponent<ObjPatrolNPC>();
				}
				objPatrolNPC.InitInfo(characterModelDataByID);
				objPatrolNPC.Init();
				UnityVersionUtil.SetActiveRecursive(objPatrolNPC.gameObject, state: true);
				objPatrolNPC.ResetNpc(objInitNpcData);
			}
			objPatrolNPC.LoadingModelData = BundleManager.LoadModelInList(characterModelDataByID.Name, isNeedUnload: true, isDoNotCache: false, OnLoadNpcModelFinished, objInitNpcData, objPatrolNPC);
			objPatrolNPC.LoadingModelDataId = ((objPatrolNPC.LoadingModelData != null) ? objPatrolNPC.LoadingModelData.ID : (-1));
			return objPatrolNPC;
		}
		Debug.Log("characterModelData == null " + objInitNpcData.npcInfoData.Model);
		return null;
	}

	private void OnLoadNpcModelFinished(object modelBundle, object param1, object param2)
	{
		ObjInitNpcData objInitNpcData = param1 as ObjInitNpcData;
		ObjNPC objNPC = param2 as ObjNPC;
		objNPC.LoadingModelData = null;
		objNPC.LoadingModelDataId = -1L;
		GameObject gameObject = modelBundle as GameObject;
		if (gameObject == null)
		{
			Log.ERROR_MSG("Instantiate Bundle Error");
			return;
		}
		gameObject.gameObject.name = $"MeshRoot";
		Material sm = null;
		BundleManager.ResetShader(gameObject.transform, out sm);
		gameObject.transform.parent = objNPC.CacheTransform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		objNPC.LoadModelFinishInit();
		objNPC.MeshRoot = gameObject;
		gameObject.transform.localScale = Vector3.one * objInitNpcData.npcInfoData.ModelScale;
		if (objNPC.enabled)
		{
			UnityVersionUtil.SetActiveRecursive(gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(gameObject, state: false);
		}
	}

	private void DestroyPatrolNPC(ObjPatrolNPC objNpc)
	{
		if (objNpc.LoadingModelData != null)
		{
			objNpc.LoadingModelData.OnLoadFinished = null;
			objNpc.LoadingModelData = null;
			BundleManager.RemoveFromLoadModelList(objNpc.LoadingModelDataId);
			objNpc.LoadingModelDataId = -1L;
		}
		if (objNpc.MeshRoot != null)
		{
			BundleManager.UnloadModel(objNpc.CurrentCharacterModelData.ID, -1L, isMainPlayerUnload: false);
		}
		UnityEngine.Object.Destroy(objNpc.gameObject);
		objNpc = null;
	}

	public ObjPatrolNPC GetPatrolNPC(ObjInitNpcData initData, OnGetNPC func)
	{
		ObjPatrolNPC objPatrolNPC = mObjPatrolNPCPoolGroup.Get(initData.mCharacterModelId, initData);
		if (objPatrolNPC != null)
		{
			objPatrolNPC.ResetNpc(initData);
			if (objPatrolNPC.LoadingModelData == null && objPatrolNPC.MeshRoot == null)
			{
				objPatrolNPC.LoadingModelData = BundleManager.LoadModelInList(objPatrolNPC.CurrentCharacterModelData.Name, isNeedUnload: true, isDoNotCache: false, OnLoadNpcModelFinished, initData, objPatrolNPC);
				objPatrolNPC.LoadingModelDataId = ((objPatrolNPC.LoadingModelData != null) ? objPatrolNPC.LoadingModelData.ID : (-1));
			}
		}
		else
		{
			SimplePool<ObjPatrolNPC> value = null;
			if (mObjPatrolNPCPoolGroup.ObjPoolDic.TryGetValue(initData.mCharacterModelId, out value) && value.EnableObjList.Count > 0)
			{
				value.EnableObjList[0].RecycleSelf();
				objPatrolNPC = mObjPatrolNPCPoolGroup.Get(initData.mCharacterModelId, initData);
				if (objPatrolNPC != null)
				{
					objPatrolNPC.ResetNpc(initData);
					if (objPatrolNPC.LoadingModelData == null && objPatrolNPC.MeshRoot == null)
					{
						objPatrolNPC.LoadingModelData = BundleManager.LoadModelInList(objPatrolNPC.CurrentCharacterModelData.Name, isNeedUnload: true, isDoNotCache: false, OnLoadNpcModelFinished, initData, objPatrolNPC);
						objPatrolNPC.LoadingModelDataId = ((objPatrolNPC.LoadingModelData != null) ? objPatrolNPC.LoadingModelData.ID : (-1));
					}
				}
			}
		}
		if (objPatrolNPC == null)
		{
			Debug.Log("NO NPC!!!!!!!!!!!!!!!!!!!!!!!");
		}
		if (objPatrolNPC != null)
		{
			func?.Invoke(objPatrolNPC);
		}
		return objPatrolNPC;
	}

	public void RecyclePatrolNPC(ObjPatrolNPC npc)
	{
		mObjPatrolNPCPoolGroup.Recycle(npc, npc.CurrentCharacterModelData.ID);
		if (npc != null && npc.LoadingModelData != null)
		{
			npc.LoadingModelData.OnLoadFinished = null;
			npc.LoadingModelData = null;
		}
	}

	public ObjShiftNPC GetShiftNPC(ObjInitNpcData initData, OnGetNPC func)
	{
		CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(initData.npcInfoData.Model);
		if (characterModelDataByID != null)
		{
			GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/NPCRoot") as GameObject;
			ObjShiftNPC objShiftNPC = null;
			if (gameObject != null)
			{
				gameObject.name = "NPC" + initData.mServerID;
				objShiftNPC = gameObject.GetComponent<ObjShiftNPC>();
				if (objShiftNPC == null)
				{
					objShiftNPC = gameObject.AddComponent<ObjShiftNPC>();
				}
				objShiftNPC.InitInfo(characterModelDataByID);
				objShiftNPC.Init();
				UnityVersionUtil.SetActiveRecursive(objShiftNPC.gameObject, state: true);
				objShiftNPC.ResetNpc(initData);
			}
			objShiftNPC.LoadingModelData = BundleManager.LoadModelInList(characterModelDataByID.Name, isNeedUnload: true, isDoNotCache: false, OnLoadNpcModelFinished, initData, objShiftNPC);
			objShiftNPC.LoadingModelDataId = ((objShiftNPC.LoadingModelData != null) ? objShiftNPC.LoadingModelData.ID : (-1));
			AddDict(initData.mServerID, objShiftNPC);
			AddToTargetCampList(objShiftNPC);
			return objShiftNPC;
		}
		Debug.Log("characterModelData == null " + initData.npcInfoData.Model);
		return null;
	}

	private void ResetBombSustainedRangeObjPool()
	{
		mBombSustainedRangeObjPool = new SimplePool<BombSustainedRangeObj>();
		mBombSustainedRangeObjPool.Reset(CreateBombSustainedRangeObj, DestroyBombSustainedRangeObj, 10);
	}

	private BombSustainedRangeObj CreateBombSustainedRangeObj(object data)
	{
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("Items/SustainedBomb") as GameObject;
		return gameObject.GetComponent<BombSustainedRangeObj>();
	}

	private void DestroyBombSustainedRangeObj(BombSustainedRangeObj obj)
	{
		UnityEngine.Object.Destroy(obj.gameObject);
	}

	public BombSustainedRangeObj GetBombSustainedRangeObj()
	{
		BombSustainedRangeObj bombSustainedRangeObj = mBombSustainedRangeObjPool.Get();
		if (bombSustainedRangeObj == null && mBombSustainedRangeObjPool.EnableObjList.Count > 0)
		{
			bombSustainedRangeObj = mBombSustainedRangeObjPool.EnableObjList[0];
			mBombSustainedRangeObjPool.EnableObjList.Remove(bombSustainedRangeObj);
			mBombSustainedRangeObjPool.EnableObjList.Add(bombSustainedRangeObj);
		}
		return bombSustainedRangeObj;
	}

	public void RecycleBombSustainedRangeObj(BombSustainedRangeObj bomb)
	{
		mBombSustainedRangeObjPool.Recycle(bomb);
	}

	public void ClearBombPool()
	{
		mBombSustainedRangeObjPool.Clear();
	}

	public void ClearSceneBomb()
	{
		for (int num = mBombSustainedRangeObjPool.EnableObjList.Count - 1; num >= 0; num--)
		{
			mBombSustainedRangeObjPool.EnableObjList[num].OnRecycle();
		}
	}

	private void ResetMountCarPoolGroup()
	{
		mMountCarPoolGroup = new SimplePoolGroup<ObjPlayerMountCar>();
		mMountCarPoolGroup.Reset(CreateMountCar, DestroyMountCar, 20);
	}

	private void ClearMountCarPoolGroup()
	{
		mMountCarPoolGroup.Clear();
	}

	private ObjPlayerMountCar CreateMountCar(object data)
	{
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/MountCarRoot") as GameObject;
		ObjPlayerMountCar objPlayerMountCar = gameObject.GetComponent<ObjPlayerMountCar>();
		if (objPlayerMountCar == null)
		{
			objPlayerMountCar = gameObject.AddComponent<ObjPlayerMountCar>();
		}
		RideMountData rideMountData = data as RideMountData;
		bool isNeedUnload = !rideMountData.MountData.ID.Equals(mMainPlayer.MountId) || GameSettingData.IsBundleNeedUnload[GameSettingData.GetPhoneClass()];
		ModelData modeDataByID = DataManager.GetModeDataByID(rideMountData.MountData.ModelId);
		if (modeDataByID != null)
		{
			objPlayerMountCar.LoadingMeshData = BundleManager.LoadModelInList(modeDataByID.Name, isNeedUnload, isDoNotCache: false, OnLoadMountCarModelFinished, rideMountData, objPlayerMountCar, modeDataByID.ModelPath);
			objPlayerMountCar.LoadingModelDataId = ((objPlayerMountCar.LoadingMeshData != null) ? objPlayerMountCar.LoadingMeshData.ID : (-1));
		}
		else
		{
			Debug.Log("carModelData == null " + rideMountData.MountData.ModelId);
		}
		return objPlayerMountCar;
	}

	private void OnLoadMountCarModelFinished(object modelBundle, object param1, object param2)
	{
		RideMountData rideMountData = param1 as RideMountData;
		ObjPlayerMountCar objPlayerMountCar = param2 as ObjPlayerMountCar;
		ObjOtherPlayer player = rideMountData.Player;
		MountData mountData = rideMountData.MountData;
		GameObject gameObject = modelBundle as GameObject;
		if (gameObject == null)
		{
			Log.ERROR_MSG("Instantiate Bundle Error");
			return;
		}
		BundleManager.ResetAllShader(gameObject.transform);
		NGUITools.SetLayer(gameObject, objPlayerMountCar.gameObject.layer);
		gameObject.gameObject.name = $"CarMeshRoot";
		MountCarMeshRoot component = gameObject.GetComponent<MountCarMeshRoot>();
		objPlayerMountCar.LoadingMeshData = null;
		objPlayerMountCar.LoadingModelDataId = -1L;
		component.CarShadow.transform.localPosition = new Vector3(0f, rideMountData.MountData.ShadowHeight, 0f);
		objPlayerMountCar.OnMeshLoadDone(component, rideMountData);
	}

	private void DestroyMountCar(ObjPlayerMountCar objMount)
	{
		if (objMount.LoadingMeshData != null)
		{
			objMount.LoadingMeshData.OnLoadFinished = null;
			objMount.LoadingMeshData = null;
			BundleManager.RemoveFromLoadModelList(objMount.LoadingModelDataId);
			objMount.LoadingModelDataId = -1L;
		}
		if (objMount.MeshRoot != null)
		{
			BundleManager.UnloadModel(objMount.CurRideMountData.MountData.ModelId, -1L, isMainPlayerUnload: false);
		}
		UnityEngine.Object.Destroy(objMount.gameObject);
	}

	public ObjPlayerMountCar GetMountCar(RideMountData initData)
	{
		ObjPlayerMountCar objPlayerMountCar = mMountCarPoolGroup.Get(initData.MountData.ModelId, initData);
		if (objPlayerMountCar != null)
		{
			if (objPlayerMountCar.MeshRoot == null && objPlayerMountCar.LoadingMeshData == null)
			{
				bool isNeedUnload = !initData.MountData.ID.Equals(mMainPlayer.MountId) || GameSettingData.IsBundleNeedUnload[GameSettingData.GetPhoneClass()];
				ModelData modeDataByID = DataManager.GetModeDataByID(initData.MountData.ModelId);
				if (modeDataByID != null)
				{
					objPlayerMountCar.LoadingMeshData = BundleManager.LoadModelInList(modeDataByID.Name, isNeedUnload, isDoNotCache: false, OnLoadMountCarModelFinished, initData, objPlayerMountCar, modeDataByID.ModelPath);
					objPlayerMountCar.LoadingModelDataId = ((objPlayerMountCar.LoadingMeshData != null) ? objPlayerMountCar.LoadingMeshData.ID : (-1));
				}
				else
				{
					Debug.Log("carModelData == null " + initData.MountData.ModelId);
				}
			}
			objPlayerMountCar.Reset(initData);
		}
		else
		{
			Debug.Log("No Mount Car!!!!!!!!!!!!!!!!!!!!!!");
		}
		return objPlayerMountCar;
	}

	public void RecycleMountCar(ObjPlayerMountCar mountCar)
	{
		if (mountCar.LoadingMeshData != null)
		{
			mountCar.LoadingMeshData.OnLoadFinished = null;
			mountCar.LoadingMeshData = null;
		}
		if (mountCar.CurRideMountData != null && mountCar.CurRideMountData.Player != null)
		{
			mountCar.CurRideMountData.Player.LoadingMountFlag = false;
		}
		mountCar.ResetFlag = false;
		mMountCarPoolGroup.Recycle(mountCar, mountCar.CurRideMountData.MountData.ModelId);
	}

	public void RefreshPlayerGuildPic()
	{
		if (mMainPlayer != null)
		{
			mMainPlayer.RefreshHeadInfo();
		}
		for (int i = 0; i < mObjOtherPlayerVisibleList.Count; i++)
		{
			mObjOtherPlayerVisibleList[i].RefreshHeadInfo();
		}
	}

	public float GetActiveNpcHpPercent()
	{
		long num = 0L;
		long num2 = 0L;
		for (int i = 0; i < mObjNpcPoolGroup.EnableNpcList.Count; i++)
		{
			num += mObjNpcPoolGroup.EnableNpcList[i].AttributeData.MaxHP;
			num2 += mObjNpcPoolGroup.EnableNpcList[i].AttributeData.HP;
		}
		if (num != 0L)
		{
			return (float)num2 / (float)num;
		}
		return 0f;
	}
}
