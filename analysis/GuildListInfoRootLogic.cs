using System.Collections.Generic;
using System.Text.RegularExpressions;
using SprotoType;
using UnityEngine;

public class GuildListInfoRootLogic : SingletonUnity<GuildListInfoRootLogic>
{
	private const int PAGE_NUM = 10;

	private const float UPDATE_INTERVAL = 1000f;

	public List<GuildListLineItemLogic> GuildList = new List<GuildListLineItemLogic>();

	public UIScrollView ScrollView;

	public UIGrid GuildListGrid;

	public UILabel PageLabel;

	public UIInput SearchInput;

	private int mCurPage;

	private int mMaxPage;

	private List<GuildInfo> mCurGuildList = new List<GuildInfo>();

	private List<GuildInfo> mAllGuildList = new List<GuildInfo>();

	private Dictionary<long, GuildInfo> mAllGuildDic = new Dictionary<long, GuildInfo>();

	private List<float> mLastUpdateTime = new List<float>();

	private bool mSearchResultFlag;

	private void OnEnable()
	{
		for (int i = 0; i < GuildList.Count; i++)
		{
			NGUITools.SetActive(GuildList[i].gameObject, i < mCurGuildList.Count);
		}
	}

	public void PreResetGuildListInfo()
	{
		for (int i = 0; i < GuildList.Count; i++)
		{
			NGUITools.SetActive(GuildList[i].gameObject, state: false);
		}
	}

	public void UpdateGuildListInfo(Dictionary<long, guild_info> guildDic, int curPage, int maxPage)
	{
		if (guildDic == null)
		{
			guildDic = new Dictionary<long, guild_info>();
			guildDic.Clear();
		}
		List<guild_info> list = new List<guild_info>(guildDic.Values);
		list.Sort((guild_info preVal, guild_info nextVal) => (int)(preVal.createTime - nextVal.createTime));
		mCurGuildList.Clear();
		for (int i = 0; i < list.Count; i++)
		{
			GuildInfo guildInfo = new GuildInfo(list[i]);
			mCurGuildList.Add(guildInfo);
			if (mAllGuildDic.ContainsKey(guildInfo.ServerId))
			{
				mAllGuildDic[guildInfo.ServerId] = guildInfo;
				continue;
			}
			mAllGuildList.Add(guildInfo);
			mAllGuildDic.Add(guildInfo.ServerId, guildInfo);
		}
		mCurPage = curPage;
		mMaxPage = maxPage;
		if (mMaxPage > mLastUpdateTime.Count)
		{
			int num = mMaxPage - mLastUpdateTime.Count;
			for (int j = 0; j < num; j++)
			{
				mLastUpdateTime.Add(float.MinValue);
			}
		}
		mLastUpdateTime[mCurPage - 1] = Time.time;
		UpdateGuildLine(10 * (mCurPage - 1));
		mSearchResultFlag = false;
	}

