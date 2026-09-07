using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DownloadHelper
{
	public delegate void OnDownloadFinished(bool isSuccess);

	private OnDownloadFinished mCurDownloadDelegate;

	private long mAlreadyDownloadSize;

	public float NeedDownloadSize;

	private List<DownloadUrlInfo> mDownloadUrlList;

	private List<DownloadUrlInfo> mFinishUrlList;

	private List<DownloadUrlInfo> mErrorUrlList;

	private MonoBehaviour curMono;

	private int MaxThreadCount = 10;

	private List<DownloadingFileInfo> mCurDownloadingInfo = new List<DownloadingFileInfo>();

	public long AlreadyDownloadSize => mAlreadyDownloadSize;

	private DownloadHelper(string url, bool isRemote, string savePath, OnDownloadFinished onFinished, MonoBehaviour mono, int maxThreadCount)
	{
		NeedDownloadSize = 0f;
		mDownloadUrlList = new List<DownloadUrlInfo>();
		mDownloadUrlList.Add(new DownloadUrlInfo(url, savePath, 0L));
		mFinishUrlList = new List<DownloadUrlInfo>();
		mFinishUrlList.Add(new DownloadUrlInfo(url, savePath, 0L));
		mErrorUrlList = new List<DownloadUrlInfo>();
		mErrorUrlList.Clear();
		mCurDownloadDelegate = onFinished;
		curMono = mono;
		MaxThreadCount = maxThreadCount;
	}

	private DownloadHelper(List<DownloadUrlInfo> urlList, bool isRemote, OnDownloadFinished onFinished, long downloadSize, MonoBehaviour mono, int maxThreadCount)
	{
		mDownloadUrlList = new List<DownloadUrlInfo>(urlList);
		mFinishUrlList = new List<DownloadUrlInfo>(urlList);
		mErrorUrlList = new List<DownloadUrlInfo>();
		mErrorUrlList.Clear();
		mCurDownloadDelegate = onFinished;
		NeedDownloadSize = downloadSize;
		curMono = mono;
		MaxThreadCount = maxThreadCount;
	}

	public static DownloadHelper StartDownload(string url, bool isRemote, string savePath, MonoBehaviour mono, OnDownloadFinished onFinished = null, int maxThreadCount = 5)
	{
		DownloadHelper downloadHelper = new DownloadHelper(url, isRemote, savePath, onFinished, mono, maxThreadCount);
		if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
		{
			mono.StartCoroutine(downloadHelper.DownloadFileList());
		}
		return downloadHelper;
	}

	public static DownloadHelper StartDownload(List<DownloadUrlInfo> urlList, bool isRemote, MonoBehaviour mono, long downloadSize, OnDownloadFinished onFinished = null, int maxThreadCount = 5)
	{
		DownloadHelper downloadHelper = new DownloadHelper(urlList, isRemote, onFinished, downloadSize, mono, maxThreadCount);
		if (UnityVersionUtil.IsactiveInHierarchy(mono.gameObject))
		{
			mono.StartCoroutine(downloadHelper.DownloadFileList());
		}
		return downloadHelper;
	}

	private IEnumerator DownloadFileList()
	{
		if (mDownloadUrlList == null || mDownloadUrlList.Count == 0)
		{
			Log.ERROR_MSG("urlInfoList or urlInfoList.Count == null");
			if (mCurDownloadDelegate != null)
			{
				mCurDownloadDelegate(isSuccess: false);
			}
			yield break;
		}
		for (int j = MaxThreadCount - mCurDownloadingInfo.Count; j > 0; j--)
		{
			DownloadingFileInfo tempInfo = new DownloadingFileInfo();
			mCurDownloadingInfo.Add(tempInfo);
		}
		for (int i = 0; i < mCurDownloadingInfo.Count; i++)
		{
			if (!mCurDownloadingInfo[i].IsDownloading)
			{
				mCurDownloadingInfo[i].SetUrlInfo(mDownloadUrlList[0]);
				mDownloadUrlList.RemoveAt(0);
				if (UnityVersionUtil.IsactiveInHierarchy(curMono.gameObject))
				{
					curMono.StartCoroutine(DownloadFile(mCurDownloadingInfo[i]));
				}
				if (mDownloadUrlList.Count <= 0)
				{
					break;
				}
			}
		}
	}

	private IEnumerator DownloadFile(DownloadingFileInfo info)
	{
		info.IsDownloading = true;
		info.StartDownLoadTime = Time.realtimeSinceStartup;
		info.curWWW = new WWW(info.CurDownloadUrlInfo.Url);
		bool timeOutFLag = false;
		while (!info.curWWW.isDone)
		{
			if (Time.realtimeSinceStartup - info.StartDownLoadTime > info.ExpectDownloadTime)
			{
				timeOutFLag = true;
				break;
			}
			yield return null;
		}
		if (timeOutFLag)
		{
			OnDownLoadFile(info, isSuccess: false);
			yield break;
		}
		if (!string.IsNullOrEmpty(info.curWWW.error))
		{
			Log.ERROR_MSG("Download File Error : " + info.CurDownloadUrlInfo.Url + "  Error : " + info.curWWW.error);
			OnDownLoadFile(info, isSuccess: false);
			yield break;
		}
		try
		{
			MyFileUtil.CheckPath(info.CurDownloadUrlInfo.SavePath);
			MyFileUtil.DeleteFile(info.CurDownloadUrlInfo.SavePath);
			FileStream fs = new FileStream(info.CurDownloadUrlInfo.SavePath, FileMode.OpenOrCreate);
			fs.Write(info.curWWW.bytes, 0, info.curWWW.size);
			fs.Close();
			mAlreadyDownloadSize += info.curWWW.size;
			OnDownLoadFile(info, isSuccess: true);
		}
		catch (Exception ex)
		{
			Log.ERROR_MSG("download file error : " + ex.ToString());
			OnDownLoadFile(info, isSuccess: false);
		}
	}

	public void OnDownLoadFile(DownloadingFileInfo info, bool isSuccess)
	{
		info.IsDownloading = false;
		info.curWWW = null;
		if (!isSuccess)
		{
			mErrorUrlList.Add(info.CurDownloadUrlInfo);
		}
		for (int i = 0; i < mFinishUrlList.Count; i++)
		{
			if (mFinishUrlList[i].Url.Equals(info.CurDownloadUrlInfo.Url))
			{
				mFinishUrlList.RemoveAt(i);
				break;
			}
		}
		if (mFinishUrlList.Count <= 0)
		{
			if (mCurDownloadDelegate != null)
			{
				if (mErrorUrlList.Count <= 0)
				{
					mCurDownloadDelegate(isSuccess: true);
				}
				else
				{
					mCurDownloadDelegate(isSuccess: false);
				}
			}
		}
		else if (mDownloadUrlList.Count > 0)
		{
			info.SetUrlInfo(mDownloadUrlList[0]);
			mDownloadUrlList.RemoveAt(0);
			if (UnityVersionUtil.IsactiveInHierarchy(curMono.gameObject))
			{
				curMono.StartCoroutine(DownloadFile(info));
			}
		}
	}

	public void ContinueDownload()
	{
		if (UnityVersionUtil.IsactiveInHierarchy(curMono.gameObject))
		{
			curMono.StartCoroutine(DownloadFileList());
		}
	}

	public float GetDownloadProgress()
	{
		float num = 0f;
		for (int i = 0; i < mCurDownloadingInfo.Count; i++)
		{
			if (mCurDownloadingInfo[i].IsDownloading && mCurDownloadingInfo[i].curWWW != null)
			{
				num += mCurDownloadingInfo[i].curWWW.progress * (float)mCurDownloadingInfo[i].CurDownloadUrlInfo.size;
			}
		}
		return ((float)mAlreadyDownloadSize + num) / NeedDownloadSize;
	}

	public void DisposeWWW()
	{
		for (int i = 0; i < mCurDownloadingInfo.Count; i++)
		{
			if (mCurDownloadingInfo[i].curWWW != null)
			{
				mCurDownloadingInfo[i].curWWW = null;
			}
		}
	}
}
