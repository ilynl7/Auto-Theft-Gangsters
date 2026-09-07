using UnityEngine;

public class DownloadingFileInfo
{
	public bool IsDownloading;

	public DownloadUrlInfo CurDownloadUrlInfo;

	public WWW curWWW;

	public float StartDownLoadTime;

	public float ExpectDownloadTime;

	public static int MIN_DOWNLOAD_SPEED = 5120;

	public static float MIN_DOWNLOAD_TIME = 20f;

	public DownloadingFileInfo()
	{
		IsDownloading = false;
		CurDownloadUrlInfo = null;
		curWWW = null;
	}

	public DownloadingFileInfo(DownloadUrlInfo info)
	{
		IsDownloading = false;
		CurDownloadUrlInfo = info;
		curWWW = null;
		ExpectDownloadTime = Mathf.Max(info.size / MIN_DOWNLOAD_SPEED + 1, MIN_DOWNLOAD_TIME);
	}

	public void SetUrlInfo(DownloadUrlInfo info)
	{
		CurDownloadUrlInfo = info;
		ExpectDownloadTime = Mathf.Max(info.size / MIN_DOWNLOAD_SPEED + 1, MIN_DOWNLOAD_TIME);
	}
}
