using UnityEngine;

public class SpriteState : MonoBehaviour
{
	public Color activeColor;

	public Color deactiveColor;

	[SerializeField]
	private bool mActive;

	private UISprite mSprite;

	public bool active
	{
		get
		{
			return mActive;
		}
		set
		{
			if (mSprite == null)
			{
				mSprite = GetComponent<UISprite>();
			}
			if (value)
			{
				mSprite.color = activeColor;
			}
			else
			{
				mSprite.color = deactiveColor;
			}
			mActive = value;
		}
	}
}
