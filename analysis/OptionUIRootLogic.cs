using UnityEngine;

public class OptionUIRootLogic : SingletonUnity<OptionUIRootLogic>
{
	public UILabel IDLabel;

	private PlayerData playerData;

	private ObjMainPlayer mMainPlayer;

	public UISlider DragSlider;

	public UILabel DragLabel;

	public UILabel FaceBookLabel;

	public Transform MusicSelectTra;

	public Transform SoundSelectTra;

	public UISlider MusicSlider;

	public UISlider SoundSlider;

	public UISprite BindSpriteIcon;

	private Vector3 leftPos = new Vector3(-11.5f, 0f, 0f);

	private Vector3 rightPos = new Vector3(12.5f, 0f, 0f);

	public Transform GraphicsSelect;

	public Transform GraphicsHigh;

	public Transform GraphicsLow;

	public Transform StreetSelect;

	public Transform StreetNormal;

	public Transform StreetMotion;

	public Transform Scr_VibratSelect;

	public Transform Scr_on;

	public Transform Scr_off;

	public Transform NotifySelect;

	public Transform Notify_on;

	public Transform Notify_off;

	private Vector3 selectPos = new Vector3(14f, 5f, 0f);

	private int CurPage;

	public Transform SelectTra;

	public Transform FeatureTra;

	public Transform BattleTra;

	public Transform ChatTra;

	public GameObject FeaturesObj;

	public GameObject BattleObj;

	public GameObject ChatObj;

	public Transform ChatSystemSelect;

	public Transform ChatSystem_on;

	public Transform ChatSystem_off;

	public Transform ChatWorldSelect;

	public Transform ChatWorld_on;

	public Transform ChatWorld_off;

	public Transform ChatNearbySelect;

	public Transform ChatNearby_on;

	public Transform ChatNearby_off;

	public Transform ChatTeamSelect;

	public Transform ChatTeam_on;

	public Transform ChatTeam_off;

	public Transform ChatGuildSelect;

	public Transform ChatGuild_on;

	public Transform ChatGuild_off;

	public Transform ChatPrivateSelect;

	public Transform ChatPrivate_on;

	public Transform ChatPrivate_off;

	public UILabel LevelInfoLabel;

	public UILabel DanceLabel;

	public UILabel ExpLabel;

	public UISprite NonMissionSelectSp;

	public UISprite VehicleSelectSp;

	private long reamainTime;

	private float tempCountTime;

	public void Reset()
	{
		playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		IDLabel.text = PlayerData.MainPlayerServerId.ToString();
		if (PlayerData.FaceBookBind < 0)
		{
			BindSpriteIcon.spriteName = "CZ_zhuangHaoGuanLian";
			FaceBookLabel.text = StrDictionary.GetDictionaryString("#{200071}");
		}
		else
		{
			FaceBookLabel.text = StrDictionary.GetDictionaryString("#{200072}");
			if (PlayerData.FaceBookBind == 100)
			{
				BindSpriteIcon.spriteName = "CZ_faceBook";
			}
			else
			{
				BindSpriteIcon.spriteName = "CZ_faceBook";
			}
		}
		CurPage = 0;
		OnClickFeaturesBtn();
	}

	public void OnClickFeaturesBtn()
	{
		if (CurPage != 1)
		{
			NGUITools.SetActive(ChatObj, state: false);
			NGUITools.SetActive(BattleObj, state: false);
			NGUITools.SetActive(FeaturesObj, state: true);
			ResetFeatures();
			SelectTra.parent = FeatureTra;
			SelectTra.localPosition = Vector3.zero;
			CurPage = 1;
		}
	}

	public void OnClickBattleBtn()
	{
		if (CurPage != 2)
		{
			NGUITools.SetActive(ChatObj, state: false);
			NGUITools.SetActive(FeaturesObj, state: false);
			NGUITools.SetActive(BattleObj, state: true);
			ResetBattle();
			SelectTra.parent = BattleTra;
			SelectTra.localPosition = Vector3.zero;
			CurPage = 2;
		}
	}

	public void OnClickChatBtn()
	{
		if (CurPage != 3)
		{
			NGUITools.SetActive(BattleObj, state: false);
			NGUITools.SetActive(FeaturesObj, state: false);
			NGUITools.SetActive(ChatObj, state: true);
			ResetChat();
			SelectTra.parent = ChatTra;
			SelectTra.localPosition = Vector3.zero;
			CurPage = 3;
		}
	}

