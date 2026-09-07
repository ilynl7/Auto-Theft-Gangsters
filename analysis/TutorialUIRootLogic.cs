using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUIRootLogic : SingletonUnity<TutorialUIRootLogic>
{
	public class TutorialUIInfo
	{
		public GameObject CenterObj;

		public Vector3 CenterPos;

		public int Width;

		public int Height;

		public string TipText;

		public SCREEN_DIRECTION TipTextPos;

		public bool IsTipCircleEnable;

		public bool IsMaskEnable;

		public bool IsMaskShow;

		public UIWidget.Pivot Pivot;

		public float Duration;

		public TutorialUIInfo(GameObject centerObj, int width, int height, string tipText, SCREEN_DIRECTION tipTextPos, float duration, bool isTipCircleEnable, bool isMaskEnable, bool isMaskShow = true, UIWidget.Pivot pivot = UIWidget.Pivot.Top)
		{
			CenterObj = centerObj;
			CenterPos = CenterObj.transform.position;
			Width = width;
			Height = height;
			TipText = tipText;
			TipTextPos = tipTextPos;
			IsTipCircleEnable = isTipCircleEnable;
			IsMaskEnable = isMaskEnable;
			IsMaskShow = isMaskShow;
			Pivot = pivot;
			Duration = duration;
		}

		public TutorialUIInfo(Vector3 centerPos, int width, int height, string tipText, SCREEN_DIRECTION tipTextPos, bool isTipCircleEnable, bool isMaskEnable, bool isMaskShow = true, UIWidget.Pivot pivot = UIWidget.Pivot.Top)
		{
			CenterObj = null;
			CenterPos = centerPos;
			Width = width;
			Height = height;
			TipText = tipText;
			TipTextPos = tipTextPos;
			IsTipCircleEnable = isTipCircleEnable;
			IsMaskEnable = isMaskEnable;
			IsMaskShow = isMaskShow;
			Pivot = pivot;
		}
	}

	public class TutorialUIChangePannelInfo
	{
		public GameObject CenterObj;

		public List<UIWidget> HighLightObjList;

		public int Width;

		public int Height;

		public string TipText;

		public SCREEN_DIRECTION TipTextPos;

		public bool IsTipCircleEnable;

		public bool IsMaskEnable;

		public bool IsMaskShow;

		public bool IsShowHand;

		public bool IsShowCircle;

		public TutorialUIChangePannelInfo(GameObject centerObj, List<UIWidget> objList, int width, int height, string tipText, SCREEN_DIRECTION tipTextPos, bool isTipCircleEnable, bool isMaskEnable, bool isMaskShow, bool isShowHand, bool isShowCircle)
		{
			CenterObj = centerObj;
			HighLightObjList = objList;
			Width = width;
			Height = height;
			TipText = tipText;
			TipTextPos = tipTextPos;
			IsTipCircleEnable = isTipCircleEnable;
			IsMaskEnable = isMaskEnable;
			IsMaskShow = isMaskShow;
			IsShowHand = isShowHand;
			IsShowCircle = isShowCircle;
		}
	}

	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	private float mTutorialStartTime;

	private float mTutorialWaitTime = -1f;

	public UIPanel TutorialPannel;

	public Transform RootOffsetTrans;

	public Transform LeftMaskTrans;

	public Transform RightMaskTrans;

	public Transform TopMaskTrans;

	public Transform BottomMaskTrans;

	public List<UISprite> MaskPic;

	public UISprite CenterMaskObj;

	public UISprite MaskSprite;

	public TweenScale HandTipTweenS;

	public UILabel TipTextLabel;

	public UISprite TipTextBottomSprite;

	public UITexture TipPic;

	public UISprite TipCircleSprite;

	private bool ChangePannelFlag;

	private List<UIWidget> mHighLightObjList = new List<UIWidget>();

	private List<Transform> mPreTransParentList = new List<Transform>();

	private List<Vector3> mPreLocalPosition = new List<Vector3>();

	public Transform HightLightObjRoot;

	public Transform CenterRoot;

	public UIAnchor AnchorRoot;

	private TutorialUIChangePannelInfo info;

	private int mTipLabelLength = 286;

	private TutorialUIInfo infoo;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent, float waitTime = -1f)
	{
		mOnClickTutorialBtn = tutorialEvent;
		mTutorialStartTime = Time.realtimeSinceStartup;
		mTutorialWaitTime = waitTime;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null && Time.realtimeSinceStartup - mTutorialStartTime > mTutorialWaitTime)
		{
			TutorialManager.OnClickTutorialBtn onClickTutorialBtn = mOnClickTutorialBtn;
			mOnClickTutorialBtn = null;
			onClickTutorialBtn();
		}
	}

	public void ClearTutorialEvent()
	{
		mOnClickTutorialBtn = null;
	}

	public static void ShowWindow(GameObject centerObj, int width, int height, string tipText, SCREEN_DIRECTION tipTextPos, float duration, bool isTipCircleEnable = false, bool isMaskEnable = false, bool isMaskShow = true, UIWidget.Pivot pivot = UIWidget.Pivot.Center)
	{
		if (centerObj == null)
		{
			Debug.Log("Center Obj == null");
			return;
		}
		TutorialUIInfo param = new TutorialUIInfo(centerObj, width, height, tipText, tipTextPos, duration, isTipCircleEnable, isMaskEnable, isMaskShow, pivot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TutorialUIRoot, OnTutorialUIRootOpen, param);
	}

	public static void ShowWindow(Vector3 centerPos, int width, int height, string tipText, SCREEN_DIRECTION tipTextPos, bool isTipCircleEnable = false, bool isMaskEnable = false, bool isMaskShow = true)
	{
		TutorialUIInfo param = new TutorialUIInfo(centerPos, width, height, tipText, tipTextPos, isTipCircleEnable, isMaskEnable, isMaskShow);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TutorialUIRoot, OnTutorialUIRootOpen, param);
	}

	public static void ShowWindow(GameObject centerObj, List<UIWidget> showObjList, int width, int height, string tipText, SCREEN_DIRECTION tipTextPos, bool isTipCircleEnable = false, bool isMaskEnable = false, bool isMaskShow = true, bool isShowHand = true, bool isShowCircle = true)
	{
		TutorialUIChangePannelInfo param = new TutorialUIChangePannelInfo(centerObj, showObjList, width, height, tipText, tipTextPos, isTipCircleEnable, isMaskEnable, isMaskShow, isShowHand, isShowCircle);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TutorialUIRoot, OnTutorialUIRootOpenChangePannel, param);
	}

	private static void OnTutorialUIRootOpenChangePannel(bool isSuccess, object param)
	{
		if (isSuccess && param is TutorialUIChangePannelInfo infos && SingletonUnity<TutorialUIRootLogic>.Exists)
		{
			SingletonUnity<TutorialUIRootLogic>.Instance.Reset(infos);
		}
	}

	private static void OnTutorialUIRootOpen(bool isSuccess, object param)
	{
		if (isSuccess && param is TutorialUIInfo tutorialUIInfo && SingletonUnity<TutorialUIRootLogic>.Exists)
		{
			SingletonUnity<TutorialUIRootLogic>.Instance.Reset(tutorialUIInfo);
		}
	}

	private IEnumerator DelayReset()
	{
		yield return null;
		UIAnchor targetAnchor = NGUITools.FindInParents<UIAnchor>(info.CenterObj);
		if (targetAnchor != null)
		{
			AnchorRoot.side = targetAnchor.side;
		}
		else
		{
			AnchorRoot.side = UIAnchor.Side.Center;
		}
		AnchorRoot.ScreenSizeChanged();
		ChangePannelFlag = true;
		HandTipTweenS.enabled = true;
		HandTipTweenS.transform.localPosition = Vector3.zero;
		UnityVersionUtil.SetActiveRecursive(RootOffsetTrans.gameObject, state: true);
		RootOffsetTrans.transform.position = info.CenterObj.transform.position;
		mHighLightObjList = info.HighLightObjList;
		if (mHighLightObjList != null && mHighLightObjList.Count != 0)
		{
			for (int j = 0; j < mHighLightObjList.Count; j++)
			{
				mPreTransParentList.Add(mHighLightObjList[j].transform.parent);
				mPreLocalPosition.Add(mHighLightObjList[j].transform.localPosition);
				mHighLightObjList[j].transform.parent = HightLightObjRoot;
				NGUITools.MarkParentAsChanged(mHighLightObjList[j].gameObject);
			}
		}
		if (!info.IsTipCircleEnable)
		{
			UnityVersionUtil.SetActiveRecursive(TipCircleSprite.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(HandTipTweenS.gameObject, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(TipCircleSprite.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(HandTipTweenS.gameObject, state: true);
		}
		if (!info.IsMaskEnable)
		{
			UnityVersionUtil.SetActiveRecursive(LeftMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(RightMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(TopMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(BottomMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(CenterMaskObj.gameObject, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(LeftMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(RightMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(TopMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(BottomMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(CenterMaskObj.gameObject, state: true);
			if (info.IsMaskShow)
			{
				CenterMaskObj.alpha = 0.4f;
			}
			else
			{
				CenterMaskObj.alpha = 0.01f;
			}
		}
		if (string.IsNullOrEmpty(info.TipText))
		{
			UnityVersionUtil.SetActiveRecursive(TipTextLabel.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(TipTextBottomSprite.gameObject, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(TipTextLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(TipTextBottomSprite.gameObject, state: true);
			TipTextLabel.text = info.TipText;
			TipTextBottomSprite.width = TipTextLabel.width + 84;
			TipTextBottomSprite.height = TipTextLabel.height + 20;
			int width = TipTextBottomSprite.width;
			int height = TipTextBottomSprite.height;
			int XoffSet = 0;
			int YoffSet = 0;
			int Distance = 20;
			switch (info.TipTextPos)
			{
			case SCREEN_DIRECTION.TOP:
				YoffSet = Distance + height / 2 + info.Height / 2;
				break;
			case SCREEN_DIRECTION.BOTTOM:
				YoffSet = -(Distance + height / 2 + info.Height / 2);
				break;
			case SCREEN_DIRECTION.LEFT:
				XoffSet = -(Distance + width / 2 + info.Width / 2);
				break;
			case SCREEN_DIRECTION.RIGHT:
				XoffSet = Distance + width / 2 + info.Width / 2;
				break;
			case SCREEN_DIRECTION.TOP_LEFT:
				YoffSet = Distance + height / 2 + info.Height / 2;
				XoffSet = -(Distance + width / 2 + info.Width / 2);
				break;
			case SCREEN_DIRECTION.TOP_RIGHT:
				YoffSet = Distance + height / 2 + info.Height / 2;
				XoffSet = Distance + width / 2 + info.Width / 2;
				break;
			case SCREEN_DIRECTION.BOTTOM_LEFT:
				YoffSet = -(Distance + height / 2 + info.Height / 2);
				XoffSet = -(Distance + width / 2 + info.Width / 2);
				break;
			case SCREEN_DIRECTION.BOTTOM_RIGHT:
				YoffSet = -(Distance + height / 2 + info.Height / 2);
				XoffSet = Distance + width / 2 + info.Width / 2;
				break;
			}
			TipTextBottomSprite.transform.localPosition = new Vector3(XoffSet, YoffSet, 0f);
			if (info.TipTextPos == SCREEN_DIRECTION.LEFT)
			{
				SetPicLeft();
			}
			else if (info.TipTextPos == SCREEN_DIRECTION.RIGHT)
			{
				SetPicRight();
			}
			else if (TipTextBottomSprite.transform.position.x > CenterRoot.transform.position.x)
			{
				SetPicLeft();
			}
			else
			{
				SetPicRight();
			}
		}
		if (info.IsShowHand)
		{
			UnityVersionUtil.SetActiveRecursive(HandTipTweenS.gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(HandTipTweenS.gameObject, state: false);
		}
		if (info.IsShowCircle)
		{
			UnityVersionUtil.SetActiveRecursive(TipCircleSprite.gameObject, state: true);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(TipCircleSprite.gameObject, state: false);
		}
		if (mHighLightObjList != null && mHighLightObjList.Count != 0 && SingletonUnity<FunctionTipsRootLogic>.Exists)
		{
			for (int i = 0; i < mHighLightObjList.Count; i++)
			{
				if (SingletonUnity<FunctionTipsRootLogic>.Instance.IsHaveTipsCircle(mHighLightObjList[i].gameObject))
				{
					UnityVersionUtil.SetActiveRecursive(TipCircleSprite.gameObject, state: false);
					break;
				}
			}
		}
		info = null;
	}

	public void Reset(TutorialUIChangePannelInfo infos)
	{
		if (SingletonUnity<MissionTeamTipLogic>.Exists && SingletonUnity<MissionTeamTipLogic>.Instance.gameObject.active)
		{
			SingletonUnity<MissionTeamTipLogic>.Instance.MissionTipRoot.CloseHandTip();
		}
		info = infos;
		UnityVersionUtil.SetActiveRecursive(TipCircleSprite.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(HandTipTweenS.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(TipTextLabel.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(TipTextBottomSprite.gameObject, state: false);
		if (!info.IsMaskEnable)
		{
			UnityVersionUtil.SetActiveRecursive(LeftMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(RightMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(TopMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(BottomMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(CenterMaskObj.gameObject, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(LeftMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(RightMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(TopMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(BottomMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(CenterMaskObj.gameObject, state: true);
			if (info.IsMaskShow)
			{
				CenterMaskObj.alpha = 0.4f;
			}
			else
			{
				CenterMaskObj.alpha = 0.01f;
			}
		}
		mOnClickTutorialBtn = null;
		if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(DelayReset());
		}
	}

	private void SetPicRight()
	{
		TipPic.transform.localPosition = new Vector3(TipTextBottomSprite.width / 2, -TipTextBottomSprite.height / 2, 0f);
		TipPic.transform.localScale = new Vector3(1f, 1f, 1f);
		TipTextLabel.transform.localPosition = new Vector3(-TipTextBottomSprite.width / 2 + 10, 0f, 0f);
	}

	private void SetPicLeft()
	{
		TipPic.transform.localPosition = new Vector3(-TipTextBottomSprite.width / 2, -TipTextBottomSprite.height / 2, 0f);
		TipPic.transform.localScale = new Vector3(-1f, 1f, 1f);
		TipTextLabel.transform.localPosition = new Vector3(TipTextBottomSprite.width / 2 - 10 - TipTextLabel.width, 0f, 0f);
	}

	public void Reset(TutorialUIInfo info)
	{
		if (SingletonUnity<MissionTeamTipLogic>.Exists && SingletonUnity<MissionTeamTipLogic>.Instance.gameObject.active)
		{
			SingletonUnity<MissionTeamTipLogic>.Instance.MissionTipRoot.CloseHandTip();
		}
		infoo = info;
		UIAnchor uIAnchor = NGUITools.FindInParents<UIAnchor>(info.CenterObj);
		if (uIAnchor != null)
		{
			AnchorRoot.side = uIAnchor.side;
		}
		else
		{
			AnchorRoot.side = UIAnchor.Side.Center;
		}
		AnchorRoot.ScreenSizeChanged();
		if (info.CenterObj == null)
		{
			RootOffsetTrans.transform.localPosition = info.CenterPos;
		}
		else if (info.Pivot == UIWidget.Pivot.Center)
		{
			RootOffsetTrans.transform.position = info.CenterObj.transform.position;
		}
		else if (info.Pivot == UIWidget.Pivot.Top)
		{
			RootOffsetTrans.transform.position = info.CenterObj.transform.position - new Vector3(0f, (float)(info.Height / 2) * RootOffsetTrans.transform.lossyScale.y, 0f);
		}
		HandTipTweenS.enabled = true;
		HandTipTweenS.transform.localPosition = Vector3.zero;
		if (UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(NoChangeDelayReset());
		}
	}

	private IEnumerator NoChangeDelayReset()
	{
		yield return null;
		TutorialUIInfo info = infoo;
		UIAnchor targetAnchor = NGUITools.FindInParents<UIAnchor>(info.CenterObj);
		if (targetAnchor != null)
		{
			AnchorRoot.side = targetAnchor.side;
		}
		else
		{
			AnchorRoot.side = UIAnchor.Side.Center;
		}
		AnchorRoot.ScreenSizeChanged();
		if (info.CenterObj == null)
		{
			RootOffsetTrans.transform.localPosition = info.CenterPos;
		}
		else if (info.Pivot == UIWidget.Pivot.Center)
		{
			RootOffsetTrans.transform.position = info.CenterObj.transform.position;
		}
		else if (info.Pivot == UIWidget.Pivot.Top)
		{
			RootOffsetTrans.transform.position = info.CenterObj.transform.position - new Vector3(0f, (float)(info.Height / 2) * RootOffsetTrans.transform.lossyScale.y, 0f);
		}
		ChangePannelFlag = false;
		UnityVersionUtil.SetActiveRecursive(RootOffsetTrans.gameObject, state: true);
		if (!info.IsTipCircleEnable)
		{
			UnityVersionUtil.SetActiveRecursive(TipCircleSprite.gameObject, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(TipCircleSprite.gameObject, state: true);
		}
		if (!info.IsMaskEnable)
		{
			UnityVersionUtil.SetActiveRecursive(LeftMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(RightMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(TopMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(BottomMaskTrans.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(CenterMaskObj.gameObject, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(LeftMaskTrans.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(RightMaskTrans.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(TopMaskTrans.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(BottomMaskTrans.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(CenterMaskObj.gameObject, state: false);
			if (info.IsMaskShow)
			{
				for (int j = 0; j < MaskPic.Count; j++)
				{
					MaskPic[j].color = new Color(1f, 1f, 1f, 0.3f);
				}
			}
			else
			{
				for (int i = 0; i < MaskPic.Count; i++)
				{
					MaskPic[i].color = new Color(1f, 1f, 1f, 0.01f);
				}
			}
			int xOffset = MaskSprite.width / 2;
			int yOffset = MaskSprite.height / 2;
			int xFit = MaskSprite.width % 2;
			int yFit = MaskSprite.height % 2;
			LeftMaskTrans.transform.localPosition = new Vector3(-xOffset - info.Width / 2, -yOffset + info.Height / 2 - yFit, 0f);
			RightMaskTrans.transform.localPosition = new Vector3(xOffset + info.Width / 2, yOffset - info.Height / 2 + yFit, 0f);
			TopMaskTrans.transform.localPosition = new Vector3(-xOffset + info.Width / 2 - xFit, yOffset + info.Height / 2, 0f);
			BottomMaskTrans.transform.localPosition = new Vector3(xOffset - info.Width / 2 + xFit, -yOffset - info.Height / 2, 0f);
		}
		if (string.IsNullOrEmpty(info.TipText))
		{
			UnityVersionUtil.SetActiveRecursive(TipTextLabel.gameObject, state: false);
			UnityVersionUtil.SetActiveRecursive(TipTextBottomSprite.gameObject, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(TipTextLabel.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(TipTextBottomSprite.gameObject, state: true);
			TipTextLabel.width = mTipLabelLength;
			TipTextLabel.text = info.TipText;
			if (TipTextLabel.printedSize.x < (float)mTipLabelLength)
			{
				TipTextLabel.width = Mathf.CeilToInt(TipTextLabel.printedSize.x);
			}
			else
			{
				TipTextLabel.width = mTipLabelLength;
			}
			TipTextBottomSprite.width = TipTextLabel.width + 84;
			TipTextBottomSprite.height = TipTextLabel.height + 20;
			int width = TipTextBottomSprite.width;
			int height = TipTextBottomSprite.height;
			int XoffSet = 0;
			int YoffSet = 0;
			int Distance = 20;
			switch (info.TipTextPos)
			{
			case SCREEN_DIRECTION.TOP:
				YoffSet = Distance + height / 2 + info.Height / 2;
				break;
			case SCREEN_DIRECTION.BOTTOM:
				YoffSet = -(Distance + height / 2 + info.Height / 2);
				break;
			case SCREEN_DIRECTION.LEFT:
				XoffSet = -(Distance + width / 2 + info.Width / 2);
				break;
			case SCREEN_DIRECTION.RIGHT:
				XoffSet = Distance + width / 2 + info.Width / 2;
				break;
			case SCREEN_DIRECTION.TOP_LEFT:
				YoffSet = Distance + height / 2 + info.Height / 2;
				XoffSet = -(Distance + width / 2 + info.Width / 2);
				break;
			case SCREEN_DIRECTION.TOP_RIGHT:
				YoffSet = Distance + height / 2 + info.Height / 2;
				XoffSet = Distance + width / 2 + info.Width / 2;
				break;
			case SCREEN_DIRECTION.BOTTOM_LEFT:
				YoffSet = -(Distance + height / 2 + info.Height / 2);
				XoffSet = -(Distance + width / 2 + info.Width / 2);
				break;
			case SCREEN_DIRECTION.BOTTOM_RIGHT:
				YoffSet = -(Distance + height / 2 + info.Height / 2);
				XoffSet = Distance + width / 2 + info.Width / 2;
				break;
			}
			TipTextBottomSprite.transform.localPosition = new Vector3(XoffSet, YoffSet, 0f);
			if (info.TipTextPos == SCREEN_DIRECTION.LEFT)
			{
				SetPicLeft();
			}
			else if (info.TipTextPos == SCREEN_DIRECTION.RIGHT)
			{
				SetPicRight();
			}
			else if (TipTextBottomSprite.transform.position.x > CenterRoot.transform.position.x)
			{
				SetPicLeft();
			}
			else
			{
				SetPicRight();
			}
		}
		mOnClickTutorialBtn = null;
	}

	public void CloseCheck()
	{
		if (mHighLightObjList == null || mHighLightObjList.Count <= 0 || !ChangePannelFlag || mHighLightObjList == null || mHighLightObjList.Count == 0)
		{
			return;
		}
		for (int i = 0; i < mHighLightObjList.Count; i++)
		{
			mHighLightObjList[i].transform.parent = mPreTransParentList[i];
			mHighLightObjList[i].transform.localPosition = mPreLocalPosition[i];
			if (!mPreTransParentList[i].gameObject.active)
			{
				NGUITools.SetActive(mHighLightObjList[i].gameObject, state: false);
			}
			NGUITools.MarkParentAsChanged(mHighLightObjList[i].gameObject);
		}
		mHighLightObjList.Clear();
		mPreTransParentList.Clear();
		mPreLocalPosition.Clear();
	}

	private void OnEnable()
	{
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.IsTalking = true;
		}
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(ScreenSizeChanged));
	}

	private void OnDisable()
	{
		if (Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.IsTalking = false;
		}
		UICamera.onScreenResize = (UICamera.OnScreenResize)Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(ScreenSizeChanged));
		info = null;
		infoo = null;
	}

	private void ScreenSizeChanged()
	{
		if (infoo != null && infoo.IsMaskEnable)
		{
			int num = MaskSprite.width / 2;
			int num2 = MaskSprite.height / 2;
			int num3 = MaskSprite.width % 2;
			int num4 = MaskSprite.height % 2;
			LeftMaskTrans.transform.localPosition = new Vector3(-num - infoo.Width / 2, -num2 + infoo.Height / 2 - num4, 0f);
			RightMaskTrans.transform.localPosition = new Vector3(num + infoo.Width / 2, num2 - infoo.Height / 2 + num4, 0f);
			TopMaskTrans.transform.localPosition = new Vector3(-num + infoo.Width / 2 - num3, num2 + infoo.Height / 2, 0f);
			BottomMaskTrans.transform.localPosition = new Vector3(num - infoo.Width / 2 + num3, -num2 - infoo.Height / 2, 0f);
		}
	}

	public static void CloseWindow()
	{
		if (SingletonUnity<TutorialUIRootLogic>.Exists && SingletonUnity<TutorialUIRootLogic>.Instance.gameObject.active)
		{
			SingletonUnity<TutorialUIRootLogic>.Instance.CloseCheck();
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.TutorialUIRoot);
		}
	}
}
