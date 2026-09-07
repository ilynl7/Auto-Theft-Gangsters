public class NpcOptionDialogData
{
	public string ID = string.Empty;

	public string CenterDialog = string.Empty;

	public string Option1 = string.Empty;

	public string Option2 = string.Empty;

	public int OptionType = -1;

	public string OptionParam = string.Empty;

	public string OptionFailDialog = string.Empty;

	public string FailOption = string.Empty;

	public string MCenterDialog => StrDictionary.GetDictionaryString(CenterDialog);

	public string MOption1 => StrDictionary.GetDictionaryString(Option1);

	public string MOption2 => StrDictionary.GetDictionaryString(Option2);

	public string MOptionFailDialog => StrDictionary.GetDictionaryString(OptionFailDialog);

	public string MFailOption => StrDictionary.GetDictionaryString(FailOption);

	public OPTION_TYPE Type => (OPTION_TYPE)OptionType;
}
