using UnityEngine;

public class ColorData
{
	public string ID;

	public string Name;

	public int PriceType;

	public int PriceCost;

	[ServerExclude("ServerNoUse")]
	public string ShaderColor;

	[ServerExclude("ServerNoUse")]
	public string ShaderRimColor;

	[ServerExclude("ServerNoUse")]
	public float ShaderReflAmount;

	[ServerExclude("ServerNoUse")]
	public float ShaderRimPower;

	[ServerExclude("ServerNoUse")]
	public string UIColor;

	public Color CShaderColor => NGUIText.ParseColor(ShaderColor, 0);

	public Color CShaderRimColor => NGUIText.ParseColor(ShaderRimColor, 0);

	public Color CUIColor => NGUIText.ParseColor(UIColor, 0);
}
