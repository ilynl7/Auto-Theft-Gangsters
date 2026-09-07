using UnityEngine;

public class BlurShadow : MonoBehaviour
{
	public int Iterations = 2;

	public float Spread = 0.7f;

	private Material mBlurMaterial;

	public Shader blurShader;

	private static string shadowMatString = "Shader \"Hidden/ShadowMat\" {\n\tProperties {\n\t\t_ShadowLightness1 (\"_ShadowLightnesszz\", Color) = (0.5,0.5,0.5,0.1)\n\t}\n\tSubShader {\n\t\tPass {\n\t\t\tColor(0.12,0.12,0.12,0)    \n\t\t}\n\t}\n\tFallback off\n}";

	private Material m_ShadowMaterial;

	public Material blurMaterial
	{
		get
		{
			if (mBlurMaterial == null)
			{
				mBlurMaterial = new Material(blurShader);
				mBlurMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return mBlurMaterial;
		}
	}

	private Material shadowMaterial
	{
		get
		{
			if (m_ShadowMaterial == null)
			{
				m_ShadowMaterial = new Material(shadowMatString);
				m_ShadowMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
				m_ShadowMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			return m_ShadowMaterial;
		}
	}

	private void Start()
	{
		blurShader = Shader.Find("Hidden/OutterLineShapeBlur");
		base.camera.SetReplacementShader(shadowMaterial.shader, null);
	}

	private void OnDisable()
	{
		if ((bool)blurMaterial)
		{
			Object.DestroyImmediate(blurMaterial);
		}
		if ((bool)shadowMaterial)
		{
			Object.DestroyImmediate(shadowMaterial);
		}
	}

	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float num = 0.5f + (float)iteration * Spread;
		Graphics.BlitMultiTap(source, dest, blurMaterial, new Vector2(num, num), new Vector2(0f - num, num), new Vector2(num, 0f - num), new Vector2(0f - num, 0f - num));
	}

	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		float num = 1f;
		Graphics.BlitMultiTap(source, dest, blurMaterial, new Vector2(0f - num, 0f - num), new Vector2(0f - num, num), new Vector2(num, num), new Vector2(num, 0f - num));
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Iterations = Mathf.Clamp(Iterations, 0, 15);
		Spread = Mathf.Clamp(Spread, 0.5f, 6f);
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
		RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
		bool flag = true;
		Graphics.Blit(source, temporary);
		for (int i = 0; i < Iterations; i++)
		{
			if (flag)
			{
				FourTapCone(temporary, temporary2, i);
			}
			else
			{
				FourTapCone(temporary2, temporary, i);
			}
			flag = !flag;
		}
		if (flag)
		{
			Graphics.Blit(temporary, destination);
		}
		else
		{
			Graphics.Blit(temporary2, destination);
		}
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.ReleaseTemporary(temporary2);
	}
}
