using UnityEngine;

public class NewMissionLineLogic : MonoBehaviour
{
	public delegate void OnClickNewMissionLine(NewMissionLineLogic line);

	public UISprite IconPic;

	public UISprite StateIcon;

	public UISprite CompletePic;

	public UILabel TitleLabel;

	public UILabel StateLabel;

	public TweenRotation ArrowTween;

	public TweenScale InfoTween;

	public UISprite BottomLinePic;

	public NewMissionInfoRootLogic MissionInfoPage;

	public UIMyCenterOnChild MyCenterOn;

	public UITable mTable;

	private OnClickNewMissionLine onClickLine;

	private bool OpenFlag;

	private MissionData curMissionData;

	public string curMissionId => curMissionData.ID;

	public MissionData CurMissionData => curMissionData;

	public void RegisterOnClickLine(OnClickNewMissionLine func)
	{
		onClickLine = func;
	}

	public void ResetLine(MissionData curData, OnClickNewMissionLine func)
	{
		curMissionData = curData;
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		MISSION_STATE missionState = missionManager.GetMissionState(curMissionData.ID);
		IconPic.spriteName = GameDefine.MAP_ACTIVITY_MISSION_ICON[curMissionData.Class];
		if (missionManager.IsMissionAccepted(curMissionData.ID))
		{
			switch (missionState)
			{
			case MISSION_STATE.ACCEPTED:
				StateIcon.spriteName = "CZ_renWu_wenHao_hui";
				StateIcon.width = 20;
				StateIcon.height = 32;
				break;
			case MISSION_STATE.COMPLETE:
				StateIcon.spriteName = "CZ_renWu_wenHao";
				StateIcon.width = 20;
				StateIcon.height = 32;
				break;
			}
		}
		else
		{
			StateIcon.spriteName = "CZ_renWu_tanHao";
			StateIcon.width = 10;
			StateIcon.height = 37;
		}
		string empty = string.Empty;
		switch (curMissionData.Class)
		{
		case 0:
			empty = StrDictionary.GetDictionaryString("#{100168}", missionManager.GetMissionParam(curMissionData.ID, 3) + 1);
			break;
		case 1:
			empty = StrDictionary.GetDictionaryString("#{100169}");
			break;
		case 2:
			empty = StrDictionary.GetDictionaryString("#{100170}");
			break;
		case 3:
		case 6:
			empty = StrDictionary.GetDictionaryString("#{100171}");
			break;
		case 4:
			empty = StrDictionary.GetDictionaryString("#{100172}");
			break;
		case 5:
			empty = StrDictionary.GetDictionaryString("#{100173}");
			break;
		case 7:
			empty = StrDictionary.GetDictionaryString("#{100200}");
			break;
		case 8:
			empty = StrDictionary.GetDictionaryString("#{100001}");
			break;
		default:
			empty = "[Need Loc]";
			break;
		}
		if (curMissionData.Class == 0)
		{
			TitleLabel.text = StrDictionary.GetDictionaryString("#{100171}") + "[FDAE33]" + StrDictionary.GetDictionaryString(curMissionData.TipDescribeID) + empty + "[-]";
			MissionManager.GetMissionStateLabel(curMissionData, missionState, StateLabel);
		}
		else if (curMissionData.Class == 8)
		{
			TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(curMissionData.TimeLimitId);
			TitleLabel.text = empty + "[FDAE33]" + timeLimitMissionDataByID.MName + "[-]";
			if (missionState == MISSION_STATE.INVALID)
			{
				StateLabel.text = timeLimitMissionDataByID.MTip;
			}
			else
			{
				MissionManager.GetMissionStateLabel(curMissionData, missionState, StateLabel);
			}
		}
		else
		{
			TitleLabel.text = empty + "[FDAE33]" + StrDictionary.GetDictionaryString(curMissionData.TipDescribeID) + "[-]";
			MissionManager.GetMissionStateLabel(curMissionData, missionState, StateLabel);
		}
		if (missionState == MISSION_STATE.COMPLETE)
		{
			if (UnityVersionUtil.IsActive(base.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(CompletePic.gameObject, state: true);
			}
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(CompletePic.gameObject, state: false);
		}
		InfoTween.ResetToBeginning();
		ArrowTween.ResetToBeginning();
		OpenFlag = false;
		onClickLine = func;
		DisableSelect();
	}

	public void OnClickItemLine()
	{
		if (!OpenFlag)
		{
			OpenLine();
		}
		else
		{
			CloseLine();
		}
		if (onClickLine != null)
		{
			onClickLine(this);
		}
	}

	public void OpenLine()
	{
		OpenFlag = true;
		MissionInfoPage.Reset(curMissionData.ID);
		ArrowTween.PlayForward();
		InfoTween.PlayForward();
		EnableSelect();
	}

	public void CloseLine(bool directClose = false)
	{
		OpenFlag = false;
		if (directClose)
		{
			ArrowTween.ResetToBeginning();
			InfoTween.ResetToBeginning();
			mTable.Reposition();
		}
		else
		{
			ArrowTween.PlayReverse();
			InfoTween.PlayReverse();
		}
		DisableSelect();
	}

	public void EnableSelect()
	{
		BottomLinePic.spriteName = "CZ_huaDongBG_1";
	}

	public void DisableSelect()
	{
		BottomLinePic.spriteName = "CZ_huaDongBG";
	}
}
