using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildStarMapLogic : MonoBehaviour
{
	public UITexture mainTexture;

	public List<UISprite> StarItems;

	public Transform NextTra;

	private List<GuildStarData> DataList = new List<GuildStarData>();

	private string MapName;

	public void UpdateInfo(Dictionary<string, guild_star> GuildstarsDic, int CurMapId)
	{
		MapName = string.Empty;
		DataList.Clear();
		DataList = DataManager.GetGuildStarDataListByMapID(CurMapId);
		if (DataList.Count > 0)
		{
			MapName = DataList[0].PicName;
		}
		InitTexture();
		int num = DataList.Count - StarItems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(StarItems[0].gameObject) as GameObject;
				UISprite component = gameObject.GetComponent<UISprite>();
				gameObject.name = $"{StarItems.Count:D2}";
				gameObject.transform.parent = base.transform;
				gameObject.transform.localScale = Vector3.one;
				StarItems.Add(component);
			}
		}
		NGUITools.SetActive(NextTra.gameObject, state: false);
		int num2 = -1;
		for (int j = 0; j < StarItems.Count; j++)
		{
			if (j < DataList.Count)
			{
				NGUITools.SetActive(StarItems[j].gameObject, state: true);
				StarItems[j].transform.localPosition = new Vector3(DataList[j].PosX, DataList[j].PosY);
				if (GuildstarsDic.ContainsKey(DataList[j].ID) && GuildstarsDic[DataList[j].ID].state == 1)
				{
					StarItems[j].enabled = true;
					continue;
				}
				StarItems[j].enabled = false;
				if (num2 == -1)
				{
					num2 = j;
					NextTra.localPosition = new Vector3(DataList[j].PosX, DataList[j].PosY);
					NGUITools.SetActive(NextTra.gameObject, state: true);
				}
			}
			else
			{
				NGUITools.SetActive(StarItems[j].gameObject, state: false);
			}
		}
	}

	public void InitTexture()
	{
		if (!string.IsNullOrEmpty(MapName) && (mainTexture.mainTexture == null || !mainTexture.mainTexture.name.Equals(MapName)) && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(MapName, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(string name, Texture textureObj)
	{
		if (textureObj != null)
		{
			mainTexture.mainTexture = textureObj;
		}
	}
}
