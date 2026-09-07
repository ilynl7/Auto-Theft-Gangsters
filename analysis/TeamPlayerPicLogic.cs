using UnityEngine;

public class TeamPlayerPicLogic : MonoBehaviour
{
	public UILabel NameLabel;

	public UILabel LevelLabel;

	public UILabel FightingLabel;

	public UISprite ProfessionPic;

	public UISprite TeamLeaderPic;

	public UISprite ReadyPic;

	public UISprite EmptyPlusPic;

	public UITexture PlayerModelPic;

	public UILabel RestNumLabel;

	public TeamFakeObjPicRootLogic CurFakeObjRoot;

	public FakeObjLogic CurFakeObj;

	private TeamMember mCurPlayerInfo;

	private PROFESSION_TYPE curProfession;

	private bool mIsEmpty;

	private bool mIsLock;

	public bool IsEmpty => mIsEmpty;

	public bool IsLock => mIsLock;

	public void Reset(TeamMember playerInfo, bool isLock = false)
	{
		if (playerInfo == null)
		{
			mIsLock = isLock;
			mIsEmpty = true;
			SetEmpty();
		}
		else
		{
			mIsEmpty = false;
			mIsLock = false;
			SetPlayer(playerInfo);
		}
	}

	private void SetEmpty()
	{
		CurFakeObjRoot.DisableFakeObjRoot();
		PlayerModelPic.enabled = false;
		UnityVersionUtil.SetActiveRecursive(ProfessionPic.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(TeamLeaderPic.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(ReadyPic.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(NameLabel.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(LevelLabel.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(FightingLabel.gameObject, state: false);
		NGUITools.SetActive(RestNumLabel.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(EmptyPlusPic.gameObject, state: true);
		if (mIsLock)
		{
			EmptyPlusPic.spriteName = "CZ_huaDongBG_Suo";
		}
		else
		{
			EmptyPlusPic.spriteName = "CZ_anNiu_tianJiaDuiYou";
		}
	}

	private void SetPlayer(TeamMember playerInfo)
	{
		CurFakeObjRoot.CreateModelPic();
		CurFakeObjRoot.EnableFakeObjRoot();
		PlayerModelPic.mainTexture = CurFakeObjRoot.ModelPic;
		PlayerModelPic.enabled = true;
		if (mCurPlayerInfo != null && curProfession == playerInfo.Profession)
		{
			CurFakeObj.CheckFakeObject(playerInfo.Visual);
		}
		else
		{
			CurFakeObj.DestroyFakeObj();
			CurFakeObj.InitFakeObject(playerInfo.Visual, playerInfo.Profession, CurFakeObjRoot.MeshRoot);
		}
		mCurPlayerInfo = playerInfo;
		curProfession = playerInfo.Profession;
		UnityVersionUtil.SetActiveRecursive(NameLabel.gameObject, state: true);
		NameLabel.text = mCurPlayerInfo.Name;
		UnityVersionUtil.SetActiveRecursive(ProfessionPic.gameObject, state: true);
		ProfessionPic.spriteName = GameDefine.Player_Profession_Pic[(int)mCurPlayerInfo.Profession];
		UnityVersionUtil.SetActiveRecursive(LevelLabel.gameObject, state: true);
		LevelLabel.text = $"Lv.{mCurPlayerInfo.Level}";
		UnityVersionUtil.SetActiveRecursive(FightingLabel.gameObject, state: true);
		FightingLabel.text = $"{mCurPlayerInfo.CombValue}";
		if (mCurPlayerInfo.TeamJob == 0)
		{
			UnityVersionUtil.SetActiveRecursive(TeamLeaderPic.gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(TeamLeaderPic.gameObject, state: false);
		}
		Team teamInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.TeamInfo;
		if (teamInfo.IsCheckingEnterCopy)
		{
			if (playerInfo.IsReadyEnterCopy)
			{
				ReadyPic.spriteName = "CZ_fuBenTuBiao_yiZhunBei";
				ReadyPic.MakePixelPerfect();
				UnityVersionUtil.SetActiveRecursive(ReadyPic.gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(ReadyPic.gameObject, state: false);
			}
		}
		else if (playerInfo.IsRefuseEnterCopy)
		{
			ReadyPic.spriteName = "CZ_fuBenTuBiao_juJue";
			ReadyPic.MakePixelPerfect();
			UnityVersionUtil.SetActiveRecursive(ReadyPic.gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(ReadyPic.gameObject, state: false);
		}
		UnityVersionUtil.SetActiveRecursive(EmptyPlusPic.gameObject, state: false);
		if (teamInfo.TeamGoalData.GoalType == 1)
		{
			if (playerInfo.CopyRestNum == -1)
			{
				NGUITools.SetActive(RestNumLabel.gameObject, state: false);
				return;
			}
			NGUITools.SetActive(RestNumLabel.gameObject, state: true);
			string copyId = teamInfo.TeamGoalData.CopyId;
			CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(copyId);
			RestNumLabel.text = string.Format("{0}:{1}/{2}", StrDictionary.GetDictionaryString("#{100749}"), playerInfo.CopyRestNum, copySceneDataById.MaxPlayNum);
		}
		else
		{
			NGUITools.SetActive(RestNumLabel.gameObject, state: false);
		}
	}

	public void OnClickPic()
	{
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsTeamLeader())
		{
			if (!mIsLock)
			{
				if (mIsEmpty)
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TeamInviteRoot);
				}
				else if (mCurPlayerInfo.ServerId != PlayerData.MainPlayerServerId)
				{
					TargetBasicInfo selectTargetBasicInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SelectTargetBasicInfo;
					selectTargetBasicInfo.ResetInfo(mCurPlayerInfo.ServerId, mCurPlayerInfo.Level, mCurPlayerInfo.CombValue, mCurPlayerInfo.Name, mCurPlayerInfo.Profession, 1, mCurPlayerInfo.GuildId, mCurPlayerInfo.GuildName, UICamera.currentTouch.pos);
					HitOtherPLayerLogic.ShowMenu(HitType.HitTeamMemberIcon, selectTargetBasicInfo);
				}
			}
		}
		else if (!mIsEmpty && mCurPlayerInfo.ServerId != PlayerData.MainPlayerServerId)
		{
			TargetBasicInfo selectTargetBasicInfo2 = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SelectTargetBasicInfo;
			selectTargetBasicInfo2.ResetInfo(mCurPlayerInfo.ServerId, mCurPlayerInfo.Level, mCurPlayerInfo.CombValue, mCurPlayerInfo.Name, mCurPlayerInfo.Profession, 1, mCurPlayerInfo.GuildId, mCurPlayerInfo.GuildName, UICamera.currentTouch.pos);
			HitOtherPLayerLogic.ShowMenu(HitType.HitTeamMemberIcon, selectTargetBasicInfo2);
		}
	}

	private void OnDisable()
	{
		UnLoadFakeObj();
	}

	public void UnLoadFakeObj()
	{
		if (CurFakeObj != null)
		{
			CurFakeObj.DestroyFakeObj();
			CurFakeObj = null;
		}
		else
		{
			Debug.Log("mPlayerModelVisual == null");
		}
		if (CurFakeObjRoot != null)
		{
			CurFakeObjRoot.DisableFakeObjRoot();
		}
	}
}
