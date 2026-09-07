using System.Collections.Generic;
using UnityEngine;

public class JSShengWangLogic : SingletonUnity<JSShengWangLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	private List<TitleData> TitleDataList;

	private int CurTitleIndex;

	public List<TitleItemLogic> Titleitemlist;

	public UISprite curTitleSp;

	public UILabel curTitleNameLabel;

	public UILabel curHonorLabel;

	public UISprite promoteBtnsp;

	public Transform selectSpTra;

	private PROMOTE_TYPE curPromoteType;

	private int playerHonorLevel;

	private int playerHonor;

	public UISprite ATKsp;

	public UISprite HPsp;

	public UISprite DEFsp;

	public UISprite HITsp;

	public UISprite DGEsp;

	public UISprite CRIsp;

	public UISprite RESsp;

	public UISprite EXDsp;

	public UISprite EXRsp;

	public UISprite Tensp;

	public UILabel ATKValLabel;

	public UILabel HPValLabel;

	public UILabel DEFValLabel;

	public UILabel HITValLabel;

	public UILabel DGEValLabel;

	public UILabel CRIValLabel;

	public UILabel RESValLabel;

	public UILabel EXDValLabel;

	public UILabel EXRValLabel;

	public UILabel TenValLabel;

	public UILabel ATKLabel;

	public UILabel HPLabel;

	public UILabel DEFLabel;

	public UILabel HITLabel;

	public UILabel DGELabel;

	public UILabel CRILabel;

	public UILabel RESLabel;

	public UILabel EXDLabel;

	public UILabel EXRLabel;

	public UILabel TenLabel;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	private void Start()
	{
	}

	public void Reset()
	{
		TitleDataList = DataManager.GetTitleList();
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		playerHonor = playerData.MainPlayerAttrData.CurTitleExp;
		playerHonorLevel = playerData.MainPlayerAttrData.CurTitleLevel;
		for (int i = 0; i < Titleitemlist.Count; i++)
		{
			if (i < TitleDataList.Count)
			{
				Titleitemlist[i].reset(i + 1, TitleDataList[i + 1], playerHonorLevel, OnClickTitleItem);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(Titleitemlist[i].gameObject, state: false);
			}
		}
		CurTitleIndex = -1;
		if (playerHonorLevel < 10)
		{
			OnClickTitleItem(playerHonorLevel + 1);
		}
		else
		{
			OnClickTitleItem(10);
		}
		SetAttribute();
	}

	public void OnClickTitleItem(int chooseindex)
	{
		if (CurTitleIndex == chooseindex)
		{
			return;
		}
		CurTitleIndex = chooseindex;
		Titleitemlist[CurTitleIndex - 1].SetSelect(selectSpTra);
		for (int i = 0; i < Titleitemlist.Count; i++)
		{
			if (CurTitleIndex - 1 == i)
			{
				Titleitemlist[i].transform.localScale = Vector3.one;
			}
			else
			{
				Titleitemlist[i].transform.localScale = Vector3.one * 0.8f;
			}
		}
		curPromoteType = PROMOTE_TYPE.NORMAL;
		UpdateAttribute();
		curTitleSp.spriteName = TitleDataList[CurTitleIndex].Icon;
		curTitleSp.MakePixelPerfect();
		curTitleNameLabel.text = StrDictionary.GetDictionaryString(TitleDataList[CurTitleIndex].Name);
		int curALLHonor = GetCurALLHonor();
		curHonorLabel.text = $"{playerHonor}/{curALLHonor}";
		if (playerHonor < curALLHonor)
		{
			SetPromoteBtn(PROMOTE_TYPE.HONOR_LIMIT);
		}
		else if (playerHonorLevel >= CurTitleIndex)
		{
			SetPromoteBtn(PROMOTE_TYPE.LEVEL_GET);
		}
		else if (playerHonorLevel == CurTitleIndex - 1)
		{
			SetPromoteBtn(PROMOTE_TYPE.NORMAL);
		}
		else
		{
			SetPromoteBtn(PROMOTE_TYPE.LEVEL_LIMIT);
		}
	}

	public void OnClickPromoteBtn()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.TITLE_CLICK)
		{
			CheckTutorialEvent();
		}
		if (curPromoteType != 0)
		{
			switch (curPromoteType)
			{
			case PROMOTE_TYPE.LEVEL_GET:
				break;
			case PROMOTE_TYPE.LEVEL_LIMIT:
				NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101706}", StrDictionary.GetDictionaryString(TitleDataList[playerHonorLevel + 1].Name)));
				break;
			case PROMOTE_TYPE.HONOR_LIMIT:
				NoticeLogic.AddNotifyData("#{101705}");
				break;
			}
		}
		else
		{
			WaitResponseUIRootLogic.OpenWaitBox(156, 10f, 0f);
			NetLogic.GetInstance().Send<Protocol.title_req_level_up>();
		}
	}

	public void SetPromoteBtn(PROMOTE_TYPE isenable)
	{
		curPromoteType = isenable;
		if (isenable == PROMOTE_TYPE.NORMAL)
		{
			promoteBtnsp.spriteName = "CZ_anNiu_2";
		}
		else
		{
			promoteBtnsp.spriteName = "CZ_anNiu_2+";
		}
		if (isenable == PROMOTE_TYPE.LEVEL_GET)
		{
			NGUITools.SetActive(promoteBtnsp.gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(promoteBtnsp.gameObject, state: true);
		}
	}

	public void OnClickHonur()
	{
		ItemData itemDataByID = DataManager.GetItemDataByID("2002");
		ItemInfoRootLogicNew.ShowItemTips(itemDataByID);
	}

	public int GetCurALLHonor()
	{
		return TitleDataList[CurTitleIndex - 1].EXP;
	}

	public void UpdateAttribute()
	{
		TitleData titleData = TitleDataList[CurTitleIndex];
		TitleData titleData2 = TitleDataList[CurTitleIndex];
		ATKValLabel.text = GameDefine.GetAttributeValueStr(titleData2.Status1, titleData2.Value1);
		HPValLabel.text = GameDefine.GetAttributeValueStr(titleData2.Status2, titleData2.Value2);
		DEFValLabel.text = GameDefine.GetAttributeValueStr(titleData2.Status3, titleData2.Value3);
		HITValLabel.text = GameDefine.GetAttributeValueStr(titleData2.Status4, titleData2.Value4);
		DGEValLabel.text = GameDefine.GetAttributeValueStr(titleData2.Status5, titleData2.Value5);
		CRIValLabel.text = GameDefine.GetAttributeValueStr(titleData2.Status6, titleData2.Value6);
		RESValLabel.text = GameDefine.GetAttributeValueStr(titleData2.Status7, titleData2.Value7);
		EXDValLabel.text = GameDefine.GetAttributeValueStr(titleData2.Status8, titleData2.Value8);
		EXRValLabel.text = GameDefine.GetAttributeValueStr(titleData2.Status9, titleData2.Value9);
		TenValLabel.text = GameDefine.GetAttributeValueStr(titleData2.Status10, titleData2.Value10);
	}

	public void SetAttribute()
	{
		TitleData titleData = TitleDataList[CurTitleIndex];
		ATKLabel.text = GameDefine.GetAttributeName_S(titleData.Status1);
		HPLabel.text = GameDefine.GetAttributeName_S(titleData.Status2);
		DEFLabel.text = GameDefine.GetAttributeName_S(titleData.Status3);
		HITLabel.text = GameDefine.GetAttributeName_S(titleData.Status4);
		DGELabel.text = GameDefine.GetAttributeName_S(titleData.Status5);
		CRILabel.text = GameDefine.GetAttributeName_S(titleData.Status6);
		RESLabel.text = GameDefine.GetAttributeName_S(titleData.Status7);
		EXDLabel.text = GameDefine.GetAttributeName_S(titleData.Status8);
		EXRLabel.text = GameDefine.GetAttributeName_S(titleData.Status9);
		TenLabel.text = GameDefine.GetAttributeName_S(titleData.Status10);
		ATKsp.spriteName = GameDefine.GetAttributeIcon(titleData.Status1);
		HPsp.spriteName = GameDefine.GetAttributeIcon(titleData.Status2);
		DEFsp.spriteName = GameDefine.GetAttributeIcon(titleData.Status3);
		HITsp.spriteName = GameDefine.GetAttributeIcon(titleData.Status4);
		DGEsp.spriteName = GameDefine.GetAttributeIcon(titleData.Status5);
		CRIsp.spriteName = GameDefine.GetAttributeIcon(titleData.Status6);
		RESsp.spriteName = GameDefine.GetAttributeIcon(titleData.Status7);
		EXDsp.spriteName = GameDefine.GetAttributeIcon(titleData.Status8);
		EXRsp.spriteName = GameDefine.GetAttributeIcon(titleData.Status9);
		Tensp.spriteName = GameDefine.GetAttributeIcon(titleData.Status10);
	}

	private void OnDisable()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.TITLE_CLICK)
		{
			CheckTutorialEvent();
		}
	}
}
