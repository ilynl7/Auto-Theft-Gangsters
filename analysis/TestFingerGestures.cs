using UnityEngine;

public class TestFingerGestures : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTap(TapGesture gesture)
	{
		Debug.Log(string.Concat("Tap gesture detected at ", gesture.Position, ". It was sent by ", gesture.Recognizer.name));
		if ((bool)gesture.Selection)
		{
			Debug.Log("Tapped object: " + gesture.Selection.name);
		}
		else
		{
			Debug.Log("No object was tapped at " + gesture.Position);
		}
	}
}
