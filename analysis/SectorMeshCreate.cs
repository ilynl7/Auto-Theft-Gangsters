using System;
using UnityEngine;

public class SectorMeshCreate : MeshCreate
{
	private float mRadius;

	private float mAngel;

	private float angelDiff = 1f;

	private float mSegments;

	public override Mesh Create(float distance, float parm1)
	{
		mRadius = distance;
		mAngel = parm1;
		mSegments = mAngel / angelDiff;
		return CreateInternal();
	}

	private Mesh CreateInternal()
	{
		Mesh mesh = new Mesh();
		Vector3[] array = new Vector3[(int)mSegments + 3 - 1];
		ref Vector3 reference = ref array[0];
		reference = Vector3.zero;
		float num = (float)Math.PI / 180f * mAngel;
		float num2 = num / 2f;
		float num3 = num / mSegments;
		int num4 = 0;
		for (num4 = 1; num4 < array.Length; num4++)
		{
			ref Vector3 reference2 = ref array[num4];
			reference2 = new Vector3(Mathf.Sin(num2) * mRadius, 0f, Mathf.Cos(num2) * mRadius);
			num2 -= num3;
		}
		int[] array2 = new int[(int)mSegments * 3];
		int num5 = 1;
		num4 = 0;
		num5 = 1;
		while (num4 < array2.Length)
		{
			array2[num4] = 0;
			array2[num4 + 1] = num5 + 1;
			array2[num4 + 2] = num5;
			num4 += 3;
			num5++;
		}
		Vector2[] array3 = new Vector2[array.Length];
		for (num4 = 0; num4 < array3.Length; num4++)
		{
			ref Vector2 reference3 = ref array3[num4];
			reference3 = new Vector2(Mathf.Abs(array[num4].x) / mRadius, Mathf.Abs(array[num4].z) / mRadius);
		}
		mesh.vertices = array;
		mesh.triangles = array2;
		mesh.uv = array3;
		return mesh;
	}
}
