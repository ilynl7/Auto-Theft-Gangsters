using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class RankPVPLocalSceneManager : SceneManager
{
	private GameObject men1Obj;

	private GameObject men2Obj;

	private PlayerData mPlayerData;

	private bool tempPlayerAutoCombatFlag;

	public override void Init(string id)
	{
		base.Init(id);
		SingletonUnity<MyEvent>.Instance.Register("OnZombiePlayerDie", this, "OnZombiePlayerDie");
		LockControl();
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.NotifyRootUI);
		mPlayerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		tempPlayerAutoCombatFlag = mPlayerData.IsOpenAutoCombat;
		mPlayerData.AutoComabat = false;
		mPlayerData.IsOpenAutoCombat = false;
	}

	public void LockControl()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.JueseJiNengQuUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.YiDongKongZhiUI);
	}

	private void OpenControl()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.JueseJiNengQuUI);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.YiDongKongZhiUI);
	}

	private void PlayerMoveToCenter()
	{
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer != null)
		{
			mainPlayer.MoveTo(Vector3.zero);
		}
	}

	public override void StartGame()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.PVPBeforeStartRoot);
		SingletonUnity<UIManager>.Instance.ShowDefaultUI();
		mPlayerData.IsOpenAutoCombat = tempPlayerAutoCombatFlag;
		mPlayerData.AutoComabat = tempPlayerAutoCombatFlag;
		if (SingletonUnity<CopyFunctionRootLogic>.Exists)
		{
			SingletonUnity<CopyFunctionRootLogic>.Instance.Reset(base.CurrentMapInofData.MapType, base.CurrentMapInofData.Name);
		}
		PlayerMoveToCenter();
		OpenControl();
		EnableZombiePlayer();
	}

	public override void SuccessMission()
	{
		rank_pvp_other_player_die.request rpcReq = new rank_pvp_other_player_die.request();
		NetLogic.GetInstance().Send<Protocol.rank_pvp_other_player_die>(rpcReq);
	}

	~RankPVPLocalSceneManager()
	{
		SingletonUnity<MyEvent>.Instance.DeRegister("OnZombiePlayerDie", this, "OnZombiePlayerDie");
	}

	private void EnableZombiePlayer()
	{
		List<ObjCharacter> list = Singleton<ObjManager>.Instance.CampList[2];
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].ObjType == GameDefine.OBJ_TYPE.OBJ_ZOMBIE_PLAYER)
			{
				ObjZombiePlayer objZombiePlayer = list[i] as ObjZombiePlayer;
				if (objZombiePlayer != null)
				{
					objZombiePlayer.ActiveAutoFight();
				}
			}
		}
	}

	public void OnZombiePlayerDie(object obj)
	{
		SingletonUnity<MyEvent>.Instance.DeRegister("OnZombiePlayerDie", this, "OnZombiePlayerDie");
		SuccessMission();
	}
}
