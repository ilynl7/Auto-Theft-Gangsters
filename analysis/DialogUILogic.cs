using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DialogUILogic : SingletonUnity<DialogUILogic>
{
	public UILabel TitleLabel;

	public UILabel TextLabel;

	public GameObject AcceptBtn;

	public GameObject CompleteBtn;

	public GameObject CancelBtn;

	public GameObject OkBtn;

	public ShowRewardItems ShowRewardItem;

	private string mCurMissionID = string.Empty;

	private MISSION_STATE mCurMissionType = MISSION_STATE.INVALID;

	public TeamFakeObjPicRootLogic NpcFakeObjRoot;

	public FakeObjLogic NpcFakeObj = new FakeObjLogic();

	public UITexture NpcPic;

	public void OnClickAcceptBtn()
	{
		CloseDialog();
		MissionData curMissionData = DataManager.GetMissionDataByID(mCurMissionID);
		if (curMissionData.Class == 4 || curMissionData.Class == 5)
		{
			GameManager gameManager = SingletonDontDestoryUnity<GameManager>.Instance;
			ActivityData activityData = gameManager.PlayerData.ActivityData;
			activity_info activity_info = null;
			activity_info = ((curMissionData.Class != 4) ? activityData.GetActivityInfoByType(2) : activityData.GetActivityInfoByType(1));
			if (activity_info.State == 1 && gameManager.PlayerCommonData.GetCurServerTime() < activity_info.Parm)
			{
				gameManager.MissionManager.AcceptMission(mCurMissionID);
				return;
			}
			if (activity_info.State == 0L && gameManager.PlayerCommonData.GetCurServerTime() >= activity_info.Parm)
			{
				gameManager.MissionManager.AcceptMission(mCurMissionID);
				return;
			}
			MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{101572}"), StrDictionary.GetDictionaryString("#{100127}"), delegate
			{
				SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AcceptMission(mCurMissionID);
				if (curMissionData.Class == 5 && gameManager.PlayerData.MainPlayerAttrData.PkMode == 0)
				{
					vp_Timer.In(Time.deltaTime * 2f, delegate
					{
						MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{102015}"), StrDictionary.GetDictionaryString("#{100127}"), delegate
						{
							request_change_pk_mode.request rpcReq = new request_change_pk_mode.request
							{
								pk = 1L
							};
							NetLogic.GetInstance().Send<Protocol.request_change_pk_mode>(rpcReq);
							gameManager.PlayerData.SetPKModeState(1);
						});
					});
				}
			});
		}
		else
		{
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AcceptMission(mCurMissionID);
		}
	}

	public void OnClickCompleteBtn()
	{
		CloseDialog();
		SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.CompleteMission(mCurMissionID);
	}

	public void OnClickCancelBtn()
	{
		CloseDialog();
		if (mCurMissionType == MISSION_STATE.ACCEPTED)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AcceptMission(mCurMissionID);
		}
	}

	public void OnClickOkBtn()
	{
		CloseDialog();
		MissionData missionDataByID = DataManager.GetMissionDataByID(mCurMissionID);
		if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
		{
			MissionManager missionManager = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager;
			List<MultiDeliveryMissionData> multiDeliveryMissionDataListById = DataManager.GetMultiDeliveryMissionDataListById(missionDataByID.LogicID);
			int num = (int)missionManager.GetMissionParam(mCurMissionID, 0);
			if (num == multiDeliveryMissionDataListById.Count - 1)
			{
				update_misison_complete.request request = new update_misison_complete.request();
				request.missionId = mCurMissionID;
				NetLogic.GetInstance().Send<Protocol.update_misison_complete>(request);
			}
			else
			{
				update_misison_parm.request request2 = new update_misison_parm.request();
				request2.missionId = mCurMissionID;
				request2.paramType = 1L;
				request2.paramValue = 1L;
				NetLogic.GetInstance().Send<Protocol.update_misison_parm>(request2);
			}
		}
	}

	private void CloseDialog()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DialogUI);
		Singleton<DialogManager>.Instance.OnCloseDialog();
	}

	public void Reset()
	{
		mCurMissionType = MISSION_STATE.INVALID;
		TitleLabel.text = string.Empty;
		TextLabel.text = string.Empty;
	}

	public void ResetSideMissionAcceptUI(string missionId, NpcData npcData)
	{
		Reset();
		mCurMissionID = missionId;
		MissionData missionDataByID = DataManager.GetMissionDataByID(mCurMissionID);
		if (missionDataByID != null)
		{
			NpcFakeObjRoot.EnableFakeObjRoot();
			NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
			NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			UnityVersionUtil.SetActiveRecursive(AcceptBtn, state: true);
			UnityVersionUtil.SetActiveRecursive(CompleteBtn, state: false);
			UnityVersionUtil.SetActiveRecursive(OkBtn, state: false);
			CancelBtn.transform.localPosition = new Vector3(180f, -34f, 0f);
			UnityVersionUtil.SetActiveRecursive(CancelBtn, state: true);
			ShowRewardItem.transform.localPosition = new Vector3(90f, -198f, 0f);
			TitleLabel.text = npcData.MName;
			TextLabel.text = StrDictionary.GetDictionaryString(missionDataByID.AcceptDialog);
			ShowRewardManager.ShowRewardItem(ShowRewardItem, mCurMissionID, REWARD_TYPE.ESCORT, playerData.Level, playerData.Profession);
		}
	}

	public void ResetMissionUI(string missionId, NpcData npcData, MISSION_STATE type)
	{
		Reset();
		mCurMissionID = missionId;
		MissionData missionDataByID = DataManager.GetMissionDataByID(mCurMissionID);
		if (missionDataByID != null)
		{
			mCurMissionType = type;
			NpcFakeObjRoot.EnableFakeObjRoot();
			NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
			if (mCurMissionType == MISSION_STATE.ACCEPTED)
			{
				UnityVersionUtil.SetActiveRecursive(AcceptBtn, state: true);
				UnityVersionUtil.SetActiveRecursive(CompleteBtn, state: false);
				UnityVersionUtil.SetActiveRecursive(OkBtn, state: false);
				UnityVersionUtil.SetActiveRecursive(CancelBtn, state: false);
				TitleLabel.text = npcData.MName;
				TextLabel.text = StrDictionary.GetDictionaryString(missionDataByID.AcceptDialog);
				ShowRewardManager.ShowRewardItem(ShowRewardItem, mCurMissionID, REWARD_TYPE.MISSION, 1, SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession);
				NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
			}
			else if (mCurMissionType == MISSION_STATE.COMPLETE)
			{
				UnityVersionUtil.SetActiveRecursive(AcceptBtn, state: false);
				UnityVersionUtil.SetActiveRecursive(OkBtn, state: false);
				UnityVersionUtil.SetActiveRecursive(CompleteBtn, state: true);
				UnityVersionUtil.SetActiveRecursive(CancelBtn, state: false);
				TitleLabel.text = npcData.MName;
				TextLabel.text = StrDictionary.GetDictionaryString(missionDataByID.CompleteDialog);
				ShowRewardManager.ShowRewardItem(ShowRewardItem, mCurMissionID, REWARD_TYPE.MISSION, 1, SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession);
				NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
			}
		}
	}

	public void ResetNormalDialogUI(string dialogId, NpcData npcData)
	{
		Reset();
		NPCDialogData nPCDialogDataByID = DataManager.GetNPCDialogDataByID(dialogId);
		if (nPCDialogDataByID != null)
		{
			UnityVersionUtil.SetActiveRecursive(AcceptBtn, state: false);
			UnityVersionUtil.SetActiveRecursive(CompleteBtn, state: false);
			UnityVersionUtil.SetActiveRecursive(OkBtn, state: false);
			UnityVersionUtil.SetActiveRecursive(CancelBtn, state: true);
			UnityVersionUtil.SetActiveRecursive(TitleLabel.gameObject, state: true);
			TitleLabel.text = npcData.MName;
			TextLabel.text = StrDictionary.GetDictionaryString(nPCDialogDataByID.Dialog);
			UnityVersionUtil.SetActiveRecursive(ShowRewardItem.gameObject, state: false);
			NpcFakeObjRoot.EnableFakeObjRoot();
			NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
			NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
		}
	}

	public void ResetNormalDialogByStr(string str, NpcData npcData)
	{
		Reset();
		NpcFakeObjRoot.EnableFakeObjRoot();
		NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
		NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
		UnityVersionUtil.SetActiveRecursive(AcceptBtn, state: false);
		UnityVersionUtil.SetActiveRecursive(CompleteBtn, state: false);
		UnityVersionUtil.SetActiveRecursive(OkBtn, state: false);
		UnityVersionUtil.SetActiveRecursive(CancelBtn, state: true);
		UnityVersionUtil.SetActiveRecursive(TitleLabel.gameObject, state: true);
		TitleLabel.text = npcData.MName;
		TextLabel.text = StrDictionary.GetDictionaryString(str);
		UnityVersionUtil.SetActiveRecursive(ShowRewardItem.gameObject, state: false);
	}

	public void ResetMissionTargetDialogUI(string missionId, NpcData npcData)
	{
		Reset();
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID != null)
		{
			NpcFakeObjRoot.EnableFakeObjRoot();
			NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
			NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
			mCurMissionID = missionId;
			UnityVersionUtil.SetActiveRecursive(AcceptBtn, state: false);
			UnityVersionUtil.SetActiveRecursive(CompleteBtn, state: false);
			UnityVersionUtil.SetActiveRecursive(OkBtn, state: true);
			UnityVersionUtil.SetActiveRecursive(CancelBtn, state: false);
			UnityVersionUtil.SetActiveRecursive(TitleLabel.gameObject, state: true);
			TitleLabel.text = npcData.MName;
			if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
			{
				MultiDeliveryMissionData curMultiDeliveryTargetData = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetCurMultiDeliveryTargetData(missionId);
				TextLabel.text = curMultiDeliveryTargetData.Dialog;
			}
			else if (!string.IsNullOrEmpty(missionDataByID.TargetDialog))
			{
				TextLabel.text = StrDictionary.GetDictionaryString(missionDataByID.TargetDialog);
			}
			else
			{
				NPCDialogData nPCDialogDataByID = DataManager.GetNPCDialogDataByID(npcData.TalkGroup);
				TextLabel.text = StrDictionary.GetDictionaryString(nPCDialogDataByID.Dialog);
			}
			UnityVersionUtil.SetActiveRecursive(ShowRewardItem.gameObject, state: false);
		}
	}

	private void OnEnable()
	{
		Singleton<ObjManager>.Instance.MainPlayer.IsTalking = true;
	}

	private void OnDisable()
	{
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.IsTalking = false;
		}
		NpcFakeObj.DisableNpcAnimaHandle();
	}

	protected override void OnDestroy()
	{
		NpcFakeObj.DestroyNpcFakeObj();
		base.OnDestroy();
	}
}
