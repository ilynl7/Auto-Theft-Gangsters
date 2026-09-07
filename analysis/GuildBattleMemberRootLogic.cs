using System;
using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildBattleMemberRootLogic : SingletonUnity<GuildBattleMemberRootLogic>
{
	public UIWrapContentNew uiWrapContent;

	private int lineMinCount = 7;

	public UIWidget WrapContentBottomWidget;

	public UIScrollView uiScrollView;

	private List<guild_member_info> GuildMemberList = new List<guild_member_info>();

	public List<GuildBattleMemberLine> GuildMemberLines;

	public UILabel NumLabel;

	private List<long> SelectList = new List<long>();

	public UISprite ConfirmBtnSp;

	private GuildBattleData curGuildBattleData;

	protected override void Awake()
	{
		base.Awake();
		uiWrapContent.enabled = false;
		UIWrapContentNew uIWrapContentNew = uiWrapContent;
		uIWrapContentNew.onInitializeItem = (UIWrapContentNew.OnInitializeItem)Delegate.Combine(uIWrapContentNew.onInitializeItem, new UIWrapContentNew.OnInitializeItem(OnInitializeItem));
	}

	public void EnableReset()
	{
		for (int i = 0; i < GuildMemberLines.Count; i++)
		{
			NGUITools.SetActive(GuildMemberLines[i].gameObject, state: false);
		}
		NumLabel.text = string.Empty;
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.isGuildChief())
		{
			NGUITools.SetActive(ConfirmBtnSp.gameObject, state: true);
		}
		else
		{
			NGUITools.SetActive(ConfirmBtnSp.gameObject, state: false);
		}
	}

	private void InitGuildMemberList()
	{
		GuildMemberList = new List<guild_member_info>();
		long serverId = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.ServerId;
		int num = 1;
		for (int i = 0; i < 15; i++)
		{
			GuildMemberList.Add(new guild_member_info());
			GuildMemberList[i].guildId = serverId;
			GuildMemberList[i].characterId = UUID.GenUUID();
			GuildMemberList[i].combValue = UnityEngine.Random.Range(10000, 20000);
			GuildMemberList[i].name = string.Empty + i;
			GuildMemberList[i].profession = UnityEngine.Random.Range(0, 3);
			GuildMemberList[i].job = UnityEngine.Random.Range(0, 2);
			GuildMemberList[i].battle = ((num < 10) ? 1 : 0);
			GuildMemberList[i].state = UnityEngine.Random.Range(0, 2);
			num++;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		GuildMemberList.Insert(0, new guild_member_info());
		GuildMemberList[0].guildId = serverId;
		GuildMemberList[0].characterId = PlayerData.MainPlayerServerId;
		GuildMemberList[0].combValue = playerData.MainPlayerAttrData.ComboValue;
		GuildMemberList[0].name = playerData.MainPlayerAttrData.Name;
		GuildMemberList[0].profession = (long)playerData.Profession;
		GuildMemberList[0].job = (long)playerData.PlayerGuild.PlayerJob;
		GuildMemberList[0].battle = 1L;
		GuildMemberList[0].state = 1L;
	}

	public void RefershInfo(ret_guild_battle_member.request request)
	{
		SelectList.Clear();
		GuildMemberList.Clear();
		if (request.HasGuild_member_info)
		{
			GuildMemberList = request.guild_member_info;
		}
		GuildMemberList.Sort((guild_member_info x, guild_member_info y) => (int)(y.combValue - x.combValue));
		for (int i = 0; i < GuildMemberList.Count; i++)
		{
			if (GuildMemberList[i].job == 0L)
			{
				guild_member_info item = GuildMemberList[i];
				GuildMemberList.RemoveAt(i);
				GuildMemberList.Insert(0, item);
				break;
			}
		}
		int num = Mathf.Min(GuildMemberList.Count, lineMinCount) - GuildMemberLines.Count;
		if (num > 0)
		{
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(GuildMemberLines[0].gameObject) as GameObject;
				GuildBattleMemberLine component = gameObject.GetComponent<GuildBattleMemberLine>();
				gameObject.name = $"{GuildMemberLines.Count:D2}";
				gameObject.transform.parent = uiWrapContent.transform;
				gameObject.transform.localScale = Vector3.one;
				gameObject.transform.localPosition = Vector3.zero;
				GuildMemberLines.Add(component);
			}
		}
		for (int k = 0; k < GuildMemberLines.Count; k++)
		{
			if (k < GuildMemberList.Count)
			{
				NGUITools.SetActive(GuildMemberLines[k].gameObject, state: true);
				GuildMemberLines[k].UpdateInfo(GuildMemberList[k], k);
			}
			else
			{
				NGUITools.SetActive(GuildMemberLines[k].gameObject, state: false);
			}
		}
		uiWrapContent.minIndex = 1 - GuildMemberList.Count;
		WrapContentBottomWidget.height = GuildMemberList.Count * uiWrapContent.itemSize;
		uiWrapContent.SortBasedOnScrollMovement();
		uiScrollView.ResetPosition();
		uiWrapContent.enabled = true;
		for (int l = 0; l < GuildMemberList.Count; l++)
		{
			if (GuildMemberList[l].battle == 1)
			{
				SelectList.Add(GuildMemberList[l].characterId);
			}
		}
		if (SingletonUnity<GuildBattleRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildBattleRootLogic>.Instance.gameObject))
		{
			curGuildBattleData = DataManager.GetGuildBattleDataById(SingletonUnity<GuildBattleRootLogic>.Instance.CurBattleInfo.ID);
		}
		NumLabel.text = StrDictionary.GetDictionaryString("#{105047}") + ": " + SelectList.Count + "/" + curGuildBattleData.MaxPlayNum;
	}

	public bool OnClickSelectAdd(long selectIndex)
	{
		if (SelectList.Count >= 10)
		{
			NoticeLogic.AddNotifyData("#{105073}");
			return false;
		}
		SelectList.Add(selectIndex);
		NumLabel.text = StrDictionary.GetDictionaryString("#{105047}") + ": " + SelectList.Count + "/" + curGuildBattleData.MaxPlayNum;
		return true;
	}

	public void OnClickSelectDec(long selectIndex)
	{
		if (SelectList.Contains(selectIndex))
		{
			SelectList.Remove(selectIndex);
		}
		NumLabel.text = StrDictionary.GetDictionaryString("#{105047}") + ": " + SelectList.Count + "/" + curGuildBattleData.MaxPlayNum;
	}

	public void OnClickConfirmBtn()
	{
		Guild playerGuild = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild;
		if (playerGuild.GuildMemberNum >= 10 && SelectList.Count < 10)
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{105074}", curGuildBattleData.MaxPlayNum));
			return;
		}
		if (playerGuild.GuildMemberNum < 10 && SelectList.Count < playerGuild.GuildMemberNum)
		{
			NoticeLogic.AddNotifyData(StrDictionary.GetDictionaryString("#{105074}", curGuildBattleData.MaxPlayNum));
			return;
		}
		if (!SingletonUnity<GuildBattleRootLogic>.Instance.IsPreparingState() || SingletonUnity<GuildBattleRootLogic>.Instance.CurBattleInfo.time - SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime() <= 0)
		{
			NoticeLogic.AddNotifyData("#{105050}");
			return;
		}
		set_guild_battle_member.request request = new set_guild_battle_member.request();
		request.list = SelectList;
		NetLogic.GetInstance().Send<Protocol.set_guild_battle_member>(request);
		OnClickCloseBtn();
		NoticeLogic.AddNotifyData("#{105057}");
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleMemberRoot);
	}

	private void OnInitializeItem(GameObject obj, int index, int realIndex)
	{
		if (index < GuildMemberLines.Count)
		{
			GuildBattleMemberLine itemLogic = GuildMemberLines[index];
			ResetItemLine(itemLogic, Mathf.Abs(realIndex));
		}
	}

	private void ResetItemLine(GuildBattleMemberLine itemLogic, int idx)
	{
		if (idx < GuildMemberList.Count)
		{
			itemLogic.UpdateInfo(GuildMemberList[idx], idx);
		}
	}
}
