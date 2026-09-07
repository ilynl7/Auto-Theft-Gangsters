using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HitHyperlink : MonoBehaviour
{
	[CompilerGenerated]
	private static Dictionary<string, int> _003C_003Ef__switch_0024map5;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnClick()
	{
		UILabel component = GetComponent<UILabel>();
		string urlAtPosition = component.GetUrlAtPosition(UICamera.lastHit.point);
		if (urlAtPosition == null)
		{
			return;
		}
		Debug.Log("Hit->" + urlAtPosition.Substring(1));
		string text = urlAtPosition.Substring(1);
		if (text != null)
		{
			if (_003C_003Ef__switch_0024map5 == null)
			{
				_003C_003Ef__switch_0024map5 = new Dictionary<string, int>(0);
			}
			if (!_003C_003Ef__switch_0024map5.TryGetValue(text, out var _))
			{
			}
		}
	}
}
