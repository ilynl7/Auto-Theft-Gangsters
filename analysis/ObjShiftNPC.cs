using System.Collections.Generic;
using UnityEngine;

public class ObjShiftNPC : ObjNPC
{
	private int[] mShiftingHP;

	private int mShiftingState;

	private int mShiftStateCount;

	private string[] mShiftSkill;

	private List<string[]> mShiftSkillGroup;

	private bool mIsShifting;

	public int[] ShiftingHP
	{
		get
		{
			return mShiftingHP;
		}
		set
		{
			mShiftingHP = value;
		}
	}

	public int ShiftingState
	{
		get
		{
			return mShiftingState;
		}
		set
		{
			mShiftingState = value;
		}
	}

	public int ShiftStateCount
	{
		get
		{
			return mShiftStateCount;
		}
		set
		{
			mShiftStateCount = value;
		}
	}

	public string[] ShiftSkill => mShiftSkill;

	public List<string[]> ShiftSkillGroup => mShiftSkillGroup;

	public bool IsShifting => mIsShifting;

	public new virtual void ResetNpc(ObjInitNpcData initData)
	{
		base.Reset();
		ServerId = initData.mServerID;
		base.Position = initData.mPos;
		mTransform.forward = initData.mDir;
		mNpcData = initData.npcInfoData;
		base.BornPos = initData.mPos;
		AttributeData.Camp = (GameDefine.CAMP_TYPE)initData.npcInfoData.Group;
		mNPCFunctionType = (GameDefine.NPC_FUNCTION_TYPE)initData.npcInfoData.FunctionType;
		mNPCType = (GameDefine.NPC_TYPE)initData.npcInfoData.Type;
		AttributeData.HP = initData.HP;
		AttributeData.MaxHP = initData.MaxHP;
		AttributeData.CurATK = initData.npcInfoData.Atk;
		AttributeData.CurDEF = initData.npcInfoData.Def;
		AttributeData.Name = initData.npcInfoData.Name;
		AttributeData.CurEXD = (float)mNpcData.EXD / 10000f;
		AttributeData.CurEXR = (float)mNpcData.EXR / 10000f;
		AttributeData.CurHIT = mNpcData.HIT;
		AttributeData.CurDGE = mNpcData.DGE;
		AttributeData.CurCRI = mNpcData.CRI;
		AttributeData.CurRES = mNpcData.RES;
		AttributeData.CurCRD = (float)mNpcData.CRD / 10000f;
		AttributeData.CurCRR = (float)mNpcData.CRR / 10000f;
		AttributeData.CurDEFA = mNpcData.DEFA;
		AttributeData.CurSpeed = initData.npcInfoData.MoveSpeedMeter;
		AttributeData.WalkSpeed = initData.npcInfoData.WalkSpeedMeter;
		mPatrolRange = initData.npcInfoData.PatrolRadius;
		mSearchRange = initData.npcInfoData.SearchRadius;
		mPathID = initData.PathID;
		mDefaultDialogID = initData.npcInfoData.TalkGroup;
		InitSkill(mNpcData.SkillList);
		InitNavMeshAgent();
		InitNPCHeadInfo();
		AddDialogMission();
		if (mTransform.childCount > 0)
		{
			Transform child = mTransform.GetChild(0);
			child.localScale = Vector3.one * initData.npcInfoData.ModelScale;
		}
		CapsuleCollider component = base.gameObject.GetComponent<CapsuleCollider>();
		if (component != null)
		{
			component.height = base.CurrentCharacterModelData.ModelHeight;
			component.center = Vector3.up * base.CurrentCharacterModelData.ModelHeight / 2f;
			component.radius = base.CurrentCharacterModelData.ModelRadius;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsSingleCopyScene())
		{
			if (!IsMissionNpc())
			{
				if (mAILogic == null)
				{
					mAILogic = base.gameObject.AddComponent<AILogic>();
				}
				if (mAILogic != null)
				{
					mAILogic.ResetAI(mNpcData.AI, mNpcData.AIID, initData.PathID);
				}
			}
		}
		else if (mNpcData.AI.Equals("FollowAI"))
		{
			if (mAILogic == null)
			{
				mAILogic = base.gameObject.AddComponent<AILogic>();
			}
			if (mAILogic != null)
			{
				mAILogic.ResetAI(mNpcData.AI, mNpcData.AIID, initData.PathID);
			}
		}
		else
		{
			if (mAILogic != null)
			{
				Object.Destroy(mAILogic);
				mAILogic = null;
			}
			if (IsMissionNpc())
			{
				mNavMeshAgent.enabled = false;
			}
			else
			{
				mNavMeshAgent.enabled = true;
			}
		}
		mShiftingState = 0;
		mShiftingHP = mNpcData.ShiftHP;
		mShiftSkill = mNpcData.ShiftSkill;
		mShiftSkillGroup = mNpcData.ShiftSkillGroup;
		mShiftStateCount = mNpcData.ShiftStateCount;
		mIsShifting = false;
	}

	public override void ChangeHPVal(long newHP)
	{
		if (base.IsDie)
		{
			return;
		}
		AttributeData.HP = newHP;
		if (mNPCType == GameDefine.NPC_TYPE.BOSS)
		{
			SingletonUnity<BossXueTiaoLogicNew>.Instance.ChangeHP(newHP, this);
		}
		else
		{
			UpdateHeadInfo();
		}
		if (AttributeData.HP <= 0)
		{
			if (mShiftingState < mShiftStateCount)
			{
				Shifting();
			}
			else
			{
				OnDie();
			}
		}
	}

	public override void ChangeHPEffect(long newHP, GameDefine.DAMAGEBOARD_TYPE type)
	{
		if (!base.IsDie && !mIsShifting)
		{
			long num = AttributeData.HP - newHP;
			if (num > 0)
			{
				UpdateDamgeBoard(type, num);
			}
			else
			{
				UpdateDamgeBoard(type, num);
			}
			AttributeData.HP = newHP;
			if (mNPCType == GameDefine.NPC_TYPE.BOSS)
			{
				SingletonUnity<BossXueTiaoLogicNew>.Instance.ChangeHP(newHP, this);
			}
			else
			{
				UpdateHeadInfo();
			}
		}
	}

	public void Shifting()
	{
		mIsShifting = true;
		mAILogic.enabled = false;
		DisableNavMeshAgent();
		if (mSkillLogic.IsUsingSkill)
		{
			mSkillLogic.BreakCurSkill();
		}
		if (base.IsMoving)
		{
			StopMove();
		}
		vp_Timer.In(3f, delegate
		{
			AttributeData.HP = mShiftingHP[mShiftingState];
			if (mNPCType == GameDefine.NPC_TYPE.BOSS)
			{
				SingletonUnity<BossXueTiaoLogicNew>.Instance.ChangeHP(AttributeData.HP, this);
			}
			else
			{
				UpdateHeadInfo();
			}
			InitSkill(mShiftSkillGroup[mShiftingState]);
			base.SkillLogic.UseSkill(mShiftSkill[mShiftingState], ServerId, -1L);
			RegisterOnSkillFinished(delegate
			{
				vp_Timer.In(3f, delegate
				{
					mIsShifting = false;
					mAILogic.enabled = true;
					EnableNavMeshAgent();
				});
			});
			mShiftingState++;
		});
	}
}
