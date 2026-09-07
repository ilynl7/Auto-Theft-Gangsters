using SprotoType;
using UnityEngine;

public class CopyMissionShowRootLogic : SingletonUnity<CopyMissionShowRootLogic>
{
	public ShowRewardItems ShowRewardItem;

	public GameObject RankPvPObj;

	public UIGrid BtnGrid;

	public GameObject ContinueObj;

	public UILabel ContentLabel;

	public UILabel TitleLabel;

	public GameObject BestRankObj;

	public GameObject NowRankObj;

	public UILabel BestLabel;

	public UIGrid RankGrid;

	public UILabel CurLabel;

	public UILabel NextLabel;

	public GameObject LevelUpObj;

	private bool isTowerCanContinue;

	private int mWaitTime = 12;

	private int mCurTimeCount;

	private float mStartTime;

	private int tempTime;

	public UILabel ContinueTimelabel;

	public void ResetTowerCopy(copy_scene_result.request request)
	{
		NGUITools.SetActive(RankPvPObj, state: false);
		ShowRewardItem.ShowRewards(request.items);
		mStartTime = Time.time;
		isTowerCanContinue = request.grade > 0;
		TowerSceneManager towerSceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager as TowerSceneManager;
		int currentFloor = towerSceneManager.CurrentFloor;
		TowerData towerDataByFloorID = DataManager.GetTowerDataByFloorID(currentFloor);
		if (towerDataByFloorID != null)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (playerData.CheckLevel(towerDataByFloorID.LimitLevel))
			{
				NGUITools.SetActive(ContinueObj, request.grade > 0);
			}
			else
			{
				NGUITools.SetActive(ContinueObj, state: false);
			}
		}
		else
		{
			NGUITools.SetActive(ContinueObj, state: false);
		}
		mCurTimeCount = mWaitTime;
		ContinueTimelabel.text = $"{mWaitTime}s";
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100753}");
		ContentLabel.text = StrDictionary.GetDictionaryString("#{101579}");
		BtnGrid.Reposition();
	}

	public void ResetRankPvP(tiantti_result.request request)
	{
		ContentLabel.text = string.Empty;
		TitleLabel.text = StrDictionary.GetDictionaryString("#{100753}");
		if (request.HasBestRankPos)
		{
			NGUITools.SetActive(BestRankObj, state: true);
			BestLabel.text = $"{request.bestRankPos}";
		}
		else
		{
			NGUITools.SetActive(BestRankObj, state: false);
		}
		ShowRewardItem.ShowRewards(request.items);
		NGUITools.SetActive(NowRankObj, state: true);
		CurLabel.text = request.rankPos1.ToString();
		NextLabel.text = request.rankPos2.ToString();
		NGUITools.SetActive(LevelUpObj, request.rankPos1 - request.rankPos2 > 0);
		NGUITools.SetActive(ContinueObj, state: false);
		BtnGrid.Reposition();
		RankGrid.Reposition();
		UIManager.SetSpecialType(UIManager.SHOW_TYPE.RANK_PVP);
	}

	public void OnClickLeave()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CopyMissionShowRoot);
		NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
	}

	public void OnClickContinue()
	{
		if (UnityVersionUtil.IsActive(ContinueObj))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CopyMissionShowRoot);
			if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager is TowerSceneManager towerSceneManager)
			{
				towerSceneManager.GoNextLevel();
			}
		}
		else
		{
			OnClickLeave();
		}
	}

	private void Update()
	{
		if (!isTowerCanContinue)
		{
			return;
		}
		tempTime = (int)(Time.time - mStartTime);
		if (mCurTimeCount != mWaitTime - tempTime)
		{
			mCurTimeCount = mWaitTime - tempTime;
			if (mCurTimeCount < 0)
			{
				ContinueTimelabel.text = string.Empty;
				isTowerCanContinue = false;
				OnClickContinue();
			}
			else
			{
				ContinueTimelabel.text = $"{mCurTimeCount}s";
			}
		}
	}

	private void OnEnable()
	{
		isTowerCanContinue = false;
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.IsTalking = true;
		}
		SingletonUnity<UIManager>.Instance.CloseOtherPlayerUI();
	}

	private void OnDisable()
	{
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.IsTalking = false;
		}
	}
}
