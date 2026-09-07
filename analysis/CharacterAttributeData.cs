using System;
using SprotoType;
using UnityEngine;

[Serializable]
public class CharacterAttributeData
{
	private string mName = string.Empty;

	private string mGuildName = string.Empty;

	private long mGuildId = -1L;

	private long mHP = -1L;

	private int mLevel = 1;

	private long mMaxHP = long.MaxValue;

	private int mATK;

	private XorFloat mCurATK = new XorFloat();

	private int mHIT;

	private XorFloat mCurHIT = new XorFloat();

	private int mCRI;

	private XorFloat mCurCRI = new XorFloat();

	private float mRES;

	private XorFloat mCurRES = new XorFloat();

	private int mDEF;

	private XorFloat mCurDEF = new XorFloat();

	private int mDGE;

	private XorFloat mCurDGE = new XorFloat();

	private float mEXD;

	private XorFloat mCurEXD = new XorFloat();

	private float mEXR;

	private XorFloat mCurEXR = new XorFloat();

	private XorFloat mCurCRD = new XorFloat();

	private float mCRD;

	private XorFloat mCurCRR = new XorFloat();

	private float mCRR;

	public long mCurEXP;

	public int mDEFA;

	private XorInt mCurDEFA = new XorInt();

	public int mDGEA;

	private XorInt mCurDGEA = new XorInt();

	public int mRESA;

	private XorInt mCurRESA = new XorInt();

	public int mHITA;

	private XorInt mCurHITA = new XorInt();

	public int mCRIA;

	private XorInt mCurCRIA = new XorInt();

	public int mATE;

	private XorInt mCurATE = new XorInt();

	public int mSATM;

	private XorInt mCurSATM = new XorInt();

	public int mSATC;

	private XorInt mCurSATC = new XorInt();

	public int mSATP;

	private XorInt mCurSATP = new XorInt();

	private int mCurTitleLevel;

	private int mCurTitleExp;

	private int mBLK;

	private float mCurBLK;

	private XorFloat speedFloat1 = new XorFloat();

	private XorFloat speedFloat = new XorFloat();

	private XorFloat walkFloat = new XorFloat();

	private XorInt mCurRec = new XorInt();

	public int mRec;

	private XorFloat mCurAntiStun = new XorFloat();

	private XorFloat mAntiStun = new XorFloat();

	private XorFloat mCurAntiKnockDown = new XorFloat();

	public float mAntiKnockDown;

	private int mComboValue;

	private int mRefineNeckLevel;

	private int mRefineRing1Level;

	private int mRefineRing2Level;

	private int mRefineBeltLevel;

	private int mRefineLevel;

	private int[] mEquipEnhanceList = new int[6];

	private XorInt mPkMode = new XorInt();

	private GameDefine.CAMP_TYPE mCamp;

	private int mDanceState;

	private string mDanceId;

	public string Name
	{
		get
		{
			return mName;
		}
		set
		{
			mName = value;
		}
	}

	public string GuildName
	{
		get
		{
			return mGuildName;
		}
		set
		{
			mGuildName = value;
		}
	}

	public long GuildId
	{
		get
		{
			return mGuildId;
		}
		set
		{
			mGuildId = value;
		}
	}

	public long HP
	{
		get
		{
			return mHP;
		}
		set
		{
			mHP = value;
			if (mHP >= MaxHP)
			{
				mHP = MaxHP;
			}
			if (mHP <= 0)
			{
				mHP = 0L;
			}
		}
	}

	public int Level
	{
		get
		{
			return mLevel;
		}
		set
		{
			mLevel = value;
		}
	}

	public long MaxHP
	{
		get
		{
			return mMaxHP;
		}
		set
		{
			mMaxHP = value;
		}
	}

	public int ATK
	{
		get
		{
			return mATK;
		}
		set
		{
			mATK = value;
		}
	}

	public float CurATK
	{
		get
		{
			return mCurATK;
		}
		set
		{
			mCurATK.value = value;
		}
	}

	public int HIT
	{
		get
		{
			return mHIT;
		}
		set
		{
			mHIT = value;
		}
	}

	public float CurHIT
	{
		get
		{
			return mCurHIT;
		}
		set
		{
			mCurHIT.value = value;
		}
	}

