using UnityEngine;

public class PopMenuItemLogic : MonoBehaviour
{
	public delegate void MenuItemOnClicked();

	public UILabel m_MenuItemLabel;

	private MenuItemOnClicked deleMenuItemOnClicked;

	private void Start()
	{
	}

	private void OnClick()
	{
		MenuItemOnClicked menuItemOnClicked = deleMenuItemOnClicked;
		if (menuItemOnClicked != null)
		{
			menuItemOnClicked();
			SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(4);
		}
	}

	public void InitMenuItem(string strLabel, MenuItemOnClicked funcItemOnClicked)
	{
		m_MenuItemLabel.text = strLabel;
		deleMenuItemOnClicked = funcItemOnClicked;
	}
}
