using System.Collections.Generic;

public class UIPathData
{
	public enum UIType
	{
		TYPE_BASE,
		TYPE_POP,
		TYPE_MENU_POP,
		TYPE_MENU_TOP,
		TYPE_MEMU_TOP_2,
		TYPE_MEMU_TOP_3,
		TYPE_MESSAGE,
		TYPE_ITEM
	}

	public string path;

	public string name;

	public UIType uiType;

	public bool isDestoryOnUnload;

	public bool hideBase;

	public bool backFlag;

	public bool needSelfClose;

	public bool ReShowFlag;

	public static Dictionary<string, UIPathData> UINameDic = new Dictionary<string, UIPathData>();

	public UIPathData(string path1, UIType uiType1, bool HideBase = false, bool isDestroy = true, bool addback = false, bool closestate = false, bool reshow = false)
	{
		path = path1;
		uiType = uiType1;
		int num = path1.LastIndexOf('/');
		if (num > 0)
		{
			name = path1.Substring(num + 1);
		}
		else
		{
			name = path1;
		}
		isDestoryOnUnload = isDestroy;
		UINameDic.Add(name, this);
		hideBase = HideBase;
		backFlag = addback;
		needSelfClose = closestate;
		ReShowFlag = reshow;
	}
}
