using UnityEngine;

public class PotionItem : MonoBehaviour
{
	public UISprite IconSprite;

	public UILabel NumLabel;

	private GameItem useItem;

	private void Start()
	{
	}

	public void Init(GameItem gameItem)
	{
		useItem = gameItem;
		ItemData itemDataByID = DataManager.GetItemDataByID(useItem.ItemId);
		IconSprite.spriteName = itemDataByID.BackPackIcon + "_Min";
		NumLabel.text = gameItem.StackNum.ToString();
	}

	public void ClickUseItem()
	{
		Debug.Log("ClickUseItem");
		if (SingletonUnity<PotionLogic>.Exists)
		{
			SingletonUnity<PotionLogic>.Instance.ClickSelectItem(useItem);
		}
	}
}
