using System.Collections.Generic;
using UnityEngine;

public class PlayerModelObjData
{
	private ModelObjData[] mPartModelObjData = new ModelObjData[4]
	{
		new ModelObjData(),
		new ModelObjData(),
		new ModelObjData(),
		new ModelObjData()
	};

	public ModelObjData[] PartModelObjData => mPartModelObjData;

	public void StopLoadMesh()
	{
		for (int i = 0; i < mPartModelObjData.Length; i++)
		{
			mPartModelObjData[i].StopLoadingMesh();
		}
	}

	public void SetTargetPartId(MODEL_TYPE type, string id)
	{
		mPartModelObjData[(int)type].TargetModelId = id;
	}

	public string GetTargetPartId(MODEL_TYPE type)
	{
		return mPartModelObjData[(int)type].TargetModelId;
	}

	public void LoadModel(MODEL_TYPE type, string targetId, GameObject fakeObjRoot, BundleManager.OnLoadModelFinish onLoadModelFinish, ref int needLoadPartNum, string layerStr)
	{
		bool flag = false;
		if (!mPartModelObjData[(int)type].TargetModelId.Equals(targetId))
		{
			if (!mPartModelObjData[(int)type].ModelId.Equals(targetId))
			{
				flag = true;
				needLoadPartNum++;
				mPartModelObjData[(int)type].StopLoadingMesh();
			}
			else
			{
				flag = false;
				mPartModelObjData[(int)type].StopLoadingMesh();
			}
			mPartModelObjData[(int)type].TargetModelId = targetId;
		}
		else
		{
			flag = false;
			if (!mPartModelObjData[(int)type].ModelId.Equals(targetId))
			{
				needLoadPartNum++;
			}
		}
		ModelData modeDataByID = DataManager.GetModeDataByID(targetId);
		if (flag && modeDataByID != null)
		{
			mPartModelObjData[(int)type].LoadingModelData = BundleManager.LoadModelInList(modeDataByID.Name, isNeedUnload: true, isDoNotCache: false, onLoadModelFinish, modeDataByID, fakeObjRoot, modeDataByID.ModelPath);
			mPartModelObjData[(int)type].LoadingModelId = ((mPartModelObjData[(int)type].LoadingModelData != null) ? mPartModelObjData[(int)type].LoadingModelData.ID : (-1));
		}
		CheckModelEffect(modeDataByID, fakeObjRoot, layerStr);
	}

	private void CheckModelEffect(ModelData modelData, GameObject player, string layerStr)
	{
		List<GameObject> effectList = mPartModelObjData[modelData.ModelType].EffectList;
		List<BundleManager.LoadModelData> loadingEffectDataList = mPartModelObjData[modelData.ModelType].LoadingEffectDataList;
		if (GameSettingData.IsShowFashionEffect[GameSettingData.GetPhoneClass()] && !string.IsNullOrEmpty(modelData.EffectId))
		{
			mPartModelObjData[modelData.ModelType].ClearEffect();
			List<FxEffInfoData> fxEffInfoDataListById = DataManager.GetFxEffInfoDataListById(modelData.EffectId);
			for (int i = 0; i < fxEffInfoDataListById.Count; i++)
			{
				ModelEffectData modelEffectData = new ModelEffectData(modelData, fxEffInfoDataListById[i], i == 0, layerStr);
				string path = $"{fxEffInfoDataListById[i].EffFilePath}/{fxEffInfoDataListById[i].EffName}";
				GameObject gameObject = ResourcesManager.LoadAndInstantiate(path) as GameObject;
				if (gameObject != null)
				{
					effectList.Add(gameObject);
					gameObject.transform.parent = TransformUtil.FindChildTransform(player.transform, modelEffectData.fxEffinfoData.EffLinkNode);
					gameObject.transform.localPosition = modelEffectData.fxEffinfoData.Position;
					gameObject.transform.localEulerAngles = modelEffectData.fxEffinfoData.Angel;
					NGUITools.SetLayer(gameObject, LayerMask.NameToLayer(layerStr));
				}
				else
				{
					modelEffectData.Index = loadingEffectDataList.Count;
					loadingEffectDataList.Add(BundleManager.LoadEffectInList(fxEffInfoDataListById[i].EffName, isNeedUnload: true, isDoNotCache: false, OnLoadPlayerPartEffectFinished, modelEffectData, player));
				}
			}
		}
		else
		{
			mPartModelObjData[modelData.ModelType].ClearEffect();
		}
	}

	public void OnLoadPlayerPartEffectFinished(object objBundle, object param1 = null, object param2 = null)
	{
		GameObject gameObject = param2 as GameObject;
		if (gameObject == null)
		{
			return;
		}
		ModelEffectData modelEffectData = param1 as ModelEffectData;
		GameObject gameObject2 = objBundle as GameObject;
		List<GameObject> effectList = mPartModelObjData[modelEffectData.modelData.ModelType].EffectList;
		effectList.Add(gameObject2);
		gameObject2.transform.parent = TransformUtil.FindChildTransform(gameObject.transform, modelEffectData.fxEffinfoData.EffLinkNode, active: true);
		gameObject2.transform.localPosition = modelEffectData.fxEffinfoData.Position;
		gameObject2.transform.localEulerAngles = modelEffectData.fxEffinfoData.Angel;
		if (!string.IsNullOrEmpty(modelEffectData.LayerStr))
		{
			NGUITools.SetLayer(gameObject2, LayerMask.NameToLayer(modelEffectData.LayerStr));
		}
		List<BundleManager.LoadModelData> loadingEffectDataList = mPartModelObjData[modelEffectData.modelData.ModelType].LoadingEffectDataList;
		if (modelEffectData.Index >= 0 && modelEffectData.Index < loadingEffectDataList.Count)
		{
			loadingEffectDataList[modelEffectData.Index] = null;
			bool flag = true;
			for (int i = 0; i < loadingEffectDataList.Count; i++)
			{
				if (loadingEffectDataList[i] != null)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				loadingEffectDataList.Clear();
			}
		}
		if (UnityVersionUtil.IsActive(gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(gameObject2.gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(gameObject2.gameObject, state: false);
		}
	}

	public void LoadModelFinished(MODEL_TYPE type, ModelData modelData, GameObject modelObj)
	{
		LoadModelFinished((int)type, modelData, modelObj);
	}

	public void LoadModelFinished(int type, ModelData modelData, GameObject modelObj)
	{
		mPartModelObjData[type].TargetModelId = string.Empty;
		mPartModelObjData[type].LoadingModelData = null;
		mPartModelObjData[type].LoadingModelId = -1L;
		mPartModelObjData[type].UnloadCurModel();
		mPartModelObjData[type].ModelData = modelData;
		mPartModelObjData[type].ModelObj = modelObj;
	}

	public void ActiveModelObj()
	{
		for (int i = 0; i < mPartModelObjData.Length; i++)
		{
			if (mPartModelObjData[i].ModelObj != null)
			{
				UnityVersionUtil.SetActiveRecursive(mPartModelObjData[i].ModelObj, state: true);
			}
		}
	}

	public void UnloadAllModel()
	{
		for (int i = 0; i < mPartModelObjData.Length; i++)
		{
			mPartModelObjData[i].UnloadAllModel();
		}
	}
}
