using UnityEngine;

public class TestMachine : MonoBehaviour
{
	private StateMachine<TestMachine> mStateMachine;

	private void InitStateMachine()
	{
		mStateMachine = new StateMachine<TestMachine>(this, 2);
		mStateMachine.AddState(Singleton<Teststate1>.Instance, 0);
		mStateMachine.AddState(Singleton<Teststate2>.Instance, 1);
		mStateMachine.SetState(0);
		mStateMachine.SetState(1);
	}

	private void Start()
	{
		InitStateMachine();
	}

	private void Update()
	{
		mStateMachine.Update();
	}
}
