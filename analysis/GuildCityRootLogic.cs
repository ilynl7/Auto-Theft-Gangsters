using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildCityRootLogic : SingletonUnity<GuildCityRootLogic>
{
	public UIGrid ParentGrid;

	public List<GuildCityItemLogic> CityItemList;

	private List<guild_map_info> GuildMapList = new List<guild_map_info>();

	public void EnableReset()
	{
		for (int i = 0; i < CityItemList.Count; i++)
		{
			NGUITools.SetActive(CityItemList[i].gameObject, state: false);
		}
	}

	public void UpdateCityInfo()
	{
		GuildMapList.Clear();
		GuildMapList = new List<guild_map_info>(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.CurGuildCityDataDic.Values);
		int num = GuildMapList.Count - CityItemList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(CityItemList[i].gameObject) as GameObject;
				gameObject.transform.gameObject.name = $"cityitem_{CityItemList.Count:D2}";
				gameObject.transform.parent = ParentGrid.transform;
				gameObject.transform.position = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				GuildCityItemLogic component = gameObject.GetComponent<GuildCityItemLogic>();
				CityItemList.Add(component);
			}
		}
		for (int j = 0; j < CityItemList.Count; j++)
		{
			if (j < GuildMapList.Count)
			{
				NGUITools.SetActive(CityItemList[j].gameObject, state: true);
				CityItemList[j].UpdateInfo(GuildMapList[j]);
			}
			else
			{
				NGUITools.SetActive(CityItemList[j].gameObject, state: false);
			}
		}
		ParentGrid.Reposition();
	}

	public void UpdateCityItemInfo(ret_guild_map_reward.request request)
	{
		if (!request.HasId)
		{
			return;
		}
		int num = -1;
		for (int i = 0; i < GuildMapList.Count; i++)
		{
			if (GuildMapList[i].id.Equals(request.id))
			{
				num = i;
				GuildMapList[i].requireState = request.state;
				break;
			}
		}
		if (num != -1)
		{
			for (int j = 0; j < CityItemList.Count && !CityItemList[j].UpdateRewardInfo(GuildMapList[num]); j++)
			{
			}
		}
	}
}
