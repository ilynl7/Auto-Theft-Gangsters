using UnityEngine;

public class SkillMotion : MonoBehaviour
{
	public delegate void Finish();

	private Vector3 mMoveStartPos;

	private Vector3 mMoveTargetPos;

	private float mMoveTime;

	private float mMoveStartTime;

	private bool mNeedMoveFlag;

	private float movePercent;

	private ObjCharacter ownner;

	public Finish MoveArrive;

	private vp_Timer.Handle handle = new vp_Timer.Handle();

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

	public void Init(ObjCharacter objCharacter)
	{
		ownner = objCharacter;
	}

	public void SkillMoveTo(Vector3 targetPos, float time, float delay)
	{
	}

	public void ResetSkillMove(SkillData skillData, float delayTime, ObjCharacter target)
	{
		if (delayTime > 0f)
		{
			if (mNeedMoveFlag)
			{
				handle.Cancel();
			}
			vp_Timer.In(delayTime, delegate
			{
				ResetSkillMove(skillData, target);
			}, handle);
		}
		else
		{
			ResetSkillMove(skillData, target);
		}
	}

	private void ResetSkillMove(SkillData skillData, ObjCharacter target)
	{
		mMoveStartPos = ownner.Position;
		float num = 0f;
		num = ((skillData.MoveAngle != -1) ? ((float)skillData.MoveAngle) : ((float)Random.Range(0, 360)));
		if (skillData.AutoMoveFlag == 0 || target == null)
		{
			mMoveTargetPos = Quaternion.AngleAxis(num, Vector3.up) * ownner.CacheTransform.forward * skillData.MoveDistanceMeter + ownner.Position;
		}
		else
		{
			if (!(target != null))
			{
				return;
			}
			mMoveTargetPos = target.Position - (target.Position - ownner.Position).normalized * (target.ModelRadius + ownner.ModelRadius + 0.5f);
		}
		if (NavMesh.Raycast(mMoveStartPos, mMoveTargetPos, out var hit, ownner.NavMeshAgent.walkableMask))
		{
			mMoveTime = skillData.MoveTimeSecond * VectorXZ.Distance(hit.position, mMoveStartPos) / skillData.MoveDistanceMeter;
			mMoveTargetPos = hit.position;
		}
		else if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsTutorialScene())
		{
			if (Physics.Raycast(mMoveStartPos, (mMoveTargetPos - mMoveStartPos).normalized, out var hitInfo, Vector3.Distance(mMoveTargetPos, mMoveStartPos), int.MinValue))
			{
				mMoveTime = skillData.MoveTimeSecond * VectorXZ.Distance(hitInfo.point, mMoveStartPos) / skillData.MoveDistanceMeter;
				mMoveTargetPos = hitInfo.point;
			}
			else
			{
				mMoveTime = skillData.MoveTimeSecond;
			}
		}
		else
		{
			mMoveTime = skillData.MoveTimeSecond;
		}
		mMoveTargetPos = new Vector3(mMoveTargetPos.x, SceneManager.GetHitHeight(mMoveTargetPos), mMoveTargetPos.z);
		if (mMoveTime <= 0.1f)
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
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void UpdateSkillMotion()
	{
		if (NeedMoveFlag)
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

	public void BreakCurSkillMotion()
	{
		NeedMoveFlag = false;
		movePercent = 0f;
	}
}
