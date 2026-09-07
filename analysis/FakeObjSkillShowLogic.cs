using System.Collections.Generic;
using UnityEngine;

public class FakeObjSkillShowLogic : MonoBehaviour
{
	private GameObject mFakeObj;

	public List<PlayEffInfoData> PlayEffInfoDataList = new List<PlayEffInfoData>();

	private Dictionary<string, GameObject> mBindPosDic = new Dictionary<string, GameObject>();

	private Animation anima;

	private string mCurAnimaName;

	private string mModelType;

	private AnimationLogic.OnAnimFinished onSkillFinished;

	public void Reset(GameObject fakeObj, string modelType)
	{
		mFakeObj = fakeObj;
		anima = mFakeObj.animation;
		mModelType = modelType;
		AnimationClip clip = AnimationManager.LoadAnimation(modelType, "idle_attack") as AnimationClip;
		anima.AddClip(clip, "idle_attack");
	}

	public void UseSkill(string skillId, string modelType, AnimationLogic.OnAnimFinished func)
	{
		SkillData skillDataById = DataManager.GetSkillDataById(skillId);
		if (skillDataById == null)
		{
			Log.DEBUG_MSG("mUsingSkillData == null  skillId : " + skillId);
			return;
		}
		ActionData actionDataByName = DataManager.GetActionDataByName($"{modelType}_{skillDataById.ActionName}");
		if (actionDataByName == null)
		{
			Log.DEBUG_MSG("mCurActionData == null  ActionName : " + $"{modelType}_{skillDataById.ActionName}");
			return;
		}
		onSkillFinished = func;
		PlayAnim(skillDataById.ActionName, modelType);
		EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(skillDataById.EffId_0);
		if (effInfoDataById.Target == 1 && !string.IsNullOrEmpty(effInfoDataById.BuffID))
		{
			BuffInfoData buffInfoDataByID = DataManager.GetBuffInfoDataByID(effInfoDataById.BuffID);
			AddPlayeBufEffInfoData(buffInfoDataByID.Effect, 0f, 1f, mFakeObj.transform.position);
		}
		else
		{
			AddPlayEffInfoData(skillDataById.ActionName, modelType, string.Empty, 0f, mFakeObj.transform.position);
		}
	}

	public void PlayAnim(string animationName, string modelType)
	{
		anima = mFakeObj.animation;
		AnimationState animationState = anima[animationName];
		mModelType = modelType;
		if (animationState == null)
		{
			AnimationClip clip = AnimationManager.LoadAnimation(modelType, animationName) as AnimationClip;
			anima.AddClip(clip, animationName);
		}
		if (!anima.IsPlaying(animationName))
		{
			mCurAnimaName = animationName;
			anima.Play(animationName);
		}
	}

	public void AddPlayeBufEffInfoData(string fxeffectInfoId, float delayTime, float duration, Vector3 senderPosition)
	{
		if (GameSettingData.IsShowSkillEffect[GameSettingData.GetPhoneClass()] && !GameSettingData.IsLowPhone && !IsEffectInPlayingList(fxeffectInfoId))
		{
			List<PlayEffInfoData> playBufEffInfoDataList = PlayEffInfoData.GetPlayBufEffInfoDataList(fxeffectInfoId, delayTime, duration, senderPosition);
			if (playBufEffInfoDataList != null)
			{
				AddPlayEffInfoList(playBufEffInfoDataList);
			}
		}
	}

