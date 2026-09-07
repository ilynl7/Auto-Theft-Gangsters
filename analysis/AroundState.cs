using UnityEngine;

public class AroundState : FSMState<AILogic>
{
	public override void Enter(AILogic owner, int previous)
	{
		owner.EnterAroundStateTime = Time.time;
		owner.AroundTime = owner.AiData.AroundTimeSecond;
		if (owner.Ownner.SelectedTarget != null)
		{
			if (!owner.ReturnBackFlag)
			{
				FindNextPoint(owner);
				return;
			}
			owner.Ownner.StopMove();
			owner.Ownner.WalkMoveTo(owner.Ownner.BornPos);
		}
		else
		{
			owner.Ownner.StopMove();
			owner.Ownner.WalkMoveTo(owner.Ownner.BornPos);
		}
	}

	private void FindNextPoint(AILogic owner)
	{
		if (!(owner.Ownner.SelectedTarget == null))
		{
			float aroundDistanceMeter = owner.AiData.AroundDistanceMeter;
			float y = owner.AiData.AroundAngle;
			Quaternion quaternion = Quaternion.Euler(0f, y, 0f);
			Vector3 vector = quaternion * owner.Ownner.SelectedTarget.CacheTransform.forward * aroundDistanceMeter + owner.Ownner.SelectedTarget.Position;
			if (!NavMesh.SamplePosition(vector, out var _, 0.1f, 15))
			{
				NavMeshHit hit2 = default(NavMeshHit);
				NavMesh.Raycast(owner.Ownner.SelectedTarget.Position, vector, out hit2, owner.Ownner.NavMeshAgent.walkableMask);
				vector = hit2.position;
			}
			owner.Ownner.MoveTo(vector);
		}
	}

	public override void Exit(AILogic owner, int previous)
	{
	}

	public override void Update(AILogic owner)
	{
		if (!owner.OutOffRangeFlag)
		{
			if (VectorXZ.Distance(owner.Ownner.Position, owner.Ownner.BornPos) > owner.AiData.ReturnDistanceMeter)
			{
				owner.OutOffRangeFlag = true;
				owner.StartChaseTime = Time.time;
			}
			else if (Time.time - owner.EnterAroundStateTime < owner.AroundTime)
			{
				if (!owner.Ownner.IsMoving)
				{
					FindNextPoint(owner);
				}
			}
			else
			{
				owner.ChangeState(1);
			}
		}
		else if (!owner.ReturnBackFlag)
		{
			if (Time.time - owner.StartChaseTime >= owner.ChaseTime)
			{
				if (!owner.Ownner.BeforeMoveCheck())
				{
					owner.Ownner.StopMove();
					owner.Ownner.WalkMoveTo(owner.Ownner.BornPos);
					owner.ReturnBackFlag = true;
				}
			}
			else
			{
				owner.ChangeState(1);
			}
		}
		else if (VectorXZ.Distance(owner.Ownner.Position, owner.Ownner.BornPos) < owner.Ownner.NavMeshAgent.stoppingDistance + 0.5f)
		{
			owner.ChangeState(0);
		}
	}

	public override void Notify(int messageID, params object[] messageParams)
	{
		if (messageID == 0)
		{
			AILogic aILogic = (AILogic)messageParams[0];
			aILogic.ReturnBackFlag = false;
			aILogic.StartChaseTime = Time.time;
		}
	}
}
