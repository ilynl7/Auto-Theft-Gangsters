using UnityEngine;

public class DecisionState : FSMState<AILogic>
{
	public override void Enter(AILogic owner, int previous)
	{
		if (previous == (int)owner.AttackState)
		{
			if (IsNeedAround(owner))
			{
				EnterAroundState(owner);
			}
			else
			{
				EnterAttackState(owner);
			}
		}
		else
		{
			EnterAttackState(owner);
		}
	}

	private void EnterAroundState(AILogic owner)
	{
		owner.ChangeState(2);
	}

	private void EnterAttackState(AILogic owner)
	{
		owner.ChangeState(1);
	}

	private bool IsNeedAround(AILogic owner)
	{
		if (Random.Range(0, 100) <= owner.AiData.AroundProb)
		{
			return true;
		}
		return false;
	}

	public override void Exit(AILogic owner, int previous)
	{
	}

	public override void Update(AILogic owner)
	{
	}
}
