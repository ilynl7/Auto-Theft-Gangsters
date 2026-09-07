using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class CityDamageRootLogic : SingletonUnity<CityDamageRootLogic>
{
	public List<DamageRankItemLogic> DamageItemList;

	public UIGrid GridParent;

	private List<damage_list> SingleDamageList = new List<damage_list>();

	private List<damage_list> GangDamageList = new List<damage_list>();

	private int myRank;

	private long myDamage;

	private int myGangRank;

	private long myGangDamage;

	private bool IsSingleInfo;

	public UISprite SelectSp;

	public Transform SingleTrans;

	public Transform GangTrans;

	public UILabel RankLabel;

	public UILabel NameLabel;

	public UILabel DamageLabel;

	public void EnableReset()
	{
		for (int i = 0; i < DamageItemList.Count; i++)
		{
			NGUITools.SetActive(DamageItemList[i].gameObject, state: false);
		}
		IsSingleInfo = true;
		UpdataSelect();
	}

	public void UpdateInfo(ret_guild_map_domine_top.request request)
	{
		SingleDamageList.Clear();
		GangDamageList.Clear();
		myRank = -1;
		myDamage = 0L;
		myGangRank = -1;
		myGangDamage = 0L;
		if (request.HasDamage_list)
		{
			SingleDamageList = request.damage_list;
		}
		SingleDamageList.Sort((damage_list x, damage_list y) => (int)(-x.damage + y.damage));
		if (request.HasGuild_damage_list)
		{
			GangDamageList = request.guild_damage_list;
		}
		GangDamageList.Sort((damage_list x, damage_list y) => (int)(-x.damage + y.damage));
		if (request.HasMy_rank2)
		{
			myRank = (int)request.my_rank2;
		}
		if (request.HasMy_damage2)
		{
			myDamage = request.my_damage2;
		}
		if (request.HasMy_rank)
		{
			myGangRank = (int)request.my_rank;
		}
		if (request.HasMy_damage)
		{
			myGangDamage = request.my_damage;
		}
		if (IsSingleInfo)
		{
			ShowSingleInfo();
		}
		else
		{
			ShowGangInfo();
		}
	}

	public void ShowSingleInfo()
	{
		int num = SingleDamageList.Count - DamageItemList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(DamageItemList[0].gameObject) as GameObject;
				DamageRankItemLogic component = gameObject.GetComponent<DamageRankItemLogic>();
				gameObject.name = $"{DamageItemList.Count:D2}";
				gameObject.transform.parent = GridParent.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				DamageItemList.Add(component);
			}
		}
		for (int j = 0; j < DamageItemList.Count; j++)
		{
			if (j < SingleDamageList.Count)
			{
				NGUITools.SetActive(DamageItemList[j].gameObject, state: true);
				DamageItemList[j].updateItem(j + 1, SingleDamageList[j]);
			}
			else
			{
				NGUITools.SetActive(DamageItemList[j].gameObject, state: false);
			}
		}
		GridParent.Reposition();
		if (myRank != -1)
		{
			RankLabel.text = $"NO.{myRank}";
			NameLabel.text = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.Name;
			DamageLabel.text = myDamage.ToString();
		}
		else
		{
			RankLabel.text = string.Empty;
			NameLabel.text = string.Empty;
			DamageLabel.text = string.Empty;
		}
	}

	public void ShowGangInfo()
	{
		int num = GangDamageList.Count - DamageItemList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(DamageItemList[0].gameObject) as GameObject;
				DamageRankItemLogic component = gameObject.GetComponent<DamageRankItemLogic>();
				gameObject.name = $"{DamageItemList.Count:D2}";
				gameObject.transform.parent = GridParent.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				DamageItemList.Add(component);
			}
		}
		for (int j = 0; j < DamageItemList.Count; j++)
		{
			if (j < GangDamageList.Count)
			{
				NGUITools.SetActive(DamageItemList[j].gameObject, state: true);
				DamageItemList[j].updateItem(j + 1, GangDamageList[j]);
			}
			else
			{
				NGUITools.SetActive(DamageItemList[j].gameObject, state: false);
			}
		}
		GridParent.Reposition();
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsHaveGuild() && myGangRank != -1)
		{
			RankLabel.text = $"NO.{myGangRank}";
			NameLabel.text = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.GuilName;
			DamageLabel.text = myGangDamage.ToString();
		}
		else
		{
			RankLabel.text = string.Empty;
			NameLabel.text = string.Empty;
			DamageLabel.text = string.Empty;
		}
	}

	public void OnClickSingleBtn()
	{
		if (!IsSingleInfo)
		{
			IsSingleInfo = true;
			ShowSingleInfo();
			UpdataSelect();
		}
	}

	public void OnClickGangBtn()
	{
		if (IsSingleInfo)
		{
			IsSingleInfo = false;
			ShowGangInfo();
			UpdataSelect();
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.CityDamageRoot);
	}

	public void UpdataSelect()
	{
		if (IsSingleInfo)
		{
			SelectSp.transform.parent = SingleTrans.transform;
			SelectSp.transform.localPosition = Vector3.zero;
		}
		else
		{
			SelectSp.transform.parent = GangTrans.transform;
			SelectSp.transform.localPosition = Vector3.zero;
		}
	}
}
