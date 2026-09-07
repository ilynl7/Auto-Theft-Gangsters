using UnityEngine;

public class TeamTargetTabSubLine : MonoBehaviour
{
	public UILabel TitleLabel;

	private string mKey;

	private DelegateDefine.StringGameObjectDelegate onClickTab;

	public string Key => mKey;

	public void Reset(string title, string key, DelegateDefine.StringGameObjectDelegate clickFunc)
	{
		TitleLabel.text = title;
		mKey = key;
		onClickTab = clickFunc;
	}

	public void OnClickTab()
	{
		if (onClickTab != null)
		{
			onClickTab(mKey, base.gameObject);
		}
	}
}
