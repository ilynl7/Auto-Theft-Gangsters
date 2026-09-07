using UnityEngine;

public class MapPoint
{
	public string PointName;

	public string NpcId;

	public Vector3 PointPos;

	public MAP_POINT_TYPE PointType;

	public MapPoint(string name, Vector3 pos, MAP_POINT_TYPE type, string id)
	{
		PointName = name;
		PointPos = pos;
		PointType = type;
		NpcId = id;
	}

	public MapPoint()
	{
	}
}
