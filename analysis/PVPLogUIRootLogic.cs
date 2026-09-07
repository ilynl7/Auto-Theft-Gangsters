using System;
using System.Collections.Generic;
using SprotoType;

public class PVPLogUIRootLogic : SingletonUnity<PVPLogUIRootLogic>
{
	public UILabel titleLabel;

	public UIGrid LogListGrid;

	public UILabel contentLabel;

	public UIScrollView uIScrollView;

	private DelegateDefine.NoParamDelegate onClosed;

	private Logs GetLogs(string str)
	{
		Logs logs = null;
		string[] array = str.Split('^');
		try
		{
			long time = long.Parse(array[array.Length - 1]);
			string arg = DateTimeTool.LongToDateTimeLocal(time).ToString();
			logs = new Logs();
			logs.str = $"{arg}    {StrDictionary.GetServerDictionaryString(array[0])}";
			logs.time = time;
			return logs;
		}
		catch (Exception)
		{
			return null;
		}
	}

	public void UpdataLogList(List<string> list)
	{
		if (list == null)
		{
			return;
		}
		List<Logs> list2 = new List<Logs>();
		string text = string.Empty;
		for (int i = 0; i < list.Count; i++)
		{
			Logs logs = GetLogs(list[i]);
			if (logs != null)
			{
				list2.Add(logs);
			}
		}
		if (list2.Count > 0)
		{
			list2.Sort((Logs x, Logs y) => (int)(x.time - y.time));
		}
		for (int j = 0; j < list2.Count; j++)
		{
			text += list2[j].str;
			if (j != list2.Count - 1)
			{
				text += "\n";
			}
		}
		contentLabel.UpdateNGUIText();
		NGUIText.rectHeight = 1000000;
		string finalText = string.Empty;
		NGUIText.WrapText(text, out finalText);
		contentLabel.text = finalText;
		uIScrollView.ResetPosition();
	}

	public void Reset(string titlestr, DelegateDefine.NoParamDelegate closefun = null)
	{
		onClosed = closefun;
		titleLabel.text = StrDictionary.GetDictionaryString(titlestr);
		WaitResponseUIRootLogic.OpenWaitBox(211, 10f, 0f);
		request_rank_pvp_history.request rpcReq = new request_rank_pvp_history.request();
		NetLogic.GetInstance().Send<Protocol.request_rank_pvp_history>(rpcReq);
	}

	public void OnlClickClose()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PVPLogUIRoot);
		if (onClosed != null)
		{
			onClosed();
		}
	}
}
