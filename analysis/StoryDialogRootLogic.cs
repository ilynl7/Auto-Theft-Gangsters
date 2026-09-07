using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class StoryDialogRootLogic : SingletonUnity<StoryDialogRootLogic>
{
	public UITexture NpcPic;

	public UITexture PlayerPic;

	public UILabel RoleNameLabel;

	public UILabel TextLabel;

	public TeamFakeObjPicRootLogic NpcFakeObjRoot;

	public FakeObjLogic NpcFakeObj;

	public TeamFakeObjPicRootLogic PlayerFakeObjRoot;

	public FakeObjLogic PlayerFakeObj;

	private List<StoryData> mCurStoryDataList;

	private string mCurStoryId = string.Empty;

	private int mCurStoryStep;

	private string mCurStoryMissionId = string.Empty;

	private string mCurNpcId;

	private void ClearCurStoryData()
	{
		mCurStoryId = string.Empty;
		mCurStoryStep = 0;
		mCurStoryMissionId = string.Empty;
		mCurStoryDataList = null;
	}

	public static void ShowStory(string storyId, NpcData targetNpcData)
	{
		if (SingletonUnity<UIManager>.Instance.CloseAllPOPUI())
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.StoryDialogUIRoot, delegate
			{
				SingletonUnity<StoryDialogRootLogic>.Instance.StartStory(storyId, targetNpcData);
			});
		}
	}

	private static void OnStoryDialogShow(bool isSuccess, object param)
	{
		Debug.Log("OnStoryDialogShow");
		if (!isSuccess)
		{
		}
	}

	public bool StartStory(string storyId, NpcData npcData)
	{
		if (string.IsNullOrEmpty(storyId))
		{
			return false;
		}
		ClearCurStoryData();
		mCurStoryDataList = DataManager.GetStoryDataListById(storyId);
		if (mCurStoryDataList == null)
		{
			return false;
		}
		mCurStoryMissionId = mCurStoryDataList[0].MissionId;
		mCurStoryId = storyId;
		if (npcData != null)
		{
			NpcFakeObjRoot.EnableFakeObjRoot();
			if (!string.IsNullOrEmpty(mCurNpcId) && !mCurNpcId.Equals(npcData.ID))
			{
				NpcFakeObj.DestroyNpcFakeObj();
			}
			NpcFakeObj.InitFakeNpcObj(npcData.Model, NpcFakeObjRoot.MeshRoot);
			mCurNpcId = npcData.ID;
			NpcPic.mainTexture = NpcFakeObjRoot.ModelPic;
		}
		else
		{
			NpcFakeObjRoot.DisableFakeObjRoot();
			NpcFakeObj.DestroyNpcFakeObj();
			NGUITools.SetActive(NpcPic.gameObject, state: false);
			mCurNpcId = string.Empty;
		}
		PlayerFakeObjRoot.EnableFakeObjRoot();
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (PlayerFakeObj.FakeObj == null)
		{
			if (playerData.IsShowFashion)
			{
				if (playerData.CheckWeaponIsSame())
				{
					PlayerFakeObj.InitFakeObject(playerData.FashionWeaponId, playerData.FashionHeadId, playerData.FashionBodyId, playerData.FashionLegId, PlayerFakeObjRoot.MeshRoot, null, "FakeObj2");
				}
				else
				{
					PlayerFakeObj.InitFakeObject(playerData.PartWeaponId, playerData.FashionHeadId, playerData.FashionBodyId, playerData.FashionLegId, PlayerFakeObjRoot.MeshRoot, null, "FakeObj2");
				}
			}
			else
			{
				PlayerFakeObj.InitFakeObject(playerData.PartWeaponId, playerData.PartHeadId, playerData.PartBodyId, playerData.PartLegId, PlayerFakeObjRoot.MeshRoot, null, "FakeObj2");
			}
		}
		else if (playerData.IsShowFashion)
		{
			if (playerData.CheckWeaponIsSame())
			{
				PlayerFakeObj.CheckFakeObject(playerData.FashionWeaponId, playerData.FashionHeadId, playerData.FashionBodyId, playerData.FashionLegId);
			}
			else
			{
				PlayerFakeObj.CheckFakeObject(playerData.PartWeaponId, playerData.FashionHeadId, playerData.FashionBodyId, playerData.FashionLegId);
			}
		}
		else
		{
			PlayerFakeObj.CheckFakeObject(playerData.PartWeaponId, playerData.PartHeadId, playerData.PartBodyId, playerData.PartLegId);
		}
		PlayerPic.mainTexture = PlayerFakeObjRoot.ModelPic;
		PlayStory();
		return true;
	}

	private void PlayStory()
	{
		if (string.IsNullOrEmpty(mCurStoryId) || mCurStoryStep < 0)
		{
			return;
		}
		if (mCurStoryStep < mCurStoryDataList.Count)
		{
			SetSotryPage(mCurStoryDataList[mCurStoryStep].RolePicName, mCurStoryDataList[mCurStoryStep].SpeakerName, mCurStoryDataList[mCurStoryStep].TextInfo);
			return;
		}
		bool flag = false;
		if (!string.IsNullOrEmpty(mCurStoryMissionId))
		{
			MissionData missionDataByID = DataManager.GetMissionDataByID(mCurStoryMissionId);
			if (missionDataByID != null && missionDataByID.MissionLogicType == MISSION_LOGICTYPE.STORY)
			{
				update_misison_complete.request request = new update_misison_complete.request();
				request.missionId = mCurStoryMissionId;
				NetLogic.GetInstance().Send<Protocol.update_misison_complete>(request);
				if (missionDataByID.Submit.Equals(mCurNpcId))
				{
					flag = true;
				}
			}
		}
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.StoryDialogUIRoot);
		if (!flag)
		{
			SingletonUnity<UIManager>.Instance.CheckReShowUI(UIInfo.StoryDialogUIRoot);
		}
		if (UIUpdateEvent.OnStoryShowOver != null)
		{
			UIUpdateEvent.OnStoryShowOver(mCurStoryId);
		}
		ClearCurStoryData();
		Singleton<DialogManager>.Instance.OnCloseDialog();
	}

	private void SetSotryPage(string rolePicName, string speakerName, string textInfo)
	{
		if (speakerName.Equals("&"))
		{
			RoleNameLabel.text = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.MainPlayerAttrData.Name;
			NGUITools.SetActive(PlayerPic.gameObject, state: true);
			NGUITools.SetActive(NpcPic.gameObject, state: false);
		}
		else
		{
			RoleNameLabel.text = speakerName;
			NGUITools.SetActive(PlayerPic.gameObject, state: false);
			NGUITools.SetActive(NpcPic.gameObject, state: true);
		}
		TextLabel.text = StrDictionary.GetDictionaryString(textInfo);
	}

	private void MoveNext()
	{
		mCurStoryStep++;
		PlayStory();
	}

	public void OnClickContinueBtn()
	{
		MoveNext();
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
	}

	protected override void OnDestroy()
	{
		NpcFakeObj.DestroyNpcFakeObj();
		PlayerFakeObj.DestroyFakeObj();
		base.OnDestroy();
	}
}
