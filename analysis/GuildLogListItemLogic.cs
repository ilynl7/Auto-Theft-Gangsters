using UnityEngine;

public class GuildLogListItemLogic : MonoBehaviour
{
	public UILabel LogInfo;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void InitLogINfo(string log)
	{
		LogInfo.text = GetStringInfo(log);
	}

	private string GetStringInfo(string str)
	{
		string[] array = str.Split('_');
		string arg = DateTimeTool.LongToDateTimeLocal(long.Parse(array[2])).ToString();
		return $"{array[0]} {array[1]} this Gang {arg}!";
	}
}