	public void ResetFeatures()
	{
		playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (playerData.SystemMusic == 1)
		{
			MusicSelectTra.localPosition = leftPos;
		}
		else
		{
			MusicSelectTra.localPosition = rightPos;
		}
		if (playerData.SystemSoundEffect == 1)
		{
			SoundSelectTra.localPosition = leftPos;
		}
		else
		{
			SoundSelectTra.localPosition = rightPos;
		}
		MusicSlider.value = playerData.MusicDragValue;
		SoundSlider.value = playerData.SoundDragValue;
		if (playerData.GraphicsQua == 1)
		{
			GraphicsSelect.parent = GraphicsHigh;
			GraphicsSelect.localPosition = selectPos;
		}
		else
		{
			GraphicsSelect.parent = GraphicsLow;
			GraphicsSelect.localPosition = selectPos;
		}
		if (playerData.StreetRacingMode == 1)
		{
			StreetSelect.parent = StreetNormal;
			StreetSelect.localPosition = selectPos;
		}
		else
		{
			StreetSelect.parent = StreetMotion;
			StreetSelect.localPosition = selectPos;
		}
		if (playerData.Screen_Vibrating == 1f)
		{
			Scr_VibratSelect.parent = Scr_on;
			Scr_VibratSelect.localPosition = selectPos;
		}
		else
		{
			Scr_VibratSelect.parent = Scr_off;
			Scr_VibratSelect.localPosition = selectPos;
		}
		if (playerData.Notify == 1f)
		{
			NotifySelect.parent = Notify_on;
			NotifySelect.localPosition = selectPos;
		}
		else
		{
			NotifySelect.parent = Notify_off;
			NotifySelect.localPosition = selectPos;
		}
	}

	private void Update()
	{
		if (CurPage == 2 && reamainTime > 0)
		{
			tempCountTime += Time.deltaTime;
			if (tempCountTime >= 1f)
			{
				reamainTime--;
				tempCountTime -= 1f;
				LevelInfoLabel.text = StrDictionary.GetDictionaryString("#{102126}", TimeTools.GetFullTime(reamainTime)) + string.Format("\n{0}", StrDictionary.GetDictionaryString("#{102127}"));
			}
			if (reamainTime <= 0)
			{
				tempCountTime = 0f;
				ShowLevelsealInfo();
			}
		}
	}

