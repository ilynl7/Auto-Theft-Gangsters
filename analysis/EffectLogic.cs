using System.Collections.Generic;
using UnityEngine;

public class EffectLogic : MonoBehaviour
{
	private static float EffectAvailableDistance = 20f;

	private static int[,] MAX_PLAYING_NUM = new int[3, 4]
	{
		{ 0, 5, 0, 0 },
		{ 5, 5, 8, 5 },
		{ 10, 10, 16, 10 }
	};

	private static int MAX_CACHE_NUM = 20;

	private static Dictionary<string, List<FxControl>> mCacheEffectDic = new Dictionary<string, List<FxControl>>();

	private static List<List<FxControl>> mCacheEffectList = new List<List<FxControl>>();

	private Dictionary<string, GameObject> mBindPosDic = new Dictionary<string, GameObject>();

	private ObjCharacter ownCharacter;

	public List<PlayEffInfoData> PlayEffInfoDataList = new List<PlayEffInfoData>();

	private static Dictionary<GameDefine.OBJ_TYPE, List<FxControl>> mCurrentPlayEffectDic = new Dictionary<GameDefine.OBJ_TYPE, List<FxControl>>();

	private static List<List<FxControl>> mCurrentPlayEffectList = new List<List<FxControl>>();

	private static Dictionary<GameDefine.OBJ_TYPE, List<PlayingEffectData>> mCurrentPlayEffectDataDic = new Dictionary<GameDefine.OBJ_TYPE, List<PlayingEffectData>>();

	private YinChangEffectControl mCurYinChangEffect;

	public static Dictionary<string, List<FxControl>> CacheEffectDic => mCacheEffectDic;

	public static Dictionary<GameDefine.OBJ_TYPE, List<FxControl>> CurrentPlayEffectDic => mCurrentPlayEffectDic;

	public static void Clear()
	{
		if (CacheEffectDic != null)
		{
			CacheEffectDic.Clear();
		}
		if (CurrentPlayEffectDic != null)
		{
			CurrentPlayEffectDic.Clear();
		}
		if (mCurrentPlayEffectDataDic != null)
		{
			mCurrentPlayEffectDataDic.Clear();
		}
	}

	private static int GetCacheEffectNum()
	{
		int num = 0;
		for (int i = 0; i < mCacheEffectList.Count; i++)
		{
			num += mCacheEffectList[i].Count;
		}
		return num;
	}

	private int GetPlayingEffectNum()
	{
		int num = 0;
		for (int i = 0; i < mCurrentPlayEffectList.Count; i++)
		{
			num += mCurrentPlayEffectList[i].Count;
		}
		return num;
	}

	private int GetPlayingEffectNum(GameDefine.OBJ_TYPE type)
	{
		if (mCurrentPlayEffectDataDic.ContainsKey(type))
		{
			return mCurrentPlayEffectDataDic[type].Count;
		}
		return 0;
	}

	private static FxControl GetEarliestCachedEffect()
	{
		float num = float.MaxValue;
		FxControl result = null;
		for (int i = 0; i < mCacheEffectList.Count; i++)
		{
			for (int j = 0; j < mCacheEffectList[i].Count; j++)
			{
				if (mCacheEffectList[i][j].GenerateTime < num)
				{
					num = mCacheEffectList[i][j].GenerateTime;
					result = mCacheEffectList[i][j];
				}
			}
		}
		return result;
	}

	private static FxControl GetEarliestPlayingEffect()
	{
		float num = float.MaxValue;
		FxControl result = null;
		for (int i = 0; i < mCurrentPlayEffectList.Count; i++)
		{
			for (int j = 0; j < mCurrentPlayEffectList[i].Count; j++)
			{
				if (mCurrentPlayEffectList[i][j].GenerateTime < num)
				{
					num = mCurrentPlayEffectList[i][j].GenerateTime;
					result = mCurrentPlayEffectList[i][j];
				}
			}
		}
		return result;
	}

