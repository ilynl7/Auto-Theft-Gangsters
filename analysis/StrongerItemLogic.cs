using System.Collections.Generic;
using UnityEngine;

public class StrongerItemLogic : MonoBehaviour
{
	public UISprite btnSp;

	public UILabel NameLabel;

	public UISprite RecommendFlag;

	private DelegateDefine.OneIntParamDelegate ClickFun;

	private int curType;

	private List<int> RecommendList = new List<int> { 1, 2, 4, 6, 7, 11, 12 };

	public void Reset(int type, DelegateDefine.OneIntParamDelegate clickback = null)
	{
		curType = type;
		ClickFun = clickback;
		NameLabel.text = StrDictionary.GetDictionaryString(GameDefine.STRONGER_TYPE_NAME[curType]);
		if (RecommendList.Contains(type))
		{
			RecommendFlag.alpha = 1f;
		}
		else
		{
			RecommendFlag.alpha = 0f;
		}
	}

	public void RefershSelect(int selectid)
	{
		if (selectid == curType)
		{
			btnSp.spriteName = "CZ_huaDongBG_1";
		}
		else
		{
			btnSp.spriteName = "CZ_huaDongBG";
		}
	}

	public void OnClickItemBtn()
	{
		if (ClickFun != null)
		{
			ClickFun(curType);
		}
	}
}
