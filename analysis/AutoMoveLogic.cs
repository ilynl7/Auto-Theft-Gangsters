using System.Collections.Generic;
using UnityEngine;

public class AutoMoveLogic : MonoBehaviour
{
	private List<MovePoint> mMovePosList = new List<MovePoint>();

	private int curIndex;

	private ObjCharacter owner;

	private float currentAngle;

	private float targetAngel;

	private float mObstacleCheckInterval = 2f;

	private float mObstacleCheckTime;

	private Vector3 mLastPos = Vector3.zero;

	private bool mIsMoving => owner.IsMoving;

	public void Init(ObjCharacter objOwner)
	{
		owner = objOwner;
		targetAngel = (currentAngle = MathUtil.Heading(owner.CacheTransform.forward));
		curIndex = 0;
		mMovePosList.Clear();
	}

	public void Reset()
	{
		curIndex = 0;
		owner.StopMove();
		mMovePosList.Clear();
	}

	public void AddMovePoint(MovePoint point)
	{
		if (owner.CurPlayerState != PLAYER_STATE.DANCE)
		{
			if (mMovePosList.Count > 32)
			{
				mMovePosList.Clear();
			}
			if (owner.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
			{
				mMovePosList.Clear();
				mMovePosList.Add(point);
			}
			else
			{
				mMovePosList.Add(point);
			}
		}
	}

	public void PrintMoveList()
	{
		for (int i = 0; i < mMovePosList.Count; i++)
		{
			MonoBehaviour.print(mMovePosList[i].Index + " : " + mMovePosList[i].xPos + "," + mMovePosList[i].zPos);
		}
	}

	private void ReachPoint()
	{
		mLastPos = owner.Position;
		if (mMovePosList.Count > 0)
		{
			mMovePosList.RemoveAt(0);
			if (mMovePosList.Count <= 0)
			{
				StopMove();
			}
			else
			{
				MoveTo(mMovePosList[0]);
			}
		}
	}

	private void MoveTo(MovePoint point)
	{
		targetAngel = point.O;
		if (!owner.IsDie)
		{
			mObstacleCheckTime = Time.time;
			curIndex = point.Index;
			Vector3 pos = new Vector3(point.xPos, owner.Position.y + 0.1f, point.zPos);
			if (owner.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
			{
				if (point.walk)
				{
					owner.WalkMoveTo(pos, 0f);
				}
				else
				{
					owner.MoveTo(pos, 0f);
				}
			}
			else
			{
				owner.MoveTo(pos, 0f);
			}
		}
		else
		{
			StopMove();
		}
	}

	public void StopMove()
	{
		owner.StopMove();
		Reset();
	}

	public void InterruptMove(MovePoint point)
	{
		int num = 0;
		if (num < mMovePosList.Count)
		{
			if (point.Index == mMovePosList[num].Index && num < mMovePosList.Count - 1)
			{
				mMovePosList.RemoveRange(num + 1, mMovePosList.Count - num - 1);
			}
			mMovePosList.RemoveAt(num);
			mMovePosList.Add(point);
			if (num == 0)
			{
				MoveTo(mMovePosList[num]);
			}
		}
	}

	private void ObstacleCheck()
	{
		if (!(Time.time - mObstacleCheckTime > mObstacleCheckInterval))
		{
			return;
		}
		mObstacleCheckTime = Time.time;
		if (mIsMoving && mMovePosList.Count > 0)
		{
			if (VectorXZ.Distance(mLastPos, owner.Position) < 0.1f)
			{
				Vector3 position = new Vector3(mMovePosList[0].xPos, 0f, mMovePosList[0].zPos);
				owner.CacheTransform.position = position;
				ReachPoint();
			}
			mLastPos = owner.Position;
		}
	}

	private void UpdateAngel()
	{
		currentAngle = MathUtil.WrapDegrees(Mathf.Lerp(currentAngle, targetAngel, 10f * Time.deltaTime));
		owner.CacheTransform.forward = MathUtil.HeadingToVector3(currentAngle);
	}

	private bool IsReachPoint(MovePoint p)
	{
		VectorXZ a = new VectorXZ(p.xPos, p.zPos);
		VectorXZ b = new VectorXZ(owner.Position.x, owner.Position.z);
		float num = 0.5f;
		if (VectorXZ.Distance(a, b) < num)
		{
			return true;
		}
		return false;
	}

	private void CheckMove()
	{
		if (mMovePosList.Count <= 0)
		{
			return;
		}
		if (mIsMoving)
		{
			if (IsReachPoint(mMovePosList[0]))
			{
				ReachPoint();
			}
			else
			{
				MoveTo(mMovePosList[0]);
			}
		}
		else
		{
			MoveTo(mMovePosList[0]);
		}
	}

	private void FixedUpdate()
	{
		ObstacleCheck();
		CheckMove();
	}
}
