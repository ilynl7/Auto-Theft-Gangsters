public class AnnounceData
{
	public string ID;

	public string ICON;

	public string Name1;

	public string Name2;

	public int StartLevel;

	public int EndLevel;

	public int Type;

	public string ItemId = string.Empty;

	public int quality;

	public string MName1 => StrDictionary.GetDictionaryString(Name1);

	public string MName2 => StrDictionary.GetDictionaryString(Name2);
}
