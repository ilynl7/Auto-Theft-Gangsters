using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class LevelAnnounceRootLogic : SingletonUnity<LevelAnnounceRootLogic>
{
	public static int curGetRewardId = -1;

	public GameObject ObjRoot;

	public UILabel NameLabel1;

	public UILabel NameLabel2;

	private LevelRewardData CurData;

	private Dictionary<string, level_reward> levelrewardDic = new Dictionary<string, level_reward>();

	private List<level_reward> levelrewardList = new List<level_reward>();

	private level_reward CurInfo;

	private int Playerlevel;

	public UITexture EffectEdge;

	public TweenRotation BoxAnima;

	private int curshowrewardid = -1;

	public void Reset()
	{
		Refresh();
	}

	private void OnEnable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(Refresh));
	}

	private void OnDisable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(Refresh));
	}

	private void Refresh()
	{
		NetLogic.GetInstance().Send<Protocol.req_level_reward>();
		NGUITools.SetActive(ObjRoot, state: false);
		EffectEdge.enabled = false;
		BoxAnima.ResetToBeginning();
		BoxAnima.enabled = false;
	}

	public void UpdataInfo(ret_level_reward.request request)
	{
		CurData = null;
		CurInfo = null;
		levelrewardDic.Clear();
		levelrewardList.Clear();
		if (request.HasLevel_reward)
		{
			levelrewardDic = request.level_reward;
			levelrewardList = new List<level_reward>(request.level_reward.Values);
		}
		refershUI();
	}

	public void RefershInfo(get_level_reward.request request)
	{
		CurData = null;
		CurInfo = null;
		levelrewardDic.Clear();
		levelrewardList.Clear();
		if (request.HasLevel_reward)
		{
			levelrewardDic = request.level_reward;
			levelrewardList = new List<level_reward>(request.level_reward.Values);
		}
		refershUI();
	}

	public bool CheckCompleteState()
	{
		if (CurInfo != null && CurInfo.HasState && (CurInfo.state & 8) != 0L)
		{
			return true;
		}
		return false;
	}

	public void refershUI()
	{
		Playerlevel = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		List<LevelRewardData> levelRewardDataList = DataManager.GetLevelRewardDataList();
		levelRewardDataList.Sort((LevelRewardData x, LevelRewardData y) => (x.ID.Length == y.ID.Length) ? x.ID.CompareTo(y.ID) : (x.ID.Length - y.ID.Length));
		for (int i = 0; i < levelRewardDataList.Count; i++)
		{
			if (Playerlevel < levelRewardDataList[i].StartLv || Playerlevel > levelRewardDataList[i].EndLv)
			{
				continue;
			}
			CurData = levelRewardDataList[i];
			if (levelrewardDic.ContainsKey(CurData.ID))
			{
				CurInfo = levelrewardDic[CurData.ID];
			}
			if (!CheckCompleteState())
			{
				continue;
			}
			if (i < levelRewardDataList.Count - 1)
			{
				CurData = levelRewardDataList[i + 1];
				if (levelrewardDic.ContainsKey(CurData.ID))
				{
					CurInfo = levelrewardDic[CurData.ID];
				}
			}
			else
			{
				CurData = null;
			}
		}
		if (CurData == null)
		{
			NGUITools.SetActive(ObjRoot, state: false);
			return;
		}
		NameLabel1.text = StrDictionary.GetDictionaryString("#{100189}", CurData.EndLv);
		NameLabel2.text = StrDictionary.GetDictionaryString("#{100190}");
		NGUITools.SetActive(ObjRoot, state: true);
		CheckShowEffect();
	}

	public void CheckShowEffect()
	{
		curshowrewardid = -1;
		if (CurData != null && CurInfo != null)
		{
			if (Playerlevel >= CurData.TargetLevel1 && (CurInfo.state & 1) == 0L)
			{
				curshowrewardid = int.Parse(CurData.ID) * 4;
			}
			if (Playerlevel >= CurData.TargetLevel2 && (CurInfo.state & 2) == 0L)
			{
				curshowrewardid = int.Parse(CurData.ID) * 4 + 1;
			}
			if (Playerlevel >= CurData.TargetLevel3 && (CurInfo.state & 4) == 0L)
			{
				curshowrewardid = int.Parse(CurData.ID) * 4 + 2;
			}
			if (Playerlevel >= CurData.TargetLevel && (CurInfo.state & 8) == 0L && (CurInfo.state & 1) != 0L && (CurInfo.state & 2) != 0L && (CurInfo.state & 4) != 0L)
			{
				curshowrewardid = int.Parse(CurData.ID) * 4 + 3;
			}
		}
		if (curGetRewardId >= curshowrewardid)
		{
			EffectEdge.enabled = false;
			BoxAnima.ResetToBeginning();
			BoxAnima.enabled = false;
		}
		else
		{
			EffectEdge.enabled = true;
			BoxAnima.enabled = true;
		}
	}

	public void OnClickLevelBtn()
	{
		if (CurData != null && CurInfo != null)
		{
			curGetRewardId = curshowrewardid;
			EffectEdge.enabled = false;
			BoxAnima.ResetToBeginning();
			BoxAnima.enabled = false;
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.LevelRewardRoot, delegate
			{
				SingletonUnity<LevelRewardRootLogic>.Instance.RefershInfo(CurData, CurInfo);
			});
		}
	}
}
