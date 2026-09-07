using UnityEngine;

public class TestEvent : MonoBehaviour
{
	private void Start()
	{
		SingletonUnity<MyEvent>.Instance.Register("Test", this, "MyTest");
		SingletonUnity<MyEvent>.Instance.Register("Test", this, "zz");
		SingletonUnity<MyEvent>.Instance.Fire("Test", "first!!!!");
		SingletonUnity<MyEvent>.Instance.DelayFire("Test", 2f, "hahah");
	}

	private void Update()
	{
	}

	public void MyTest(string str)
	{
		Log.DEBUG_MSG("MyTest1=" + str);
		Log.DEBUG_MSG(Time.time);
	}

	public void zz(string str)
	{
		Log.DEBUG_MSG("zz=" + str);
	}
}
