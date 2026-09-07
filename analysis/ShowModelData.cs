using UnityEngine;

public class ShowModelData
{
	public string ID;

	public string ModelName = string.Empty;

	public float posX;

	public float posY;

	public float posZ;

	public float rotX;

	public float rotY;

	public float rotZ;

	public Vector3 Position => new Vector3(posX, posY, posZ);

	public Vector3 Rotation => new Vector3(rotX, rotY, rotZ);
}
