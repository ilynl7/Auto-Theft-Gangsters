using UnityEngine;

public class DominData
{
	public string ID = string.Empty;

	public string ZoneName = string.Empty;

	public string BuildingName = string.Empty;

	public string Resources1 = string.Empty;

	public int Total1;

	public int Speed1 = 1;

	public string PriceType1 = string.Empty;

	public int Price1;

	public string Resources2 = string.Empty;

	public int Total2;

	public int Speed2 = 1;

	public string PriceType2 = string.Empty;

	public int Price2;

	public int DominantMin;

	public int DominantMax;

	public int Lossspeed;

	public int matchpower;

	public int LevelMin;

	public string MapID = string.Empty;

	public int IsOpen = 1;

	public int Entrytype;

	public string AcceptMapID;

	public int PosX;

	public int PosZ;

	public float Speed1_Second => (float)Speed1 / 1000f;

	public float Speed2_Second => (float)Speed2 / 1000f;

	public string GetName => StrDictionary.GetDictionaryString(ZoneName);

	public Vector3 GetPos()
	{
		float x = (float)PosX / 100f;
		float z = (float)PosZ / 100f;
		return new Vector3(x, SceneManager.GetHitHeight(new Vector3(x, 0f, z)), z);
	}
}
