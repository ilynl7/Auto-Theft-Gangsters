using System.Collections.Generic;
using UnityEngine;

public class EquipCopySceneManager : SceneManager
{
	private List<BlockController> mBlockControllerList = new List<BlockController>();

	private List<int> OpenBlockList = new List<int>();

	public override void Init(string id)
	{
		base.Init(id);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExpBattleInfoRoot, delegate
		{
			SingletonUnity<ExpBattleInfoRootLogic>.Instance.ResetToTeam();
		});
		InitBlock();
	}

	private void InitBlock()
	{
		OpenBlockList.Clear();
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

	public void OpenBlock(int i)
	{
		if (mBlockControllerList.Count <= 0 || i <= 0)
		{
			return;
		}
		if (OpenBlockList.Contains(i))
		{
			return;
		}
		OpenBlockList.Add(i);
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
			for (int j = 0; j < Singleton<ObjManager>.Instance.ObjOtherPlayerVisibleList.Count; j++)
			{
				Singleton<ObjManager>.Instance.ObjOtherPlayerVisibleList[j].NavMeshAgent.walkableMask += 2 << i + 1;
			}
		});
	}

	public override void Update()
	{
		base.Update();
	}
}
