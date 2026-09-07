using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

[Serializable]
public class FakeObjLogic
{
	public delegate void OnLoadFinishedDel(FakeObjLogic obj);

	private const string talkName = "talk";

	private OnLoadFinishedDel OnLoadFinished;

	public GameObject FakeObj;

	private Transform mFakeObjParent;

	private PlayerModelObjData mPlayerModelObjData = new PlayerModelObjData();

	private string mModelName;

	private string mWeaponId;

	private Animation mAnim;

	private FakeObjSkillShowLogic mShowSkillLogic;

	private string LayerStr = "FakeObj";

	private int mLoadPartNum;

	private int mLoadPartCount;

	private int mChangePartNum;

	private int mChangePartCount;

	private vp_Timer.Handle npcAnimaHandle = new vp_Timer.Handle();

	private string mCurNpcModelId = string.Empty;

	private BundleManager.LoadModelData mCurNpcLoadingModelData;

	private long mCurNpcLoadingModelDataId;

	public string CurModelName => mModelName;

	public Animation FakeObjAnim
	{
		get
		{
			if (mAnim == null)
			{
				mAnim = FakeObj.animation;
				if (mAnim == null)
				{
					mAnim = FakeObj.GetComponentInChildren<Animation>();
				}
			}
			return mAnim;
		}
	}

