using UnityEngine;

public class MountCarMeshRoot : MonoBehaviour
{
	public Transform PlayerRoot;

	public Transform QLWheel;

	public Transform QRWheel;

	public Transform HLWheel;

	public Transform HRWheel;

	public Transform CarBodyRoot;

	public GameObject CarShadow;

	public float WheelRadius;

	public Vector3 ColliderCenter;

	public Vector3 ColliderSize;

	private Material mCarMaterial;

	public void ChangeColor(ColorData colordata)
	{
		if (mCarMaterial == null)
		{
			mCarMaterial = new Material(CarBodyRoot.gameObject.renderer.material);
			Shader shader = Shader.Find(mCarMaterial.shader.name);
			if (shader != null)
			{
				mCarMaterial.shader = shader;
			}
			else
			{
				Debug.Log("unable to refresh shader: in material " + mCarMaterial.name);
			}
			CarBodyRoot.gameObject.renderer.sharedMaterial = mCarMaterial;
			for (int i = 0; i < CarBodyRoot.transform.childCount; i++)
			{
				CarBodyRoot.transform.GetChild(i).gameObject.renderer.sharedMaterial = mCarMaterial;
			}
			if (QLWheel.gameObject.renderer != null)
			{
				QLWheel.gameObject.renderer.sharedMaterial = mCarMaterial;
				QRWheel.gameObject.renderer.sharedMaterial = mCarMaterial;
				HLWheel.gameObject.renderer.sharedMaterial = mCarMaterial;
				HRWheel.gameObject.renderer.sharedMaterial = mCarMaterial;
			}
		}
		if (mCarMaterial != null)
		{
			mCarMaterial.SetColor("_Color", colordata.CShaderColor);
			mCarMaterial.SetColor("_RimColor", colordata.CShaderRimColor);
			mCarMaterial.SetFloat("_ReflAmount", colordata.ShaderReflAmount);
			mCarMaterial.SetFloat("_RimPower", colordata.ShaderRimPower);
		}
		else
		{
			Debug.Log("car material is null");
		}
	}
}
