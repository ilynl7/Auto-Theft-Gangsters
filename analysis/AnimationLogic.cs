using System.Collections.Generic;
using UnityEngine;

public class AnimationLogic : MonoBehaviour
{
	public delegate void OnAnimFinished();

	public string[] animationName = new string[6] { "idle", "run", "walk", "die", "idle_attack", "kaiChe" };

	private Animation mAnimation;

	private AnimationState mCurAnimationState;

	private string mCurAnimationName = string.Empty;

	private string mNextActionName = string.Empty;

	private ActionData mCurActionData;

	private float mCurActionCrossOutTime;

	private ActionData mPreActionData;

	private float mDuration;

	public string idleAction = "idle";

	public string idleShowAction = "show";

	public ActionData idleShowActionData;

	public string walkAction = "walk";

	public string runAction = "run";

	public string attackRunAction = "run_naQiang";

	public string idleAttackAction = "idle_attack";

	public string getUpAction = "getUp";

	public string dieAction = "die";

	public string idleAnimationName = "idle";

	public string knockDownAnimationName = "knockDown";

	public List<string> beatonAnimationNameList = new List<string> { "attack_P", "attack_p" };

	public OnAnimFinished onAnimFinished;

	public OnAnimFinished tempAnimFinished;

	private ObjCharacter owner;

	private bool IsStunFlag;

	private bool IsKnockDownFlag;

	private bool IsSleepFlag;

	private float mCurAnimationStartTime;

	private float mCurAnimationLength;

	private float mIdleTimeCount;

	private float mIdleShowTime = 10f;

	private bool IsIdleFlag;

	private ActionData mNpcBeforeLoadAction;

	private SoundManager mSoundManager;

	public List<PlayAnimationData> PlayAnimationDataList = new List<PlayAnimationData>();

	private AnimationFit mAnimaFitCtl;

	private string talkActionName = "talk";

	public Animation AnimaObj => mAnimation;

	public AnimationState CurrentAnimationState => mCurAnimationState;

	public string CurrentAnimationName => mCurAnimationName;

	public string NextActionName
	{
		get
		{
			return mNextActionName;
		}
		set
		{
			mNextActionName = value;
		}
	}

	public ActionData CurActionData
	{
		get
		{
			return mCurActionData;
		}
		set
		{
			mCurActionData = value;
		}
	}

	public float Duration => mDuration;

	public float CurAnimationLength => mCurAnimationLength;

	public void AddPlayAnimationData(string actionName, float delayTime, string effInfoID)
	{
		if (owner.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || owner.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER || owner.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_PLAYER || owner.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL)
		{
			PlayAnimationDataList.Add(new PlayAnimationData(actionName, delayTime, effInfoID));
		}
		else
		{
			PlayAnimationDataList.Add(new PlayAnimationData(owner.IndexName + "_" + actionName, delayTime, effInfoID));
		}
	}

	public void Init(ObjCharacter owner)
	{
		if (mAnimation == null)
		{
			mAnimation = GetComponent<Animation>();
		}
		if (mAnimation == null)
		{
			mAnimation = GetComponentInChildren<Animation>();
		}
		this.owner = owner;
		if (mAnimation != null)
		{
			RegisterEvent();
			LoadDefaultAnima();
			mAnimaFitCtl = mAnimation.gameObject.GetComponent<AnimationFit>();
		}
		if (mNpcBeforeLoadAction != null)
		{
			if (mNpcBeforeLoadAction.AnimationWrapMode == WrapMode.ClampForever)
			{
				PlayAnimation(mNpcBeforeLoadAction, null, -1f, 0.9f);
			}
			else
			{
				PlayAnimation(mNpcBeforeLoadAction, null, -1f, 0f);
			}
			mNpcBeforeLoadAction = null;
		}
		if (mSoundManager == null)
		{
			mSoundManager = SingletonDontDestoryUnity<SoundManager>.Instance;
		}
	}

