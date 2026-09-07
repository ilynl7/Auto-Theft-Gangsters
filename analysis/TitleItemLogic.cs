using UnityEngine;

public class TitleItemLogic : MonoBehaviour
{
	public UISprite GetFlag;

	public UISprite TitleSp;

	public UILabel Titilename;

	public DelegateDefine.OneIntParamDelegate clickitemfun;

	private int curkey;

	public void reset(int key, TitleData curdata, int playerlevel, DelegateDefine.OneIntParamDelegate clickfun)
	{
		TitleSp.spriteName = curdata.Icon;
		Titilename.text = StrDictionary.GetDictionaryString(curdata.Name);
		curkey = key;
		clickitemfun = clickfun;
		updateitem(playerlevel);
	}

	public void updateitem(int level)
	{
		if (curkey <= level)
		{
			TitleSp.color = Color.white;
			GetFlag.enabled = true;
		}
		else
		{
			TitleSp.color = Color.gray;
			GetFlag.enabled = false;
		}
	}

	public void SetSelect(Transform seltra)
	{
		seltra.parent = base.transform;
		seltra.localPosition = Vector3.zero;
		seltra.localRotation = Quaternion.identity;
		seltra.localScale = Vector3.one;
	}

	public void OnClickItemBtn()
	{
		if (clickitemfun != null)
		{
			clickitemfun(curkey);
		}
	}
}
