using System.Collections.Generic;
using Sproto;
using SprotoType;
using UnityEngine;

public class ModelRankRootLogic : MonoBehaviour
{
	public RANK_TYPE CurRankType;

	public UIEventListener RotateModelBtnListener;

	private FakeObjLogic mCurFakeObj;

	public UITexture ModelPic;

	private PROFESSION_TYPE modelProfession = PROFESSION_TYPE.INVALID;

	private List<sort_item> mCurRankList;

	public UILabel SelfRankLabel;

	public UILabel SelfNameLabel;

	public UILabel SelfValLabel;

	public UILabel rankInfoLabel;

	public UILabel VocationLabel;

	public UISprite VocationFlag;

	public UISprite ModelProfessionFlag;

	public UILabel ModelNameLabel;

	private bool mInitFlag;

	public List<RankItemLogic> RankItemList;

	public UISprite selectitemPic;

	private int mCurRankNum;

	private int mCurRankObjIndex;

	public UILabel PageInfoLabel;

	private int curPagenum;

	private int MaxPageNum = 10;

	public UIScrollView RankScrollView;

	private string curValuename;

	public UITexture CarModelPic;

	public Transform CarMeshRoot;

	private character_look currentLook;

	private void Start()
	{
		if (!mInitFlag)
		{
			for (int i = 0; i < RankItemList.Count; i++)
			{
				RankItemList[i].Init(OnClickRankNumBtn, i);
			}
			mInitFlag = true;
		}
	}

	public void Reset(List<sort_item> ranklist, RANK_TYPE curtype)
	{
		mCurRankList = ranklist;
		if (curtype == RANK_TYPE.CAR)
		{
			ResetFakeCarObjRoot();
		}
		else
		{
			ResetFakeObjRoot();
		}
		if (ranklist.Count % 10 != 0)
		{
			MaxPageNum = ranklist.Count / 10 + 1;
		}
		else
		{
			MaxPageNum = ranklist.Count / 10;
			if (MaxPageNum == 0)
			{
				MaxPageNum = 1;
			}
		}
		CurRankType = curtype;
		curPagenum = 1;
		SelfInfoShow();
		mCurRankNum = -1;
		modelProfession = PROFESSION_TYPE.INVALID;
		PageInfoLabel.text = curPagenum + "/" + MaxPageNum;
		ResetItemList((curPagenum - 1) * 10);
		OnClickRankNum(0, 0);
		RankScrollView.ResetPosition();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Rank", $"rank_{(int)curtype}", "opentimes");
	}

	public void ResetItemList(int startRanknum)
	{
		for (int i = 0; i < RankItemList.Count; i++)
		{
			if (startRanknum + i < mCurRankList.Count)
			{
				RankItemList[i].Reset(startRanknum + i, mCurRankList[startRanknum + i], CurRankType);
				UnityVersionUtil.SetActiveRecursive(RankItemList[i].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(RankItemList[i].gameObject, state: false);
			}
		}
	}

	public void OnClickLeftpageBtn()
	{
		if (curPagenum > 1)
		{
			curPagenum--;
			PageInfoLabel.text = curPagenum + "/" + MaxPageNum;
			ResetItemList((curPagenum - 1) * 10);
			RankScrollView.ResetPosition();
			RankItemList[0].OnClickBtn();
		}
	}

	public void OnClickRightpageBtn()
	{
		if (curPagenum < MaxPageNum)
		{
			curPagenum++;
			PageInfoLabel.text = curPagenum + "/" + MaxPageNum;
			ResetItemList((curPagenum - 1) * 10);
			RankScrollView.ResetPosition();
			RankItemList[0].OnClickBtn();
		}
	}

	public void OnClickRankNumBtn(int clickNum, int objIndex)
	{
		OnClickRankNum(clickNum, objIndex);
	}

	public void OnClickHitOtherPlayer(sort_item item)
	{
		if (currentLook != null && item.id != Singleton<ObjManager>.Instance.MainPlayer.ServerId)
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			TargetBasicInfo selectTargetBasicInfo = playerData.SelectTargetBasicInfo;
			selectTargetBasicInfo.ResetInfo(item.id, (int)currentLook.attribute_other.level, (int)currentLook.attribute_other.combValue, currentLook.general.name, (PROFESSION_TYPE)currentLook.general.profession, 0, currentLook.attribute_other.guildId, currentLook.attribute_other.guildName, UICamera.currentTouch.pos);
			HitOtherPLayerLogic.ShowMenu(HitType.HitTop, selectTargetBasicInfo);
		}
	}

