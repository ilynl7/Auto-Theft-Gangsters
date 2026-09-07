using UnityEngine;

public class CullDistance : MonoBehaviour
{
	public float[] distances = new float[32];

	private void Awake()
	{
		distances[24] = 0f;
		base.camera.layerCullSpherical = true;
		base.camera.layerCullDistances = distances;
	}
}
