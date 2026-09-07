public class FSMState<T> : Singleton<FSMState<T>>
{
	public StateMachine<T> StateMachine { get; set; }

	public T BaseObject { get; set; }

	public virtual void Enter(T owner, int previous)
	{
	}

	public virtual void Enter(T owner, int previous, params object[] list)
	{
	}

	public virtual void Exit(T owner, int next)
	{
	}

	public virtual void Update(T owner)
	{
	}

	public virtual void Notify(int messageID, params object[] messageParams)
	{
	}

	public void SetState(int newId, params object[] list)
	{
		StateMachine.SetState(newId, list);
	}

	public void SetState(int newId)
	{
		StateMachine.SetState(newId);
	}
}