	public int CRI
	{
		get
		{
			return mCRI;
		}
		set
		{
			mCRI = value;
		}
	}

	public float CurCRI
	{
		get
		{
			return mCurCRI;
		}
		set
		{
			mCurCRI.value = value;
		}
	}

	public float RES
	{
		get
		{
			return mRES;
		}
		set
		{
			mRES = value;
		}
	}

	public float CurRES
	{
		get
		{
			return mCurRES;
		}
		set
		{
			mCurRES.value = value;
		}
	}

	public int DEF
	{
		get
		{
			return mDEF;
		}
		set
		{
			mDEF = value;
		}
	}

	public float CurDEF
	{
		get
		{
			return mCurDEF;
		}
		set
		{
			mCurDEF.value = value;
		}
	}

	public int DGE
	{
		get
		{
			return mDGE;
		}
		set
		{
			mDGE = value;
		}
	}

	public float CurDGE
	{
		get
		{
			return mCurDGE;
		}
		set
		{
			mCurDGE.value = value;
		}
	}

	public float EXD
	{
		get
		{
			return mEXD;
		}
		set
		{
			mEXD = value;
		}
	}

	public float CurEXD
	{
		get
		{
			return mCurEXD;
		}
		set
		{
			mCurEXD.value = value;
		}
	}

	public float EXR
	{
		get
		{
			return mEXR;
		}
		set
		{
			mEXR = value;
		}
	}

	public float CurEXR
	{
		get
		{
			return mCurEXR;
		}
		set
		{
			mCurEXR.value = value;
		}
	}

	public float CurCRD
	{
		get
		{
			return mCurCRD;
		}
		set
		{
			mCurCRD.value = value;
		}
	}

	public float CRD
	{
		get
		{
			return mCRD;
		}
		set
		{
			mCRD = value;
		}
	}

	public float CurCRR
	{
		get
		{
			return mCurCRR;
		}
		set
		{
			mCurCRR.value = value;
		}
	}

	public float CRR
	{
		get
		{
			return mCRR;
		}
		set
		{
			mCRR = value;
		}
	}

	public long CurEXP
	{
		get
		{
			return mCurEXP;
		}
		set
		{
			mCurEXP = value;
		}
	}

	public int DEFA
	{
		get
		{
			return mDEFA;
		}
		set
		{
			mDEFA = value;
		}
	}

	public int CurDEFA
	{
		get
		{
			return mCurDEFA;
		}
		set
		{
			mCurDEFA.value = value;
		}
	}

	public int DGEA
	{
		get
		{
			return mDGEA;
		}
		set
		{
			mDGEA = value;
		}
	}

	public int CurDGEA
	{
		get
		{
			return mCurDGEA;
		}
		set
		{
			mCurDGEA.value = value;
		}
	}

	public int RESA
	{
		get
		{
			return mRESA;
		}
		set
		{
			mRESA = value;
		}
	}

	public int CurRESA
	{
		get
		{
			return mCurRESA;
		}
		set
		{
			mCurRESA.value = value;
		}
	}

	public int HITA
	{
		get
		{
			return mHITA;
		}
		set
		{
			mHITA = value;
		}
	}

	public int CurHITA
	{
		get
		{
			return mCurHITA;
		}
		set
		{
			mCurHITA.value = value;
		}
	}

	public int CRIA
	{
		get
		{
			return mCRIA;
		}
		set
		{
			mCRIA = value;
		}
	}

	public int CurCRIA
	{
		get
		{
			return mCurCRIA;
		}
		set
		{
			mCurCRIA.value = value;
		}
	}

	public int ATE
	{
		get
		{
			return mATE;
		}
		set
		{
			mATE = value;
		}
	}

	public int CurATE
	{
		get
		{
			return mCurATE;
		}
		set
		{
			mCurATE.value = value;
		}
	}

	public int SATM
	{
		get
		{
			return mSATM;
		}
		set
		{
			mSATM = value;
		}
	}

	public int CurSATM
	{
		get
		{
			return mCurSATM;
		}
		set
		{
			mCurSATM.value = value;
		}
	}

	public int SATC
	{
		get
		{
			return mSATC;
		}
		set
		{
			mSATC = value;
		}
	}

	public int CurSATC
	{
		get
		{
			return mCurSATC;
		}
		set
		{
			mCurSATC.value = value;
		}
	}

