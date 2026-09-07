using SprotoType;
using UnityEngine;

public class RankPVPUIRootLogic : SingletonUnity<RankPVPUIRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UILabel PlayerNameLabel;

	public UILabel PlayerLevelLabel;

	public UILabel PlayerComboValueLabel;

	public UILabel PlayerRankLabel;

	public UILabel PlayerBestRankLabel;

	public UILabel RestFightNumLabel;

	public UISprite PlayerIconSprite;

	private long[] OtherPlayerIndex = new long[3];

	public GameObject[] FightBtnList;

	public RankPVPPlayerLogic[] PVPPlayers;

	private int mInitFakeObjNum;

	private int mInitFakeObjCount;

	private bool mEnableReChooseBtnFlag;

	private long tempChaId;

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

	private void OnEnable()
	{
		ResetPlayerInfo();
	}

	private void OnDisable()
	{
		OnClickBackBtn();
		if (TutorialManager.CurStep == TUTORIAL_STEP.RANK_PVP_CHOOSE)
		{
			FunctionTipsRootLogic.ClearHandTip();
		}
	}

	public void EnableReset()
	{
		for (int i = 0; i < PVPPlayers.Length; i++)
		{
			UnityVersionUtil.SetActiveRecursive(PVPPlayers[i].gameObject, state: false);
		}
	}

	public void ResetOtherPlayer(ret_request_random_rank_pvp_opponent.request request)
	{
		mInitFakeObjNum = PVPPlayers.Length;
		mInitFakeObjCount = 0;
		for (int i = 0; i < PVPPlayers.Length; i++)
		{
			if (request.HasCharacters && request.HasRankPos)
			{
				if (i < request.characters.Count && i < request.rankPos.Count)
				{
					UnityVersionUtil.SetActiveRecursive(PVPPlayers[i].gameObject, state: true);
					PVPPlayers[i].UpdateInfo(request.characters[i], (int)request.rankPos[i], OnClickFightBtn, OnInitFakeObjDone);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(PVPPlayers[i].gameObject, state: false);
				}
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(PVPPlayers[i].gameObject, state: false);
			}
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.RANK_PVP_WAIT_DATA)
		{
			CheckTutorialEvent();
		}
	}

	private void OnInitFakeObjDone()
	{
		mInitFakeObjCount++;
		if (mInitFakeObjCount >= mInitFakeObjNum)
		{
			mEnableReChooseBtnFlag = true;
		}
	}

	public void ResetPlayerInfo()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		PlayerIconSprite.spriteName = GameDefine.Game_Player_Icon_pic[(int)playerData.Profession];
		PlayerNameLabel.text = playerData.MainPlayerAttrData.Name;
		PlayerLevelLabel.text = $"Lv.{playerData.MainPlayerAttrData.Level}";
		PlayerComboValueLabel.text = playerData.MainPlayerAttrData.ComboValue.ToString();
		PlayerRankLabel.text = playerData.RankPVPData.GetRankPosStr();
		PlayerBestRankLabel.text = playerData.RankPVPData.GetBestRankPosStr();
		RestFightNumLabel.text = string.Format("{0}:{1}/{2}", StrDictionary.GetDictionaryString("#{101007}"), playerData.RankPVPData.RankTimes, 10);
	}

	public void OnClickFightBtn(long fightid)
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.RANK_PVP_CHOOSE)
		{
			CheckTutorialEvent();
		}
		SelectFightPerson(fightid);
	}

	private void SelectFightPerson(long characterId)
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.RankPVPData.RankTimes > 0)
		{
			WaitResponseUIRootLogic.OpenWaitBox(135, 10f, 0f);
			select_pk_character.request request = new select_pk_character.request();
			request.characterId = characterId;
			NetLogic.GetInstance().Send<Protocol.select_pk_character>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.RankPVPData.SyncTimesPVPlocal();
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("PVP", "PVP", "pvptimes");
		}
		else
		{
			tempChaId = characterId;
			MessageBoxLogic.OpenOKCancelBox("#{101020}", "#{100127}", OnClickOKBtn);
		}
	}

	private void OnClickOKBtn()
	{
		select_pk_character.request request = new select_pk_character.request();
		request.characterId = tempChaId;
		NetLogic.GetInstance().Send<Protocol.select_pk_character>(request);
	}

	public void OnClickChangePersonBtn()
	{
		if (mEnableReChooseBtnFlag)
		{
			mEnableReChooseBtnFlag = false;
			WaitResponseUIRootLogic.OpenWaitBox(133, 10f, 0f);
			request_random_rank_pvp_opponent.request rpcReq = new request_random_rank_pvp_opponent.request();
			NetLogic.GetInstance().Send<Protocol.request_random_rank_pvp_opponent>(rpcReq);
		}
	}

	public void OnClickRewardBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.RankPvpShowRewardRoot, delegate
		{
			SingletonUnity<RankPVPRewardRootLogic>.Instance.Reset();
		});
	}

	public void OnClickRecordBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PVPLogUIRoot, delegate
		{
			SingletonUnity<PVPLogUIRootLogic>.Instance.Reset("#{101005}");
		});
	}

	public void OnClickRankListBtn()
	{
		OnClickBackBtn();
		SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickCloseBtn();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerRankInfoRoot, delegate
		{
			SingletonUnity<PlayerRankInfoRootLogic>.Instance.ResetToPVP();
		});
	}

	public void OnClickBackBtn()
	{
		for (int i = 0; i < PVPPlayers.Length; i++)
		{
			if (PVPPlayers[i] != null)
			{
				UnityVersionUtil.SetActiveRecursive(PVPPlayers[i].gameObject, state: false);
			}
		}
	}

	public void OnClickTishiBtn()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
		{
			PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", "#{101622}", null, TimeTools.GetLocalShowTime_HM(playerCommonData.ResetTime, playerCommonData.TimeOffset));
		});
	}
}
