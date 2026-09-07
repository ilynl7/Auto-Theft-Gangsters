using System.Collections.Generic;
using UnityEngine;

public class ConsignPopList : MonoBehaviour
{
	public List<ConsignPopItem> PopItemList = new List<ConsignPopItem>();

	public Transform ItemRoot;

	public UISprite BottomPic;

	public TweenScale tweenObj;

	public UILabel textLabel;

	public ConsignPopItem.OnClickItemDelegate onClickItem;

	public void Init(List<int> itemIndexList, List<string> itemTxtList, ConsignPopItem.OnClickItemDelegate func)
	{
		int count = itemIndexList.Count;
		int num = count - PopItemList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(PopItemList[0].gameObject) as GameObject;
				gameObject.transform.parent = ItemRoot;
				gameObject.transform.localScale = Vector3.one;
				PopItemList.Add(gameObject.GetComponent<ConsignPopItem>());
			}
		}
		else if (num < 0)
		{
			for (int num2 = 0; num2 > num; num2--)
			{
				GameObject obj = PopItemList[PopItemList.Count - 1].gameObject;
				PopItemList.RemoveAt(PopItemList.Count - 1);
				Object.Destroy(obj);
			}
		}
		for (int j = 0; j < count; j++)
		{
			PopItemList[j].Reset(itemIndexList[j], itemTxtList[j], OnClickItem);
			PopItemList[j].transform.localPosition = new Vector3(0f, 15 + (count - j - 1) * 30, 0f);
		}
		BottomPic.height = 30 * count;
		onClickItem = func;
	}

	public void OnClickItem(int index, string val)
	{
		tweenObj.PlayReverse();
		textLabel.text = val;
		if (onClickItem != null)
		{
			onClickItem(index, val);
		}
	}

	private void OnDisable()
	{
		tweenObj.ResetToBeginning();
	}

	public void CloseAnima()
	{
		tweenObj.ResetToBeginning();
	}
}
