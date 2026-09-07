public class DownloadUrlInfo
{
	public string Url;

	public string SavePath;

	public long size;

	public DownloadUrlInfo(string dUrl, string sPath, long sSize)
	{
		Url = dUrl;
		SavePath = sPath;
		size = sSize;
	}
}
