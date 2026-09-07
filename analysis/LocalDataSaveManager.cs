using System;
using UnityEngine;

public class LocalDataSaveManager
{
	private static string keySystemUseDrag = "keySystemUseDrag";

	private static string keySystemMusic = "SystemMusic";

	private static string keySystemSoundEffect = "SystemSoundEffect";

	private static string musicDrag = "musicVolumeDrag";

	private static string soundDrag = "soundVolumeDrag";

	private static string playerOnScreen = "PlayerOnScreen";

	private static string Graphics = "GraphicsQuality";

	private static string StreetModel = "StreetModel";

	private static string ScreenVibrat = "ScreenVibrating";

	private static string notyfition = "Notification";

	private static string TestLastGameIP = "TestLastGameIP";

	private static string TestLastLoginIP = "TestLastLoginIP";

	private static string TestLastServerName = "TestLastServerName";

	private static string TestIsUseDns = "TestIsUseDns";

	public static bool ShowDailyBuyTips = true;

	public static bool ShowInvestTips = true;

	private static string keyPopFaceBookTime = "keyPopFaceBookTime";

	private static string keyPopFaceBookCount = "keyPopFaceBookCount";

	private static string keyFirstEnterGame = "keyFirstEnterGame";

	private static int RewardFlag;

	public static int IsADFree = -1;

	public static bool AdFree
	{
		get
		{
			if (IsADFree == 1)
			{
				return true;
			}
			return false;
		}
	}

	public static int GetADFreeFlag()
	{
		IsADFree = PlayerPrefs.GetInt("ADFreeFlag", -1);
		return IsADFree;
	}

	public static void SetADFreeFlag(int value)
	{
		if (IsADFree != value)
		{
			IsADFree = value;
			PlayerPrefs.SetInt("ADFreeFlag", value);
		}
	}

	public static bool GetFaceBookPopTips(int type)
	{
		return PlayerPrefs.GetInt(keyPopFaceBookCount + type, 0) <= 4;
	}

	public static bool IsFirstEnterGame()
	{
		bool flag = PlayerPrefs.GetInt(keyFirstEnterGame, 0) == 0;
		if (flag)
		{
			PlayerPrefs.SetInt(keyFirstEnterGame, 1);
		}
		return flag;
	}

	public static void SetFaceBookPopCount(int type)
	{
		int @int = PlayerPrefs.GetInt(keyPopFaceBookCount + type, 0);
		PlayerPrefs.SetInt(keyPopFaceBookCount + type, @int + 1);
	}

	public static void SetFaceBookBindTime()
	{
		DateTime dateTime = new DateTime(1970, 1, 1);
		int value = (int)(DateTime.Now - dateTime).TotalDays;
		PlayerPrefs.SetInt(keyPopFaceBookTime, value);
	}

	public static bool GetFaceBookNextPopDay(int day = 3)
	{
		int @int = PlayerPrefs.GetInt(keyPopFaceBookTime, 0);
		DateTime dateTime = new DateTime(1970, 1, 1);
		int num = (int)(DateTime.Now - dateTime).TotalDays;
		return num - @int >= day;
	}

	public static void SetChooseRoleIndex(int i)
	{
		PlayerPrefs.SetInt("ChooseRoleIndex", i);
	}

	public static int GetChooseRoleIndex()
	{
		return PlayerPrefs.GetInt("ChooseRoleIndex", 0);
	}

	public static bool IsTutorialClose()
	{
		if (PlayerPrefs.GetInt("IsTutorialClose", 0) == 0)
		{
			return false;
		}
		return true;
	}

	public static void SetTutorialCLose(bool isClose)
	{
		if (isClose)
		{
			PlayerPrefs.SetInt("IsTutorialClose", 1);
		}
		else
		{
			PlayerPrefs.SetInt("IsTutorialClose", 0);
		}
	}

	public static void SetCameraViewType(long serverId, CameraController.CAMERAVIEWSTATE type)
	{
		PlayerPrefs.SetInt($"CameraViewType{serverId}", (int)type);
	}

	public static CameraController.CAMERAVIEWSTATE GetCameraViewType(long serverId)
	{
		int num = PlayerPrefs.GetInt($"CameraViewType{serverId}", 1);
		if (num > 1)
		{
			num = 1;
		}
		return (CameraController.CAMERAVIEWSTATE)num;
	}

	public static void SetkeySystemUseDrag(float value)
	{
		PlayerPrefs.SetFloat(keySystemUseDrag, value);
	}

	public static float GetkeySystemUseDrag()
	{
		return PlayerPrefs.GetFloat(keySystemUseDrag, 0.5f);
	}

