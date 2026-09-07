public class BossRandomSkillState : FSMState<AILogic>
{
	public override void Enter(AILogic owner, int previous)
	{
	}

	public override void Exit(AILogic owner, int previous)
	{
	}

	public override void Update(AILogic owner)
	{
		if (!owner.Ownner.IsDie)
		{
			CheckUseSkill(owner);
		}
	}

	public void CheckUseSkill(AILogic owner)
	{
		UseSkill(owner.Ownner);
	}

	private void UseSkill(ObjNPC owner)
	{
		if (owner.CharacterSkillData.Count == 1)
		{
			owner.UseSkill(owner.CharacterSkillData[0].ID);
			return;
		}
		string targetSkill = GetTargetSkill(owner);
		if (!string.IsNullOrEmpty(targetSkill))
		{
			owner.UseSkill(targetSkill);
		}
	}

	private string GetTargetSkill(ObjNPC owner)
	{
		if (owner.EnableSkillIDList.Count == 0)
		{
			return string.Empty;
		}
		return owner.EnableSkillIDList[0].ID;
	}

	public override void Notify(int messageID, params object[] messageParams)
	{
	}
}
