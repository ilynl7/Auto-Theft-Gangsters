using System.Collections.Generic;
using UnityEngine;

public class ModelObjData
{
	public ModelData ModelData;

	public long LoadingModelId = -1L;

	public BundleManager.LoadModelData LoadingModelData;

	public GameObject ModelObj;

	public ModelData TargetModelData;

	public string CurEffectId = string.Empty;

	public List<GameObject> EffectList = new List<GameObject>();

	public List<BundleManager.LoadModelData> LoadingEffectDataList = new List<BundleManager.LoadModelData>();

	public string ModelId
	{
		get
		{
			if (ModelData != null)
			{
				return ModelData.ID;
			}
			return string.Empty;
		}
	}

	public string TargetModelId
	{
		get
		{
			if (TargetModelData != null)
			{
				return TargetModelData.ID;
			}
			return string.Empty;
		}
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
				TargetModelData = DataManager.GetModeDataByID(value);
			}
			else
			{
				TargetModelData = null;
			}
		}
	}

	public void ClearCurEffect()
	{
		for (int i = 0; i < EffectList.Count; i++)
		{
			Object.Destroy(EffectList[i]);
		}
		EffectList.Clear();
	}

	public void ClearEffect()
	{
		ClearCurEffect();
		StopLoadingEffect();
	}

	public void StopLoadingEffect()
	{
		for (int i = 0; i < LoadingEffectDataList.Count; i++)
		{
			if (LoadingEffectDataList[i] != null)
			{
				LoadingEffectDataList[i].OnLoadFinished = null;
			}
		}
		LoadingEffectDataList.Clear();
	}

	public void UnloadCurModel()
	{
		if (ModelData != null)
		{
			if (ModelObj != null)
			{
				Object.Destroy(ModelObj);
				BundleManager.UnloadModel(ModelData.Name, ModelData.ModelPath, -1L, isMainPlayerPart: false);
				ModelObj = null;
			}
			ModelData = null;
		}
	}

	public void UnloadAllModel()
	{
		UnloadCurModel();
		StopLoadingMesh();
		ClearEffect();
	}

	public void StopLoadingMesh()
	{
		if (LoadingModelData != null)
		{
			LoadingModelData.OnLoadFinished = null;
			LoadingModelData = null;
			BundleManager.RemoveFromLoadModelList(LoadingModelId);
			LoadingModelId = -1L;
		}
		StopLoadingEffect();
	}
}
