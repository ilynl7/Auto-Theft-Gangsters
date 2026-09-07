using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class DataReader
{
	public static byte[] GetBytes(object obj)
	{
		TextAsset textAsset = obj as TextAsset;
		if (textAsset == null)
		{
			return null;
		}
		return textAsset.bytes;
	}

	public static List<T> LoadImportData<T>(string fileName)
	{
		object obj = BundleManager.LoadTable(fileName);
		if (obj == null)
		{
			obj = Resources.Load("Data/" + fileName);
			if (obj == null)
			{
				return null;
			}
		}
		ByteReader byteReader = new ByteReader(GetBytes(obj));
		BetterList<string> betterList;
		for (betterList = byteReader.ReadCSV(); betterList != null; betterList = byteReader.ReadCSV())
		{
			if (betterList[0].Contains("*"))
			{
				betterList.RemoveAt(0);
				break;
			}
		}
		List<T> list = new List<T>();
		int size = betterList.size;
		FieldInfo[] array = new FieldInfo[size];
		for (int i = 0; i < size; i++)
		{
			array[i] = typeof(T).GetField(betterList[i]);
		}
		BetterList<string> betterList2 = byteReader.ReadCSV();
		while (betterList2 != null)
		{
			if (string.IsNullOrEmpty(betterList2[0]) || !betterList2[0].Contains("*"))
			{
				betterList2 = byteReader.ReadCSV();
				continue;
			}
			betterList2.RemoveAt(0);
			T val = Activator.CreateInstance<T>();
			for (int j = 0; j < betterList2.size; j++)
			{
				object value = null;
				string text = betterList2[j];
				if (string.IsNullOrEmpty(text) || array[j] == null)
				{
					continue;
				}
				if (array[j].FieldType.IsEnum)
				{
					try
					{
						value = Enum.Parse(array[j].FieldType, text);
					}
					catch (Exception)
					{
						Log.WARING_MSG("error datafile=" + fileName + " fieldinfo=" + array[j].Name + " fieldinfoValue=" + text);
					}
				}
				else
				{
					try
					{
						value = Convert.ChangeType(text, array[j].FieldType);
					}
					catch (Exception)
					{
						Log.WARING_MSG("error datafile=" + fileName + " fieldinfo=" + array[j].Name + " fieldinfoValue=" + text + "  ID " + betterList2[0]);
					}
				}
				array[j].SetValue(val, value);
			}
			list.Add(val);
			betterList2 = byteReader.ReadCSV();
		}
		return list;
	}

	public static List<T> LoadImportDicData<T>(string fileName, string DicDataName)
	{
		object obj = BundleManager.LoadTable(fileName);
		if (obj == null)
		{
			obj = Resources.Load("Data/" + fileName);
			if (obj == null)
			{
				return null;
			}
		}
		ByteReader byteReader = new ByteReader(GetBytes(obj));
		BetterList<string> betterList;
		for (betterList = byteReader.ReadCSV(); betterList != null; betterList = byteReader.ReadCSV())
		{
			if (betterList[0].Contains("*"))
			{
				betterList.RemoveAt(0);
				break;
			}
		}
		List<string> list = new List<string>();
		for (int i = 0; i < betterList.size; i++)
		{
			list.Add(betterList[i]);
		}
		List<T> list2 = new List<T>();
		int count = list.Count;
		FieldInfo[] array = new FieldInfo[count];
		for (int j = 0; j < count; j++)
		{
			array[j] = typeof(T).GetField(list[j]);
		}
		FieldInfo field = typeof(T).GetField(DicDataName);
		BetterList<string> betterList2 = byteReader.ReadCSV();
		while (betterList2 != null)
		{
			if (string.IsNullOrEmpty(betterList2[0]) || !betterList2[0].Contains("*"))
			{
				betterList2 = byteReader.ReadCSV();
				continue;
			}
			betterList2.RemoveAt(0);
			T val = Activator.CreateInstance<T>();
			Dictionary<string, string> dictionary = field.GetValue(val) as Dictionary<string, string>;
			for (int k = 0; k < betterList2.size; k++)
			{
				object value = null;
				string text = betterList2[k];
				if (list[k].StartsWith("_"))
				{
					dictionary.Add(list[k], text);
				}
				if (string.IsNullOrEmpty(text) || array[k] == null)
				{
					continue;
				}
				if (array[k].FieldType.IsEnum)
				{
					try
					{
						value = Enum.Parse(array[k].FieldType, text);
					}
					catch (Exception)
					{
						Log.WARING_MSG("error datafile=" + fileName + " fieldinfo=" + array[k].Name + " fieldinfoValue=" + text);
					}
				}
				else
				{
					try
					{
						value = Convert.ChangeType(text, array[k].FieldType);
					}
					catch (Exception)
					{
						Log.WARING_MSG("error datafile=" + fileName + " fieldinfo=" + array[k].Name + " fieldinfoValue=" + text + "  ID " + betterList2[0]);
					}
				}
				array[k].SetValue(val, value);
			}
			list2.Add(val);
			betterList2 = byteReader.ReadCSV();
		}
		return list2;
	}

	public static Dictionary<T1, T2> LoadDicTable<T1, T2>(string fileName, string keyName, string dicName) where T2 : class
	{
		List<T2> list = LoadImportDicData<T2>(fileName, dicName);
		return LoadTable<T1, T2>(list, keyName);
	}

	public static Dictionary<string, MissionData> LoadMissionDataTable(string fileName, string keyName, ref Dictionary<int, List<string>> autoAcceptDic)
	{
		autoAcceptDic.Clear();
		List<MissionData> list = LoadImportData<MissionData>(fileName);
		return LoadMissionDataTable(list, keyName, ref autoAcceptDic);
	}

	public static Dictionary<string, MissionData> LoadMissionDataTable(List<MissionData> list, string keyName, ref Dictionary<int, List<string>> autoAcceptDic)
	{
		if (list != null)
		{
			Dictionary<string, MissionData> dictionary = new Dictionary<string, MissionData>(list.Count);
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					try
					{
						dictionary.Add(list[i].ID, list[i]);
					}
					catch (Exception)
					{
						Debug.Log(i.ToString());
						Debug.LogError(list[i].ID);
					}
					if (list[i].AutoAcceptLv > 0)
					{
						if (!autoAcceptDic.ContainsKey(list[i].AutoAcceptLv))
						{
							autoAcceptDic.Add(list[i].AutoAcceptLv, new List<string>());
						}
						if (!autoAcceptDic[list[i].AutoAcceptLv].Contains(list[i].ID))
						{
							autoAcceptDic[list[i].AutoAcceptLv].Add(list[i].ID);
						}
					}
				}
			}
			return dictionary;
		}
		return null;
	}

	public static Dictionary<T1, T2> LoadTable<T1, T2>(string fileName, string keyName) where T2 : class
	{
		List<T2> list = LoadImportData<T2>(fileName);
		return LoadTable<T1, T2>(list, keyName);
	}

	public static Dictionary<T1, T2> LoadTable<T1, T2>(List<T2> list, string keyName) where T2 : class
	{
		if (list != null)
		{
			Dictionary<T1, T2> dictionary = new Dictionary<T1, T2>(list.Count);
			FieldInfo field = typeof(T2).GetField(keyName);
			if (field != null && list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					T2 val = list[i];
					T1 val2 = (T1)field.GetValue(val);
					try
					{
						dictionary.Add(val2, val);
					}
					catch (Exception)
					{
						Debug.Log(field.Name + " " + keyName);
						Debug.Log(typeof(T2).Name);
						Debug.Log(i.ToString());
						Debug.LogError(val2);
					}
				}
			}
			return dictionary;
		}
		return null;
	}

	public static Dictionary<T1, List<T2>> LoadTableList<T1, T2>(string fileName, string keyName) where T2 : class
	{
		List<T2> list = LoadImportData<T2>(fileName);
		return LoadTableList<T1, T2>(list, keyName);
	}

	public static Dictionary<T1, List<T2>> LoadTableList<T1, T2>(List<T2> list, string keyName) where T2 : class
	{
		Dictionary<T1, List<T2>> dictionary = new Dictionary<T1, List<T2>>();
		FieldInfo field = typeof(T2).GetField(keyName);
		if (field != null && list != null)
		{
			for (int i = 0; i < list.Count; i++)
			{
				T2 val = list[i];
				T1 key = (T1)field.GetValue(val);
				List<T2> value = null;
				if (!dictionary.TryGetValue(key, out value))
				{
					value = new List<T2>();
					dictionary.Add(key, value);
				}
				value.Add(val);
			}
		}
		return dictionary;
	}

	public static T2 GetTableRow<T1, T2>(Dictionary<T1, T2> dataDict, T1 key) where T2 : class
	{
		if (dataDict != null && dataDict.TryGetValue(key, out var value))
		{
			return value;
		}
		return (T2)null;
	}

	public static List<T2> GetTableList<T1, T2>(Dictionary<T1, List<T2>> dataDict, T1 key) where T2 : class
	{
		if (dataDict.TryGetValue(key, out var value))
		{
			return value;
		}
		return null;
	}
}
