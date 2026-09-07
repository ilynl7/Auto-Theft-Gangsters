using UnityEngine;

public class MaterialRenderQuene : MonoBehaviour
{
	public bool IsShare = true;

	public UIWidget CurWidget;

	private Material mMaterial;

	public int DefaultQuenue = 3200;

	private bool flag;

	private void Start()
	{
		if (IsShare)
		{
			mMaterial = base.renderer.sharedMaterial;
		}
		else
		{
			mMaterial = base.renderer.material;
		}
		if (mMaterial != null)
		{
			if (CurWidget != null)
			{
				UIPanel panel = CurWidget.panel;
				if (panel != null)
				{
					mMaterial.renderQueue = panel.startingRenderQueue + 1;
					flag = true;
				}
			}
		}
		else
		{
			mMaterial.renderQueue = DefaultQuenue;
			flag = true;
		}
	}

	private void Update()
	{
		if (flag)
		{
			base.enabled = false;
		}
		else if (mMaterial != null)
		{
			if (CurWidget != null)
			{
				UIPanel panel = CurWidget.panel;
				if (panel != null)
				{
					mMaterial.renderQueue = panel.startingRenderQueue + 1;
					flag = true;
				}
			}
		}
		else
		{
			mMaterial.renderQueue = DefaultQuenue;
			flag = true;
		}
	}
}
