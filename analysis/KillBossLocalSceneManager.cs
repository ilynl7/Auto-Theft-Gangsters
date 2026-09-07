using System.Collections.Generic;
using UnityEngine;

public class KillBossLocalSceneManager : SceneManager
{
	public int mStateIndex;

	private int mWaveCount;

	private List<BlockController> mBlockControllerList = new List<BlockController>();

	private float time = 5f;

	public override void Init(string id)
	{
		base.Init(id);
		InitBlock();
		mWaveCount = GetMonsterGroupCount();
		mStateIndex = 0;
		ResetStage(mStateIndex);
	}

	private void InitBlock()
	{
		GameObject gameObject = GameObject.Find("zudang");
		if (gameObject != null)
		{
			Transform transform = gameObject.transform;
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				BlockController component = child.GetComponent<BlockController>();
				if (component != null)
				{
					component.Init();
					mBlockControllerList.Add(component);
				}
			}
		}
		mBlockControllerList.Sort(delegate(BlockController block1, BlockController block2)
		{
			string name = block1.gameObject.name;
			string name2 = block2.gameObject.name;
			return (name.Length != name2.Length) ? ((name.Length > name2.Length) ? 1 : (-1)) : name.CompareTo(name2);
		});
	}

	private void ResetStage(int index)
	{
		if (index < mWaveCount)
		{
			OpenBlock(index);
			int num = mStateIndex + 1;
		}
	}

	private void NpcCreate(List<MonsterData> list)
	{
		if (list != null)
		{
			NpcData npcData = null;
			for (int i = 0; i < list.Count; i++)
			{
				ObjInitNpcData objInitNpcData = new ObjInitNpcData();
				MonsterData monsterData = list[i];
				objInitNpcData.mServerID = UUID.GenUUID();
				objInitNpcData.mPos = new Vector3(monsterData.PositionX, 0f, monsterData.PositionZ);
				npcData = (objInitNpcData.npcInfoData = DataManager.GetNpcDataByID(monsterData.NpcID));
				objInitNpcData.HP = npcData.Hp;
				objInitNpcData.MaxHP = npcData.Hp;
				Singleton<ObjManager>.Instance.CreateNPC(objInitNpcData);
			}
		}
	}

	private void test()
	{
		ObjCharacter objCharacter = Singleton<ObjManager>.Instance.FindObjInScene(99L);
		Singleton<ObjManager>.Instance.MainPlayer.CameraController.BossDieCameraEffect(objCharacter);
	}

	public override void OnNPCDie(object objNpc)
	{
	}

	public void OpenBlock(int i)
	{
		if (mBlockControllerList.Count > 0 && i > 0)
		{
			Transform trs = mBlockControllerList[i - 1].transform;
			ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			vp_Timer.In(1f, delegate
			{
				mBlockControllerList[i - 1].OpenBlock(1f);
				mainPlayer.CameraController.LookAtGameObject(trs, 1f);
			});
			vp_Timer.In(3f, delegate
			{
				mainPlayer.MoveTo(trs.position, 3f);
				Singleton<ObjManager>.Instance.MainPlayer.NavMeshAgent.walkableMask += 2 << i + 1;
				Singleton<ObjManager>.Instance.MainPlayer.CameraController.BackToPlayer(1.5f);
			});
		}
	}

	public override void Update()
	{
		base.Update();
	}
}
