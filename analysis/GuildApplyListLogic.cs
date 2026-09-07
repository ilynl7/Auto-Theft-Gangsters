using System.Collections.Generic;
using UnityEngine;

public class GuildApplyListLogic : SingletonUnity<GuildApplyListLogic>
{
	public List<GuildApplyListItemLogic> ApplyList = new List<GuildApplyListItemLogic>();

	public UIGrid ApplyGrid;

	public void UpdataApplyList(List<GuildMember> list)
	{
		int num = list.Count - ApplyList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(ApplyList[0].gameObject) as GameObject;
				ApplyGrid.AddChild(gameObject.transform);
				gameObject.transform.localScale = Vector3.one;
				ApplyList.Add(gameObject.GetComponent<GuildApplyListItemLogic>());
			}
		}
		for (int j = 0; j < ApplyList.Count; j++)
		{
			NGUITools.SetActive(ApplyList[j].gameObject, j < list.Count);
		}
		ApplyGrid.Reposition();
		int num2 = 0;
		for (int k = 0; k < list.Count; k++)
		{
			ApplyList[num2++].InitApplyListItem(list[k]);
		}
	}

	public void AllAgree()
	{
		for (int num = ApplyList.Count - 1; num >= 0; num--)
		{
			ApplyList[num].Agree();
		}
	}

	public void DelAll()
	{
		for (int num = ApplyList.Count - 1; num >= 0; num--)
		{
			ApplyList[num].DisAgree();
		}
	}

	public void OnClickClose()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildApplyListLogicRoot);
	}
}