	private bool IsEffectInPlayingList(string effectId)
	{
		for (int i = 0; i < PlayEffInfoDataList.Count; i++)
		{
			if (PlayEffInfoDataList[i].fxControl.EffectId.Equals(effectId))
			{
				return true;
			}
		}
		if (!mCurrentPlayEffectDataDic.ContainsKey(ownCharacter.ObjType))
		{
			return false;
		}
		List<PlayingEffectData> list = mCurrentPlayEffectDataDic[ownCharacter.ObjType];
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].OwnerId == ownCharacter.ServerId && list[j].EffectDataId.Equals(effectId))
			{
				return true;
			}
		}
		return false;
	}

	public void AddPlayEffInfoData(string actionName, string effInfoId, float delayTime, Vector3 senderPos, Transform targetTransform = null)
	{
		if (!GameSettingData.IsShowSkillEffect[GameSettingData.GetPhoneClass()] || GameSettingData.IsLowPhone || (ownCharacter.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER && !(ownCharacter as ObjOtherPlayer).IsVisible()) || ownCharacter.MainPlayerDistance() > EffectAvailableDistance)
		{
			return;
		}
		string actionName2 = ownCharacter.GetActionName(actionName);
		ActionData actionDataByName = DataManager.GetActionDataByName(actionName2);
		if (actionDataByName == null || !IsEffectInPlayingList(actionDataByName.FxEffID))
		{
			List<PlayEffInfoData> playEffInfoDataList = PlayEffInfoData.GetPlayEffInfoDataList(actionName2, effInfoId, delayTime, senderPos, targetTransform);
			if (playEffInfoDataList != null && GetPlayingEffectNum(ownCharacter.ObjType) + playEffInfoDataList.Count <= MAX_PLAYING_NUM[GameSettingData.GetPhoneClass(), (int)ownCharacter.ObjType])
			{
				AddPlayEffInfoList(playEffInfoDataList);
			}
		}
	}

	public void AddPlayeBufEffInfoData(string fxeffectInfoId, float delayTime, float duration, Vector3 senderPosition)
	{
		if (GameSettingData.IsShowSkillEffect[GameSettingData.GetPhoneClass()] && !GameSettingData.IsLowPhone && (ownCharacter.ObjType != GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER || (ownCharacter as ObjOtherPlayer).IsVisible()) && !IsEffectInPlayingList(fxeffectInfoId))
		{
			List<PlayEffInfoData> playBufEffInfoDataList = PlayEffInfoData.GetPlayBufEffInfoDataList(fxeffectInfoId, delayTime, duration, senderPosition);
			if (playBufEffInfoDataList != null && GetPlayingEffectNum(ownCharacter.ObjType) + playBufEffInfoDataList.Count <= MAX_PLAYING_NUM[GameSettingData.GetPhoneClass(), (int)ownCharacter.ObjType])
			{
				AddPlayEffInfoList(playBufEffInfoDataList);
			}
		}
	}

	private void AddPlayEffInfoList(List<PlayEffInfoData> list)
	{
		if (list == null)
		{
			return;
		}
		for (int i = 0; i < list.Count; i++)
		{
			PlayEffInfoData playEffInfoData = list[i];
			FxControl fxControl = null;
			string effName = playEffInfoData.mFxEffinfoData.EffName;
			if (mCacheEffectDic.ContainsKey(effName) && mCacheEffectDic[effName].Count > 0)
			{
				fxControl = mCacheEffectDic[effName][0];
				if (fxControl != null)
				{
					mCacheEffectDic[effName].Remove(fxControl);
					fxControl.Reset(this, playEffInfoData.mFxEffinfoData, playEffInfoData.DurationTime, ownCharacter.ServerId, ownCharacter.ObjType);
					AddBindPos(fxControl, playEffInfoData);
				}
			}
			else
			{
				string fxLoadPath = playEffInfoData.fxLoadPath;
				GameObject gameObject = ResourcesManager.LoadAndInstantiate(fxLoadPath) as GameObject;
				if (gameObject != null)
				{
					gameObject.name = effName;
					fxControl = gameObject.GetComponent<FxControl>();
					fxControl.Reset(this, playEffInfoData.mFxEffinfoData, playEffInfoData.DurationTime, ownCharacter.ServerId, ownCharacter.ObjType);
					AddBindPos(fxControl, playEffInfoData);
				}
			}
			if (fxControl != null)
			{
				NGUITools.SetLayer(fxControl.gameObject, 0);
				playEffInfoData.fxControl = fxControl;
				PlayEffInfoDataList.Add(playEffInfoData);
				AddCurrentPlayEffectDic(playEffInfoData);
			}
			else
			{
				playEffInfoData.LoadStartTime = Time.time;
				BundleManager.LoadEffectInList(effName, isNeedUnload: false, isDoNotCache: false, OnLoadEffect, playEffInfoData);
			}
			AddCurrentPlayEffectDataDic(playEffInfoData.mFxEffinfoData.ID);
		}
	}

	private void AddCurrentPlayEffectDataDic(string effectId)
	{
		if (!mCurrentPlayEffectDataDic.ContainsKey(ownCharacter.ObjType))
		{
			mCurrentPlayEffectDataDic.Add(ownCharacter.ObjType, new List<PlayingEffectData>());
		}
		mCurrentPlayEffectDataDic[ownCharacter.ObjType].Add(new PlayingEffectData(ownCharacter.ServerId, effectId));
	}

	private void AddCurrentPlayEffectDic(PlayEffInfoData playeffInfoData)
	{
		if (!mCurrentPlayEffectDic.ContainsKey(playeffInfoData.fxControl.OwnerType))
		{
			mCurrentPlayEffectDic.Add(playeffInfoData.fxControl.OwnerType, new List<FxControl>());
			mCurrentPlayEffectList.Add(mCurrentPlayEffectDic[playeffInfoData.fxControl.OwnerType]);
		}
		mCurrentPlayEffectDic[playeffInfoData.fxControl.OwnerType].Add(playeffInfoData.fxControl);
	}

	private void OnLoadEffect(object objBundle, object param1 = null, object param2 = null)
	{
		GameObject gameObject = objBundle as GameObject;
		NGUITools.SetLayer(gameObject, 0);
		BundleManager.ResetParticleShader(gameObject.transform);
		PlayEffInfoData playEffInfoData = param1 as PlayEffInfoData;
		if (gameObject != null)
		{
			FxControl component = gameObject.GetComponent<FxControl>();
			component.Reset(this, playEffInfoData.mFxEffinfoData, playEffInfoData.DurationTime, ownCharacter.ServerId, ownCharacter.ObjType);
			gameObject.name = playEffInfoData.mFxEffinfoData.EffName;
			if (Time.time - playEffInfoData.LoadStartTime < playEffInfoData.DelayTime + playEffInfoData.DurationTime)
			{
				playEffInfoData.DelayTime -= Time.time - playEffInfoData.LoadStartTime;
				AddBindPos(component, playEffInfoData);
				playEffInfoData.fxControl = component;
				PlayEffInfoDataList.Add(playEffInfoData);
				AddCurrentPlayEffectDic(playEffInfoData);
			}
			else
			{
				RecyleEffect(component);
			}
		}
	}

	public void AddBindPos(FxControl fxControl, PlayEffInfoData playEffInfoData)
	{
		FxEffInfoData mFxEffinfoData = playEffInfoData.mFxEffinfoData;
		string effLinkNode = mFxEffinfoData.EffLinkNode;
		GameObject value = null;
		if (!mBindPosDic.TryGetValue(effLinkNode, out value))
		{
			value = TransformUtil.FindChildGameObject(ownCharacter.gameObject, effLinkNode);
			if (value != null)
			{
				mBindPosDic.Add(effLinkNode, value);
			}
		}
		if (value != null)
		{
			fxControl.CacheTransform.parent = value.transform;
			fxControl.CacheTransform.localPosition = mFxEffinfoData.Position;
			fxControl.CacheTransform.localEulerAngles = mFxEffinfoData.Angel;
		}
		else
		{
			fxControl.transform.position = playEffInfoData.SenderPos;
		}
	}

	public void BreakEffect(string effectId)
	{
		if (mCurrentPlayEffectDic.ContainsKey(ownCharacter.ObjType))
		{
			List<FxControl> list = mCurrentPlayEffectDic[ownCharacter.ObjType];
			for (int num = list.Count - 1; num > -1; num--)
			{
				if (list[num].OwnerId == ownCharacter.ServerId && list[num].EffectId.Equals(effectId))
				{
					RecyleEffect(list[num]);
				}
			}
		}
		if (mCurrentPlayEffectDataDic.ContainsKey(ownCharacter.ObjType))
		{
			List<PlayingEffectData> list2 = mCurrentPlayEffectDataDic[ownCharacter.ObjType];
			for (int num2 = list2.Count - 1; num2 > -1; num2--)
			{
				if (list2[num2].OwnerId == ownCharacter.ServerId && list2[num2].EffectDataId.Equals(effectId))
				{
					list2.RemoveAt(num2);
				}
			}
		}
		for (int num3 = PlayEffInfoDataList.Count - 1; num3 > -1; num3--)
		{
			if (PlayEffInfoDataList[num3].mFxEffinfoData.ID.Equals(effectId))
			{
				PlayEffInfoDataList.RemoveAt(num3);
			}
		}
	}

	public static void RecyleEffect(FxControl fxControl)
	{
		if (fxControl == null)
		{
			return;
		}
		List<FxControl> value = null;
		if (mCurrentPlayEffectDic.ContainsKey(fxControl.OwnerType))
		{
			mCurrentPlayEffectDic[fxControl.OwnerType].Remove(fxControl);
		}
		if (mCurrentPlayEffectDataDic.ContainsKey(fxControl.OwnerType))
		{
			List<PlayingEffectData> list = mCurrentPlayEffectDataDic[fxControl.mOwnerType];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].OwnerId == fxControl.OwnerId && list[i].EffectDataId.Equals(fxControl.EffectId))
				{
					list.RemoveAt(i);
					break;
				}
			}
		}
		fxControl.transform.parent = null;
		UnityVersionUtil.SetActiveRecursive(fxControl.gameObject, state: false);
		if (GetCacheEffectNum() > MAX_CACHE_NUM)
		{
			FxControl earliestCachedEffect = GetEarliestCachedEffect();
			if (earliestCachedEffect != null)
			{
				mCacheEffectDic[earliestCachedEffect.EffectName].Remove(earliestCachedEffect);
				Object.Destroy(earliestCachedEffect.gameObject);
			}
		}
		if (mCacheEffectDic.TryGetValue(fxControl.EffectName, out value))
		{
			value.Add(fxControl);
			return;
		}
		value = new List<FxControl>();
		value.Add(fxControl);
		mCacheEffectDic.Add(fxControl.EffectName, value);
		mCacheEffectList.Add(value);
	}

	public void UpdatePlayEffInfoData()
	{
		if (PlayEffInfoDataList.Count <= 0)
		{
			return;
		}
		for (int num = PlayEffInfoDataList.Count - 1; num > -1; num--)
		{
			PlayEffInfoDataList[num].DelayTime -= Time.deltaTime;
			PlayEffInfoData playEffInfoData = PlayEffInfoDataList[num];
			if (playEffInfoData.DelayTime <= 0f)
			{
				if (playEffInfoData.fxControl != null)
				{
					if (playEffInfoData.mFxEffinfoData.AutoMoveFlag)
					{
						playEffInfoData.fxControl.Play(playEffInfoData.targetTransform);
					}
					else
					{
						playEffInfoData.fxControl.Play();
					}
				}
				PlayEffInfoDataList.RemoveAt(num);
			}
		}
	}

	public void BreakYinChangEffect(string effId)
	{
		if (mCurYinChangEffect != null && mCurYinChangEffect.gameObject != null && mCurYinChangEffect.effectId.Equals(effId))
		{
			Object.Destroy(mCurYinChangEffect.gameObject);
			mCurYinChangEffect = null;
		}
	}

	public void OnYinChangFinished()
	{
		mCurYinChangEffect = null;
	}

	public void PlayYinChangeEffInfo(string effInfoId, float duration, Vector3 senderPos)
	{
		EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(effInfoId);
		if (effInfoDataById != null)
		{
			GameObject gameObject = ResourcesManager.LoadAndInstantiate("Effect/Yinchang") as GameObject;
			if (gameObject != null)
			{
				YinChangEffectControl component = gameObject.GetComponent<YinChangEffectControl>();
				component.InitEffect(effInfoDataById, duration, senderPos, base.transform.rotation, OnYinChangFinished);
				mCurYinChangEffect = component;
			}
			else
			{
				BundleManager.LoadEffectInList("Yinchang", isNeedUnload: false, isDoNotCache: false, OnLoadYinchangEffect, new BundleLoadYinChangData(effInfoDataById, duration, senderPos));
			}
		}
	}

	private void OnLoadYinchangEffect(object objBundle, object param1 = null, object param2 = null)
	{
		GameObject gameObject = objBundle as GameObject;
		BundleManager.ResetParticleShader(gameObject.transform);
		BundleLoadYinChangData bundleLoadYinChangData = param1 as BundleLoadYinChangData;
		if (gameObject != null)
		{
			YinChangEffectControl component = gameObject.GetComponent<YinChangEffectControl>();
			component.InitEffect(bundleLoadYinChangData.effInfoData, bundleLoadYinChangData.duration, bundleLoadYinChangData.senderPos, base.transform.rotation, OnYinChangFinished);
		}
	}

	public bool EffInfoPlayCheck(PlayEffInfoData effInfo)
	{
		if (effInfo.mActionData.AnimCanBeBreak == 1)
		{
			return true;
		}
		if (effInfo.EffinfoData == null)
		{
			return true;
		}
		if (effInfo.EffinfoData.MoveDistance != 0 && effInfo.EffinfoData.ForceMove != 1)
		{
			return true;
		}
		Debug.Log(effInfo.EffinfoData.MoveDistance);
		Debug.Log(effInfo.EffinfoData.ForceMove);
		return false;
	}

	public void Init(ObjCharacter objCharacter)
	{
		ownCharacter = objCharacter;
	}

	public static void PrintCurPlayingEffect()
	{
		Debug.Log("*******************************************");
		List<List<FxControl>> list = new List<List<FxControl>>(mCurrentPlayEffectDic.Values);
		for (int i = 0; i < list.Count; i++)
		{
			Debug.Log("Cur i :: " + i);
			for (int j = 0; j < list[i].Count; j++)
			{
				Debug.Log(list[i][j].EffectId);
			}
		}
		Debug.Log("******************************************");
		List<List<PlayingEffectData>> list2 = new List<List<PlayingEffectData>>(mCurrentPlayEffectDataDic.Values);
		for (int k = 0; k < list2.Count; k++)
		{
			Debug.Log("Data i :: " + k);
			for (int l = 0; l < list2[k].Count; l++)
			{
				Debug.Log("Playing Data :: " + list2[k][l].EffectDataId);
			}
		}
	}
}
