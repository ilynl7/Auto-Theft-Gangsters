public class StateMachine<T>
{
	private T mObject;

	public int mCurrentId = -1;

	private FSMState<T>[] mStateType;

	private FSMState<T> mCurrentState;

	public int mPreId = -1;

	private FSMState<T> mPreState;

	public T BaseObject => mObject;

	public int CurrentStateId => mCurrentId;

	public FSMState<T> CurrentState => mCurrentState;

	public int PreStateId => mPreId;

	public FSMState<T> PreState => mPreState;

	public StateMachine()
	{
	}

	public StateMachine(T t, int numState)
	{
		mObject = t;
		mStateType = new FSMState<T>[numState];
	}

	public void Notify(int messageID, params object[] messageParams)
	{
		mCurrentState.Notify(messageID, messageParams);
	}

	public void AddState(FSMState<T> State, int Id)
	{
		mStateType[Id] = State;
		mStateType[Id].StateMachine = this;
	}

	public void SetState(int newId, params object[] list)
	{
		int previous = mCurrentId;
		SetInState(newId);
		mCurrentState.Enter(mObject, previous, list);
	}

	public void SetState(int newId)
	{
		int previous = mCurrentId;
		SetInState(newId);
		mCurrentState.Enter(mObject, previous);
	}

	private void SetInState(int newId)
	{
		if (mCurrentState != null)
		{
			mPreState = mCurrentState;
			mCurrentState.Exit(mObject, newId);
		}
		if (newId >= 0)
		{
			mCurrentState = mStateType[newId];
		}
		else
		{
			mCurrentState = null;
		}
		mPreId = mCurrentId;
		mCurrentId = newId;
	}

	public void RevertToPreviousState()
	{
		SetState(mPreId);
	}

	public void Stop()
	{
		SetInState(-1);
	}

	public void Update()
	{
		mCurrentState.Update(mObject);
	}
}
