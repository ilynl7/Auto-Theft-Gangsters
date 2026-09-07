using System;
using System.Collections.Generic;

public class GuildLogUIRootLogic : SingletonUnity<GuildLogUIRootLogic>
{
	public UILabel contentLabel;

	public UIScrollView scrollView;

	private Logs GetLogs(string str)
	{
		Logs logs = null;
		string[] array = str.Split('^');
		try
		{
			long time = long.Parse(array[array.Length - 2]);
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

	private string GetStringInfo(string str)
	{
		string[] array = str.Split('^');
		try
		{
			string arg = DateTimeTool.LongToDateTimeLocal(long.Parse(array[array.Length - 2])).ToString();
			return $"{arg}    {StrDictionary.GetServerDictionaryString(array[0])}";
		}
		catch (Exception)
		{
			return string.Empty;
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
		contentLabel.text = text;
		scrollView.ResetPosition();
	}

	public void OnlClickClose()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildLogUIRootLogic);
	}
}