	public static void SetSystemMusic(int value)
	{
		PlayerPrefs.SetInt(keySystemMusic, value);
	}

	public static int GetSystemMusic()
	{
		return PlayerPrefs.GetInt(keySystemMusic, 1);
	}

	public static void SetSystemSoundEffect(int value)
	{
		PlayerPrefs.SetInt(keySystemSoundEffect, value);
	}

	public static int GetSystemSoundEffect()
	{
		return PlayerPrefs.GetInt(keySystemSoundEffect, 1);
	}

	public static void SetmusicDrag(float value)
	{
		PlayerPrefs.SetFloat(musicDrag, value);
	}

	public static float GetmusicDrag()
	{
		return PlayerPrefs.GetFloat(musicDrag, 1f);
	}

	public static void SetsoundDrag(float value)
	{
		PlayerPrefs.SetFloat(soundDrag, value);
	}

	public static float GetsoundDrag()
	{
		return PlayerPrefs.GetFloat(soundDrag, 1f);
	}

	public static void SetplayerOnScreen(float value)
	{
		PlayerPrefs.SetFloat(playerOnScreen, value);
	}

	public static float GetplayerOnScreen()
	{
		return PlayerPrefs.GetFloat(playerOnScreen, 0.5f);
	}

	public static void SetGraphics(int value)
	{
		PlayerPrefs.SetInt(Graphics, value);
	}

	public static int GetGraphics(int default1)
	{
		return PlayerPrefs.GetInt(Graphics, default1);
	}

	public static void SetStreetModel(int value)
	{
		PlayerPrefs.SetInt(StreetModel, value);
	}

	public static int GetStreetModel()
	{
		return PlayerPrefs.GetInt(StreetModel, 1);
	}

	public static void SetScreenVibrat(int value)
	{
		PlayerPrefs.SetInt(ScreenVibrat, value);
	}

	public static int GetScreenVibrat()
	{
		return PlayerPrefs.GetInt(ScreenVibrat, 1);
	}

	public static void Setnotyfition(int value)
	{
		PlayerPrefs.SetInt(notyfition, value);
	}

	public static int Getnotyfition()
	{
		return PlayerPrefs.GetInt(notyfition, 1);
	}

	public static string GetTestLastLoginIP()
	{
		return PlayerPrefs.GetString(TestLastLoginIP, "ec2-52-91-30-19.compute-1.amazonaws.com");
	}

	public static void SetTestLastLoginIP(string ip)
	{
		PlayerPrefs.SetString(TestLastLoginIP, ip);
	}

	public static string GetTestLastGameIP()
	{
		return PlayerPrefs.GetString(TestLastGameIP, "ec2-52-91-30-19.compute-1.amazonaws.com");
	}

	public static void SetTestLastGameIP(string ip)
	{
		PlayerPrefs.SetString(TestLastGameIP, ip);
	}

	public static string GetTestLastServerName()
	{
		return PlayerPrefs.GetString(TestLastServerName, "Liu");
	}

	public static void SetTestLastServerName(string name)
	{
		PlayerPrefs.SetString(TestLastServerName, name);
	}

	public static int GetIsUseDnsTest()
	{
		return PlayerPrefs.GetInt(TestIsUseDns, 1);
	}

	public static void SetIsUseDnsTest(bool isUseDns)
	{
		PlayerPrefs.SetInt(TestIsUseDns, isUseDns ? 1 : 0);
	}

	public static string GetNoticeVersion()
	{
		return PlayerPrefs.GetString("NoticeVersion", string.Empty);
	}

	public static void SetNoticeVersion(string version)
	{
		PlayerPrefs.SetString("NoticeVersion", version);
	}

	public static int GetShowStrongerFlag()
	{
		return PlayerPrefs.GetInt("StrongerTimes" + PlayerData.MainPlayerServerId, 10);
	}

	public static void SetStrongerFlag(int val)
	{
		PlayerPrefs.SetInt("StrongerTimes" + PlayerData.MainPlayerServerId, val);
	}

	public static int GetDiedFlag()
	{
		return PlayerPrefs.GetInt("diedflag" + PlayerData.MainPlayerServerId, 0);
	}

	public static void SetDiedFlag(int val)
	{
		PlayerPrefs.SetInt("diedflag" + PlayerData.MainPlayerServerId, val);
	}

	public static int GetRateFlag()
	{
		return PlayerPrefs.GetInt("rateflag", 1);
	}

	public static void SetRateFlag(int val)
	{
		PlayerPrefs.SetInt("rateflag", val);
	}

