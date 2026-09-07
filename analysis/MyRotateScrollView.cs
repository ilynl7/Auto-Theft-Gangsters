using System;
using System.Collections.Generic;
using UnityEngine;

public class MyRotateScrollView : MonoBehaviour
{
	public List<UIWidget> PicList;

	public List<UIEventListener> PicListener;

	private int mCurTopIndex;

	private Vector3 mTopPos;

	private Vector3 mBottomPos;

	private Vector3 mDeltaPos;

	private List<Vector3> mPosList = new List<Vector3>();

	private int mTopDepth;

	private float mMovePercent;

	private float mMoveSpeed = 4f;

	private int mPicCount;

	private Vector3[] mPrePos;

	public DelegateDefine.OneIntParamDelegate onMoveOver;

	public bool EnableChangeFlag;

	private bool mLeftFlag;

	private bool dragLeft;

	private void Start()
	{
		Reset();
	}

	public void Reset()
	{
		mTopPos = PicList[0].transform.position;
		mBottomPos = PicList[PicList.Count - 1].transform.position;
		mTopDepth = PicList[0].depth;
		mDeltaPos = (mTopPos - mBottomPos) / (PicList.Count - 1);
		for (int i = 0; i < PicList.Count; i++)
		{
			mPosList.Add(mTopPos - mDeltaPos * i);
			PicList[i].transform.position = mPosList[i];
		}
		mCurTopIndex = 0;
		mMovePercent = 1f;
		mPicCount = PicList.Count;
		mPrePos = new Vector3[PicList.Count];
		for (int j = 0; j < PicList.Count; j++)
		{
			UIEventListener uIEventListener = PicListener[j];
			uIEventListener.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(uIEventListener.onDrag, new UIEventListener.VectorDelegate(OnDragPic));
			UIEventListener uIEventListener2 = PicListener[j];
			uIEventListener2.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener2.onPress, new UIEventListener.BoolDelegate(OnPress));
		}
	}

	public void MoveLeft()
	{
		if (EnableChangeFlag)
		{
			EnableChangeFlag = false;
			mCurTopIndex = (mCurTopIndex + mPicCount - 1) % mPicCount;
			mMovePercent = 0f;
			mLeftFlag = true;
			SetPicColor();
			for (int i = 0; i < PicList.Count; i++)
			{
				ref Vector3 reference = ref mPrePos[i];
				reference = PicList[i].transform.position;
			}
			onMoveOver(mCurTopIndex);
		}
	}

	public void MoveRight()
	{
		if (EnableChangeFlag)
		{
			EnableChangeFlag = false;
			mCurTopIndex = (mCurTopIndex + mPicCount + 1) % mPicCount;
			mMovePercent = 0f;
			SetPicTop();
			SetPicColor();
			for (int i = 0; i < PicList.Count; i++)
			{
				ref Vector3 reference = ref mPrePos[i];
				reference = PicList[i].transform.position;
			}
			onMoveOver(mCurTopIndex);
		}
	}

	private void SetPicTop()
	{
		for (int i = 0; i < PicList.Count; i++)
		{
			PicList[(i + mCurTopIndex) % mPicCount].depth = mTopDepth - i;
		}
	}

	private void SetPicColor()
	{
		for (int i = 0; i < PicList.Count; i++)
		{
			if (i == 0)
			{
				PicList[(i + mCurTopIndex) % mPicCount].color = Color.white;
			}
			else
			{
				PicList[(i + mCurTopIndex) % mPicCount].color = Color.gray;
			}
		}
	}

	public void Update()
	{
		if (mMovePercent < 1f)
		{
			mMovePercent += mMoveSpeed * Time.deltaTime;
			for (int i = 0; i < PicList.Count; i++)
			{
				PicList[(i + mCurTopIndex) % mPicCount].transform.position = Vector3.Lerp(mPrePos[(i + mCurTopIndex) % mPicCount], mPosList[i], mMovePercent);
			}
		}
		else if (mLeftFlag)
		{
			mLeftFlag = false;
			SetPicTop();
		}
		if (Input.GetKeyUp(KeyCode.LeftArrow))
		{
			EnableChangeFlag = true;
		}
	}

	public void OnDragPic(GameObject obj, Vector2 deltaPos)
	{
		if (deltaPos.x > 0f)
		{
			dragLeft = false;
		}
		else
		{
			dragLeft = true;
		}
	}

	public void OnPress(GameObject obj, bool isPress)
	{
		if (!isPress)
		{
			if (dragLeft)
			{
				MoveLeft();
			}
			else
			{
				MoveRight();
			}
		}
		else
		{
			dragLeft = false;
		}
	}
}
