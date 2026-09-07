using System.Collections.Generic;
using UnityEngine;

public class BombSustainedRangeObj : SustainedRangeObj
{
	public GameObject MeshObj;

	private Vector3 mStartPos;

	private float mDuartion;

	private float mUseTime;

	private Vector3 mTargetPos;

	private float percent;

	public void Reset(Vector3 startPos, Vector3 targetPos, ObjCharacter skillSender, List<ObjCharacter> targetList, EffInfoData effInfoData, float damageInterval = 1f)
	{
		mTargetPos = targetPos;
		mDuartion = 1f;
		mUseTime = 0f;
		mStartPos = startPos;
		base.transform.position = startPos;
		ResetSustainedRange(skillSender, targetList, effInfoData, damageInterval);
	}

	private void Update()
	{
		mUseTime += Time.deltaTime;
		percent = mUseTime / mDuartion;
		if (percent <= 1f)
		{
			float num = 3f - 12f * (percent - 0.5f) * (percent - 0.5f) + 0.1f;
			base.transform.position = Vector3.Lerp(mStartPos, mTargetPos, percent) + Vector3.up * num;
			return;
		}
		if (UnityVersionUtil.IsActive(MeshObj.gameObject))
		{
			MeshObj.active = false;
			base.transform.position = mTargetPos + Vector3.up * 0.1f;
		}
		StartDamage();
		UpdateDamage();
	}

	public override void OnRecycle()
	{
		UnityVersionUtil.SetActiveRecursive(base.gameObject, state: false);
		Singleton<ObjManager>.Instance.RecycleBombSustainedRangeObj(this);
	}
}
