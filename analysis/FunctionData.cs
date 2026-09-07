public class FunctionData
{
	public string ID;

	public string Name;

	public int Class;

	public int Condition;

	public int IsDownload;

	public string CloseTips;

	public int Tips;

	public int FirstOpen;

	public int UnlockType = -1;

	public string SideMissionId = string.Empty;

	public int Index;

	private string[] mSideMissionIdList;

	public string MName => StrDictionary.GetDictionaryString(Name);

	public string[] SideMissionIdList
	{
		get
		{
			if (mSideMissionIdList == null && !string.IsNullOrEmpty(SideMissionId))
			{
				mSideMissionIdList = SideMissionId.Split(';');
			}
			return mSideMissionIdList;
		}
	}
}
