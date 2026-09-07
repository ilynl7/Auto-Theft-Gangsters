using UnityEngine;

public class Log
{
	public static DEBUGLEVEL debugLevel;

	public static string getTitle()
	{
		return string.Empty;
	}

	public static void DEBUG_MSG(object s)
	{
		if (DEBUGLEVEL.DEBUG >= debugLevel)
		{
			Debug.Log(getTitle() + s);
		}
	}

	public static void WARING_MSG(object s)
	{
		if (DEBUGLEVEL.WARING >= debugLevel)
		{
			Debug.LogWarning(getTitle() + s);
		}
	}

	public static void ERROR_MSG(object s)
	{
		if (DEBUGLEVEL.ERRO >= debugLevel)
		{
			Debug.LogError(getTitle() + s);
		}
	}
}