	public List<GameObject> GetBodyPartList()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < mPlayerModelObjData.PartModelObjData.Length; i++)
		{
			if (mPlayerModelObjData.PartModelObjData[i].ModelObj != null)
			{
				list.Add(mPlayerModelObjData.PartModelObjData[i].ModelObj);
			}
		}
		return list;
	}

	public void StopLoadMesh()
	{
		mPlayerModelObjData.StopLoadMesh();
		if (mCurNpcLoadingModelData != null)
		{
			mCurNpcLoadingModelData.OnLoadFinished = null;
			mCurNpcLoadingModelData = null;
		}
	}

	public void InitFakeObject(characterVisual visual, string profession, string modelName, Transform parentObj, OnLoadFinishedDel func = null, string layerName = "FakeObj")
	{
		string weaponId = ObjManager.GetWeaponId(visual);
		string headId = ObjManager.GetHeadId(visual);
		string bodyId = ObjManager.GetBodyId(visual);
		string legId = ObjManager.GetLegId(visual);
		InitFakeObject(weaponId, headId, bodyId, legId, profession, modelName, parentObj, func, layerName);
	}

	private void InitFakeObject(string weaponId, string headId, string bodyId, string legId, string profession, string modelName, Transform parentObj, OnLoadFinishedDel func = null, string layerName = "FakeObj")
	{
		LayerStr = layerName;
		if (FakeObj == null)
		{
			FakeObj = CreatePlayerModel(profession, weaponId, headId, bodyId, legId);
			mFakeObjParent = parentObj;
			FakeObj.transform.parent = mFakeObjParent;
			FakeObj.transform.localPosition = Vector3.zero;
			FakeObj.transform.localRotation = Quaternion.identity;
			FakeObj.transform.localScale = Vector3.one;
			mModelName = modelName;
			mWeaponId = weaponId;
			PlayAnim("idle", modelName);
			LoadPlayerVisual(FakeObj, weaponId, headId, bodyId, legId, func);
		}
		else
		{
			mModelName = modelName;
			mWeaponId = weaponId;
			CheckFakeObject(weaponId, headId, bodyId, legId, func);
		}
	}

	private void InitFakeObject(string weaponId, string headId, string bodyId, string legId, string profession, Transform parentObj, OnLoadFinishedDel func = null, string layerName = "FakeObj")
	{
		string empty = string.Empty;
		empty = (profession.Equals("XD") ? "baiRen" : ((!profession.Equals("QJ")) ? "nvRen" : "heiRen"));
		InitFakeObject(weaponId, headId, bodyId, legId, profession, empty, parentObj, func, layerName);
	}

	public void InitFakeObject(characterVisual visual, int profession, Transform parentObj, OnLoadFinishedDel func = null, string layerName = "FakeObj")
	{
		string weaponId = ObjManager.GetWeaponId(visual);
		string headId = ObjManager.GetHeadId(visual);
		string bodyId = ObjManager.GetBodyId(visual);
		string legId = ObjManager.GetLegId(visual);
		InitFakeObject(weaponId, headId, bodyId, legId, profession, parentObj, func, layerName);
	}

	public void InitFakeObject(characterVisual visual, PROFESSION_TYPE profession, Transform parentObj, OnLoadFinishedDel func = null, string layerName = "FakeObj")
	{
		string weaponId = ObjManager.GetWeaponId(visual);
		string headId = ObjManager.GetHeadId(visual);
		string bodyId = ObjManager.GetBodyId(visual);
		string legId = ObjManager.GetLegId(visual);
		InitFakeObject(weaponId, headId, bodyId, legId, profession, parentObj, func, layerName);
	}

	public void InitFakeObject(string weaponId, string headId, string bodyId, string legId, PROFESSION_TYPE profession, Transform parentObj, OnLoadFinishedDel func = null, string layerName = "FakeObj")
	{
		InitFakeObject(weaponId, headId, bodyId, legId, (int)profession, parentObj, func);
	}

	public void InitFakeObject(string weaponId, string headId, string bodyId, string legId, int profession, Transform parentObj, OnLoadFinishedDel func = null, string layerName = "FakeObj")
	{
		string empty = string.Empty;
		string empty2 = string.Empty;
		switch (profession)
		{
		case 0:
			empty = "baiRen";
			empty2 = "XD";
			break;
		case 1:
			empty = "heiRen";
			empty2 = "QJ";
			break;
		default:
			empty = "nvRen";
			empty2 = "NQS";
			break;
		}
		mWeaponId = weaponId;
		InitFakeObject(weaponId, headId, bodyId, legId, empty2, empty, parentObj, func, layerName);
	}

	public void InitFakeObject(string weaponId, string headId, string bodyId, string legId, Transform parentObj, OnLoadFinishedDel func = null, string layerName = "NGUI")
	{
		string text = headId.Substring(0, headId.IndexOf('_'));
		string empty = string.Empty;
		empty = (text.Equals("XD") ? "baiRen" : ((!text.Equals("QJ")) ? "nvRen" : "heiRen"));
		mWeaponId = weaponId;
		InitFakeObject(weaponId, headId, bodyId, legId, text, empty, parentObj, func, layerName);
	}

	private GameObject CreatePlayerModel(string profession, string weaponId, string headId, string bodyId, string legId)
	{
		return ResourcesManager.LoadAndInstantiate("TestModel/Model/" + profession + "/ModelRoot") as GameObject;
	}

	private void LoadPlayerVisual(GameObject playerRootObj, string partWeaponId, string partHeadId, string partBodyId, string partLegId, OnLoadFinishedDel func = null)
	{
		OnLoadFinished = func;
		mLoadPartNum = 0;
		mLoadPartCount = 0;
		ModelData modeDataByID = DataManager.GetModeDataByID(partWeaponId);
		ModelData modeDataByID2 = DataManager.GetModeDataByID(partHeadId);
		ModelData modeDataByID3 = DataManager.GetModeDataByID(partBodyId);
		ModelData modeDataByID4 = DataManager.GetModeDataByID(partLegId);
		mPlayerModelObjData.StopLoadMesh();
		if (modeDataByID != null)
		{
			mPlayerModelObjData.LoadModel(MODEL_TYPE.WEAPON, partWeaponId, playerRootObj, OnLoadPlayerPartFinished, ref mLoadPartNum, LayerStr);
		}
		if (modeDataByID2 != null)
		{
			mPlayerModelObjData.LoadModel(MODEL_TYPE.HEAD, partHeadId, playerRootObj, OnLoadPlayerPartFinished, ref mLoadPartNum, LayerStr);
		}
		if (modeDataByID3 != null)
		{
			mPlayerModelObjData.LoadModel(MODEL_TYPE.BODY, partBodyId, playerRootObj, OnLoadPlayerPartFinished, ref mLoadPartNum, LayerStr);
		}
		if (modeDataByID4 != null)
		{
			mPlayerModelObjData.LoadModel(MODEL_TYPE.LEG, partLegId, playerRootObj, OnLoadPlayerPartFinished, ref mLoadPartNum, LayerStr);
		}
	}

	private void OnLoadPlayerPartFinished(object objBundle, object param1 = null, object param2 = null)
	{
		ModelData modelData = param1 as ModelData;
		GameObject gameObject = param2 as GameObject;
		GameObject gameObject2 = objBundle as GameObject;
		BundleManager.ResetShader(gameObject2.transform);
		gameObject2.layer = LayerMask.NameToLayer(LayerStr);
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = Vector3.up * 1.5f;
		gameObject2.transform.localScale = Vector3.one;
		gameObject2.transform.localRotation = Quaternion.identity;
		gameObject2.gameObject.name = modelData.Name;
		BundleManager.RebuildBones(gameObject, gameObject2);
		mPlayerModelObjData.LoadModelFinished(modelData.ModelType, modelData, gameObject2);
		mLoadPartCount++;
		if (mLoadPartCount >= mLoadPartNum)
		{
			if (UnityVersionUtil.IsActive(gameObject))
			{
				mPlayerModelObjData.ActiveModelObj();
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(gameObject2.gameObject, state: false);
			}
			OnInitFinished();
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(gameObject2.gameObject, state: false);
		}
	}

	private void OnInitFinished()
	{
		if (OnLoadFinished != null)
		{
			OnLoadFinished(this);
		}
	}

	public void CheckFakeObject(characterVisual visual, OnLoadFinishedDel func = null)
	{
		string weaponId = ObjManager.GetWeaponId(visual);
		string headId = ObjManager.GetHeadId(visual);
		string bodyId = ObjManager.GetBodyId(visual);
		string legId = ObjManager.GetLegId(visual);
		CheckFakeObject(weaponId, headId, bodyId, legId, func);
	}

	public void CheckFakeObject(string weaponId, string headId, string bodyId, string legId, OnLoadFinishedDel func = null)
	{
		OnLoadFinished = func;
		mChangePartNum = 0;
		mChangePartCount = 0;
		mWeaponId = weaponId;
		PlayAnim("idle", mModelName);
		if (FakeObj != null)
		{
			FakeObj.transform.parent = mFakeObjParent;
			FakeObj.transform.localPosition = Vector3.zero;
			FakeObj.transform.localRotation = Quaternion.identity;
			FakeObj.transform.localScale = Vector3.one;
		}
		mPlayerModelObjData.LoadModel(MODEL_TYPE.WEAPON, weaponId, FakeObj, OnLoadPlayerPartFinished, ref mChangePartNum, LayerStr);
		mPlayerModelObjData.LoadModel(MODEL_TYPE.HEAD, headId, FakeObj, OnLoadPlayerPartFinished, ref mChangePartNum, LayerStr);
		mPlayerModelObjData.LoadModel(MODEL_TYPE.BODY, bodyId, FakeObj, OnLoadPlayerPartFinished, ref mChangePartNum, LayerStr);
		mPlayerModelObjData.LoadModel(MODEL_TYPE.LEG, legId, FakeObj, OnLoadPlayerPartFinished, ref mChangePartNum, LayerStr);
		if (mChangePartNum == 0)
		{
			OnInitFinished();
		}
	}

	private void OnChangePartLoadFinished(object objBundle, object param1 = null, object param2 = null)
	{
		ModelData modelData = param1 as ModelData;
		GameObject gameObject = param2 as GameObject;
		GameObject gameObject2 = objBundle as GameObject;
		BundleManager.ResetShader(gameObject2.transform);
		gameObject2.layer = LayerMask.NameToLayer(LayerStr);
		gameObject2.transform.parent = gameObject.transform;
		gameObject2.transform.localPosition = Vector3.zero;
		gameObject2.transform.localScale = Vector3.one;
		UnityVersionUtil.SetActiveRecursive(gameObject2.gameObject, state: true);
		gameObject2.gameObject.name = modelData.Name;
		BundleManager.RebuildBones(gameObject, gameObject2);
		mPlayerModelObjData.LoadModelFinished(modelData.ModelType, modelData, gameObject2);
		mChangePartCount++;
		if (mChangePartCount >= mChangePartNum)
		{
			OnInitFinished();
		}
	}

	public void DestroyFakeObj()
	{
		if (FakeObj != null)
		{
			mPlayerModelObjData.UnloadAllModel();
			UnityEngine.Object.Destroy(FakeObj);
			FakeObj = null;
		}
		mAnim = null;
	}

	public void PlayAnim(string animationName, string modelType)
	{
		Animation fakeObjAnim = FakeObjAnim;
		string text = modelType;
		if (!string.IsNullOrEmpty(mWeaponId))
		{
			text = $"{modelType}_{GameDefine.GetWeaponName(mWeaponId)}";
		}
		AnimationState animationState = fakeObjAnim[$"{text}_{animationName}"];
		if (animationState == null)
		{
			AnimationClip animationClip = AnimationManager.LoadAnimation(text, animationName) as AnimationClip;
			if (animationClip != null)
			{
				fakeObjAnim.AddClip(animationClip, $"{text}_{animationName}");
			}
		}
		fakeObjAnim.Play($"{text}_{animationName}");
	}

	public string GetAnimaStateName(string animationName, CharacterModelData modelData)
	{
		if (modelData.TypeID == 0)
		{
			string arg = modelData.ModelFirstType;
			if (!string.IsNullOrEmpty(mWeaponId))
			{
				arg = $"{arg}_{GameDefine.GetWeaponName(mWeaponId)}";
			}
			return $"{arg}_{animationName}";
		}
		return animationName;
	}

	public string GetAnimaStateName(string animationName)
	{
		string arg = mModelName;
		if (!string.IsNullOrEmpty(mWeaponId))
		{
			arg = $"{arg}_{GameDefine.GetWeaponName(mWeaponId)}";
			return $"{arg}_{animationName}";
		}
		return animationName;
	}

	public void ActivePlayerModelObj()
	{
		mPlayerModelObjData.ActiveModelObj();
	}

	public void PlayAnim(string animationName, CharacterModelData modelData)
	{
		Animation fakeObjAnim = FakeObjAnim;
		AnimationState animationState = fakeObjAnim[GetAnimaStateName(animationName, modelData)];
		if (animationState == null)
		{
			if (modelData.TypeID == 0)
			{
				string text = modelData.ModelFirstType;
				if (!string.IsNullOrEmpty(mWeaponId))
				{
					text = $"{text}_{GameDefine.GetWeaponName(mWeaponId)}";
				}
				AnimationClip animationClip = AnimationManager.LoadAnimation(text, animationName) as AnimationClip;
				if (animationClip != null)
				{
					fakeObjAnim.AddClip(animationClip, $"{text}_{animationName}");
				}
			}
			else
			{
				AnimationClip animationClip2 = AnimationManager.LoadAnimation(modelData.ModelFirstType, animationName) as AnimationClip;
				if (animationClip2 != null)
				{
					fakeObjAnim.AddClip(animationClip2, animationName);
				}
				else
				{
					animationClip2 = AnimationManager.LoadAnimation(modelData.ModelSubType, animationName) as AnimationClip;
					if (animationClip2 != null)
					{
						fakeObjAnim.AddClip(animationClip2, animationName);
					}
					else
					{
						animationClip2 = AnimationManager.LoadAnimation(modelData.ModelType, animationName) as AnimationClip;
						if (animationClip2 != null)
						{
							fakeObjAnim.AddClip(animationClip2, animationName);
						}
					}
				}
			}
		}
		fakeObjAnim.enabled = false;
		fakeObjAnim.enabled = true;
		fakeObjAnim.Play(GetAnimaStateName(animationName, modelData));
	}

	public void CrossFadeAnima(string animationName, string modelType)
	{
		Animation fakeObjAnim = FakeObjAnim;
		string text = modelType;
		if (!string.IsNullOrEmpty(mWeaponId))
		{
			text = $"{modelType}_{GameDefine.GetWeaponName(mWeaponId)}";
		}
		AnimationState animationState = fakeObjAnim[$"{text}_{animationName}"];
		if (animationState == null)
		{
			AnimationClip clip = AnimationManager.LoadAnimation(text, animationName) as AnimationClip;
			fakeObjAnim.AddClip(clip, $"{text}_{animationName}");
		}
		fakeObjAnim.CrossFade($"{text}_{animationName}", 0.3f);
	}

	public void CrossFadeIdelSelect(int profession, string modelType)
	{
		if (GameDefine.GetWeaponType(mWeaponId) != -1)
		{
			CrossFadeAnima(GameDefine.GetRoleIdelSelectName(profession, GameDefine.GetWeaponType(mWeaponId)), modelType);
		}
	}

	public void UseSkill(string skillId, AnimationLogic.OnAnimFinished func)
	{
		string modelType = mModelName;
		if (!string.IsNullOrEmpty(mWeaponId))
		{
			modelType = $"{mModelName}_{GameDefine.GetWeaponName(mWeaponId)}";
		}
		if (mShowSkillLogic == null)
		{
			mShowSkillLogic = FakeObj.AddComponent<FakeObjSkillShowLogic>();
			mShowSkillLogic.Reset(FakeObj, modelType);
		}
		mShowSkillLogic.UseSkill(skillId, modelType, func);
	}

	public void DisableNpcAnimaHandle()
	{
		npcAnimaHandle.Cancel();
	}

	public void playNpcAnim(string animationName, CharacterModelData characterModelData)
	{
		Animation fakeObjAnim = FakeObjAnim;
		if (fakeObjAnim == null)
		{
			return;
		}
		AnimationState animationState = fakeObjAnim[animationName];
		if (animationState == null)
		{
			AnimationClip animationClip = AnimationManager.LoadAnimation(characterModelData.ModelFirstType, animationName) as AnimationClip;
			if (animationClip != null)
			{
				fakeObjAnim.AddClip(animationClip, animationName);
			}
			else
			{
				animationClip = AnimationManager.LoadAnimation(characterModelData.ModelSubType, animationName) as AnimationClip;
				if (animationClip != null)
				{
					fakeObjAnim.AddClip(animationClip, animationName);
				}
				else
				{
					animationClip = AnimationManager.LoadAnimation(characterModelData.ModelType, animationName) as AnimationClip;
					if (animationClip != null)
					{
						fakeObjAnim.AddClip(animationClip, animationName);
					}
				}
			}
			if (!(animationClip != null))
			{
				return;
			}
			fakeObjAnim.CrossFade(animationName);
			if (animationName.Equals("talk"))
			{
				float delay = Mathf.Max(animationClip.length - 0.3f, 0f);
				vp_Timer.In(delay, delegate
				{
					playNpcAnim("idle", characterModelData);
				}, npcAnimaHandle);
			}
			return;
		}
		fakeObjAnim.CrossFade(animationName);
		if (animationName.Equals("talk"))
		{
			float delay2 = Mathf.Max(fakeObjAnim["talk"].length - 0.3f, 0f);
			vp_Timer.In(delay2, delegate
			{
				playNpcAnim("idle", characterModelData);
			}, npcAnimaHandle);
		}
	}

	public void onLoadNpcFinished(object modelBundle, object param1, object param2)
	{
		mCurNpcLoadingModelData = null;
		mCurNpcLoadingModelDataId = -1L;
		GameObject gameObject = modelBundle as GameObject;
		Transform transform = param1 as Transform;
		CharacterModelData characterModelData = param2 as CharacterModelData;
		if (gameObject == null)
		{
			Log.ERROR_MSG("Instantiate Bundle Error");
			return;
		}
		gameObject.gameObject.name = $"MeshRoot";
		Material sm = null;
		BundleManager.ResetShader(gameObject.transform, out sm);
		FakeObj = gameObject;
		mFakeObjParent = transform;
		FakeObj.transform.parent = mFakeObjParent;
		FakeObj.transform.localScale = Vector3.one;
		FakeObj.transform.localPosition = Vector3.zero;
		FakeObj.transform.localRotation = Quaternion.identity;
		UnityVersionUtil.SetActiveRecursive(gameObject.gameObject, state: true);
		NGUITools.SetLayer(gameObject, LayerMask.NameToLayer(LayerStr));
		playNpcAnim("talk", characterModelData);
	}

	public void InitFakeNpcObj(string npcModelId, Transform parentObj, OnLoadFinishedDel func = null)
	{
		CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(npcModelId);
		if (!mCurNpcModelId.Equals(npcModelId))
		{
			if (FakeObj != null)
			{
				DestroyNpcFakeObj();
			}
			if (FakeObj == null)
			{
				if (mCurNpcLoadingModelData != null)
				{
					mCurNpcLoadingModelData.OnLoadFinished = null;
					mCurNpcLoadingModelData = null;
				}
				mCurNpcLoadingModelData = BundleManager.LoadModelInList(characterModelDataByID.Name, isNeedUnload: true, isDoNotCache: false, onLoadNpcFinished, parentObj, characterModelDataByID);
				mCurNpcLoadingModelDataId = ((mCurNpcLoadingModelData != null) ? mCurNpcLoadingModelData.ID : (-1));
				mModelName = characterModelDataByID.Name;
			}
		}
		else
		{
			playNpcAnim("talk", characterModelDataByID);
		}
		mCurNpcModelId = npcModelId;
	}

	public void InitAnimaFakeNpcObj(string npcModelId, Transform parentObj, string firstAnimaName)
	{
		CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(npcModelId);
		if (!mCurNpcModelId.Equals(npcModelId))
		{
			mCurNpcModelId = npcModelId;
			if (FakeObj != null)
			{
				DestroyNpcFakeObj();
			}
			if (FakeObj == null)
			{
				if (mCurNpcLoadingModelData != null)
				{
					mCurNpcLoadingModelData.OnLoadFinished = null;
					mCurNpcLoadingModelData = null;
				}
				mCurNpcLoadingModelData = BundleManager.LoadModelInList(characterModelDataByID.Name, isNeedUnload: true, isDoNotCache: false, onLoadAnimaNpcFinished, parentObj, firstAnimaName);
				mCurNpcLoadingModelDataId = ((mCurNpcLoadingModelData != null) ? mCurNpcLoadingModelData.ID : (-1));
				mModelName = characterModelDataByID.Name;
			}
		}
		else
		{
			playNpcAnim("talk", characterModelDataByID);
		}
	}

	public void onLoadAnimaNpcFinished(object modelBundle, object param1, object param2)
	{
		mCurNpcLoadingModelData = null;
		mCurNpcLoadingModelDataId = -1L;
		GameObject gameObject = modelBundle as GameObject;
		Transform transform = param1 as Transform;
		string animationName = param2 as string;
		if (gameObject == null)
		{
			Log.ERROR_MSG("Instantiate Bundle Error");
			return;
		}
		gameObject.gameObject.name = $"MeshRoot";
		Material sm = null;
		BundleManager.ResetShader(gameObject.transform, out sm);
		NGUITools.SetLayer(transform.gameObject, LayerMask.NameToLayer(LayerStr));
		FakeObj = gameObject;
		mFakeObjParent = transform;
		FakeObj.transform.parent = mFakeObjParent;
		FakeObj.transform.localScale = Vector3.one;
		FakeObj.transform.localPosition = Vector3.zero;
		FakeObj.transform.localRotation = Quaternion.identity;
		UnityVersionUtil.SetActiveRecursive(gameObject.gameObject, state: true);
		FakeObjAnim.cullingType = AnimationCullingType.AlwaysAnimate;
		PlayAnim(animationName, DataManager.GetCharacterModelDataByID(mCurNpcModelId));
	}

	public void DestroyNpcFakeObj()
	{
		if (mCurNpcLoadingModelData != null)
		{
			mCurNpcLoadingModelData.OnLoadFinished = null;
			mCurNpcLoadingModelData = null;
			BundleManager.RemoveFromLoadModelList(mCurNpcLoadingModelDataId);
			mCurNpcLoadingModelDataId = -1L;
		}
		if (FakeObj != null)
		{
			npcAnimaHandle.Cancel();
			UnityEngine.Object.Destroy(FakeObj);
			FakeObj = null;
			mAnim = null;
			BundleManager.UnloadModel(mCurNpcModelId, -1L, isMainPlayerUnload: false);
		}
	}
}
