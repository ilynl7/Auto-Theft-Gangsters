using System.Collections.Generic;
using UnityEngine;

public class NpcData
{
	public string ID;

	public string Name;

	[ServerExclude("ServerNoUse")]
	public int IsPlayerModel;

	public string Model;

	public int Size = 100;

	public int Lv;

	public int Group = 1;

	public string TalkGroup = string.Empty;

	public int FunctionType;

	public int Type;

	public long Hp;

	public int Atk;

	public int Def;

	public int HIT;

	public int DGE;

	public int CRI;

	public int RES;

	public int EXD;

	public int EXR;

	public int CRD;

	public int CRR;

	public int DEFA;

	public int DGEA;

	public int RESA;

	public int HITA;

	public int CRIA;

	public int AntiStun;

	public int AntiKnockDown;

	public int HpCoe;

	public int AtkCoe;

	public int DefCoe;

	public int HITCoe;

	public int DGECoe;

	public int CRICoe;

	public int RESCoe;

	public int EXDCoe;

	public int EXRCoe;

	public int CRDCoe;

	public int CRRCoe;

	public int DEFACoe;

	public int DGEACoe;

	public int RESACoe;

	public int HITACoe;

	public int CRIACoe;

	public int AntiStunCoe;

	public int AntiKnockDownCoe;

	public string SkillGroup;

	public int Buff;

	public int Patrol;

	public int Search;

	public string AI = string.Empty;

	public string DropID = string.Empty;

	public string AIID = "1001";

	public int WalkSpeed = 10;

	public int MoveSpeed = 50;

	public string TabShiftHP;

	public string TabShiftSkill;

	public string TabShiftSkillGroup;

	private int[] mShiftHP;

	private string[] mShiftSkill;

	private List<string[]> mShiftSkillGroup;

	private string[] SkillGroupID;

	public int[] ShiftHP
	{
		get
		{
			if (mShiftHP == null && !string.IsNullOrEmpty(TabShiftHP))
			{
				string[] array = TabShiftHP.Split('#');
				mShiftHP = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					if (!int.TryParse(array[i], out mShiftHP[i]))
					{
						Debug.Log("TabShiftHP Input Wrong!!!!!!!!!!");
					}
				}
			}
			return mShiftHP;
		}
	}

	public string[] ShiftSkill
	{
		get
		{
			if (mShiftSkill == null && !string.IsNullOrEmpty(TabShiftSkill))
			{
				string[] array = TabShiftSkill.Split('#');
				mShiftSkill = array;
			}
			return mShiftSkill;
		}
	}

	public string MName => StrDictionary.GetDictionaryString(Name);

	public List<string[]> ShiftSkillGroup
	{
		get
		{
			if (mShiftSkillGroup == null && !string.IsNullOrEmpty(TabShiftSkillGroup))
			{
				mShiftSkillGroup = new List<string[]>();
				string[] array = TabShiftSkillGroup.Split('#');
				for (int i = 0; i < array.Length; i++)
				{
					string[] item = array[i].Split(';');
					mShiftSkillGroup.Add(item);
				}
			}
			return mShiftSkillGroup;
		}
	}

	public int ShiftStateCount
	{
		get
		{
			if (mShiftHP != null)
			{
				return mShiftHP.Length;
			}
			return 0;
		}
	}

	public float WalkSpeedMeter => (float)WalkSpeed / 10f;

	public float MoveSpeedMeter => (float)MoveSpeed / 10f;

	public float ModelScale => (float)Size / 100f;

	public string[] SkillList
	{
		get
		{
			if (SkillGroupID == null && !string.IsNullOrEmpty(SkillGroup))
			{
				string[] skillGroupID = SkillGroup.Split(';');
				SkillGroupID = skillGroupID;
			}
			return SkillGroupID;
		}
	}

	public float PatrolRadius => (float)Patrol / 100f;

	public float SearchRadius => (float)Search / 100f;
}
