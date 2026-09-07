using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class PlayerRankInfoRootLogic : SingletonUnity<PlayerRankInfoRootLogic>
{
	public UIGrid LeftTabGrid;

	public MenuBaseTabBtnLogic BtnPrefab;

	public List<MenuBaseTabBtnLogic> mCurBtnList;

	private List<MenuTabBtnInfo> mCurMenuTabBtnInfoList;

	public ModelRankRootLogic mModelRankLogic;

	public NoModelRankRootLogic mNoModelRankLogic;

	private RANK_TYPE mCurRankType;

	private RANK_TYPE mPreRankType;

	private RANK_TYPE BackRankType;

	private List<sort_item> mCurRankList = new List<sort_item>();

	private Color ambientLight;

	private void OnEnable()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			List<MenuTabBtnInfo> leftBtnInfo = new List<MenuTabBtnInfo>();
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(leftBtnInfo, OnClickCloseBtn, hideTab: true);
		});
		List<MenuTabBtnInfo> list = new List<MenuTabBtnInfo>();
		MenuTabBtnInfo item = new MenuTabBtnInfo(OnClickLevelBtn, isIcon: false, "icon", StrDictionary.GetDictionaryString("#{101301}"), FUNCTION_TYPE.COUNT);
		MenuTabBtnInfo item2 = new MenuTabBtnInfo(OnClickFightBtn, isIcon: false, "icon", StrDictionary.GetDictionaryString("#{101302}"), FUNCTION_TYPE.COUNT);
		MenuTabBtnInfo item3 = new MenuTabBtnInfo(OnClickPVPBtn, isIcon: false, "icon", StrDictionary.GetDictionaryString("#{101303}"), FUNCTION_TYPE.COUNT);
		MenuTabBtnInfo item4 = new MenuTabBtnInfo(OnClickTowerrBtn, isIcon: false, "icon", StrDictionary.GetDictionaryString("#{101304}"), FUNCTION_TYPE.COUNT);
		MenuTabBtnInfo item5 = new MenuTabBtnInfo(OnClickGuildBtn, isIcon: false, "icon", StrDictionary.GetDictionaryString("#{101305}"), FUNCTION_TYPE.COUNT);
		MenuTabBtnInfo item6 = new MenuTabBtnInfo(OnClickCarBtn, isIcon: false, "icon", StrDictionary.GetDictionaryString("#{101306}"), FUNCTION_TYPE.COUNT);
		MenuTabBtnInfo item7 = new MenuTabBtnInfo(OnClickSexBtn, isIcon: false, "icon", StrDictionary.GetDictionaryString("#{101629}"), FUNCTION_TYPE.COUNT);
		list.Add(item);
		list.Add(item2);
		list.Add(item3);
		list.Add(item4);
		list.Add(item5);
		list.Add(item6);
		list.Add(item7);
		ResetPage(list);
		ambientLight = RenderSettings.ambientLight;
	}

	public void SetCarLight()
	{
		float num = 40f / 51f;
		RenderSettings.ambientLight = new Color(0f, 0f, 0f, 1f);
	}

	public void ResetNormalLight()
	{
		RenderSettings.ambientLight = ambientLight;
	}

	public void ResetPage(List<MenuTabBtnInfo> leftBtnInfo)
	{
		if (leftBtnInfo == null)
		{
			return;
		}
		mCurMenuTabBtnInfoList = leftBtnInfo;
		int num = mCurMenuTabBtnInfoList.Count - mCurBtnList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(BtnPrefab.gameObject) as GameObject;
				MenuBaseTabBtnLogic component = gameObject.GetComponent<MenuBaseTabBtnLogic>();
				gameObject.name = mCurBtnList.Count.ToString();
				gameObject.transform.parent = LeftTabGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				mCurBtnList.Add(component);
			}
		}
		for (int j = 0; j < mCurBtnList.Count; j++)
		{
			if (j < mCurMenuTabBtnInfoList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(mCurBtnList[j].gameObject, state: true);
				mCurBtnList[j].Reset(mCurMenuTabBtnInfoList[j]);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(mCurBtnList[j].gameObject, state: false);
			}
		}
		LeftTabGrid.Reposition();
	}

	public void OnClickFightBtn()
	{
		OnClickRankType(1);
	}

	public void OnClickLevelBtn()
	{
		OnClickRankType(2);
	}

	public void OnClickPVPBtn()
	{
		OnClickRankType(3);
	}

	public void OnClickTowerrBtn()
	{
		OnClickRankType(5);
	}

	public void OnClickGuildBtn()
	{
		OnClickRankType(6);
	}

	public void OnClickCarBtn()
	{
		OnClickRankType(7);
	}

	public void OnClickSexBtn()
	{
		OnClickRankType(8);
	}

	public void SetTargetBtnToggleEnable(int index)
	{
		for (int i = 0; i < mCurBtnList.Count; i++)
		{
			if (i == index)
			{
				mCurBtnList[i].BtnToggle.alpha = 1f;
				if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
				{
					SingletonUnity<MenuBaseRootLogic>.Instance.SetPageLabel(mCurMenuTabBtnInfoList[i].PageName);
				}
			}
			else
			{
				mCurBtnList[i].BtnToggle.alpha = 0f;
			}
		}
	}

	public void Reset()
	{
		mPreRankType = RANK_TYPE.INVALID;
		UnityVersionUtil.SetActiveRecursive(mModelRankLogic.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(mNoModelRankLogic.gameObject, state: false);
		SetTargetBtnToggleEnable(0);
		OnClickRankType(2);
		BackRankType = RANK_TYPE.INVALID;
	}

	public void ResetToTower()
	{
		mPreRankType = RANK_TYPE.INVALID;
		UnityVersionUtil.SetActiveRecursive(mModelRankLogic.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(mNoModelRankLogic.gameObject, state: false);
		SetTargetBtnToggleEnable(3);
		OnClickRankType(5);
		BackRankType = RANK_TYPE.TOWER;
	}

	public void ResetToCar()
	{
		mPreRankType = RANK_TYPE.INVALID;
		UnityVersionUtil.SetActiveRecursive(mModelRankLogic.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(mNoModelRankLogic.gameObject, state: false);
		SetTargetBtnToggleEnable(5);
		OnClickRankType(7);
		BackRankType = RANK_TYPE.CAR;
	}

	public void ResetToSexMini()
	{
		mPreRankType = RANK_TYPE.INVALID;
		UnityVersionUtil.SetActiveRecursive(mModelRankLogic.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(mNoModelRankLogic.gameObject, state: false);
		SetTargetBtnToggleEnable(6);
		OnClickRankType(8);
		BackRankType = RANK_TYPE.SEX;
	}

	public void ResetToPVP()
	{
		mPreRankType = RANK_TYPE.INVALID;
		UnityVersionUtil.SetActiveRecursive(mModelRankLogic.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(mNoModelRankLogic.gameObject, state: false);
		SetTargetBtnToggleEnable(2);
		OnClickRankType(3);
		BackRankType = RANK_TYPE.LADDER;
	}

	private void OnClickRankType(int rankType)
	{
		if (rankType != (int)mPreRankType)
		{
			WaitResponseUIRootLogic.OpenWaitBox(191, 10f, 0f);
			request_top_rank_list.request request = new request_top_rank_list.request();
			request.sortType = rankType;
			NetLogic.GetInstance().Send<Protocol.request_top_rank_list>(request);
		}
	}

	public void UpdateRankTypeList(ret_top_rank_list.request request)
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			mCurRankList.Clear();
			if (request.HasSort_items)
			{
				mCurRankList = request.sort_items;
			}
			mCurRankType = (RANK_TYPE)request.sortType;
			switch (mCurRankType)
			{
			case RANK_TYPE.FIGHT:
				ResetNormalLight();
				UnityVersionUtil.SetActiveRecursive(mModelRankLogic.gameObject, state: true);
				UnityVersionUtil.SetActiveRecursive(mNoModelRankLogic.gameObject, state: false);
				mModelRankLogic.Reset(mCurRankList, mCurRankType);
				SetTargetBtnToggleEnable(1);
				break;
			case RANK_TYPE.LEVEL:
				ResetNormalLight();
				UnityVersionUtil.SetActiveRecursive(mModelRankLogic.gameObject, state: true);
				UnityVersionUtil.SetActiveRecursive(mNoModelRankLogic.gameObject, state: false);
				mModelRankLogic.Reset(mCurRankList, mCurRankType);
				SetTargetBtnToggleEnable(0);
				break;
			case RANK_TYPE.LADDER:
				ResetNormalLight();
				UnityVersionUtil.SetActiveRecursive(mModelRankLogic.gameObject, state: true);
				UnityVersionUtil.SetActiveRecursive(mNoModelRankLogic.gameObject, state: false);
				mModelRankLogic.Reset(mCurRankList, mCurRankType);
				SetTargetBtnToggleEnable(2);
				break;
			case RANK_TYPE.TOWER:
				ResetNormalLight();
				UnityVersionUtil.SetActiveRecursive(mModelRankLogic.gameObject, state: true);
				UnityVersionUtil.SetActiveRecursive(mNoModelRankLogic.gameObject, state: false);
				mModelRankLogic.Reset(mCurRankList, mCurRankType);
				SetTargetBtnToggleEnable(3);
				break;
			case RANK_TYPE.GUILD:
				ResetNormalLight();
				UnityVersionUtil.SetActiveRecursive(mModelRankLogic.gameObject, state: false);
				UnityVersionUtil.SetActiveRecursive(mNoModelRankLogic.gameObject, state: true);
				mNoModelRankLogic.Reset(mCurRankList, mCurRankType);
				SetTargetBtnToggleEnable(4);
				break;
			case RANK_TYPE.CAR:
				SetCarLight();
				UnityVersionUtil.SetActiveRecursive(mModelRankLogic.gameObject, state: true);
				UnityVersionUtil.SetActiveRecursive(mNoModelRankLogic.gameObject, state: false);
				mModelRankLogic.Reset(mCurRankList, mCurRankType);
				SetTargetBtnToggleEnable(5);
				break;
			case RANK_TYPE.SEX:
				ResetNormalLight();
				UnityVersionUtil.SetActiveRecursive(mModelRankLogic.gameObject, state: true);
				UnityVersionUtil.SetActiveRecursive(mNoModelRankLogic.gameObject, state: false);
				mModelRankLogic.Reset(mCurRankList, mCurRankType);
				SetTargetBtnToggleEnable(6);
				break;
			}
			mPreRankType = mCurRankType;
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PlayerRankInfoRoot);
		mModelRankLogic.UnLoadFakeObj();
		switch (BackRankType)
		{
		case RANK_TYPE.LADDER:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.ResetToPVP();
			});
			break;
		case RANK_TYPE.TOWER:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.OnClickDailyBtn(MAPTYPE.INVALID, null, null, GameDefine.ACTIVITY_TYPE.TOWER);
			});
			break;
		case RANK_TYPE.CAR:
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NewActivityUIRootLogic, delegate
			{
				SingletonUnity<NewActivityUIRootLogic>.Instance.ResetToDaily();
			});
			break;
		}
		ResetNormalLight();
	}

	private void OnDisable()
	{
		mModelRankLogic.UnLoadFakeObj();
		ResetNormalLight();
	}
}
