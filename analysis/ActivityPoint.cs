using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ActivityPoint : MonoBehaviour
{
	public delegate void OnArrivePointDelegate();

	public GameObject PicObj;

	public GameObject LockObj;

	public ActivityMapData CurActData;

	private Material PicMat;

	private Material LockPicMat;

	public string ID;

	private Transform mTransform;

	private float SqrActiveRadius;

	private float ExitSqrActiveRadius;

	private int ActiveRadius = 4;

	private Transform mMainPlayerTransform;

	private bool mInCircleFlag;

	private List<string> textureList = new List<string>();

	public bool IsEnable;

	private bool IsExitFlag;

	public OnArrivePointDelegate OnArrivePoint;

	private float sqrDis;

	private void Awake()
	{
		mTransform = base.transform;
		SqrActiveRadius = ActiveRadius * ActiveRadius;
		ExitSqrActiveRadius = ((float)ActiveRadius + 0.5f) * ((float)ActiveRadius + 0.5f);
	}

	public void Reset(ActivityMapData curinfo, Transform parenttra, bool isenable)
	{
		IsEnable = isenable;
		CurActData = curinfo;
		ID = CurActData.ID;
		if (CurActData.IsShowDoorFlag())
		{
			ActiveRadius = 3;
			SqrActiveRadius = ActiveRadius * ActiveRadius;
			ExitSqrActiveRadius = ((float)ActiveRadius + 0.5f) * ((float)ActiveRadius + 0.5f);
		}
		else
		{
			InitTexture();
			ActiveRadius = 4;
			SqrActiveRadius = ActiveRadius * ActiveRadius;
			ExitSqrActiveRadius = ((float)ActiveRadius + 0.5f) * ((float)ActiveRadius + 0.5f);
		}
		mTransform.parent = parenttra;
		mTransform.localPosition = CurActData.Position;
		mTransform.name = ID;
		IsExitFlag = false;
		OnArrivePoint = null;
	}

	public void ResetExit(OnArrivePointDelegate func = null)
	{
		CurActData = null;
		IsExitFlag = true;
		OnArrivePoint = func;
		ActiveRadius = 2;
		SqrActiveRadius = ActiveRadius * ActiveRadius;
		ExitSqrActiveRadius = ((float)ActiveRadius + 0.5f) * ((float)ActiveRadius + 0.5f);
	}

	public void InitTexture()
	{
		UnityVersionUtil.SetActiveRecursive(PicObj, state: false);
		if (LockObj != null)
		{
			UnityVersionUtil.SetActiveRecursive(LockObj, state: false);
		}
		PicMat = PicObj.gameObject.renderer.material;
		if (LockObj != null)
		{
			LockPicMat = LockObj.gameObject.renderer.material;
		}
		else
		{
			LockPicMat = null;
		}
		if (!(PicMat == null))
		{
			textureList.Clear();
			if (!string.IsNullOrEmpty(CurActData.Icon) && (PicMat.mainTexture == null || !PicMat.mainTexture.name.Equals(CurActData.Icon)))
			{
				textureList.Add(CurActData.Icon);
			}
			if (LockPicMat != null && (LockPicMat.mainTexture == null || LockPicMat.mainTexture.name.Equals("CZ_effect_suo")))
			{
				textureList.Add("CZ_effect_suo");
			}
			if (textureList.Count != 0 && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager != null && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.MapActivityManager != null && UnityVersionUtil.IsactiveInHierarchy(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.MapActivityManager.gameObject))
			{
				SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.MapActivityManager.StartCoroutine(BundleManager.LoadWaitTexture(textureList, TextureLoadFinish));
			}
		}
	}

	private void TextureLoadFinish(string name, Texture curtex)
	{
		if (!(curtex == null))
		{
			if (CurActData.Icon.Equals(name) && PicObj != null)
			{
				UnityVersionUtil.SetActiveRecursive(PicObj, state: true);
			}
			if ("CZ_effect_suo".Equals(name) && LockObj != null)
			{
				UnityVersionUtil.SetActiveRecursive(LockObj, state: true);
			}
			if (PicMat != null && CurActData.Icon.Equals(name))
			{
				PicMat.SetTexture("_MainTex", curtex);
			}
			if (LockPicMat != null && "CZ_effect_suo".Equals(name))
			{
				LockPicMat.SetTexture("_MainTex", curtex);
			}
		}
	}

	private void FixedUpdate()
	{
		if (!IsExitFlag && CurActData == null)
		{
			return;
		}
		if (null == mMainPlayerTransform)
		{
			if (null != Singleton<ObjManager>.Instance.MainPlayer)
			{
				mMainPlayerTransform = Singleton<ObjManager>.Instance.MainPlayer.transform;
			}
			if (null == mMainPlayerTransform)
			{
				return;
			}
		}
		if (!(null != Singleton<ObjManager>.Instance.MainPlayer))
		{
			return;
		}
		sqrDis = (mMainPlayerTransform.position - mTransform.position).sqrMagnitude;
		if (sqrDis <= SqrActiveRadius)
		{
			if (mInCircleFlag)
			{
				return;
			}
			if (IsExitFlag)
			{
				if (OnArrivePoint != null)
				{
					OnArrivePoint();
				}
			}
			else if (CurActData.ActivityType == GameDefine.ACTIVITY_TYPE.SHOP_GATE)
			{
				if (!CurActData.IsUnlock)
				{
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ActivityTipstRoot, delegate
					{
						SingletonUnity<ActivityTipsRootLogic>.Instance.ShowInfo(CurActData);
					});
				}
				else if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload && !string.IsNullOrEmpty(CurActData.TargetMapID))
				{
					SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
					enter_empty_scene.request request = new enter_empty_scene.request();
					request.mapInfoId = CurActData.TargetMapID;
					NetLogic.GetInstance().Send<Protocol.enter_empty_scene>(request);
				}
			}
			else
			{
				SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ActivityTipstRoot, delegate
				{
					SingletonUnity<ActivityTipsRootLogic>.Instance.ShowInfo(CurActData);
				});
			}
			mInCircleFlag = true;
		}
		else if (sqrDis > ExitSqrActiveRadius)
		{
			if (mInCircleFlag && SingletonUnity<ActivityTipsRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ActivityTipsRootLogic>.Instance.gameObject))
			{
				SingletonUnity<ActivityTipsRootLogic>.Instance.CloseUI();
			}
			mInCircleFlag = false;
		}
	}
}
