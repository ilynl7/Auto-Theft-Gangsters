using System.Collections.Generic;
using UnityEngine;

public class ObjNpcPool
{
	private class LoadNPCModelParam
	{
		public ObjInitNpcData InitData;

		public ObjManager.OnGetNPC LoadFinishedFunc;
	}

	public float LastUseTime;

	private string mID;

	private List<ObjNPC> mEnableNpcList = new List<ObjNPC>();

	private int mMaxPoolNum;

	private List<ObjNPC> mDisableNpcList = new List<ObjNPC>();

	private int CreatingNPCNum;

	public string ID => mID;

	public List<ObjNPC> EnableNpcList => mEnableNpcList;

	public int MaxPoolNum
	{
		get
		{
			return mMaxPoolNum;
		}
		set
		{
			mMaxPoolNum = value;
		}
	}

	public List<ObjNPC> DisableNpcList => mDisableNpcList;

	public ObjNpcPool()
	{
		Reset();
	}

	public void Reset()
	{
		for (int i = 0; i < mEnableNpcList.Count; i++)
		{
			Object.Destroy(mEnableNpcList[i]);
		}
		for (int j = 0; j < mDisableNpcList.Count; j++)
		{
			Object.Destroy(mDisableNpcList[j]);
		}
		mEnableNpcList.Clear();
		mDisableNpcList.Clear();
	}

	public void SetPoolMaxNum(int maxNum, string id)
	{
		mMaxPoolNum = maxNum;
		mID = id;
	}

	public void RefreshPoolMaxNum(int newPoolNum)
	{
		mMaxPoolNum = newPoolNum;
	}

	public void GetNpc(ObjInitNpcData initData, ObjManager.OnGetNPC func, ObjManager.OnGetNPC onLoadModelFinished = null)
	{
		LastUseTime = Time.time;
		if (mDisableNpcList.Count > 0)
		{
			ObjNPC objNPC = mDisableNpcList[0];
			UnityVersionUtil.SetActiveRecursive(objNPC.gameObject, state: true);
			objNPC.ResetNpc(initData);
			mDisableNpcList.RemoveAt(0);
			mEnableNpcList.Add(objNPC);
			objNPC.gameObject.name = "NPC" + initData.mServerID + "_" + initData.npcInfoData.ID;
			if (objNPC.MeshRoot != null)
			{
				onLoadModelFinished?.Invoke(objNPC);
			}
			else
			{
				LoadNPCModelParam loadNPCModelParam = new LoadNPCModelParam();
				loadNPCModelParam.InitData = initData;
				loadNPCModelParam.LoadFinishedFunc = onLoadModelFinished;
				objNPC.LoadingModelData = BundleManager.LoadModelInList(objNPC.CurrentCharacterModelData.Name, isNeedUnload: true, isDoNotCache: false, OnLoadNpcModelFinished, loadNPCModelParam, objNPC);
				objNPC.LoadingModelDataId = ((objNPC.LoadingModelData != null) ? objNPC.LoadingModelData.ID : (-1));
			}
			func?.Invoke(objNPC);
			return;
		}
		CharacterModelData characterModelDataByID = DataManager.GetCharacterModelDataByID(initData.npcInfoData.Model);
		if (characterModelDataByID != null)
		{
			GameObject gameObject = ResourcesManager.LoadAndInstantiate("TestModel/NPCRoot") as GameObject;
			ObjNPC objNPC2 = null;
			if (gameObject != null)
			{
				gameObject.name = "NPC" + initData.mServerID + "_" + initData.npcInfoData.ID;
				objNPC2 = gameObject.GetComponent<ObjNPC>();
				if (objNPC2 == null)
				{
					objNPC2 = gameObject.AddComponent<ObjNPC>();
				}
				objNPC2.InitInfo(characterModelDataByID);
				objNPC2.Init();
				UnityVersionUtil.SetActiveRecursive(objNPC2.gameObject, state: true);
				objNPC2.ResetNpc(initData);
				mEnableNpcList.Add(objNPC2);
				LoadNPCModelParam loadNPCModelParam2 = new LoadNPCModelParam();
				loadNPCModelParam2.InitData = initData;
				loadNPCModelParam2.LoadFinishedFunc = onLoadModelFinished;
				objNPC2.LoadingModelData = BundleManager.LoadModelInList(characterModelDataByID.Name, isNeedUnload: true, isDoNotCache: false, OnLoadNpcModelFinished, loadNPCModelParam2, objNPC2);
				objNPC2.LoadingModelDataId = ((objNPC2.LoadingModelData != null) ? objNPC2.LoadingModelData.ID : (-1));
				func?.Invoke(objNPC2);
			}
		}
		else
		{
			Debug.Log("characterModelData == null " + initData.npcInfoData.Model);
		}
	}

