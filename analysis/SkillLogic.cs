using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SkillLogic
{
	private ObjCharacter mSkillSender;

	public bool mIsUsingSkill;

	private int mLastSkillId = -1;

	private SkillData mUsingSkillData;

	private ActionData mCurActionData;

	private vp_Timer.Handle handle = new vp_Timer.Handle();

	private List<SkillEffInfoData> mSkillEffectList = new List<SkillEffInfoData>();

	private List<ObjCharacter> mTempEffectTargetList = new List<ObjCharacter>();

	private use_skill_buff.request sendServerBuffRequest = new use_skill_buff.request();

	private List<buff> sendServerBuffList = new List<buff>();

	private accept_damge.request request1 = new accept_damge.request();

	private List<acceptdamge> damagelist = new List<acceptdamge>();

	private local_character_attack.request request = new local_character_attack.request();

	public ObjCharacter SkillSender
	{
		get
		{
			return mSkillSender;
		}
		set
		{
			mSkillSender = value;
		}
	}

	public bool IsUsingSkill
	{
		get
		{
			return mIsUsingSkill;
		}
		set
		{
			mIsUsingSkill = value;
		}
	}

	public int LastSkillId
	{
		get
		{
			return mLastSkillId;
		}
		set
		{
			mLastSkillId = value;
		}
	}

	public SkillData UsingSkillData
	{
		get
		{
			return mUsingSkillData;
		}
		set
		{
			mUsingSkillData = value;
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

	public bool ServerUseSkill(string skillId, long senderId, long targetId, List<attack_list> attackList)
	{
		mSkillSender = Singleton<ObjManager>.Instance.FindObjInScene(senderId);
		if (mSkillSender == null)
		{
			Log.DEBUG_MSG("SkillSender is null" + senderId);
			return false;
		}
		if (mSkillSender.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC && (mSkillSender as ObjNPC).MeshRoot == null)
		{
			return false;
		}
		if (mIsUsingSkill && CheckSkillCanBeBreak(skillId))
		{
			BreakCurSkill();
		}
		if (mSkillSender.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
		{
			ObjOtherPlayer objOtherPlayer = mSkillSender as ObjOtherPlayer;
			if (objOtherPlayer.IsDrivingMount())
			{
				objOtherPlayer.DisMountCar();
			}
		}
		mUsingSkillData = DataManager.GetSkillDataById(skillId);
		if (mUsingSkillData == null)
		{
			Log.DEBUG_MSG("mUsingSkillData == null  skillId : " + skillId);
			mSkillSender.OnSkillUseFail(skillId);
			return false;
		}
		mCurActionData = DataManager.GetActionDataByName(mSkillSender.GetActionName(mUsingSkillData.ActionName));
		if (mCurActionData == null)
		{
			Log.DEBUG_MSG("mCurActionData == null  ActionName : " + mSkillSender.GetActionName(mUsingSkillData.ActionName));
			mSkillSender.OnSkillUseFail(skillId);
			return false;
		}
		if (mSkillSender.IsDie)
		{
			mSkillSender.OnSkillUseFail(skillId);
			return false;
		}
		mIsUsingSkill = true;
		if (mSkillSender.IsMoving)
		{
			mSkillSender.StopMove();
		}
		mSkillSender.OnSkillUseSuccess(skillId);
		ObjCharacter objCharacter = null;
		EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(UsingSkillData.EffId_0);
		if (targetId != -1 && targetId != senderId && effInfoDataById != null && IsNeedFaceTarget(effInfoDataById))
		{
			objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(targetId);
			if (objCharacter != null)
			{
				SkillSender.FaceToPub(objCharacter.Position);
			}
		}
		float num = 0f;
		float durationTime = -1f;
		if (mUsingSkillData.MoveTime != 0)
		{
			mSkillSender.SkillMotion.ResetSkillMove(mUsingSkillData, mUsingSkillData.CastTime, objCharacter);
		}
		if (mUsingSkillData.CastTime > 0)
		{
			Log.ERROR_MSG("Server Skill Can not contains CastTime!!!!!!!!!!!!!!!!!!");
			PlayAnimation(mUsingSkillData.CastAction, null, -1f);
			mSkillSender.PlayYinChangeEffInfo(mUsingSkillData.EffId_0, mUsingSkillData.CastTimeSecond, mSkillSender.Position);
			vp_Timer.In(mUsingSkillData.CastTimeSecond, delegate
			{
				PlayAnimation(mUsingSkillData.ActionName, null, durationTime);
			}, handle);
			num = mUsingSkillData.CastTimeSecond;
		}
		else
		{
			PlayAnimation(mUsingSkillData.ActionName, null, durationTime);
			num = 0f;
		}
		if (objCharacter != null)
		{
			mSkillSender.AddPlayEffInfoData(mUsingSkillData.ActionName, string.Empty, num, mSkillSender.Position, objCharacter.transform);
		}
		else
		{
			mSkillSender.AddPlayEffInfoData(mUsingSkillData.ActionName, string.Empty, num, mSkillSender.Position);
		}
		ResetSenderEffInfoList(mUsingSkillData, num, mSkillSender);
		if (!string.IsNullOrEmpty(mUsingSkillData.EffId_0))
		{
			mSkillEffectList.Add(new SkillEffInfoData(mUsingSkillData.ID, mUsingSkillData.EffId_0, num + mUsingSkillData.EffTime_0Second, targetId, attackList, isServerAttack: true));
		}
		if (!string.IsNullOrEmpty(mUsingSkillData.EffId_1))
		{
			mSkillEffectList.Add(new SkillEffInfoData(mUsingSkillData.ID, mUsingSkillData.EffId_1, num + mUsingSkillData.EffTime_1Second, targetId, attackList, isServerAttack: true));
		}
		if (!string.IsNullOrEmpty(mUsingSkillData.EffId_2))
		{
			mSkillEffectList.Add(new SkillEffInfoData(mUsingSkillData.ID, mUsingSkillData.EffId_2, num + mUsingSkillData.EffTime_2Second, targetId, attackList, isServerAttack: true));
		}
		CharacterSkillData characterSkillDataByID = mSkillSender.GetCharacterSkillDataByID(skillId);
		if (characterSkillDataByID != null)
		{
			characterSkillDataByID.CDTimeCount = mUsingSkillData.CDSecond;
		}
		mSkillSender.HoldTimeCount = mUsingSkillData.HoldTimeSecond;
		if (mSkillSender.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			CameraOpt();
		}
		return true;
	}

	public bool IsNeedFaceTarget(EffInfoData effInfoData)
	{
		if ((effInfoData.AreaType == 0 || effInfoData.AreaType == 1 || effInfoData.AreaType == 3 || effInfoData.AreaType == 2 || effInfoData.AreaType == 4) && effInfoData.Target != 1)
		{
			return true;
		}
		return false;
	}

	public void ResetSkillLogic()
	{
		mIsUsingSkill = false;
		mSkillEffectList.Clear();
	}

	public bool UseSkill(string skillId, long senderId, long targetId)
	{
		if (mIsUsingSkill)
		{
			if (!CheckSkillCanBeBreak(skillId))
			{
				return false;
			}
			BreakCurSkill();
		}
		mSkillSender = Singleton<ObjManager>.Instance.FindObjInScene(senderId);
		if (mSkillSender == null)
		{
			Log.DEBUG_MSG("SkillSender is null" + senderId);
			return false;
		}
		mUsingSkillData = DataManager.GetSkillDataById(skillId);
		if (mUsingSkillData == null)
		{
			Log.DEBUG_MSG("mUsingSkillData == null  skillId : " + skillId);
			mSkillSender.OnSkillUseFail(skillId);
			return false;
		}
		mCurActionData = DataManager.GetActionDataByName(mSkillSender.GetActionName(mUsingSkillData.ActionName));
		if (mCurActionData == null)
		{
			Log.DEBUG_MSG("mCurActionData == null  ActionName : " + mSkillSender.IndexName + "_" + mUsingSkillData.ActionName);
			mSkillSender.OnSkillUseFail(skillId);
			return false;
		}
		if (mSkillSender.IsDie)
		{
			Log.DEBUG_MSG("PlayerDie");
			mSkillSender.OnSkillUseFail(skillId);
			return false;
		}
		mIsUsingSkill = true;
		if (mSkillSender.IsMoving)
		{
			mSkillSender.StopMove();
		}
		mSkillSender.OnSkillUseSuccess(skillId);
		ObjCharacter objCharacter = null;
		EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(UsingSkillData.EffId_0);
		if (targetId != -1 && targetId != senderId && effInfoDataById != null && IsNeedFaceTarget(effInfoDataById))
		{
			objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(targetId);
			if (objCharacter != null)
			{
				SkillSender.FaceToPub(objCharacter.Position);
			}
		}
		float num = 0f;
		float durationTime = -1f;
		if (mUsingSkillData.MoveTime != 0)
		{
			mSkillSender.SkillMotion.ResetSkillMove(mUsingSkillData, mUsingSkillData.CastTime, objCharacter);
		}
		if (mUsingSkillData.CastTime > 0)
		{
			PlayAnimation(mUsingSkillData.CastAction, null, -1f);
			mSkillSender.PlayYinChangeEffInfo(mUsingSkillData.EffId_0, mUsingSkillData.CastTimeSecond, mSkillSender.Position);
			vp_Timer.In(mUsingSkillData.CastTimeSecond, delegate
			{
				PlayAnimation(mUsingSkillData.ActionName, null, durationTime);
			}, handle);
			num = mUsingSkillData.CastTimeSecond;
		}
		else
		{
			PlayAnimation(mUsingSkillData.ActionName, null, durationTime);
			num = 0f;
		}
		if (objCharacter != null)
		{
			mSkillSender.AddPlayEffInfoData(mUsingSkillData.ActionName, string.Empty, num, mSkillSender.Position, objCharacter.transform);
		}
		else
		{
			mSkillSender.AddPlayEffInfoData(mUsingSkillData.ActionName, string.Empty, num, mSkillSender.Position);
		}
		ResetSenderEffInfoList(mUsingSkillData, num, mSkillSender);
		if (!string.IsNullOrEmpty(mUsingSkillData.EffId_0))
		{
			mSkillEffectList.Add(new SkillEffInfoData(mUsingSkillData.ID, mUsingSkillData.EffId_0, num + mUsingSkillData.EffTime_0Second, targetId));
		}
		if (!string.IsNullOrEmpty(mUsingSkillData.EffId_1))
		{
			mSkillEffectList.Add(new SkillEffInfoData(mUsingSkillData.ID, mUsingSkillData.EffId_1, num + mUsingSkillData.EffTime_1Second, targetId));
		}
		if (!string.IsNullOrEmpty(mUsingSkillData.EffId_2))
		{
			mSkillEffectList.Add(new SkillEffInfoData(mUsingSkillData.ID, mUsingSkillData.EffId_2, num + mUsingSkillData.EffTime_2Second, targetId));
		}
		CharacterSkillData characterSkillDataByID = mSkillSender.GetCharacterSkillDataByID(skillId);
		if (characterSkillDataByID != null)
		{
			characterSkillDataByID.CDTimeCount = mUsingSkillData.CDSecond;
		}
		mSkillSender.HoldTimeCount = mUsingSkillData.HoldTimeSecond;
		if (mSkillSender.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC || mSkillSender.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			CameraOpt();
		}
		return true;
	}

	private void CameraOpt()
	{
		if (mUsingSkillData == null || string.IsNullOrEmpty(mUsingSkillData.CamRockID))
		{
			return;
		}
		CameraController cameraController = Singleton<ObjManager>.Instance.MainPlayer.CameraController;
		if (cameraController != null)
		{
			for (int i = 0; i < mUsingSkillData.CamRockIDList.Length; i++)
			{
				cameraController.AddCamRock(mUsingSkillData.CamRockIDList[i]);
			}
		}
	}

	public void SkillFinished()
	{
		mIsUsingSkill = false;
		mSkillSender.CurUseSkillId = string.Empty;
		mLastSkillId = -1;
		mSkillSender.OnSkillFinished();
	}

	public void SetSkillEffect(EffInfoData effInfoData, float delayTime, ObjCharacter target, bool isBufSuccess)
	{
		if (effInfoData.HitAction != string.Empty)
		{
			if (CheckEffectAdd(effInfoData, target))
			{
				target.AddPlayAnimationData(effInfoData.HitAction, delayTime, effInfoData.ID);
			}
			target.AddPlayEffInfoData(effInfoData.HitAction, effInfoData.ID, delayTime, SkillSender.Position);
			target.FlashSkin();
		}
		if (effInfoData.ForceMove == 0 && effInfoData.MoveTime != 0)
		{
			target.AddPlayerEffMotionData(effInfoData.ID, delayTime, SkillSender.Position);
		}
		if (!string.IsNullOrEmpty(effInfoData.BuffID) && isBufSuccess)
		{
			target.AddBuffInfoData(effInfoData.BuffID, delayTime, effInfoData.BuffDurationSecond, mSkillSender);
		}
	}

	public void ResetSenderEffInfoList(SkillData skillData, float preActionDelayTime, ObjCharacter target)
	{
		if (!string.IsNullOrEmpty(skillData.EffId_0))
		{
			SetSenderSkillEffect(skillData, DataManager.GetEffInfoDataById(skillData.EffId_0), preActionDelayTime + skillData.EffTime_0Second, target);
		}
		if (!string.IsNullOrEmpty(skillData.EffId_1))
		{
			SetSenderSkillEffect(skillData, DataManager.GetEffInfoDataById(skillData.EffId_1), preActionDelayTime + skillData.EffTime_1Second, target);
		}
		if (!string.IsNullOrEmpty(skillData.EffId_2))
		{
			SetSenderSkillEffect(skillData, DataManager.GetEffInfoDataById(skillData.EffId_2), preActionDelayTime + skillData.EffTime_2Second, target);
		}
	}

	public void SetSenderSkillEffect(SkillData skData, EffInfoData effInfoData, float delayTime, ObjCharacter target)
	{
		if (effInfoData.ForceMove == 1 && effInfoData.MoveTime != 0)
		{
			target.AddPlayerEffMotionData(effInfoData.ID, delayTime, target.Position);
		}
	}

	public void UpdateSkill()
	{
		if (mSkillEffectList.Count != 0)
		{
			for (int num = mSkillEffectList.Count - 1; num >= 0; num--)
			{
				mSkillEffectList[num].DelayTime -= Time.deltaTime;
				if (mSkillEffectList[num].DelayTime <= 0f)
				{
					SetSkillEffectTarget(mSkillEffectList[num]);
					if (mSkillEffectList.Count > 0)
					{
						mSkillEffectList.RemoveAt(num);
					}
				}
			}
		}
		if (mIsUsingSkill && mSkillSender.AnimationLogic.CurActionData != mCurActionData && !mSkillSender.AnimationLogic.CurActionData.AnimName.Equals(mUsingSkillData.CastAction))
		{
			SkillFinished();
		}
	}

	public List<ObjCharacter> GetCandidateList(GameDefine.CAMP_TYPE camp)
	{
		return Singleton<ObjManager>.Instance.CampTargetList[(int)camp];
	}

	public void SetSkillEffectTarget(SkillEffInfoData skillEffInfoData)
	{
		SkillData skillDataById = DataManager.GetSkillDataById(skillEffInfoData.skillId);
		EffInfoData effInfoDataById = DataManager.GetEffInfoDataById(skillEffInfoData.EffInfoId);
		ObjCharacter target = Singleton<ObjManager>.Instance.FindObjInScene(skillEffInfoData.TargetId);
		List<ObjCharacter> candidateList = GetCandidateList(mSkillSender.AttributeData.Camp);
		SetEffect(skillDataById, effInfoDataById, target, candidateList, skillEffInfoData.AttackList, skillEffInfoData.IsServerAttack);
		if (!mIsUsingSkill || effInfoDataById.AreaType != 5)
		{
			return;
		}
		float delay = effInfoDataById.Param1Meter * 2f / (skillDataById.MoveDistanceMeter / skillDataById.MoveTimeSecond);
		vp_Timer.In(delay, delegate
		{
			if (mIsUsingSkill)
			{
				mSkillEffectList.Add(skillEffInfoData);
			}
		});
	}

	public void SetEffect(SkillData skillData, EffInfoData effInfoData, ObjCharacter target, List<ObjCharacter> targetList, List<attack_list> attackList, bool IsServerAttack)
	{
		if (effInfoData.AreaType == 4)
		{
			int param = effInfoData.Param3;
			int min = -effInfoData.Param2 / 2;
			int max = effInfoData.Param2 / 2;
			float max2 = (float)effInfoData.Param1 / 100f;
			for (int i = 0; i < param; i++)
			{
				int num = Random.Range(min, max);
				float num2 = Random.Range(0f, max2);
				Vector3 targetPos = mSkillSender.Position + Quaternion.AngleAxis(num, Vector3.up) * mSkillSender.CacheTransform.forward * num2;
				BombSustainedRangeObj bombSustainedRangeObj = Singleton<ObjManager>.Instance.GetBombSustainedRangeObj();
				if (bombSustainedRangeObj != null)
				{
					UnityVersionUtil.SetActiveRecursive(bombSustainedRangeObj.gameObject, state: true);
					bombSustainedRangeObj.Reset(mSkillSender.Position + Vector3.up * 2f, targetPos, mSkillSender, targetList, effInfoData, 1f);
				}
			}
			return;
		}
		List<CharacterEffInfoData> list = new List<CharacterEffInfoData>();
		if (IsServerAttack)
		{
			if (attackList != null)
			{
				ObjManager instance = Singleton<ObjManager>.Instance;
				for (int j = 0; j < attackList.Count; j++)
				{
					ObjCharacter objCharacter = instance.FindObjInScene(attackList[j].id);
					if (!(objCharacter != null))
					{
						continue;
					}
					if ((int)attackList[j].value >= 0)
					{
						BuffInfoData buffInfoData = null;
						if (!string.IsNullOrEmpty(effInfoData.BuffID))
						{
							buffInfoData = DataManager.GetBuffInfoDataByID(effInfoData.BuffID);
						}
						int num3 = 0;
						if (buffInfoData != null)
						{
							num3 = CharacterAttributeData.BuffUseState(mSkillSender, objCharacter.AttributeData, skillData.ID, effInfoData, Random.Range(0, 100), buffInfoData.BufType);
							list.Add(new CharacterEffInfoData(objCharacter, num3));
						}
						else
						{
							list.Add(new CharacterEffInfoData(objCharacter, GameDefine.BUF_USE_FAIL));
						}
					}
					else
					{
						list.Add(new CharacterEffInfoData(objCharacter, (int)attackList[j].value));
					}
				}
			}
		}
		else
		{
			BuffInfoData buffInfoData2 = null;
			if (!string.IsNullOrEmpty(effInfoData.BuffID))
			{
				buffInfoData2 = DataManager.GetBuffInfoDataByID(effInfoData.BuffID);
			}
			int num4 = 0;
			List<ObjCharacter> effectTargetList = GetEffectTargetList(mSkillSender, skillData, effInfoData, target, targetList);
			for (int k = 0; k < effectTargetList.Count; k++)
			{
				if (buffInfoData2 != null)
				{
					num4 = CharacterAttributeData.BuffUseState(mSkillSender, effectTargetList[k].AttributeData, skillData.ID, effInfoData, Random.Range(0, 100), buffInfoData2.BufType);
					list.Add(new CharacterEffInfoData(effectTargetList[k], num4));
				}
				else
				{
					list.Add(new CharacterEffInfoData(effectTargetList[k], GameDefine.BUF_USE_FAIL));
				}
			}
		}
		if (effInfoData.Target != 1)
		{
			DamageOperation(list, effInfoData, skillData);
		}
		BuffOperation(list, effInfoData);
		if (!string.IsNullOrEmpty(effInfoData.BuffID))
		{
			BuffInfoData buffInfoDataByID = DataManager.GetBuffInfoDataByID(effInfoData.BuffID);
			for (int l = 0; l < list.Count; l++)
			{
				SetSkillEffect(effInfoData, 0f, list[l].TargetObj, list[l].EffVal == GameDefine.BUF_USE_SUCCESS);
			}
		}
		else
		{
			for (int m = 0; m < list.Count; m++)
			{
				SetSkillEffect(effInfoData, 0f, list[m].TargetObj, isBufSuccess: false);
			}
		}
	}

	public List<ObjCharacter> GetEffectTargetList(ObjCharacter sender, SkillData skillData, EffInfoData effInfoData, ObjCharacter target, List<ObjCharacter> targetList)
	{
		List<ObjCharacter> list = new List<ObjCharacter>();
		if (effInfoData.Target == 1)
		{
			list.Add(sender);
		}
		else if (effInfoData.AreaType == 0)
		{
			if (target != null && !target.IsDie && !target.InvincibleFlag && VectorXZ.Distance(sender.Position, target.Position) < skillData.TraceDistanceMeter + sender.ModelRadius + target.ModelRadius + 0.5f)
			{
				list.Add(target);
			}
		}
		else if (effInfoData.AreaType == 1)
		{
			for (int i = 0; i < targetList.Count; i++)
			{
				if (CanAttack(targetList[i], sender))
				{
					if (target == null)
					{
						Debug.Log("target == null");
					}
					else if (AreaCheckTool.CheckInCircle(targetList[i].Position, target.Position, (float)effInfoData.Param1 / 100f + targetList[i].ModelRadius))
					{
						list.Add(targetList[i]);
					}
				}
			}
		}
		else if (effInfoData.AreaType == 2)
		{
			for (int j = 0; j < targetList.Count; j++)
			{
				if (CanAttack(targetList[j], sender) && AreaCheckTool.CheckInForwardRectangle(targetList[j].Position, sender.CacheTransform, (float)effInfoData.Param1 / 100f + targetList[j].ModelRadius, (float)effInfoData.Param2 / 100f + targetList[j].ModelRadius))
				{
					list.Add(targetList[j]);
				}
			}
		}
		else if (effInfoData.AreaType == 3)
		{
			for (int k = 0; k < targetList.Count; k++)
			{
				if (CanAttack(targetList[k], sender) && AreaCheckTool.CheckInSector(targetList[k].Position, sender.CacheTransform, (float)effInfoData.Param1 / 100f + sender.ModelRadius + targetList[k].ModelRadius, effInfoData.Param2))
				{
					list.Add(targetList[k]);
				}
			}
		}
		else if (effInfoData.AreaType == 5)
		{
			for (int l = 0; l < targetList.Count; l++)
			{
				if (CanAttack(targetList[l], sender) && (sender.ObjType != GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || targetList[l].ObjType != GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER || CampTool.ISPlayerCanAttack(sender as ObjOtherPlayer, targetList[l] as ObjOtherPlayer)) && AreaCheckTool.CheckInCircle(targetList[l].Position, SkillSender.Position, (float)effInfoData.Param1 / 100f))
				{
					list.Add(targetList[l]);
				}
			}
		}
		if (list.Count > skillData.MaxAttackCount)
		{
			int num = list.Count - skillData.MaxAttackCount;
			for (int m = 0; m < num; m++)
			{
				list.RemoveAt(Random.Range(0, list.Count));
			}
		}
		return list;
	}

	private bool CanAttack(ObjCharacter target, ObjCharacter sender)
	{
		if (target.ServerId == sender.ServerId)
		{
			return false;
		}
		if (target.IsDie || target.InvincibleFlag)
		{
			return false;
		}
		if (target.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC && (target as ObjNPC).IsMissionNpc())
		{
			return false;
		}
		if (!CheckSenderCanAttackOtherPlayer(sender, target))
		{
			return false;
		}
		return true;
	}

	private bool CheckSenderCanAttackOtherPlayer(ObjCharacter sender, ObjCharacter target)
	{
		if (sender.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER && target.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER)
		{
			return CampTool.ISPlayerCanAttack(sender as ObjOtherPlayer, target as ObjOtherPlayer);
		}
		return true;
	}

	private bool NeedSendBuffToServer(ObjCharacter target)
	{
		if (mSkillSender.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			if (target.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
			{
				if (!SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsSingleCopyScene())
				{
					return true;
				}
				return false;
			}
			if (target.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	private void BuffOperation(List<CharacterEffInfoData> targetList, EffInfoData effInfoData)
	{
		if (string.IsNullOrEmpty(effInfoData.BuffID))
		{
			return;
		}
		sendServerBuffList.Clear();
		for (int i = 0; i < targetList.Count; i++)
		{
			ObjCharacter targetObj = targetList[i].TargetObj;
			if (NeedSendBuffToServer(targetObj) && targetList[i].EffVal == GameDefine.BUF_USE_SUCCESS)
			{
				buff buff = new buff();
				buff.id = targetObj.ServerId;
				buff.effinfoId = effInfoData.ID;
				sendServerBuffList.Add(buff);
			}
		}
		if (sendServerBuffList.Count > 0)
		{
			sendServerBuffRequest.buffs = sendServerBuffList;
			NetLogic.GetInstance().Send<Protocol.use_skill_buff>(sendServerBuffRequest);
		}
	}

	private bool IsPlayerAttackOthers()
	{
		if (mSkillSender.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			return true;
		}
		return false;
	}

	private bool IsPlayerAttackLocalCharacter()
	{
		if ((SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsSingleCopyScene() || SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsRankPvPScene()) && mSkillSender.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			return true;
		}
		return false;
	}

	private bool IsPlayerAttackServerCharacter()
	{
		if (mSkillSender.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			return true;
		}
		return false;
	}

	private bool IsLocalChracterAttackPlayer(ObjCharacter target)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.IsSingleCopyScene() && mSkillSender.ObjType != GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER && (target.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || target.ObjType == GameDefine.OBJ_TYPE.OBJ_PLAYER_CAR))
		{
			return true;
		}
		return false;
	}

	private bool IsOnlineAIAttackPlayer()
	{
		if ((SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsBigWorld() || SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsRealPvPScene() || SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsMultiScene()) && mSkillSender.ObjType != GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER)
		{
			return true;
		}
		return false;
	}

	private void DamageOperation(List<CharacterEffInfoData> list, EffInfoData effInfoData, SkillData skillData)
	{
		if (IsOnlineAIAttackPlayer() || !effInfoData.IsHaveDamage())
		{
			return;
		}
		CharacterAttributeData attributeData = mSkillSender.AttributeData;
		int num = -1;
		if (IsPlayerAttackOthers())
		{
			num = 0;
			damagelist.Clear();
		}
		for (int i = 0; i < list.Count; i++)
		{
			ObjCharacter targetObj = list[i].TargetObj;
			CharacterAttributeData attributeData2 = targetObj.AttributeData;
			bool isHit = false;
			bool isCri = false;
			int damage = CharacterAttributeData.GetDamage(mSkillSender, targetObj, skillData.ID, effInfoData, out isHit, out isCri);
			if (IsPlayerAttackOthers())
			{
				if (damage > 0)
				{
					if (isCri)
					{
						targetObj.ChangeHPEffect(targetObj.AttributeData.HP - damage, GameDefine.DAMAGEBOARD_TYPE.PLAYER_ATTACK_CRITICAL);
					}
					else
					{
						targetObj.ChangeHPEffect(targetObj.AttributeData.HP - damage, GameDefine.DAMAGEBOARD_TYPE.TARGET_HPDOWN_PLAYER);
					}
				}
				else
				{
					targetObj.ChangeHPEffect(targetObj.AttributeData.HP - damage, GameDefine.DAMAGEBOARD_TYPE.PLAYER_ATTACK_MISS);
				}
			}
			else if (IsLocalChracterAttackPlayer(targetObj) && !targetObj.InvincibleFlag)
			{
				if (damage > 0)
				{
					if (isCri)
					{
						targetObj.ChangeHPEffect(targetObj.AttributeData.HP - damage, GameDefine.DAMAGEBOARD_TYPE.TARGET_ATTACK_CRITICAL);
					}
					else
					{
						targetObj.ChangeHPEffect(targetObj.AttributeData.HP - damage, GameDefine.DAMAGEBOARD_TYPE.PLAYER_HP_DOWN);
					}
				}
				else
				{
					targetObj.ChangeHPEffect(targetObj.AttributeData.HP - damage, GameDefine.DAMAGEBOARD_TYPE.TARGET_ATTACK_MISS);
				}
				request.clear();
				request.characterId = targetObj.ServerId;
				request.damage = damage;
				request.effinfoId = effInfoData.ID;
				NetLogic.GetInstance().Send<Protocol.local_character_attack>(request);
			}
			if (num > -1)
			{
				num++;
				acceptdamge acceptdamge = new acceptdamge();
				acceptdamge.damage = damage;
				acceptdamge.id = targetObj.ServerId;
				acceptdamge.skillId = skillData.ID;
				acceptdamge.effinfoId = effInfoData.ID;
				acceptdamge.cri = isCri;
				acceptdamge.parm = (isCri ? 1 : 0);
				acceptdamge.parm2 = (isHit ? 1 : 0);
				acceptdamge.parm3 = (int)(Time.realtimeSinceStartup * 100f);
				acceptdamge.parm4 = PlayerCommonData.sendIndex;
				damagelist.Add(acceptdamge);
			}
		}
		if (num > 0)
		{
			request1.clear();
			request1.damges = damagelist;
			NetLogic.GetInstance().Send<Protocol.accept_damge>(request1);
		}
	}

	public bool CheckEffectAdd(EffInfoData effInfoData, ObjCharacter objCha)
	{
		if (objCha == null || objCha.IsDie)
		{
			return false;
		}
		if (objCha.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC && (objCha as ObjNPC).MeshRoot == null)
		{
			return false;
		}
		if (objCha.ObjType == GameDefine.OBJ_TYPE.OBJ_PLAYER_CAR || objCha.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC_CAR)
		{
			return false;
		}
		if (objCha.AnimationLogic.CurActionData != null && objCha.AnimationLogic.CurActionData.AnimCanBeBreak == 1)
		{
			return true;
		}
		if (effInfoData.MoveDistance != 0 && effInfoData.ForceMove != 1)
		{
			return true;
		}
		return false;
	}

	public bool CheckSkillCanBeBreak(string skillId)
	{
		if (CheckSameSkill(skillId))
		{
			return false;
		}
		if (mUsingSkillData.CanBeBreak == 0)
		{
			return false;
		}
		return true;
	}

	public bool CheckSameSkill(string skillId)
	{
		if (skillId == mUsingSkillData.ID)
		{
			return true;
		}
		if (!string.IsNullOrEmpty(DataManager.GetSkillDataById(skillId).NextSkill) && !string.IsNullOrEmpty(mUsingSkillData.NextSkill))
		{
			return true;
		}
		return false;
	}

	public void BreakCurSkill()
	{
		if (!mIsUsingSkill)
		{
			return;
		}
		mIsUsingSkill = false;
		handle.Cancel();
		mSkillEffectList.Clear();
		if (mSkillSender.AnimationLogic != null)
		{
			mSkillSender.AnimationLogic.BreakCurAnima();
		}
		if (mSkillSender.EffectLogic != null)
		{
			mSkillSender.EffectLogic.BreakEffect(mCurActionData.FxEffID);
			if (!string.IsNullOrEmpty(mUsingSkillData.CastAction))
			{
				mSkillSender.EffectLogic.BreakYinChangEffect(mUsingSkillData.EffId_0);
			}
		}
		if (mSkillSender.SkillMotion != null)
		{
			mSkillSender.SkillMotion.BreakCurSkillMotion();
		}
		if (mSkillSender.EffectMotion != null)
		{
			mSkillSender.EffectMotion.BreakCurEffectMotion();
		}
		SkillFinished();
	}

	private void PlayAnimation(string actionName, AnimationLogic.OnAnimFinished onSkillAnimFinished = null, float duration = -1f)
	{
		if (SkillSender != null && SkillSender.AnimationLogic != null)
		{
			SkillSender.AnimationLogic.PlayAnimation(actionName, onSkillAnimFinished, duration);
		}
	}
}
