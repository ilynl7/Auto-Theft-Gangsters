public class ConfigData
{
	public string Key;

	public int Type;

	public string ContentValue;

	public float Valuef
	{
		get
		{
			if (Type == 0)
			{
				return float.Parse(ContentValue);
			}
			return 1f;
		}
	}

	public int Valuei
	{
		get
		{
			if (Type == 1)
			{
				return int.Parse(ContentValue);
			}
			return 1;
		}
	}
}
