using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager
{
	private static Dictionary<string, string[]> mDictionary = new Dictionary<string, string[]>();

	private static int mLanguageIndex = -1;

	private static string mLanguage;

	public static bool ResourceLoadDictionary()
	{
		TextAsset textAsset = Resources.Load("LocalizationBase", typeof(TextAsset)) as TextAsset;
		SystemLanguage systemLanguage = Application.systemLanguage;
		string @string = PlayerPrefs.GetString("Language", "English");
		@string = "English";
		if (textAsset != null && LoadCSV(textAsset))
		{
			SelectLanguage(@string);
			return true;
		}
		return false;
	}

	public static bool BundleLoadDictionary()
	{
		TextAsset textAsset = BundleManager.LoadTable("Localization") as TextAsset;
		SystemLanguage systemLanguage = Application.systemLanguage;
		string @string = PlayerPrefs.GetString("Language", "English");
		@string = "English";
		Debug.Log(@string + "lange");
		if (textAsset != null && LoadCSV(textAsset))
		{
			SelectLanguage(@string);
			return true;
		}
		return false;
	}

	private static bool LoadAndSelect(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			if (mDictionary.Count == 0 && !BundleLoadDictionary())
			{
				return false;
			}
			if (SelectLanguage(value))
			{
				return true;
			}
		}
		return false;
	}

	public static bool LoadCSV(TextAsset asset)
	{
		ByteReader byteReader = new ByteReader(asset);
		BetterList<string> betterList = byteReader.ReadCSV();
		if (betterList.size < 2)
		{
			return false;
		}
		betterList[0] = "KEY";
		mDictionary.Clear();
		while (betterList != null)
		{
			AddCSV(betterList);
			betterList = byteReader.ReadCSV();
		}
		return true;
	}

	private static bool SelectLanguage(string language)
	{
		mLanguageIndex = -1;
		if (mDictionary.Count == 0)
		{
			return false;
		}
		if (mDictionary.TryGetValue("KEY", out var value))
		{
			for (int i = 0; i < value.Length; i++)
			{
				if (value[i] == language)
				{
					mLanguageIndex = i;
					mLanguage = language;
					PlayerPrefs.SetString("Language", mLanguage);
					UIRoot.Broadcast("OnLocalize");
					return true;
				}
			}
		}
		return false;
	}

	private static void AddCSV(BetterList<string> values)
	{
		if (values.size >= 2)
		{
			string[] array = new string[values.size - 1];
			for (int i = 1; i < values.size; i++)
			{
				array[i - 1] = values[i];
			}
			if (mDictionary.ContainsKey(values[0]))
			{
				Debug.LogWarning(values[0]);
			}
			mDictionary.Add(values[0], array);
		}
	}

	public static string Get(string key)
	{
		if (mLanguageIndex != -1 && mDictionary.TryGetValue(key, out var value) && mLanguageIndex < value.Length)
		{
			return value[mLanguageIndex];
		}
		return key;
	}

	[Obsolete("Use Localization.Get instead")]
	public static string Localize(string key)
	{
		return Get(key);
	}

	public static bool Exists(string key)
	{
		if (mLanguageIndex != -1)
		{
			return mDictionary.ContainsKey(key);
		}
		return false;
	}
}
