using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffLogic : MonoBehaviour
{
	public delegate void BuffDelegate(BuffInfoData bufInfoData);

	public delegate void BuffSenderDelegate(BuffInfoData bufInfoData, ObjCharacter sender);

	public BuffDelegate onStun;

	public BuffDelegate onStunDone;

	public BuffSenderDelegate onKnockDown;

	public BuffDelegate onKnockDownDone;

	public BuffDelegate onSleep;

	public BuffDelegate onSleepDone;

	public BuffDelegate onChangeAttr;

	public BuffDelegate onChangeAttrDone;

	private ObjCharacter owner;

	private List<PlayBuffInfoData> mBuffInfoDataList = new List<PlayBuffInfoData>();

	public List<PlayBuffInfoData> CurBuffInfoDataList = new List<PlayBuffInfoData>();

	public void AddBuffInfoData(string buffID, float delayTime, float duration, ObjCharacter sender)
	{
		if (!owner.IsDie)
		{
			mBuffInfoDataList.Add(new PlayBuffInfoData(buffID, delayTime, duration, sender));
		}
	}

	private void Start()
	{
	}

	public void Init(ObjCharacter ownerObj)
	{
		owner = ownerObj;
	}

	public void ClearBuff()
	{
		if (CurBuffInfoDataList.Count > 0)
		{
			for (int num = CurBuffInfoDataList.Count - 1; num >= 0; num--)
			{
				RemoveCurBuffList(CurBuffInfoDataList[num]);
			}
		}
		CurBuffInfoDataList.Clear();
		mBuffInfoDataList.Clear();
	}

	public void RegisterOnStun(BuffDelegate func)
	{
		onStun = (BuffDelegate)Delegate.Combine(onStun, func);
	}

	public void DeRegisterOnStun(BuffDelegate func)
	{
		if (onStun != null)
		{
			onStun = (BuffDelegate)Delegate.Remove(onStun, func);
		}
	}

	public void RegisterOnStunDone(BuffDelegate func)
	{
		onStunDone = (BuffDelegate)Delegate.Combine(onStunDone, func);
	}

	public void DeRegisterOnStunDone(BuffDelegate func)
	{
		if (onStunDone != null)
		{
			onStunDone = (BuffDelegate)Delegate.Remove(onStunDone, func);
		}
	}

	public void RegisterOnKnockDown(BuffSenderDelegate func)
	{
		onKnockDown = (BuffSenderDelegate)Delegate.Combine(onKnockDown, func);
	}

	public void DeRegisterOnKnockDown(BuffSenderDelegate func)
	{
		if (onKnockDown != null)
		{
			onKnockDown = (BuffSenderDelegate)Delegate.Remove(onKnockDown, func);
		}
	}

	public void RegisterOnKnockDownDone(BuffDelegate func)
	{
		onKnockDownDone = (BuffDelegate)Delegate.Combine(onKnockDownDone, func);
	}

	public void DeRegisterOnKnockDownDone(BuffDelegate func)
	{
		if (onKnockDownDone != null)
		{
			onKnockDownDone = (BuffDelegate)Delegate.Remove(onKnockDownDone, func);
		}
	}

	public void RegisterOnSleep(BuffDelegate func)
	{
		onSleep = (BuffDelegate)Delegate.Combine(onSleep, func);
	}

	public void DeRegisterOnSleep(BuffDelegate func)
	{
		if (onSleep != null)
		{
			onSleep = (BuffDelegate)Delegate.Remove(onSleep, func);
		}
	}

	public void RegisterOnSleepDone(BuffDelegate func)
	{
		onSleepDone = (BuffDelegate)Delegate.Combine(onSleepDone, func);
	}

	public void DeRegisterOnSleepDone(BuffDelegate func)
	{
		if (onSleepDone != null)
		{
			onSleepDone = (BuffDelegate)Delegate.Remove(onSleepDone, func);
		}
	}

	public void RegisterOnChangeAttr(BuffDelegate func)
	{
		onChangeAttr = (BuffDelegate)Delegate.Combine(onChangeAttr, func);
	}

	public void DeRegisterOnChangeAttr(BuffDelegate func)
	{
		if (onChangeAttr != null)
		{
			onChangeAttr = (BuffDelegate)Delegate.Remove(onChangeAttr, func);
		}
	}

	public void RegisterOnChangeAttrDone(BuffDelegate func)
	{
		onChangeAttrDone = (BuffDelegate)Delegate.Combine(onChangeAttrDone, func);
	}

	public void DeRegisterOnChangeAttrDone(BuffDelegate func)
	{
		if (onChangeAttrDone != null)
		{
			onChangeAttrDone = (BuffDelegate)Delegate.Remove(onChangeAttrDone, func);
		}
	}

	private void Update()
	{
	}

	public void UpdateBuffLogic()
	{
		if (mBuffInfoDataList.Count > 0)
		{
			for (int num = mBuffInfoDataList.Count - 1; num >= 0; num--)
			{
				mBuffInfoDataList[num].DelayTime -= Time.deltaTime;
				if (mBuffInfoDataList[num].DelayTime <= 0f)
				{
					ResetBuff(mBuffInfoDataList[num]);
					mBuffInfoDataList.RemoveAt(num);
				}
			}
		}
		if (CurBuffInfoDataList.Count <= 0)
		{
			return;
		}
		for (int num2 = CurBuffInfoDataList.Count - 1; num2 >= 0; num2--)
		{
			if (CurBuffInfoDataList[num2].Duration != -1f)
			{
				CurBuffInfoDataList[num2].Duration -= Time.deltaTime;
				if (CurBuffInfoDataList[num2].Duration <= 0f)
				{
					RemoveBuff(CurBuffInfoDataList[num2]);
				}
			}
		}
	}

	private PlayBuffInfoData GetCurTypeBuff(BUFF_TYPE type)
	{
		for (int i = 0; i < CurBuffInfoDataList.Count; i++)
		{
			if (CurBuffInfoDataList[i].BuffInfoData.BufType == type)
			{
				return CurBuffInfoDataList[i];
			}
		}
		return null;
	}

	private PlayBuffInfoData GetCurBuffById(string id)
	{
		for (int i = 0; i < CurBuffInfoDataList.Count; i++)
		{
			if (CurBuffInfoDataList[i].BuffInfoData.ID.Equals(id))
			{
				return CurBuffInfoDataList[i];
			}
		}
		return null;
	}

	private void ResetBuff(PlayBuffInfoData buff)
	{
		if (buff.BuffInfoData.BufType == BUFF_TYPE.STUN)
		{
			if (GetCurTypeBuff(BUFF_TYPE.KNOCK_DOWN) != null)
			{
				return;
			}
			PlayBuffInfoData curTypeBuff = GetCurTypeBuff(BUFF_TYPE.STUN);
			if (curTypeBuff != null)
			{
				curTypeBuff.Duration = Mathf.Max(buff.Duration, curTypeBuff.Duration);
				return;
			}
			CurBuffInfoDataList.Add(buff);
		}
		else if (buff.BuffInfoData.BufType == BUFF_TYPE.KNOCK_DOWN)
		{
			PlayBuffInfoData curTypeBuff2 = GetCurTypeBuff(BUFF_TYPE.STUN);
			if (curTypeBuff2 != null)
			{
				RemoveBuff(curTypeBuff2);
			}
			if (GetCurTypeBuff(BUFF_TYPE.KNOCK_DOWN) != null)
			{
				return;
			}
			CurBuffInfoDataList.Add(buff);
		}
		else if (buff.BuffInfoData.BufType == BUFF_TYPE.CHANGE_ATTR)
		{
			PlayBuffInfoData curBuffById = GetCurBuffById(buff.BuffInfoData.ID);
			if (curBuffById != null)
			{
				curBuffById.Duration = buff.Duration;
				return;
			}
			CurBuffInfoDataList.Add(buff);
		}
		else
		{
			CurBuffInfoDataList.Add(buff);
		}
		switch (buff.BuffInfoData.BufType)
		{
		case BUFF_TYPE.CHANGE_ATTR:
			ResetAttrBuff(buff.BuffInfoData);
			break;
		case BUFF_TYPE.STUN:
			ResetStunBuff(buff.BuffInfoData);
			break;
		case BUFF_TYPE.SLEEP:
			ResetSleepBuff(buff.BuffInfoData);
			break;
		case BUFF_TYPE.KNOCK_DOWN:
			ResetKnockDownBuff(buff.BuffInfoData, buff.Sender);
			break;
		case BUFF_TYPE.INVINCIBLE:
			ResetInvincibleBuff();
			break;
		default:
			MonoBehaviour.print("buff Type Error :: " + buff.BuffInfoData.BufType);
			break;
		}
		if (!string.IsNullOrEmpty(buff.BuffInfoData.Effect))
		{
			owner.AddPlayeBufEffInfoData(buff.BuffInfoData.Effect, 0f, float.MaxValue, owner.Position);
			buff.FxEffectId = buff.BuffInfoData.Effect;
		}
	}

	private void ResetAttrBuff(BuffInfoData buffInfoData)
	{
		if (onChangeAttr != null)
		{
			onChangeAttr(buffInfoData);
		}
	}

	private void ResetStunBuff(BuffInfoData buffInfoData)
	{
		if (onStun != null)
		{
			onStun(buffInfoData);
		}
	}

	private void ResetKnockDownBuff(BuffInfoData buffInfoData, ObjCharacter sender)
	{
		if (onKnockDown != null)
		{
			onKnockDown(buffInfoData, sender);
		}
	}

	private void ResetSleepBuff(BuffInfoData buffInfoData)
	{
		if (onSleep != null)
		{
			onSleep(buffInfoData);
		}
	}

	private void ResetInvincibleBuff()
	{
		owner.InvincibleFlag = true;
	}

	private void RemoveBuff(PlayBuffInfoData buff)
	{
		switch (buff.BuffInfoData.BufType)
		{
		case BUFF_TYPE.CHANGE_ATTR:
			RemoveAttrBuff(buff.BuffInfoData);
			break;
		case BUFF_TYPE.STUN:
			RemoveStunBuff(buff.BuffInfoData);
			break;
		case BUFF_TYPE.SLEEP:
			RemoveSleepBuff(buff.BuffInfoData);
			break;
		case BUFF_TYPE.KNOCK_DOWN:
			RemoveKnockDown(buff.BuffInfoData);
			break;
		case BUFF_TYPE.INVINCIBLE:
			RemoveInvincibleBuff();
			break;
		default:
			MonoBehaviour.print("buff Type Error");
			break;
		}
		RemoveCurBuffList(buff);
	}

	private void RemoveCurBuffList(PlayBuffInfoData buff)
	{
		if (CurBuffInfoDataList.Contains(buff))
		{
			owner.EffectLogic.BreakEffect(buff.FxEffectId);
			CurBuffInfoDataList.Remove(buff);
		}
	}

	private void RemoveAttrBuff(BuffInfoData buffInfoData)
	{
		if (onChangeAttrDone != null)
		{
			onChangeAttrDone(buffInfoData);
		}
	}

	private void RemoveStunBuff(BuffInfoData buffInfoData)
	{
		if (onStunDone != null)
		{
			onStunDone(buffInfoData);
		}
	}

	private void RemoveKnockDown(BuffInfoData buffInfoData)
	{
		if (onKnockDownDone != null)
		{
			onKnockDownDone(buffInfoData);
		}
	}

	private void RemoveSleepBuff(BuffInfoData buffInfoData)
	{
		if (onSleepDone != null)
		{
			onSleepDone(buffInfoData);
		}
	}

	private void RemoveInvincibleBuff()
	{
		owner.InvincibleFlag = false;
	}

	public void BreakBuff(BuffInfoData buffInfo)
	{
		for (int i = 0; i < CurBuffInfoDataList.Count; i++)
		{
			if (CurBuffInfoDataList[i].BuffInfoData.ID == buffInfo.ID)
			{
				RemoveBuff(CurBuffInfoDataList[i]);
			}
		}
	}
}
