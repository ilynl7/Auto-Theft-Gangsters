public class FollowState : FSMState<AILogic>
{
	public override void Enter(AILogic owner, int previous)
	{
	}

	public override void Exit(AILogic owner, int previous)
	{
	}

	public override void Update(AILogic owner)
	{
		owner.Ownner.MoveTo(owner.Ownner.FollowTarget.position);
	}
}
