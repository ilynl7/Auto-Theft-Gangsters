using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CreateGuildRootLogic : SingletonUnity<CreateGuildRootLogic>
{
	public List<Transform> IconList;

	public UICenterOnChild CenterOnChild;

	private int mCurIconIndex;

	public UIInput InputGangName;

	public UIInput InputNotice;

	private int MaxNoticeCount = 150;

	public UILabel DiamondLabel;

	public UILabel CashLabel;

	public UISprite DiamondCreateBtn;

	public UISprite CashCreateBtn;

	private int MinNameCharNum = 5;

	private int MaxNameCharNum = 15;

	private void Init()
	{
		DiamondLabel.text = $"x{49}";
		CashLabel.text = $"x{499}";
	}

	private void Start()
	{
		CenterOnChild.RegisterCenterOnEvent(OnCenterOnIcon);
	}

	private void OnEnable()
	{
		CenterOnChild.CenterOn(IconList[mCurIconIndex]);
		Init();
	}

	private void OnCenterOnIcon(Transform target)
	{
		for (int i = 0; i < IconList.Count; i++)
		{
			if (IconList[i] == target)
			{
				mCurIconIndex = i;
				break;
			}
		}
	}

	private bool NoticeStrCheck(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return true;
		}
		if (str.Length > MaxNoticeCount)
		{
			return false;
		}
		return true;
	}

	public void OnClickCreateBtnRight()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.GUILD_COST)
		{
			TutorialManager.MoveNext();
			return;
		}
		string text = InputGangName.value;
		string text2 = InputNotice.value;
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Trim();
		}
		if (string.IsNullOrEmpty(text2))
		{
			text2 = InputNotice.defaultText;
		}
		if (string.IsNullOrEmpty(text))
		{
			MessageBoxLogic.OpenOKBox("#{200108}", "#{100127}");
		}
		else if (!StrIsTrue(text))
		{
			MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{200109}", MinNameCharNum, MaxNameCharNum), "#{100127}");
		}
		else if (!NoticeStrCheck(text2))
		{
			MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{200120}", MaxNoticeCount), "#{100127}");
		}
		else
		{
			Singleton<ObjManager>.Instance.MainPlayer.CreatGuild(text, text2, mCurIconIndex, GameDefine.MONEY_TYPE.GOLD);
		}
	}

	public void OnClickCreatBtnLeft()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.GUILD_COST)
		{
			TutorialManager.MoveNext();
			return;
		}
		string text = InputGangName.value;
		string text2 = InputNotice.value;
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Trim();
		}
		if (string.IsNullOrEmpty(text2))
		{
			text2 = InputNotice.defaultText;
		}
		if (string.IsNullOrEmpty(text))
		{
			MessageBoxLogic.OpenOKBox("#{200108}", "#{100127}");
		}
		else if (!StrIsTrue(text))
		{
			MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{200109}", MinNameCharNum, MaxNameCharNum), "#{100127}");
		}
		else if (!NoticeStrCheck(text2))
		{
			MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{200120}", MaxNoticeCount), "#{100127}");
		}
		else
		{
			Singleton<ObjManager>.Instance.MainPlayer.CreatGuild(text, text2, mCurIconIndex, GameDefine.MONEY_TYPE.DIAMOND);
		}
	}

	private bool StrIsTrue(string str)
	{
		string pattern = "^[a-zA-Z0-9]{1}([a-zA-Z0-9]|[ _]){4,14}$";
		if (Regex.IsMatch(str, pattern))
		{
			return true;
		}
		return false;
	}

	public void OnClickLeftBtn()
	{
		mCurIconIndex = (mCurIconIndex + IconList.Count - 1) % IconList.Count;
		CenterOnChild.CenterOn(IconList[mCurIconIndex]);
	}

	public void OnClickRightBtn()
	{
		mCurIconIndex = (mCurIconIndex + 1) % IconList.Count;
		CenterOnChild.CenterOn(IconList[mCurIconIndex]);
	}
}
