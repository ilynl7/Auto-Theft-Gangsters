using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class WildBossRootLogic : SingletonUnity<WildBossRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UIScrollView uiScrollView;

	public List<WildBossLineLogic> wildBossLines = new List<WildBossLineLogic>();

	private List<activity_info> wildBossInfos = new List<activity_info>();

	private activity_info curBossInfo;

	public ShowRewardItems ShowRewardItemScripts;

	public UILabel DescLabel;

	public UITable RootTable;

	private int mCurChoosedIndex = -1;

	public UISprite startBtnSp;

	private int canStartFlag;

	public List<UILabel> rankLabels;

	public UISprite tishiFlag;

	private WildBossData curWildBossData;

	public UITexture CopyBG;

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

	public void EnableReset()
	{
		for (int i = 0; i < wildBossLines.Count; i++)
		{
			NGUITools.SetActive(wildBossLines[i].gameObject, state: false);
		}
		UnityVersionUtil.SetActiveRecursive(wildBossLines[0].Sublineobj, state: false);
		UnityVersionUtil.SetActiveRecursive(ShowRewardItemScripts.gameObject, state: false);
		curBossInfo = null;
		curWildBossData = null;
	}

	public void RefershBossInfo(ret_request_wild_boss_info.request request)
	{
		wildBossInfos = new List<activity_info>(request.activity_info.Values);
		wildBossInfos.Sort((activity_info x, activity_info y) => int.Parse(x.ID) - int.Parse(y.ID));
		int num = 1;
		int num2 = num - wildBossLines.Count;
		int count = wildBossLines.Count;
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				GameObject gameObject = Object.Instantiate(wildBossLines[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{count + i}";
				WildBossLineLogic component = gameObject.GetComponent<WildBossLineLogic>();
				if (component != null)
				{
					component.transform.parent = wildBossLines[0].transform.parent;
					component.transform.localScale = Vector3.one;
					component.transform.localPosition = Vector3.zero;
					wildBossLines.Add(component);
				}
			}
		}
		for (int j = 0; j < wildBossLines.Count; j++)
		{
			NGUITools.SetActive(wildBossLines[j].gameObject, state: true);
			wildBossLines[j].ResetItem(wildBossInfos, OnClickItemBtn);
		}
		mCurChoosedIndex = -1;
		RootTable.Reposition();
		uiScrollView.ResetPosition();
		wildBossLines[0].OnClickItemBtn();
	}

	public void OnClickItemBtn(int index, int isenable)
	{
		if (mCurChoosedIndex == index)
		{
			return;
		}
		mCurChoosedIndex = index;
		curBossInfo = wildBossInfos[index];
		ShowRewardData showRewardData = null;
		string text = string.Empty;
		if (curBossInfo.Type == 5)
		{
			curWildBossData = DataManager.GetWildBossDataByID(curBossInfo.ID);
			showRewardData = DataManager.GetShowRewardDataByID(curWildBossData.ShowRewardID);
			text = StrDictionary.GetDictionaryString(curWildBossData.Desc);
		}
		DescLabel.text = text;
		if (curBossInfo.Parmstr != null)
		{
			string[] array = curBossInfo.Parmstr.Split('#');
			for (int i = 0; i < rankLabels.Count; i++)
			{
				if (i < array.Length)
				{
					if (!array[i].Equals(string.Empty))
					{
						string[] array2 = array[i].Split('@');
						rankLabels[i].text = $"{array2[1]}";
					}
					else
					{
						rankLabels[i].text = string.Format("{0}", "- - - -");
					}
				}
				else
				{
					rankLabels[i].text = string.Format("{0}", "- - - -");
				}
			}
		}
		else
		{
			for (int j = 0; j < rankLabels.Count; j++)
			{
				rankLabels[j].text = string.Format("{0}", "- - - -");
			}
		}
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemScripts.gameObject, state: true);
			SetRewardItem(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ShowRewardItemScripts.gameObject, state: false);
		}
		if (isenable == 0)
		{
			startBtnSp.spriteName = "CZ_anNiu_2";
		}
		else
		{
			startBtnSp.spriteName = "CZ_anNiu_2+";
		}
		canStartFlag = isenable;
		UpdateSelectItem();
		if (GameSettingData.GetPhoneClass() == 0)
		{
			if (CopyBG.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
			{
				StartCoroutine(BundleManager.LoadTexture(GameDefine.CopyBGNameDefault, TextureLoadFinish));
			}
		}
		else if (curWildBossData != null && (CopyBG.mainTexture == null || !CopyBG.mainTexture.name.Equals(curWildBossData.Background)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(curWildBossData.Background, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(string name, Texture tex)
	{
		CopyBG.mainTexture = tex;
	}

	public void UpdateSelectItem()
	{
		for (int i = 0; i < wildBossLines.Count; i++)
		{
			wildBossLines[i].RefreshSelect(mCurChoosedIndex);
		}
	}

	private void SetRewardItem(List<string> itemIds, List<EQUIP_QUALITY> qualitys, List<int> counts)
	{
		ShowRewardItemScripts.ShowRewards(itemIds, qualitys, counts);
	}

	public void OnClickStart()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.WORLD_BOSS_CLICK_START)
		{
			CheckTutorialEvent();
		}
		if (canStartFlag != 0)
		{
			switch (canStartFlag)
			{
			case 1:
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
				break;
			case 6:
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101539}"));
				break;
			case 7:
				TutorialManager.LevelLimitAction();
				break;
			case 3:
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{102057}"));
				break;
			case 2:
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101541}"));
				break;
			case 4:
			case 5:
				break;
			}
		}
		else if (curBossInfo != null && curBossInfo.Type == 5)
		{
			enter_wild_boss.request request = new enter_wild_boss.request();
			request.ID = curBossInfo.ID;
			WaitResponseUIRootLogic.OpenWaitBox(201, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.enter_wild_boss>(request);
			SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("wildboss", $"wildboss_{curBossInfo.ID}", "starttimes");
		}
	}

	public void OnClicktishiBtn()
	{
		if (curWildBossData != null)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
			{
				SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", curWildBossData.Rule, null);
			});
		}
	}

	private void OnDisable()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.WORLD_BOSS_CLICK_START)
		{
			CheckTutorialEvent();
		}
	}
}
