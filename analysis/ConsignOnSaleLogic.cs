using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class ConsignOnSaleLogic : MonoBehaviour
{
	public List<ConsignItemLogic> consignItemLogicList = new List<ConsignItemLogic>();

	public UIScrollView scrollView;

	public UIGrid grid;

	private int PageCount = 10;

	private int CurPage;

	private int MaxPage;

	public UILabel PageInfoLabel;

	private List<consign_item> currentConsignItemList;

	public void Reset()
	{
		List<consign_item> list = (currentConsignItemList = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SaleNowList);
		if (list != null && list.Count > 0)
		{
			MaxPage = (list.Count + PageCount - 1) / PageCount;
			if (CurPage >= MaxPage)
			{
				CurPage = MaxPage - 1;
			}
		}
		else
		{
			CurPage = 0;
			MaxPage = 0;
		}
		PageInfoLabel.text = $"{CurPage + 1}/{MaxPage}";
		RefreshPage(list, CurPage, isRefresh: true);
	}

	public void RefreshPage(List<consign_item> list, int page, bool isRefresh)
	{
		if (list != null)
		{
			PageInfoLabel.text = $"{page + 1}/{MaxPage}";
			int num = Mathf.Min(list.Count, PageCount) - consignItemLogicList.Count;
			int count = consignItemLogicList.Count;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					GameObject gameObject = Object.Instantiate(consignItemLogicList[0].gameObject) as GameObject;
					gameObject.name = $"ItemLine_{count + i + 1}";
					ConsignItemLogic component = gameObject.GetComponent<ConsignItemLogic>();
					if (component != null)
					{
						component.transform.parent = consignItemLogicList[0].transform.parent;
						component.transform.localScale = Vector3.one;
						component.transform.localPosition = Vector3.zero;
						consignItemLogicList.Add(component);
					}
				}
			}
			for (int j = 0; j < consignItemLogicList.Count; j++)
			{
				if (j + page * PageCount < list.Count)
				{
					UnityVersionUtil.SetActiveRecursive(consignItemLogicList[j].gameObject, state: true);
					consignItemLogicList[j].Reset(list[j + page * PageCount]);
				}
				else
				{
					UnityVersionUtil.SetActiveRecursive(consignItemLogicList[j].gameObject, state: false);
				}
			}
		}
		else
		{
			for (int k = 0; k < consignItemLogicList.Count; k++)
			{
				UnityVersionUtil.SetActiveRecursive(consignItemLogicList[k].gameObject, state: false);
			}
		}
		if (isRefresh)
		{
			grid.Reposition();
			scrollView.ResetPosition();
		}
	}

	public void ClickRightBtn()
	{
		CurPage++;
		if (CurPage >= MaxPage)
		{
			CurPage = MaxPage - 1;
		}
		else
		{
			RefreshPage(currentConsignItemList, CurPage, isRefresh: true);
		}
	}

	public void ClickLeftBtn()
	{
		CurPage--;
		if (CurPage < 0)
		{
			CurPage = 0;
		}
		else
		{
			RefreshPage(currentConsignItemList, CurPage, isRefresh: true);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
