using System.Collections.Generic;

public class TeamTargetTabData
{
	public string Title;

	public string Key;

	public List<string> SubTitle;

	public List<string> SubKey;

	public void Reset(string title, List<string> subTitle, string key, List<string> subKey)
	{
		Title = title;
		SubTitle = subTitle;
		Key = key;
		SubKey = subKey;
	}
}
