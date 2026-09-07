using UnityEngine;

public class AttackState : FSMState<AILogic>
{
	public override void Enter(AILogic owner, int previous)
	{
		owner.OutOffRangeFlag = false;
		owner.ReturnBackFlag = false;
		owner.CurUseSkillID = GetTargetSkill(owner.Ownner);
		owner.ActionTime = owner.AiData.ActionTimeSecond;
		owner.EnterAttackStateTime = Time.time;
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
		if (owner.Ownner.SkillLogic.IsUsingSkill)
		{
			return;
		}
		if (Time.time - owner.EnterAttackStateTime > owner.ActionTime)
		{
			owner.ChangeState(3);
		}
		else if (owner.AiData.ReturnDistance <= 0)
		{
			string targetSkill = GetTargetSkill(owner.Ownner);
			if (!string.IsNullOrEmpty(targetSkill))
			{
				SkillData skillDataById = DataManager.GetSkillDataById(targetSkill);
				ObjCharacter objCharacter = owner.Ownner.ChooseTarget(owner.AiData.SqrtLockDistanceMeter);
				if (!(objCharacter == null))
				{
					owner.Ownner.UseSkill(targetSkill);
				}
			}
		}
		else if (!owner.OutOffRangeFlag)
		{
			if (VectorXZ.Distance(owner.Ownner.Position, owner.Ownner.BornPos) > owner.AiData.ReturnDistanceMeter)
			{
				owner.OutOffRangeFlag = true;
				owner.StartChaseTime = Time.time;
			}
			else
			{
				UseSkill(owner);
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
				UseSkill(owner);
			}
		}
		else if (VectorXZ.Distance(owner.Ownner.Position, owner.Ownner.BornPos) < owner.Ownner.NavMeshAgent.stoppingDistance + 0.5f)
		{
			owner.ChangeState(0);
		}
	}

	private void UseSkill(AILogic owner)
	{
		if (owner.Ownner.SkillLogic.IsUsingSkill)
		{
			return;
		}
		if (string.IsNullOrEmpty(owner.CurUseSkillID))
		{
			owner.CurUseSkillID = GetTargetSkill(owner.Ownner);
			if (string.IsNullOrEmpty(owner.CurUseSkillID))
			{
				return;
			}
		}
		owner.Ownner.UseSkill(owner.CurUseSkillID);
		if (owner.Ownner.SelectedTarget == null)
		{
			owner.OutOffRangeFlag = true;
			if (!owner.Ownner.BeforeMoveCheck())
			{
				owner.Ownner.AILogic.ReturnBackFlag = true;
				owner.Ownner.StopMove();
				owner.Ownner.WalkMoveTo(owner.Ownner.BornPos);
			}
			else
			{
				owner.StartChaseTime = Time.time - owner.ChaseTime;
			}
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
		if (messageID == 0)
		{
			AILogic aILogic = (AILogic)messageParams[0];
			aILogic.ReturnBackFlag = false;
			aILogic.StartChaseTime = Time.time;
		}
		if (messageID == 1)
		{
			AILogic aILogic2 = (AILogic)messageParams[0];
			aILogic2.ReturnBackFlag = false;
			aILogic2.StartChaseTime = Time.time;
			aILogic2.ChangeState(3);
		}
	}
}
