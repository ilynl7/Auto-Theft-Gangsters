using SprotoType;
using UnityEngine;

public class ObjInitNpcData : ObjInitData
{
	public NpcData npcInfoData;

	public long HP;

	public long MaxHP;

	public int ATK;

	public int DEF;

	public int HIT;

	public int EVA;

	public int CRI;

	public int EXD;

	public int EXR;

	public int RES;

	public int CRD;

	public int CRR;

	public int DEFA;

	public int DGEA;

	public int RESA;

	public int HITA;

	public int CRIA;

	public int X;

	public int Z;

	public int O;

	public int Level;

	public int AntiStun;

	public int AntiKnockDown;

	public string PathID = string.Empty;

	public string PlayerName = string.Empty;

	public new long GuildId = -1L;

	public long TeamId = -1L;

	public void InitData(npc_attribute npc_attribute)
	{
		mServerID = npc_attribute.id;
		mDir = MathUtil.HeadingToVector3((float)npc_attribute.o / 100f);
		mPos = new Vector3((float)npc_attribute.x / 100f, 0f, (float)npc_attribute.z / 100f);
		mPos = new Vector3(mPos.x, SceneManager.GetHitHeight(mPos), mPos.z);
		NpcData npcDataByID = DataManager.GetNpcDataByID(npc_attribute.npcdataid);
		HP = npc_attribute.hp;
		MaxHP = npc_attribute.max_hp;
		npcInfoData = npcDataByID;
		ATK = (int)npc_attribute.atk;
		DEF = (int)npc_attribute.def;
		HIT = (int)npc_attribute.hit;
		EVA = (int)npc_attribute.eva;
		CRI = (int)npc_attribute.cri;
		EXD = (int)npc_attribute.exd;
		EXR = (int)npc_attribute.exr;
		RES = (int)npc_attribute.res;
		CRD = (int)npc_attribute.crd;
		CRR = (int)npc_attribute.crr;
		DEFA = (int)npc_attribute.defa;
		DGEA = (int)npc_attribute.dgea;
		RESA = (int)npc_attribute.resa;
		HITA = (int)npc_attribute.hita;
		CRIA = (int)npc_attribute.cria;
		X = (int)npc_attribute.x;
		Z = (int)npc_attribute.z;
		O = (int)npc_attribute.o;
		Level = (int)npc_attribute.level;
		AntiStun = (int)npc_attribute.anti_stun;
		AntiKnockDown = (int)npc_attribute.anti_knock_down;
		PlayerName = npc_attribute.player_name;
		GuildId = ((!npc_attribute.HasGuildId) ? (-1) : npc_attribute.guildId);
		TeamId = ((!npc_attribute.HasTeamid) ? (-1) : npc_attribute.teamid);
	}
}
