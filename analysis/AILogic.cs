using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILogic : MonoBehaviour
{
	private ObjNPC mOwner;

	public AISTATE curState;

	private StateMachine<AILogic> mStateMachine;

	private AISTATE mAttackState;

	private float mDelayMoveTimeCount;

	public bool mOutOffRangeFlag;

	public bool mReturnBackFlag;

	private float mStartChaseTime;

	public float ChaseTime = 2f;

	private float mUpdateIntervalCount;

	private float mUpdateInterval = 0.2f;

	private bool mUpdateEnableFlag;

	private bool mEnableActionFlag;

	private AIData mAiData;

	private float mAroundTime;

	private float mEnterAroundStateTime;

	private float mActionTime;

	private float mEnterAttackStateTime;

	private string mPathId;

	private List<NPCPathData> mPathList;

	private int mCurPathIndex;

	private string mCurUseSkillID;

	private SceneManager mSceneManager;

	public ObjNPC Ownner
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

	public AISTATE AttackState
	{
		get
		{
			return mAttackState;
		}
		set
		{
			mAttackState = value;
		}
	}

	public float DelayMoveTimeCount
	{
		get
		{
			return mDelayMoveTimeCount;
		}
		set
		{
			mDelayMoveTimeCount = value;
		}
	}

	public bool OutOffRangeFlag
	{
		get
		{
			return mOutOffRangeFlag;
		}
		set
		{
			mOutOffRangeFlag = value;
		}
	}

	public bool ReturnBackFlag
	{
		get
		{
			return mReturnBackFlag;
		}
		set
		{
			mReturnBackFlag = value;
		}
	}

	public float StartChaseTime
	{
		get
		{
			return mStartChaseTime;
		}
		set
		{
			mStartChaseTime = value;
		}
	}

	public bool UpdateEnableFlag
	{
		get
		{
			return mUpdateEnableFlag;
		}
		set
		{
			mUpdateEnableFlag = value;
		}
	}

	public bool EnableActionFlag
	{
		get
		{
			return mEnableActionFlag;
		}
		set
		{
			mEnableActionFlag = value;
		}
	}

	public AIData AiData
	{
		get
		{
			return mAiData;
		}
		set
		{
			mAiData = value;
		}
	}

	public float AroundTime
	{
		get
		{
			return mAroundTime;
		}
		set
		{
			mAroundTime = value;
		}
	}

	public float EnterAroundStateTime
	{
		get
		{
			return mEnterAroundStateTime;
		}
		set
		{
			mEnterAroundStateTime = value;
		}
	}

	public float ActionTime
	{
		get
		{
			return mActionTime;
		}
		set
		{
			mActionTime = value;
		}
	}

	public float EnterAttackStateTime
	{
		get
		{
			return mEnterAttackStateTime;
		}
		set
		{
			mEnterAttackStateTime = value;
		}
	}

	public string PathId
	{
		get
		{
			return mPathId;
		}
		set
		{
			mPathId = value;
		}
	}

	public List<NPCPathData> PathList
	{
		get
		{
			return mPathList;
		}
		set
		{
			mPathList = value;
		}
	}

	public int CurPathIndex
	{
		get
		{
			return mCurPathIndex;
		}
		set
		{
			mCurPathIndex = value;
		}
	}

	public string CurUseSkillID
	{
		get
		{
			return mCurUseSkillID;
		}
		set
		{
			mCurUseSkillID = value;
		}
	}

	public SceneManager CurSceneManager
	{
		get
		{
			if (mSceneManager == null)
			{
				mSceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			}
			return mSceneManager;
		}
	}

	public void InitAILogic()
	{
		mOwner = base.gameObject.GetComponent<ObjNPC>();
		if (mStateMachine == null)
		{
			mStateMachine = new StateMachine<AILogic>(this, 4);
		}
		mStateMachine.AddState(Singleton<PatrolState>.Instance, 0);
		mStateMachine.AddState(Singleton<DecisionState>.Instance, 3);
		RegisterEvent();
		mUpdateEnableFlag = true;
	}

	public void ResetAI(string ai, string aiID, string pathID)
	{
		mAiData = DataManager.GetAIDataByID(aiID);
		if (mStateMachine != null)
		{
			mUpdateEnableFlag = true;
		}
		else
		{
			InitAILogic();
		}
		mPathId = pathID;
		if (!string.IsNullOrEmpty(mPathId))
		{
			mPathList = DataManager.GetNPCPathDataListById(mPathId);
			mCurPathIndex = 0;
		}
		if (string.IsNullOrEmpty(ai))
		{
			mStateMachine.AddState(Singleton<global::AttackState>.Instance, 1);
			mAttackState = AISTATE.ATTACK_STATE;
			mStateMachine.AddState(Singleton<AroundState>.Instance, 2);
		}
		else if (ai.Equals("BlockAI"))
		{
			mStateMachine.AddState(Singleton<BlockAINoAttackState>.Instance, 1);
			mAttackState = AISTATE.ATTACK_STATE;
		}
		else if (ai.Equals("RandomSkill"))
		{
			mStateMachine.AddState(Singleton<global::AttackState>.Instance, 1);
			mAttackState = AISTATE.ATTACK_STATE;
			mStateMachine.AddState(Singleton<AroundState>.Instance, 2);
		}
		else if (ai.Equals("FollowAI"))
		{
			mStateMachine.AddState(Singleton<BlockAINoAttackState>.Instance, 1);
			mAttackState = AISTATE.ATTACK_STATE;
			mStateMachine.AddState(Singleton<FollowState>.Instance, 0);
		}
		ChangeState(0);
		mEnableActionFlag = true;
	}

	public bool IsHavePath()
	{
		return !string.IsNullOrEmpty(mPathId);
	}

	private IEnumerator UpdateSecond()
	{
		while (true)
		{
			yield return new WaitForSeconds(0.2f);
			if (mStateMachine.CurrentState == null)
			{
				MonoBehaviour.print("mStateMachine.CurrentState == null");
				ChangeState(0);
			}
			else
			{
				mStateMachine.Update();
			}
		}
	}

	private void Update()
	{
		if (Ownner.IsDie || (!CurSceneManager.IsLowPhoneManager() && !GameManager.OnLineState) || !mEnableActionFlag || !mUpdateEnableFlag)
		{
			return;
		}
		mUpdateIntervalCount += Time.deltaTime;
		if (mUpdateIntervalCount > mUpdateInterval)
		{
			mUpdateIntervalCount = 0f;
			if (mStateMachine.CurrentState == null)
			{
				ChangeState(0);
			}
			else
			{
				mStateMachine.Update();
			}
		}
	}

	public void ChangeState(int newId)
	{
		curState = (AISTATE)newId;
		mStateMachine.SetState(newId);
	}

	public void OnPatrolStateArriveTarget(ObjCharacter objCha)
	{
		mDelayMoveTimeCount = Time.time;
	}

	public void OnStun(BuffInfoData buffInfoData)
	{
		mUpdateEnableFlag = false;
	}

	public void OnStunDone(BuffInfoData buffInfoData)
	{
		mUpdateEnableFlag = true;
	}

	public void OnKnockDown(BuffInfoData buffInfoData, ObjCharacter sender)
	{
		mUpdateEnableFlag = false;
	}

	public void OnKnockDownDone(BuffInfoData buffInfoData)
	{
		mUpdateEnableFlag = true;
	}

	public void OnSleep(BuffInfoData buffInfoData)
	{
		mUpdateEnableFlag = false;
	}

	public void OnSleepDone(BuffInfoData buffInfoData)
	{
		mUpdateEnableFlag = true;
	}

	public void OnBeaton()
	{
		mStateMachine.Notify(0, this);
	}

	public void OnSkillFinished()
	{
		mStateMachine.Notify(1, this);
	}

	private void OnDestroy()
	{
		DeRegisterEvent();
	}

	private void RegisterEvent()
	{
		mOwner.BuffLogic.RegisterOnStun(OnStun);
		mOwner.BuffLogic.RegisterOnStunDone(OnStunDone);
		mOwner.BuffLogic.RegisterOnSleep(OnSleep);
		mOwner.BuffLogic.RegisterOnSleepDone(OnSleepDone);
		mOwner.RegisterOnBeaton(OnBeaton);
		mOwner.RegisterOnSkillFinished(OnSkillFinished);
		mOwner.BuffLogic.RegisterOnKnockDown(OnKnockDown);
		mOwner.BuffLogic.RegisterOnKnockDownDone(OnKnockDownDone);
	}

	private void DeRegisterEvent()
	{
		mOwner.BuffLogic.DeRegisterOnStun(OnStun);
		mOwner.BuffLogic.DeRegisterOnStunDone(OnStunDone);
		mOwner.BuffLogic.DeRegisterOnSleep(OnSleep);
		mOwner.BuffLogic.DeRegisterOnSleepDone(OnSleepDone);
		mOwner.DeRegisterOnBeaton(OnBeaton);
		mOwner.DeRegisterOnSkillFinished(OnSkillFinished);
		mOwner.BuffLogic.DeRegisterOnKnockDown(OnKnockDown);
		mOwner.BuffLogic.DeRegisterOnKnockDownDone(OnKnockDownDone);
	}

	public void EnableAIAction()
	{
		mEnableActionFlag = true;
	}

	public void DisableAIAction()
	{
		mEnableActionFlag = false;
	}
}
