using System.Collections.Generic;
using UnityEngine;

public class RandomWalkNpcController : MonoBehaviour
{
	public List<string> NpcIdList;

	private List<Transform> PointList = new List<Transform>();

	public float RecycleDistance;

	public float CheckTimeInterval = 3f;

	public int MaxWalkNpcNum;

	private List<ObjNPC> mCurNpcList = new List<ObjNPC>();

	private ObjMainPlayer mMainPlayer;

	private float sqrRecycleDistance;

	private ObjManager objManager;

	private float lastCheckTime;

	private void Awake()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			PointList.Add(base.transform.GetChild(i));
		}
		sqrRecycleDistance = RecycleDistance * RecycleDistance;
	}

	private void Update()
	{
		if (Time.time - lastCheckTime > CheckTimeInterval)
		{
			lastCheckTime = Time.time;
			CheckWalkNpc();
		}
	}

	private void CheckWalkNpc()
	{
		if (mMainPlayer == null)
		{
			if (objManager == null)
			{
				objManager = Singleton<ObjManager>.Instance;
			}
			mMainPlayer = objManager.MainPlayer;
			if (mMainPlayer == null)
			{
				return;
			}
		}
		for (int num = mCurNpcList.Count - 1; num >= 0; num--)
		{
			if (mCurNpcList[num] == null)
			{
				mCurNpcList.RemoveAt(num);
			}
			else if (Vector3.SqrMagnitude(mCurNpcList[num].Position - mMainPlayer.Position) > sqrRecycleDistance)
			{
				ObjNPC objNPC = mCurNpcList[num];
				mCurNpcList.Remove(objNPC);
				objManager.RecycleNpc(objNPC);
			}
		}
		if (mCurNpcList.Count >= MaxWalkNpcNum)
		{
			return;
		}
		List<Transform> list = new List<Transform>();
		list.Clear();
		for (int i = 0; i < PointList.Count; i++)
		{
			if (Vector3.SqrMagnitude(PointList[i].position - mMainPlayer.Position) < sqrRecycleDistance)
			{
				list.Add(PointList[i]);
			}
		}
		if (list.Count != 0)
		{
			ObjInitNpcData objInitNpcData = new ObjInitNpcData();
			objInitNpcData.mServerID = UUID.GenUUID();
			objInitNpcData.mPos = list[Random.Range(0, list.Count)].position;
			objInitNpcData.mDir = MathUtil.HeadingToVector3(Random.Range(0, 360));
			objInitNpcData.npcInfoData = DataManager.GetNpcDataByID(NpcIdList[Random.Range(0, NpcIdList.Count)]);
			objManager.CreateNPC(objInitNpcData, OnCreateNpc);
		}
	}

	private void OnCreateNpc(ObjNPC objNpc)
	{
		mCurNpcList.Add(objNpc);
		objNpc.WalkMoveTo(PointList[Random.Range(0, PointList.Count)].position, 1f, OnArrivePoint);
	}

	private void OnArrivePoint(ObjCharacter objCha)
	{
		vp_Timer.In(Random.Range(0, 4), delegate
		{
			if (objCha != null && UnityVersionUtil.IsActive(objCha.gameObject) && objCha.NavMeshAgent.enabled)
			{
				objCha.WalkMoveTo(PointList[Random.Range(0, PointList.Count)].position, 1f, OnArrivePoint);
			}
		});
	}
}
