using SprotoType;
using UnityEngine;

public class TouXiangKuangLogic : SingletonUnity<TouXiangKuangLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UILabel HpValueLable;

	public UILabel NameLabel;

	public UISprite HpLinePic;

	public UISprite HpBottomPic;

	public UISprite PlayerIcon;

	public UILabel LvLable;

	public GameObject PvpRoot;

	public UISprite PvpBtnPic;

	public UISprite PveBtnPic;

	public UILabel PvpLabel;

	public UILabel PveLabel;

	public UILabel CurStateLabel;

	public UIWidget StateListObj;

	private bool isOpenState;

	private bool isLockPVP;

	private float mSliderProgress = 1f;

	private int preLevel = -1;

	private int HPLineLength;

	private int mCurCombolValue;

	private PlayerData mPlayerData;

	private static float LastChangeTime;

	private float StateCDTime = 60f;

	public UISprite CDFlag;

	private bool Iscdflag;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	public void Init()
	{
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (mPlayerData == null)
		{
			return;
		}
		PlayerIcon.spriteName = GameDefine.Game_Player_Icon_pic[(int)mPlayerData.Profession];
		PlayerIcon.MakePixelPerfect();
		NameLabel.text = string.Format("{0} {1}", StrDictionary.GetDictionaryString("#{100421}"), mPlayerData.MainPlayerAttrData.ComboValue);
		mCurCombolValue = mPlayerData.MainPlayerAttrData.ComboValue;
		ChangeLevel(mPlayerData.Level);
		HPLineLength = HpBottomPic.width;
		SetHPLine(1f);
		long hP = mPlayerData.MainPlayerAttrData.HP;
		long maxHP = mPlayerData.MainPlayerAttrData.MaxHP;
		ChangeHP(hP, maxHP);
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.MapType == MAPTYPE.TUTORIAL_CAR || string.IsNullOrEmpty(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.SaftyAreaId))
		{
			NGUITools.SetActive(PvpRoot, state: false);
		}
		else
		{
			NGUITools.SetActive(PvpRoot, state: true);
			SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			if (sceneManager.CurrentMapInofData.TargetPKMode == 2)
			{
				if (mPlayerData.IsHaveGuild())
				{
					if (mPlayerData.PlayerPkMode != 2)
					{
						request_change_pk_mode.request request = new request_change_pk_mode.request();
						request.pk = 2L;
						NetLogic.GetInstance().Send<Protocol.request_change_pk_mode>(request);
						mPlayerData.SetPKModeState(2);
					}
				}
				else if (mPlayerData.PlayerPkMode != 1)
				{
					request_change_pk_mode.request request2 = new request_change_pk_mode.request();
					request2.pk = 1L;
					NetLogic.GetInstance().Send<Protocol.request_change_pk_mode>(request2);
					mPlayerData.SetPKModeState(1);
				}
			}
			else if (mPlayerData.PlayerPkMode == 0)
			{
				request_change_pk_mode.request request3 = new request_change_pk_mode.request();
				request3.pk = 1L;
				NetLogic.GetInstance().Send<Protocol.request_change_pk_mode>(request3);
				mPlayerData.SetPKModeState(1);
			}
			UpdateStateBtn();
			isLockPVP = sceneManager.CurrentMapInofData.IsLockPVP == 1;
			if (!CanShowPvpBtnTutorial())
			{
			}
		}
		if (GameManager.IsSupportCurDataVersion177())
		{
			ConfigData configDataByKey = DataManager.GetConfigDataByKey("pkModeCDTime");
			if (configDataByKey != null)
			{
				StateCDTime = configDataByKey.Valuef;
			}
			else
			{
				StateCDTime = 60f;
			}
		}
		else
		{
			StateCDTime = 60f;
		}
		if (LastChangeTime > 0f && Time.time - LastChangeTime < StateCDTime && Time.time - LastChangeTime >= 0f)
		{
			Iscdflag = true;
			CDFlag.fillAmount = 1f - (Time.time - LastChangeTime) / StateCDTime;
		}
		else
		{
			Iscdflag = false;
			CDFlag.fillAmount = 0f;
		}
	}

	public void ChangeCarIcon(long curHp, long curMaxHp)
	{
		PlayerIcon.spriteName = "CZ_cheTouXiang";
		PlayerIcon.MakePixelPerfect();
		ChangeHP(curHp, curMaxHp);
	}

	public bool CanShowPvpBtnTutorial()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.MapType == MAPTYPE.TUTORIAL_CAR || string.IsNullOrEmpty(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.SaftyAreaId))
		{
			return false;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsTutorialCanShow(FUNCTION_TYPE.PVP_BTN_TUTORIAL_TIP) && !SingletonUnity<UIManager>.Instance.IsHideBaseUI && !SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.LoadingFlag)
		{
			return true;
		}
		return false;
	}

	public void ChangeHP(long nCurHp, long nMaxHp)
	{
		HpValueLable.text = $"{nCurHp}/{nMaxHp}";
		mSliderProgress = (float)nCurHp / (float)nMaxHp;
		if (mSliderProgress < 0.2f)
		{
			RedScreenRootLogic.EnableRedScreen();
		}
		else
		{
			RedScreenRootLogic.DisableRedScreen();
		}
	}

	public void ChangeLevel(int level)
	{
		if (preLevel != level)
		{
			preLevel = level;
			LvLable.text = $"Lv.{level}";
		}
	}

	private void SetHPLine(float t)
	{
		HpLinePic.width = (int)(t * (float)HPLineLength);
	}

	private void UpdateHPValue()
	{
		float num = (float)HpLinePic.width / (float)HPLineLength;
		if (num > mSliderProgress)
		{
			num -= Time.deltaTime * 2f;
			if (num < mSliderProgress)
			{
				num = mSliderProgress;
			}
			SetHPLine(num);
		}
		else if (num < mSliderProgress)
		{
			num += Time.deltaTime * 2f;
			if (num > mSliderProgress)
			{
				num = mSliderProgress;
			}
			SetHPLine(num);
		}
		if (mPlayerData.MainPlayerAttrData.ComboValue != mCurCombolValue)
		{
			NameLabel.text = string.Format("{0} {1}", StrDictionary.GetDictionaryString("#{100421}"), mPlayerData.MainPlayerAttrData.ComboValue);
			mCurCombolValue = mPlayerData.MainPlayerAttrData.ComboValue;
		}
	}

	private void Start()
	{
		Init();
	}

	private void Update()
	{
		UpdateHPValue();
		UpdatePkModeState();
	}

	private void UpdatePkModeState()
	{
		if (Iscdflag)
		{
			if (LastChangeTime > 0f && Time.time - LastChangeTime < StateCDTime && Time.time - LastChangeTime >= 0f)
			{
				CDFlag.fillAmount = 1f - (Time.time - LastChangeTime) / StateCDTime;
				return;
			}
			Iscdflag = false;
			CDFlag.fillAmount = 0f;
		}
	}

	public void OnClickIcon()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsBigWorld() && !SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsTutorialScene())
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OptionUIRootLogic, delegate(bool bSuccess, object param)
		{
			if (bSuccess)
			{
				SingletonUnity<OptionUIRootLogic>.Instance.Reset();
			}
		});
	}

	public void OnClickStateBtn()
	{
		if (isLockPVP)
		{
			return;
		}
		if (isOpenState)
		{
			StateListObj.alpha = 0f;
			NGUITools.SetActive(StateListObj.gameObject, state: false);
		}
		else
		{
			if (Iscdflag)
			{
				return;
			}
			StateListObj.alpha = 1f;
			NGUITools.SetActive(StateListObj.gameObject, state: true);
		}
		isOpenState = !isOpenState;
	}

	public void OnClickPVPBtn()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerPkMode != 1)
		{
			request_change_pk_mode.request request = new request_change_pk_mode.request();
			request.pk = 1L;
			NetLogic.GetInstance().Send<Protocol.request_change_pk_mode>(request);
			playerData.SetPKModeState(1);
			CurStateLabel.text = StrDictionary.GetDictionaryString("#{100125}");
			LastChangeTime = Time.time;
			Iscdflag = true;
		}
		if (isOpenState)
		{
			OnClickStateBtn();
		}
	}

	public void OnClickPVEBtn()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerPkMode != 0)
		{
			request_change_pk_mode.request request = new request_change_pk_mode.request();
			request.pk = 0L;
			NetLogic.GetInstance().Send<Protocol.request_change_pk_mode>(request);
			playerData.SetPKModeState(0);
			CurStateLabel.text = StrDictionary.GetDictionaryString("#{100124}");
			LastChangeTime = Time.time;
			Iscdflag = true;
		}
		if (isOpenState)
		{
			OnClickStateBtn();
		}
	}

	public void OnClickGangBtn()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveGuild() && playerData.PlayerPkMode != 2)
		{
			request_change_pk_mode.request request = new request_change_pk_mode.request();
			request.pk = 2L;
			NetLogic.GetInstance().Send<Protocol.request_change_pk_mode>(request);
			playerData.SetPKModeState(2);
			CurStateLabel.text = StrDictionary.GetDictionaryString("#{100114}");
			LastChangeTime = Time.time;
			Iscdflag = true;
		}
		if (isOpenState)
		{
			OnClickStateBtn();
		}
	}

	public void UpdateStateBtn()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerPkMode == 0)
		{
			CurStateLabel.text = StrDictionary.GetDictionaryString("#{100124}");
		}
		else if (playerData.PlayerPkMode == 1)
		{
			CurStateLabel.text = StrDictionary.GetDictionaryString("#{100125}");
		}
		else if (playerData.PlayerPkMode == 2)
		{
			CurStateLabel.text = StrDictionary.GetDictionaryString("#{100114}");
		}
	}
}