	public int SATP
	{
		get
		{
			return mSATP;
		}
		set
		{
			mSATP = value;
		}
	}

	public int CurSATP
	{
		get
		{
			return mCurSATP;
		}
		set
		{
			mCurSATP.value = value;
		}
	}

	public int CurTitleLevel
	{
		get
		{
			return mCurTitleLevel;
		}
		set
		{
			mCurTitleLevel = value;
		}
	}

	public int CurTitleExp
	{
		get
		{
			return mCurTitleExp;
		}
		set
		{
			mCurTitleExp = value;
		}
	}

	public int BLK
	{
		get
		{
			return mBLK;
		}
		set
		{
			mBLK = value;
		}
	}

	public float CurBLK
	{
		get
		{
			return mCurBLK;
		}
		set
		{
			mCurBLK = value;
		}
	}

	public float CurSpeed
	{
		get
		{
			return speedFloat1;
		}
		set
		{
			speedFloat1.value = value;
		}
	}

	public float Speed
	{
		get
		{
			return speedFloat;
		}
		set
		{
			speedFloat.value = value;
		}
	}

	public float WalkSpeed
	{
		get
		{
			return walkFloat;
		}
		set
		{
			walkFloat.value = value;
		}
	}

	public int CurRec
	{
		get
		{
			return mCurRec;
		}
		set
		{
			mCurRec.value = value;
		}
	}

	public int Rec
	{
		get
		{
			return mRec;
		}
		set
		{
			mRec = value;
		}
	}

	public float CurAntiStun
	{
		get
		{
			return mCurAntiStun;
		}
		set
		{
			mCurAntiStun.value = value;
		}
	}

	public float AntiStun
	{
		get
		{
			return mAntiStun;
		}
		set
		{
			mAntiStun.value = value;
		}
	}

	public float CurAntiKnockDown
	{
		get
		{
			return mCurAntiKnockDown;
		}
		set
		{
			mCurAntiKnockDown.value = value;
		}
	}

	public float AntiKnockDown
	{
		get
		{
			return mAntiKnockDown;
		}
		set
		{
			mAntiKnockDown = value;
		}
	}

	public int ComboValue
	{
		get
		{
			return mComboValue;
		}
		set
		{
			mComboValue = value;
		}
	}

	public int RefineNeckLevel
	{
		get
		{
			return mRefineNeckLevel;
		}
		set
		{
			mRefineNeckLevel = value;
		}
	}

	public int RefineRing1Level
	{
		get
		{
			return mRefineRing1Level;
		}
		set
		{
			mRefineRing1Level = value;
		}
	}

	public int RefineRing2Level
	{
		get
		{
			return mRefineRing2Level;
		}
		set
		{
			mRefineRing2Level = value;
		}
	}

	public int RefineBeltLevel
	{
		get
		{
			return mRefineBeltLevel;
		}
		set
		{
			mRefineBeltLevel = value;
		}
	}

	public int RefineLevel
	{
		get
		{
			return mRefineLevel;
		}
		set
		{
			mRefineLevel = value;
		}
	}

	public int[] EquipEnhanceList => mEquipEnhanceList;

	public virtual int PkMode
	{
		get
		{
			return mPkMode.value;
		}
		set
		{
			mPkMode.value = value;
		}
	}

	public virtual GameDefine.CAMP_TYPE Camp
	{
		get
		{
			return mCamp;
		}
		set
		{
			mCamp = value;
		}
	}

	public int DanceState
	{
		get
		{
			return mDanceState;
		}
		set
		{
			mDanceState = value;
		}
	}

	public string DanceId
	{
		get
		{
			return mDanceId;
		}
		set
		{
			mDanceId = value;
		}
	}

