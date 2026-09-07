using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ChooseRoleRootLogic : SingletonUnity<ChooseRoleRootLogic>
{
	public ChooseRoleLine[] ChooseRoleLine;

	public UILabel LevelLabel;

	public UILabel FightingLabel;

	public UILabel ProfessionLabel;

	public GameObject ChooseLineObj;

	public int RoleCount;

	private int mCurChooseRoleIndex;

	private Transform mModelRoot;

	private List<character_overview> mPlayerRoleList;

	private float mWaitTimeCount;

	private string mChoosedLinePicName = "CZ_renWubg_2";

	private string mUnChoosedLinePicName = "CZ_renWubg_1";

	private FakeObjLogic[] mFakeObjList = new FakeObjLogic[4];

	private CharacterModelData[] mCharacterModelData = new CharacterModelData[4];

	private float mShowTimeInterval = 10f;

	public TweenPosition[] RoleLineTwPos;

	public UIEventListener BottomRotateBtn;

	private Dictionary<long, character_overview> mPlayerInfoDic;

	public UILabel IdLabel;

	public UILabel Acclabel;

	private Material mXRayMat;

	private int test;

	public Material XRayMat
	{
		get
		{
			if (mXRayMat == null)
			{
				mXRayMat = ResourcesManager.Load("Material/XRay") as Material;
			}
			return mXRayMat;
		}
		set
		{
			mXRayMat = value;
		}
	}

	private new void Awake()
	{
		base.Awake();
		mModelRoot = GameObject.Find("PlayerModelRoot").transform;
		BottomRotateBtn.onDrag = OnDragModel;
	}

	public void Reset(character_list.response characterList)
	{
		for (int i = 0; i < RoleLineTwPos.Length; i++)
		{
			RoleLineTwPos[i].ResetToBeginning();
			RoleLineTwPos[i].PlayForward();
		}
		Acclabel.text = $"Account:{PlayerData.GetPlayerAccountId()}";
		if (characterList == null)
		{
			mCurChooseRoleIndex = 0;
			ChooseRole(mCurChooseRoleIndex);
			return;
		}
		mPlayerInfoDic = characterList.character;
		if (characterList.character.Count <= 0)
		{
			return;
		}
		mCurChooseRoleIndex = LocalDataSaveManager.GetChooseRoleIndex();
		if (mCurChooseRoleIndex >= characterList.character.Count)
		{
			mCurChooseRoleIndex = 0;
		}
		mPlayerRoleList = new List<character_overview>(characterList.character.Values);
		mPlayerRoleList.Sort(delegate(character_overview left, character_overview right)
		{
			if (left.createtime < right.createtime)
			{
				return -1;
			}
			return (left.createtime > right.createtime) ? 1 : 0;
		});
		RoleCount = mPlayerRoleList.Count;
		for (int j = 0; j < ChooseRoleLine.Length; j++)
		{
			if (j < mPlayerRoleList.Count)
			{
				long profession = mPlayerRoleList[j].general.profession;
				if (profession < 0 || profession > 2)
				{
					goto IL_0197;
				}
				switch (profession)
				{
				case 0L:
					break;
				case 1L:
					goto IL_0169;
				case 2L:
					goto IL_0180;
				default:
					goto IL_0197;
				}
				mCharacterModelData[j] = DataManager.GetCharacterModelDataByID("100");
				goto IL_01ae;
			}
			ResetRoleLine(null, ChooseRoleLine[j]);
			continue;
			IL_0169:
			mCharacterModelData[j] = DataManager.GetCharacterModelDataByID("104");
			goto IL_01ae;
			IL_0197:
			mCharacterModelData[j] = DataManager.GetCharacterModelDataByID("100");
			goto IL_01ae;
			IL_01ae:
			ResetRoleLine(mPlayerRoleList[j], ChooseRoleLine[j]);
			if (mCurChooseRoleIndex == j)
			{
				InitModel(j);
			}
			continue;
			IL_0180:
			mCharacterModelData[j] = DataManager.GetCharacterModelDataByID("105");
			goto IL_01ae;
		}
	}

	public void InitModel(int i)
	{
		if (mFakeObjList[i] != null && mFakeObjList[i].FakeObj != null)
		{
			mFakeObjList[i].CheckFakeObject(mPlayerRoleList[i].visual);
			ChooseRole(mCurChooseRoleIndex);
			return;
		}
		if (mFakeObjList[i] == null)
		{
			mFakeObjList[i] = new FakeObjLogic();
		}
		mFakeObjList[i].InitFakeObject(mPlayerRoleList[i].visual, (int)mPlayerRoleList[i].general.profession, mModelRoot, OnLoadModelFinish, "ShadowCaster");
	}

	private void ResetCurModelShadow(FakeObjLogic obj)
	{
		if (!GameSettingData.IsShowPlayerShadow[GameSettingData.GetPhoneClass()])
		{
			return;
		}
		Material[] array = null;
		SkinnedMeshRenderer skinnedMeshRenderer = null;
		List<GameObject> bodyPartList = obj.GetBodyPartList();
		for (int i = 0; i < bodyPartList.Count; i++)
		{
			skinnedMeshRenderer = bodyPartList[i].GetComponent<SkinnedMeshRenderer>();
			array = new Material[skinnedMeshRenderer.materials.Length + 1];
			bool flag = false;
			for (int j = 0; j < skinnedMeshRenderer.materials.Length; j++)
			{
				if (skinnedMeshRenderer.materials[j].name.Contains("XRay"))
				{
					flag = true;
					break;
				}
				array[j] = skinnedMeshRenderer.materials[j];
			}
			if (!flag)
			{
				array[skinnedMeshRenderer.materials.Length] = XRayMat;
				skinnedMeshRenderer.materials = array;
			}
		}
		if (UnityVersionUtil.IsActive(obj.FakeObj.gameObject))
		{
			if (!SingletonUnity<RealTimeShadow>.Exists)
			{
				GameObject gameObject = ResourcesManager.LoadAndInstantiate("Items/PlayerRealTimeShadow") as GameObject;
				RealTimeShadow component = gameObject.GetComponent<RealTimeShadow>();
				component.Reset(bodyPartList);
			}
			else
			{
				SingletonUnity<RealTimeShadow>.Instance.Reset(bodyPartList);
			}
		}
	}

	private void OnLoadModelFinish(FakeObjLogic obj)
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			ChooseRole(mCurChooseRoleIndex);
		}
	}

	private void ChooseRole(int index)
	{
		mCurChooseRoleIndex = index;
		ResetRole();
		if (mPlayerRoleList.Count > index)
		{
			ChooseRoleLine[mCurChooseRoleIndex].BottomLinePic.spriteName = mChoosedLinePicName;
			ChooseLineObj.transform.parent = ChooseRoleLine[mCurChooseRoleIndex].BottomLinePic.transform;
			ChooseLineObj.transform.localPosition = new Vector3(-50f, 0f, 0f);
			if (mFakeObjList[mCurChooseRoleIndex] != null && mFakeObjList[index].FakeObj != null)
			{
				mWaitTimeCount = 0f;
				UnityVersionUtil.SetActiveRecursive(mFakeObjList[mCurChooseRoleIndex].FakeObj.gameObject, state: true);
				mFakeObjList[mCurChooseRoleIndex].ActivePlayerModelObj();
				mFakeObjList[mCurChooseRoleIndex].FakeObj.transform.localEulerAngles = Vector3.zero;
				mFakeObjList[mCurChooseRoleIndex].PlayAnim(GameDefine.ShowSelectAnimaName, mCharacterModelData[mCurChooseRoleIndex].IndexName);
				ResetCurModelShadow(mFakeObjList[mCurChooseRoleIndex]);
			}
			else
			{
				InitModel(mCurChooseRoleIndex);
			}
			IdLabel.text = $"ID:{mPlayerRoleList[mCurChooseRoleIndex].id}";
			LevelLabel.text = $"Lv.{mPlayerRoleList[mCurChooseRoleIndex].attribute_other.level}";
			FightingLabel.text = mPlayerRoleList[mCurChooseRoleIndex].attribute_other.combValue.ToString();
			ProfessionLabel.text = mPlayerRoleList[mCurChooseRoleIndex].general.name;
		}
		else
		{
			SingletonUnity<MenuSceneController>.Instance.ShowCreateRole(isNewAccount: false);
		}
	}

	private void ResetRole()
	{
		for (int i = 0; i < ChooseRoleLine.Length; i++)
		{
			if (mFakeObjList[i] != null && mFakeObjList[i].FakeObj != null && UnityVersionUtil.IsActive(mFakeObjList[i].FakeObj.gameObject))
			{
				UnityVersionUtil.SetActiveRecursive(mFakeObjList[i].FakeObj.gameObject, state: false);
			}
			ChooseRoleLine[i].BottomLinePic.spriteName = mUnChoosedLinePicName;
		}
	}

	private void ResetRoleLine(character_overview chaInfo, ChooseRoleLine roleLine)
	{
		if (chaInfo != null)
		{
			roleLine.roleId = chaInfo.id;
			roleLine.RolePic.spriteName = GameDefine.Player_Icon_Pic[chaInfo.general.profession];
			roleLine.NameLabel.text = chaInfo.general.name;
			roleLine.BottomLinePic.spriteName = mUnChoosedLinePicName;
		}
		else
		{
			roleLine.RolePic.spriteName = "CZ_touXiangTianjia";
			roleLine.NameLabel.text = string.Empty;
			roleLine.BottomLinePic.spriteName = mUnChoosedLinePicName;
		}
	}

	public void OnClickRoleBtn0()
	{
		ChooseRole(0);
	}

	public void OnClickRoleBtn1()
	{
		ChooseRole(1);
	}

	public void OnClickRoleBtn2()
	{
		ChooseRole(2);
	}

	public void OnClickRoleBtn3()
	{
		ChooseRole(3);
	}

	public void OnClickBackBtn()
	{
		for (int i = 0; i < mFakeObjList.Length; i++)
		{
			if (mFakeObjList[i] != null)
			{
				mFakeObjList[i].DestroyFakeObj();
			}
		}
		RoleCount = 0;
		SingletonUnity<MenuSceneController>.Instance.ShowChooseServer();
	}

	public void OnClickPlayBtn()
	{
		LocalDataSaveManager.SetChooseRoleIndex(mCurChooseRoleIndex);
		PlayerData.MainPlayerServerId = ChooseRoleLine[mCurChooseRoleIndex].roleId;
		GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
		character_overview character_overview = mPlayerInfoDic[PlayerData.MainPlayerServerId];
		if (character_overview.HasForbidden && character_overview.forbidden == 1)
		{
			MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{201011}"), StrDictionary.GetDictionaryString("#{200005}"));
		}
		PlayerData playerData = instance.PlayerData;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		playerCommonData.CreateTime = mPlayerInfoDic[PlayerData.MainPlayerServerId].createtime;
		playerData.Level = (int)mPlayerInfoDic[PlayerData.MainPlayerServerId].attribute_other.level;
		playerData.IsTutorialFinish = mPlayerInfoDic[PlayerData.MainPlayerServerId].general.HasTutorial && mPlayerInfoDic[PlayerData.MainPlayerServerId].general.tutorial == 1;
		WaitResponseUIRootLogic.OpenWaitBox(105, 0f, 0f);
		character_pick.request request = new character_pick.request();
		request.id = PlayerData.MainPlayerServerId;
		NetLogic.GetInstance().Send<Protocol.character_pick>(request, SingletonDontDestoryUnity<NetManager>.Instance.PickResponse);
		instance.FirstEnterGame = true;
		instance.IsShowMainMissionTip = true;
		instance.PlayerData.ViewType = LocalDataSaveManager.GetCameraViewType(PlayerData.MainPlayerServerId);
	}

	public void OnDragModel(GameObject obj, Vector2 delta)
	{
		mFakeObjList[mCurChooseRoleIndex].FakeObj.transform.Rotate(new Vector3(0f, 0f - delta.x, 0f));
	}

	private void Update()
	{
		mWaitTimeCount += Time.deltaTime;
		if (mWaitTimeCount >= mShowTimeInterval)
		{
			mWaitTimeCount = 0f;
			mFakeObjList[mCurChooseRoleIndex].CrossFadeAnima(GameDefine.ShowSelectAnimaName, mCharacterModelData[mCurChooseRoleIndex].IndexName);
		}
		if (mFakeObjList[mCurChooseRoleIndex] != null && mFakeObjList[mCurChooseRoleIndex].FakeObj != null && mFakeObjList[mCurChooseRoleIndex].FakeObj.animation.IsPlaying(mFakeObjList[mCurChooseRoleIndex].GetAnimaStateName(GameDefine.ShowSelectAnimaName)) && mFakeObjList[mCurChooseRoleIndex].FakeObj.animation[mFakeObjList[mCurChooseRoleIndex].GetAnimaStateName(GameDefine.ShowSelectAnimaName)].normalizedTime >= 0.8f)
		{
			mFakeObjList[mCurChooseRoleIndex].CrossFadeIdelSelect((int)mPlayerRoleList[mCurChooseRoleIndex].general.profession, mCharacterModelData[mCurChooseRoleIndex].IndexName);
		}
	}
}
