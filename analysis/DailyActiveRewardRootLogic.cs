using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DailyActiveRewardRootLogic : SingletonUnity<DailyActiveRewardRootLogic>
{
	public UIWrapContentNew uiWrapContent;

	private int lineMinCount = 6;

	public UIWidget WrapContentBottomWidget;

	public UIScrollView uiScrollView;

	public List<DailyActiveLineLogic> LineItemsList;

	public List<DailyActiveRewardItem> rewardItemsList;

	public GameObject rewardObj;

	public UIGrid RewardGrid;

	public UISlider ActiveSlider;

	public UILabel ActiveLabel;

	private int MaxActive = 100;

	private List<daily_active> activeList;

	private List<daily_reward> rewardList;

	private List<DailyActiveRewardData> RewardDataList = new List<DailyActiveRewardData>();

	private int curScore;

	private float[] sliderValue = new float[4] { 0.175f, 0.45f, 0.7f, 1f };

	protected override void Awake()
	{
		base.Awake();
		uiWrapContent.enabled = false;
		UIWrapContentNew uIWrapContentNew = uiWrapContent;
		uIWrapContentNew.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(uIWrapContentNew.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeItem));
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DailyActiveRewardRoot);
	}

	public void EnableReset()
	{
		for (int i = 0; i < LineItemsList.Count; i++)
		{
			UnityVersionUtil.SetActiveRecursive(LineItemsList[i].gameObject, state: false);
		}
		UnityVersionUtil.SetActiveRecursive(rewardObj, state: false);
	}

	public void Reset(ret_request_daily_active.request request)
	{
		if (request.HasDaily_actives)
		{
			activeList = new List<daily_active>(request.daily_actives.Values);
		}
		activeList.Sort((daily_active x, daily_active y) => (x.ID.Length == y.ID.Length) ? x.ID.CompareTo(y.ID) : (x.ID.Length - y.ID.Length));
		activeList = CheckActivityList(activeList);
		if (request.HasDaily_rewards)
		{
			rewardList = new List<daily_reward>(request.daily_rewards.Values);
		}
		rewardList.Sort((daily_reward x, daily_reward y) => (x.ID.Length == y.ID.Length) ? x.ID.CompareTo(y.ID) : (x.ID.Length - y.ID.Length));
		RewardDataList.Clear();
		for (int i = 0; i < rewardList.Count; i++)
		{
			DailyActiveRewardData dailyActiveRewardDataById = DataManager.GetDailyActiveRewardDataById(rewardList[i].ID);
			if (dailyActiveRewardDataById != null)
			{
				RewardDataList.Add(dailyActiveRewardDataById);
			}
		}
		if (RewardDataList.Count > 0)
		{
			MaxActive = RewardDataList[RewardDataList.Count - 1].Score;
		}
		curScore = (int)request.score;
		int num = Mathf.Min(activeList.Count, lineMinCount) - LineItemsList.Count;
		if (num > 0)
		{
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(LineItemsList[0].gameObject) as GameObject;
				DailyActiveLineLogic component = gameObject.GetComponent<DailyActiveLineLogic>();
				gameObject.name = $"line{LineItemsList.Count:D2}";
				gameObject.transform.parent = uiWrapContent.transform;
				gameObject.transform.localScale = Vector3.one;
				LineItemsList.Add(component);
			}
		}
		for (int k = 0; k < LineItemsList.Count; k++)
		{
			if (k < activeList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(LineItemsList[k].gameObject, state: true);
				LineItemsList[k].UpdateInfo(activeList[k]);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(LineItemsList[k].gameObject, state: false);
			}
		}
		uiWrapContent.minIndex = 1 - activeList.Count;
		WrapContentBottomWidget.height = activeList.Count * uiWrapContent.itemSize;
		uiWrapContent.SortBasedOnScrollMovement();
		uiScrollView.ResetPosition();
		uiWrapContent.enabled = true;
		UnityVersionUtil.SetActiveRecursive(rewardObj, state: true);
		num = rewardList.Count - rewardItemsList.Count;
		if (num > 0)
		{
			for (int l = 0; l < num; l++)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate(rewardItemsList[0].gameObject) as GameObject;
				DailyActiveRewardItem component2 = gameObject2.GetComponent<DailyActiveRewardItem>();
				gameObject2.name = $"reward{rewardItemsList.Count:D2}";
				gameObject2.transform.parent = RewardGrid.transform;
				gameObject2.transform.localScale = Vector3.one;
				rewardItemsList.Add(component2);
			}
		}
		for (int m = 0; m < rewardItemsList.Count; m++)
		{
			if (m < rewardList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(rewardItemsList[m].gameObject, state: true);
				rewardItemsList[m].UpdateInfo(rewardList[m]);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(rewardItemsList[m].gameObject, state: false);
			}
		}
		RewardGrid.Reposition();
		SetActiveSlider((int)request.score);
		ActiveLabel.text = $"{(int)request.score}/{MaxActive}";
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "DailyActive", "open");
	}

	private void SetActiveSlider(int score)
	{
		int num = -1;
		for (int i = 0; i < RewardDataList.Count; i++)
		{
			if (score <= RewardDataList[i].Score)
			{
				num = i;
				break;
			}
		}
		if (score > MaxActive)
		{
			ActiveSlider.value = 1f;
		}
		else if (num > 0)
		{
			ActiveSlider.value = sliderValue[num - 1] + (sliderValue[num] - sliderValue[num - 1]) * ((float)(score - RewardDataList[num - 1].Score) / (float)(RewardDataList[num].Score - RewardDataList[num - 1].Score));
		}
		else if (RewardDataList.Count > 0)
		{
			ActiveSlider.value = sliderValue[0] * ((float)score / (float)RewardDataList[0].Score);
		}
		else
		{
			ActiveSlider.value = 0f;
		}
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		DailyActiveLineLogic itemLogic = LineItemsList[index];
		ResetItemLine(itemLogic, Mathf.Abs(realIndex));
	}

	private void ResetItemLine(DailyActiveLineLogic itemLogic, int idx)
	{
		if (idx < activeList.Count)
		{
			itemLogic.UpdateInfo(activeList[idx]);
		}
	}

	public void UpdateInfo(string id)
	{
		for (int i = 0; i < rewardList.Count; i++)
		{
			if (rewardList[i].ID.Equals(id))
			{
				rewardList[i].state = 2L;
			}
		}
		for (int j = 0; j < rewardItemsList.Count; j++)
		{
			rewardItemsList[j].UpdateInfo(rewardList[j]);
		}
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "DailyActive", $"require_{id}");
	}

	public List<daily_active> CheckActivityList(List<daily_active> list)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		for (int num = list.Count - 1; num >= 0; num--)
		{
			DailyActiveData dailyActiveDataById = DataManager.GetDailyActiveDataById(list[num].ID);
			if (dailyActiveDataById.Type == 9)
			{
				RefineData[] array = new RefineData[5];
				int num2 = 0;
				for (int i = 1; i < 5; i++)
				{
					array[i] = DataManager.GetRefineDataByPartLevelPRO(i, playerData.MainPlayerAttrData.GetTargetRefinePartLevel((REFINE_PART)i), (int)playerData.Profession);
					num2 += array[i].Lv;
				}
				if (num2 >= 40)
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 10)
			{
				ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
				int num3 = 0;
				for (int j = 0; j < mainPlayer.CharacterSkillData.Count; j++)
				{
					CharacterSkillData characterSkillData = mainPlayer.CharacterSkillData[j];
					if (characterSkillData == null || !SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(characterSkillData.UnlockLevel))
					{
						continue;
					}
					int index = characterSkillData.Index;
					if (index > 3)
					{
						SkillData skillDataById = DataManager.GetSkillDataById(characterSkillData.ID);
						if (skillDataById != null && skillDataById.IsUpgrade == 1)
						{
							num3 += characterSkillData.Level + 1;
						}
					}
				}
				if (num3 >= 400)
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 0)
			{
				CopySceneData copySceneDataById = DataManager.GetCopySceneDataById("901");
				if (!playerData.CheckLevel(copySceneDataById.MinLevel))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 1)
			{
				CopySceneData copySceneDataById2 = DataManager.GetCopySceneDataById("223");
				if (copySceneDataById2 == null)
				{
					list.RemoveAt(num);
				}
				if (!playerData.CheckLevel(copySceneDataById2.MinLevel))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 2)
			{
				CopySceneData copySceneDataById3 = DataManager.GetCopySceneDataById("201");
				if (!playerData.CheckLevel(copySceneDataById3.MinLevel))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 3)
			{
				CopySceneData copySceneDataById4 = DataManager.GetCopySceneDataById("211");
				if (!playerData.CheckLevel(copySceneDataById4.MinLevel))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 4)
			{
				EscortData escortDataById = DataManager.GetEscortDataById("30002");
				if (!playerData.CheckLevel(escortDataById.UnlockLevel))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 5)
			{
				if (!CheckUnlockFunction(FUNCTION_TYPE.ACTIVITY_CHALLENGE))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 6)
			{
				WildBossData wildBossDataByID = DataManager.GetWildBossDataByID("801");
				if (!playerData.CheckLevel(wildBossDataByID.LevelMin))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 7)
			{
				if (!CheckUnlockFunction(FUNCTION_TYPE.RANK_PVP))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 8)
			{
				if (!CheckUnlockFunction(FUNCTION_TYPE.ENHANCE_EQUIP))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 9)
			{
				if (!CheckUnlockFunction(FUNCTION_TYPE.ENHANCE_STAR))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 10)
			{
				if (!CheckUnlockFunction(FUNCTION_TYPE.SKILL))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 11)
			{
				if (!CheckUnlockFunction(FUNCTION_TYPE.SHOP_EQUIP))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 12)
			{
				if (!CheckUnlockFunction(FUNCTION_TYPE.SHOP_TOOL))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 13)
			{
				if (!CheckUnlockFunction(FUNCTION_TYPE.GIFT_DAILY))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 14)
			{
				EscortData escortDataById2 = DataManager.GetEscortDataById("30001");
				if (!playerData.CheckLevel(escortDataById2.UnlockLevel))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 15)
			{
				if (!CheckUnlockFunction(FUNCTION_TYPE.DOMIN))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 16)
			{
				CopySceneData copySceneDataById5 = DataManager.GetCopySceneDataById("1201");
				if (!playerData.CheckLevel(copySceneDataById5.MinLevel))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 18)
			{
				SurviveBattleData surviveBattleDataById = DataManager.GetSurviveBattleDataById("1101");
				if (!playerData.CheckLevel(surviveBattleDataById.UnlockLevel))
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 19)
			{
				if (!playerData.IsHaveGuild())
				{
					list.RemoveAt(num);
				}
			}
			else if (dailyActiveDataById.Type == 20 && !playerData.IsHaveGuild())
			{
				list.RemoveAt(num);
			}
		}
		return list;
	}

	public bool CheckUnlockFunction(FUNCTION_TYPE curFunction)
	{
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData.IsFunctionUnlock(curFunction))
		{
			return true;
		}
		return false;
	}
}