	public static string GetLoadindex()
	{
		return PlayerPrefs.GetString("Loadtextureindex", "1");
	}

	public static void SetLoadindex(string value)
	{
		PlayerPrefs.SetString("Loadtextureindex", value);
	}

	public static string GetTimeOffset()
	{
		return PlayerPrefs.GetString("LocalTimeOffset", "-1");
	}

	public static void SetTimeOffset(long timeoffset)
	{
		PlayerPrefs.SetString("LocalTimeOffset", timeoffset.ToString());
	}

	public static int GetRewardFlag()
	{
		RewardFlag = PlayerPrefs.GetInt("RewardFlag", 0);
		return RewardFlag;
	}

	public static void SetRewardFlag(int val)
	{
		RewardFlag = 0;
		if (((uint)val & (true ? 1u : 0u)) != 0)
		{
			RewardFlag++;
		}
		if (((uint)val & 2u) != 0)
		{
			RewardFlag++;
		}
		PlayerPrefs.SetInt("RewardFlag", RewardFlag);
	}

	public static void SetRewardFlag()
	{
		RewardFlag--;
		if (RewardFlag < 0)
		{
			RewardFlag = 0;
		}
		PlayerPrefs.SetInt("RewardFlag", RewardFlag);
	}

	public static int GetPhoneStateFlag()
	{
		return PlayerPrefs.GetInt("PhoneStateFlag", 0);
	}

	public static void SetPhoneStateFlag()
	{
		PlayerPrefs.SetInt("PhoneStateFlag", 1);
	}

	public static bool GetNeedCountAutoDownload()
	{
		switch (PlayerPrefs.GetInt("NeedCountAutoDownload", 0))
		{
		case 0:
			if (GetPhoneStateFlag() == 1)
			{
				PlayerPrefs.SetInt("NeedCountAutoDownload", 2);
				return false;
			}
			PlayerPrefs.SetInt("NeedCountAutoDownload", 1);
			return true;
		case 1:
			return true;
		default:
			return false;
		}
	}

	public static bool GetAutoDownloadFlag()
	{
		return false;
	}

	public static bool GetTranslationFlag()
	{
		return PlayerPrefs.GetInt("TranslationFlag", GameManager.IsEnglishLanguage() ? 1 : 0) == 1;
	}

	public static void SetTranslationFlag(bool isNeedTranslate)
	{
		if (isNeedTranslate)
		{
			PlayerPrefs.SetInt("TranslationFlag", 1);
		}
		else
		{
			PlayerPrefs.SetInt("TranslationFlag", 0);
		}
	}

	public static void SetLastViewDistance(float scale)
	{
		PlayerPrefs.SetFloat("LastViewDistance", scale);
	}

	public static float GetLastViewDistance()
	{
		return PlayerPrefs.GetFloat("LastViewDistance", 0.7f);
	}

	public static void SetChannelShow(int value)
	{
		PlayerPrefs.SetInt("ChannelSet" + PlayerData.MainPlayerServerId, value);
	}

	public static int GetChannelShow()
	{
		return PlayerPrefs.GetInt("ChannelSet" + PlayerData.MainPlayerServerId, 63);
	}

	public static void SetChatShow(int value)
	{
		PlayerPrefs.SetInt("ChatShow" + PlayerData.MainPlayerServerId, value);
	}

	public static int GetChatShow()
	{
		return PlayerPrefs.GetInt("ChatShow" + PlayerData.MainPlayerServerId, 1);
	}

	public static bool IsNeedCountDownload()
	{
		return PlayerPrefs.GetInt("StartDownloadFlag", 0) == 0;
	}

	public static void SetDownloadCountFinish()
	{
		PlayerPrefs.SetInt("StartDownloadFlag", 1);
	}

	public static bool IsNeedCountDownloadFinish()
	{
		return PlayerPrefs.GetInt("FinishDownloadFlag", 0) == 0;
	}

	public static void SetDownloadFinishCountFinish()
	{
		PlayerPrefs.SetInt("FinishDownloadFlag", 1);
	}

	public static void SetNonMissionTarget(int value)
	{
		PlayerPrefs.SetInt("NonMissionTarget", value);
	}

	public static int GetNonMissionTarget()
	{
		return PlayerPrefs.GetInt("NonMissionTarget", 1);
	}

	public static void SetVehicleTarget(int value)
	{
		PlayerPrefs.SetInt("VehicleTarget", value);
	}

	public static int GetVehicleTarget()
	{
		return PlayerPrefs.GetInt("VehicleTarget", 1);
	}
}
