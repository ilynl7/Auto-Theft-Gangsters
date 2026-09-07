using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SustainedRangeObj : MonoBehaviour
{
	public ParticleSystem DurationEffect;

	private List<ObjCharacter> mTargetList;

	private EffInfoData mEffInfoData;

	private float mDamageInterval;

	private float mDamageRange;

	private int mDamageVal;

	private float mDamageMulti;

	private float mDamageDuration;

	private ObjCharacter mSkillSender;

	private bool mEnableFlag;

	private float mLastDamageTime;

	private float mStartTime;

	private local_character_attack.request request = new local_character_attack.request();

	public void ResetSustainedRange(ObjCharacter skillSender, List<ObjCharacter> targetList, EffInfoData effInfoData, float damageInterval = 1f)
	{
		mTargetList = targetList;
		mEffInfoData = effInfoData;
		mDamageInterval = damageInterval;
		mDamageRange = effInfoData.Param4Meter;
		mDamageVal = effInfoData.Damagex;
		mDamageMulti = effInfoData.DamageMulti_100f;
		mDamageDuration = effInfoData.Param5Meter;
		mSkillSender = skillSender;
		mEnableFlag = false;
		UnityVersionUtil.SetActiveRecursive(DurationEffect.gameObject, state: false);
	}

	public void StartDamage()
	{
		if (!mEnableFlag)
		{
			mEnableFlag = true;
			mLastDamageTime = Time.time;
			mStartTime = Time.time;
			UnityVersionUtil.SetActiveRecursive(DurationEffect.gameObject, state: true);
		}
	}

	public void UpdateDamage()
	{
		if (!mEnableFlag)
		{
			return;
		}
		if (Time.time - mLastDamageTime >= mDamageInterval)
		{
			mLastDamageTime = Time.time;
			int num = 0;
			bool isHit = false;
			bool isCri = false;
			for (int i = 0; i < mTargetList.Count; i++)
			{
				if (!(Vector3.SqrMagnitude(mTargetList[i].Position - base.transform.position) <= mDamageRange * mDamageRange) || mTargetList[i].ObjType != GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || mTargetList[i].IsDie || mTargetList[i].InvincibleFlag)
				{
					continue;
				}
				num = CharacterAttributeData.GetDamage(mSkillSender, mTargetList[i], mEffInfoData, mDamageVal, mDamageMulti, out isHit, out isCri);
				request.clear();
				request.characterId = mTargetList[i].ServerId;
				request.damage = num;
				NetLogic.GetInstance().Send<Protocol.local_character_attack>(request);
				if (num > 0)
				{
					if (isCri)
					{
						mTargetList[i].ChangeHPEffect(mTargetList[i].AttributeData.HP - num, GameDefine.DAMAGEBOARD_TYPE.TARGET_ATTACK_CRITICAL);
					}
					else
					{
						mTargetList[i].ChangeHPEffect(mTargetList[i].AttributeData.HP - num, GameDefine.DAMAGEBOARD_TYPE.PLAYER_HP_DOWN);
					}
				}
				else
				{
					mTargetList[i].ChangeHPEffect(mTargetList[i].AttributeData.HP - num, GameDefine.DAMAGEBOARD_TYPE.TARGET_ATTACK_MISS);
				}
				SetSkillEffect(mEffInfoData, 0f, mTargetList[i]);
			}
		}
		if (Time.time - mStartTime >= mDamageDuration)
		{
			OnRecycle();
		}
	}

	public void SetSkillEffect(EffInfoData effInfoData, float delayTime, ObjCharacter target)
	{
		if (CheckEffectAdd(effInfoData, target))
		{
			if (effInfoData.HitAction != string.Empty)
			{
				target.AddPlayAnimationData(effInfoData.HitAction, delayTime, effInfoData.ID);
			}
			target.AddPlayEffInfoData(effInfoData.HitAction, effInfoData.ID, delayTime, base.transform.position);
		}
		if (effInfoData.ForceMove == 0 && effInfoData.MoveTime != 0)
		{
			target.AddPlayerEffMotionData(effInfoData.ID, delayTime, base.transform.position);
		}
		if (!string.IsNullOrEmpty(effInfoData.BuffID))
		{
			target.AddBuffInfoData(effInfoData.BuffID, delayTime, effInfoData.BuffDurationSecond, null);
		}
	}

	public bool CheckEffectAdd(EffInfoData effInfoData, ObjCharacter objCha)
	{
		if (objCha == null || objCha.IsDie)
		{
			return false;
		}
		if (objCha.AnimationLogic.CurActionData.AnimCanBeBreak == 1)
		{
			return true;
		}
		if (effInfoData.MoveDistance != 0 && effInfoData.ForceMove != 1)
		{
			return true;
		}
		return false;
	}

	public virtual void OnRecycle()
	{
		Debug.Log("BaseRecycle");
	}
}
