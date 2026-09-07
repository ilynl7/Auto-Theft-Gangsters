using UnityEngine;

public class TestEnableBreak : MonoBehaviour
{
	private void OnEnable()
	{
		Debug.Log(base.gameObject.name + " :: Enable!!!!!!!!!!!!!!!!!!!");
	}

	private void OnDisable()
	{
		Debug.Log(base.gameObject.name + " :: Disable!!!!!!!!!!!!!!!!!!!");
	}
}
