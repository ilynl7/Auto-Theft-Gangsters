using UnityEngine;

public class RectangleCreate : MeshCreate
{
	private float distancex;

	private float distancey;

	private float mAngel;

	private float angelDiff = 1f;

	private float mSegments;

	public override Mesh Create(float distance, float parm1)
	{
		distancex = distance;
		distancey = parm1;
		return CreateInternal();
	}

	private Mesh CreateInternal()
	{
		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3((0f - distancex) / 2f, 0f, 0f),
			new Vector3(distancex / 2f, 0f, 0f),
			new Vector3(distancex / 2f, 0f, distancey),
			new Vector3((0f - distancex) / 2f, 0f, distancey)
		};
		int[] triangles = new int[6] { 2, 1, 0, 2, 0, 3 };
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(1f, 1f),
			new Vector2(0f, 1f)
		};
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		return mesh;
	}
}
