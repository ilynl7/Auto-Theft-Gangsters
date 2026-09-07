using SprotoType;
using UnityEngine;

public class CarRewardPageRootLogic : SingletonUnity<CarRewardPageRootLogic>
{
	public UILabel CurTimeLabel;

	public UILabel PreRankLabel;

	public UILabel CurRankLabel;

	public ShowRewardItems CarShowRewardItem;

	public GameObject CarRankRoot;

	public GameObject CarNotRankRoot;

	public GameObject NewRecordObj;

	public GameObject RankUpObj;

	private string Id;

	public void ResetCarRewardPageRoot(car_copy_result.request request)
	{
		CurTimeLabel.text = TimeTools.GetCentiSecondStr((int)request.parm);
		NGUITools.SetActive(NewRecordObj, request.new_record == 1);
		Id = request.id;
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
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("DailyCopy", $"copy_{Id}", "success");
	}

	public void OnClickContinueBtn()
	{
		NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
	}

	public void OnClickRepeatBtn()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		copyscene_info copyinfoByID = playerData.CopyInfoData.GetCopyinfoByID(Id);
		int num = 0;
		if (copyinfoByID != null)
		{
			num = (int)copyinfoByID.CurNum;
		}
		if (num <= 0)
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{102057}"));
		}
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
		enter_copy_scene.request request = new enter_copy_scene.request();
		request.mapInfoId = Id;
		NetLogic.GetInstance().Send<Protocol.enter_copy_scene>(request);
		playerData.CopyInfoData.DecTimesByID(Id);
	}
}
