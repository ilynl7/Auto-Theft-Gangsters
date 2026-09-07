using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sproto;
using SprotoType;
using UnityEngine;

public class CreateRoleRootLogic : SingletonUnity<CreateRoleRootLogic>
{
	public UILabel ProfessionLabel;

	public UIInput NameInput;

	public MyRotateScrollView RotateScrollView;

	public UIEventListener RotateModelBtn;

	public CreateEquilateralPic InsideAttrPic;

	private Transform mModelRoot;

	private FakeObjLogic[] mFakeObjList = new FakeObjLogic[3];

	private int mCurRoleIndex;

	private GameObject mCurRole;

	private int NameMinLength = 5;

	private int NameMaxLength = 16;

	public UILabel NameTipsLabel;

	private string[][] mDefaultModel = new string[3][]
	{
		new string[4] { "XD_A_WQ", "XD_A_T", "XD_A_S", "XD_A_X" },
		new string[4] { "QJ_A_WQ", "QJ_A_T", "QJ_A_S", "QJ_A_X" },
		new string[4] { "NQS_A_WQ", "NQS_A_T", "NQS_A_S", "NQS_A_X" }
	};

	private CharacterModelData[] mModelData = new CharacterModelData[3];

	private string[] mProfessionName = new string[3] { "#{100128}", "#{100129}", "#{100130}" };

	private float[][] mAttrPicPos = new float[3][]
	{
		new float[6] { 0.7f, 0.8f, 0.8f, 0.6f, 0.7f, 0.7f },
		new float[6] { 0.7f, 0.9f, 0.6f, 0.9f, 0.65f, 0.7f },
		new float[6] { 0.75f, 0.6f, 0.9f, 0.6f, 0.75f, 0.7f }
	};

	private bool NewAccountFlag;