	private void LoadDefaultAnima()
	{
		ActionData actionDataByName = DataManager.GetActionDataByName(owner.GetActionNameNoName("idle"));
		LoadAnim(actionDataByName);
		actionDataByName = DataManager.GetActionDataByName(owner.GetActionNameNoName("run"));
		LoadAnim(actionDataByName);
	}

	public void PlayAnimation(int id, OnAnimFinished animFinished = null)
	{
		if (owner.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || owner.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER || owner.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_PLAYER || owner.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL)
		{
			PlayAnimation(animationName[id], animFinished, -1f);
		}
		else
		{
			PlayAnimation(DataManager.GetActionDataByName(animationName[id]), animFinished, -1f, 0f);
		}
	}

	public void PlayAnimation(string actionName, OnAnimFinished animFinished = null, float duration = -1f)
	{
		ActionData actionDataByName = DataManager.GetActionDataByName(owner.GetActionName(actionName));
		if (actionDataByName == null)
		{
			Debug.Log("actionName == null !!!! actionName :: " + actionName);
		}
		else
		{
			PlayAnimation(actionDataByName, animFinished, duration, 0f);
		}
	}

	public void PlayAnimation(ActionData actionData, OnAnimFinished animFinished = null, float duration = -1f, float startTime = 0f)
	{
		if (actionData == null)
		{
			return;
		}
		if (mAnimation == null)
		{
			if (actionData.AnimationWrapMode == WrapMode.Loop || actionData.AnimationWrapMode == WrapMode.PingPong || actionData.AnimationWrapMode == WrapMode.ClampForever)
			{
				mNpcBeforeLoadAction = actionData;
			}
			else
			{
				mNpcBeforeLoadAction = null;
			}
		}
		else
		{
			InternalPlayAnimation(actionData, animFinished, duration, startTime);
		}
	}

	private void InternalPlayAnimation(ActionData actionData, OnAnimFinished animFinished = null, float duration = -1f, float startTime = 0f)
	{
		if (actionData == null || IsKnockDownFlag || (!actionData.AnimName.Equals(dieAction) && !actionData.AnimName.Equals(knockDownAnimationName) && (IsStunFlag || IsSleepFlag)))
		{
			return;
		}
		mPreActionData = mCurActionData;
		mCurActionData = actionData;
		if (!SetCurAnimationState(mCurActionData))
		{
			return;
		}
		mCurAnimationName = GetAnimationStateName(actionData);
		mCurAnimationLength = mAnimation[mCurAnimationName].length;
		if (actionData.AnimDurationTimeSecond > float.Epsilon)
		{
			mAnimation[mCurAnimationName].speed = mAnimation[mCurAnimationName].length / actionData.AnimDurationTimeSecond;
			mCurAnimationLength = actionData.AnimDurationTimeSecond;
		}
		if (startTime > float.Epsilon)
		{
			mAnimation[mCurAnimationName].normalizedTime = startTime;
		}
		if (mAnimaFitCtl != null)
		{
			mAnimaFitCtl.UpdateAnim(mCurAnimationName, GetCrossFadeTime());
		}
		mAnimation.CrossFade(mCurAnimationName, GetCrossFadeTime());
		PlaySoundAtPos(actionData.SoundID, owner.Position);
		if (mCurActionData.CrossOutTime != -1)
		{
			mCurActionCrossOutTime = mCurActionData.CrossOutTimeSecond;
		}
		else
		{
			mCurActionCrossOutTime = GameSettingData.DefaultCrossOutTime;
		}
		CheckAnimaState(mCurAnimationName);
		mCurAnimationStartTime = Time.time;
		if (onAnimFinished != null)
		{
			onAnimFinished();
		}
		onAnimFinished = animFinished;
		mDuration = duration;
		if (actionData.NextActionName != string.Empty)
		{
			mNextActionName = actionData.NextActionName;
		}
		else if (actionData.AnimationWrapMode == WrapMode.Loop || actionData.AnimationWrapMode == WrapMode.ClampForever)
		{
			if (mDuration < 0f)
			{
				mNextActionName = string.Empty;
			}
			else if (!owner.IdleAttackFlag)
			{
				mNextActionName = owner.GetActionNameNoName(idleAction);
			}
			else
			{
				mNextActionName = owner.GetActionNameNoName(idleAttackAction);
			}
		}
		else if (!owner.IdleAttackFlag)
		{
			mNextActionName = owner.GetActionNameNoName(idleAction);
		}
		else
		{
			mNextActionName = owner.GetActionNameNoName(idleAttackAction);
		}
	}

