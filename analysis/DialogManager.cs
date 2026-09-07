using SprotoType;

public class DialogManager : Singleton<DialogManager>
{
	private ObjNPC mTargetNPC;

	public ObjNPC TargetNPC => mTargetNPC;

	public void OnCloseDialog()
	{
	}

	public void ShowDialog(ObjNPC objNpc, string missionId)
	{
		mTargetNPC = objNpc;
		mTargetNPC.AnimationLogic.PlayTalkAnimation();
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mTargetNPC != null)
		{
			mTargetNPC.FaceToPub(mainPlayer.Position);
			if (!mainPlayer.IsLocalDrivingCar)
			{
				mainPlayer.FaceToPub(mTargetNPC.Position);
			}
		}
		if (!(mTargetNPC == null) && mTargetNPC.CheckInDialogRange() && !PopCompleteMission(missionId) && !PopTargetMission(missionId) && !PopAcceptableMission(missionId) && !PopOptionDialog())
		{
			ShowNormalDialog(mTargetNPC.DefaultDialogID);
		}
	}

	private bool PopTargetMission(string targetMissionId)
	{
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		if (!string.IsNullOrEmpty(targetMissionId))
		{
			if (!missionManager.IsMissionAccepted(targetMissionId))
			{
				return false;
			}
			MissionData missionDataByID = DataManager.GetMissionDataByID(targetMissionId);
			if (missionDataByID == null)
			{
				return false;
			}
			if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level < missionDataByID.MinLv)
			{
				return false;
			}
			if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
			{
				MultiDeliveryMissionData curMultiDeliveryTargetData = missionManager.GetCurMultiDeliveryTargetData(targetMissionId);
				if (!mTargetNPC.NPCDataID.Equals(curMultiDeliveryTargetData.TargetNpcId))
				{
					return false;
				}
			}
			else if (missionDataByID.Target != mTargetNPC.NPCDataID)
			{
				return false;
			}
			if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.STORY && missionManager.GetMissionState(targetMissionId) == MISSION_STATE.ACCEPTED)
			{
				StoryDialogRootLogic.ShowStory(missionDataByID.LogicID, mTargetNPC.NPCData);
				return true;
			}
			if (missionManager.GetMissionState(targetMissionId) == MISSION_STATE.ACCEPTED)
			{
				ShowMissionDialogUI(targetMissionId);
				return true;
			}
		}
		else
		{
			for (int i = 0; i < mTargetNPC.MissionIdList.Count; i++)
			{
				string text = mTargetNPC.MissionIdList[i];
				if (!missionManager.IsMissionAccepted(text))
				{
					continue;
				}
				MissionData missionDataByID2 = DataManager.GetMissionDataByID(text);
				if (missionDataByID2 == null || SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level < missionDataByID2.MinLv)
				{
					continue;
				}
				if (missionDataByID2.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
				{
					MultiDeliveryMissionData curMultiDeliveryTargetData2 = missionManager.GetCurMultiDeliveryTargetData(text);
					if (!mTargetNPC.NPCDataID.Equals(curMultiDeliveryTargetData2.TargetNpcId))
					{
						continue;
					}
				}
				else if (missionDataByID2.Target != mTargetNPC.NPCDataID)
				{
					continue;
				}
				if (missionDataByID2.MissionLogicType == MISSION_LOGICTYPE.STORY && missionManager.GetMissionState(text) == MISSION_STATE.ACCEPTED)
				{
					StoryDialogRootLogic.ShowStory(missionDataByID2.LogicID, mTargetNPC.NPCData);
					return true;
				}
				if (missionManager.GetMissionState(text) != MISSION_STATE.ACCEPTED)
				{
					continue;
				}
				ShowMissionDialogUI(text);
				return true;
			}
		}
		return false;
	}

	private bool PopCompleteMission(string targetMissionId)
	{
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		if (!string.IsNullOrEmpty(targetMissionId))
		{
			if (mTargetNPC.MissionIdList.Contains(targetMissionId))
			{
				if (!missionManager.IsMissionAccepted(targetMissionId))
				{
					return false;
				}
				MissionData missionDataByID = DataManager.GetMissionDataByID(targetMissionId);
				if (missionDataByID == null)
				{
					return false;
				}
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level < missionDataByID.MinLv)
				{
					return false;
				}
				if (missionDataByID.Submit != mTargetNPC.NPCDataID)
				{
					return false;
				}
				if (missionManager.GetMissionState(targetMissionId) == MISSION_STATE.COMPLETE)
				{
					ShowMissionDialogUI(targetMissionId);
					return true;
				}
				if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.STORY)
				{
					StoryDialogRootLogic.ShowStory(missionDataByID.LogicID, mTargetNPC.NPCData);
					return true;
				}
			}
		}
		else
		{
			for (int i = 0; i < mTargetNPC.MissionIdList.Count; i++)
			{
				string text = mTargetNPC.MissionIdList[i];
				if (!missionManager.IsMissionAccepted(text))
				{
					continue;
				}
				MissionData missionDataByID2 = DataManager.GetMissionDataByID(text);
				if (missionDataByID2 == null)
				{
					return false;
				}
				if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level < missionDataByID2.MinLv)
				{
					return false;
				}
				if (!(missionDataByID2.Submit != mTargetNPC.NPCDataID))
				{
					if (missionManager.GetMissionState(text) == MISSION_STATE.COMPLETE)
					{
						ShowMissionDialogUI(text);
						return true;
					}
					if (missionDataByID2.MissionLogicType == MISSION_LOGICTYPE.STORY)
					{
						StoryDialogRootLogic.ShowStory(missionDataByID2.LogicID, mTargetNPC.NPCData);
						return true;
					}
				}
			}
		}
		return false;
	}

	private bool PopAcceptableMission(string missionId)
	{
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		if (!string.IsNullOrEmpty(missionId))
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
			if (missionDataByID == null)
			{
				return false;
			}
			if (missionDataByID.Accept != mTargetNPC.NPCDataID)
			{
				return false;
			}
			if (missionManager.IsMissionAcceptable(missionId))
			{
				ShowMissionDialogUI(missionId);
				return true;
			}
			if (missionDataByID.Class == 4 || missionDataByID.Class == 5)
			{
				if (missionManager.IsMissionAccepted(missionId))
				{
					ShowNormalDialogStr("#{102013}");
					return true;
				}
				PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
				if (missionDataByID.Class == 4)
				{
					CurMission robboryMission2 = missionManager.GetCurMissionByClassType(MISSION_CLASS_TYPE.ROBBERY);
					if (robboryMission2 != null)
					{
						MessageBoxLogic.OpenOKCancelBox("#{201001}", "#{100127}", delegate
						{
							missionManager.AbandonMission(robboryMission2.MissionId);
						});
						return true;
					}
					if (playerData.Level < missionDataByID.MinLv)
					{
						ShowNormalDialogStr("#{102010}");
						return true;
					}
					activity_info activityInfoByType = playerData.ActivityData.GetActivityInfoByType(1);
					if (activityInfoByType == null || activityInfoByType.CurNum <= 0)
					{
						ShowNormalDialogStr("#{102009}");
						return true;
					}
				}
				else if (missionDataByID.Class == 5)
				{
					CurMission escortMission2 = missionManager.GetCurMissionByClassType(MISSION_CLASS_TYPE.ESCORT);
					if (escortMission2 != null)
					{
						MessageBoxLogic.OpenOKCancelBox("#{201002}", "#{100127}", delegate
						{
							missionManager.AbandonMission(escortMission2.MissionId);
						});
						return true;
					}
					if (playerData.Level < missionDataByID.MinLv)
					{
						ShowNormalDialogStr("#{102012}");
						return true;
					}
					activity_info activityInfoByType2 = playerData.ActivityData.GetActivityInfoByType(2);
					if (activityInfoByType2 == null || activityInfoByType2.CurNum <= 0)
					{
						ShowNormalDialogStr("#{102011}");
						return true;
					}
				}
			}
		}
		else
		{
			for (int i = 0; i < mTargetNPC.MissionIdList.Count; i++)
			{
				string text = mTargetNPC.MissionIdList[i];
				MissionData missionDataByID2 = DataManager.GetMissionDataByID(text);
				if (missionDataByID2 == null || missionDataByID2.Accept != mTargetNPC.NPCDataID)
				{
					continue;
				}
				if (missionManager.IsMissionAcceptable(text))
				{
					ShowMissionDialogUI(text);
					return true;
				}
				if (missionDataByID2.Class != 4 && missionDataByID2.Class != 5)
				{
					continue;
				}
				if (missionManager.IsMissionAccepted(text))
				{
					ShowNormalDialogStr("#{102013}");
					return true;
				}
				PlayerData playerData2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
				if (missionDataByID2.Class == 4)
				{
					CurMission robboryMission = missionManager.GetCurMissionByClassType(MISSION_CLASS_TYPE.ROBBERY);
					if (robboryMission != null)
					{
						MessageBoxLogic.OpenOKCancelBox("#{201001}", "#{100127}", delegate
						{
							missionManager.AbandonMission(robboryMission.MissionId);
						});
						return true;
					}
					if (playerData2.Level < missionDataByID2.MinLv)
					{
						ShowNormalDialogStr("#{102010}");
						return true;
					}
					activity_info activityInfoByType3 = playerData2.ActivityData.GetActivityInfoByType(1);
					if (activityInfoByType3 == null || activityInfoByType3.CurNum <= 0)
					{
						ShowNormalDialogStr("#{102009}");
						return true;
					}
				}
				else
				{
					if (missionDataByID2.Class != 5)
					{
						continue;
					}
					CurMission escortMission = missionManager.GetCurMissionByClassType(MISSION_CLASS_TYPE.ESCORT);
					if (escortMission != null)
					{
						MessageBoxLogic.OpenOKCancelBox("#{201002}", "#{100127}", delegate
						{
							missionManager.AbandonMission(escortMission.MissionId);
						});
						return true;
					}
					if (playerData2.Level < missionDataByID2.MinLv)
					{
						ShowNormalDialogStr("#{102012}");
						return true;
					}
					activity_info activityInfoByType4 = playerData2.ActivityData.GetActivityInfoByType(2);
					if (activityInfoByType4 == null || activityInfoByType4.CurNum <= 0)
					{
						ShowNormalDialogStr("#{102011}");
						return true;
					}
				}
			}
		}
		return false;
	}

	public void ShowMissionDialogUI(string missionId)
	{
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (mTargetNPC == null || !mTargetNPC.IsContainsMission(missionId))
		{
			return;
		}
		MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
		bool flag = missionManager.IsMissionAccepted(missionId);
		MISSION_STATE missionState = missionManager.GetMissionState(missionId);
		if (flag)
		{
			switch (missionState)
			{
			case MISSION_STATE.COMPLETE:
				if (missionDataByID.Submit == mTargetNPC.NPCDataID)
				{
					ShowMissionDialogUI(missionId, MISSION_STATE.COMPLETE);
				}
				return;
			case MISSION_STATE.ACCEPTED:
				if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
				{
					MultiDeliveryMissionData curMultiDeliveryTargetData = missionManager.GetCurMultiDeliveryTargetData(missionId);
					if (mTargetNPC.NPCDataID.Equals(curMultiDeliveryTargetData.TargetNpcId))
					{
						ShowMissionTargetDialogUI(missionId);
					}
				}
				else if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.COPYSCENE_KILLMONSTER)
				{
					ShowEnterCopyDialogUI(missionId);
				}
				else if (missionDataByID.Target == mTargetNPC.NPCDataID)
				{
					ShowMissionTargetDialogUI(missionId);
				}
				return;
			default:
				return;
			case MISSION_STATE.FAIL:
				break;
			}
		}
		if (missionManager.IsMissionAcceptable(missionId) && missionDataByID.Accept == mTargetNPC.NPCDataID)
		{
			if (missionDataByID.Class == 4 || missionDataByID.Class == 5)
			{
				ShowSideMissionAcceptUI(missionId);
			}
			else
			{
				ShowMissionDialogUI(missionId, MISSION_STATE.ACCEPTED);
			}
		}
	}

	public void ShowEnterCopyDialogUI(string missionId)
	{
		if (SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OptionDialogUI, OnShowEnterMissionCopyDialogUI, missionId);
		}
	}

	private void OnShowEnterMissionCopyDialogUI(bool isSuccess, object misId)
	{
		if (!isSuccess)
		{
			return;
		}
		string id = (string)misId;
		MissionData missionDataByID = DataManager.GetMissionDataByID(id);
		NpcData npcDataByID = DataManager.GetNpcDataByID(missionDataByID.Target);
		if (SingletonUnity<OptionDialogUILogic>.Exists)
		{
			SingletonUnity<OptionDialogUILogic>.Instance.ResetOptionDialog(npcDataByID, missionDataByID.TargetDialog, "Yes", "No", missionDataByID.LogicID, delegate(string val)
			{
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
				enter_copy_scene.request rpcReq = new enter_copy_scene.request
				{
					mapInfoId = val
				};
				NetLogic.GetInstance().Send<Protocol.enter_copy_scene>(rpcReq);
			});
		}
	}

	public void ShowMissionTargetDialogUI(string missionId)
	{
		if (SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DialogUI, OnShowMissionTargetDialogUI, missionId);
		}
	}

	private void OnShowMissionTargetDialogUI(bool isSuccess, object misId)
	{
		if (isSuccess)
		{
			string missionId = (string)misId;
			if (SingletonUnity<DialogUILogic>.Exists)
			{
				SingletonUnity<DialogUILogic>.Instance.ResetMissionTargetDialogUI(missionId, mTargetNPC.NPCData);
			}
		}
	}

	public void ShowSideMissionAcceptUI(string missionId)
	{
		if (!SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
		{
			return;
		}
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		activity_info activity_info = null;
		activity_info = ((missionDataByID.Class != 4) ? playerData.ActivityData.GetActivityInfoByType(2) : playerData.ActivityData.GetActivityInfoByType(1));
		if (activity_info == null)
		{
			return;
		}
		if (activity_info.State == 0L && SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime() < activity_info.Parm)
		{
			MessageBoxLogic.OpenOKCancelBox("#{200067}", "#{100127}", delegate
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DialogMissionUI, delegate
				{
					SingletonUnity<DialogMissionUIRoot>.Instance.ResetSideMissionAcceptUI(missionId, mTargetNPC.NPCData);
				});
			});
		}
		else
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DialogMissionUI, delegate
			{
				SingletonUnity<DialogMissionUIRoot>.Instance.ResetSideMissionAcceptUI(missionId, mTargetNPC.NPCData);
			});
		}
	}

	public void ShowMissionDialogUI(string missionId, MISSION_STATE type)
	{
		MissionUIInfo param = new MissionUIInfo(missionId, type);
		if (SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DialogMissionUI, OnShowMissionDialog, param);
		}
	}

	private void OnShowMissionDialog(bool isSuccess, object info)
	{
		if (isSuccess)
		{
			MissionUIInfo missionUIInfo = (MissionUIInfo)info;
			if (SingletonUnity<DialogMissionUIRoot>.Exists && missionUIInfo != null)
			{
				SingletonUnity<DialogMissionUIRoot>.Instance.ResetMissionUI(missionUIInfo.MissionID, mTargetNPC.NPCData, missionUIInfo.uiType);
			}
		}
	}

	private void ShowNormalDialogStr(string str)
	{
		if (!SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
		{
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DialogUI, delegate
		{
			if (SingletonUnity<DialogUILogic>.Exists)
			{
				SingletonUnity<DialogUILogic>.Instance.ResetNormalDialogByStr(str, mTargetNPC.NPCData);
			}
		});
	}

	private void ShowNormalDialog(string dialogId)
	{
		if (SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DialogUI, OnShowNormalDialogUI, dialogId);
		}
	}

	private void OnShowNormalDialogUI(bool isSuccess, object diaId)
	{
		if (isSuccess)
		{
			string dialogId = (string)diaId;
			if (SingletonUnity<DialogUILogic>.Exists)
			{
				SingletonUnity<DialogUILogic>.Instance.ResetNormalDialogUI(dialogId, mTargetNPC.NPCData);
			}
		}
	}

	private bool PopOptionDialog()
	{
		NPCDialogData nPCDialogDataByID = DataManager.GetNPCDialogDataByID(mTargetNPC.DefaultDialogID);
		if (nPCDialogDataByID != null && !string.IsNullOrEmpty(nPCDialogDataByID.OptionDialogID))
		{
			ShowOptionDialogUI(nPCDialogDataByID.OptionDialogID);
			return true;
		}
		return false;
	}

	public void ShowOptionDialogUI(string optionDialogId)
	{
		if (SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.OptionDialogUI, OnShowOptionDialog, optionDialogId);
		}
	}

	private void OnShowOptionDialog(bool isSuccess, object optDialogId)
	{
		if (isSuccess && SingletonUnity<OptionDialogUILogic>.Exists)
		{
			SingletonUnity<OptionDialogUILogic>.Instance.ResetOptionDialog(optDialogId as string, mTargetNPC.NPCData);
		}
	}
}
