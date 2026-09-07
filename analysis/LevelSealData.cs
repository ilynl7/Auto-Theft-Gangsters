public class LevelSealData
{
	public string ID;

	public int inhibit;

	public int encourage;

	public int Inhibit => inhibit / 100;

	public int Encourage => encourage / 100;

	public float InhibitRatio => (float)inhibit / 10000f;

	public float EncourageRatio => (float)encourage / 10000f;
}
