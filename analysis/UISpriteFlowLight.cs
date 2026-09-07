using UnityEngine;

public class UISpriteFlowLight : UISprite
{
	public Material mTestMat;

	public override Material material
	{
		get
		{
			if (mTestMat != null)
			{
				return mTestMat;
			}
			return null;
		}
	}
}
