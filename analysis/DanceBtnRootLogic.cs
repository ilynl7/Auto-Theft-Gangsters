using System;
using SprotoType;
using UnityEngine;

public class DanceBtnRootLogic : SingletonUnity<DanceBtnRootLogic>
{
	public UISprite DanceBtnSprite;

	public UITweener DanceBtnScale;

	public UISprite DanceEffect;

	public GameObject SingleToolsObj;

	public GameObject GangToolsObj;

	public UIGrid DynamicBtnGride;

	private float mLastSendServerTimeCheck;

	private float mSendSerTimeInterval = 5f;

	private PlayerData mPlayerdata;

	private ObjMainPlayer mMainPlayer;

	private SceneManager mCurSceneManager;

	public DanceToolItem SingleToolItem;

	public DanceToolItem GangToolItem;

	private int SingleGameItemNum;

	private int GangGameItemNum;

	public void Reset()
	{
		mPlayerdata = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		mCurSceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (!(mMainPlayer == null))
		{
			if (mMainPlayer.CurPlayerState == PLAYER_STATE.DANCE)
			{
				DanceBtnSprite.spriteName = "CZ_zhuJieMianAnNiu_Dance_2";
				DanceEffect.alpha = 1f;
			}
			else
			{
				DanceBtnSprite.spriteName = "CZ_zhuJieMianAnNiu_Dance_1";
				DanceEffect.alpha = 0f;
			}
			if (CheckDanceLevel())
			{
				DanceBtnScale.ResetToBeginning();
				DanceBtnScale.PlayForward();
				DanceBtnScale.enabled = true;
			}
			SingleGameItemNum = mPlayerdata.ItemBackPack.GetItemStackNumById(GameDefine.SingleDanceToolItem);
			GangGameItemNum = mPlayerdata.ItemBackPack.GetItemStackNumById(GameDefine.GangDanceToolItem);
			SingleToolItem.Init(GameDefine.SingleDanceToolItem, SingleGameItemNum);
			GangToolItem.Init(GameDefine.GangDanceToolItem, GangGameItemNum);
			if (!mPlayerdata.IsHaveGuild())
			{
				NGUITools.SetActive(GangToolItem.gameObject, state: false);
			}
			DynamicBtnGride.Reposition();
			UpdateCDTime();
		}
	}

	public void UpdateCDTime()
	{
		dance_state_info danceInfoByType = mPlayerdata.ActivityData.GetDanceInfoByType(GameDefine.DANCE_TYPE.SINGLE_TOOL);
		if (danceInfoByType != null && danceInfoByType.HasEnd_time)
		{
			CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(danceInfoByType.ID);
			SingleToolItem.SetCDTime(danceInfoByType.end_time, cityDanceDataById.DurationTime);
		}
		else
		{
			SingleToolItem.SetCDTime(0L, 1L);
		}
		dance_state_info danceInfoByType2 = mPlayerdata.ActivityData.GetDanceInfoByType(GameDefine.DANCE_TYPE.GANG_TOOL);
		if (danceInfoByType2 != null && danceInfoByType2.HasParm2 && mMainPlayer.ServerId == danceInfoByType2.parm2 && danceInfoByType2.HasEnd_time)
		{
			CityDanceData cityDanceDataById2 = DataManager.GetCityDanceDataById(danceInfoByType2.ID);
			GangToolItem.SetCDTime(danceInfoByType2.end_time, cityDanceDataById2.DurationTime);
		}
		else
		{
			GangToolItem.SetCDTime(0L, 1L);
		}
	}

