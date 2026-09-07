using UnityEngine;

public class DanceLogic : MonoBehaviour
{
	private bool mDancingFlag;

	private DanceData mCurDanceData;

	private ObjOtherPlayer mOwner;

	private ActionData mCurActionData;

	public bool DancingFlag
	{
		get
		{
			return mDancingFlag;
		}
		set
		{
			mDancingFlag = value;
		}
	}

	public DanceData CurDanceData
	{
		get
		{
			return mCurDanceData;
		}
		set
		{
			mCurDanceData = value;
		}
	}

	public ObjOtherPlayer Owner
	{
		get
		{
			return mOwner;
		}
		set
		{
			mOwner = value;
		}
	}

	public ActionData CurActionData
	{
		get
		{
			return mCurActionData;
		}
		set
		{
			mCurActionData = value;
		}
	}

	public void Reset(ObjOtherPlayer owner)
	{
		mOwner = owner;
	}

	public void StartDance(string danceId)
	{
		mCurDanceData = DataManager.GetDanceDataById(danceId);
		string actionName = mOwner.GetActionName(mCurDanceData.ActionName);
		mCurActionData = DataManager.GetActionDataByName(actionName);
		float startTime = Time.realtimeSinceStartup % mCurActionData.AnimDurationTimeSecond / mCurActionData.AnimDurationTimeSecond;
		mOwner.AnimationLogic.PlayAnimation(mCurActionData, null, -1f, startTime);
		if (!string.IsNullOrEmpty(mCurActionData.FxEffID))
		{
			mOwner.EffectLogic.AddPlayeBufEffInfoData(mCurActionData.FxEffID, 0f, float.MaxValue, mOwner.Position);
		}
	}

	public void StopDance()
	{
		mOwner.EffectLogic.BreakEffect(mCurActionData.FxEffID);
	}
}
