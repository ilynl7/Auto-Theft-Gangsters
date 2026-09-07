using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ObjZombiePlayer : ObjOtherPlayer
{
	private List<SkillData> mEnableSkillIDList = new List<SkillData>();

	protected bool mIsAutoFight;

	private bool mChoosedSkillFlag;

	private int mServerNotDieNumCount;

	public bool IsAutoFight => mIsAutoFight;

	public ObjZombiePlayer()
	{
		mObjType = GameDefine.OBJ_TYPE.OBJ_ZOMBIE_PLAYER;
	}

	public override void Init()
	{
		base.Init();
		BonesTrsDict = base.gameObject.GetComponentInChildren<TransDicts>().TransDict;
	}

	private void OnDisable()
	{
		if (mNavMeshAgent != null && mNavMeshAgent.enabled)
		{
			mNavMeshAgent.Stop();
			mNavMeshAgent.ResetPath();
			mNavMeshAgent.enabled = false;
		}
	}

	public void ResetZombiePlayer(ObjInitPlayerData playerInitData)
	{
		base.Reset();
		base.Position = playerInitData.mPos;
		base.CacheTransform.forward = playerInitData.mDir;
		ServerId = playerInitData.mServerID;
		Profession = playerInitData.Profession;
		AttributeData.HP = playerInitData.HP;
		AttributeData.Name = playerInitData.Name;
		AttributeData.GuildName = playerInitData.GuildName;
		AttributeData.CurEXP = playerInitData.EXP;
		AttributeData.Level = playerInitData.Level;
		AttributeData.InitData(playerInitData.Attribute, playerInitData.AttributeAll);
		AttributeData.HP = AttributeData.MaxHP;
		AttributeData.CurSpeed = playerInitData.Speed;
		AttributeData.WalkSpeed = playerInitData.WalkSpeed;
		AttributeData.CurRec = playerInitData.Rec;
		AttributeData.Camp = playerInitData.Camp;
		AttributeData.CurTitleLevel = playerInitData.TitleLevel;
		UpdateSkillList(playerInitData.skills);
		ResetCombo();
		InitHeadInfo();
		InitNavMeshAgent();
		mServerNotDieNumCount = 0;
	}

	public new void InitHeadInfo()
	{
		ResourcesManager.LoadHeadInfoPrefab(UIInfo.PlayerHeadInfoUI, "PlayerHeadInfoRoot", LoadZombiePlayerHeadInfo);
	}

	private void LoadZombiePlayerHeadInfo(GameObject obj)
	{
		if (obj != null)
		{
			BillBoard billBoard = obj.GetComponent<BillBoard>();
			if (billBoard == null)
			{
				billBoard = obj.AddComponent<BillBoard>();
			}
			billBoard.enabled = true;
			billBoard.BindObj = base.gameObject;
			billBoard.DeltaHeight = base.CurrentCharacterModelData.ModelHeight + 0.2f;
			PlayerHeadInfoLogic playerHeadInfoLogic = (PlayerHeadInfoLogic)(mHeadInfoLogic = obj.GetComponent<PlayerHeadInfoLogic>());
			bool flag = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerCamp == AttributeData.Camp;
			playerHeadInfoLogic.Reset(isMainPlayer: false, AttributeData.CurTitleLevel, AttributeData.Name, string.Empty, AttributeData.Camp, ShowHpLine: true, AttributeData.IsChampionGuild());
		}
	}

	public override void UpdateSkillList(Dictionary<string, skill_info> skills)
	{
		CharacterSkillData.Clear();
		int count = skills.Count;
		CharacterSkillData = new List<CharacterSkillData>(count);
		foreach (KeyValuePair<string, skill_info> skill in skills)
		{
			if (!skill.Value.disable || !skill.Value.HasDisable)
			{
				CharacterSkillData.Add(new CharacterSkillData(skill.Value.skillId, (int)skill.Value.skillLevel, (int)skill.Value.indexPos, (int)skill.Value.indexPos2, skill.Value.disable));
			}
		}
		CharacterSkillData.Sort((CharacterSkillData temp1, CharacterSkillData temp2) => temp1.Index - temp2.Index);
		List<SkillData> list = new List<SkillData>();
		for (int i = 0; i < CharacterSkillData.Count; i++)
		{
			int index = CharacterSkillData[i].Index;
			if (index > 3 && index < 7)
			{
				SkillData skillDataById = DataManager.GetSkillDataById(CharacterSkillData[i].ID);
				list.Add(skillDataById);
			}
		}
		list.Sort((SkillData x, SkillData y) => y.PriorityAutoCombat - x.PriorityAutoCombat);
		for (int j = 0; j < list.Count; j++)
		{
			mEnableSkillIDList.Add(list[j]);
		}
	}

	private void Update()
	{
		UpdateComponent();
		UpdateMove();
		UpdateSkillCD();
		UpdateHoldTime();
		UpdateComboTime();
		base.SkillLogic.UpdateSkill();
		if (mIsAutoFight)
		{
			AutoFight();
		}
	}

	protected void AutoFight()
	{
		if (base.SkillLogic.IsUsingSkill)
		{
			if (!mComboNextFlag && base.SkillLogic.UsingSkillData.IsComboSkill() && !base.SkillLogic.UsingSkillData.IsComboLastSkill())
			{
				mComboNextFlag = true;
			}
			return;
		}
		mChoosedSkillFlag = true;
		if (mEnableSkillIDList.Count > 0)
		{
			UseSkill(mEnableSkillIDList[0].ID);
		}
		else
		{
			UseComboSkill();
		}
	}

	public void UseComboSkill()
	{
		if (CharacterSkillData.Count <= 0)
		{
			return;
		}
		if (!CheckHoldTime())
		{
			CheckComboDelay();
		}
		else
		{
			if (BeforeSkillCheck())
			{
				return;
			}
			ObjCharacter objCharacter = null;
			if (mSelectedTarget != null && mSelectedTarget.ServerId != ServerId)
			{
				objCharacter = mSelectedTarget;
			}
			if (objCharacter == null || objCharacter.IsDie)
			{
				objCharacter = ChooseTarget();
				SelectTarget(objCharacter);
			}
			if (objCharacter == null || objCharacter.IsDie)
			{
				return;
			}
			base.CurUseSkillId = mComboIndex;
			if (mComboIndex == CharacterSkillData[0].ID)
			{
				EnterCombat(objCharacter, OnComboSuccess);
			}
			else if (!base.SkillLogic.IsUsingSkill || base.SkillLogic.CheckSkillCanBeBreak(base.CurUseSkillId))
			{
				SkillData skillDataById = DataManager.GetSkillDataById(base.CurUseSkillId);
				if (!CheckSkillDistance(skillDataById, objCharacter))
				{
					MoveTo(objCharacter.Position, skillDataById.TraceDistanceMeter + objCharacter.ModelRadius + ModelRadius - 0.5f);
				}
				else if (base.SkillLogic.UseSkill(base.CurUseSkillId, ServerId, objCharacter.ServerId))
				{
					OnComboSuccess(base.CurUseSkillId);
				}
			}
		}
	}

	private bool CheckDistance(ObjCharacter target)
	{
		float num = VectorXZ.Distance(base.Position, target.Position);
		float num2 = num - 1f - ModelRadius - target.ModelRadius;
		if (num2 > 0f)
		{
			return false;
		}
		return true;
	}

	public override void EnterCombat(ObjCharacter target, SkillUseSuccess onSkillUseSuccess = null)
	{
		if (base.SkillLogic.IsUsingSkill && !base.SkillLogic.CheckSkillCanBeBreak(base.CurUseSkillId))
		{
			return;
		}
		SkillData skillDataById = DataManager.GetSkillDataById(base.CurUseSkillId);
		if (skillDataById == null || !(target != null))
		{
			return;
		}
		if (skillDataById.TraceDistanceMeter > 0f)
		{
			if (!CheckSkillDistance(skillDataById, target))
			{
				MoveTo(target.Position, skillDataById.TraceDistanceMeter + target.ModelRadius + ModelRadius - 0.5f);
				return;
			}
		}
		else if (!CheckDistance(target))
		{
			MoveTo(target.Position, skillDataById.TraceDistanceMeter + target.ModelRadius + ModelRadius - 0.5f);
			return;
		}
		if (CheckSkillCD(skillDataById))
		{
			if (onSkillUseSuccess != null && !mSkillUseSuccessDic.ContainsKey(base.CurUseSkillId))
			{
				mSkillUseSuccessDic.Add(base.CurUseSkillId, onSkillUseSuccess);
			}
			if (target != null)
			{
				base.SkillLogic.UseSkill(base.CurUseSkillId, ServerId, target.ServerId);
			}
			else
			{
				base.SkillLogic.UseSkill(base.CurUseSkillId, ServerId, -1L);
			}
		}
	}

	private void UseSkill(string skillId, SkillUseSuccess onSkillUseSuccess = null)
	{
		if (!BeforeSkillCheck())
		{
			ObjCharacter objCharacter = null;
			if (mSelectedTarget != null && mSelectedTarget.ServerId != ServerId)
			{
				objCharacter = mSelectedTarget;
			}
			if (objCharacter == null || objCharacter.IsDie)
			{
				objCharacter = ChooseTarget();
				SelectTarget(objCharacter);
			}
			if (!(objCharacter == null) && !objCharacter.IsDie)
			{
				base.CurUseSkillId = skillId;
				EnterCombat(objCharacter, onSkillUseSuccess);
			}
		}
	}

	private void OnComboSuccess(string skillId)
	{
		SkillData skillDataById = DataManager.GetSkillDataById(skillId);
		mComboIndex = skillDataById.NextSkill;
		mComboTimeCount = skillDataById.ComboValidTimeSecond;
		mComboTimeTotalCount = mComboTimeCount;
		mChoosedSkillFlag = false;
		OnSkillDisable(skillId);
	}

	private ObjCharacter ChooseTarget()
	{
		return Singleton<ObjManager>.Instance.MainPlayer;
	}

	private void SelectTarget(ObjCharacter target)
	{
		base.SelectedTarget = target;
	}

	public void CheckComboDelay()
	{
		if (base.SkillLogic.UsingSkillData != null && base.SkillLogic.UsingSkillData.NextSkill == mComboIndex)
		{
			mComboNextFlag = true;
		}
	}

	private void ResetCombo()
	{
		mComboTimeCount = 0f;
		if (CharacterSkillData.Count != 0)
		{
			mComboIndex = CharacterSkillData[0].ID;
		}
	}

	public override void OnSkillFinished()
	{
		base.OnSkillFinished();
		if (mComboNextFlag)
		{
			UseComboSkill();
			mComboNextFlag = false;
		}
		mChoosedSkillFlag = false;
	}

	public void TargetArriveUseSkill()
	{
		SkillData skillDataById = DataManager.GetSkillDataById(mCurUseSkillId);
		if (skillDataById != null)
		{
			if (!string.IsNullOrEmpty(skillDataById.NextSkill))
			{
				UseComboSkill();
			}
			else
			{
				UseSkill(mCurUseSkillId);
			}
		}
	}

	public void ActiveAutoFight()
	{
		mIsAutoFight = true;
	}

	public void DeactiveAutoFight()
	{
		mIsAutoFight = false;
	}

	public void UpdateComboTime()
	{
		if (mComboTimeCount > 0f)
		{
			mComboTimeCount -= Time.deltaTime;
			if (mComboTimeCount <= 0f)
			{
				ResetCombo();
			}
		}
	}

	public override void OnSkillUseSuccess(string skillId)
	{
		base.OnSkillUseSuccess(skillId);
		OnSkillDisable(base.CurUseSkillId);
	}

	public override void OnSkillEnable(string skillID)
	{
		if (DataManager.GetSkillDataById(skillID).IsComboSkill())
		{
			return;
		}
		SkillData skillDataById = DataManager.GetSkillDataById(skillID);
		if (mEnableSkillIDList.Count > 0)
		{
			for (int i = 0; i < mEnableSkillIDList.Count; i++)
			{
				if (skillDataById.PriorityAutoCombat > mEnableSkillIDList[i].PriorityAutoCombat)
				{
					mEnableSkillIDList.Insert(i, skillDataById);
					return;
				}
			}
		}
		mEnableSkillIDList.Add(skillDataById);
	}

	public override void OnSkillDisable(string skillID)
	{
		mEnableSkillIDList.Remove(DataManager.GetSkillDataById(skillID));
	}

	public override void OnDie()
	{
		base.OnDie();
		SingletonUnity<MyEvent>.Instance.Fire("OnZombiePlayerDie", this);
	}

	public override void ChangeHPVal(long newHP)
	{
		if (!base.IsDie)
		{
			AttributeData.HP = newHP;
			UpdateHeadInfo();
			if (AttributeData.HP <= 0)
			{
				OnDie();
			}
		}
		else if (newHP > 0)
		{
			Debug.Log("new hp:" + newHP);
			mServerNotDieNumCount++;
			if (mServerNotDieNumCount > GameDefine.NPC_SERVER_WAIT_RELIFE_NUM)
			{
				OnRelife(newHP, base.Position);
				mServerNotDieNumCount = 0;
			}
		}
	}

	public override void ChangeHPEffect(long newHP, GameDefine.DAMAGEBOARD_TYPE type)
	{
		if (!base.IsDie)
		{
			long num = AttributeData.HP - newHP;
			if (num > 0)
			{
				UpdateDamgeBoard(type, num);
				OnBeaton();
			}
			else
			{
				UpdateDamgeBoard(type, num);
			}
			mReceiveBiggerHpTimeCount = Time.time;
		}
	}

	public bool CheckInDialogRange()
	{
		float num = ((!Singleton<ObjManager>.Instance.MainPlayer.IsLocalDrivingCar) ? 5 : 10);
		if (VectorXZ.Distance(base.Position, Singleton<ObjManager>.Instance.MainPlayer.Position) < num)
		{
			return true;
		}
		return false;
	}
}
