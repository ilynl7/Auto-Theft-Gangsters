using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(UILabel))]
public class LabelState : MonoBehaviour
{
	public Color activeColor;

	public Color deactiveColor;

	public bool useGradientActive;

	public Color gradientActiveTopColor;

	public Color gradientActiveBottomColor;

	public bool useGradientDeactive;

	public Color gradientDeactiveTopColor;

	public Color gradientDeactiveBottomColor;

	[SerializeField]
	private bool mActive;

	private UILabel mLabel;

	public bool active
	{
		get
		{
			return mActive;
		}
		set
		{
			if (mLabel == null)
			{
				mLabel = GetComponent<UILabel>();
			}
			if (value == mActive)
			{
				return;
			}
			if (value)
			{
				mLabel.color = activeColor;
				if (useGradientActive)
				{
					mLabel.gradientTop = gradientActiveTopColor;
					mLabel.gradientBottom = gradientActiveBottomColor;
					mLabel.applyGradient = true;
				}
				else
				{
					mLabel.applyGradient = false;
				}
			}
			else
			{
				mLabel.color = deactiveColor;
				if (useGradientDeactive)
				{
					mLabel.gradientTop = gradientDeactiveTopColor;
					mLabel.gradientBottom = gradientDeactiveBottomColor;
					mLabel.applyGradient = true;
				}
				else
				{
					mLabel.applyGradient = false;
				}
			}
			mActive = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
