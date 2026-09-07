using System.Collections.Generic;
using UnityEngine;

public class NewMessageUIRootLogic : SingletonUnity<NewMessageUIRootLogic>
{
	public UIGrid ParentGrid;

	public List<NewMessageItemLogic> newsitemList;

	public static List<InviteTeamInfo> teamInfoList = new List<InviteTeamInfo>();

	public static List<GameDefine.ACTIVITY_TYPE> CurActList = new List<GameDefine.ACTIVITY_TYPE>();

	public static List<string> MissionList = new List<string>();

	public void ResetInfo(List<GameDefine.ACTIVITY_TYPE> messagelist)
	{
		CurActList = messagelist;
		MissionList = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.MissionTimeOutList;
		int num = messagelist.Count + teamInfoList.Count + MissionList.Count - newsitemList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(newsitemList[0].gameObject) as GameObject;
				NewMessageItemLogic component = gameObject.GetComponent<NewMessageItemLogic>();
				gameObject.name = $"message{newsitemList.Count:D2}";
				gameObject.transform.parent = ParentGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				newsitemList.Add(component);
			}
		}
		for (int j = 0; j < newsitemList.Count; j++)
		{
			if (j < messagelist.Count)
			{
				NGUITools.SetActive(newsitemList[j].gameObject, state: true);
				newsitemList[j].ResetInfo(messagelist[j]);
			}
			else if (j < messagelist.Count + teamInfoList.Count)
			{
				NGUITools.SetActive(newsitemList[j].gameObject, state: true);
				newsitemList[j].ResetInfo(teamInfoList[j - messagelist.Count]);
			}
			else if (j < messagelist.Count + teamInfoList.Count + MissionList.Count)
			{
				NGUITools.SetActive(newsitemList[j].gameObject, state: true);
				newsitemList[j].ResetInfo(MissionList[j - messagelist.Count - teamInfoList.Count]);
			}
			if (j >= messagelist.Count + teamInfoList.Count + MissionList.Count)
			{
				NGUITools.SetActive(newsitemList[j].gameObject, state: false);
			}
		}
		ParentGrid.Reposition();
	}

	public static void AddteamInfo(InviteTeamInfo newteaminfo)
	{
		if (newteaminfo.IsUrgeFlag)
		{
			for (int num = teamInfoList.Count - 1; num >= 0; num--)
			{
				if (teamInfoList[num].characterId == newteaminfo.characterId)
				{
					teamInfoList.RemoveAt(num);
				}
			}
		}
		else
		{
			for (int num2 = teamInfoList.Count - 1; num2 >= 0; num2--)
			{
				if (teamInfoList[num2].characterId == newteaminfo.characterId && teamInfoList[num2].teamId == newteaminfo.teamId)
				{
					teamInfoList.RemoveAt(num2);
				}
			}
		}
		if (teamInfoList.Count >= 10)
		{
			teamInfoList.RemoveAt(teamInfoList.Count - 1);
		}
		teamInfoList.Insert(0, newteaminfo);
		UpdateFunctionBtn();
	}

	public static void ClearAllteamMessage()
	{
		if (SingletonUnity<NewMessageUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewMessageUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<NewMessageUIRootLogic>.Instance.OnClickClearBtn();
			return;
		}
		teamInfoList.Clear();
		UpdateFunctionBtn();
	}

	public static bool IsHaveTeamMessageInfo()
	{
		if (teamInfoList != null && teamInfoList.Count > 0)
		{
			return true;
		}
		return false;
	}

	public void DecMissionLine(string missionid)
	{
		for (int num = MissionList.Count - 1; num >= 0; num--)
		{
			if (MissionList[num].Equals(missionid))
			{
				MissionList.RemoveAt(num);
			}
		}
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.DecMissionLine(missionid);
		UpdateFunctionBtn();
		ResetInfo(CurActList);
	}

	public void OnClickClearBtn()
	{
		for (int i = 0; i < newsitemList.Count; i++)
		{
			if (i >= CurActList.Count)
			{
				NGUITools.SetActive(newsitemList[i].gameObject, state: false);
			}
		}
		MissionList.Clear();
		SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ActivityData.MissionTimeOutList.Clear();
		ParentGrid.Reposition();
		teamInfoList.Clear();
		UpdateFunctionBtn();
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.NewMessageUIRoot);
	}

	public static void UpdateFunctionBtn()
	{
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateMessageTips();
		}
	}
}
