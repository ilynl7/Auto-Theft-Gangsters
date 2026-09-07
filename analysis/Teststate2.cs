using UnityEngine;

public class Teststate2 : FSMState<TestMachine>
{
	public override void Enter(TestMachine owner, int previous)
	{
		Debug.Log("Teststate2 Enter");
	}

	public override void Update(TestMachine owner)
	{
		Debug.Log("Teststate2 Update");
	}

	public override void Exit(TestMachine owner, int next)
	{
		Debug.Log("Teststate2 Exit");
	}

	public override void Notify(int messageID, params object[] messageParams)
	{
	}
}
