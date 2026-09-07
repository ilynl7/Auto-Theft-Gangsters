using System;
using UnityEngine;

public class CountDownTimeLogic : SingletonUnity<CountDownTimeLogic>
{
	public UILabel timeLable;

	public UISprite bkSprite;

	private float reamainTime = -1f;

	private long mType;

	public Transform Offset;

	protected override void Awake()
	{
		base.Awake();
		bkSprite.enabled = false;
		timeLable.text = string.Empty;
	}

	public void SetReamainTime(long time, long type = 0)
	{
		mType = type;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		if (playerCommonData != null)
		{
			reamainTime = time - playerCommonData.GetCurServerTime();
		}
		bkSprite.enabled = true;
		if (reamainTime <= 10f)
		{
			timeLable.color = Color.red;
		}
		else
		{
			timeLable.color = Color.white;
		}
		if (reamainTime <= 0f)
		{
			Close();
		}
	}

	public void Close()
	{
		bkSprite.enabled = false;
		timeLable.text = string.Empty;
		reamainTime = -1f;
	}

	public static void CloseTime()
	{
		if (SingletonUnity<CountDownTimeLogic>.Exists)
		{
			SingletonUnity<CountDownTimeLogic>.Instance.Close();
		}
	}

	private void Update()
	{
		if (!(reamainTime > 0f))
		{
			return;
		}
		reamainTime -= Time.deltaTime;
		if (reamainTime < 0f)
		{
			reamainTime = 0f;
			if (mType > 1)
			{
				CloseTime();
				return;
			}
		}
		if (reamainTime <= 10f)
		{
			timeLable.color = Color.red;
		}
		else
		{
			timeLable.color = Color.white;
		}
		if (mType == 1)
		{
			timeLable.text = $"Start Time:{new TimeSpan(0, 0, (int)reamainTime)}";
		}
		else
		{
			timeLable.text = $"{new TimeSpan(0, 0, (int)reamainTime)}";
		}
	}
}