	public void OnClickRankNum(int rankNum, int itemIndex)
	{
		if (mCurRankNum == rankNum)
		{
			OnClickHitOtherPlayer(mCurRankList[rankNum]);
			return;
		}
		currentLook = null;
		if (rankNum >= mCurRankList.Count)
		{
			ModelProfessionFlag.enabled = false;
			ModelNameLabel.text = string.Empty;
			return;
		}
		mCurRankNum = rankNum;
		mCurRankObjIndex = itemIndex;
		selectitemPic.transform.parent = RankItemList[mCurRankObjIndex].transform;
		selectitemPic.transform.localPosition = Vector3.zero;
		if (!UnityVersionUtil.IsActive(selectitemPic.gameObject))
		{
			UnityVersionUtil.SetActiveRecursive(selectitemPic.gameObject, state: true);
		}
		if (CurRankType == RANK_TYPE.CAR)
		{
			ModelProfessionFlag.enabled = false;
			string[] array = mCurRankList[rankNum].name.Split('#');
			MountData mountDataById = DataManager.GetMountDataById(array[1]);
			ModelNameLabel.text = StrDictionary.GetDictionaryString(mountDataById.CarName);
			ColorData datacolor = null;
			if (array.Length > 2)
			{
				datacolor = DataManager.GetColorDataById(array[2]);
			}
			ResetCarModelVisual(mountDataById, datacolor);
		}
		else if (CurRankType == RANK_TYPE.LADDER || CurRankType == RANK_TYPE.SEX)
		{
			string[] array2 = mCurRankList[rankNum].name.Split('#');
			ModelProfessionFlag.enabled = true;
			ModelProfessionFlag.spriteName = GameDefine.Profession_PicName[mCurRankList[rankNum].profession];
			ModelNameLabel.text = array2[0];
			ask_character_info.request request = new ask_character_info.request();
			request.characterId = mCurRankList[rankNum].id;
			NetLogic.GetInstance().Send<Protocol.ask_character_info>(request, RetAskCharacterinfo);
		}
		else
		{
			ModelProfessionFlag.enabled = true;
			ModelProfessionFlag.spriteName = GameDefine.Profession_PicName[mCurRankList[rankNum].profession];
			ModelNameLabel.text = mCurRankList[rankNum].name;
			ask_character_info.request request2 = new ask_character_info.request();
			request2.characterId = mCurRankList[rankNum].id;
			NetLogic.GetInstance().Send<Protocol.ask_character_info>(request2, RetAskCharacterinfo);
		}
	}

	private void RetAskCharacterinfo(SprotoTypeBase req)
	{
		if (!(this == null) && UnityVersionUtil.IsActive(base.gameObject) && req is ask_character_info.response { HasCharacter: not false } response)
		{
			currentLook = response.character;
			character_look character = response.character;
			ResetModelVisual(character);
		}
	}

	private void ResetFakeObjRoot()
	{
		RotateModelBtnListener.onDrag = OnDragModelBtn;
		FakeObjRootLogic instance = SingletonUnity<FakeObjRootLogic>.Instance;
		if (instance == null)
		{
			ResourcesManager.LoadAndInstantiate("UIRoot/FakeObjRoot");
			instance = SingletonUnity<FakeObjRootLogic>.Instance;
		}
		SingletonUnity<FakeObjRootLogic>.Instance.SetPicValue(0.65f);
		SingletonUnity<FakeObjRootLogic>.Instance.EnableFakeObjRoot();
		ModelPic.mainTexture = instance.ModelPic;
		if (mCurRankList.Count > 0)
		{
			ModelPic.enabled = true;
		}
		else
		{
			ModelPic.enabled = false;
		}
		CarModelPic.enabled = false;
	}

	private void ResetFakeCarObjRoot()
	{
		RotateModelBtnListener.onDrag = OnDragCarModelPic;
		FakeCarObjRootLogic instance = SingletonUnity<FakeCarObjRootLogic>.Instance;
		if (instance == null)
		{
			ResourcesManager.LoadAndInstantiate("UIRoot/FakeCarObjRoot");
			instance = SingletonUnity<FakeCarObjRootLogic>.Instance;
		}
		CarMeshRoot = instance.MeshRoot;
		SingletonUnity<FakeCarObjRootLogic>.Instance.EnableFakeObjRoot();
		CarModelPic.mainTexture = instance.ModelPic;
		ModelPic.enabled = false;
		CarModelPic.enabled = true;
	}

	private void ResetCarModelVisual(MountData mountData, ColorData datacolor)
	{
		SingletonUnity<FakeCarObjRootLogic>.Instance.InitFakeCarObj(mountData, datacolor);
	}

