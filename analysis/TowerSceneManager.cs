using UnityEngine;

public class TowerSceneManager : SceneManager
{
	private GameObject men1Obj;

	private GameObject men2Obj;

	public int CurrentFloor;

	public override void Init(string id)
	{
		base.Init(id);
		SingletonUnity<MyEvent>.Instance.Register("OnMainPlayerCreate", this, "OnPlayerCreate");
	}

	public void OnPlayerCreate()
	{
		SingletonUnity<MyEvent>.Instance.DeRegister("OnMainPlayerCreate", this, "OnPlayerCreate");
		StartGame();
	}

	private void InitDoor()
	{
		men1Obj = GameObject.Find("FB_PKTai/men_1");
		men2Obj = GameObject.Find("FB_PKTai/men_2");
		if (men1Obj == null || men1Obj == null)
		{
			Debug.LogWarning("open door object miss!!!!");
		}
	}

	private void PlayDoorAnimation()
	{
		if (men1Obj != null)
		{
			men1Obj.animation.Play();
		}
		if (men2Obj != null)
		{
			men2Obj.animation.Play();
		}
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
		PlayerMoveToCenter();
		CurrentFloor = base.CurrentMapInofData.Param5;
		ShowFloor();
	}

	public void ShowFloor()
	{
		TowerCurrentFloorInfoLogic.ShowFloor(CurrentFloor);
	}

	public override void SuccessMission()
	{
		Debug.Log("SuccessMission");
	}

	public override void OnNPCDie(object objNpc)
	{
	}

	public void Retry()
	{
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer == null || mainPlayer.IsDie)
		{
			WaitResponseUIRootLogic.OpenWaitBox(204, 0f, 0f);
			NetLogic.GetInstance().Send<Protocol.stop_leave_copy>();
			NetLogic.GetInstance().Send<Protocol.continue_tower_copy>();
			Singleton<ObjManager>.Instance.RecycleAllNPC();
			return;
		}
		mainPlayer.StopAutoAndSkill();
		NetLogic.GetInstance().Send<Protocol.stop_leave_copy>();
		SingletonUnity<UIManager>.Instance.HideBaseUI();
		mainPlayer.MoveTo(new Vector3(-15f, 0f, 0f), 1f, delegate
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BlackScreenRoot, delegate
			{
				SingletonUnity<BlackScreenLogic>.Instance.CloseScreen();
			});
			Singleton<ObjManager>.Instance.RecycleAllNPC();
			vp_Timer.In(0.5f, delegate
			{
				mainPlayer.Position = new Vector3(15f, 0f, 0f);
				mainPlayer.MoveTo(Vector3.zero);
				SingletonUnity<BlackScreenLogic>.Instance.OpenScreen();
			});
			NetLogic.GetInstance().Send<Protocol.continue_tower_copy>();
			mainPlayer.IsTalking = false;
		});
		ShowFloor();
	}

	public void GoNextLevel()
	{
		CurrentFloor++;
		SingletonUnity<UIManager>.Instance.HideBaseUI();
		Singleton<ObjManager>.Instance.RecycleAllNPC();
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		mainPlayer.StopAutoAndSkill();
		mainPlayer.CameraController.LerpBackToPlayer(0.5f);
		NetLogic.GetInstance().Send<Protocol.stop_leave_copy>();
		mainPlayer.MoveTo(new Vector3(-15f, 0f, 0f), 1f, delegate
		{
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.BlackScreenRoot, delegate
			{
				SingletonUnity<BlackScreenLogic>.Instance.CloseScreen();
			});
			vp_Timer.In(0.5f, delegate
			{
				mainPlayer.Position = new Vector3(15f, 0f, 0f);
				mainPlayer.MoveTo(Vector3.zero);
				SingletonUnity<BlackScreenLogic>.Instance.OpenScreen();
			});
			NetLogic.GetInstance().Send<Protocol.continue_tower_copy>();
			mainPlayer.IsTalking = false;
		});
		ShowFloor();
	}
}
