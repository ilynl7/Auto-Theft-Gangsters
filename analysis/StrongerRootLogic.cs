using System.Collections.Generic;
using UnityEngine;

public class StrongerRootLogic : SingletonUnity<StrongerRootLogic>
{
	public enum LeftPage
	{
		expPage = 2,
		equipPage = 6,
		badgePage = 7,
		MaterialPage = 12
	}

	private GameDefine.UIBACKTYPE curBackType;

	public UIGrid LeftGrid;

	public List<StrongerItemLogic> leftItemList;

	public UIScrollView LeftScrollView;

	public UIGrid RightGrid;

	public List<StrongerLineLogic> rightLineList;

	public UIScrollView RightScrollview;

	public UILabel LevelLabel;

	public UILabel TargetPower;

	public UILabel CurPower;

	public UILabel Rank1Label;

	public UILabel Rank2Label;

	public UITexture BannerTexture;

	public UISlider LeftSlider;

	private List<StrongerData> curStrongList = new List<StrongerData>();

	private int curType;

	private int TargetTypePage = -1;

	private void OnEnable()
	{
		TargetTypePage = -1;
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			List<MenuTabBtnInfo> leftBtnInfo = new List<MenuTabBtnInfo>();
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(leftBtnInfo, OnClickCloseBtn, hideTab: true);
			SingletonUnity<MenuBaseRootLogic>.Instance.SetPageLabel(StrDictionary.GetDictionaryString("#{800101}"));
		});
		InitTexture();
	}

	public void InitTexture()
	{
		if (BannerTexture.mainTexture == null && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(GameDefine.TestureBannerStronger, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(string name, Texture textureObj)
	{
		BannerTexture.mainTexture = textureObj;
	}

	public void SetTargetPage(LeftPage targetid)
	{
		TargetTypePage = (int)targetid;
	}

	public void Reset(GameDefine.UIBACKTYPE needback = GameDefine.UIBACKTYPE.NOTHINTG)
	{
		curType = -1;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		LevelLabel.text = $"Lv.{playerData.Level}";
		CurPower.text = playerData.MainPlayerAttrData.ComboValue.ToString();
		BaseLvData levelDataByLevel = DataManager.GetLevelDataByLevel(playerData.Level);
		TargetPower.text = $"{levelDataByLevel.RecomPower}";
		float num = (float)playerData.MainPlayerAttrData.ComboValue / (float)levelDataByLevel.RecomPower;
		if (num >= 0.9f)
		{
			Rank1Label.text = "A";
			Rank2Label.text = "A";
		}
		else if (num >= 0.75f)
		{
			Rank1Label.text = "B";
			Rank2Label.text = "B";
		}
		else if (num >= 0.5f)
		{
			Rank1Label.text = "C";
			Rank2Label.text = "C";
		}
		else
		{
			Rank1Label.text = "D";
			Rank2Label.text = "D";
		}
		int num2 = GameDefine.STRONGER_TYPE_NAME.Count - leftItemList.Count;
		if (num2 > 0)
		{
			for (int i = 0; i < num2; i++)
			{
				GameObject gameObject = Object.Instantiate(leftItemList[0].gameObject) as GameObject;
				LeftGrid.AddChild(gameObject.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.name = $"left{leftItemList.Count:d2}";
				leftItemList.Add(gameObject.GetComponent<StrongerItemLogic>());
			}
		}
		int num3 = 0;
		for (int j = 0; j < leftItemList.Count; j++)
		{
			if (j < GameDefine.STRONGER_TYPE_NAME.Count)
			{
				UnityVersionUtil.SetActiveRecursive(leftItemList[j].gameObject, state: true);
				leftItemList[j].Reset(j + 1, OnClickLeftBtn);
				if (j + 1 == TargetTypePage)
				{
					num3 = j;
				}
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(leftItemList[j].gameObject, state: false);
			}
		}
		LeftScrollView.ResetPosition();
		LeftGrid.Reposition();
		leftItemList[num3].OnClickItemBtn();
		if (num3 > 6)
		{
			LeftSlider.value = 1f;
		}
		curBackType = needback;
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("BeStronger", "BeStronger", "open");
	}

	public void OnClickLeftBtn(int selecttype)
	{
		if (curType == selecttype)
		{
			return;
		}
		curType = selecttype;
		SelectLeftItem();
		curBackType = GameDefine.UIBACKTYPE.NOTHINTG;
		GetCurList();
		int num = curStrongList.Count - rightLineList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(rightLineList[0].gameObject) as GameObject;
				RightGrid.AddChild(gameObject.transform);
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.name = $"right{rightLineList.Count:d2}";
				rightLineList.Add(gameObject.GetComponent<StrongerLineLogic>());
			}
		}
		for (int j = 0; j < rightLineList.Count; j++)
		{
			if (j < curStrongList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(rightLineList[j].gameObject, state: true);
				rightLineList[j].Reset(curStrongList[j]);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(rightLineList[j].gameObject, state: false);
			}
		}
		RightScrollview.ResetPosition();
		RightGrid.Reposition();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("BeStronger", "BeStronger", $"clicktab_{curType}");
	}

	private void SelectLeftItem()
	{
		for (int i = 0; i < leftItemList.Count; i++)
		{
			leftItemList[i].RefershSelect(curType);
		}
	}

	public void GetCurList()
	{
		curStrongList.Clear();
		List<StrongerData> strongerDataList = DataManager.GetStrongerDataList();
		for (int i = 0; i < strongerDataList.Count; i++)
		{
			if (strongerDataList[i].StrongerType != curType)
			{
				continue;
			}
			if (curType == 1)
			{
				if (CheckType((GameDefine.STRONGER_ACTIVITY)strongerDataList[i].Type) && CheckLevel(strongerDataList[i].MinLevel, strongerDataList[i].MaxLevel))
				{
					curStrongList.Add(strongerDataList[i]);
				}
			}
			else
			{
				curStrongList.Add(strongerDataList[i]);
			}
		}
		curStrongList.Sort((StrongerData x, StrongerData y) => (x.SortID == y.SortID) ? (int.Parse(x.ID) - int.Parse(y.ID)) : (x.SortID - y.SortID));
	}

	public bool CheckLevel(int minlevel, int maxlevel)
	{
		if (maxlevel <= 0)
		{
			return true;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.Level >= minlevel && playerData.Level < maxlevel)
		{
			return true;
		}
		return false;
	}

	public bool CheckType(GameDefine.STRONGER_ACTIVITY strongertype)
	{
		switch (strongertype)
		{
		case GameDefine.STRONGER_ACTIVITY.FIRSTBUY:
		{
			PlayerCommonData playerCommonData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			return playerCommonData2.First_PackFlag;
		}
		case GameDefine.STRONGER_ACTIVITY.BIGSALE:
		{
			PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			return playerCommonData.Big_PackFlag;
		}
		default:
			return true;
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.StrongerRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		if (curBackType == GameDefine.UIBACKTYPE.EQUIP)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate
			{
				SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickEquipBackPackBtn();
			});
		}
		else if (curBackType == GameDefine.UIBACKTYPE.BADGE)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate
			{
				SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickBadgeBtn();
			});
		}
		else if (curBackType == GameDefine.UIBACKTYPE.ITEM)
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.PlayerInfoMenuRoot, delegate
			{
				SingletonUnity<PlayerInfoMenuRootLogic>.Instance.OnClickItemBackPackBtn();
			});
		}
	}
}
