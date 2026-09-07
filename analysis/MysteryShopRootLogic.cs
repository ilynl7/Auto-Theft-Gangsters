using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class MysteryShopRootLogic : SingletonUnity<MysteryShopRootLogic>
{
	public TweenPosition MainAnima;

	public TweenPosition TempAnima;

	public MysteryShopPageLogic MainPage;

	public MysteryShopPageLogic TempPage;

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

	public string TargetID = string.Empty;

	private void OnEnable()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MenuBaseRootUI, delegate
		{
			List<MenuTabBtnInfo> leftBtnInfo = new List<MenuTabBtnInfo>();
			SingletonUnity<MenuBaseRootLogic>.Instance.ResetPage(leftBtnInfo, OnClickBackBtn, hideTab: true, StrDictionary.GetDictionaryString("#{100161}"));
		});
		UIEventListener uIEventListener = uidragEvent;
		uIEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener.onDrag, new UIEventListener.VectorDelegate(OnDragMoveBtn));
		TargetID = string.Empty;
	}

	public void EnableReset()
	{
		UnityVersionUtil.SetActiveRecursive(MainPage.gameObject, state: true);
		UnityVersionUtil.SetActiveRecursive(TempPage.gameObject, state: false);
		MainPage.EnableReset();
		TempPage.EnableReset();
	}

	public void Reset(ret_special_big_pack.request request)
	{
		SpePackList.Clear();
		SpePackList = new List<special_big_pack>(request.special_big_packs.Values);
		for (int num = SpePackList.Count - 1; num >= 0; num--)
		{
			BigPackageData bigPackageDataById = DataManager.GetBigPackageDataById(SpePackList[num].ID);
			if (bigPackageDataById != null)
			{
				if (bigPackageDataById.SellType != 1 || SpePackList[num].state != 0L)
				{
					SpePackList.RemoveAt(num);
				}
			}
			else
			{
				SpePackList.RemoveAt(num);
			}
		}
		if (SpePackList.Count == 0)
		{
			OnClickBackBtn();
			return;
		}
		SpePackList.Sort(delegate(special_big_pack x, special_big_pack y)
		{
			BigPackageData bigPackageDataById2 = DataManager.GetBigPackageDataById(x.ID);
			BigPackageData bigPackageDataById3 = DataManager.GetBigPackageDataById(y.ID);
			return (bigPackageDataById2.sortID == bigPackageDataById3.sortID) ? (int.Parse(x.ID) - int.Parse(y.ID)) : (bigPackageDataById2.sortID - bigPackageDataById3.sortID);
		});
		ResetPoint();
		CurShowIndex = 0;
		if (!string.IsNullOrEmpty(TargetID))
		{
			for (int i = 0; i < SpePackList.Count; i++)
			{
				if (SpePackList[i].ID.Equals(TargetID))
				{
					CurShowIndex = i;
					break;
				}
			}
			TargetID = string.Empty;
		}
		UnityVersionUtil.SetActiveRecursive(MainPage.gameObject, state: true);
		UnityVersionUtil.SetActiveRecursive(TempPage.gameObject, state: false);
		MainPage.RefershInfo(SpePackList[CurShowIndex]);
		SelectPoint();
		SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "Mystery", "open");
	}

	public void UpdateInfo(string id)
	{
		bool flag = true;
		for (int i = 0; i < SpePackList.Count; i++)
		{
			if (SpePackList[i].ID.Equals(id))
			{
				SpePackList[i].remain_times++;
				SpePackList[i].state = 2L;
				MainPage.UpdateInfo(SpePackList[i]);
				SingletonDontDestoryUnity<GameManager>.Instance.FlurryLogEventMap("Welfare", "Mystery", $"require_{id}");
			}
			if (SpePackList[i].state == 0L)
			{
				flag = false;
			}
		}
		if (flag && SingletonUnity<FunctionBtnRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<FunctionBtnRootLogic>.Instance.gameObject))
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.HideMysterySaleBtn();
		}
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
			UnityVersionUtil.SetActiveRecursive(TempPage.gameObject, state: true);
			TempPage.RefershInfo(SpePackList[CurShowIndex]);
			SwapPage();
			SelectPoint();
		}
	}

	public void OnClickRightBtn()
	{
		if (CurShowIndex < SpePackList.Count - 1)
		{
			isMoveFlag = true;
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
			UnityVersionUtil.SetActiveRecursive(TempPage.gameObject, state: true);
			TempPage.RefershInfo(SpePackList[CurShowIndex]);
			SwapPage();
			SelectPoint();
		}
	}

	private void Update()
	{
		if (isMoveFlag)
		{
			temptime += Time.deltaTime;
			if (temptime >= MoveTime)
			{
				temptime -= MoveTime;
				isMoveFlag = false;
			}
		}
	}

	public void OnClickBackBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MysteryShopRoot);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MenuBaseRootUI);
		if (SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
		{
			SingletonUnity<AutoPopUIRoot>.Instance.NextPop();
		}
	}

	public void SelectPoint()
	{
		for (int i = 0; i < PointList.Count; i++)
		{
			if (i == CurShowIndex)
			{
				PointList[i].SetDimensions(13, 13);
				PointList[i].color = Color.white;
			}
			else
			{
				PointList[i].SetDimensions(10, 10);
				PointList[i].color = Color.gray;
			}
		}
		if (CurShowIndex == 0)
		{
			LeftSp.alpha = 0f;
		}
		else
		{
			LeftSp.alpha = 1f;
		}
		if (CurShowIndex == SpePackList.Count - 1)
		{
			RightSp.alpha = 0f;
		}
		else
		{
			RightSp.alpha = 1f;
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
	}

	public void SwapPage()
	{
		TweenPosition mainAnima = MainAnima;
		MainAnima = TempAnima;
		TempAnima = mainAnima;
		MysteryShopPageLogic mainPage = MainPage;
		MainPage = TempPage;
		TempPage = mainPage;
	}
}
