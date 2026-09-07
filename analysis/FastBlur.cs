using UnityEngine;

public class FastBlur : MonoBehaviour
{
	public int downsample = 2;

	public float blurSize = 3.5f;

	public int blurIterations = 1;

	public Shader blurShader;

	private Material blurMaterial;

	private float widthOffset;

	private float heightOffset;

	private void Start()
	{
		if (!SystemInfo.supportsImageEffects || GameSettingData.IsLowPhone)
		{
			base.enabled = false;
		}
		else if (!blurShader)
		{
			base.enabled = false;
		}
		else
		{
			blurMaterial = new Material(blurShader);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		float num = 1f / (1f * (float)(1 << downsample));
		source.filterMode = FilterMode.Bilinear;
		int width = source.width >> downsample;
		int height = source.height >> downsample;
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
		renderTexture.filterMode = FilterMode.Bilinear;
		Graphics.Blit(source, renderTexture);
		for (int i = 0; i < blurIterations; i++)
		{
			float num2 = (float)i * 1f;
			blurMaterial.SetFloat("_Parameter", blurSize * num + num2);
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(renderTexture, temporary, blurMaterial, 0);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
			temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(renderTexture, temporary, blurMaterial, 1);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
		}
		Graphics.Blit(renderTexture, destination);
		RenderTexture.ReleaseTemporary(renderTexture);
	}
}
