using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class MultiCopyRankResultRootLogic : SingletonUnity<MultiCopyRankResultRootLogic>
{
	public UILabel titleLabel;

	public List<DamageRankItemLogic> rankItemList;

	public List<damage_list> DamageList;

	public UIGrid RankGrid;

	public ShowRewardItems showrewarditem;

	public UILabel loseLabel;

	public UILabel selfRankLabel;

	public UILabel selfNameLabel;

	public UILabel selfDamageLabel;

	public UILabel finalImpactlabel;

	public UILabel finalImpactNamelabel;

	private battle_info curBattleInfo;

	private bool isbestFlag;

	public UISprite BestSp;

	public UILabel LastKillLabel;

	public void Reset(bool issuccess, battle_info battleinfo, List<item> items, int type, string activityid)
	{
		LastKillLabel.enabled = false;
		curBattleInfo = battleinfo;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		DamageList = curBattleInfo.damage_list;
		DamageList.Sort((damage_list x, damage_list y) => (int)y.damage - (int)x.damage);
		int num = DamageList.Count - rankItemList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(rankItemList[0].gameObject) as GameObject;
				gameObject.transform.parent = RankGrid.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				DamageRankItemLogic component = gameObject.GetComponent<DamageRankItemLogic>();
				rankItemList.Add(component);
				if (rankItemList.Count < 10)
				{
					gameObject.gameObject.name = "0" + rankItemList.Count;
				}
				else
				{
					gameObject.gameObject.name = rankItemList.Count.ToString();
				}
			}
			RankGrid.Reposition();
		}
		for (int j = 0; j < rankItemList.Count; j++)
		{
			if (j < DamageList.Count)
			{
				NGUITools.SetActive(rankItemList[j].gameObject, state: true);
				rankItemList[j].updateItem(j + 1, DamageList[j]);
			}
			else
			{
				NGUITools.SetActive(rankItemList[j].gameObject, state: false);
			}
		}
		if (type == 4)
		{
			for (int k = 0; k < DamageList.Count; k++)
			{
				if (DamageList[k].id == PlayerData.MainPlayerServerId)
				{
					isbestFlag = k == 0;
					selfRankLabel.text = $"NO.{k + 1}";
					selfNameLabel.text = DamageList[k].name;
					selfDamageLabel.text = $"{DamageList[k].damage}";
					break;
				}
			}
		}
		else
		{
			isbestFlag = battleinfo.my_rank == 1;
			selfRankLabel.text = $"NO.{battleinfo.my_rank}";
			selfNameLabel.text = playerData.MainPlayerAttrData.Name;
			selfDamageLabel.text = $"{battleinfo.my_damage}";
		}
		if (issuccess)
		{
			titleLabel.text = StrDictionary.GetDictionaryString("#{100753}");
			switch (type)
			{
			case 5:
			case 6:
				finalImpactlabel.text = StrDictionary.GetDictionaryString("#{100761}");
				finalImpactNamelabel.text = $"{curBattleInfo.lastKill}";
				if (curBattleInfo.lastKill.Equals(playerData.MainPlayerAttrData.Name))
				{
					LastKillLabel.enabled = true;
				}
				break;
			case 4:
				finalImpactlabel.text = string.Empty;
				finalImpactNamelabel.text = string.Empty;
				break;
			}
			showrewarditem.ShowRewards(items);
			NGUITools.SetActive(showrewarditem.gameObject, state: true);
			loseLabel.enabled = false;
			if (isbestFlag)
			{
				BestSp.enabled = true;
			}
			else
			{
				BestSp.enabled = false;
			}
			SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(7);
		}
		else
		{
			LocalDataSaveManager.SetDiedFlag(1);
			titleLabel.text = StrDictionary.GetDictionaryString("#{100757}");
			BestSp.enabled = false;
			finalImpactlabel.text = StrDictionary.GetDictionaryString("#{100758}");
			switch (type)
			{
			case 4:
			{
				BarFightCopyData barFightCopyDataByID = DataManager.GetBarFightCopyDataByID(activityid);
				finalImpactNamelabel.text = $"{new TimeSpan(0, 0, barFightCopyDataByID.ExistTime)}";
				break;
			}
			case 5:
			{
				WildBossData wildBossDataByID = DataManager.GetWildBossDataByID(activityid);
				finalImpactNamelabel.text = $"{new TimeSpan(0, 0, wildBossDataByID.ExistTime)}";
				break;
			}
			case 6:
			{
				GuildBossData guildBossDataByID = DataManager.GetGuildBossDataByID(activityid);
				finalImpactNamelabel.text = $"{new TimeSpan(0, 0, guildBossDataByID.ExistTime)}";
				break;
			}
			}
			NGUITools.SetActive(showrewarditem.gameObject, state: false);
			loseLabel.enabled = true;
		}
		SimpleRewardRootLogic.AddRewards(items);
		switch (type)
		{
		case 6:
			if (issuccess)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("guildboss", $"guildboss_{activityid}", "success");
			}
			else
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("guildboss", $"guildboss_{activityid}", "failure");
			}
			break;
		case 5:
			if (issuccess)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("wildboss", $"wildboss_{activityid}", "success");
			}
			else
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("wildboss", $"wildboss_{activityid}", "failure");
			}
			break;
		case 4:
			if (issuccess)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", $"activity_{type}", "success");
			}
			else
			{
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("TimeActivity", $"activity_{type}", "failure");
			}
			break;
		}
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

	public void OnClickleaveBtn()
	{
		NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
	}
}