	private Material mXRayMat;

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
		RotateScrollView.onMoveOver = OnChangeRole;
		mModelRoot = GameObject.Find("PlayerModelRoot").transform;
		if (mModelRoot == null)
		{
			Debug.Log("mModelRoot == null");
		}
		mModelData[0] = DataManager.GetCharacterModelDataByID("100");
		mModelData[1] = DataManager.GetCharacterModelDataByID("104");
		mModelData[2] = DataManager.GetCharacterModelDataByID("105");
		RotateModelBtn.onDrag = OnDragModel;
		NameTipsLabel.text = StrDictionary.GetDictionaryString("#{200056}", NameMinLength, NameMaxLength);
	}

	public void Reset(bool isNewAccount)
	{
		NewAccountFlag = isNewAccount;
		OnChangeRole(mCurRoleIndex);
	}

	private void OnChangeRole(int roleIndex)
	{
		SetChangeRoleEnable(isEnable: false);
		EnableRoleModel(roleIndex);
		InsideAttrPic.UpdatePos(mAttrPicPos[roleIndex]);
	}

	private void SetChangeRoleEnable(bool isEnable)
	{
		RotateScrollView.EnableChangeFlag = true;
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

	private void EnableRoleModel(int index)
	{
		mCurRoleIndex = index;
		for (int i = 0; i < mFakeObjList.Length; i++)
		{
			if (i == index)
			{
				if (mFakeObjList[i] != null && mFakeObjList[i].FakeObj != null)
				{
					mFakeObjList[i].FakeObj.transform.localEulerAngles = Vector3.zero;
					UnityVersionUtil.SetActiveRecursive(mFakeObjList[i].FakeObj.gameObject, state: true);
					mFakeObjList[i].PlayAnim(GameDefine.ShowSelectAnimaName, mModelData[i]);
					SetChangeRoleEnable(isEnable: true);
					ResetCurModelShadow(mFakeObjList[i]);
				}
				else
				{
					if (mFakeObjList[i] == null)
					{
						mFakeObjList[i] = new FakeObjLogic();
					}
					mFakeObjList[i].InitFakeObject(mDefaultModel[i][0], mDefaultModel[i][1], mDefaultModel[i][2], mDefaultModel[i][3], i, mModelRoot, OnLoadModelFinish, "ShadowCaster");
				}
			}
			else if (mFakeObjList[i] != null && mFakeObjList[i].FakeObj != null && UnityVersionUtil.IsActive(mFakeObjList[i].FakeObj))
			{
				UnityVersionUtil.SetActiveRecursive(mFakeObjList[i].FakeObj, state: false);
			}
		}
		ProfessionLabel.text = StrDictionary.GetDictionaryString(mProfessionName[mCurRoleIndex]);
		CharacterGetRandomName();
	}

	private void OnLoadModelFinish(FakeObjLogic obj)
	{
		if (obj == mFakeObjList[mCurRoleIndex])
		{
			mFakeObjList[mCurRoleIndex].PlayAnim(GameDefine.ShowSelectAnimaName, mModelData[mCurRoleIndex]);
			SetChangeRoleEnable(isEnable: true);
			ResetCurModelShadow(mFakeObjList[mCurRoleIndex]);
		}
	}

	public void OnClickRandomNameBtn()
	{
		CharacterGetRandomName();
	}

	public void OnClickCreateBtn()
	{
		if (!string.IsNullOrEmpty(NameInput.value))
		{
			NameInput.value = NameInput.value.Trim();
		}
		if (!string.IsNullOrEmpty(NameInput.value))
		{
			if (NameInput.value.Length < NameMinLength || NameInput.value.Length > NameMaxLength)
			{
				MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{200060}", NameMinLength, NameMaxLength), "#{100127}");
				return;
			}
			if (!CheckNameIsRight(NameInput.value))
			{
				MessageBoxLogic.OpenOKBox("#{200058}", "#{100127}");
				return;
			}
			WaitResponseUIRootLogic.OpenWaitBox(104, 0f, 0f);
			character_create.request request = new character_create.request();
			general general = new general();
			general.name = NameInput.value;
			if (mCurRoleIndex == 0)
			{
				general.profession = 0L;
			}
			else if (mCurRoleIndex == 1)
			{
				general.profession = 1L;
			}
			else
			{
				general.profession = 2L;
			}
			request.character = general;
			NetLogic.GetInstance().Send<Protocol.character_create>(request, CharacterCreateResponse);
		}
		else
		{
			MessageBoxLogic.OpenOKBox("#{200057}", "#{100127}");
		}
	}

	private bool CheckNameIsRight(string namestr)
	{
		string pattern = "^[a-zA-Z0-9]{1}([a-zA-Z0-9]|[ _]){4,15}$";
		if (Regex.IsMatch(namestr, pattern))
		{
			return true;
		}
		return false;
	}

	private void CharacterCreateResponse(SprotoTypeBase req)
	{
		if (!(req is character_create.response response))
		{
			return;
		}
		if (response.errno == 0L)
		{
			if (SingletonUnity<ChooseRoleRootLogic>.Exists)
			{
				LocalDataSaveManager.SetChooseRoleIndex(SingletonUnity<ChooseRoleRootLogic>.Instance.RoleCount);
			}
			else
			{
				LocalDataSaveManager.SetChooseRoleIndex(0);
			}
			DisactiveCurCreateRole();
			if (response.HasCharacter)
			{
				if (response.character.general.profession == 0L)
				{
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("CharacterEvent", "Profession", "XD");
				}
				else if (response.character.general.profession == 1)
				{
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("CharacterEvent", "Profession", "QJ");
				}
				else if (response.character.general.profession == 2)
				{
					SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("CharacterEvent", "Profession", "NQS");
				}
				PlayerData.MainPlayerServerId = response.character.id;
				GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
				PlayerData playerData = instance.PlayerData;
				PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
				playerCommonData.CreateTime = response.character.createtime;
				playerData.Level = (int)response.character.attribute_other.level;
				playerData.IsTutorialFinish = false;
				WaitResponseUIRootLogic.OpenWaitBox(105, 0f, 0f);
				character_pick.request request = new character_pick.request();
				request.id = PlayerData.MainPlayerServerId;
				NetLogic.GetInstance().Send<Protocol.character_pick>(request, SingletonDontDestoryUnity<NetManager>.Instance.PickResponse);
				instance.FirstEnterGame = true;
				instance.IsShowMainMissionTip = true;
				instance.PlayerData.ViewType = LocalDataSaveManager.GetCameraViewType(PlayerData.MainPlayerServerId);
			}
		}
		else if (response.errno == 1)
		{
			WaitResponseUIRootLogic.CloseBox();
			NoticeLogic.AddNotifyData("#{100150}");
		}
		else if (response.errno == 2)
		{
			WaitResponseUIRootLogic.CloseBox();
			NoticeLogic.AddNotifyData("#{100151}");
		}
		else if (response.errno == 3)
		{
			WaitResponseUIRootLogic.CloseBox();
			NoticeLogic.AddNotifyData("#{100152}");
		}
	}

	public void OnClickBackBtn()
	{
		DisactiveCurCreateRole();
		if (SingletonUnity<ChooseRoleRootLogic>.Exists)
		{
			if (NewAccountFlag)
			{
				SingletonUnity<MenuSceneController>.Instance.ShowChooseServer();
			}
			else
			{
				SingletonUnity<MenuSceneController>.Instance.ShowChooseRole(null);
			}
		}
		else
		{
			SingletonUnity<MenuSceneController>.Instance.ShowChooseServer();
		}
	}

	public void DisactiveCurCreateRole()
	{
		if (mFakeObjList != null && mFakeObjList[mCurRoleIndex] != null && mFakeObjList[mCurRoleIndex].FakeObj != null)
		{
			UnityVersionUtil.SetActiveRecursive(mFakeObjList[mCurRoleIndex].FakeObj, state: false);
		}
	}

	private void OnDisable()
	{
		DisactiveCurCreateRole();
	}

	private void CharacterGetRandomName()
	{
		long type = 0L;
		if (mCurRoleIndex == 2)
		{
			type = 1L;
		}
		request_random_name.request request = new request_random_name.request();
		request.type = type;
		NetLogic.GetInstance().Send<Protocol.request_random_name>(request, CharacterGetRandomNameResponse);
	}

	private void CharacterGetRandomNameResponse(SprotoTypeBase req)
	{
		if (req is request_random_name.response response)
		{
			NameInput.value = response.name;
		}
	}

	public void OnDragModel(GameObject obj, Vector2 delta)
	{
		mFakeObjList[mCurRoleIndex].FakeObj.transform.Rotate(new Vector3(0f, 0f - delta.x, 0f));
	}

	private void Update()
	{
		if (mFakeObjList[mCurRoleIndex] != null && mFakeObjList[mCurRoleIndex].FakeObj != null && mFakeObjList[mCurRoleIndex].FakeObj.animation.IsPlaying(mFakeObjList[mCurRoleIndex].GetAnimaStateName(GameDefine.ShowSelectAnimaName)) && mFakeObjList[mCurRoleIndex].FakeObj.animation[mFakeObjList[mCurRoleIndex].GetAnimaStateName(GameDefine.ShowSelectAnimaName)].normalizedTime >= 0.8f)
		{
			mFakeObjList[mCurRoleIndex].CrossFadeIdelSelect(mCurRoleIndex, mModelData[mCurRoleIndex].IndexName);
		}
	}
}