	private void ResetModelVisual(character_look CurCharacterLook)
	{
		PROFESSION_TYPE pROFESSION_TYPE = (PROFESSION_TYPE)CurCharacterLook.general.profession;
		string modeName = ServerToClientTools.GetModeName(CurCharacterLook.visual.HeadId);
		if (pROFESSION_TYPE != modelProfession)
		{
			modelProfession = pROFESSION_TYPE;
			if (mCurFakeObj != null)
			{
				mCurFakeObj.DestroyFakeObj();
			}
		}
		if (mCurFakeObj == null || mCurFakeObj.FakeObj == null)
		{
			mCurFakeObj = new FakeObjLogic();
			mCurFakeObj.InitFakeObject(CurCharacterLook.visual, pROFESSION_TYPE.ToString(), modeName, SingletonUnity<FakeObjRootLogic>.Instance.MeshRoot);
		}
		else
		{
			mCurFakeObj.CheckFakeObject(CurCharacterLook.visual);
			mCurFakeObj.PlayAnim("idle", modeName);
		}
	}

	private void OnDragModelBtn(GameObject btn, Vector2 delta)
	{
		if (mCurFakeObj != null && mCurFakeObj.FakeObj != null)
		{
			mCurFakeObj.FakeObj.transform.localEulerAngles -= delta.x * Vector3.up;
		}
	}

	public void OnDragCarModelPic(GameObject btn, Vector2 delta)
	{
		if (CarMeshRoot != null)
		{
			CarMeshRoot.transform.localEulerAngles -= new Vector3(0f, delta.x, 0f);
		}
	}

	public void SelfInfoShow()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		switch (CurRankType)
		{
		case RANK_TYPE.FIGHT:
			curValuename = StrDictionary.GetDictionaryString("#{101315}");
			VocationLabel.text = StrDictionary.GetDictionaryString("#{101311}");
			break;
		case RANK_TYPE.LEVEL:
			curValuename = StrDictionary.GetDictionaryString("#{101314}");
			VocationLabel.text = StrDictionary.GetDictionaryString("#{101311}");
			break;
		case RANK_TYPE.LADDER:
			curValuename = StrDictionary.GetDictionaryString("#{101315}");
			VocationLabel.text = StrDictionary.GetDictionaryString("#{101311}");
			break;
		case RANK_TYPE.TOWER:
			curValuename = StrDictionary.GetDictionaryString("#{101317}");
			VocationLabel.text = StrDictionary.GetDictionaryString("#{101311}");
			break;
		case RANK_TYPE.CAR:
			curValuename = StrDictionary.GetDictionaryString("#{101319}");
			VocationLabel.text = StrDictionary.GetDictionaryString("#{101568}");
			break;
		case RANK_TYPE.SEX:
			curValuename = StrDictionary.GetDictionaryString("#{100609}");
			VocationLabel.text = StrDictionary.GetDictionaryString("#{101311}");
			break;
		}
		rankInfoLabel.text = curValuename;
		SelfRankLabel.text = string.Format("{0}", StrDictionary.GetDictionaryString("#{101322}"));
		SelfValLabel.enabled = false;
		SelfNameLabel.enabled = false;
		for (int i = 0; i < mCurRankList.Count; i++)
		{
			if (mCurRankList[i].id != PlayerData.MainPlayerServerId)
			{
				continue;
			}
			if (CurRankType == RANK_TYPE.LADDER)
			{
				string[] array = mCurRankList[i].name.Split('#');
				if (array.Length > 1)
				{
					SelfValLabel.text = $"{curValuename}:{array[1]}";
				}
			}
			else if (CurRankType == RANK_TYPE.CAR)
			{
				SelfValLabel.text = $"{curValuename}:{TimeTools.GetCentiSecondStr((int)(GameDefine.DayCentiSecond - (mCurRankList[i].score >> 32)))}";
			}
			else if (CurRankType == RANK_TYPE.TOWER)
			{
				SelfValLabel.text = $"{curValuename}:{(mCurRankList[i].score >> 32) + 1}";
			}
			else
			{
				SelfValLabel.text = $"{curValuename}:{mCurRankList[i].score >> 32}";
			}
			SelfRankLabel.text = string.Format("{0}:{1}", StrDictionary.GetDictionaryString("#{101309}"), i + 1);
			SelfNameLabel.text = playerData.MainPlayerAttrData.Name;
			SelfValLabel.enabled = true;
			SelfNameLabel.enabled = true;
			break;
		}
	}

	public void UnLoadFakeObj()
	{
		if (mCurFakeObj != null)
		{
			mCurFakeObj.DestroyFakeObj();
			mCurFakeObj = null;
		}
		if (SingletonUnity<FakeObjRootLogic>.Instance != null)
		{
			SingletonUnity<FakeObjRootLogic>.Instance.DisableFakeObjRoot();
		}
		if (SingletonUnity<FakeCarObjRootLogic>.Instance != null)
		{
			SingletonUnity<FakeCarObjRootLogic>.Instance.DisableFakeObjRoot();
		}
	}
}
