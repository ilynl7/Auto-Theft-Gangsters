using UnityEngine;

public class ObjInitData
{
	public Vector3 mPos;

	public Vector3 mDir;

	public long mServerID;

	public int mCharacterId = -1;

	public string mCharacterModelId = string.Empty;

	public string Name = string.Empty;

	public string GuildName = string.Empty;

	public long GuildId = -1L;

	public ObjInitData(long ServerId, Vector3 pos)
	{
		mServerID = ServerId;
		mPos = pos;
	}

	public ObjInitData()
	{
	}
}
