public class Hyperlink
{
	private string mHyperlinkName;

	private HyperlinkType mHyperlinkType;

	public string HyperlinkName
	{
		get
		{
			return mHyperlinkName;
		}
		set
		{
			mHyperlinkName = value;
		}
	}

	public HyperlinkType HyperlinkType => mHyperlinkType;
}
