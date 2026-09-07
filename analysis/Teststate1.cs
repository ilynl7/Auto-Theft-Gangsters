using UnityEngine;

public class Teststate1 : FSMState<TestMachine>
{
	public override void Enter(TestMachine owner, int previous)
	{
		Debug.Log("Teststate1 Enter");
	}

	public override void Update(TestMachine owner)
	{
		Debug.Log("Teststate1 Update");
	}

	public override void Exit(TestMachine owner, int next)
	{
		Debug.Log("Teststate1 Exit");
	}

	public override void Notify(int messageID, params object[] messageParams)
	{
	}
}
