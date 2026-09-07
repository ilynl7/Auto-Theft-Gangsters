using SprotoType;
using UnityEngine;

public class ServerSubLineLogic : MonoBehaviour
{
	public delegate void ClickServer(game_server server);

	public UISprite StateSp;

	public UILabel ServerName;

	private game_server CurServer;

	public UISprite NewFlag;

	private ClickServer clickitem;

	public void Reset(game_server serverinfo, ClickServer lineserver = null)
	{
		CurServer = serverinfo;
		ServerName.text = CurServer.serverName;
		if (serverinfo.HasNewServer && serverinfo.newServer == 1 && CurServer.serverState != 3)
		{
			StateSp.color = Color.red;
			NewFlag.enabled = true;
		}
		else
		{
			NewFlag.enabled = false;
			StateSp.color = GameDefine.SERVER_STATE_COLOR[(int)CurServer.serverState];
		}
		clickitem = lineserver;
	}

	public void OnClickServerItem()
	{
		if (clickitem != null)
		{
			clickitem(CurServer);
		}
	}
}
