using System;

public class StrDictionary
{
	public static string GetClientDictionaryString(string key, params object[] args)
	{
		if (string.IsNullOrEmpty(key))
		{
			return "Empty ---key erro!";
		}
		if (key.Length < 3)
		{
			return key + " -- ServerDictionaryFormat ERROR2 Length < 3";
		}
		string key2 = key.Substring(2, key.Length - 3);
		try
		{
			string text = string.Format(LocalizationManager.Get(key2), args);
			return text.Replace("#r", "\n");
		}
		catch (Exception)
		{
			return "formate erro!";
		}
	}

	public static string[] ParseArgs(string[] all)
	{
		if (all == null)
		{
			return null;
		}
		string[] array = new string[all.Length];
		for (int i = 0; i < all.Length; i++)
		{
			string text = all[i];
			if (!string.IsNullOrEmpty(text) && text[0] == '#')
			{
				array[i] = GetClientDictionaryString(text);
			}
			else
			{
				array[i] = all[i];
			}
		}
		return array;
	}

	public static string GetServerDictionaryString(string keystr)
	{
		if (string.IsNullOrEmpty(keystr))
		{
			return "Empty ---key erro!";
		}
		char c = keystr[0];
		if (c != '#')
		{
			return keystr;
		}
		int num = keystr.IndexOf('*');
		if (num > 0)
		{
			string key = keystr.Substring(0, num);
			string text = keystr.Substring(num + 1, keystr.Length - num - 1);
			string[] args = ParseArgs(text.Split('*'));
			return GetClientDictionaryString(key, args);
		}
		if (keystr.Length < 3)
		{
			return keystr + " -- ServerDictionaryFormat ERROR2 Length < 3";
		}
		string key2 = keystr.Substring(2, keystr.Length - 3);
		string text2 = LocalizationManager.Get(key2);
		return text2.Replace("#r", "\n");
	}

	public static string GetDictionaryString(string keystr, params object[] args)
	{
		string result = string.Empty;
		if (!string.IsNullOrEmpty(keystr))
		{
			char c = keystr[0];
			result = ((c == '#') ? GetClientDictionaryString(keystr, args) : string.Format(keystr, args));
		}
		return result;
	}
}
