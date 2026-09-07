using UnityEngine;

public class SchedulingDemo : MonoBehaviour
{
	private Vector3 m_CubeScale = new Vector3(0.5f, 0.5f, 0.5f);

	private Color m_StatusColor = Color.white;

	private string m_StatusString = string.Empty;

	public AudioClip m_SmackSound;

	private void Update()
	{
		base.transform.position = (Object.FindObjectOfType(typeof(Camera)) as Camera).ScreenToWorldPoint(new Vector3(Screen.width - 165, Screen.height - 105, 6f));
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, m_CubeScale, Time.deltaTime * 5f);
		base.renderer.enabled = base.transform.localScale.x > 0.01f;
		base.renderer.material.color = Color.Lerp(base.renderer.material.color, Color.red, Time.deltaTime * 3f);
	}

	private void OnGUI()
	{
		m_StatusColor = Color.Lerp(m_StatusColor, new Color(1f, 1f, 0f, 0f), Time.deltaTime * 0.5f);
		GUI.color = Color.white;
		GUILayout.Space(50f);
		GUILayout.BeginHorizontal();
		GUILayout.Space(50f);
		GUILayout.Label("SCHEDULING EXAMPLE\n    - Each of these examples will schedule some functionality in one (1)\n      second using different options.\n    - Please study the source code in 'Examples/Scheduling/Scheduling.cs' ...");
		GUILayout.EndHorizontal();
		GUILayout.BeginArea(new Rect(100f, 150f, 400f, 600f));
		GUI.color = Color.white;
		GUILayout.Label("Methods, Arguments, Delegates, Iterations, Intervals & Canceling");
		if (DoButton("A simple method"))
		{
			vp_Timer.In(1f, DoMethod);
		}
		if (DoButton("A method with a single argument"))
		{
			vp_Timer.In(1f, DoMethodWithSingleArgument, 242);
		}
		if (DoButton("A method with multiple arguments"))
		{
			vp_Timer.In(arguments: new object[3] { "December", 31, 2012 }, delay: 1f, callback: DoMethodWithMultipleArguments);
		}
		if (DoButton("A delegate"))
		{
			vp_Timer.In(1f, delegate
			{
				SmackCube();
			});
		}
		if (DoButton("A delegate with a single argument"))
		{
			vp_Timer.In(1f, delegate(object o)
			{
				int num3 = (int)o;
				SmackCube();
				SetStatus("A delegate with a single argument ... \"" + num3 + "\"");
			}, 242);
		}
		if (DoButton("A delegate with multiple arguments"))
		{
			vp_Timer.In(1f, delegate(object o)
			{
				object[] array = (object[])o;
				string text = (string)array[0];
				int num = (int)array[1];
				int num2 = (int)array[2];
				SmackCube();
				SetStatus("A delegate with multiple arguments ... \"" + text + " " + num + ", " + num2 + "\"");
			}, new object[3] { "December", 31, 2012 });
		}
		if (DoButton("5 iterations of a method"))
		{
			vp_Timer.In(1f, SmackCube, 5);
		}
		if (DoButton("5 iterations of a method, with 0.2 sec intervals"))
		{
			vp_Timer.In(1f, SmackCube, 5, 0.2f);
		}
		if (DoButton("5 iterations of a delegate, canceled after 3 seconds"))
		{
			vp_Timer.Handle timer = new vp_Timer.Handle();
			vp_Timer.In(0f, delegate
			{
				SmackCube();
			}, 5, 1f, timer);
			vp_Timer.In(3f, delegate
			{
				timer.Cancel();
			});
		}
		GUILayout.Label("\nMethod & object accessibility:");
		if (DoButton("Running a method from a non-monobehaviour class", showCube: false))
		{
			vp_Timer.In(1f, delegate
			{
				NonMonoBehaviour nonMonoBehaviour = new NonMonoBehaviour();
				nonMonoBehaviour.Test();
			});
		}
		if (DoButton("Running a method from a specific external gameobject", showCube: false))
		{
			vp_Timer.In(1f, delegate
			{
				TestComponent testComponent2 = (TestComponent)GameObject.Find("ExternalGameObject").GetComponent("TestComponent");
				testComponent2.Test("Hello World!");
			});
		}
		if (DoButton("Running a method from the first component of a certain type\nin current transform or any of its children", showCube: false))
		{
			vp_Timer.In(1f, delegate
			{
				TestComponent componentInChildren = base.transform.root.GetComponentInChildren<TestComponent>();
				componentInChildren.Test("Hello World!");
			});
		}
		if (DoButton("Running a method from the first component of a certain type\nin the whole Hierarchy", showCube: false))
		{
			vp_Timer.In(1f, delegate
			{
				TestComponent testComponent = (TestComponent)Object.FindObjectOfType(typeof(TestComponent));
				testComponent.Test("Hello World!");
			});
		}
		GUILayout.EndArea();
		GUI.color = m_StatusColor;
		GUILayout.BeginArea(new Rect(Screen.width - 255, 205f, 240f, 600f));
		GUILayout.Label(m_StatusString);
		GUILayout.EndArea();
	}

	private void DoMethod()
	{
		SmackCube();
	}

	private void DoMethodWithSingleArgument(object o)
	{
		int num = (int)o;
		SmackCube();
		SetStatus("A method with a single argument ... \"" + num + "\"");
	}

	private void DoMethodWithMultipleArguments(object o)
	{
		object[] array = (object[])o;
		string text = (string)array[0];
		int num = (int)array[1];
		int num2 = (int)array[2];
		SetStatus("A method with multiple arguments ... \"" + text + ", " + num + ", " + num2 + "\"");
		SmackCube();
	}

	public void SmackCube()
	{
		base.transform.localScale += new Vector3(0.6f, 0.6f, 0.6f);
		base.renderer.material.color = Color.yellow;
		Vector3 torque = new Vector3(Random.Range(50f, 100f), Random.Range(50f, 100f), Random.Range(50f, 100f));
		if (Random.value < 0.5f)
		{
			torque.x = 0f - torque.x;
		}
		if (Random.value < 0.5f)
		{
			torque.y = 0f - torque.y;
		}
		if (Random.value < 0.5f)
		{
			torque.z = 0f - torque.z;
		}
		base.rigidbody.maxAngularVelocity = 1000f;
		base.rigidbody.AddTorque(torque);
		base.audio.PlayOneShot(m_SmackSound);
	}

	private bool DoButton(string s, bool showCube = true)
	{
		bool result = false;
		GUILayout.BeginHorizontal();
		GUILayout.Space(30f);
		if (GUILayout.Button(s + "."))
		{
			if (!showCube)
			{
				m_CubeScale = new Vector3(0.001f, 0.001f, 0.001f);
			}
			else
			{
				m_CubeScale = new Vector3(0.5f, 0.5f, 0.5f);
			}
			SetStatus(s + " ...");
			result = true;
		}
		GUILayout.EndHorizontal();
		return result;
	}

	private void SetStatus(string s)
	{
		m_StatusString = s;
		m_StatusColor = Color.yellow;
	}
}