	public string GetAnimationStateName(ActionData actionData)
	{
		if (owner.CurrentCharacterModelData.TypeID == 0)
		{
			return $"{actionData.GetAnimaFileName()}_{actionData.AnimName}";
		}
		return actionData.AnimName;
	}

	private float GetCrossFadeTime()
	{
		float result = GameSettingData.DefaultCrossInTime;
		if (mCurActionData.CrossInTime != -1)
		{
			result = ((mPreActionData == null || mPreActionData.CrossOutTime == -1) ? mCurActionData.CrossInTimeSecond : ((!(mPreActionData.CrossOutTimeSecond > mCurActionData.CrossInTimeSecond)) ? mCurActionData.CrossInTimeSecond : mPreActionData.CrossOutTimeSecond));
		}
		else if (mPreActionData != null && mPreActionData.CrossOutTime != -1)
		{
			result = mPreActionData.CrossOutTimeSecond;
		}
		return result;
	}

	public bool LoadAnim(ActionData actionData)
	{
		if (mAnimation[GetAnimationStateName(actionData)] != null)
		{
			return true;
		}
		if (owner.CurrentCharacterModelData.TypeID == 0)
		{
			AnimationClip animationClip = AnimationManager.LoadAnimation(actionData.GetAnimaFileName(), actionData.AnimName) as AnimationClip;
			if (animationClip != null)
			{
				animationClip.wrapMode = actionData.AnimationWrapMode;
				mAnimation.AddClip(animationClip, $"{actionData.GetAnimaFileName()}_{actionData.AnimName}");
				return true;
			}
			return false;
		}
		AnimationClip animationClip2 = AnimationManager.LoadAnimation(owner.CurrentCharacterModelData.ModelFirstType, actionData.AnimName) as AnimationClip;
		if (animationClip2 != null)
		{
			animationClip2.wrapMode = actionData.AnimationWrapMode;
			mAnimation.AddClip(animationClip2, actionData.AnimName);
			return true;
		}
		animationClip2 = AnimationManager.LoadAnimation(owner.CurrentCharacterModelData.ModelSubType, actionData.AnimName) as AnimationClip;
		if (animationClip2 != null)
		{
			animationClip2.wrapMode = actionData.AnimationWrapMode;
			mAnimation.AddClip(animationClip2, actionData.AnimName);
			return true;
		}
		animationClip2 = AnimationManager.LoadAnimation(owner.CurrentCharacterModelData.ModelType, actionData.AnimName) as AnimationClip;
		if (animationClip2 != null)
		{
			animationClip2.wrapMode = actionData.AnimationWrapMode;
			mAnimation.AddClip(animationClip2, actionData.AnimName);
			return true;
		}
		return false;
	}

	public void ForceSampleAnimation(string actionName, float normalizedTime, OnAnimFinished animFinished = null, float duration = -1f, float startTime = 0f)
	{
		ActionData actionDataByName = DataManager.GetActionDataByName(owner.GetActionNameNoName(actionName));
		if (actionDataByName != null)
		{
			string animationStateName = GetAnimationStateName(actionDataByName);
			mAnimation.Stop(animationStateName);
			if (!(mAnimation[animationStateName] == null) || LoadAnim(actionDataByName))
			{
				mAnimation.Play(animationStateName);
				mAnimation[animationStateName].normalizedTime = normalizedTime;
				mAnimation.Sample();
				mAnimation.Stop();
			}
		}
	}

