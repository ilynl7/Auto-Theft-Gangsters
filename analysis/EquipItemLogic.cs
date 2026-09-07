using UnityEngine;

public class EquipItemLogic : MonoBehaviour
{
	public delegate void OnClickItem(GameItem date);

	public OnClickItem OnClick;

	public GameItem mCurItem;

	public UILabel NameLab;

	public UILabel LvLab;

	public UISprite Icon;

	public UISprite QualityIcon;

	public UIToggle Tog;

	private void Awake()
	{
	}

	public void InitEuipInfo(GameItem item, bool IsSelect)
	{
		mCurItem = item;
		EquipData equipDataById = DataManager.GetEquipDataById(mCurItem.ItemId);
		ItemData itemDataByID = DataManager.GetItemDataByID(mCurItem.ItemId);
		if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			UnityVersionUtil.SetActiveRecursive(QualityIcon.gameObject, state: true);
			QualityIcon.spriteName = mCurItem.GetItemQuality().ToString();
		}
		else if (itemDataByID.Quality != -1)
		{
			UnityVersionUtil.SetActiveRecursive(QualityIcon.gameObject, state: true);
			QualityIcon.spriteName = ((EQUIP_QUALITY)itemDataByID.Quality).ToString();
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(QualityIcon.gameObject, state: false);
		}
		NameLab.text = itemDataByID.MName;
		NameLab.color = GameDefine.GetColorByQuality(item.GetItemQuality());
		Icon.spriteName = itemDataByID.BackPackIcon;
		LvLab.text = $"LV.{mCurItem.ItemLevel}";
		Tog.value = IsSelect;
	}

	public void UpdateInfo(GameItem item)
	{
		mCurItem = item;
		EquipData equipDataById = DataManager.GetEquipDataById(mCurItem.ItemId);
		ItemData itemDataByID = DataManager.GetItemDataByID(mCurItem.ItemId);
		UnityVersionUtil.SetActiveRecursive(QualityIcon.gameObject, state: true);
		if (itemDataByID.Type == GameDefine.ITEM_TYPE.EQUIP)
		{
			QualityIcon.spriteName = mCurItem.GetItemQuality().ToString();
		}
		else
		{
			QualityIcon.spriteName = itemDataByID.QualityType.ToString();
		}
		NameLab.text = itemDataByID.MName;
		NameLab.color = GameDefine.GetColorByQuality(item.GetItemQuality());
		Icon.spriteName = itemDataByID.BackPackIcon;
		LvLab.text = $"LV.{mCurItem.ItemLevel}";
		Tog.value = true;
	}

	public void ResetInfo()
	{
		mCurItem = null;
		UnityVersionUtil.SetActiveRecursive(QualityIcon.gameObject, state: false);
		NameLab.text = string.Empty;
		Icon.spriteName = string.Empty;
		LvLab.text = string.Empty;
		Tog.value = false;
		Tog.enabled = false;
	}

	public void OnClickEquipItem()
	{
		if (OnClick != null && mCurItem != null)
		{
			OnClick(mCurItem);
		}
	}
}
