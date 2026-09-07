using UnityEngine;

public class CopyDrugItemLogic : MonoBehaviour
{
	public UISprite SpriteIcon;

	public UILabel DrugLabel;

	public UILabel RemainLabel;

	public GameObject TweenerObj;

	private float curTime = -1f;

	private GameItem curItem;

	private int count;

	private Color RedColor = new Color(1f, 0.20392157f, 16f / 85f, 1f);

	private Color BlueColor = new Color(16f / 85f, 0.7607843f, 1f, 1f);

	private Color YellowColor = new Color(1f, 81f / 85f, 0.1254902f, 1f);

	public UISprite GuangTiaoSprite;

	public UISprite GuangQuanSprite;

	public void Reset(GameItem item, float time)
	{
		curItem = item;
		count = curItem.StackNum;
		ItemData itemData = item.ItemData;
		SpriteIcon.spriteName = itemData.BackPackIcon + "_Min";
		DrugLabel.text = item.StackNum.ToString();
		RemainLabel.text = string.Empty;
		curTime = time;
		UpdateTime(time);
		if (itemData.BackPackIcon.Contains("Red"))
		{
			GuangTiaoSprite.color = RedColor;
			GuangQuanSprite.color = RedColor;
		}
		else if (itemData.BackPackIcon.Contains("Blue"))
		{
			GuangTiaoSprite.color = BlueColor;
			GuangQuanSprite.color = BlueColor;
		}
		else
		{
			GuangTiaoSprite.color = YellowColor;
			GuangQuanSprite.color = YellowColor;
		}
	}

	public void UpdateTime(float time)
	{
		curTime = time;
		NGUITools.SetActive(TweenerObj, curTime > Time.realtimeSinceStartup);
		if (!(curTime > Time.realtimeSinceStartup))
		{
			RemainLabel.text = string.Empty;
		}
		if (curItem.StackNum <= 0 && !UnityVersionUtil.IsActive(TweenerObj.gameObject))
		{
			NGUITools.SetActive(base.gameObject, state: false);
			SingletonUnity<CopyDrugUseUIRoot>.Instance.OnResetPosition(curItem);
		}
	}

	private void UpdateRemainTime()
	{
		if (curTime > Time.realtimeSinceStartup)
		{
			RemainLabel.text = $"{(int)(curTime - Time.realtimeSinceStartup + 0.5f)}";
		}
		else
		{
			RemainLabel.text = string.Empty;
		}
	}

	public void OnClickItem()
	{
		if (curItem.StackNum <= 0)
		{
			NoticeLogic.AddNotifyData2Client(false, "#{100145}", true, curItem.ItemData.MName);
		}
		else
		{
			SingletonUnity<CopyDrugUseUIRoot>.Instance.UseItem(this, curItem);
		}
	}

	private void Update()
	{
		if (curTime >= 0f)
		{
			UpdateRemainTime();
			if (curTime < Time.realtimeSinceStartup)
			{
				curTime = -1f;
				UpdateTime(curTime);
			}
		}
	}
}
