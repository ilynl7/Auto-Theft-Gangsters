using UnityEngine;

public class TestTime : MonoBehaviour
{
	private void Start()
	{
		Test();
	}

	private void Test()
	{
		Debug.Log("one1=" + Time.time);
		vp_Timer.In(2f, delegate
		{
			Debug.Log("one=" + Time.time);
		});
		vp_Timer.In(1f, tesmore, new object[2] { "zzzz", 0.5f });
		vp_Timer.Handle timerHandle = new vp_Timer.Handle();
		vp_Timer.In(3f, delegate
		{
			Debug.Log("cancel");
		}, timerHandle);
	}

	private void tesmore(object o)
	{
		object[] array = (object[])o;
		Debug.Log("more=" + array[1]);
	}

	private void Update()
	{
	}
}
