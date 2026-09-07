using System;
using UnityEngine;

public class Skidmarks : SingletonUnity<Skidmarks>
{
	private class MarkSection
	{
		public Vector3 pos = Vector3.zero;

		public Vector3 normal = Vector3.zero;

		public Vector4 tangent = Vector4.zero;

		public Vector3 posl = Vector3.zero;

		public Vector3 posr = Vector3.zero;

		public float intensity;

		public int lastIndex = -1;

		public void Clear()
		{
			pos = Vector3.zero;
			normal = Vector3.zero;
			tangent = Vector3.zero;
			posl = Vector3.zero;
			posr = Vector3.zero;
			intensity = 0f;
			lastIndex = -1;
		}
	}

	public MeshFilter meshFilter;

	public int maxMarks = 128;

	[HideInInspector]
	public float markWidth = 0.275f;

	public float groundOffset = 0.02f;

	public float minDistance = 0.1f;

	private float minDistanceSq;

	private int numMarks;

	private int maxSegmentCount;

	private float markAlphaDec;

	private Mesh mesh;

	private Vector3[] vertices;

	private Vector3[] normals;

	private Vector4[] tangents;

	private Color[] colors;

	private Vector2[] uvs;

	private int[] triangles;

	private Material mat;

	private Material mat_sand;

	private MarkSection[] skidmarks;

	private bool updated;

	protected override void Awake()
	{
		base.Awake();
		maxSegmentCount = maxMarks;
		InitSkidmarks();
		minDistanceSq = minDistance * minDistance;
		markAlphaDec = 5f / (float)maxSegmentCount;
		vertices = new Vector3[maxSegmentCount * 4];
		normals = new Vector3[maxSegmentCount * 4];
		tangents = new Vector4[maxSegmentCount * 4];
		colors = new Color[maxSegmentCount * 4];
		uvs = new Vector2[maxSegmentCount * 4];
		triangles = new int[maxSegmentCount * 6];
	}

	private void InitSkidmarks()
	{
		skidmarks = new MarkSection[maxMarks];
		for (int i = 0; i < maxMarks; i++)
		{
			skidmarks[i] = new MarkSection();
		}
		mesh = meshFilter.mesh;
		if (mesh == null)
		{
			mesh = new Mesh();
			meshFilter.mesh = mesh;
		}
	}

	public void InitMark()
	{
		numMarks = 0;
		mesh.Clear();
	}

	public int AddSkidMark(Vector3 pos, Vector3 normal, float intensity, int lastIndex, float width)
	{
		if (intensity > 1f)
		{
			intensity = 1f;
		}
		if (intensity < 0f)
		{
			return -1;
		}
		MarkSection markSection = null;
		Vector3 lhs = default(Vector3);
		if (lastIndex != -1)
		{
			markSection = skidmarks[lastIndex % maxMarks];
			lhs = pos - markSection.pos;
			if (lhs.sqrMagnitude < minDistanceSq)
			{
				return lastIndex;
			}
		}
		if (skidmarks == null)
		{
			InitSkidmarks();
		}
		if (intensity > 1f)
		{
			intensity = 1f;
		}
		else if (intensity <= 0f)
		{
			return -1;
		}
		MarkSection markSection2 = skidmarks[numMarks % maxMarks];
		markSection2.pos = pos + normal * groundOffset;
		markSection2.normal = normal;
		markSection2.intensity = intensity;
		markSection2.lastIndex = lastIndex;
		if (lastIndex != -1)
		{
			Vector3 normalized = Vector3.Cross(lhs, normal).normalized;
			markSection2.posl = markSection2.pos + normalized * width * 0.5f;
			markSection2.posr = markSection2.pos - normalized * width * 0.5f;
			markSection2.tangent = new Vector4(normalized.x, normalized.y, normalized.z, 1f);
			if (markSection.lastIndex == -1)
			{
				markSection.tangent = markSection2.tangent;
				markSection.posl = markSection.pos + normalized * width * 0.5f;
				markSection.posr = markSection.pos - normalized * width * 0.5f;
			}
		}
		updated = true;
		return numMarks++;
	}

	public void SetMaterial(bool isSand)
	{
		if (isSand)
		{
			if (mat_sand == null)
			{
				mat_sand = UnityEngine.Object.Instantiate(Resources.Load("Cars/Skidmarks_sand")) as Material;
			}
			mat = base.renderer.material;
			base.renderer.material = mat_sand;
		}
		else
		{
			base.renderer.material = mat;
		}
	}

