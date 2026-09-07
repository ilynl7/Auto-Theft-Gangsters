using SprotoType;
using UnityEngine;

public class ShopItemInfoNew : MonoBehaviour
{
	public ShopItemSubInfo ItemViewObj;

	public ShopItemViewInfo ModelViewObj;

	public UISprite StatusBtnSp;

	public UISprite ViewBtnSp;

	private bool IsShowAttInfo;

	private shop_item curSelectItem;

	public void Reset()
	{
		NGUITools.SetActive(ModelViewObj.gameObject, state: false);
		NGUITools.SetActive(ItemViewObj.gameObject, state: false);
		NGUITools.SetActive(StatusBtnSp.gameObject, state: false);
		NGUITools.SetActive(ViewBtnSp.gameObject, state: false);
		ModelViewObj.Reset();
		IsShowAttInfo = true;
	}

	public void RefershInfo(shop_item shopitem)
	{
		curSelectItem = shopitem;
		ItemData itemDataByID = DataManager.GetItemDataByID(curSelectItem.ItemID);
		if (itemDataByID != null)
		{
			if (itemDataByID.CanShowModel || itemDataByID.Type == GameDefine.ITEM_TYPE.EXCHANGE)
			{
				NGUITools.SetActive(StatusBtnSp.gameObject, state: true);
				NGUITools.SetActive(ViewBtnSp.gameObject, state: true);
			}
			else
			{
				NGUITools.SetActive(StatusBtnSp.gameObject, state: false);
				NGUITools.SetActive(ViewBtnSp.gameObject, state: false);
				IsShowAttInfo = true;
				NGUITools.SetActive(ModelViewObj.gameObject, state: false);
			}
		}
		if (IsShowAttInfo)
		{
			if (!UnityVersionUtil.IsActive(ItemViewObj.gameObject))
			{
				NGUITools.SetActive(ItemViewObj.gameObject, state: true);
			}
			ItemViewObj.UpdateSelectItem(curSelectItem);
		}
		else
		{
			if (!UnityVersionUtil.IsActive(ModelViewObj.gameObject))
			{
				NGUITools.SetActive(ModelViewObj.gameObject, state: true);
			}
			ModelViewObj.UpdateSelectItem(curSelectItem);
		}
		SelectTable();
	}

	public void OnClickStatusBtn()
	{
		if (!IsShowAttInfo)
		{
			IsShowAttInfo = true;
			NGUITools.SetActive(ModelViewObj.gameObject, state: false);
			NGUITools.SetActive(ItemViewObj.gameObject, state: true);
			ItemViewObj.UpdateSelectItem(curSelectItem);
			SelectTable();
		}
	}

	public void OnClickViewBtn()
	{
		if (IsShowAttInfo)
		{
			IsShowAttInfo = false;
			NGUITools.SetActive(ModelViewObj.gameObject, state: true);
			NGUITools.SetActive(ItemViewObj.gameObject, state: false);
			ModelViewObj.UpdateSelectItem(curSelectItem);
			SelectTable();
		}
	}

	private void SelectTable()
	{
		if (IsShowAttInfo)
		{
			StatusBtnSp.spriteName = GameDefine.BtnIcon[0];
			ViewBtnSp.spriteName = GameDefine.BtnIcon[1];
		}
		else
		{
			StatusBtnSp.spriteName = GameDefine.BtnIcon[1];
			ViewBtnSp.spriteName = GameDefine.BtnIcon[0];
		}
	}
}
