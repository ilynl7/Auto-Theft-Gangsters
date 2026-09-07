public class CharacterModelData
{
	public string ID;

	[ServerExclude("ServerNoUse")]
	public string Name;

	public string IndexName;

	public int TypeID;

	public int Radius;

	public int Height;

	[ServerExclude("ServerNoUse")]
	public string ModelFirstType = string.Empty;

	[ServerExclude("ServerNoUse")]
	public string ModelSubType = string.Empty;

	[ServerExclude("ServerNoUse")]
	public string ModelType;

	private XorFloat mdx = new XorFloat();

	public float ModelHeight => (float)Height / 100f;

	public float ModelRadius => mdx.value;

	public void Init()
	{
		mdx.value = (float)Radius / 100f;
	}
}
