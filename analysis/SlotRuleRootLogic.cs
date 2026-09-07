using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class SlotRuleRootLogic : SingletonUnity<SlotRuleRootLogic>
{
	public UILabel InfoLabel;

	public List<SlotItemLogic> slotItemsList;

	public UIGrid parentGrid;

	private List<slot_data> SlotDataList;

	public void Reset(List<slot_data> curlist)
	{
		InfoLabel.text = StrDictionary.GetDictionaryString("#{101632}");
		SlotDataList = curlist;
		SlotDataList.Sort((slot_data x, slot_data y) => (x.ID.Length != y.ID.Length) ? (x.ID.Length - y.ID.Length) : x.ID.CompareTo(y.ID));
		int num = SlotDataList.Count - slotItemsList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(slotItemsList[0].gameObject) as GameObject;
				gameObject.transform.parent = parentGrid.transform;
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localScale = Vector3.one;
				SlotItemLogic component = gameObject.GetComponent<SlotItemLogic>();
				slotItemsList.Add(component);
				gameObject.gameObject.name = $"slotitem{slotItemsList.Count - 1:D2}";
			}
		}
		for (int j = 0; j < slotItemsList.Count; j++)
		{
			if (j < SlotDataList.Count)
			{
				UnityVersionUtil.SetActiveRecursive(slotItemsList[j].gameObject, state: true);
				slotItemsList[j].Reset(SlotDataList[j]);
			}
			else
			{
				UnityVersionUtil.SetActiveRecursive(slotItemsList[j].gameObject, state: false);
			}
		}
		parentGrid.Reposition();
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SlotRuleRoot);
	}
}