	public void OnLoadNpcModelFinished(object modelBundle, object param1, object param2)
	{
		LoadNPCModelParam loadNPCModelParam = param1 as LoadNPCModelParam;
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
		objNPC.MeshRoot = gameObject;
		Material sm = null;
		BundleManager.ResetShader(gameObject.transform, out sm);
		objNPC.MeshMat = sm;
		ReloadModel(objNPC.CacheTransform, gameObject);
		objNPC.LoadModelFinishInit();
		gameObject.transform.localScale = Vector3.one * loadNPCModelParam.InitData.npcInfoData.ModelScale;
		if (objNPC.enabled)
		{
			UnityVersionUtil.SetActiveRecursive(gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(gameObject, state: false);
		}
		if (loadNPCModelParam.LoadFinishedFunc != null)
		{
			loadNPCModelParam.LoadFinishedFunc(objNPC);
		}
	}

	public void RecycleNpc(ObjNPC objNpc)
	{
		if (mDisableNpcList.Contains(objNpc))
		{
			return;
		}
		if (objNpc.dieDelayHandle != null)
		{
			objNpc.dieDelayHandle.Cancel();
		}
		mEnableNpcList.Remove(objNpc);
		if (objNpc.LoadingModelData != null)
		{
			objNpc.LoadingModelData.OnLoadFinished = null;
			objNpc.LoadingModelData = null;
			BundleManager.RemoveFromLoadModelList(objNpc.LoadingModelDataId);
			objNpc.LoadingModelDataId = -1L;
		}
		objNpc.OnRecycle();
		if (mDisableNpcList.Count >= mMaxPoolNum)
		{
			if (objNpc.MeshRoot != null)
			{
				BundleManager.UnloadModel(objNpc.CurrentCharacterModelData.ID, -1L, isMainPlayerUnload: false);
			}
			Object.Destroy(objNpc.gameObject);
			return;
		}
		mDisableNpcList.Add(objNpc);
		UnityVersionUtil.SetActiveRecursive(objNpc.gameObject, state: false);
		if (!(objNpc.MeshRoot != null) || !(objNpc.AnimationLogic.AnimaObj != null))
		{
			return;
		}
		if (objNpc.NPCType == GameDefine.NPC_TYPE.BOSS)
		{
			if (objNpc.AnimationLogic.AnimaObj["idle_attack"] == null)
			{
				objNpc.AnimationLogic.LoadAnim(DataManager.GetActionDataByName("idle_attack"));
			}
			objNpc.AnimationLogic.AnimaObj.Play("idle_attack");
			objNpc.AnimationLogic.AnimaObj["idle_attack"].time = 0f;
			objNpc.AnimationLogic.AnimaObj.Sample();
		}
		else
		{
			if (objNpc.AnimationLogic.AnimaObj["idle"] == null)
			{
				objNpc.AnimationLogic.LoadAnim(DataManager.GetActionDataByName("idle"));
			}
			objNpc.AnimationLogic.AnimaObj.Play("idle");
			objNpc.AnimationLogic.AnimaObj["idle"].time = 0f;
			objNpc.AnimationLogic.AnimaObj.Sample();
		}
	}

	public void ReloadModel(Transform root, GameObject obj)
	{
		obj.transform.parent = root;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
	}

	public void DestroyClearPool()
	{
		for (int num = mDisableNpcList.Count - 1; num >= 0; num--)
		{
			if (mDisableNpcList[num].LoadingModelData != null)
			{
				mDisableNpcList[num].LoadingModelData.OnLoadFinished = null;
				mDisableNpcList[num].LoadingModelData = null;
				BundleManager.RemoveFromLoadModelList(mDisableNpcList[num].LoadingModelDataId);
				mDisableNpcList[num].LoadingModelDataId = -1L;
			}
			if (mDisableNpcList[num].MeshRoot != null)
			{
				BundleManager.UnloadModel(mDisableNpcList[num].CurrentCharacterModelData.ID, -1L, isMainPlayerUnload: false);
			}
			Object.Destroy(mDisableNpcList[num].gameObject);
		}
	}

	public void Clear()
	{
		mEnableNpcList.Clear();
		mDisableNpcList.Clear();
	}
}
