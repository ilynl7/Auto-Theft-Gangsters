using UnityEngine;

public class UIButtonExtent : MonoBehaviour
{
	public float scale = 1f;

	private UIWidget widget;

	private BoxCollider boxCollider;

	private bool run;

	private void Init()
	{
		if (widget == null)
		{
			widget = GetComponent<UIWidget>();
		}
		if (widget != null)
		{
			if (boxCollider == null)
			{
				boxCollider = GetComponent<BoxCollider>();
			}
			if (boxCollider != null)
			{
				boxCollider.size = new Vector3((float)widget.width * scale, (float)widget.height * scale, 0f);
			}
		}
	}

	private void OnEnable()
	{
		Init();
	}

	private void Update()
	{
		if (!run)
		{
			Init();
			base.enabled = false;
			run = true;
		}
	}
}