	private void UpdateGuildLine(int startIndex)
	{
		int num = mCurGuildList.Count - GuildList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(GuildList[0].gameObject) as GameObject;
				gameObject.name = $"GangListLineItem_{GuildList.Count}";
				GuildListGrid.AddChild(gameObject.transform);
				gameObject.transform.localScale = Vector3.one;
				GuildList.Add(gameObject.GetComponent<GuildListLineItemLogic>());
			}
		}
		for (int j = 0; j < GuildList.Count; j++)
		{
			NGUITools.SetActive(GuildList[j].gameObject, j < mCurGuildList.Count);
		}
		for (int k = 0; k < mCurGuildList.Count; k++)
		{
			GuildList[k].InitGuildInfo(mCurGuildList[k], startIndex + k + 1);
		}
		PageLabel.text = $"{mCurPage}/{mMaxPage}";
		ScrollView.ResetPosition();
		GuildListGrid.Reposition();
	}

	private void UpdateGuildLine(List<long> indexList)
	{
		int num = mCurGuildList.Count - GuildList.Count;
		if (num > 0)
		{
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate(GuildList[0].gameObject) as GameObject;
				GuildListGrid.AddChild(gameObject.transform);
				gameObject.transform.localScale = Vector3.one;
				GuildList.Add(gameObject.GetComponent<GuildListLineItemLogic>());
			}
			GuildListGrid.Reposition();
		}
		for (int j = 0; j < GuildList.Count; j++)
		{
			NGUITools.SetActive(GuildList[j].gameObject, j < mCurGuildList.Count);
		}
		for (int k = 0; k < mCurGuildList.Count; k++)
		{
			GuildList[k].InitGuildInfo(mCurGuildList[k], (int)indexList[k]);
		}
		PageLabel.text = $"{mCurPage}/{mMaxPage}";
		ScrollView.ResetPosition();
	}

	private void LocalUpdateGuildList(int page)
	{
		mCurPage = page;
		int num = 10 * (mCurPage - 1);
		int num2 = Mathf.Min(10 * mCurPage, mAllGuildList.Count);
		mCurGuildList.Clear();
		for (int i = num; i < num2; i++)
		{
			mCurGuildList.Add(mAllGuildList[i]);
		}
		UpdateGuildLine(10 * (mCurPage - 1));
		mSearchResultFlag = false;
	}

	public void ShowSearchResult(GuildInfo result, int index)
	{
		mCurGuildList.Clear();
		mCurGuildList.Add(result);
		UpdateGuildLine(index);
		PageLabel.text = $"1/1";
	}

	public void OnClickNextPageBtn()
	{
		if (mCurPage < mMaxPage && !mSearchResultFlag)
		{
			if (Time.time - mLastUpdateTime[mCurPage] > 1000f)
			{
				WaitResponseUIRootLogic.OpenWaitBox(152, 10f, 0f);
				guild_req_list.request request = new guild_req_list.request();
				request.characterId = PlayerData.MainPlayerServerId;
				request.curPage = mCurPage + 1;
				NetLogic.GetInstance().Send<Protocol.guild_req_list>(request);
			}
			else
			{
				LocalUpdateGuildList(mCurPage + 1);
			}
			ScrollView.ResetPosition();
		}
	}

	public void OnClickPrePageBtn()
	{
		if (mCurPage > 1 && !mSearchResultFlag)
		{
			if (Time.time - mLastUpdateTime[mCurPage - 2] > 1000f)
			{
				WaitResponseUIRootLogic.OpenWaitBox(152, 10f, 0f);
				guild_req_list.request request = new guild_req_list.request();
				request.characterId = PlayerData.MainPlayerServerId;
				request.curPage = mCurPage - 1;
				NetLogic.GetInstance().Send<Protocol.guild_req_list>(request);
			}
			else
			{
				LocalUpdateGuildList(mCurPage - 1);
			}
			ScrollView.ResetPosition();
		}
	}

	public void OnClickSearchBtn()
	{
		string value = SearchInput.value;
		SearchInput.value = string.Empty;
		value = value.Trim();
		if (string.IsNullOrEmpty(value))
		{
			if (mSearchResultFlag)
			{
				LocalUpdateGuildList(mCurPage);
				mSearchResultFlag = false;
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100794}");
			}
		}
		else if (!StrIsTrue(value))
		{
			NoticeLogic.AddNotifyData("#{100793}");
		}
		else
		{
			mSearchResultFlag = true;
			search_guild.request request = new search_guild.request();
			long num = -1L;
			request.name = value;
			NetLogic.GetInstance().Send<Protocol.search_guild>(request);
			WaitResponseUIRootLogic.OpenWaitBox(176, 10f, 0f);
		}
	}

	private bool StrIsTrue(string str)
	{
		string pattern = "^[a-zA-Z0-9]{1}([a-zA-Z0-9]|[ _]){0,14}$";
		if (Regex.IsMatch(str, pattern))
		{
			return true;
		}
		return false;
	}

	public void ShowSearchResult(List<guild_info> resultList, List<long> rankList)
	{
		mCurGuildList.Clear();
		for (int i = 0; i < resultList.Count; i++)
		{
			GuildInfo item = new GuildInfo(resultList[i]);
			mCurGuildList.Add(item);
		}
		UpdateGuildLine(rankList);
		PageLabel.text = $"1/1";
	}

	public void DisableSearchFlag()
	{
		mSearchResultFlag = false;
	}
}
