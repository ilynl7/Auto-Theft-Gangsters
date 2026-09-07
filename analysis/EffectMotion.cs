using System.Collections.Generic;
using UnityEngine;

public class EffectMotion : MonoBehaviour
{
	private ObjCharacter ownner;

	public List<PlayEffInfoMotionData> PlayEffInfoMotionDataList = new List<PlayEffInfoMotionData>();

	private Vector3 mMoveStartPos;

	private Vector3 mMoveTargetPos;

	private float mMoveTime;

	private float mMoveStartTime;

	private bool mNeedMoveFlag;

	private bool mMoveLockFlag;

	private float moveLockTimeCount;

	private float movePercent;

	public Vector3 MoveStartPos
	{
		get
		{
			return mMoveStartPos;
		}
		set
		{
			mMoveStartPos = value;
		}
	}

	public Vector3 MoveTargetPos
	{
		get
		{
			return mMoveTargetPos;
		}
		set
		{
			mMoveTargetPos = value;
		}
	}

	public float MoveTime
	{
		get
		{
			return mMoveTime;
		}
		set
		{
			mMoveTime = value;
		}
	}

	public float MoveStartTime
	{
		get
		{
			return mMoveStartTime;
		}
		set
		{
			mMoveStartTime = value;
		}
	}

	public bool NeedMoveFlag
	{
		get
		{
			return mNeedMoveFlag;
		}
		set
		{
			mNeedMoveFlag = value;
		}
	}

	public bool MoveLockFlag
	{
		get
		{
			return mMoveLockFlag;
		}
		set
		{
			mMoveLockFlag = value;
		}
	}

	public void Init(ObjCharacter objCharacter)
	{
		ownner = objCharacter;
	}

	public void AddPlayEffInfoMotionData(string effInfoId, float delayTime, Vector3 senderPos)
	{
		if (!ownner.IsDie)
		{
			PlayEffInfoMotionDataList.Add(new PlayEffInfoMotionData(effInfoId, delayTime, senderPos));
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		UpdateEffectMotion();
	}

	public void UpdateEffectMotion()
	{
		if (!mNeedMoveFlag)
		{
			if (PlayEffInfoMotionDataList.Count > 0)
			{
				for (int i = 0; i < PlayEffInfoMotionDataList.Count; i++)
				{
					PlayEffInfoMotionDataList[i].DelayTime -= Time.deltaTime;
				}
				if (PlayEffInfoMotionDataList[0].DelayTime <= 0f)
				{
					ResetMotion();
				}
			}
		}
		else
		{
			movePercent = (Time.time - mMoveStartTime) / mMoveTime;
			ownner.Position = Vector3.Lerp(mMoveStartPos, mMoveTargetPos, movePercent);
			if (movePercent >= 1f)
			{
				movePercent = 0f;
				NeedMoveFlag = false;
			}
		}
	}

	public void ResetMotion()
	{
		EffInfoData effInfoData = PlayEffInfoMotionDataList[0].EffInfoData;
		mNeedMoveFlag = true;
		mMoveStartPos = ownner.Position;
		if (effInfoData.MoveAngle == -1)
		{
			effInfoData.MoveAngle = Random.Range(0, 360);
		}
		if (effInfoData.ForceMove != 1)
		{
			ownner.FaceToPub(PlayEffInfoMotionDataList[0].SenderPos);
		}
		mMoveTargetPos = Quaternion.AngleAxis(effInfoData.MoveAngle, Vector3.up) * (ownner.CacheTransform.forward * -1f) * effInfoData.MoveDistanceMeter + ownner.Position;
		if (NavMesh.Raycast(mMoveStartPos, mMoveTargetPos, out var hit, 15))
		{
			mMoveTime = effInfoData.MoveTimeSecond * VectorXZ.Distance(hit.position, mMoveStartPos) / effInfoData.MoveDistanceMeter;
			mMoveTargetPos = hit.position;
		}
		else
		{
			mMoveTime = effInfoData.MoveTimeSecond;
		}
		if (MoveTime <= 0.1f)
		{
			NeedMoveFlag = false;
			movePercent = 0f;
		}
		else
		{
			NeedMoveFlag = true;
			movePercent = 0f;
			mMoveStartTime = Time.time;
		}
		PlayEffInfoMotionDataList.RemoveAt(0);
	}

	public void BreakCurEffectMotion()
	{
		NeedMoveFlag = false;
		movePercent = 0f;
		PlayEffInfoMotionDataList.Clear();
	}
}
