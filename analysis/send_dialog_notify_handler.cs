using Sproto;
using SprotoType;
using UnityEngine;

public class send_dialog_notify_handler
{
	public static SprotoTypeBase send_dialog_notify_request(SprotoTypeBase req)
	{
		if (req is send_dialog_notify.request { type: var type } request)
		{
			int num = Random.Range(10, 15);
			if (request.HasParm)
			{
				MessageBoxLogic.OpenCancelWaitBox(StrDictionary.GetDictionaryString(request.key, request.parm), StrDictionary.GetDictionaryString("#{100127}"), num);
			}
			else
			{
				MessageBoxLogic.OpenCancelWaitBox(StrDictionary.GetDictionaryString(request.key), StrDictionary.GetDictionaryString("#{100127}"), num);
			}
		}
		return null;
	}
}
