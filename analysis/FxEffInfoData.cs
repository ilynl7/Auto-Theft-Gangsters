using UnityEngine;

public class FxEffInfoData
{
	public string ID;

	public string EffName = string.Empty;

	public string EffFilePath = string.Empty;

	public int EffDurationTime;

	public int EffDelayTime;

	public string EffLinkNode = string.Empty;

	public float PX;

	public float PY;

	public float PZ;

	public float AX;

	public float AY;

	public float AZ;

	public int AutoMove;

	public bool AutoMoveFlag => AutoMove == 1;

	public float EffDurationTimeSeconds => (float)EffDurationTime / 1000f;

	public float fEffDelayTimeSeconds => (float)EffDelayTime / 1000f;

	public Vector3 Position => new Vector3(PX, PY, PZ);

	public Vector3 Angel => new Vector3(AX, AY, AZ);
}
