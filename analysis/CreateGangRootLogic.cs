using System.Collections.Generic;
using UnityEngine;

public class CreateGangRootLogic : MonoBehaviour
{
	public List<Transform> IconList;

	public UICenterOnChild CenterOnChild;

	private int mCurIconIndex;

	public UIInput InputGangName;

	private int MinNameCharNum = 5;

	private int MaxNameCharNum = 36;

	private void Start()
	{
		CenterOnChild.RegisterCenterOnEvent(OnCenterOnIcon);
	}

	private void OnEnable()
	{
		CenterOnChild.CenterOn(IconList[0]);
	}

	private void OnCenterOnIcon(Transform target)
	{
		for (int i = 0; i < IconList.Count; i++)
		{
			if (IconList[i] == target)
			{
				mCurIconIndex = i;
				Debug.Log("mCurIconIndex :: " + mCurIconIndex);
				break;
			}
		}
	}

	public void OnClickCreateBtn()
	{
	}

	public void OnClickLeftBtn()
	{
		mCurIconIndex = (mCurIconIndex + IconList.Count - 1) % IconList.Count;
		CenterOnChild.CenterOn(IconList[mCurIconIndex]);
	}

	public void OnClickRightBtn()
	{
		mCurIconIndex = (mCurIconIndex + 1) % IconList.Count;
		CenterOnChild.CenterOn(IconList[mCurIconIndex]);
	}
}
