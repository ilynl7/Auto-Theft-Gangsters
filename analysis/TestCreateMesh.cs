using UnityEngine;

public class TestCreateMesh : MonoBehaviour
{
	private void Start()
	{
		MeshCreate meshCreate = new SectorMeshCreate();
		Mesh mesh = meshCreate.Create(2f, 360f);
		GetComponent<MeshFilter>().mesh = mesh;
	}

	private void Update()
	{
	}
}
