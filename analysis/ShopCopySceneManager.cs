using System.Collections.Generic;

public class ShopCopySceneManager : SceneManager
{
	public override void Init(string id)
	{
		base.Init(id);
		CreateNpc();
	}

	public override void OnLoadingOver()
	{
		base.OnLoadingOver();
	}

	public void CreateNpc()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		List<MonsterData> monsterDataList = base.MonsterDataList;
		for (int i = 0; i < monsterDataList.Count; i++)
		{
			NpcData npcDataByID = DataManager.GetNpcDataByID(monsterDataList[i].NpcID);
			if (npcDataByID != null)
			{
				GetNpc(monsterDataList[i]);
			}
		}
	}

	public void GetNpc(MonsterData curData)
	{
		ObjInitNpcData objInitNpcData = new ObjInitNpcData();
		objInitNpcData.mServerID = UUID.GenUUID();
		objInitNpcData.mPos = curData.GetNpcPos();
		objInitNpcData.mDir = MathUtil.HeadingToVector3(curData.PositionO);
		objInitNpcData.npcInfoData = DataManager.GetNpcDataByID(curData.NpcID);
		objInitNpcData.mCharacterModelId = objInitNpcData.npcInfoData.Model;
		objInitNpcData.MaxHP = objInitNpcData.npcInfoData.Hp;
		objInitNpcData.HP = objInitNpcData.npcInfoData.Hp;
		objInitNpcData.ATK = objInitNpcData.npcInfoData.Atk;
		objInitNpcData.DEF = objInitNpcData.npcInfoData.Def;
		objInitNpcData.HIT = objInitNpcData.npcInfoData.HIT;
		objInitNpcData.EVA = objInitNpcData.npcInfoData.DGE;
		objInitNpcData.CRI = objInitNpcData.npcInfoData.CRI;
		objInitNpcData.EXD = objInitNpcData.npcInfoData.EXD;
		objInitNpcData.EXR = objInitNpcData.npcInfoData.EXR;
		objInitNpcData.RES = objInitNpcData.npcInfoData.RES;
		objInitNpcData.CRD = objInitNpcData.npcInfoData.CRD;
		objInitNpcData.CRR = objInitNpcData.npcInfoData.CRR;
		objInitNpcData.DEFA = objInitNpcData.npcInfoData.DEFA;
		objInitNpcData.DGEA = objInitNpcData.npcInfoData.DGEA;
		objInitNpcData.HITA = objInitNpcData.npcInfoData.HITA;
		objInitNpcData.RESA = objInitNpcData.npcInfoData.RESA;
		objInitNpcData.CRIA = objInitNpcData.npcInfoData.CRIA;
		objInitNpcData.Level = objInitNpcData.npcInfoData.Lv;
		objInitNpcData.AntiStun = objInitNpcData.npcInfoData.AntiStun;
		objInitNpcData.AntiKnockDown = objInitNpcData.npcInfoData.AntiKnockDown;
		objInitNpcData.PathID = curData.PathId;
		Singleton<ObjManager>.Instance.CreateNPC(objInitNpcData);
	}
}
