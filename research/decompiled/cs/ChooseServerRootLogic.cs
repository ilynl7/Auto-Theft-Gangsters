using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ChooseServerRootLogic : SingletonUnity<ChooseServerRootLogic>
{
	public UIGrid AreaGrid;

	public List<ServerAreaLogic> AreaBtn;

	private int CurAreaID = -2;

	private List<game_server> CurGameServerList = new List<game_server>();

	public List<ServerLineLogic> LineItems;

	public UIGrid LineGrid;

	private int CurLineIndex = -1;

	public List<ServerSubLineLogic> SubLineItems;

	public UIGrid SubLineGrid;

	private game_server curSelectServer;

	public UILabel EmptyTipsLabel;

	public void Reset()
	{
		int num = GameDefine.SERVER_AREA_NAME.Count - AreaBtn.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(AreaBtn[0].gameObject) as GameObject;
				AreaGrid.AddChild(gameObject.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.name = $"AreaBtn{AreaBtn.Count:d2}";
				AreaBtn.Add(gameObject.GetComponent<ServerAreaLogic>());
			}
			AreaGrid.Reposition();
		}
		for (int j = 0; j < AreaBtn.Count; j++)
		{
			AreaBtn[j].Reset(j - 1, OnClickAreaItem);
		}
		CurAreaID = -2;
		AreaBtn[0].OnClickAreaItem();
	}

	public void RefershServerList()
	{
		if (CurGameServerList.Count > 0)
		{
			CurGameServerList = SingletonUnity<MenuSceneController>.Instance.GetAreaServerList(CurAreaID);
			RefershServerItems();
		}
	}

	public void OnClickAreaItem(int areaid)
	{
		if (CurAreaID == areaid)
		{
			return;
		}
		CurAreaID = areaid;
		SelectAreaItem();
		CurGameServerList.Clear();
		CurGameServerList = SingletonUnity<MenuSceneController>.Instance.GetAreaServerList(CurAreaID);
		int num = CurGameServerList.Count / 10;
		if (CurGameServerList.Count % 10 != 0)
		{
			num++;
		}
		int num2 = num - LineItems.Count;
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				GameObject gameObject = Object.Instantiate(LineItems[0].gameObject) as GameObject;
				LineGrid.AddChild(gameObject.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.name = $"line{LineItems.Count:d2}";
				LineItems.Add(gameObject.GetComponent<ServerLineLogic>());
			}
		}
		for (int j = 0; j < LineItems.Count; j++)
		{
			if (j < num)
			{
				UnityVersionUtil.SetActiveRecursive(LineItems[j].gameObject, state: true);
				LineItems[j].Reset(j, OnClickLineItem);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(LineItems[j].gameObject, state: false);
			}
		}
		LineGrid.Reposition();
		CurLineIndex = -1;
		for (int k = 0; k < SubLineItems.Count; k++)
		{
			UnityVersionUtil.SetActiveRecursive(SubLineItems[k].gameObject, state: false);
		}
		if (CurGameServerList.Count > 0)
		{
			int index = 0;
			if (CurAreaID >= 0)
			{
				index = num - 1;
			}
			LineItems[index].OnClickLineItem();
			EmptyTipsLabel.enabled = false;
		}
		else
		{
			EmptyTipsLabel.enabled = CurAreaID >= 0;
		}
	}

	private void SelectAreaItem()
	{
		for (int i = 0; i < AreaBtn.Count; i++)
		{
			AreaBtn[i].RefershSelect(CurAreaID);
		}
	}

	public void OnClickLineItem(int lineindex)
	{
		if (CurLineIndex == lineindex)
		{
			return;
		}
		CurLineIndex = lineindex;
		SelectLineItem();
		int num = 10;
		if (num > CurGameServerList.Count - CurLineIndex * 10)
		{
			num = CurGameServerList.Count - CurLineIndex * 10;
		}
		int num2 = num - SubLineItems.Count;
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				GameObject gameObject = Object.Instantiate(SubLineItems[0].gameObject) as GameObject;
				SubLineGrid.AddChild(gameObject.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.name = $"lineitem{SubLineItems.Count:d2}";
				SubLineItems.Add(gameObject.GetComponent<ServerSubLineLogic>());
			}
		}
		for (int j = 0; j < SubLineItems.Count; j++)
		{
			if (j < num)
			{
				UnityVersionUtil.SetActiveRecursive(SubLineItems[j].gameObject, state: true);
				SubLineItems[j].Reset(CurGameServerList[CurLineIndex * 10 + j], OnClickSubLineItem);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(SubLineItems[j].gameObject, state: false);
			}
		}
		SubLineGrid.Reposition();
	}

	public void RefershServerItems()
	{
		if (CurLineIndex == -1)
		{
			return;
		}
		SelectLineItem();
		int num = 10;
		if (num > CurGameServerList.Count - CurLineIndex * 10)
		{
			num = CurGameServerList.Count - CurLineIndex * 10;
		}
		int num2 = num - SubLineItems.Count;
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				GameObject gameObject = Object.Instantiate(SubLineItems[0].gameObject) as GameObject;
				SubLineGrid.AddChild(gameObject.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.name = $"lineitem{SubLineItems.Count:d2}";
				SubLineItems.Add(gameObject.GetComponent<ServerSubLineLogic>());
			}
		}
		for (int j = 0; j < SubLineItems.Count; j++)
		{
			if (j < num)
			{
				UnityVersionUtil.SetActiveRecursive(SubLineItems[j].gameObject, state: true);
				SubLineItems[j].Reset(CurGameServerList[CurLineIndex * 10 + j], OnClickSubLineItem);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(SubLineItems[j].gameObject, state: false);
			}
		}
		SubLineGrid.Reposition();
	}

	public void OnClickSubLineItem(game_server serverinfo)
	{
		if (serverinfo.HasServerState && serverinfo.serverState == 3)
		{
			MessageBoxLogic.OpenOKBox("#{200128}", "#{100127}");
			return;
		}
		curSelectServer = serverinfo;
		PlayerData.CurGameServerData = new game_server();
		PlayerData.CurGameServerData.serverId = curSelectServer.serverId;
		PlayerData.CurGameServerData.serverIP = curSelectServer.serverIP;
		PlayerData.CurGameServerData.serverPort = curSelectServer.serverPort;
		PlayerData.CurGameServerData.serverName = curSelectServer.serverName;
		PlayerData.CurGameServerData.serverState = curSelectServer.serverState;
		if (curSelectServer.HasNewServer)
		{
			PlayerData.CurGameServerData.newServer = curSelectServer.newServer;
		}
		OnClickCloseBtn();
	}

	private void SelectLineItem()
	{
		for (int i = 0; i < LineItems.Count; i++)
		{
			LineItems[i].RefershSelect(CurLineIndex);
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ChooseServerRootUI);
		SingletonUnity<LoginRootLogic>.Instance.Reset(string.Empty, string.Empty);
	}
}