	public void ForcePlayAnimation(string actionName, OnAnimFinished animFinished = null, float duration = -1f, float startTime = 0f)
	{
		ActionData actionData = null;
		actionData = DataManager.GetActionDataByName(owner.GetActionNameNoName(actionName));
		if (mAnimation == null)
		{
			if (actionData.AnimationWrapMode == WrapMode.Loop || actionData.AnimationWrapMode == WrapMode.PingPong || actionData.AnimationWrapMode == WrapMode.ClampForever)
			{
				mNpcBeforeLoadAction = actionData;
			}
			else
			{
				mNpcBeforeLoadAction = null;
			}
		}
		else
		{
			mAnimation.Stop(GetAnimationStateName(actionData));
			InternalPlayAnimation(actionData, animFinished, duration, startTime);
		}
	}

	private void GoNextAnimation(string actionName)
	{
		mPreActionData = mCurActionData;
		mCurActionData = DataManager.GetActionDataByName(actionName);
		mCurAnimationName = GetAnimationStateName(mCurActionData);
		if (SetCurAnimationState(mCurActionData))
		{
			if (mCurActionData.CrossOutTime != -1)
			{
				mCurActionCrossOutTime = mCurActionData.CrossOutTimeSecond;
			}
			else
			{
				mCurActionCrossOutTime = GameSettingData.DefaultCrossOutTime;
			}
			if (mAnimaFitCtl != null)
			{
				mAnimaFitCtl.UpdateAnim(mCurAnimationName, GetCrossFadeTime());
			}
			mAnimation.CrossFade(mCurAnimationName, GetCrossFadeTime());
			PlaySoundAtPos(mCurActionData.SoundID, owner.Position);
			CheckAnimaState(mCurAnimationName);
			mCurAnimationStartTime = Time.time;
			mCurAnimationLength = mAnimation[mCurAnimationName].length;
			mNextActionName = string.Empty;
		}
	}

	private void CheckAnimaState(string animationName)
	{
		if (animationName.Equals(idleAnimationName))
		{
			IsIdleFlag = true;
			return;
		}
		IsIdleFlag = false;
		mIdleTimeCount = 0f;
	}

	private bool SetCurAnimationState(ActionData curActData)
	{
		mCurAnimationState = mAnimation[GetAnimationStateName(curActData)];
		if (mCurAnimationState == null)
		{
			if (!LoadAnim(curActData))
			{
				return false;
			}
			mCurAnimationState = mAnimation[GetAnimationStateName(curActData)];
		}
		mCurAnimationState.wrapMode = curActData.AnimationWrapMode;
		return true;
	}

