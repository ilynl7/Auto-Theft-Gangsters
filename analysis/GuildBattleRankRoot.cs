using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildBattleRankRoot : SingletonUnity<GuildBattleRankRoot>
{
	public List<GuildRankItemLogic> itemlines;

	public int minNum = 8;

	private List<guild_info> TopList = new List<guild_info>();

	public UIGrid parentGrid;

	public void EnableReset()
	{
		for (int i = 0; i < itemlines.Count; i++)
		{
			NGUITools.SetActive(itemlines[i].gameObject, state: false);
		}
	}

	public void ResershInfo(ret_guild_battle_rank.request request)
	{
		TopList.Clear();
		if (request.HasGuild_info)
		{
			TopList = request.guild_info;
		}
		int num = Mathf.Min(TopList.Count, minNum) - itemlines.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(itemlines[0].gameObject) as GameObject;
				GuildRankItemLogic component = gameObject.GetComponent<GuildRankItemLogic>();
				gameObject.name = $"{itemlines.Count:D2}";
				gameObject.transform.parent = parentGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				itemlines.Add(component);
			}
		}
		for (int j = 0; j < itemlines.Count; j++)
		{
			if (j < TopList.Count)
			{
				NGUITools.SetActive(itemlines[j].gameObject, state: true);
				itemlines[j].ResetInfo(TopList[j], j);
			}
			else
			{
				NGUITools.SetActive(itemlines[j].gameObject, state: false);
			}
		}
		parentGrid.Reposition();
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleRankRoot);
	}
}
