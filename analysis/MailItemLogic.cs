using UnityEngine;

public class MailItemLogic : MonoBehaviour
{
	public delegate void OnClick(long mailid);

	public UISprite bkSprite;

	public UILabel mailNameLabel;

	public UISprite ItemFlagSprite;

	public UILabel TimeLabel;

	public UILabel MailStateLabel;

	private OnClick onClick;

	private long curMailid = -1L;

	private FriendInfo.Mail curMail;

	public UISprite mailFlag;

	public UIWidget colorAlphwi;

	public void InitSelct()
	{
		bkSprite.spriteName = "CZ_huaDongBG";
	}

	public void UpdateSelect(long selectid)
	{
		if (selectid != curMailid)
		{
			bkSprite.spriteName = "CZ_huaDongBG";
		}
		else
		{
			bkSprite.spriteName = "CZ_huaDongBG_1";
		}
	}

	public void UpdateMail(FriendInfo.Mail mail, OnClick func)
	{
		onClick = func;
		curMail = mail;
		curMailid = mail.key;
		mailNameLabel.text = StrDictionary.GetServerDictionaryString(mail.title);
		if (curMail.read)
		{
			MailStateLabel.enabled = true;
			mailFlag.spriteName = "CZ_youJian_YiLingQu";
			mailFlag.MakePixelPerfect();
			colorAlphwi.alpha = 0.6f;
		}
		else
		{
			MailStateLabel.enabled = false;
			mailFlag.spriteName = "CZ_youJian_WeiLingQu";
			mailFlag.MakePixelPerfect();
			colorAlphwi.alpha = 1f;
		}
		long curServerTime = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime();
		if (curMail.expireday >= 0)
		{
			TimeLabel.text = $"{Mathf.Max(curMail.expireday - TimeTools.GetPassDays(curMail.time, curServerTime), 0f)}";
		}
		else
		{
			TimeLabel.text = $"{Mathf.Max(30 - TimeTools.GetPassDays(curMail.time, curServerTime), 0)}";
		}
		ItemFlagSprite.enabled = curMail.IsHaveItem() && !curMail.IsGetItem();
	}

	public void OnOnClickItemBtn()
	{
		if (onClick != null)
		{
			onClick(curMailid);
		}
	}
}
