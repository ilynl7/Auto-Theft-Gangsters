using UnityEngine;

public class NonMonoBehaviour
{
	public void Test()
	{
		TestComponent testComponent = (TestComponent)GameObject.Find("ExternalGameObject").GetComponent("TestComponent");
		testComponent.Test("Hello World!");
	}
}
