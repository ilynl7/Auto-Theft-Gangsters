using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class RewardPageRootLogic : SingletonUnity<RewardPageRootLogic>
{
	public GameObject WinPageRoot;

	public GameObject LoosePageRoot;

	public GameObject RewardPageRoot;

	public GameObject CarRewardPageRoot;

	public GameObject NormalCtlRoot;

	public GameObject TowerCtlRoot;

	public GameObject BottomRoot;

	public UISprite[] TopStars;

	public GameObject[] LineStars;

	public UILabel[] LineLabelList;

	public ShowRewardItems ShowRewardItem;

	public UILabel CurTimeLabel;

	public UILabel PreRankLabel;

	public UILabel CurRankLabel;

	public ShowRewardItems CarShowRewardItem;

	public GameObject CarRankRoot;

	public GameObject CarNotRankRoot;

	public GameObject NewRecordObj;

	public GameObject RankUpObj;

	public void BeforeResultReset()
	{
		UnityVersionUtil.SetActiveRecursive(WinPageRoot, state: false);
		UnityVersionUtil.SetActiveRecursive(RewardPageRoot, state: false);
		UnityVersionUtil.SetActiveRecursive(LoosePageRoot, state: false);
		UnityVersionUtil.SetActiveRecursive(CarRewardPageRoot, state: false);
	}

	public void ResetPVPReward(tiantti_result.request request)
	{
		if (request.win)
		{
			UnityVersionUtil.SetActiveRecursive(WinPageRoot, state: true);
			UnityVersionUtil.SetActiveRecursive(RewardPageRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(LoosePageRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(CarRewardPageRoot, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(WinPageRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(RewardPageRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(LoosePageRoot, state: true);
			UnityVersionUtil.SetActiveRecursive(CarRewardPageRoot, state: false);
		}
	}

	public void ResetCopySceneReward(copy_scene_result.request request)
	{
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		mainPlayer.CameraController.FinishCopyCameraEffect(1f, null);
		if (request.win)
		{
			UnityVersionUtil.SetActiveRecursive(WinPageRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(RewardPageRoot, state: false);
			UnityVersionUtil.SetActiveRecursive(LoosePageRoot, state: false);
			ResetCopySceneRewardPage(request);
			UnityVersionUtil.SetActiveRecursive(CarRewardPageRoot, state: false);
			return;
		}
		UnityVersionUtil.SetActiveRecursive(WinPageRoot, state: false);
		UnityVersionUtil.SetActiveRecursive(RewardPageRoot, state: false);
		UnityVersionUtil.SetActiveRecursive(LoosePageRoot, state: true);
		UnityVersionUtil.SetActiveRecursive(CarRewardPageRoot, state: false);
		if ((int)request.subType == 9)
		{
			vp_Timer.In(2f, delegate
			{
				OnClickTowerLeaveBtn();
			});
		}
	}

	private void ResetCopySceneRewardPage(copy_scene_result.request request)
	{
		UnityVersionUtil.SetActiveRecursive(RewardPageRoot, state: true);
		UnityVersionUtil.SetActiveRecursive(BottomRoot, state: false);
		if ((int)request.subType == 9)
		{
			UnityVersionUtil.SetActiveRecursive(TowerCtlRoot.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(NormalCtlRoot, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(TowerCtlRoot.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(NormalCtlRoot, state: true);
		}
		for (int i = 0; i < TopStars.Length; i++)
		{
			if (i <= request.grade)
			{
				UnityVersionUtil.SetActiveRecursive(TopStars[i].gameObject, state: true);
				UnityVersionUtil.SetActiveRecursive(LineStars[i].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(TopStars[i].gameObject, state: false);
				UnityVersionUtil.SetActiveRecursive(LineStars[i].gameObject, state: false);
			}
		}
		if ((int)request.subType != 9)
		{
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(request.id);
			List<string> list = new List<string>();
			copySceneDataById.GetStarDescriptionList((int)request.gradeFlag, list);
			for (int j = 0; j < list.Count; j++)
			{
				LineLabelList[j].text = list[j];
			}
		}
		ShowRewardItem.ShowRewards(request.items);
	}

	public void ResetCarRewardPageRoot(car_copy_result.request request)
	{
		BeforeResultReset();
		UnityVersionUtil.SetActiveRecursive(CarRewardPageRoot, state: true);
		UnityVersionUtil.SetActiveRecursive(BottomRoot, state: false);
		CurTimeLabel.text = TimeTools.GetCentiSecondStr((int)request.parm);
		NGUITools.SetActive(NewRecordObj, request.new_record == 1);
		Debug.Log("request.rankPos1 :: " + request.rankPos1);
		Debug.Log("request.rankPos2 :: " + request.rankPos2);
		if (request.rankPos1 == -1 || request.rankPos2 == -1)
		{
			NGUITools.SetActive(CarNotRankRoot, state: true);
			NGUITools.SetActive(CarRankRoot, state: false);
		}
		else
		{
			NGUITools.SetActive(CarNotRankRoot, state: false);
			NGUITools.SetActive(CarRankRoot, state: true);
			NGUITools.SetActive(RankUpObj, request.rankPos2 < request.rankPos1);
			PreRankLabel.text = request.rankPos1.ToString();
			CurRankLabel.text = request.rankPos2.ToString();
		}
		CarShowRewardItem.ShowRewards(request.items);
	}

	public void OnClickContinueBtn()
	{
		NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
	}

	public void OnClickRepeatBtn()
	{
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
		enter_copy_scene.request request = new enter_copy_scene.request();
		request.mapInfoId = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData.ID;
		NetLogic.GetInstance().Send<Protocol.enter_copy_scene>(request);
	}

	public void OnClickTowerNextBtn()
	{
		TowerSceneManager towerSceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager as TowerSceneManager;
		towerSceneManager.GoNextLevel();
	}

	public void OnClickTowerLeaveBtn()
	{
		NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
	}
}
