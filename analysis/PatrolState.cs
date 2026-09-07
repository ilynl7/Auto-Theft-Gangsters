using UnityEngine;

public class PatrolState : FSMState<AILogic>
{
	private Vector3 mTarget;

	private ObjMainPlayer mMainPlayer;

	public ObjMainPlayer MainPlayer
	{
		get
		{
			if (mMainPlayer == null)
			{
				mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			}
			return mMainPlayer;
		}
	}

	public override void Enter(AILogic owner, int previous)
	{
		FindNextTarget(owner);
	}

	public override void Exit(AILogic owner, int previous)
	{
	}

	public override void Update(AILogic owner)
	{
		if (!owner.Ownner.IsDie)
		{
			if (owner.DelayMoveTimeCount > 0f && Time.time - owner.DelayMoveTimeCount > 2f)
			{
				owner.DelayMoveTimeCount = -1f;
				FindNextTarget(owner);
			}
			CheckState(owner);
		}
	}

	public void FindNextTarget(AILogic owner)
	{
		if (owner.IsHavePath())
		{
			mTarget = owner.PathList[owner.CurPathIndex].Position;
			owner.CurPathIndex = (owner.CurPathIndex + 1) % owner.PathList.Count;
			owner.Ownner.WalkMoveTo(mTarget, 1f, owner.OnPatrolStateArriveTarget);
		}
		else if (owner.AiData.PATROL_Type == PATROL_TYPE.CIRCLE)
		{
			float x = Random.Range(owner.AiData.PatrolDistanceMeter * -1f, owner.AiData.PatrolDistanceMeter);
			float z = Random.Range(owner.AiData.PatrolDistanceMeter * -1f, owner.AiData.PatrolDistanceMeter);
			Vector3 vector = owner.Ownner.BornPos + new Vector3(x, 0f, z);
			if (NavMesh.SamplePosition(vector, out var _, 0.1f, 15))
			{
				mTarget = vector;
			}
			else
			{
				NavMeshHit hit2 = default(NavMeshHit);
				NavMesh.Raycast(owner.Ownner.BornPos, vector, out hit2, owner.Ownner.NavMeshAgent.walkableMask);
				vector = hit2.position;
				mTarget = vector;
			}
			owner.Ownner.WalkMoveTo(mTarget, 1f, owner.OnPatrolStateArriveTarget);
		}
		else if (owner.AiData.PATROL_Type != 0)
		{
		}
	}

	public void CheckState(AILogic owner)
	{
		if (owner.AiData.LockType == 1 && MainPlayer != null && !MainPlayer.IsDie && AreaCheckTool.CheckInCircle(MainPlayer.Position, owner.Ownner.Position, owner.AiData.LockDistanceMeter))
		{
			owner.ChangeState((int)owner.AttackState);
		}
	}

	public override void Notify(int messageID, params object[] messageParams)
	{
		if (messageID == 0)
		{
			AILogic aILogic = (AILogic)messageParams[0];
			aILogic.ChangeState((int)aILogic.AttackState);
		}
	}
}
