using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class BigPackRootLogic : SingletonUnity<BigPackRootLogic>
{
	public TweenPosition MainAnima;

	public TweenPosition TempAnima;

	public BigPackPageLogic MainPage;

	public BigPackPageLogic TempPage;

	public UISprite LeftSp;

	public UISprite RightSp;

	public UIGrid PointGrid;

	public List<UISprite> PointList;

	public UIEventListener uidragEvent;

	private List<special_big_pack> SpePackList = new List<special_big_pack>();

	private int CurShowIndex;

	private Vector3 LeftPos = new Vector3(-1000f, 0f, 0f);

	private Vector3 rightPos = new Vector3(1000f, 0f, 0f);

	private float MoveTime = 0.5f;

	private float temptime;

	private bool isMoveFlag;

	private bool textureFinish;

	private bool requestFinish;

	private float mWaitTime = 10f;

	private List<string> textureList = new List<string>();

	private float starttime;

	private float DelAutoDragtime = 5f;

	public string TargetShowPack = string.Empty;

	private Color ambientLight;

	private bool moveDirRight = true;

	public void EnableReset()
	{
		ambientLight = RenderSettings.ambientLight;
		TargetShowPack = string.Empty;
		textureFinish = true;
		requestFinish = false;
		mWaitTime = 10f;
		UnityVersionUtil.SetActiveRecursive(MainPage.gameObject, state: false);
		UnityVersionUtil.SetActiveRecursive(TempPage.gameObject, state: false);
		UIEventListener uIEventListener = uidragEvent;
		uIEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener.onDrag, new UIEventListener.VectorDelegate(OnDragMoveBtn));
		UnityVersionUtil.SetActiveRecursive(PointGrid.gameObject, state: false);
		NGUITools.SetActive(LeftSp.gameObject, state: false);
		NGUITools.SetActive(RightSp.gameObject, state: false);
	}

	private void OnDisable()
	{
		ResetNormalLight();
	}

	public void ResetNormalLight()
	{
		RenderSettings.ambientLight = ambientLight;
	}

	private void CheckShowUI()
	{
		if (textureFinish && requestFinish)
		{
			CurShowIndex = 0;
			starttime = Time.time;
			ResetPoint();
			UnityVersionUtil.SetActiveRecursive(MainPage.gameObject, state: true);
			UnityVersionUtil.SetActiveRecursive(TempPage.gameObject, state: false);
			MainPage.RefershInfo(SpePackList[CurShowIndex], ambientLight);
			SelectPoint();
		}
	}

	private void Update()
	{
		if ((!requestFinish || !textureFinish) && mWaitTime > 0f)
		{
			mWaitTime -= Time.deltaTime;
			if (mWaitTime <= 0f)
			{
				OnClickCloseBtn();
			}
		}
		if (Input.touchCount > 0)
		{
			starttime = Time.time;
		}
		if (isMoveFlag)
		{
			temptime += Time.deltaTime;
			if (temptime >= MoveTime)
			{
				temptime -= MoveTime;
				isMoveFlag = false;
			}
		}
		else if (requestFinish && textureFinish && SpePackList.Count > 1 && Time.time - starttime >= DelAutoDragtime)
		{
			AutoMove();
		}
	}

	public void AutoMove()
	{
		if (SingletonUnity<ItemInfoRootLogicNew>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ItemInfoRootLogicNew>.Instance.gameObject))
		{
			starttime = Time.time;
		}
		else if (moveDirRight)
		{
			if (CurShowIndex < SpePackList.Count - 1)
			{
				OnClickRightBtn();
				return;
			}
			moveDirRight = false;
			AutoMove();
		}
		else if (CurShowIndex > 0)
		{
			OnClickLeftBtn();
		}
		else
		{
			moveDirRight = true;
			AutoMove();
		}
	}

	public void Reset(ret_request_big_pack.request request)
	{
		SpePackList.Clear();
		if (request.HasSpecial_big_packs)
		{
			SpePackList = new List<special_big_pack>(request.special_big_packs.Values);
			for (int num = SpePackList.Count - 1; num >= 0; num--)
			{
				BigPackageData bigPackageDataById = DataManager.GetBigPackageDataById(SpePackList[num].ID);
				if (bigPackageDataById != null)
				{
					if (bigPackageDataById.SellType != 0 || SpePackList[num].state != 0L)
					{
						SpePackList.RemoveAt(num);
					}
				}
				else
				{
					SpePackList.RemoveAt(num);
				}
			}
			SpePackList.Sort(delegate(special_big_pack x, special_big_pack y)
			{
				BigPackageData bigPackageDataById2 = DataManager.GetBigPackageDataById(x.ID);
				BigPackageData bigPackageDataById3 = DataManager.GetBigPackageDataById(y.ID);
				return (bigPackageDataById2.sortID == bigPackageDataById3.sortID) ? (int.Parse(x.ID) - int.Parse(y.ID)) : (bigPackageDataById2.sortID - bigPackageDataById3.sortID);
			});
		}
		else
		{
			special_big_pack special_big_pack = new special_big_pack();
			special_big_pack.ID = request.ID;
			special_big_pack.state = request.state;
			if (request.HasEnd_time)
			{
				special_big_pack.end_time = request.end_time;
			}
			SpePackList.Add(special_big_pack);
		}
		if (SpePackList.Count == 0)
		{
			OnClickCloseBtn();
			return;
		}
		special_big_pack special_big_pack2 = null;
		if (!string.IsNullOrEmpty(TargetShowPack))
		{
			for (int i = 0; i < SpePackList.Count; i++)
			{
				if (SpePackList[i].ID.Equals(TargetShowPack))
				{
					special_big_pack2 = SpePackList[i];
					break;
				}
			}
		}
		if (special_big_pack2 != null)
		{
			SpePackList.Clear();
			SpePackList.Add(special_big_pack2);
		}
		requestFinish = true;
		CheckShowUI();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "BigSales", "open");
	}

	public void OnDragMoveBtn(GameObject btn, Vector2 delta)
	{
		if (!isMoveFlag)
		{
			if (delta.x > 20f)
			{
				OnClickLeftBtn();
			}
			else if (delta.x < -20f)
			{
				OnClickRightBtn();
			}
		}
	}

	public void OnClickLeftBtn()
	{
		if (CurShowIndex > 0)
		{
			isMoveFlag = true;
			moveDirRight = false;
			starttime = Time.time;
			CurShowIndex--;
			MainAnima.from = MainAnima.transform.localPosition;
			MainAnima.to = rightPos;
			MainAnima.duration = MoveTime;
			MainAnima.ResetToBeginning();
			TempAnima.from = LeftPos;
			TempAnima.to = Vector3.zero;
			TempAnima.duration = MoveTime;
			TempAnima.ResetToBeginning();
			MainAnima.PlayForward();
			TempAnima.PlayForward();
			MainPage.UnLoadFakeObj();
			UnityVersionUtil.SetActiveRecursive(TempPage.gameObject, state: true);
			TempPage.RefershInfo(SpePackList[CurShowIndex], ambientLight);
			SwapPage();
			SelectPoint();
		}
	}

	public void OnClickRightBtn()
	{
		if (CurShowIndex < SpePackList.Count - 1)
		{
			isMoveFlag = true;
			moveDirRight = true;
			starttime = Time.time;
			CurShowIndex++;
			MainAnima.from = MainAnima.transform.localPosition;
			MainAnima.to = LeftPos;
			MainAnima.duration = MoveTime;
			MainAnima.ResetToBeginning();
			TempAnima.from = rightPos;
			TempAnima.to = Vector3.zero;
			TempAnima.duration = MoveTime;
			TempAnima.ResetToBeginning();
			MainAnima.PlayForward();
			TempAnima.PlayForward();
			MainPage.UnLoadFakeObj();
			UnityVersionUtil.SetActiveRecursive(TempPage.gameObject, state: true);
			TempPage.RefershInfo(SpePackList[CurShowIndex], ambientLight);
			SwapPage();
			SelectPoint();
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.BigPackRoot);
		if (SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
		{
			SingletonUnity<AutoPopUIRoot>.Instance.NextPop();
		}
	}

	public void UpdateInfo(string id)
	{
		bool flag = true;
		for (int i = 0; i < SpePackList.Count; i++)
		{
			if (SpePackList[i].ID.Equals(id))
			{
				SpePackList[i].state = 1L;
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "BigSales", $"require_{id}");
			}
			if (SpePackList[i].state == 0L)
			{
				flag = false;
			}
		}
		if (flag && SingletonUnity<FunctionBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionBtnRootLogic>.Instance.gameObject))
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.HideBigSaleBtn();
		}
		MainPage.UpdateInfo(id);
		TempPage.UpdateInfo(id);
	}

	public void SelectPoint()
	{
		for (int i = 0; i < PointList.Count; i++)
		{
			if (i == CurShowIndex)
			{
				PointList[i].color = new Color(0f, 1f, 1f);
			}
			else
			{
				PointList[i].color = new Color(23f / 85f, 23f / 85f, 23f / 85f);
			}
		}
		if (CurShowIndex == 0)
		{
			NGUITools.SetActive(LeftSp.gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(LeftSp.gameObject, state: true);
		}
		if (CurShowIndex == SpePackList.Count - 1)
		{
			NGUITools.SetActive(RightSp.gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(RightSp.gameObject, state: true);
		}
	}

	public void ResetPoint()
	{
		int num = SpePackList.Count - PointList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(PointList[0].gameObject) as GameObject;
				UISprite component = gameObject.GetComponent<UISprite>();
				gameObject.name = $"point{PointList.Count:D2}";
				gameObject.transform.parent = PointGrid.transform;
				gameObject.transform.localScale = Vector3.one;
				PointList.Add(component);
			}
		}
		for (int j = 0; j < PointList.Count; j++)
		{
			if (j < SpePackList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(PointList[j].gameObject, state: true);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(PointList[j].gameObject, state: false);
			}
		}
		PointGrid.Reposition();
		if (SpePackList.Count <= 1)
		{
			UnityVersionUtil.SetActiveRecursive(PointGrid.gameObject, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(PointGrid.gameObject, state: true);
		}
	}

	public void SwapPage()
	{
		TweenPosition mainAnima = MainAnima;
		MainAnima = TempAnima;
		TempAnima = mainAnima;
		BigPackPageLogic mainPage = MainPage;
		MainPage = TempPage;
		TempPage = mainPage;
	}
}
