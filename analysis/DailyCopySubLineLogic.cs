using SprotoType;
using UnityEngine;

public class DailyCopySubLineLogic : MonoBehaviour
{
	public UILabel NameLabel;

	public UISprite IconSprite;

	public UILabel TimesLabel;

	public UILabel DurationLabel;

	public UILabel LevelLabel;

	public UISprite SelectBkSprite;

	public GameObject SelectUpSprite;

	public DelegateDefine.StringGameObjectDelegate onClickItem;

	private string mKey = string.Empty;

	public bool enableLineFlag;

	public string Key => mKey;

	public void ResetItem(int index, copyscene_info info, DelegateDefine.StringGameObjectDelegate clickFunc)
	{
		enableLineFlag = true;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		mKey = info.ID;
		onClickItem = clickFunc;
		CopySceneData copySceneDataById = DataManager.GetCopySceneDataById(info.ID);
		int num = (int)info.CurNum;
		if (num < 0)
		{
			num = 0;
		}
		NGUITools.SetActive(DurationLabel.gameObject, state: false);
		if (copySceneDataById.SubType == 16)
		{
			TimesLabel.enabled = false;
		}
		else
		{
			TimesLabel.enabled = true;
			TimesLabel.text = string.Format("{0}  {1}", StrDictionary.GetDictionaryString("#{101636}"), TimeTools.GetMinuteSecondStr(num));
		}
		IconSprite.spriteName = copySceneDataById.Icon;
		if (copySceneDataById.SubType != 1 && num <= 0)
		{
			enableLineFlag = false;
		}
		NameLabel.text = StrDictionary.GetDictionaryString(copySceneDataById.Name);
		LevelLabel.text = $"Lv {copySceneDataById.MinLevel}";
		if (!CheckLevel(copySceneDataById.MinLevel))
		{
			SetLabelWarining(LevelLabel, needWarning: true);
			enableLineFlag = false;
		}
		else
		{
			SetLabelWarining(LevelLabel, needWarning: false);
		}
		RefreshSelect(string.Empty);
	}

	public void OnClikcItemBtn()
	{
		if (onClickItem != null)
		{
			onClickItem(mKey, base.gameObject);
		}
	}

	public bool CheckLevel(int minLevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(minLevel);
	}

	public bool RefreshSelect(string key)
	{
		if (enableLineFlag)
		{
			SelectBkSprite.spriteName = "CZ_wuPinYanSe_3";
		}
		else
		{
			SelectBkSprite.spriteName = "CZ_tongYongDi_zhuYao_4_1";
		}
		if (!mKey.Equals(key))
		{
			UnityVersionUtil.SetActiveRecursive(SelectUpSprite, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(SelectUpSprite, state: true);
		}
		return mKey.Equals(key);
	}

	private void SetLabelWarining(UILabel label, bool needWarning)
	{
		if (needWarning)
		{
			label.color = Color.red;
		}
		else
		{
			label.color = Color.white;
		}
	}
}
