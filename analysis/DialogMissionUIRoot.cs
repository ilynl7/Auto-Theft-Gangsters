using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DialogMissionUIRoot : SingletonUnity<DialogMissionUIRoot>
{
	public UILabel TitleLabel;

	public UILabel TextLabel;

	public GameObject AcceptBtn;

	public GameObject CompleteBtn;

	public UILabel CompleteLabel;

	public GameObject RewardLineObj;

	public ShowRewardItems ShowRewardItem;

	public UILabel MissionTitleLable;

	private string mCurMissionID = string.Empty;

	private MISSION_STATE mCurMissionType = MISSION_STATE.INVALID;

	public TeamFakeObjPicRootLogic NpcFakeObjRoot;

	public FakeObjLogic NpcFakeObj = new FakeObjLogic();

	public UITexture NpcPic;

	public UITexture DefaultNpcPic;

	private string mCurNpcId = string.Empty;

	protected override void Awake()
	{
		base.Awake();
	}

	public void OnClickAcceptBtn()
	{
		CloseDialog();
		MissionData missionDataByID = DataManager.GetMissionDataByID(mCurMissionID);
		if (missionDataByID.Class == 4 || missionDataByID.Class == 5)
		{
			GameManager gameManager = SingletonDontDestoryUnity<GameManager>.Instance;
			ActivityData activityData = gameManager.PlayerData.ActivityData;
			activity_info activity_info = null;
			activity_info = ((missionDataByID.Class != 4) ? activityData.GetActivityInfoByType(2) : activityData.GetActivityInfoByType(1));
			if (activity_info.State == 1 && gameManager.PlayerCommonData.GetCurServerTime() < activity_info.Parm)
			{
				gameManager.MissionManager.AcceptMission(mCurMissionID);
				return;
			}
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AcceptMission(mCurMissionID);
			if (missionDataByID.Class == 5 && gameManager.PlayerData.MainPlayerAttrData.PkMode == 0)
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
			}
		}
		else
		{
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AcceptMission(mCurMissionID);
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AcceptMainMissionCheck(DataManager.GetMissionDataByID(mCurMissionID));
			if (!string.IsNullOrEmpty(mCurNpcId) && (string.IsNullOrEmpty(missionDataByID.Target) || !missionDataByID.Target.Equals(mCurNpcId)))
			{
				SingletonUnity<UIManager>.Instance.CheckReShowUI(UIInfo.DialogMissionUI);
			}
		}
	}

	public void OnClickCompleteBtn()
	{
		CloseDialog();
		SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.CompleteMission(mCurMissionID);
		MissionData missionDataByID = DataManager.GetMissionDataByID(mCurMissionID);
		if (string.IsNullOrEmpty(missionDataByID.NextID))
		{
			SingletonUnity<UIManager>.Instance.CheckReShowUI(UIInfo.DialogMissionUI);
		}
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
		else
		{
			update_misison_complete.request request3 = new update_misison_complete.request();
			request3.missionId = mCurMissionID;
			NetLogic.GetInstance().Send<Protocol.update_misison_complete>(request3);
		}
	}

	private void CloseDialog()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DialogMissionUI);
		Singleton<DialogManager>.Instance.OnCloseDialog();
	}

	public void Reset()
	{
		mCurMissionType = MISSION_STATE.INVALID;
		TitleLabel.text = string.Empty;
		TextLabel.text = string.Empty;
		MissionTitleLable.text = string.Empty;
		DefaultNpcPic.enabled = false;
		NpcPic.enabled = true;
		UnityVersionUtil.SetActiveRecursive(RewardLineObj, state: true);
		mCurNpcId = string.Empty;
	}

	public void ResetActMissionAcceptUI(string missionId)
	{
		Reset();
		NpcPic.enabled = false;
		DefaultNpcPic.enabled = true;
		mCurMissionID = missionId;
		MissionData missionDataByID = DataManager.GetMissionDataByID(mCurMissionID);
		if (missionDataByID != null)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			UnityVersionUtil.SetActiveRecursive(AcceptBtn, state: true);
			UnityVersionUtil.SetActiveRecursive(CompleteBtn, state: false);
			if (missionDataByID.Class == 8)
			{
				TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(missionDataByID.TimeLimitId);
				MissionTitleLable.text = timeLimitMissionDataByID.MName;
				TitleLabel.text = StrDictionary.GetDictionaryString("#{100324}");
				TextLabel.text = timeLimitMissionDataByID.MDesc;
			}
			else
			{
				MissionTitleLable.text = StrDictionary.GetDictionaryString(missionDataByID.TipDescribeID);
				TitleLabel.text = StrDictionary.GetDictionaryString("#{100324}");
				TextLabel.text = StrDictionary.GetDictionaryString(missionDataByID.DescribeID);
			}
			ShowRewardManager.ShowRewardItem(ShowRewardItem, mCurMissionID, REWARD_TYPE.MISSION, playerData.Level, playerData.Profession);
		}
	}

	public void ResetSideMissionAcceptUI(string missionId, NpcData npcData)
	{
		Reset();
		mCurMissionID = missionId;
		MissionData missionDataByID = DataManager.GetMissionDataByID(mCurMissionID);
		if (missionDataByID != null)
		{
			mCurNpcId = npcData.ID;
			NpcFakeObjRoot.EnableFakeObjRoot();
			NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
			NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			UnityVersionUtil.SetActiveRecursive(AcceptBtn, state: true);
			UnityVersionUtil.SetActiveRecursive(CompleteBtn, state: false);
			if (missionDataByID.Class == 8)
			{
				TimeLimitMissionData timeLimitMissionDataByID = DataManager.GetTimeLimitMissionDataByID(missionDataByID.TimeLimitId);
				MissionTitleLable.text = timeLimitMissionDataByID.MDesc;
				TitleLabel.text = StrDictionary.GetDictionaryString("#{100324}");
				TextLabel.text = timeLimitMissionDataByID.MDesc;
			}
			else
			{
				TitleLabel.text = npcData.MName;
				TextLabel.text = StrDictionary.GetDictionaryString(missionDataByID.AcceptDialog);
			}
			ShowRewardManager.ShowRewardItem(ShowRewardItem, mCurMissionID, REWARD_TYPE.ESCORT, playerData.Level, playerData.Profession);
		}
	}

	public void ResetMissionUI(string missionId, NpcData npcData, MISSION_STATE type)
	{
		Reset();
		mCurMissionID = missionId;
		MissionData missionDataByID = DataManager.GetMissionDataByID(mCurMissionID);
		if (missionDataByID == null)
		{
			return;
		}
		mCurNpcId = npcData.ID;
		mCurMissionType = type;
		NpcFakeObjRoot.EnableFakeObjRoot();
		NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
		if (mCurMissionType == MISSION_STATE.ACCEPTED)
		{
			UnityVersionUtil.SetActiveRecursive(AcceptBtn, state: true);
			UnityVersionUtil.SetActiveRecursive(CompleteBtn, state: false);
			TitleLabel.text = npcData.MName;
			TextLabel.text = StrDictionary.GetDictionaryString(missionDataByID.AcceptDialog);
			MissionTitleLable.text = missionDataByID.MTipDescribeID;
			ShowRewardManager.ShowRewardItem(ShowRewardItem, mCurMissionID, REWARD_TYPE.MISSION, 1, SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession);
			NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
		}
		else if (mCurMissionType == MISSION_STATE.COMPLETE)
		{
			UnityVersionUtil.SetActiveRecursive(AcceptBtn, state: false);
			UnityVersionUtil.SetActiveRecursive(CompleteBtn, state: true);
			TitleLabel.text = npcData.MName;
			MissionTitleLable.text = missionDataByID.MTipDescribeID;
			TextLabel.text = StrDictionary.GetDictionaryString(missionDataByID.CompleteDialog);
			ShowRewardManager.ShowRewardItem(ShowRewardItem, mCurMissionID, REWARD_TYPE.MISSION, 1, SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession);
			NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
			if (missionDataByID.Class == 3)
			{
				UnityVersionUtil.SetActiveRecursive(RewardLineObj, state: false);
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
			UnityVersionUtil.SetActiveRecursive(TitleLabel.gameObject, state: true);
			mCurNpcId = npcData.ID;
			TitleLabel.text = npcData.MName;
			TextLabel.text = StrDictionary.GetDictionaryString(nPCDialogDataByID.Dialog);
			UnityVersionUtil.SetActiveRecursive(ShowRewardItem.gameObject, state: false);
			NpcFakeObjRoot.EnableFakeObjRoot();
			NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
			NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
		}
	}

	public void ResetMissionTargetDialogUI(string missionId, NpcData npcData)
	{
		Reset();
		MissionData missionDataByID = DataManager.GetMissionDataByID(missionId);
		if (missionDataByID != null)
		{
			NpcFakeObjRoot.EnableFakeObjRoot();
			NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
			mCurNpcId = npcData.ID;
			NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
			mCurMissionID = missionId;
			UnityVersionUtil.SetActiveRecursive(AcceptBtn, state: false);
			UnityVersionUtil.SetActiveRecursive(CompleteBtn, state: false);
			UnityVersionUtil.SetActiveRecursive(TitleLabel.gameObject, state: true);
			TitleLabel.text = npcData.MName;
			if (missionDataByID.MissionLogicType == MISSION_LOGICTYPE.MULTI_DELIVERY)
			{
				MultiDeliveryMissionData curMultiDeliveryTargetData = SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.GetCurMultiDeliveryTargetData(missionId);
				TextLabel.text = curMultiDeliveryTargetData.Dialog;
			}
			else
			{
				TextLabel.text = StrDictionary.GetDictionaryString(missionDataByID.TargetDialog);
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