	public void OnClickDanceBtn()
	{
		if (Time.time - mLastSendServerTimeCheck < mSendSerTimeInterval)
		{
			return;
		}
		if (!CheckDanceLevel())
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
			return;
		}
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.IsCanDance())
		{
			if (SingleGameItemNum > 0)
			{
				MessageBoxLogic.OpenOKCancelBox("#{104002}", "#{100127}", OnClickSingleToolsBtn);
				return;
			}
			if (GangGameItemNum > 0 && SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
			{
				MessageBoxLogic.OpenOKCancelBox("#{104002}", "#{100127}", OnClickGangToolsBtn);
				return;
			}
			MessageBoxLogic.OpenOKCancelBox("#{104001}", "#{100127}", delegate
			{
				GameMoneyHelper.ShowItemProduct(GameDefine.SingleDanceToolItem);
			});
			return;
		}
		if (mMainPlayer.CurPlayerState != PLAYER_STATE.DANCE)
		{
			PlayerDanceData playerDanceData = mPlayerdata.PlayerDanceData;
			if (playerDanceData.IsHaveDanceData())
			{
				if (SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DanceChooseRoot);
				}
			}
			else
			{
				request_dance_info.request request = new request_dance_info.request();
				request.type = 0L;
				NetLogic.GetInstance().Send<Protocol.request_dance_info>(request);
				WaitResponseUIRootLogic.OpenWaitBox(227, 10f, 0f);
				mLastSendServerTimeCheck = Time.time;
				if (SingletonUnity<FunctionBtnRootLogic>.Exists)
				{
					SingletonUnity<FunctionBtnRootLogic>.Instance.LastSendServerTimeCheck = Time.time;
				}
			}
		}
		else
		{
			mMainPlayer.StopDance();
		}
		UpdateDanceBtn();
	}

	public void OnClickSingleToolsBtn()
	{
		if (SingleToolItem.ReamainTime <= 0)
		{
			if (SingleGameItemNum > 0)
			{
				mMainPlayer.UseDanceItem(GameDefine.SingleDanceToolItem);
			}
			else
			{
				GameMoneyHelper.ShowItemProduct(GameDefine.SingleDanceToolItem);
			}
		}
	}

	public void OnClickGangToolsBtn()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild())
		{
			NoticeLogic.AddNotifyData("#{102006}");
		}
		else if (GangToolItem.ReamainTime <= 0)
		{
			if (GangGameItemNum > 0)
			{
				mMainPlayer.UseDanceItem(GameDefine.GangDanceToolItem);
			}
			else
			{
				NoticeLogic.AddNotifyData("#{105102}");
			}
		}
	}

	public void UpdateDanceBtn()
	{
		if (UnityVersionUtil.IsActive(DanceBtnScale.gameObject) && !(mMainPlayer == null))
		{
			DanceBtnScale.ResetToBeginning();
			if (mMainPlayer.CurPlayerState == PLAYER_STATE.DANCE)
			{
				DanceBtnSprite.spriteName = "CZ_zhuJieMianAnNiu_Dance_2";
				DanceBtnScale.PlayForward();
				DanceBtnScale.enabled = true;
				DanceEffect.alpha = 1f;
			}
			else
			{
				DanceBtnSprite.spriteName = "CZ_zhuJieMianAnNiu_Dance_1";
				DanceEffect.alpha = 0f;
			}
		}
	}

	public bool CheckDanceLevel()
	{
		copyscene_info copyinfoByType = mPlayerdata.CopyInfoData.GetCopyinfoByType(26);
		string text = null;
		if (copyinfoByType != null)
		{
			text = copyinfoByType.ID;
		}
		if (string.IsNullOrEmpty(text))
		{
			return false;
		}
		CityDanceData cityDanceDataById = DataManager.GetCityDanceDataById(text);
		if (!mPlayerdata.CheckLevel(cityDanceDataById.UnlockLevel))
		{
			return false;
		}
		return true;
	}

	private void OnEnable()
	{
		UIUpdateEvent.SyncBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.SyncBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateDanceTools));
		UIUpdateEvent.UpdateBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.UpdateBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateDanceTools));
	}

	private void OnDisable()
	{
		UIUpdateEvent.SyncBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.SyncBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateDanceTools));
		UIUpdateEvent.UpdateBackPackEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.UpdateBackPackEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateDanceTools));
	}

	public void UpdateDanceTools()
	{
		Reset();
	}
}
