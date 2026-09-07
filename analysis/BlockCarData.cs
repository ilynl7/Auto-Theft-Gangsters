using System;
using UnityEngine;

[Serializable]
public class BlockCarData
{
	public string CarId;

	public Vector3 CarPos;

	public Vector3 CarAngle;

	public long ServerId = -1L;
}
