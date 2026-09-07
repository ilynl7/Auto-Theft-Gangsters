using UnityEngine;

public class ObjCarInitData
{
	public Vector3 Pos;

	public Vector3 Angle;

	public long ServerID;

	public string CharacterModelId = string.Empty;

	public bool PoliceFlag;

	public MountData CarMountData;

	public ObjCarInitData(Vector3 pos, Vector3 angle, long serverId, string characterModelId, bool policeFlag, MountData mountData = null)
	{
		Pos = pos;
		Angle = angle;
		ServerID = serverId;
		CharacterModelId = characterModelId;
		PoliceFlag = policeFlag;
		CarMountData = mountData;
	}

	public ObjCarInitData()
	{
	}
}
