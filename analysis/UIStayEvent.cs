using System.Collections.Generic;
using UnityEngine;

public class UIStayEvent : MonoBehaviour
{
	public static UIEventTrigger current;

	public List<EventDelegate> onStay = new List<EventDelegate>();

	public List<EventDelegate> onClick = new List<EventDelegate>();

	private bool press;

	private float time = -1f;

	public bool click;

	private float startTime;

	private void OnPress(bool pressed)
	{
		if (pressed)
		{
			time = Time.realtimeSinceStartup + 1f;
			startTime = time;
		}
		else
		{
			click = startTime < Time.realtimeSinceStartup;
			time = -1f;
		}
	}

	private void OnClick()
	{
		if (!click)
		{
			EventDelegate.Execute(onClick);
		}
	}

	private void Update()
	{
		if (time > 0f && Time.realtimeSinceStartup > time)
		{
			EventDelegate.Execute(onStay);
			time = Time.realtimeSinceStartup + 0.2f;
		}
	}
}
