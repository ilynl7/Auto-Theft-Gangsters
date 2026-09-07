using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class LevelRewardRootLogic : SingletonUnity<LevelRewardRootLogic>
{
	public UILabel headinfoLabel;

	public UILabel bigrewardlabel;

	public UILabel diamondlabel;

	public UISprite Completeflag;

	public UISprite BtnSp;

	public UILabel BtnLabel;

	public List<LevelRewardLineLogic> LineItems;

	public UITexture BgTexture;

	private LevelRewardData CurData;

	private level_reward CurInfo;

	private bool CanGetBigRewardflag;

	private Dictionary<string, level_reward> levelrewardDic = new Dictionary<string, level_reward>();

	private List<level_reward> levelrewardList = new List<level_reward>();

	private int Playerlevel;

	private void OnEnable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(Refresh));
	}

	private void OnDisable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(Refresh));
	}

	public void EnableReset()
	{
		for (int i = 0; i < LineItems.Count; i++)
		{
			NGUITools.SetActive(LineItems[i].gameObject, state: false);
		}
		NGUITools.SetActive(BtnSp.gameObject, state: false);
		Completeflag.enabled = false;
	}

	public void Reset(ret_level_reward.request request)
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
		SelectData();
	}

	private void Refresh()
	{
		if (CurData != null && CurInfo != null)
		{
			RefershInfo(CurData, CurInfo);
		}
	}

	public void UpdateInfo(get_level_reward.request request)
	{
		if (CurData != null && CurInfo != null && request.HasLevel_reward && request.level_reward.ContainsKey(CurInfo.ID))
		{
			CurInfo = request.level_reward[CurInfo.ID];
			RefershInfo(CurData, CurInfo);
		}
	}

	public void SelectData()
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
			if (CheckCompleteState() && i < levelRewardDataList.Count - 1)
			{
				CurData = levelRewardDataList[i + 1];
				if (levelrewardDic.ContainsKey(CurData.ID))
				{
					CurInfo = levelrewardDic[CurData.ID];
				}
			}
		}
		if (CurData != null && CurInfo != null)
		{
			RefershInfo(CurData, CurInfo);
		}
	}

	public bool CheckCompleteState()
	{
		if (CurInfo != null && CurInfo.HasState && (CurInfo.state & 8) != 0L)
		{
			return true;
		}
		return false;
	}

	public void RefershInfo(LevelRewardData curdata, level_reward curinfo)
	{
		InitTexture();
		CurData = curdata;
		CurInfo = curinfo;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		for (int i = 0; i < LineItems.Count; i++)
		{
			NGUITools.SetActive(LineItems[i].gameObject, state: true);
			LineItems[i].UpdateInfo(curdata, i, curinfo);
		}
		headinfoLabel.text = StrDictionary.GetDictionaryString("#{100195}");
		bigrewardlabel.text = StrDictionary.GetDictionaryString("#{100196}", curdata.RewardNum);
		diamondlabel.text = curdata.RewardNum.ToString();
		Completeflag.enabled = false;
		NGUITools.SetActive(BtnSp.gameObject, state: true);
		BtnSp.spriteName = GameDefine.BtnIconNew[1];
		CanGetBigRewardflag = false;
		if (CurInfo == null || !CurInfo.HasState)
		{
			return;
		}
		if ((CurInfo.state & 8) != 0L)
		{
			Completeflag.enabled = true;
			BtnSp.spriteName = GameDefine.BtnIconNew[1];
			BtnLabel.text = StrDictionary.GetDictionaryString("#{300403}");
			return;
		}
		Completeflag.enabled = false;
		BtnLabel.text = StrDictionary.GetDictionaryString("#{300402}");
		if ((CurInfo.state & 1) != 0L && (CurInfo.state & 2) != 0L && (CurInfo.state & 4) != 0L && playerData.Level >= CurData.TargetLevel)
		{
			CanGetBigRewardflag = true;
			BtnSp.spriteName = GameDefine.BtnIconNew[0];
		}
		else
		{
			BtnSp.spriteName = GameDefine.BtnIconNew[1];
		}
	}

	public void OnClickLeftReceiveBtn()
	{
		if ((CurInfo.state & 8) == 0L)
		{
			if (CanGetBigRewardflag)
			{
				WaitResponseUIRootLogic.OpenWaitBox(297, 10f, 0f);
				receive_level_reward.request request = new receive_level_reward.request();
				request.ID = CurData.ID;
				request.index = 3L;
				NetLogic.GetInstance().Send<Protocol.receive_level_reward>(request);
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100197}");
			}
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.LevelRewardRoot);
	}

	public void InitTexture()
	{
		if (BgTexture.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(GameDefine.LevelRewardBg, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(string name, Texture textureObj)
	{
		if (textureObj != null)
		{
			BgTexture.mainTexture = textureObj;
		}
	}
}
