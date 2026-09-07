using System;
using System.Collections.Generic;
using UnityEngine;

public class CreateEquilateralPic : MonoBehaviour
{
	public int LineNum;

	public List<float> PosDisList;

	private MeshFilter meshFilter;

	private List<Vector3> pointList = new List<Vector3>();

	private void Awake()
	{
		DrawPic(LineNum);
	}

	public void DrawPic(int lineNum)
	{
		if (lineNum != 0)
		{
			meshFilter = GetComponent<MeshFilter>();
			float num = (float)Math.PI * 2f / (float)lineNum;
			for (int i = 0; i < PosDisList.Count; i++)
			{
				Vector3 item = new Vector3(Mathf.Cos(num * (float)i) * PosDisList[i], Mathf.Sin(num * (float)i) * PosDisList[i], 0f);
				pointList.Add(item);
			}
			Mesh mesh = new Mesh();
			Vector3[] array = new Vector3[lineNum + 1];
			ref Vector3 reference = ref array[0];
			reference = Vector3.zero;
			for (int j = 0; j < lineNum; j++)
			{
				ref Vector3 reference2 = ref array[j + 1];
				reference2 = pointList[j];
			}
			int[] array2 = new int[3 * lineNum];
			for (int k = 0; k < lineNum; k++)
			{
				array2[k * 3] = 0;
				array2[k * 3 + 1] = k + 1;
				array2[k * 3 + 2] = (k + 2) % (lineNum + 1);
				array2[k * 3 + 2] = ((array2[k * 3 + 2] == 0) ? 1 : array2[k * 3 + 2]);
			}
			Vector2[] array3 = new Vector2[array.Length];
			ref Vector2 reference3 = ref array3[0];
			reference3 = new Vector2(0f, 0f);
			for (int l = 1; l < array3.Length; l++)
			{
				ref Vector2 reference4 = ref array3[l];
				reference4 = new Vector2((l + 1) % 2, l % 2);
			}
			mesh.vertices = array;
			mesh.triangles = array2;
			mesh.uv = array3;
			meshFilter.mesh = mesh;
		}
	}

	public void UpdatePos(float[] posDisList)
	{
		float num = (float)Math.PI * 2f / (float)LineNum;
		pointList.Clear();
		for (int i = 0; i < posDisList.Length; i++)
		{
			Vector3 item = new Vector3(Mathf.Cos(num * (float)i) * posDisList[i], Mathf.Sin(num * (float)i) * posDisList[i], 0f);
			pointList.Add(item);
		}
		Vector3[] array = new Vector3[LineNum + 1];
		ref Vector3 reference = ref array[0];
		reference = Vector3.zero;
		for (int j = 0; j < LineNum; j++)
		{
			ref Vector3 reference2 = ref array[j + 1];
			reference2 = pointList[j];
		}
		meshFilter.mesh.vertices = array;
	}
}
