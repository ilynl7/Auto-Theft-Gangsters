using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ShowItemsRootLogic : SingletonUnity<ShowItemsRootLogic>
{
	public UILabel titlelabel;

	public UILabel infoLabel;

	public ShowRewardItems showrewarditem;

	public UILabel yesLabel;

	public UILabel noLabel;

	public GameObject YESobj;

	public GameObject NOobj;

	private DelegateDefine.NoParamDelegate onClickClose;

	private DelegateDefine.NoParamDelegate onClickNo;

	private DelegateDefine.NoParamDelegate onClickYes;

	public void Clear()
	{
		onClickClose = null;
		onClickNo = null;
		onClickYes = null;
	}

	public void ResetNoBtn(string rewardId, string titlestr, string infostr, DelegateDefine.NoParamDelegate closefun = null, params object[] args)
	{
		ShowRewardData showRewardData = null;
		showRewardData = DataManager.GetShowRewardDataByID(rewardId);
		if (showRewardData != null)
		{
			UnityVersionUtil.SetActiveRecursive(showrewarditem.gameObject, state: true);
			showrewarditem.ShowRewards(new List<string>(showRewardData.ItemIdList), new List<EQUIP_QUALITY>(showRewardData.QualityList), new List<int>(showRewardData.CountList));
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(showrewarditem.gameObject, state: false);
		}
		titlelabel.text = StrDictionary.GetDictionaryString(titlestr);
		infoLabel.text = StrDictionary.GetDictionaryString(infostr, args);
		Clear();
		onClickClose = closefun;
		NGUITools.SetActive(YESobj, state: false);
		NGUITools.SetActive(NOobj, state: false);
	}

	public static void ShowYestOrNoBtn(ItemData itemData, int count, string title, string infostr, DelegateDefine.NoParamDelegate yesfun = null, DelegateDefine.NoParamDelegate nofun = null, params object[] args)
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShowItemsRoot, delegate
		{
			SingletonUnity<ShowItemsRootLogic>.Instance.ResetYesNoBtn(itemData, count, title, infostr, yesfun, nofun, args);
		});
	}

	public void ResetYesNoBtn(ItemData itemData, int count, string titlestr, string infostr, DelegateDefine.NoParamDelegate yesfun, DelegateDefine.NoParamDelegate nofun, params object[] args)
	{
		showrewarditem.ShowRewards(itemData, count);
		titlelabel.text = StrDictionary.GetDictionaryString(titlestr);
		infoLabel.text = StrDictionary.GetDictionaryString(infostr, args);
		Clear();
		onClickYes = yesfun;
		onClickNo = nofun;
		onClickClose = nofun;
		YESobj.transform.localPosition = new Vector3(106f, -115f, 0f);
		NOobj.transform.localPosition = new Vector3(-106f, -115f, 0f);
	}

	public void ResetYesNoBtn(List<item> items, string titlestr, string infostr, DelegateDefine.NoParamDelegate yesfun, DelegateDefine.NoParamDelegate nofun, params object[] args)
	{
		if (items.Count != 0)
		{
			UnityVersionUtil.SetActiveRecursive(showrewarditem.gameObject, state: true);
			showrewarditem.ShowRewards(items);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(showrewarditem.gameObject, state: false);
		}
		titlelabel.text = StrDictionary.GetDictionaryString(titlestr);
		infoLabel.text = StrDictionary.GetDictionaryString(infostr, args);
		Clear();
		onClickYes = yesfun;
		onClickNo = nofun;
		onClickClose = nofun;
		YESobj.transform.localPosition = new Vector3(106f, -115f, 0f);
		NOobj.transform.localPosition = new Vector3(-106f, -115f, 0f);
	}

	public static void ShowYesBtn(ItemData itemData, string title, string infostr, DelegateDefine.NoParamDelegate yesfun = null, params object[] args)
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ShowItemsRoot, delegate
		{
			SingletonUnity<ShowItemsRootLogic>.Instance.ResetYesBtn(itemData, title, infostr, yesfun, args);
		});
	}

	public void ResetYesBtn(ItemData itemData, string titlestr, string infostr, DelegateDefine.NoParamDelegate yesfun = null, params object[] args)
	{
		showrewarditem.ShowRewards(itemData);
		titlelabel.text = StrDictionary.GetDictionaryString(titlestr);
		infoLabel.text = StrDictionary.GetDictionaryString(infostr, args);
		Clear();
		onClickYes = yesfun;
		NGUITools.SetActive(YESobj, state: true);
		NGUITools.SetActive(NOobj, state: false);
		YESobj.transform.localPosition = new Vector3(0f, -115f, 0f);
	}

	public void OnClickcloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShowItemsRoot);
		if (onClickClose != null)
		{
			onClickClose();
		}
	}

	public void OnClickYesBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShowItemsRoot);
		if (onClickYes != null)
		{
			onClickYes();
		}
	}

	public void OnClickNoBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ShowItemsRoot);
		if (onClickNo != null)
		{
			onClickNo();
		}
	}
}
