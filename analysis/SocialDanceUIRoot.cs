using System.Collections.Generic;
using UnityEngine;

public class SocialDanceUIRoot : SingletonUnity<SocialDanceUIRoot>
{
	public const int MAX_CD = 5;

	public List<SocialDanceItem> SocailDanceItemList = new List<SocialDanceItem>();

	public UIGrid grid;

	public static float CDTime = -1f;

	public static float progress;

	public void Reset()
	{
		List<SocialDanceData> list = new List<SocialDanceData>(DataManager.GetAllSocialDanceData().Values);
		progress = Mathf.Clamp01((CDTime - Time.realtimeSinceStartup) / 5f);
		if (list == null || list.Count == 0)
		{
			return;
		}
		int num = list.Count - SocailDanceItemList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(SocailDanceItemList[0].gameObject) as GameObject;
				gameObject.transform.parent = SocailDanceItemList[0].transform.parent;
				gameObject.name = $"{SocailDanceItemList.Count:D2}";
				gameObject.transform.localScale = Vector3.one;
				SocailDanceItemList.Add(gameObject.GetComponent<SocialDanceItem>());
			}
		}
		for (int j = 0; j < SocailDanceItemList.Count; j++)
		{
			if (j < list.Count)
			{
				SocailDanceItemList[j].Init(list[j]);
			}
			else
			{
				NGUITools.SetActive(SocailDanceItemList[j].gameObject, state: false);
			}
		}
		grid.Reposition();
	}

	public void OnClickSocial()
	{
		if (SingletonUnity<SocialDanceUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SocialDanceUIRoot>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SocialDanceRoot);
		}
	}

	private void Update()
	{
		if (CDTime > 0f)
		{
			progress = Mathf.Clamp01((CDTime - Time.realtimeSinceStartup) / 5f);
			if (progress <= 0f)
			{
				CDTime = -1f;
			}
		}
	}
}
