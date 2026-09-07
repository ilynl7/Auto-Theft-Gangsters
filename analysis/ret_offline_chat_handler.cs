using System.Collections.Generic;
using Sproto;
using SprotoType;

public class ret_offline_chat_handler
{
	public static SprotoTypeBase ret_offline_chat_request(SprotoTypeBase req)
	{
		if (req is ret_offline_chat.request { HasChat_list: not false } request)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			List<chat_item> chat_list = request.chat_list;
			for (int i = 0; i < chat_list.Count; i++)
			{
				playerData.ChatHistory.OnReceiveChatMessage(chat_list[i]);
			}
		}
		return null;
	}
}
