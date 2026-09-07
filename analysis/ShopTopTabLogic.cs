using UnityEngine;

public class ShopTopTabLogic : MonoBehaviour
{
	public UILabel NameLabel;

	public UISprite BGSp;

	private DelegateDefine.OneIntParamDelegate ClickTab;

	private int curTab;

	public void Reset(int tabnum, DelegateDefine.OneIntParamDelegate clickfun = null)
	{
		curTab = tabnum;
		NameLabel.text = StrDictionary.GetDictionaryString(GameDefine.SHOP_TAB_NAME[curTab]);
		ClickTab = clickfun;
	}

	public void RefershSelect(int selectid)
	{
		if (selectid == curTab)
		{
			BGSp.spriteName = "CZ_huaDongBG_1";
		}
		else
		{
			BGSp.spriteName = "CZ_huaDongBG";
		}
	}

	public void OnClickTabBtn()
	{
		if (ClickTab != null)
		{
			ClickTab(curTab);
		}
	}
}