	public void AddPlayEffInfoData(string actionName, string modelType, string effInfoId, float delayTime, Vector3 senderPos, Transform targetTransform = null)
	{
		string actionName2 = $"{modelType}_{actionName}";
		ActionData actionDataByName = DataManager.GetActionDataByName(actionName2);
		if (actionDataByName != null && !IsEffectInPlayingList(actionDataByName.FxEffID))
		{
			List<PlayEffInfoData> playEffInfoDataList = PlayEffInfoData.GetPlayEffInfoDataList(actionName2, effInfoId, delayTime, senderPos, targetTransform);
			AddPlayEffInfoList(playEffInfoDataList);
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
			if (EffectLogic.CacheEffectDic.ContainsKey(effName) && EffectLogic.CacheEffectDic[effName].Count > 0)
			{
				fxControl = EffectLogic.CacheEffectDic[effName][0];
				if (fxControl != null)
				{
					EffectLogic.CacheEffectDic[effName].Remove(fxControl);
					fxControl.Reset(null, playEffInfoData.mFxEffinfoData, playEffInfoData.DurationTime, -777L, GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER);
					AddBindPos(fxControl, playEffInfoData);
				}
			}
			else
			{
				string fxLoadPath = playEffInfoData.fxLoadPath;
				GameObject gameObject = ResourcesManager.LoadAndInstantiate(fxLoadPath) as GameObject;
				if (gameObject != null)
				{
					fxControl = gameObject.GetComponent<FxControl>();
					fxControl.Reset(null, playEffInfoData.mFxEffinfoData, playEffInfoData.DurationTime, -777L, GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER);
					AddBindPos(fxControl, playEffInfoData);
				}
			}
			if (fxControl != null)
			{
				NGUITools.SetLayer(fxControl.gameObject, 22);
				playEffInfoData.fxControl = fxControl;
				PlayEffInfoDataList.Add(playEffInfoData);
			}
			else
			{
				BundleManager.LoadEffectInList(effName, isNeedUnload: false, isDoNotCache: false, OnLoadEffect, playEffInfoData);
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
			value = TransformUtil.FindChildGameObject(mFakeObj, effLinkNode);
			mBindPosDic.Add(effLinkNode, value);
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

	private void OnLoadEffect(object objBundle, object param1 = null, object param2 = null)
	{
		GameObject gameObject = objBundle as GameObject;
		NGUITools.SetLayer(gameObject, 22);
		BundleManager.ResetAllShader(gameObject.transform);
		PlayEffInfoData playEffInfoData = param1 as PlayEffInfoData;
		if (gameObject != null)
		{
			FxControl component = gameObject.GetComponent<FxControl>();
			component.Reset(null, playEffInfoData.mFxEffinfoData, playEffInfoData.DurationTime, -777L, GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER);
			AddBindPos(component, playEffInfoData);
			playEffInfoData.fxControl = component;
			PlayEffInfoDataList.Add(playEffInfoData);
		}
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
		if (!EffectLogic.CurrentPlayEffectDic.ContainsKey(GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER))
		{
			return false;
		}
		List<FxControl> list = EffectLogic.CurrentPlayEffectDic[GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER];
		for (int j = 0; j < list.Count; j++)
		{
			if (list[j].OwnerId == -777 && list[j].EffectId.Equals(effectId))
			{
				return true;
			}
		}
		return false;
	}

	private void Update()
	{
		if (PlayEffInfoDataList.Count > 0)
		{
			for (int num = PlayEffInfoDataList.Count - 1; num > -1; num--)
			{
				PlayEffInfoDataList[num].DelayTime -= Time.deltaTime;
				PlayEffInfoData playEffInfoData = PlayEffInfoDataList[num];
				if (playEffInfoData.DelayTime <= 0f)
				{
					if (playEffInfoData.fxControl != null)
					{
						if (!EffectLogic.CurrentPlayEffectDic.ContainsKey(playEffInfoData.fxControl.OwnerType))
						{
							EffectLogic.CurrentPlayEffectDic.Add(playEffInfoData.fxControl.OwnerType, new List<FxControl>());
						}
						EffectLogic.CurrentPlayEffectDic[playEffInfoData.fxControl.OwnerType].Add(playEffInfoData.fxControl);
						if (playEffInfoData.mFxEffinfoData.AutoMoveFlag)
						{
							playEffInfoData.fxControl.Play(mFakeObj.transform.position + mFakeObj.transform.forward * 5f);
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
		if (onSkillFinished != null && !anima.IsPlaying(mCurAnimaName))
		{
			onSkillFinished();
			onSkillFinished = null;
		}
	}
}
