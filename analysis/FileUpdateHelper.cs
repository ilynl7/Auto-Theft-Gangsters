using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileUpdateHelper : MonoBehaviour
{
	private delegate void OnGetResVersionDelegate(bool isSuccess);

	private delegate void OnDownloadFileListDelegate(bool isSuccess);

	public delegate void OnChangeUpdateStepDelegate(UPDATE_STEP nStep);

	private UPDATE_STEP mCurUpdateStape = UPDATE_STEP.INVALID;

	private UPDATE_RESULT mCurUpdateResult = UPDATE_RESULT.INVALID;

	public static string ResCachePath = string.Empty;

	public static string ApkVersionFolderName = "/UpdateInfo";

	public static string LocalVersionPath = string.Empty;

	public static string CacheVersionPath = string.Empty;

	public static string DownloadDataFolderName = "StreamingAssets";

	public static string VersionFileName = "Version.info";

	public static string ResFileListName = "UpdateFileList_1.info";

	public static string LocalPathRoot = string.Empty;

	public string mServerUrl = "http://liuqinglin.cdn-doodlemobile.com/MMO_UNITY4";

	private string mServerResUrl = string.Empty;

	private string mCacheDataPath = string.Empty;

	private string mServerDataPath = string.Empty;

	private string mServerResRoot = "/RES";

	private int mLocalVersion = -1;

	private int mServerVersion = -1;

	private Dictionary<string, UpdateFileInfo> mLocalFileDic = new Dictionary<string, UpdateFileInfo>();

	private Dictionary<string, UpdateFileInfo> mServerFileDic = new Dictionary<string, UpdateFileInfo>();

	private List<string> mUpdateFilesList = new List<string>();

	private List<string> mUpdateErrorFilesList = new List<string>();

	private List<DownloadUrlInfo> mDownloadUrlList = new List<DownloadUrlInfo>();

	private DownloadHelper mDataFileDownloadHelper;

	private DownloadHelper mVersionFileDownloadHelper;

	private long mNeedDownloadSize;

	public OnChangeUpdateStepDelegate OnChangeUpdateStep;

	private bool IsNeedSameVersion;

	private bool mIsNeedCopyRes = true;

	public UPDATE_STEP CurUpdateStape => mCurUpdateStape;

	public UPDATE_RESULT CurUpdateResult => mCurUpdateResult;

	public long CurDownloadSize => (mDataFileDownloadHelper == null) ? 0 : mDataFileDownloadHelper.AlreadyDownloadSize;

	public long NeedDownloadSize => mNeedDownloadSize;

	public bool IsNeedCopyRes
	{
		get
		{
			return mIsNeedCopyRes;
		}
		set
		{
			mIsNeedCopyRes = value;
		}
	}

	public static string GetResCachePath()
	{
		if (string.IsNullOrEmpty(ResCachePath))
		{
			ResCachePath = $"{Application.temporaryCachePath}/CACHERES_UNITY4";
		}
		return ResCachePath;
	}

	public static string GetLocalVersionPath()
	{
		if (string.IsNullOrEmpty(LocalVersionPath))
		{
			LocalVersionPath = $"{Application.persistentDataPath}/UpdateInfo_UNITY4";
		}
		return LocalVersionPath;
	}

	public static string GetCacheVersionPath()
	{
		if (string.IsNullOrEmpty(CacheVersionPath))
		{
			CacheVersionPath = $"{GetResCachePath()}/UpdateInfo_UNITY4";
		}
		return CacheVersionPath;
	}

	public static string GetLocalPathRoot()
	{
		if (string.IsNullOrEmpty(LocalPathRoot))
		{
			LocalPathRoot = $"{Application.persistentDataPath}/ResData_UNITY4";
		}
		return LocalPathRoot;
	}

	public static string GetOldPathRoot()
	{
		return $"{Application.persistentDataPath}/ResData";
	}

	public static string GetOldVersionPathRoot()
	{
		return $"{Application.persistentDataPath}/UpdateInfo";
	}

	private void Awake()
	{
		if (GameSettingData.IsDownLoadInLocal)
		{
			mServerUrl = "http://192.168.1.97:8080/MMOGTA";
		}
		ResCachePath = Application.temporaryCachePath + "/CACHERES_UNITY4";
		LocalVersionPath = Application.persistentDataPath + "/UpdateInfo_UNITY4";
		LocalPathRoot = Application.persistentDataPath + "/ResData_UNITY4";
		CacheVersionPath = ResCachePath + "/UpdateInfo_UNITY4";
		mCacheDataPath = ResCachePath + "/" + DownloadDataFolderName;
		mCurUpdateStape = UPDATE_STEP.INVALID;
		mCurUpdateResult = UPDATE_RESULT.INVALID;
	}

	public void StartCheckRes(string resServerUrl, OnChangeUpdateStepDelegate func, bool isNeedSameVersion, bool isNeedCopyRes)
	{
		IsNeedSameVersion = isNeedSameVersion;
		mServerVersion = -1;
		mIsNeedCopyRes = isNeedCopyRes;
		if (mVersionFileDownloadHelper != null)
		{
			mVersionFileDownloadHelper.DisposeWWW();
		}
		if (mDataFileDownloadHelper != null)
		{
			mDataFileDownloadHelper.DisposeWWW();
		}
		mServerUrl = resServerUrl;
		mServerResUrl = $"{mServerUrl}{mServerResRoot}_{PlayerData.ServerDataVersion}";
		mServerDataPath = $"{mServerUrl}{mServerResRoot}_{PlayerData.ServerDataVersion}";
		mServerVersion = -1;
		mVersionFileDownloadHelper = null;
		mDataFileDownloadHelper = null;
		mNeedDownloadSize = 0L;
		OnChangeUpdateStep = func;
		ChangeUpdateStep(UPDATE_STEP.CHECK_VERSION);
		if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			GetResVersion();
		}
	}

	private void GetResVersion()
	{
		mLocalVersion = -1;
		int result = -1;
		string path = LocalVersionPath + "/" + VersionFileName;
		string apkDataUrl = GetApkDataUrl(ApkVersionFolderName, VersionFileName);
		if (File.Exists(path) && !MyFileUtil.GetIntFromFile(path, out mLocalVersion))
		{
			Log.ERROR_MSG("parse version fail");
		}
		string result2 = string.Empty;
		if (File.Exists(apkDataUrl) && !MyFileUtil.GetStringFromFile(apkDataUrl, ref result2))
		{
			Log.ERROR_MSG("parse version fail");
		}
		if (!int.TryParse(result2, out result))
		{
			result = 0;
		}
		if (result > mLocalVersion)
		{
			MyFileUtil.DeleteFolder(LocalVersionPath);
			MyFileUtil.DeleteFolder(LocalPathRoot);
			mLocalVersion = result;
		}
		mServerVersion = PlayerData.ServerDataVersion;
		OnGetResVersion(isSuccess: true);
	}

	private void OnGetResVersion(bool isSuccess)
	{
		if (isSuccess)
		{
			if (IsNeedSameVersion)
			{
				if (mServerVersion != mLocalVersion && PlayerData.downLoadFlag == 1)
				{
					MyFileUtil.DeleteFolder(CacheVersionPath);
					ChangeUpdateStep(UPDATE_STEP.GET_FILELIST);
					mVersionFileDownloadHelper = DownloadHelper.StartDownload($"{mServerUrl}{mServerResRoot}_{mServerVersion:D3}/{ResFileListName}", isRemote: true, CacheVersionPath + "/" + ResFileListName, this, OnDownloadServerResFileList, GameSettingData.DownloadThreadCount[GameSettingData.GetPhoneClass()]);
				}
				else
				{
					CheckLocalFileList();
				}
			}
			else if (mServerVersion > mLocalVersion && PlayerData.downLoadFlag == 1)
			{
				MyFileUtil.DeleteFolder(CacheVersionPath);
				ChangeUpdateStep(UPDATE_STEP.GET_FILELIST);
				mVersionFileDownloadHelper = DownloadHelper.StartDownload($"{mServerUrl}{mServerResRoot}_{mServerVersion:D3}/{ResFileListName}", isRemote: true, CacheVersionPath + "/" + ResFileListName, this, OnDownloadServerResFileList, GameSettingData.DownloadThreadCount[GameSettingData.GetPhoneClass()]);
			}
			else
			{
				CheckLocalFileList();
			}
		}
		else
		{
			UpdateFinish(UPDATE_RESULT.GET_VERSION_FAIL);
		}
	}

	private void CheckLocalFileList()
	{
		ChangeUpdateStep(UPDATE_STEP.COMPARE_RES);
		string path = $"{LocalVersionPath}/{ResFileListName}";
		if (!File.Exists(path))
		{
			if (PlayerData.downLoadFlag == 0)
			{
				UpdateFinish(UPDATE_RESULT.SUCCESS);
			}
			else
			{
				OnDownloadLocalResFileListLocalCheck(isSuccess: false);
			}
		}
		else
		{
			OnDownloadLocalResFileListLocalCheck(isSuccess: true);
		}
	}

	private void OnDownloadLocalResFileListLocalCheck(bool isSuccess)
	{
		if (isSuccess)
		{
			string fileListPath = $"{LocalVersionPath}/{ResFileListName}";
			mLocalFileDic.Clear();
			mUpdateFilesList.Clear();
			ReadFileListToDic(fileListPath, mLocalFileDic);
			List<string> list = new List<string>(mLocalFileDic.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				if (!MyFileUtil.IsFileExist(LocalPathRoot + "/" + list[i]) || MyFileUtil.GetFileMD5(LocalPathRoot + "/" + list[i]) != mLocalFileDic[list[i]].md5)
				{
					MyFileUtil.DeleteFolder(CacheVersionPath);
					ChangeUpdateStep(UPDATE_STEP.GET_FILELIST);
					mVersionFileDownloadHelper = DownloadHelper.StartDownload($"{mServerUrl}{mServerResRoot}_{mServerVersion:D3}/{ResFileListName}", isRemote: true, CacheVersionPath + "/" + ResFileListName, this, OnDownloadServerResFileList, GameSettingData.DownloadThreadCount[GameSettingData.GetPhoneClass()]);
					return;
				}
			}
			UpdateFinish(UPDATE_RESULT.SUCCESS);
		}
		else
		{
			MyFileUtil.DeleteFolder(CacheVersionPath);
			ChangeUpdateStep(UPDATE_STEP.GET_FILELIST);
			mVersionFileDownloadHelper = DownloadHelper.StartDownload($"{mServerUrl}{mServerResRoot}_{mServerVersion:D3}/{ResFileListName}", isRemote: true, CacheVersionPath + "/" + ResFileListName, this, OnDownloadServerResFileList, GameSettingData.DownloadThreadCount[GameSettingData.GetPhoneClass()]);
		}
	}

	private void OnDownloadServerResFileList(bool isSuccess)
	{
		if (isSuccess)
		{
			ChangeUpdateStep(UPDATE_STEP.COMPARE_RES);
			string text = $"{LocalVersionPath}/{ResFileListName}";
			if (!File.Exists(text))
			{
				mVersionFileDownloadHelper = DownloadHelper.StartDownload(GetApkDataUrl(ApkVersionFolderName, ResFileListName), isRemote: false, text, this, OnDownloadLocalResFileList, GameSettingData.DownloadThreadCount[GameSettingData.GetPhoneClass()]);
			}
			else
			{
				OnDownloadLocalResFileList(isSuccess: true);
			}
		}
		else
		{
			UpdateFinish(UPDATE_RESULT.GET_FILELIST_FAIL);
		}
	}

	private void OnDownloadLocalResFileList(bool isSuccess)
	{
		if (isSuccess)
		{
			string fileListPath = $"{LocalVersionPath}/{ResFileListName}";
			string fileListPath2 = $"{CacheVersionPath}/{ResFileListName}";
			mLocalFileDic.Clear();
			mServerFileDic.Clear();
			mUpdateFilesList.Clear();
			ReadFileListToDic(fileListPath, mLocalFileDic);
			if (!ReadFileListToDic(fileListPath2, mServerFileDic))
			{
				UpdateFinish(UPDATE_RESULT.LOAD_SERVER_FILELIST_ERROR);
				return;
			}
			if (Directory.Exists(GetOldPathRoot()))
			{
				MyFileUtil.DeleteFolder(GetOldPathRoot());
			}
			if (Directory.Exists(GetOldVersionPathRoot()))
			{
				MyFileUtil.DeleteFolder(GetOldVersionPathRoot());
			}
			List<string> list = new List<string>(mLocalFileDic.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				if (!mServerFileDic.ContainsKey(list[i]))
				{
					MyFileUtil.DeleteFile(LocalPathRoot + "/" + list[i]);
					mLocalFileDic.Remove(list[i]);
				}
			}
			List<string> list2 = new List<string>(mServerFileDic.Keys);
			for (int j = 0; j < list2.Count; j++)
			{
				if (mLocalFileDic.ContainsKey(list2[j]))
				{
					if (!MyFileUtil.IsFileExist(LocalPathRoot + "/" + list2[j]) || MyFileUtil.GetFileMD5(LocalPathRoot + "/" + list2[j]) != mServerFileDic[list2[j]].md5)
					{
						mUpdateFilesList.Add(list2[j]);
					}
				}
				else
				{
					mUpdateFilesList.Add(list2[j]);
				}
			}
			if (mUpdateFilesList.Count > 0)
			{
				mDownloadUrlList.Clear();
				mNeedDownloadSize = 0L;
				for (int k = 0; k < mUpdateFilesList.Count; k++)
				{
					string text = $"{mCacheDataPath}/{mUpdateFilesList[k]}";
					if (!File.Exists(text) || MyFileUtil.GetFileMD5(text) != mServerFileDic[mUpdateFilesList[k]].md5)
					{
						mDownloadUrlList.Add(new DownloadUrlInfo(mServerDataPath + "/" + mUpdateFilesList[k], text, mServerFileDic[mUpdateFilesList[k]].size));
						mNeedDownloadSize += mServerFileDic[mUpdateFilesList[k]].size;
					}
				}
				if (mDownloadUrlList.Count > 0)
				{
					ChangeUpdateStep(UPDATE_STEP.CHECK_IS_DOWNLOAD);
					return;
				}
			}
			OnDownloadRes(isSuccess: true);
		}
		else
		{
			UpdateFinish(UPDATE_RESULT.GET_FILELIST_FAIL);
		}
	}

	public void DownloadFileList()
	{
		ChangeUpdateStep(UPDATE_STEP.DOWNLOAD_RES);
		if (mDataFileDownloadHelper == null)
		{
			mDataFileDownloadHelper = DownloadHelper.StartDownload(mDownloadUrlList, isRemote: true, this, mNeedDownloadSize, OnDownloadRes, GameSettingData.DownloadThreadCount[GameSettingData.GetPhoneClass()]);
		}
	}

	public void ContinueDownload()
	{
		ChangeUpdateStep(UPDATE_STEP.DOWNLOAD_RES);
		if (mDataFileDownloadHelper != null)
		{
			mDataFileDownloadHelper.ContinueDownload();
		}
	}

	private void OnDownloadRes(bool isSuccess)
	{
		if (!mIsNeedCopyRes)
		{
			UpdateFinish(UPDATE_RESULT.SUCCESS);
		}
		else if (isSuccess)
		{
			ChangeUpdateStep(UPDATE_STEP.CHECK_RES);
			mUpdateErrorFilesList.Clear();
			for (int i = 0; i < mUpdateFilesList.Count; i++)
			{
				string text = $"{mCacheDataPath}/{mUpdateFilesList[i]}";
				if (!File.Exists(text))
				{
					mUpdateErrorFilesList.Add(mUpdateFilesList[i]);
					continue;
				}
				string fileMD = MyFileUtil.GetFileMD5(text);
				if (!mServerFileDic.ContainsKey(mUpdateFilesList[i]) || fileMD != mServerFileDic[mUpdateFilesList[i]].md5)
				{
					mUpdateErrorFilesList.Add(mUpdateFilesList[i]);
				}
				else if (mLocalFileDic.ContainsKey(mUpdateFilesList[i]))
				{
					mLocalFileDic[mUpdateFilesList[i]].CopyData(mServerFileDic[mUpdateFilesList[i]]);
				}
				else
				{
					mLocalFileDic.Add(mUpdateFilesList[i], mServerFileDic[mUpdateFilesList[i]]);
				}
			}
			if (mUpdateErrorFilesList.Count == 0)
			{
				CopyResToDataPath();
			}
			else
			{
				UpdateFinish(UPDATE_RESULT.DOWNLOAD_INCOMPLETE);
			}
		}
		else
		{
			UpdateFinish(UPDATE_RESULT.DOWNLOAD_FAIL);
		}
	}

	private void CopyResToDataPath()
	{
		try
		{
			ChangeUpdateStep(UPDATE_STEP.COPY_RES);
			string empty = string.Empty;
			for (int i = 0; i < mUpdateFilesList.Count; i++)
			{
				empty = LocalPathRoot + "/" + mUpdateFilesList[i];
				MyFileUtil.CheckPath(empty);
				File.Copy(mCacheDataPath + "/" + mUpdateFilesList[i], empty, overwrite: true);
			}
		}
		catch (Exception ex)
		{
			Log.ERROR_MSG("copy File fail : " + ex.ToString());
			UpdateFinish(UPDATE_RESULT.COPY_FILE_FAIL);
			return;
		}
		GenerateLocalFileList();
	}

	private void GenerateLocalFileList()
	{
		try
		{
			string path = LocalVersionPath + "/" + ResFileListName;
			MyFileUtil.DeleteFile(path);
			if (!MyFileUtil.GenerateFileList(path, mLocalFileDic))
			{
				Log.ERROR_MSG("Generate local FileList fail");
				UpdateFinish(UPDATE_RESULT.GENERATE_VERSION_FILE_FAIL);
				return;
			}
		}
		catch (Exception ex)
		{
			Log.ERROR_MSG("GenerateLocalFileList : " + ex.ToString());
			UpdateFinish(UPDATE_RESULT.GENERATE_VERSION_FILE_FAIL);
			return;
		}
		GenerateVersionFile();
	}

	private void GenerateVersionFile()
	{
		try
		{
			string text = LocalVersionPath + "/" + VersionFileName;
			MyFileUtil.CheckPath(text);
			FileStream fileStream = new FileStream(text, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.WriteLine($"{mServerVersion:D3}");
			streamWriter.Close();
			fileStream.Close();
			PlayerData.LocalDataVersion = mServerVersion;
		}
		catch (Exception)
		{
			UpdateFinish(UPDATE_RESULT.GENERATE_VERSION_FILE_FAIL);
			return;
		}
		ClearCacheFiles();
	}

	private void ClearCacheFiles()
	{
		ChangeUpdateStep(UPDATE_STEP.CLEAR_CACHE);
		try
		{
			MyFileUtil.DeleteFolder(mCacheDataPath);
			MyFileUtil.DeleteFolder(CacheVersionPath);
		}
		catch (Exception)
		{
			UpdateFinish(UPDATE_RESULT.CLEAN_CACHE_FAIL);
			return;
		}
		UpdateFinish(UPDATE_RESULT.SUCCESS);
	}

	private void UpdateFinish(UPDATE_RESULT result)
	{
		if (result != 0)
		{
			Debug.Log("DownloadError :: " + result);
		}
		mCurUpdateResult = result;
		ChangeUpdateStep(UPDATE_STEP.FINISH);
	}

	private void ChangeUpdateStep(UPDATE_STEP newStep)
	{
		mCurUpdateStape = newStep;
		if (OnChangeUpdateStep != null)
		{
			OnChangeUpdateStep(newStep);
		}
	}

	private bool ReadFileListToDic(string fileListPath, Dictionary<string, UpdateFileInfo> curDic)
	{
		try
		{
			if (!File.Exists(fileListPath))
			{
				return false;
			}
			FileStream fileStream = new FileStream(fileListPath, FileMode.Open, FileAccess.Read);
			StreamReader streamReader = new StreamReader(fileStream);
			string empty = string.Empty;
			string[] array = null;
			while (!streamReader.EndOfStream)
			{
				empty = streamReader.ReadLine();
				array = empty.Split(',');
				UpdateFileInfo value = new UpdateFileInfo(array[1], long.Parse(array[2]));
				curDic.Add(array[0], value);
			}
			streamReader.Close();
			fileStream.Close();
			return true;
		}
		catch (Exception ex)
		{
			Log.ERROR_MSG("Read FileList Error : " + fileListPath + "  E: " + ex.ToString());
			return false;
		}
	}

	public static string GetApkDataUrl(string folderPath, string fileName)
	{
		return Application.streamingAssetsPath + folderPath + "/" + fileName;
	}

	public float GetDownloadProgress()
	{
		return mDataFileDownloadHelper.GetDownloadProgress();
	}

	private void OnDestroy()
	{
		if (mDataFileDownloadHelper != null)
		{
			mDataFileDownloadHelper.DisposeWWW();
		}
		if (mVersionFileDownloadHelper != null)
		{
			mVersionFileDownloadHelper.DisposeWWW();
		}
	}
}
