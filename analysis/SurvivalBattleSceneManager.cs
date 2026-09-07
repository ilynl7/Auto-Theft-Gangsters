using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SurvivalBattleSceneManager : SceneManager
{
	protected float CheckInterTime;

	protected float curTemptime;

	protected List<MovePathPoint> mTransportPointList = new List<MovePathPoint>();

	protected List<Vector4> mRelifePosList;

	protected long mPlayerScores;

	protected SurviveBattleData surviveBattleData;

	protected bool mHasShowMessageBox;

	public override void Init(string id)
	{
		base.Init(id);
		CheckInterTime = 2f;
		curTemptime = 0f;
		List<Vector3> teleportPosList = base.CurrentMapInofData.TeleportPosList;
		mRelifePosList = base.CurrentMapInofData.RelifePosList;
		GameObject gameObject = ResourcesManager.LoadAndInstantiate("Items/MovePathPoint") as GameObject;
		MovePathPoint component = gameObject.GetComponent<MovePathPoint>();
		mTransportPointList.Add(component);
		component.transform.position = teleportPosList[0];
		component.RegisterOnArrivePathPoint(OnArriveTelePortPoint);
		for (int i = 1; i < teleportPosList.Count; i++)
		{
			GameObject gameObject2 = Object.Instantiate(gameObject) as GameObject;
			MovePathPoint component2 = gameObject2.GetComponent<MovePathPoint>();
			gameObject2.transform.position = teleportPosList[i];
			mTransportPointList.Add(component2);
			component2.RegisterOnArrivePathPoint(OnArriveTelePortPoint);
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MultiRankSmallRootUI, delegate
		{
			SingletonUnity<MultiRankSmallRootLogic>.Instance.EnableReset();
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MultiRankSmallRootUI);
		});
		surviveBattleData = DataManager.GetSurviveBattleDataById(base.CurrentMapInofData.Param1);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.SurviveBattleFloorInfoRoot, delegate
		{
			int floorid = ((!surviveBattleData.MapID.Equals(base.CurrentMapInofData.ID)) ? 1 : 0);
			SingletonUnity<SurviveBattleFloorInfoRootLogic>.Instance.Reset(floorid, surviveBattleData.SecondMinScore);
		});
		mHasShowMessageBox = false;
	}

	public void OnArriveTelePortPoint(Vector3 pos)
	{
		int num = Random.Range(0, mRelifePosList.Count);
		Vector4 vector = mRelifePosList[num];
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		mainPlayer.DisableNavMeshAgent();
		mainPlayer.Position = new Vector3(vector.x, vector.y, vector.z);
		mainPlayer.FaceToPub(MathUtil.HeadingToVector3(vector.w));
		mainPlayer.EnableNavMeshAgent();
		mainPlayer.UseInvincibleSkill();
		enter_teleport_point.request request = new enter_teleport_point.request();
		request.index = num;
		NetLogic.GetInstance().Send<Protocol.enter_teleport_point>(request);
		NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{101586}"));
	}

	public override void Update()
	{
		base.Update();
		curTemptime += Time.deltaTime;
		if (curTemptime > CheckInterTime)
		{
			curTemptime = 0f;
			NetLogic.GetInstance().Send<Protocol.request_survive_top>();
		}
	}

	public virtual void MoveToNextFloor()
	{
	}

	public virtual void UpdatePlayerScore(long playerScores)
	{
		mPlayerScores = playerScores;
	}

	public override void LeaveScene()
	{
		MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString(base.CurrentMapInofData.ExitCon, surviveBattleData.FirstMaxScore), StrDictionary.GetDictionaryString("#{100127}"), delegate
		{
			NetLogic.GetInstance().Send<Protocol.leave_copy_scene>();
		});
	}
}