	public void ResetBattle()
	{
		playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		DragSlider.value = playerData.AutoUseDragThreshold / 0.8f;
		string dictionaryString = StrDictionary.GetDictionaryString("#{102127}");
		reamainTime = 0L;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData != null)
		{
			reamainTime = playerCommonData.ServerLevelSealTime - playerCommonData.GetCurServerTime();
		}
		if (reamainTime > 0)
		{
			LevelInfoLabel.text = StrDictionary.GetDictionaryString("#{102126}", TimeTools.GetFullTime(reamainTime)) + $"\n{dictionaryString}";
			DanceLabel.text = string.Format("{0} {1}", StrDictionary.GetDictionaryString("#{102133}"), "0");
			ExpLabel.text = string.Format("{0} {1}", StrDictionary.GetDictionaryString("#{102134}"), "100%");
		}
		else
		{
			ShowLevelsealInfo();
		}
		if (playerData.NonMissionTarget == 1)
		{
			NonMissionSelectSp.enabled = true;
		}
		else
		{
			NonMissionSelectSp.enabled = false;
		}
		if (playerData.VehicleTarget == 1)
		{
			VehicleSelectSp.enabled = true;
		}
		else
		{
			VehicleSelectSp.enabled = false;
		}
	}

	public void ShowLevelsealInfo()
	{
		int serverLevel = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.ServerLevel;
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		string dictionaryString = StrDictionary.GetDictionaryString("#{102127}");
		DanceLabel.text = string.Format("{0} {1}", StrDictionary.GetDictionaryString("#{102133}"), serverLevel);
		if (level > serverLevel)
		{
			LevelInfoLabel.text = StrDictionary.GetDictionaryString("#{102125}", serverLevel, StrDictionary.GetDictionaryString("#{102128}")) + $" {level - serverLevel}" + $"\n{dictionaryString}";
			LevelSealData levelSealDataByID = DataManager.GetLevelSealDataByID((level - serverLevel).ToString());
			if (levelSealDataByID != null)
			{
				ExpLabel.text = string.Format("{0} {1}%", StrDictionary.GetDictionaryString("#{102134}"), levelSealDataByID.Inhibit);
			}
		}
		else if (level == serverLevel)
		{
			LevelInfoLabel.text = StrDictionary.GetDictionaryString("#{102125}", serverLevel, StrDictionary.GetDictionaryString("#{102130}")) + $"\n{dictionaryString}";
			ExpLabel.text = string.Format("{0} {1}", StrDictionary.GetDictionaryString("#{102134}"), "100%");
		}
		else
		{
			LevelInfoLabel.text = StrDictionary.GetDictionaryString("#{102125}", serverLevel, StrDictionary.GetDictionaryString("#{102129}")) + $" {serverLevel - level}" + $"\n{dictionaryString}";
			LevelSealData levelSealDataByID2 = DataManager.GetLevelSealDataByID((serverLevel - level).ToString());
			if (levelSealDataByID2 != null)
			{
				ExpLabel.text = string.Format("{0} {1}%", StrDictionary.GetDictionaryString("#{102134}"), levelSealDataByID2.Encourage);
			}
		}
	}

	public void ResetChat()
	{
		playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		int getChannelValue = playerData.GetChannelValue;
		if (((uint)getChannelValue & (true ? 1u : 0u)) != 0)
		{
			ChatSystemSelect.parent = ChatSystem_on;
			ChatSystemSelect.localPosition = selectPos;
		}
		else
		{
			ChatSystemSelect.parent = ChatSystem_off;
			ChatSystemSelect.localPosition = selectPos;
		}
		if (((uint)getChannelValue & 2u) != 0)
		{
			ChatWorldSelect.parent = ChatWorld_on;
			ChatWorldSelect.localPosition = selectPos;
		}
		else
		{
			ChatWorldSelect.parent = ChatWorld_off;
			ChatWorldSelect.localPosition = selectPos;
		}
		if (((uint)getChannelValue & 4u) != 0)
		{
			ChatNearbySelect.parent = ChatNearby_on;
			ChatNearbySelect.localPosition = selectPos;
		}
		else
		{
			ChatNearbySelect.parent = ChatNearby_off;
			ChatNearbySelect.localPosition = selectPos;
		}
		if (((uint)getChannelValue & 8u) != 0)
		{
			ChatTeamSelect.parent = ChatTeam_on;
			ChatTeamSelect.localPosition = selectPos;
		}
		else
		{
			ChatTeamSelect.parent = ChatTeam_off;
			ChatTeamSelect.localPosition = selectPos;
		}
		if (((uint)getChannelValue & 0x10u) != 0)
		{
			ChatGuildSelect.parent = ChatGuild_on;
			ChatGuildSelect.localPosition = selectPos;
		}
		else
		{
			ChatGuildSelect.parent = ChatGuild_off;
			ChatGuildSelect.localPosition = selectPos;
		}
		if (((uint)getChannelValue & 0x20u) != 0)
		{
			ChatPrivateSelect.parent = ChatPrivate_on;
			ChatPrivateSelect.localPosition = selectPos;
		}
		else
		{
			ChatPrivateSelect.parent = ChatPrivate_off;
			ChatPrivateSelect.localPosition = selectPos;
		}
	}

	public void OnClickSupport()
	{
		if (GameManager.IsSupportCurDataVersion56())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ReportRoot);
		}
	}

	public void OnClickFaceBook()
	{
		if (PlayerData.FaceBookBind > -1)
		{
			MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{200091}"), StrDictionary.GetDictionaryString("#{100127}"), OnClickYes);
		}
		else
		{
			MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{200090}"), StrDictionary.GetDictionaryString("#{100127}"), OnClickYes);
		}
	}

	public void OnClickYes()
	{
		AccountVersionCheckRootLogic.ClickBindFaceBook();
	}

	public void OnClickGraphics_high()
	{
		SetGraphics(1);
	}

	public void OnClickGraphics_low()
	{
		SetGraphics(0);
	}

	public void OnClickStreet_normal()
	{
		SetStreetRacing(1);
	}

	public void OnClickStreet_motion()
	{
		SetStreetRacing(0);
	}

	public void OnClickScreenVibrat_on()
	{
		SetScreenVibrat(1);
	}

	public void OnClickScreenVibrat_off()
	{
		SetScreenVibrat(0);
	}

	public void OnClickNotify_on()
	{
		SetNotify(1);
	}

	public void OnClickNotify_off()
	{
		SetNotify(0);
	}

	private void SetGraphics(int val)
	{
		if (val != playerData.GraphicsQua)
		{
			playerData.SetGraphics(val);
			if (playerData.GraphicsQua == 1)
			{
				GraphicsSelect.parent = GraphicsHigh;
				GraphicsSelect.localPosition = selectPos;
			}
			else
			{
				GraphicsSelect.parent = GraphicsLow;
				GraphicsSelect.localPosition = selectPos;
			}
		}
	}

	private void SetStreetRacing(int value)
	{
		if (value != playerData.StreetRacingMode)
		{
			playerData.SetStreetModel(value);
			if (playerData.StreetRacingMode == 1)
			{
				StreetSelect.parent = StreetNormal;
				StreetSelect.localPosition = selectPos;
			}
			else
			{
				StreetSelect.parent = StreetMotion;
				StreetSelect.localPosition = selectPos;
			}
			if (SingletonUnity<CarControllerRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<CarControllerRootLogic>.Instance.gameObject))
			{
				SingletonUnity<CarControllerRootLogic>.Instance.Reset();
			}
		}
	}

	private void SetScreenVibrat(int value)
	{
		if ((float)value != playerData.Screen_Vibrating)
		{
			playerData.SetScreenVibrat(value);
			if (playerData.Screen_Vibrating == 1f)
			{
				Scr_VibratSelect.parent = Scr_on;
				Scr_VibratSelect.localPosition = selectPos;
			}
			else
			{
				Scr_VibratSelect.parent = Scr_off;
				Scr_VibratSelect.localPosition = selectPos;
			}
		}
	}

	private void SetNotify(int value)
	{
		if ((float)value != playerData.Notify)
		{
			playerData.SetNotify(value);
			if (playerData.Notify == 1f)
			{
				NotifySelect.parent = Notify_on;
				NotifySelect.localPosition = selectPos;
			}
			else
			{
				NotifySelect.parent = Notify_off;
				NotifySelect.localPosition = selectPos;
			}
		}
	}

	public void OnClickMusicBtn()
	{
		if (playerData.SystemMusic == 1)
		{
			playerData.SystemMusic = 0;
			MusicSelectTra.localPosition = rightPos;
			SingletonDontDestoryUnity<SoundManager>.Instance.EnableBGM = false;
		}
		else
		{
			playerData.SystemMusic = 1;
			MusicSelectTra.localPosition = leftPos;
			SingletonDontDestoryUnity<SoundManager>.Instance.EnableBGM = true;
		}
	}

	public void OnClickSoundBtn()
	{
		if (playerData.SystemSoundEffect == 1)
		{
			playerData.SystemSoundEffect = 0;
			SoundSelectTra.localPosition = rightPos;
			SingletonDontDestoryUnity<SoundManager>.Instance.EnableSFX = false;
		}
		else
		{
			playerData.SystemSoundEffect = 1;
			SoundSelectTra.localPosition = leftPos;
			SingletonDontDestoryUnity<SoundManager>.Instance.EnableSFX = true;
		}
	}

	public void DragMusicSlider()
	{
		playerData.SetMusicDragValue(MusicSlider.value);
		SingletonDontDestoryUnity<SoundManager>.Instance.bgmVolume = MusicSlider.value;
	}

	public void DragSoundSlider()
	{
		playerData.SetSoundDragValue(SoundSlider.value);
		SingletonDontDestoryUnity<SoundManager>.Instance.sfxVolume = SoundSlider.value;
	}

	public void UseDragValue()
	{
		DragLabel.text = Mathf.RoundToInt(DragSlider.value * 0.8f * 100f) + "%";
		playerData.SetDragValue(DragSlider.value * 0.8f);
	}

	public void OnClikcSignOut()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.LeaveGame();
		LoadingWindow.LoadScene(0);
	}

	public void OnClickClose()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.OptionUIRootLogic);
	}

	public void OnClickChatSystem_on()
	{
		SetChatSystem(1);
	}

	public void OnClickChatSystem_off()
	{
		SetChatSystem(0);
	}

	private void SetChatSystem(int value)
	{
		int num = playerData.GetChannelValue;
		if (((uint)num & (true ? 1u : 0u)) != 0)
		{
			if (value == 0)
			{
				num--;
				playerData.SetChannel(num);
			}
		}
		else if (value == 1)
		{
			num++;
			playerData.SetChannel(num);
		}
		if (((uint)num & (true ? 1u : 0u)) != 0)
		{
			ChatSystemSelect.parent = ChatSystem_on;
			ChatSystemSelect.localPosition = selectPos;
		}
		else
		{
			ChatSystemSelect.parent = ChatSystem_off;
			ChatSystemSelect.localPosition = selectPos;
		}
	}

	public void OnClickChatWorld_on()
	{
		SetChatWorld(1);
	}

	public void OnClickChatWorld_off()
	{
		SetChatWorld(0);
	}

	private void SetChatWorld(int value)
	{
		int num = playerData.GetChannelValue;
		if (((uint)num & 2u) != 0)
		{
			if (value == 0)
			{
				num -= 2;
				playerData.SetChannel(num);
			}
		}
		else if (value == 1)
		{
			num += 2;
			playerData.SetChannel(num);
		}
		if (((uint)num & 2u) != 0)
		{
			ChatWorldSelect.parent = ChatWorld_on;
			ChatWorldSelect.localPosition = selectPos;
		}
		else
		{
			ChatWorldSelect.parent = ChatWorld_off;
			ChatWorldSelect.localPosition = selectPos;
		}
	}

	public void OnClickChatNearby_on()
	{
		SetChatNearby(1);
	}

	public void OnClickChatNearby_off()
	{
		SetChatNearby(0);
	}

	private void SetChatNearby(int value)
	{
		int num = playerData.GetChannelValue;
		if (((uint)num & 4u) != 0)
		{
			if (value == 0)
			{
				num -= 4;
				playerData.SetChannel(num);
			}
		}
		else if (value == 1)
		{
			num += 4;
			playerData.SetChannel(num);
		}
		if (((uint)num & 4u) != 0)
		{
			ChatNearbySelect.parent = ChatNearby_on;
			ChatNearbySelect.localPosition = selectPos;
		}
		else
		{
			ChatNearbySelect.parent = ChatNearby_off;
			ChatNearbySelect.localPosition = selectPos;
		}
	}

	public void OnClickChatTeam_on()
	{
		SetChatTeam(1);
	}

	public void OnClickChaTeam_off()
	{
		SetChatTeam(0);
	}

	private void SetChatTeam(int value)
	{
		int num = playerData.GetChannelValue;
		if (((uint)num & 8u) != 0)
		{
			if (value == 0)
			{
				num -= 8;
				playerData.SetChannel(num);
			}
		}
		else if (value == 1)
		{
			num += 8;
			playerData.SetChannel(num);
		}
		if (((uint)num & 8u) != 0)
		{
			ChatTeamSelect.parent = ChatTeam_on;
			ChatTeamSelect.localPosition = selectPos;
		}
		else
		{
			ChatTeamSelect.parent = ChatTeam_off;
			ChatTeamSelect.localPosition = selectPos;
		}
	}

	public void OnClickChatGuild_on()
	{
		SetChatGuild(1);
	}

	public void OnClickChaGuild_off()
	{
		SetChatGuild(0);
	}

	private void SetChatGuild(int value)
	{
		int num = playerData.GetChannelValue;
		if (((uint)num & 0x10u) != 0)
		{
			if (value == 0)
			{
				num -= 16;
				playerData.SetChannel(num);
			}
		}
		else if (value == 1)
		{
			num += 16;
			playerData.SetChannel(num);
		}
		if (((uint)num & 0x10u) != 0)
		{
			ChatGuildSelect.parent = ChatGuild_on;
			ChatGuildSelect.localPosition = selectPos;
		}
		else
		{
			ChatGuildSelect.parent = ChatGuild_off;
			ChatGuildSelect.localPosition = selectPos;
		}
	}

	public void OnClickChatPrivate_on()
	{
		SetChatPrivate(1);
	}

	public void OnClickChaPrivate_off()
	{
		SetChatPrivate(0);
	}

	private void SetChatPrivate(int value)
	{
		int num = playerData.GetChannelValue;
		if (((uint)num & 0x20u) != 0)
		{
			if (value == 0)
			{
				num -= 32;
				playerData.SetChannel(num);
			}
		}
		else if (value == 1)
		{
			num += 32;
			playerData.SetChannel(num);
		}
		if (((uint)num & 0x20u) != 0)
		{
			ChatPrivateSelect.parent = ChatPrivate_on;
			ChatPrivateSelect.localPosition = selectPos;
		}
		else
		{
			ChatPrivateSelect.parent = ChatPrivate_off;
			ChatPrivateSelect.localPosition = selectPos;
		}
	}

	public void OnClickNonmissionBtn()
	{
		if (playerData.NonMissionTarget == 1)
		{
			playerData.SetNonMissionTarget(0);
			NonMissionSelectSp.enabled = false;
		}
		else
		{
			playerData.SetNonMissionTarget(1);
			NonMissionSelectSp.enabled = true;
		}
	}

	public void OnClickVehicleBtn()
	{
		if (playerData.VehicleTarget == 1)
		{
			playerData.SetVehicleTarget(0);
			VehicleSelectSp.enabled = false;
		}
		else
		{
			playerData.SetVehicleTarget(1);
			VehicleSelectSp.enabled = true;
		}
	}
}
