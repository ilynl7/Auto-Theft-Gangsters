using UnityEngine;

public class GameSettingData
{
	public static bool IsLocalTestServer = false;

	public static int LocalTestServerID = 0;

	public static bool IsTestBilling = false;

	public static bool IsDownLoadInLocal = false;

	public static string UnityVersion = "Unity4.7";

	public static string GameVersion = "1.012.017";

	public static float DefaultCrossInTime = 0.1f;

	public static float DefaultCrossOutTime = 0.1f;

	private static int mPhoneClass = 1;

	public static int[] MaxOtherPlayerVisibleNum = new int[3] { 3, 7, 14 };

	public static int[] MaxOtherPlayerLogicNum = new int[3] { 6, 12, 20 };

	public static int[] MaxOtherPlayerPoolNum = new int[3] { 1, 3, 5 };

	public static int MaxRecentSpeakerNum = 10;

	public static int MinPlayerLevelInWorldSpeak = 0;

	public static int[] MaxNPCPoolGroupNum = new int[3] { 2, 4, 6 };

	public static int[] MaxNPCPoolNum = new int[3] { 3, 6, 6 };

	public static int[] MaxSceneCache = new int[3] { 1, 2, 2 };

	public static bool[] IsBundleNeedUnload = new bool[3] { true, false, false };

	public static bool[] IsShowFashionEffect = new bool[3] { false, true, true };

	public static bool[] IsShowSkillEffect = new bool[3] { true, true, true };

	public static bool[] IsShowPlayerShadow = new bool[3] { false, true, true };

	public static bool[] IsSoundCanPlay = new bool[3] { false, true, true };

	public static bool[] IsCarCopyEffectEnable = new bool[3] { false, true, true };

	public static int[] DownloadThreadCount = new int[3] { 2, 10, 10 };

	public static bool IsShowNormalNpcName = false;

	public static bool IsTutorialClose = false;

	public static bool IsLowPhone = false;

	public static int PlayerLevel = 0;

	public static int PhoneClass
	{
		set
		{
			mPhoneClass = value;
		}
	}

	public static int GetPhoneClass()
	{
		if (mPhoneClass == 0)
		{
			return 0;
		}
		if (IsLowPhone)
		{
			return 0;
		}
		return Mathf.Max(1, PlayerLevel);
	}

	public static int InitGraphic()
	{
		int height = Screen.height;
		int width = Screen.width;
		int systemMemorySize = SystemInfo.systemMemorySize;
		int graphicsMemorySize = SystemInfo.graphicsMemorySize;
		IsLowPhone = false;
		if (height < 480 || width < 800 || systemMemorySize < 512 || graphicsMemorySize <= 100)
		{
			PlayerLevel = 0;
			return 0;
		}
		PlayerLevel = 2;
		if (systemMemorySize < 1000 || graphicsMemorySize < 128)
		{
			PlayerLevel = 1;
		}
		return 1;
	}
}
