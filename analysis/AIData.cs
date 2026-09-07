using UnityEngine;

public class AIData
{
	public string ID;

	public int LockType;

	public int LockDistance;

	public int AroundProb;

	public int AroundTimeMin;

	public int AroundTimeMax;

	public int AroundMaxAngel;

	public int AroundMinAngel;

	public int AroundMinDistance;

	public int AroundMaxDistance;

	public int ActionMinTime;

	public int ActionMaxTime;

	public int SkillCDMin;

	public int SkillCDMax;

	public int ReturnDistance;

	public int ChangeTargetCD;

	public int PatrolType;

	public int PatrolDistance;

	public int WaitTime;

	public float LockDistanceMeter => (float)LockDistance / 100f;

	public float SqrtLockDistanceMeter => (float)(LockDistance * LockDistance) / 10000f;

	public float AroundTimeSecond => (float)Random.Range(AroundTimeMin, AroundTimeMax) / 1000f;

	public int AroundAngle => Random.Range(AroundMinAngel, AroundMaxAngel);

	public float AroundDistanceMeter => (float)Random.Range(AroundMinDistance, AroundMaxDistance) / 100f;

	public float ActionTimeSecond => (float)Random.Range(ActionMinTime, ActionMaxTime) / 1000f;

	public float ReturnDistanceMeter => (float)ReturnDistance / 100f;

	public float PatrolDistanceMeter => (float)PatrolDistance / 100f;

	public PATROL_TYPE PATROL_Type => (PATROL_TYPE)PatrolType;
}
