using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class TeamApplyListLogic : SingletonUnity<TeamApplyListLogic>
{
	public List<ApplyLineLogic> ApplyList = new List<ApplyLineLogic>();

	public UIGrid ApplyGrid;

	private void Start()
	{
		UpdataApplyList(new List<teammember>(SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo.ApplyMemberDic.Values));
	}

	public void UpdataApplyList(List<teammember> list)
	{
		if (!UnityVersionUtil.IsActive(base.gameObject))
		{
			return;
		}
		int num = list.Count - ApplyList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(ApplyList[0].gameObject) as GameObject;
				ApplyGrid.AddChild(gameObject.transform);
				gameObject.transform.localScale = Vector3.one;
				ApplyList.Add(gameObject.GetComponent<ApplyLineLogic>());
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
			ApplyList[num].OnClickAcceptBtn();
		}
	}

	public void DelAll()
	{
		for (int num = ApplyList.Count - 1; num >= 0; num--)
		{
			ApplyList[num].OnClickRejectBtn();
		}
	}

	public void OnClickClose()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TeamApplyListRoot);
	}
}