	public bool IsChampionGuild()
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsChampionGuild(mGuildId);
	}

	public static int GetDamageByCar(ObjPlayerCar attackercar, ObjCharacter attackerObj, ObjCharacter defenderObj)
	{
		if (attackercar == null || defenderObj == null || attackerObj == null)
		{
			return 0;
		}
		float num = Mathf.Abs(attackercar.CurSpeed) * 100f;
		float num2 = attackercar.CurMountData.ATKValue;
		CharacterAttributeData attributeData = attackerObj.AttributeData;
		float num3 = Mathf.Min((attributeData.CurHIT + 1f) / ((float)attributeData.CurHITA + attributeData.CurHIT + 1f), 1f);
		float num4 = Mathf.Min((attributeData.CurCRI + 1f) / (attributeData.CurCRI + (float)attributeData.CurCRIA + 1f), 0.9f) * attributeData.CurCRD;
		float num5 = attributeData.CurATK * num3 * (attributeData.CurEXD + 1f) * (1f + num4) * num2 * (1f + num / 1000f);
		return (int)num5;
	}

	public static int GetDamage(ObjCharacter attacker, ObjCharacter defender, string SkillId, EffInfoData effinfoData, out bool isHit, out bool isCri)
	{
		float num = effinfoData.Damagex;
		float num2 = effinfoData.DamageMulti_100f;
		float num3 = 1f;
		isHit = false;
		isCri = false;
		if (attacker == null || defender == null)
		{
			return 0;
		}
		if (attacker.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || attacker.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_PLAYER || attacker.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL)
		{
			CharacterSkillData characterSkillDataByID = attacker.GetCharacterSkillDataByID(SkillId);
			if (characterSkillDataByID == null)
			{
				return 0;
			}
			if (characterSkillDataByID.Level > 0)
			{
				SkillData skillDataById = DataManager.GetSkillDataById(SkillId);
				num = effinfoData.Damagex + effinfoData.DamageAdd * characterSkillDataByID.Level;
				num2 = effinfoData.DamageMulti_100f + effinfoData.DamageMultiAdd_f * (float)characterSkillDataByID.Level;
			}
			if (defender.ObjType == GameDefine.OBJ_TYPE.OBJ_OTHER_PLAYER || defender.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_PLAYER || defender.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || defender.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL)
			{
				num3 = ((attacker.AttributeData.ComboValue <= defender.AttributeData.ComboValue) ? ((float)PlayerCommonData.PvpScale) : ((float)PlayerCommonData.PvpScale + (float)PlayerCommonData.PvpScaleAdd));
			}
		}
		return GetDamage(attacker, defender, effinfoData, num * num3, num2 * num3, out isHit, out isCri);
	}

	public static int GetDamage(ObjCharacter attackerObj, ObjCharacter defenderObj, EffInfoData effinfoData, float skillDamge, float skillScale, out bool isHit, out bool isCri)
	{
		CharacterAttributeData attributeData = attackerObj.AttributeData;
		CharacterAttributeData attributeData2 = defenderObj.AttributeData;
		isHit = false;
		isCri = false;
		float num = 0f;
		float num2 = 1f;
		if (attributeData == null || attributeData2 == null)
		{
			return 0;
		}
		float skillTypeValue = GameDefine.GetSkillTypeValue(effinfoData, attackerObj.ObjType, SKILL_ADD_TYPE.SHIT);
		if (!IsDodeg(attributeData, attributeData2, attackerObj.ObjType, skillTypeValue))
		{
			return 0;
		}
		float skillTypeValue2 = GameDefine.GetSkillTypeValue(effinfoData, attackerObj.ObjType, SKILL_ADD_TYPE.SCRI);
		isCri = IsCri(attributeData, attributeData2, attackerObj.ObjType, skillTypeValue2);
		if (attackerObj.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || attackerObj.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_PLAYER || attackerObj.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL)
		{
			if (defenderObj.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC_CAR || defenderObj.ObjType == GameDefine.OBJ_TYPE.OBJ_PLAYER_CAR)
			{
				skillScale += GameDefine.GetSkillTypeValue(effinfoData, attackerObj.ObjType, SKILL_ADD_TYPE.SATC);
				skillDamge += (float)attributeData.CurSATC;
			}
			else if (defenderObj.ObjType == GameDefine.OBJ_TYPE.OBJ_NPC)
			{
				ObjNPC objNPC = defenderObj as ObjNPC;
				if (objNPC.NPCData.FunctionType == 10)
				{
					skillScale += GameDefine.GetSkillTypeValue(effinfoData, attackerObj.ObjType, SKILL_ADD_TYPE.SATC);
					skillDamge += (float)attributeData.CurSATC;
				}
				else
				{
					skillScale += GameDefine.GetSkillTypeValue(effinfoData, attackerObj.ObjType, SKILL_ADD_TYPE.SATM);
					skillDamge += (float)attributeData.CurSATM;
				}
			}
			else
			{
				skillScale += GameDefine.GetSkillTypeValue(effinfoData, attackerObj.ObjType, SKILL_ADD_TYPE.SATP);
				skillDamge += (float)attributeData.CurSATP;
			}
		}
		float num3 = attributeData.CurATK * skillScale + skillDamge;
		float num4 = Mathf.Min((attributeData2.CurDEF + 1f) / (attributeData2.CurDEF + (float)attributeData.CurDEFA), 0.5f);
		if (isCri)
		{
			num2 = Mathf.Max(1f, Mathf.Min(1f + (attributeData.CurCRD - attributeData2.CurCRR), 2f));
		}
		isHit = true;
		float num5 = (float)PlayerCommonData.GetRandom(attackerObj.ObjType) / 1000f + 0.95f;
		float skillTypeValue3 = GameDefine.GetSkillTypeValue(effinfoData, attackerObj.ObjType, SKILL_ADD_TYPE.SEXD);
		num = num2 * num3 * num5 * (1f - num4) * (1f + (attributeData.CurEXD - attributeData2.CurEXR + skillTypeValue3));
		return Mathf.CeilToInt(num);
	}

	public static int BuffUseState(ObjCharacter attacker, CharacterAttributeData defender, string SkillId, EffInfoData effinfoData, float value, BUFF_TYPE type)
	{
		if (type == BUFF_TYPE.CHANGE_ATTR || type == BUFF_TYPE.INVINCIBLE)
		{
			return GameDefine.BUF_USE_SUCCESS;
		}
		float buffProbFloat = effinfoData.BuffProbFloat;
		if (attacker.ObjType == GameDefine.OBJ_TYPE.OBJ_MAIN_PLAYER || attacker.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_PLAYER || attacker.ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_RAGDOLL)
		{
			CharacterSkillData characterSkillDataByID = attacker.GetCharacterSkillDataByID(SkillId);
			if (characterSkillDataByID == null)
			{
				return GameDefine.BUF_USE_FAIL;
			}
		}
		float num = 0f;
		switch (type)
		{
		case BUFF_TYPE.KNOCK_DOWN:
			num = defender.CurAntiKnockDown;
			break;
		case BUFF_TYPE.STUN:
			num = defender.CurAntiStun;
			break;
		}
		if (buffProbFloat - num >= value / 100f)
		{
			return GameDefine.BUF_USE_SUCCESS;
		}
		return GameDefine.BUF_USE_FAIL;
	}

	public static bool IsDodeg(CharacterAttributeData attacker, CharacterAttributeData defender, GameDefine.OBJ_TYPE type, float add = 0f)
	{
		if (attacker == null || defender == null)
		{
			return false;
		}
		float num = Mathf.Min((attacker.CurHIT + 1f) / ((float)attacker.CurHITA + attacker.CurHIT + 1f), 1f);
		float num2 = Mathf.Min((defender.CurDGE + 1f) / (defender.CurDGE + (float)defender.CurDGEA + 1f), 0.5f);
		float num3 = 1f + num - num2 + add;
		float num4 = (float)PlayerCommonData.GetRandom(type) / 100f;
		if (num3 >= num4)
		{
			return true;
		}
		return false;
	}

	public static bool IsCri(CharacterAttributeData attacker, CharacterAttributeData defender, GameDefine.OBJ_TYPE type, float add = 0f)
	{
		if (attacker == null || defender == null)
		{
			return false;
		}
		float num = Mathf.Min((attacker.CurCRI + 1f) / (attacker.CurCRI + (float)attacker.CurCRIA + 1f), 0.9f);
		float num2 = Mathf.Min((defender.CurRES + 1f) / (defender.CurRES + (float)defender.CurRESA + 1f), 0.8f);
		float num3 = num - num2 + add;
		float num4 = (float)PlayerCommonData.GetRandom(type) / 100f;
		if (num3 >= num4)
		{
			return true;
		}
		return false;
	}

	public int GetTargetRefinePartLevel(REFINE_PART targetPart)
	{
		return targetPart switch
		{
			REFINE_PART.NECK => mRefineNeckLevel, 
			REFINE_PART.RING1 => mRefineRing1Level, 
			REFINE_PART.RING2 => mRefineRing2Level, 
			REFINE_PART.BELT => mRefineBeltLevel, 
			_ => -1, 
		};
	}

	public void SetTargetRefinePartLevel(REFINE_PART targetPart, int level)
	{
		switch (targetPart)
		{
		case REFINE_PART.NECK:
			mRefineNeckLevel = level;
			break;
		case REFINE_PART.RING1:
			mRefineRing1Level = level;
			break;
		case REFINE_PART.RING2:
			mRefineRing2Level = level;
			break;
		case REFINE_PART.BELT:
			mRefineBeltLevel = level;
			break;
		}
	}

	public int GetEquipEnhanceLevel(EQUIP_BACKPACK_TYPE targetType)
	{
		return GetEquipEnhanceLevel((int)targetType);
	}

	public int GetEquipEnhanceLevel(int targetType)
	{
		return EquipEnhanceList[targetType];
	}

	public void SetEquipEnhanceLevel(EQUIP_BACKPACK_TYPE targetType, int level)
	{
		SetEquipEnhanceLevel((int)targetType, level);
	}

	public void SetEquipEnhanceLevel(int targetType, int level)
	{
		if (EquipEnhanceList[targetType] < level)
		{
			EquipEnhanceList[targetType] = level;
		}
	}

	public void InitData(attribute Attribute, attribute AttributeAll)
	{
		CurATK = (int)AttributeAll.atk;
		CurDEF = (int)AttributeAll.def;
		CurHIT = (int)AttributeAll.hit;
		CurDGE = (int)AttributeAll.eva;
		CurCRI = (int)AttributeAll.cri;
		CurEXD = (float)AttributeAll.exd / 10000f;
		CurEXR = (float)AttributeAll.exr / 10000f;
		CurRES = (int)AttributeAll.res;
		CurCRD = (float)AttributeAll.crd / 10000f;
		CurCRR = (float)AttributeAll.crr / 10000f;
		CurDEFA = (int)AttributeAll.defa;
		CurDGEA = (int)AttributeAll.dgea;
		CurRESA = (int)AttributeAll.resa;
		CurHITA = (int)AttributeAll.hita;
		CurCRIA = (int)AttributeAll.cria;
		CurATE = (int)AttributeAll.ate;
		CurSATM = (int)AttributeAll.satm;
		CurSATC = (int)AttributeAll.satc;
		CurSATP = (int)AttributeAll.satp;
		CurAntiStun = (float)AttributeAll.anti_stun / 10000f;
		CurAntiKnockDown = (float)AttributeAll.anti_knock_down / 10000f;
		MaxHP = Attribute.max_hp;
		ATK = (int)Attribute.atk;
		DEF = (int)Attribute.def;
		HIT = (int)Attribute.hit;
		DGE = (int)Attribute.eva;
		CRI = (int)Attribute.cri;
		EXD = (float)(int)Attribute.exd / 10000f;
		EXR = (float)(int)Attribute.exr / 10000f;
		RES = (int)Attribute.res;
		CRD = (float)Attribute.crd / 10000f;
		CRR = (float)Attribute.crr / 10000f;
		DEFA = (int)Attribute.defa;
		DGEA = (int)Attribute.dgea;
		RESA = (int)Attribute.resa;
		HITA = (int)Attribute.hita;
		CRIA = (int)Attribute.cria;
		ATE = (int)Attribute.ate;
		SATM = (int)Attribute.satm;
		SATC = (int)Attribute.satc;
		SATP = (int)Attribute.satp;
		AntiStun = (float)Attribute.anti_stun / 10000f;
		AntiKnockDown = (float)Attribute.anti_knock_down / 10000f;
	}

	public void InitData(character_aoi_attribute character, bool isMainPlayer)
	{
		if (character.HasAttribute)
		{
			ATK = (int)character.attribute.atk;
			DEF = (int)character.attribute.def;
			HIT = (int)character.attribute.hit;
			DGE = (int)character.attribute.eva;
			CRI = (int)character.attribute.cri;
			EXD = (float)character.attribute.exd / 10000f;
			EXR = (float)character.attribute.exr / 10000f;
			CRD = (float)character.attribute.crd / 10000f;
			CRR = (float)character.attribute.crr / 10000f;
			DEFA = (int)character.attribute.defa;
			ATE = (int)character.attribute.ate;
			SATM = (int)character.attribute.satm;
			SATC = (int)character.attribute.satc;
			SATP = (int)character.attribute.satp;
			DGEA = (int)character.attribute.dgea;
			RESA = (int)character.attribute.resa;
			HITA = (int)character.attribute.hita;
			CRIA = (int)character.attribute.cria;
			RES = (int)character.attribute.res;
			Speed = (float)character.attribute.mov / 100f;
			Rec = (int)character.attribute.rec;
			AntiStun = (float)character.attribute.anti_stun / 10000f;
			AntiKnockDown = (float)character.attribute.anti_knock_down / 10000f;
		}
		if (character.HasAttribute_other)
		{
			CurEXP = character.attribute_other.exp;
			int level = Level;
			Level = (int)character.attribute_other.level;
			if (Level > level && isMainPlayer)
			{
				if (UIUpdateEvent.LevelUpEvent != null)
				{
					UIUpdateEvent.LevelUpEvent();
				}
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.UpdateEnhanceTips();
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.welfareData.UpdateTips();
				if (SingletonUnity<FunctionBtnRootLogic>.Exists)
				{
					SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateSkillTips();
				}
				if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager != null)
				{
					SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.RefershMapActivity();
				}
			}
			CurTitleLevel = (int)character.attribute_other.title_level;
			CurTitleExp = (int)character.attribute_other.title_exp;
			ComboValue = (int)character.attribute_other.combValue;
			RefineNeckLevel = (int)character.attribute_other.refineNeckLevel;
			RefineRing1Level = (int)character.attribute_other.refineRing1Level;
			RefineRing2Level = (int)character.attribute_other.refineRing2Level;
			RefineBeltLevel = (int)character.attribute_other.refineBeltLevel;
			RefineLevel = (int)character.attribute_other.refineLevel;
			GameDefine.CAMP_TYPE cAMP_TYPE = (GameDefine.CAMP_TYPE)character.attribute_other.camp;
			if (cAMP_TYPE != Camp)
			{
				ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(character.id);
				if (objCharacter != null)
				{
					Singleton<ObjManager>.Instance.ChangeCamp(objCharacter, Camp, cAMP_TYPE);
				}
				Camp = cAMP_TYPE;
			}
			PkMode = (int)character.attribute_other.pkMode;
			DanceState = (int)character.attribute_other.dance_state;
			DanceId = character.attribute_other.dance_id;
			if (character.attribute_other.HasGuildId)
			{
				GuildName = character.attribute_other.guildName;
				GuildId = character.attribute_other.guildId;
			}
			else
			{
				GuildName = string.Empty;
				GuildId = -1L;
			}
		}
		if (character.HasAttribute_all)
		{
			CurATK = (int)character.attribute_all.atk;
			CurDEF = (int)character.attribute_all.def;
			CurHIT = character.attribute_all.hit;
			CurDGE = character.attribute_all.eva;
			CurCRI = character.attribute_all.cri;
			CurEXD = (float)character.attribute_all.exd / 10000f;
			CurEXR = (float)character.attribute_all.exr / 10000f;
			CurCRD = (float)character.attribute_all.crd / 10000f;
			CurCRR = (float)character.attribute_all.crr / 10000f;
			CurDEFA = (int)character.attribute_all.defa;
			CurDGEA = (int)character.attribute_all.dgea;
			CurRESA = (int)character.attribute_all.resa;
			CurHITA = (int)character.attribute_all.hita;
			CurCRIA = (int)character.attribute_all.cria;
			CurATE = (int)character.attribute_all.ate;
			CurSATM = (int)character.attribute_all.satm;
			CurSATC = (int)character.attribute_all.satc;
			CurSATP = (int)character.attribute_all.satp;
			CurRES = (int)character.attribute_all.res;
			CurSpeed = (float)character.attribute_all.mov / 100f;
			CurRec = (int)character.attribute_all.rec;
			CurAntiStun = (float)character.attribute_all.anti_stun / 10000f;
			CurAntiKnockDown = (float)character.attribute_all.anti_knock_down / 10000f;
		}
	}

	public void ReName(string newname)
	{
		Name = newname;
	}
}