	private void LateUpdate()
	{
		if (!updated)
		{
			return;
		}
		updated = false;
		mesh.Clear();
		int i = 0;
		int num = 0;
		for (; i < maxMarks; i++)
		{
			if (num >= maxSegmentCount)
			{
				break;
			}
			if (i >= numMarks)
			{
				break;
			}
			int num2 = (numMarks - 1 - i + (maxMarks - 1)) % maxMarks;
			if (skidmarks[num2].lastIndex != -1 && skidmarks[num2].lastIndex > numMarks - maxMarks)
			{
				MarkSection markSection = skidmarks[num2];
				MarkSection markSection2 = skidmarks[markSection.lastIndex % maxMarks];
				ref Vector3 reference = ref vertices[num * 4];
				reference = markSection2.posl;
				ref Vector3 reference2 = ref vertices[num * 4 + 1];
				reference2 = markSection2.posr;
				ref Vector3 reference3 = ref vertices[num * 4 + 2];
				reference3 = markSection.posl;
				ref Vector3 reference4 = ref vertices[num * 4 + 3];
				reference4 = markSection.posr;
				ref Vector3 reference5 = ref normals[num * 4];
				reference5 = markSection2.normal;
				ref Vector3 reference6 = ref normals[num * 4 + 1];
				reference6 = markSection2.normal;
				ref Vector3 reference7 = ref normals[num * 4 + 2];
				reference7 = markSection.normal;
				ref Vector3 reference8 = ref normals[num * 4 + 3];
				reference8 = markSection.normal;
				ref Vector4 reference9 = ref tangents[num * 4];
				reference9 = markSection2.tangent;
				ref Vector4 reference10 = ref tangents[num * 4 + 1];
				reference10 = markSection2.tangent;
				ref Vector4 reference11 = ref tangents[num * 4 + 2];
				reference11 = markSection.tangent;
				ref Vector4 reference12 = ref tangents[num * 4 + 3];
				reference12 = markSection.tangent;
				markSection2.intensity -= markAlphaDec;
				if (markSection2.intensity < 0f)
				{
					markSection2.intensity = 0f;
				}
				ref Color reference13 = ref colors[num * 4];
				reference13 = new Color(0f, 0f, 0f, markSection2.intensity);
				ref Color reference14 = ref colors[num * 4 + 1];
				reference14 = new Color(0f, 0f, 0f, markSection2.intensity);
				ref Color reference15 = ref colors[num * 4 + 2];
				reference15 = new Color(0f, 0f, 0f, markSection.intensity);
				ref Color reference16 = ref colors[num * 4 + 3];
				reference16 = new Color(0f, 0f, 0f, markSection.intensity);
				ref Vector2 reference17 = ref uvs[num * 4];
				reference17 = new Vector2(0f, 0f);
				ref Vector2 reference18 = ref uvs[num * 4 + 1];
				reference18 = new Vector2(1f, 0f);
				ref Vector2 reference19 = ref uvs[num * 4 + 2];
				reference19 = new Vector2(0f, 1f);
				ref Vector2 reference20 = ref uvs[num * 4 + 3];
				reference20 = new Vector2(1f, 1f);
				triangles[num * 6] = num * 4;
				triangles[num * 6 + 2] = num * 4 + 1;
				triangles[num * 6 + 1] = num * 4 + 2;
				triangles[num * 6 + 3] = num * 4 + 2;
				triangles[num * 6 + 5] = num * 4 + 1;
				triangles[num * 6 + 4] = num * 4 + 3;
				num++;
			}
		}
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.tangents = tangents;
		mesh.triangles = triangles;
		mesh.colors = colors;
		mesh.uv = uvs;
	}

	public void Clear()
	{
		numMarks = 0;
		updated = true;
		for (int i = 0; i < skidmarks.Length; i++)
		{
			skidmarks[i].Clear();
		}
		Array.Clear(normals, 0, normals.Length);
		Array.Clear(vertices, 0, vertices.Length);
		Array.Clear(tangents, 0, tangents.Length);
		Array.Clear(triangles, 0, triangles.Length);
		Array.Clear(colors, 0, colors.Length);
		Array.Clear(uvs, 0, uvs.Length);
	}
}
