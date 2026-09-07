using UnityEngine;

[SerializeField]
public class BaseAIState : Singleton<BaseAIState>
{
	public virtual void Enter(ObjNPC owner)
	{
	}

	public virtual void Exit(ObjNPC owner)
	{
	}

	public virtual void UpdateAI(ObjNPC owner)
	{
	}

	public virtual void CheckState(ObjNPC owner)
	{
	}
}