	public void CheckAnimationState()
	{
		if (mNextActionName != string.Empty)
		{
			if (mDuration < 0f)
			{
				if (CalCurAnimaLeftTime() < mCurActionCrossOutTime)
				{
					if (onAnimFinished != null)
					{
						tempAnimFinished = onAnimFinished;
						onAnimFinished = null;
						tempAnimFinished();
						return;
					}
					GoNextAnimation(mNextActionName);
				}
			}
			else
			{
				mDuration -= Time.deltaTime;
				if (mDuration <= 0f)
				{
					OnAnimaFinished();
					GoNextAnimation(mNextActionName);
					mDuration = -1f;
				}
			}
		}
		else if (onAnimFinished != null && CalCurAnimaLeftTime() < mCurActionCrossOutTime)
		{
			tempAnimFinished = onAnimFinished;
			onAnimFinished = null;
			tempAnimFinished();
		}
		if (PlayAnimationDataList.Count != 0)
		{
			for (int num = PlayAnimationDataList.Count - 1; num >= 0; num--)
			{
				PlayAnimationDataList[num].DelayTime -= Time.deltaTime;
				if (PlayAnimationDataList[num].DelayTime <= 0f)
				{
					if (!owner.IsDie && mCurActionData.AnimCanBeBreak == 1)
					{
						ForcePlayAnimation(PlayAnimationDataList[num].ActionName, null, -1f, 0f);
						OnAnimaFinished();
					}
					PlayAnimationDataList.RemoveAt(num);
				}
			}
		}
		if (mCurAnimationState == null)
		{
			if (!owner.IsDie && !owner.IdleAttackFlag)
			{
				GoNextAnimation(owner.GetActionNameNoName(idleAction));
			}
			else
			{
				GoNextAnimation(owner.GetActionNameNoName(idleAttackAction));
			}
		}
		if (!mAnimation.IsPlaying(mCurAnimationName))
		{
			if (mCurActionData.AnimName.Equals(walkAction) || mCurActionData.AnimName.Equals(runAction) || mCurActionData.AnimName.Equals(attackRunAction) || mCurActionData.AnimName.Equals(dieAction))
			{
				GoNextAnimation(mCurActionData.ID);
			}
			else if (!owner.IdleAttackFlag)
			{
				OnAnimaFinished();
				GoNextAnimation(owner.GetActionNameNoName(idleAction));
			}
			else
			{
				OnAnimaFinished();
				GoNextAnimation(owner.GetActionNameNoName(idleAttackAction));
			}
		}
		if (onAnimFinished != null || (owner.ObjType != GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER && owner.ObjType != GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER) || !IsIdleFlag)
		{
			return;
		}
		mIdleTimeCount += Time.deltaTime;
		if (mIdleTimeCount > mIdleShowTime)
		{
			mIdleTimeCount = 0f;
			mIdleShowTime = Random.Range(10, 20);
			if (Random.Range(0, 100) > 70)
			{
				PlayAnimation(idleShowAction, null, -1f);
			}
		}
	}

	public void OnAnimaFinished()
	{
		if (onAnimFinished != null)
		{
			tempAnimFinished = onAnimFinished;
			onAnimFinished = null;
			tempAnimFinished();
		}
	}

	private float CalCurAnimaNormalizedTime()
	{
		return Mathf.Clamp01((Time.time - mCurAnimationStartTime) / mCurAnimationLength);
	}

	private float CalCurAnimaLeftTime()
	{
		return mCurAnimationLength - (Time.time - mCurAnimationStartTime);
	}

	public bool AnimationPlayCheck(PlayAnimationData playAnimationData)
	{
		if (mCurActionData.AnimCanBeBreak == 1)
		{
			return true;
		}
		EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(playAnimationData.EffInfoID);
		if (effInfoDataById.MoveDistance != 0 && effInfoDataById.ForceMove != 1)
		{
			return true;
		}
		return false;
	}

	public void AnimationLogicUpdate()
	{
		if (!(mAnimation == null) && base.enabled)
		{
			CheckAnimationState();
		}
	}

	public void DisableAnimationLogic()
	{
		base.enabled = false;
		if (mAnimation != null)
		{
			mAnimation.Stop();
		}
	}

	public void EnableAnimationLogic()
	{
		base.enabled = true;
	}

	public void BreakCurAnima()
	{
		if (mCurActionData != null)
		{
			mSoundManager.StopSoundEffect(mCurActionData.SoundID);
		}
		if (!owner.IsDie)
		{
			if (!owner.IdleAttackFlag)
			{
				GoNextAnimation(owner.GetActionNameNoName(idleAction));
			}
			else
			{
				GoNextAnimation(owner.GetActionNameNoName(idleAttackAction));
			}
		}
	}

	public void OnStun(BuffInfoData buffInfoData)
	{
		if (!owner.IsDie)
		{
			if (owner.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || owner.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER || owner.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_PLAYER || owner.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL)
			{
				ForcePlayAnimation(buffInfoData.Action, null, -1f, 0f);
			}
			else
			{
				ForcePlayAnimation($"{owner.IndexName}_{buffInfoData.Action}", null, -1f, 0f);
			}
			IsStunFlag = true;
		}
	}

