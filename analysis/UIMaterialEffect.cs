using UnityEngine;

public class UIMaterialEffect : MonoBehaviour
{
	public UITexture CurTexture;

	private Material curMaterial;

	public float EdgeFloat = 0.043f;

	public float FlowLight = 2f;

	public float FlowSpeed = 3f;

	private void OnEnable()
	{
		if (CurTexture == null)
		{
			CurTexture = base.gameObject.GetComponent<UITexture>();
		}
		if (CurTexture != null)
		{
			curMaterial = CurTexture.material;
		}
		if (curMaterial != null)
		{
			curMaterial.SetFloat("_Ratio", (float)CurTexture.width / (float)CurTexture.height);
			CurTexture.onRender = UpdateMaterial;
		}
	}

	private void UpdateMaterial(Material mat)
	{
		mat.SetFloat("_Edge", EdgeFloat);
		mat.SetFloat("_FlowLight", FlowLight);
		mat.SetFloat("_FlowSpeed", FlowSpeed);
		if (CurTexture != null)
		{
			mat.SetFloat("_Ratio", (float)CurTexture.width / (float)CurTexture.height);
		}
	}
}
