using SprotoType;
using UnityEngine;

public class RankPVPPlayerLogic : MonoBehaviour
{
	public delegate void clickfightfun(long id);

	public UIEventListener RotateModelBtnListener;

	public UILabel OtherPlayerNameLabel;

	public UILabel OtherPlayerComboValueLabel;

	public UILabel OtherPlayerLevelLabel;

	public UILabel OtherPlayerRankLabel;

	public UISprite OtherPlayerIconSprite;

	private FakeObjLogic OtherPlayerFakeObjs = new FakeObjLogic();

	private long OtherPlayerIndex;

	private PROFESSION_TYPE mPreProfession = PROFESSION_TYPE.INVALID;

	public UITexture CurModelPic;

	public RankPVPFakeObj CurFakeObjRoot;

	private clickfightfun clickfight;

	private DelegateDefine.NoParamDelegate loadfinish;

	public void UpdateInfo(character_look lookvalue, int rank, clickfightfun clickfun = null, DelegateDefine.NoParamDelegate loadfun = null)
	{
		RotateModelBtnListener.onDrag = OnDragModelBtn;
		clickfight = clickfun;
		loadfinish = loadfun;
		PROFESSION_TYPE pROFESSION_TYPE = (PROFESSION_TYPE)lookvalue.general.profession;
		string empty = string.Empty;
		empty = pROFESSION_TYPE switch
		{
			PROFESSION_TYPE.XD => "baiRen", 
			PROFESSION_TYPE.QJ => "heiRen", 
			_ => "nvRen", 
		};
		if (OtherPlayerFakeObjs != null)
		{
			if (!mPreProfession.Equals(pROFESSION_TYPE))
			{
				OtherPlayerFakeObjs.DestroyFakeObj();
			}
		}
		else
		{
			OtherPlayerFakeObjs = new FakeObjLogic();
		}
		CurFakeObjRoot.EnableFakeObjRoot();
		CurModelPic.mainTexture = CurFakeObjRoot.ModelPic;
		general general = lookvalue.general;
		characterVisual visual = lookvalue.visual;
		OtherPlayerFakeObjs.InitFakeObject(visual, pROFESSION_TYPE.ToString(), empty, CurFakeObjRoot.MeshRoot, OnInitFakeObjDone);
		OtherPlayerRankLabel.text = string.Format("{0}: {1}", StrDictionary.GetDictionaryString("#{100754}"), rank);
		OtherPlayerNameLabel.text = general.name;
		OtherPlayerComboValueLabel.text = lookvalue.attribute_other.combValue.ToString();
		OtherPlayerIconSprite.spriteName = GameDefine.Profession_PicName[(int)lookvalue.general.profession];
		OtherPlayerIndex = lookvalue.id;
		mPreProfession = pROFESSION_TYPE;
		OtherPlayerLevelLabel.text = $"Lv.{lookvalue.attribute_other.level}";
	}

	private void OnDragModelBtn(GameObject btn, Vector2 delta)
	{
		if (OtherPlayerFakeObjs != null && OtherPlayerFakeObjs.FakeObj != null)
		{
			OtherPlayerFakeObjs.FakeObj.transform.localEulerAngles -= delta.x * Vector3.up;
		}
	}

	private void OnInitFakeObjDone(FakeObjLogic obj)
	{
		if (loadfinish != null)
		{
			loadfinish();
		}
	}

	public void OnClickFightBtn()
	{
		if (clickfight != null)
		{
			clickfight(OtherPlayerIndex);
		}
	}

	private void OnDisable()
	{
		UnLoadFakeObj();
	}

	public void UnLoadFakeObj()
	{
		if (OtherPlayerFakeObjs != null)
		{
			OtherPlayerFakeObjs.DestroyFakeObj();
			OtherPlayerFakeObjs = null;
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
