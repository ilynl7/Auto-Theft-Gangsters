using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class StarRewardPageRoot : SingletonUnity<StarRewardPageRoot>
{
	public UISprite[] TopStars;

	public GameObject[] LineStars;

	public UILabel[] LineLabelList;

	public ShowRewardItems ShowRewardItem;

	public UIGrid LineStarGrid;

	public void ResetCopySceneReward(copy_scene_result.request request)
	{
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		mainPlayer.AnimationLogic.PlayAnimation(GameDefine.GetRoleIdelSelectName((int)mainPlayer.Profession, mainPlayer.GetWeaponType()), null, -1f);
		mainPlayer.CameraController.FinishCopyCameraEffect(1f, delegate
		{
			Singleton<ObjManager>.Instance.MainPlayer.AnimationLogic.PlayAnimation(GameDefine.ShowSelectAnimaName, null, -1f);
		});
		if (request.win)
		{
			ResetCopySceneRewardPage(request);
		}
	}

	private void ResetCopySceneRewardPage(copy_scene_result.request request)
	{
		for (int i = 0; i < TopStars.Length; i++)
		{
			if (i < request.grade)
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
		LineStarGrid.Reposition();
		CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(request.id);
		List<string> list = new List<string>();
		copySceneDataById.GetStarDescriptionList((int)request.gradeFlag, list);
		for (int j = 0; j < list.Count; j++)
		{
			LineLabelList[j].text = StrDictionary.GetDictionaryString(list[j]);
		}
		ShowRewardItem.ShowRewards(request.items);
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("DailyCopy", $"copy_{request.id}", $"star_{request.grade}");
	}

	private void OnEnable()
	{
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

	public void OnClickLeave()
	{
		NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
	}
}
