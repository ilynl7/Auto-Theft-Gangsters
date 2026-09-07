using Sproto;
using SprotoType;
using UnityEngine;

public class update_queue_rank_handler
{
	public static SprotoTypeBase update_queue_rank_request(SprotoTypeBase req)
	{
		if (req is update_queue_rank.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			MessageBoxLogic.OpenWaitBox(StrDictionary.GetDictionaryString("#{100158}", request.rank), "#{100244}", 0f, 0f);
			Debug.Log(request.rank.ToString());
		}
		return null;
	}
}
