using System.Collections.Generic;
using UnityEngine;

public class ChatRecord : MonoBehaviour
{
	public UILabel MainText;

	public static bool canCall = false;

	private static List<string> WordChatRecordList = new List<string>();

	public string red = "[cc0033][u][url=Some Message or Link]";

	public string end = "[/url][/u][-]";

	public Dictionary<long, string> FriendChatRecordDic = new Dictionary<long, string>();

	public static void AddChatRecord(string data, bool isFilterRepeate = false, long SendId = 0)
	{
		string text = string.Empty;
		if (!string.IsNullOrEmpty(data))
		{
			char c = data[0];
			text = ((c == '#') ? StrDictionary.GetServerDictionaryString(data) : data);
		}
		if (WordChatRecordList.Count > 0 && isFilterRepeate)
		{
			if (WordChatRecordList[WordChatRecordList.Count - 1] != text)
			{
				WordChatRecordList.Add(text);
			}
		}
		else
		{
			WordChatRecordList.Add(text);
		}
		canCall = true;
	}

	public static string GetaRecord()
	{
		return WordChatRecordList[WordChatRecordList.Count - 1];
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (canCall)
		{
			UpdataToMianText();
			canCall = false;
		}
	}

	private void UpdataToMianText()
	{
		string text = GetaRecord();
		string newValue = red.Replace("Some Message or Link", getHyperlinkType(text));
		text = text.Replace("{", newValue);
		text = text.Replace("}", end);
		UILabel mainText = MainText;
		mainText.text = mainText.text + text + "\n";
		Vector3 localPosition = MainText.gameObject.transform.localPosition;
		localPosition.y -= 10f;
		MainText.gameObject.transform.localPosition = localPosition;
		Vector3 size = MainText.gameObject.GetComponent<BoxCollider>().size;
		size.y += 20f;
		MainText.gameObject.GetComponent<BoxCollider>().size = size;
	}

	private string getHyperlinkType(string str)
	{
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] != '{')
			{
				continue;
			}
			for (int j = i; j < str.Length; j++)
			{
				if (str[j] == '}')
				{
					return str.Substring(i, j - i);
				}
			}
		}
		return string.Empty;
	}
}