	public void OnStunDone(BuffInfoData buffInfoData)
	{
		IsStunFlag = false;
		if (!owner.IsDie)
		{
			GoNextAnimation(owner.GetActionNameNoName(idleAttackAction));
		}
	}

	public void OnKnockDown(BuffInfoData buffInfoData, ObjCharacter sender)
	{
		if (!owner.IsDie)
		{
			if (owner.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || owner.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER || owner.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_PLAYER || owner.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL)
			{
				ForcePlayAnimation(buffInfoData.Action, null, -1f, 0f);
			}
			else
			{
				ForcePlayAnimation($"{owner.IndexName}_{buffInfoData.Action}", null, -1f, 0f);
			}
			IsKnockDownFlag = true;
		}
	}

	public void OnKnockDownDone(BuffInfoData buffInfoData)
	{
		IsKnockDownFlag = false;
		if (!owner.IsDie)
		{
			GoNextAnimation(owner.GetActionName(getUpAction));
		}
	}

	public void OnSleep(BuffInfoData buffInfoData)
	{
		if (!owner.IsDie)
		{
			IsSleepFlag = true;
		}
	}

	public void OnSleepDone(BuffInfoData buffInfoData)
	{
		IsSleepFlag = false;
		if (!owner.IsDie)
		{
			GoNextAnimation(owner.GetActionNameNoName(idleAction));
		}
	}

	private void Destroy()
	{
		DeRegisterEvent();
	}

	private void RegisterEvent()
	{
		owner.BuffLogic.RegisterOnStun(OnStun);
		owner.BuffLogic.RegisterOnStunDone(OnStunDone);
		owner.BuffLogic.RegisterOnSleep(OnSleep);
		owner.BuffLogic.RegisterOnSleepDone(OnSleepDone);
		owner.BuffLogic.RegisterOnKnockDown(OnKnockDown);
		owner.BuffLogic.RegisterOnKnockDownDone(OnKnockDownDone);
	}

	private void DeRegisterEvent()
	{
		owner.BuffLogic.DeRegisterOnStun(OnStun);
		owner.BuffLogic.DeRegisterOnStunDone(OnStunDone);
		owner.BuffLogic.DeRegisterOnSleep(OnSleep);
		owner.BuffLogic.DeRegisterOnSleepDone(OnSleepDone);
		owner.BuffLogic.DeRegisterOnKnockDown(OnKnockDown);
		owner.BuffLogic.DeRegisterOnKnockDownDone(OnKnockDownDone);
	}

	public void PrintPLayerMessage(string message)
	{
		if (owner.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			Debug.Log(message);
		}
	}

	public void PlayTalkAnimation()
	{
		ForcePlayAnimation(talkActionName, null, -1f, 0f);
	}

	public void ClearAllAnima()
	{
		if (!(mAnimation != null))
		{
			return;
		}
		mAnimation.Stop();
		foreach (AnimationState item in mAnimation)
		{
			mAnimation.RemoveClip(item.name);
		}
	}

	public void ResetAnimationFlag()
	{
		IsStunFlag = false;
		IsSleepFlag = false;
		IsKnockDownFlag = false;
	}

	private void PlaySoundAtPos(int nSoundID, Vector3 playingPos)
	{
		if (mSoundManager == null)
		{
			if (SingletonDontDestoryUnity<SoundManager>.Exists)
			{
				mSoundManager = SingletonDontDestoryUnity<SoundManager>.Instance;
			}
			if (mSoundManager == null)
			{
				return;
			}
		}
		if (owner.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			mSoundManager.PlaySoundEffect(nSoundID);
		}
		else if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			mSoundManager.PlaySoundEffectAtPos2(nSoundID, playingPos, Singleton<ObjManager>.Instance.MainPlayer.Position);
		}
		else
		{
			mSoundManager.PlaySoundEffect(nSoundID);
		}
	}
}
