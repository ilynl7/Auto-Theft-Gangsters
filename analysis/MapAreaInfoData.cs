using UnityEngine;

public class MapAreaInfoData
{
	public string ID = string.Empty;

	public int Type;

	public int Point1X;

	public int Point1Z;

	public int Point2X;

	public int Point2Z;

	public int Point3X;

	public int Point3Z;

	public int Point4X;

	public int Point4Z;

	public int Range;

	private XorInt p1x = new XorInt();

	private XorInt p1z = new XorInt();

	private XorInt p2x = new XorInt();

	private XorInt p2z = new XorInt();

	private XorInt p3x = new XorInt();

	private XorInt p3z = new XorInt();

	private XorInt p4x = new XorInt();

	private XorInt p4z = new XorInt();

	private XorInt rx = new XorInt();

	private Vector2[] mPointList = new Vector2[4];

	public Vector2[] PointList
	{
		get
		{
			ref Vector2 reference = ref mPointList[0];
			reference = new Vector2((float)p1x.value / 100f, (float)p1z.value / 100f);
			ref Vector2 reference2 = ref mPointList[1];
			reference2 = new Vector2((float)p2x.value / 100f, (float)p2z.value / 100f);
			ref Vector2 reference3 = ref mPointList[2];
			reference3 = new Vector2((float)p3x.value / 100f, (float)p3z.value / 100f);
			ref Vector2 reference4 = ref mPointList[3];
			reference4 = new Vector2((float)p4x.value / 100f, (float)p4z.value / 100f);
			return mPointList;
		}
	}

	public float CircleRange => (float)rx.value / 100f;

	public void Init()
	{
		p1x.value = Point1X;
		p1z.value = Point1Z;
		p2x.value = Point2X;
		p2z.value = Point2Z;
		p3x.value = Point3X;
		p3z.value = Point3Z;
		p4x.value = Point4X;
		p4z.value = Point4Z;
		rx.value = Range;
	}
}
