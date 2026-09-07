public class MissionTipRootLogic : SingletonUnity<MissionTipRootLogic>
{
	public UILabel TipsLabel;

	public TweenScale ClickAnima;

	private MISSION_LOGICTYPE curLogicType;

	private bool isInCarTips;

	private ObjMainPlayer mMainPlayer;

	public static void ShowMissionTips(string missionId, MISSION_STATE misState)
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsTutorialScene())
		{
			MissionData curMisData = DataManager.GetMissionDataByID(missionId);
			if (misState == MISSION_STATE.ACCEPTED)
			{
				if (curMisData.MissionLogicType == MISSION_LOGICTYPE.ROB_CAR || curMisData.MissionLogicType == MISSION_LOGICTYPE.MASSACRE_NPC || curMisData.MissionLogicType == MISSION_LOGICTYPE.DESTROY_CAR || curMisData.MissionLogicType == MISSION_LOGICTYPE.IMPACT_NPC)
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionTipsRoot, delegate
					{
						SingletonUnity<MissionTipRootLogic>.Instance.Reset(curMisData.MissionLogicType);
					});
				}
				else if (!string.IsNullOrEmpty(curMisData.TipStr))
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MissionTipsRoot, delegate
					{
						SingletonUnity<MissionTipRootLogic>.Instance.Reset(curMisData.MissionLogicType, curMisData.TipStr);
					});
				}
				else if (SingletonUnity<MissionTipRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MissionTipRootLogic>.Instance.gameObject))
				{
					SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MissionTipsRoot);
				}
			}
			else if (SingletonUnity<MissionTipRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MissionTipRootLogic>.Instance.gameObject))
			{
				SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MissionTipsRoot);
			}
		}
		else if (SingletonUnity<MissionTipRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MissionTipRootLogic>.Instance.gameObject))
		{
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MissionTipsRoot);
		}
	}

	public void Reset(MISSION_LOGICTYPE curType, string str)
	{
		curLogicType = curType;
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		TipsLabel.text = StrDictionary.GetDictionaryString(str);
	}

	public void Reset(MISSION_LOGICTYPE curType)
	{
		curLogicType = curType;
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (curLogicType == MISSION_LOGICTYPE.ROB_CAR)
		{
			TipsLabel.text = StrDictionary.GetDictionaryString("#{102076}");
		}
		else if (curLogicType == MISSION_LOGICTYPE.MASSACRE_NPC)
		{
			TipsLabel.text = StrDictionary.GetDictionaryString("#{102077}");
		}
		else if (curLogicType == MISSION_LOGICTYPE.DESTROY_CAR)
		{
			TipsLabel.text = StrDictionary.GetDictionaryString("#{102078}");
		}
		else if (curLogicType == MISSION_LOGICTYPE.IMPACT_NPC)
		{
			if (mMainPlayer.IsLocalDrivingCar)
			{
				TipsLabel.text = StrDictionary.GetDictionaryString("#{102079}");
				isInCarTips = true;
			}
			else
			{
				TipsLabel.text = StrDictionary.GetDictionaryString("#{102080}");
				isInCarTips = false;
			}
		}
		ClickAnima.ResetToBeginning();
		ClickAnima.Play();
	}

	private void Update()
	{
		if (curLogicType == MISSION_LOGICTYPE.IMPACT_NPC && isInCarTips != mMainPlayer.IsLocalDrivingCar)
		{
			if (mMainPlayer.IsLocalDrivingCar)
			{
				TipsLabel.text = StrDictionary.GetDictionaryString("#{102079}");
				isInCarTips = true;
			}
			else
			{
				TipsLabel.text = StrDictionary.GetDictionaryString("#{102080}");
				isInCarTips = false;
			}
		}
	}
}
