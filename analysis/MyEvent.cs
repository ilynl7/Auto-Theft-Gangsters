using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MyEvent : SingletonUnity<MyEvent>
{
	public struct Pair
	{
		public object owner;

		public string funcname;

		public MethodInfo method;
	}

	public class EventInfo
	{
		public Pair info;

		public object[] args;

		public float delaytime;
	}

	private Dictionary<string, List<Pair>> events = new Dictionary<string, List<Pair>>();

	private List<EventInfo> delayevents = new List<EventInfo>();

	public bool Register(string eventname, object own, string funname)
	{
		DeRegister(eventname, own, funname);
		List<Pair> value = null;
		Pair item = default(Pair);
		item.owner = own;
		item.funcname = funname;
		item.method = own.GetType().GetMethod(funname);
		if (item.method == null)
		{
			Debug.LogError(string.Concat("MyEvent::Register ", own, " not found method[", funname, "]"));
			return false;
		}
		if (!events.TryGetValue(eventname, out value))
		{
			value = new List<Pair>();
			value.Add(item);
			events.Add(eventname, value);
			return true;
		}
		value.Add(item);
		return true;
	}

	public bool DeRegister(string eventname, object own, string funname)
	{
		List<Pair> value = null;
		if (!events.TryGetValue(eventname, out value))
		{
			return false;
		}
		for (int i = 0; i < value.Count; i++)
		{
			if (value[i].funcname == funname && value[i].owner == own)
			{
				value.RemoveAt(i);
				return true;
			}
		}
		return false;
	}

	public void Fire(string eventname, params object[] args)
	{
		List<Pair> value = null;
		if (!events.TryGetValue(eventname, out value))
		{
			Debug.LogWarning("MyEvent::Fire " + eventname + " not found");
			return;
		}
		for (int num = value.Count - 1; num >= 0; num--)
		{
			try
			{
				value[num].method.Invoke(value[num].owner, args);
			}
			catch (Exception ex)
			{
				Debug.LogError("MyEvent::Fire " + eventname + " " + ex.ToString());
			}
		}
	}

	public void DelayFire(string eventname, float delaytime, params object[] args)
	{
		if (delaytime <= 0f)
		{
			Fire(eventname, args);
		}
		List<Pair> value = null;
		if (!events.TryGetValue(eventname, out value))
		{
			Debug.LogWarning("MyEvent::Fire " + eventname + " not found");
			return;
		}
		for (int i = 0; i < value.Count; i++)
		{
			EventInfo eventInfo = new EventInfo();
			eventInfo.delaytime = delaytime;
			eventInfo.info = value[i];
			eventInfo.args = args;
			delayevents.Add(eventInfo);
		}
	}

	private void UpdateDelayEvents()
	{
		if (delayevents.Count <= 0)
		{
			return;
		}
		int num = 0;
		while (num < delayevents.Count)
		{
			EventInfo eventInfo = delayevents[num];
			eventInfo.delaytime -= Time.deltaTime;
			if (eventInfo.delaytime <= 0f)
			{
				try
				{
					eventInfo.info.method.Invoke(eventInfo.info.owner, eventInfo.args);
				}
				catch (Exception ex)
				{
					Debug.LogError("MyEvent::UpdateDelayEvents  " + ex.ToString());
				}
				delayevents.RemoveAt(num);
			}
			else
			{
				num++;
			}
		}
	}

	public bool HasRegister(string eventname)
	{
		return events.ContainsKey(eventname);
	}

	public void Clear()
	{
		delayevents.Clear();
		events.Clear();
	}

	private void Update()
	{
		UpdateDelayEvents();
	}
}
