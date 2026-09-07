public class UpdateFileInfo
{
	public string md5;

	public long size;

	public UpdateFileInfo(string fileMd5, long fileSize)
	{
		md5 = fileMd5;
		size = fileSize;
	}

	public void CopyData(UpdateFileInfo newFileInfo)
	{
		md5 = newFileInfo.md5;
		size = newFileInfo.size;
	}
}
