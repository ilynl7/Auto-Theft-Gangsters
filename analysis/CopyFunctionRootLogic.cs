public class CopyFunctionRootLogic : SingletonUnity<CopyFunctionRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public TweenScale AutoFightBtn;

	public UISprite AutoBtnSprite;

	public UISprite ExitbtnSprite;

	public UISprite AutoEffect;

	public UILabel MapNameLabel;

	private MAPTYPE curmaptype = MAPTYPE.INVALID;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	public void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	public void Reset(MAPTYPE curMapType, string MapName)
	{
		curmaptype = curMapType;
		MapNameLabel.text = StrDictionary.GetDictionaryString(MapName);
		switch (curMapType)
		{
		case MAPTYPE.CAR_CHASE_COPY:
			NGUITools.SetActive(AutoFightBtn.gameObject, state: false);
			break;
		case MAPTYPE.TUTORIAL_CAR:
			NGUITools.SetActive(AutoFightBtn.gameObject, state: false);
			NGUITools.SetActive(ExitbtnSprite.gameObject, state: false);
			break;
		default:
			UpdateAutoFightBtn();
			break;
		}
	}

	public void OnClickLeaveCopyBtn()
	{
		SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.LeaveScene();
		if (curmaptype != MAPTYPE.INVALID)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("copyleave", "leavecopy", $"maptype_{(int)curmaptype}");
		}
	}

	public void UpdateAutoFightBtn()
	{
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (!(mainPlayer == null))
		{
			if (mainPlayer.IsOpenAutoCombat)
			{
				AutoBtnSprite.spriteName = "CZ_zhuJieMianAnNiu_ZiDong_2";
				AutoEffect.alpha = 1f;
			}
			else
			{
				AutoBtnSprite.spriteName = "CZ_zhuJieMianAnNiu_ZiDong";
				AutoEffect.alpha = 0f;
			}
			AutoFightBtn.ResetToBeginning();
			AutoFightBtn.enabled = mainPlayer.IsOpenAutoCombat;
		}
	}

	public void OnClickAutoFightBtn()
	{
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (!(mainPlayer == null))
		{
			if (TutorialManager.CurStep == TUTORIAL_STEP.AUTO_FIGHT_CLICK)
			{
				CheckTutorialEvent();
			}
			if (!mainPlayer.IsOpenAutoCombat)
			{
				mainPlayer.EnterAutoCombat();
			}
			else
			{
				mainPlayer.LeveAutoCombat();
			}
			UpdateAutoFightBtn();
		}
	}
}
