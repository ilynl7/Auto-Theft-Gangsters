using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class MyFileUtil
{
	public static bool IsFileExist(string targetPath)
	{
		targetPath = targetPath.Replace('\\', '/');
		if (File.Exists(targetPath))
		{
			return true;
		}
		return false;
	}

	public static void CheckPath(string targetPath)
	{
		targetPath = targetPath.Replace('\\', '/');
		int num = targetPath.LastIndexOf(".");
		int num2 = targetPath.LastIndexOf("/");
		if (num > 0 && num2 < num)
		{
			targetPath = targetPath.Substring(0, num2);
		}
		if (Directory.Exists(targetPath))
		{
			return;
		}
		string[] array = targetPath.Split('/');
		string text = string.Empty;
		int num3 = array.Length;
		for (int i = 0; i < array.Length; i++)
		{
			text = text + array[i] + '/';
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
		}
	}

	public static void CheckPathFile(string targetPath)
	{
		targetPath = targetPath.Replace('\\', '/');
		int num = targetPath.LastIndexOf(".");
		int num2 = targetPath.LastIndexOf("/");
		if (num > 0 && num2 < num)
		{
			targetPath = targetPath.Substring(0, num2);
		}
		if (Directory.Exists(targetPath))
		{
			return;
		}
		string[] array = targetPath.Split('/');
		string text = string.Empty;
		int num3 = array.Length;
		for (int i = 0; i < array.Length - 1; i++)
		{
			text = text + array[i] + '/';
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
		}
	}

	public static bool GetStringFromFile(string path, ref string result)
	{
		try
		{
			if (!File.Exists(path))
			{
				result = string.Empty;
				return false;
			}
			FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			StreamReader streamReader = new StreamReader(fileStream);
			result = streamReader.ReadToEnd();
			streamReader.Close();
			fileStream.Close();
			return true;
		}
		catch (Exception ex)
		{
			Log.ERROR_MSG(ex.ToString());
			result = string.Empty;
			return false;
		}
	}

	public static bool GetIntFromFile(string path, out int result)
	{
		try
		{
			if (!File.Exists(path))
			{
				result = 0;
				return false;
			}
			FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			StreamReader streamReader = new StreamReader(fileStream);
			string s = streamReader.ReadToEnd();
			streamReader.Close();
			fileStream.Close();
			if (!int.TryParse(s, out result))
			{
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.ERROR_MSG(ex.ToString());
			result = 0;
			return false;
		}
	}

	public static void DeleteFolder(string path)
	{
		if (Directory.Exists(path))
		{
			string[] files = Directory.GetFiles(path);
			for (int i = 0; i < files.Length; i++)
			{
				File.Delete(files[i]);
			}
			string[] directories = Directory.GetDirectories(path);
			for (int j = 0; j < directories.Length; j++)
			{
				DeleteFolder(directories[j]);
			}
			Directory.Delete(path);
		}
	}

	public static void DeleteFile(string path)
	{
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}

	public static void CopyFolder(string resPath, string targetPath)
	{
		if (!Directory.Exists(resPath))
		{
			Log.DEBUG_MSG("Copy file resPath doesn't exist " + resPath);
		}
		CheckPath(targetPath);
		string[] files = Directory.GetFiles(resPath);
		for (int i = 0; i < files.Length; i++)
		{
			Debug.Log("res file : " + files[i] + " " + files[i].LastIndexOf("/"));
			Debug.Log("target file : " + targetPath + "/" + files[i].Substring(files[i].LastIndexOf('/') + 1));
			File.Copy(files[i].Replace("\\", "/"), targetPath + "/" + files[i].Substring(files[i].LastIndexOf("/") + 1), overwrite: true);
		}
		string[] directories = Directory.GetDirectories(resPath);
		for (int j = 0; j < directories.Length; j++)
		{
			CopyFolder(directories[j].Replace("\\", "/"), targetPath + "/" + directories[j].Substring(directories[j].LastIndexOf("/") + 1));
		}
	}

	public static string GetFileMD5(string filePath)
	{
		string result = string.Empty;
		string empty = string.Empty;
		byte[] array = null;
		FileStream fileStream = null;
		MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		try
		{
			fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			array = mD5CryptoServiceProvider.ComputeHash(fileStream);
			fileStream.Close();
			empty = BitConverter.ToString(array);
			empty = empty.Replace("-", string.Empty);
			result = empty;
		}
		catch (Exception ex)
		{
			Debug.Log("read md5 file error : " + filePath + "  e: " + ex.ToString());
		}
		return result;
	}

	public static bool GenerateFileList(string path, Dictionary<string, UpdateFileInfo> curDic)
	{
		try
		{
			FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			List<string> list = new List<string>(curDic.Keys);
			string empty = string.Empty;
			for (int i = 0; i < list.Count; i++)
			{
				empty = list[i] + "," + curDic[list[i]].md5 + "," + curDic[list[i]].size;
				streamWriter.WriteLine(empty);
			}
			streamWriter.Close();
			fileStream.Close();
			return true;
		}
		catch (Exception ex)
		{
			Log.ERROR_MSG("Generate FileList Fail : " + ex.ToString());
			return false;
		}
	}
}
