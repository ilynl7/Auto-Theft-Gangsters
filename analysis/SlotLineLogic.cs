using System.Collections.Generic;
using UnityEngine;

public class SlotLineLogic : MonoBehaviour
{
	private Transform mTransform;

	public int lineIndex;

	public List<SlotLineItemLogic> lineitems;

	private List<SlotIconData> slotIconList;

	private int CellsitemNum = 10;

	public float RollSpeed;

	public float MaxRollSpeed;

	private float startY;

	public int RollCount;

	private int delRollCount;

	public bool isRolling;

	private DelegateDefine.NoParamDelegate backfun;

	public string DesStr;

	public void Reset(List<SlotIconData> iconlist, int line)
	{
		if (mTransform == null)
		{
			mTransform = base.transform;
		}
		lineIndex = line;
		slotIconList = ListRandom(iconlist);
		int num = slotIconList.Count - lineitems.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(lineitems[0].gameObject) as GameObject;
				gameObject.transform.parent = base.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				SlotLineItemLogic component = gameObject.GetComponent<SlotLineItemLogic>();
				lineitems.Add(component);
				gameObject.gameObject.name = $"icon{lineitems.Count - 1:D2}";
			}
		}
		for (int j = 0; j < lineitems.Count; j++)
		{
			if (j < slotIconList.Count)
			{
				lineitems[j].Reset(slotIconList[j], j);
			}
			else
			{
				NGUITools.SetActive(lineitems[j].gameObject, state: false);
			}
		}
		mTransform.localPosition = new Vector3(lineIndex * 128 + -128, 0f, 0f);
		isRolling = false;
		backfun = null;
	}

	public void StopRolling(string des)
	{
		isRolling = false;
		DesStr = des;
		int num = -1;
		for (int i = 0; i < lineitems.Count; i++)
		{
			if (lineitems[i].curiconinfo.ID.Equals(DesStr))
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			ChangeSortCells(num);
			mTransform.localPosition = new Vector3(lineIndex * 128 + -128, 0f, 0f);
			for (int j = 0; j < lineitems.Count; j++)
			{
				lineitems[j].ResetPos(j);
			}
		}
		if (backfun != null)
		{
			backfun();
		}
	}

	private void FixedUpdate()
	{
		if (isRolling)
		{
			mTransform.localPosition = new Vector3(lineIndex * 128 + -128, startY, 0f);
			startY += RollSpeed * Time.deltaTime;
			delRollCount = (int)(Mathf.Abs(mTransform.localPosition.y) / 90f) - RollCount;
			if (delRollCount >= 0)
			{
				ChangeCells(delRollCount);
				RollCount += delRollCount + 1;
			}
		}
	}

	public void ChangeCells(int delcount)
	{
		delcount++;
		List<SlotLineItemLogic> list = new List<SlotLineItemLogic>();
		int num = 0;
		int cellsitemNum = CellsitemNum;
		int num2 = CellsitemNum - delcount;
		for (int i = num2; i < CellsitemNum; i++)
		{
			list.Add(lineitems[i]);
			lineitems[i].idx = --delcount;
			lineitems[i].SetMoveToUp(RollCount - delcount);
			num++;
		}
		for (int num3 = CellsitemNum - 1; num3 >= num2; num3--)
		{
		}
		for (int j = 0; j < num2; j++)
		{
			list.Add(lineitems[j]);
			lineitems[j].idx = num++;
		}
		lineitems = list;
	}

	public void ChangeSortCells(int desindex)
	{
		List<SlotLineItemLogic> list = new List<SlotLineItemLogic>();
		int num = desindex - 4;
		if (num < 0)
		{
			num += CellsitemNum;
		}
		for (int i = 0; i < CellsitemNum; i++)
		{
			list.Add(lineitems[(i + num) % CellsitemNum]);
		}
		lineitems = list;
	}

	public void StartSpin(DelegateDefine.NoParamDelegate stopfun)
	{
		backfun = stopfun;
		isRolling = true;
		startY = mTransform.localPosition.y;
		RollCount = (int)Mathf.Abs(mTransform.localPosition.y) / 90 + 1;
		MaxRollSpeed = -3000f;
		RollSpeed = MaxRollSpeed;
	}

	private List<SlotIconData> ListRandom(List<SlotIconData> myList)
	{
		List<SlotIconData> list = new List<SlotIconData>();
		int num = 0;
		for (int i = 0; i < myList.Count; i++)
		{
			num = Random.Range(0, myList.Count - 1);
			if (num != i)
			{
				SlotIconData value = myList[i];
				myList[i] = myList[num];
				myList[num] = value;
			}
		}
		return myList;
	}
}
