using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class MailUIRootLogic : SingletonUnity<MailUIRootLogic>
{
	public List<MailItemLogic> MailItems = new List<MailItemLogic>();

	public UIGrid uiWrapContent;

	public UIScrollView uiScrollView;

	public ShowRewardItems ShowRewardItemScript;

	public UILabel MailTitleLabel;

	public UILabel MailTextLabel;

	public GameObject NoSelectMailObj;

	public GameObject SelectMailObj;

	public UILabel ItemGetLabel;

	public UILabel PageLabel;

	public int LineCount = 10;

	private List<FriendInfo.Mail> curMailList = new List<FriendInfo.Mail>();

	private int curPage = 1;

	private int maxPage = 1;

	private long curMailid = -1L;

	private FriendInfo.Mail curMail;

	public GameObject GetItemBtnObj;

	public UISprite GetItemSP;

	public UISprite ReceiveFlag;

	public UILabel TimeLabel;

	public UIScrollView rewardview;

	protected override void Awake()
	{
		base.Awake();
	}

	private void OnEnable()
	{
		curPage = 1;
		curMailid = -1L;
		InitList();
		curMail = null;
	}

	private void InitList()
	{
		for (int i = 0; i < MailItems.Count; i++)
		{
			MailItems[i].InitSelct();
			NGUITools.SetActive(MailItems[i].gameObject, state: false);
		}
	}

	public void Reset()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		playerData.FriendInfo.SortMailList();
		UpdateMailList();
		uiScrollView.ResetPosition();
	}

	private void UpdateSelectItem(long mailid)
	{
		if (mailid == -1 || curMailList.Count == 0)
		{
			NGUITools.SetActive(SelectMailObj, state: false);
			NGUITools.SetActive(NoSelectMailObj, state: true);
			for (int i = 0; i < MailItems.Count; i++)
			{
				MailItems[i].InitSelct();
			}
			uiScrollView.ResetPosition();
			return;
		}
		for (int j = 0; j < MailItems.Count; j++)
		{
			MailItems[j].UpdateSelect(mailid);
		}
		NGUITools.SetActive(SelectMailObj, state: true);
		NGUITools.SetActive(NoSelectMailObj, state: false);
		for (int k = 0; k < curMailList.Count; k++)
		{
			if (mailid == curMailList[k].key)
			{
				curMail = curMailList[k];
			}
		}
		MailTitleLabel.text = StrDictionary.GetServerDictionaryString(curMail.title);
		string serverDictionaryString = StrDictionary.GetServerDictionaryString(curMail.text);
		if (!string.IsNullOrEmpty(serverDictionaryString))
		{
			MailTextLabel.text = serverDictionaryString.Replace("#r", "\n");
		}
		else
		{
			MailTextLabel.text = string.Empty;
		}
		DateTime dateTime = DateTimeTool.LongToDateTimeLocal(curMail.time);
		TimeLabel.text = $"{dateTime.Year}-{dateTime.Month:D2}-{dateTime.Day:D2}";
		OpenMail();
		ShowRewardItemScript.ShowRewards(curMail.items);
		rewardview.ResetPosition();
		if (curMail.IsGetItem())
		{
			ItemGetLabel.text = StrDictionary.GetDictionaryString("#{100238}");
			GetItemSP.spriteName = "CZ_anNiu_2+";
			ReceiveFlag.enabled = true;
		}
		else
		{
			ItemGetLabel.text = StrDictionary.GetDictionaryString("#{100237}");
			GetItemSP.spriteName = "CZ_anNiu_1";
			ReceiveFlag.enabled = false;
		}
		NGUITools.SetActive(GetItemBtnObj, curMail.IsHaveItem());
	}

	public void UpdateMailList()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		List<FriendInfo.Mail> userMailList = playerData.FriendInfo.UserMailList;
		maxPage = (userMailList.Count + LineCount - 1) / LineCount;
		curMailList.Clear();
		for (int i = (curPage - 1) * LineCount; i < curPage * LineCount && i < userMailList.Count; i++)
		{
			curMailList.Add(userMailList[i]);
		}
		int num = Mathf.Min(curMailList.Count, LineCount) - MailItems.Count;
		int count = MailItems.Count;
		if (num > 0)
		{
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(MailItems[0].gameObject) as GameObject;
				gameObject.name = $"huaDongTiao_{count + j:d2}";
				MailItemLogic component = gameObject.GetComponent<MailItemLogic>();
				if (component != null)
				{
					component.transform.parent = MailItems[0].transform.parent;
					component.transform.localScale = Vector3.one;
					component.transform.localPosition = Vector3.zero;
					MailItems.Add(component);
				}
			}
		}
		for (int k = 0; k < MailItems.Count; k++)
		{
			NGUITools.SetActive(MailItems[k].gameObject, k < curMailList.Count);
			if (k < curMailList.Count)
			{
				MailItems[k].UpdateMail(curMailList[k], OnClickMail);
			}
		}
		PageLabel.text = $"{curPage}/{maxPage}";
		uiWrapContent.Reposition();
		UpdateSelectItem(curMailid);
	}

	public void OnClickDelBtn()
	{
		if (curMail.IsHaveItem() && !curMail.IsGetItem())
		{
			MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{100245}"), StrDictionary.GetDictionaryString("#{100244}"), OnYesClick);
		}
		else
		{
			OnYesClick();
		}
	}

	public void OnYesClick()
	{
		curMailid = -1L;
		mail_operation.request request = new mail_operation.request();
		request.mailId = curMail.key;
		request.operation = 1L;
		NetLogic.GetInstance().Send<Protocol.mail_operation>(request);
		FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
		friendInfo.DelMail(curMail.key);
		curMail = null;
	}

	public void OnClickGetBtn()
	{
		if (!curMail.IsGetItem())
		{
			mail_operation.request request = new mail_operation.request();
			request.mailId = curMail.key;
			request.operation = 2L;
			NetLogic.GetInstance().Send<Protocol.mail_operation>(request);
		}
	}

	private void OpenMail()
	{
		if (!curMail.read)
		{
			mail_operation.request request = new mail_operation.request();
			request.mailId = curMail.key;
			request.operation = 0L;
			NetLogic.GetInstance().Send<Protocol.mail_operation>(request);
			curMail.read = true;
			if (curMail.mailstate == 0)
			{
				curMail.mailstate = 1;
			}
			FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
			friendInfo.ReadMail(curMail.key);
		}
	}

	public void OnClickMail(long id)
	{
		curMailid = id;
		UpdateSelectItem(curMailid);
	}

	public void OnClickLeft()
	{
		if (curPage > 1)
		{
			curPage--;
			UpdateMailList();
			uiScrollView.ResetPosition();
		}
	}

	public void OnClickRight()
	{
		if (curPage < maxPage)
		{
			curPage++;
			UpdateMailList();
			uiScrollView.ResetPosition();
		}
	}

	public void OnClikeDeletAll()
	{
		FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
		if (friendInfo.IsHaveMail())
		{
			MessageBoxLogic.OpenOKCancelBox(StrDictionary.GetDictionaryString("#{100246}"), StrDictionary.GetDictionaryString("#{100244}"), delegate
			{
				mail_operation.request rpcReq = new mail_operation.request
				{
					operation = 4L
				};
				NetLogic.GetInstance().Send<Protocol.mail_operation>(rpcReq);
				friendInfo.ClearMailData();
			});
		}
	}

	public void OnClickGetAll()
	{
		FriendInfo friendInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FriendInfo;
		if (friendInfo.IsHaveMail())
		{
			mail_operation.request request = new mail_operation.request();
			request.operation = 3L;
			NetLogic.GetInstance().Send<Protocol.mail_operation>(request);
		}
	}
}
