using UnityEngine;

public class DanceToolItem : MonoBehaviour
{
	public UISprite IconSprite;

	public UILabel NumLabel;

	public UISprite CDSprite;

	public long ReamainTime;

	private long allTime = 1L;

	private float tempCountTime;

	public void Init(string itemid, int num)
	{
		ItemData itemDataByID = DataManager.GetItemDataByID(itemid);
		if (itemDataByID != null)
		{
			NGUITools.SetActive(base.gameObject, state: true);
			IconSprite.spriteName = GameDefine.DanceToolName[itemid];
			NumLabel.text = num.ToString();
		}
		else
		{
			NGUITools.SetActive(base.gameObject, state: false);
		}
		CDSprite.fillAmount = 0f;
		ReamainTime = -1L;
	}

	public void SetCDTime(long time, long alltime)
	{
		ReamainTime = time - SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime();
		allTime = alltime;
		if (ReamainTime > 0)
		{
			CDSprite.fillAmount = (float)ReamainTime / (float)allTime;
		}
		else
		{
			CDSprite.fillAmount = 0f;
		}
	}

	private void Update()
	{
		if (ReamainTime > 0)
		{
			tempCountTime += Time.deltaTime;
			if (tempCountTime >= 1f)
			{
				ReamainTime--;
				tempCountTime -= 1f;
				CDSprite.fillAmount = (float)ReamainTime / (float)allTime;
			}
			if (ReamainTime <= 0)
			{
				tempCountTime = 0f;
				CDSprite.fillAmount = 0f;
			}
		}
	}
}
