using System.Collections.Generic;
using UnityEngine;

public class WorldMapRoot : SingletonUnity<WorldMapRoot>
{
	public UITexture WorldMapPic;

	public List<WorldItemLogic> worldItemLogicList = new List<WorldItemLogic>();

	private bool isActflag;

	private void OnEnable()
	{
		InitTexture();
	}

	public void EnableReset(bool isact = false)
	{
		isActflag = isact;
	}

	public void Reset()
	{
		InitWorld(isActflag);
	}

	public void InitTexture()
	{
		List<string> list = new List<string>();
		if (WorldMapPic.mainTexture == null)
		{
			list.Add(GameDefine.WorldMap);
		}
		if (list.Count != 0 && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(list, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(Dictionary<string, Texture> retdic)
	{
		if (retdic.Count != 0 && retdic.ContainsKey(GameDefine.WorldMap))
		{
			WorldMapPic.mainTexture = retdic[GameDefine.WorldMap];
		}
	}

	private void InitWorld(bool Isact)
	{
		List<MapInfoData> mapInfoDataListByType = DataManager.GetMapInfoDataListByType(MAPTYPE.BIG_WORLD);
		mapInfoDataListByType.Add(DataManager.GetMapInfoDataListByType(MAPTYPE.TUTORIAL_CAR)[0]);
		int num = mapInfoDataListByType.Count - worldItemLogicList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(worldItemLogicList[0].gameObject) as GameObject;
				WorldItemLogic component = gameObject.GetComponent<WorldItemLogic>();
				gameObject.transform.parent = worldItemLogicList[0].gameObject.transform.parent;
				worldItemLogicList.Add(component);
			}
		}
		for (int j = 0; j < worldItemLogicList.Count; j++)
		{
			if (j < mapInfoDataListByType.Count)
			{
				NGUITools.SetActive(worldItemLogicList[j].gameObject, state: true);
				worldItemLogicList[j].ResetItem(mapInfoDataListByType[j], Isact);
			}
			else
			{
				NGUITools.SetActive(worldItemLogicList[j].gameObject, state: false);
			}
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.WorldMapRoot);
	}
}
