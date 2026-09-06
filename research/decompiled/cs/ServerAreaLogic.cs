using UnityEngine;

public class ServerAreaLogic : MonoBehaviour
{
	public int AreaID;

	public UISprite btnSp;

	public UILabel AreaNameLabel;

	private DelegateDefine.OneIntParamDelegate ClickArea;

	public void Reset(int id, DelegateDefine.OneIntParamDelegate clickfun = null)
	{
		AreaID = id;
		AreaNameLabel.text = StrDictionary.GetDictionaryString(GameDefine.SERVER_AREA_NAME[AreaID]);
		ClickArea = clickfun;
	}

	public void RefershSelect(int selectid)
	{
		if (selectid == AreaID)
		{
			btnSp.spriteName = "CZ_huaDongBG_1";
		}
		else
		{
			btnSp.spriteName = "CZ_huaDongBG";
		}
	}

	public void OnClickAreaItem()
	{
		if (ClickArea != null)
		{
			ClickArea(AreaID);
		}
	}
}
