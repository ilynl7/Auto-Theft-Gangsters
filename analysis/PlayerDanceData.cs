using System.Collections.Generic;
using SprotoType;

public class PlayerDanceData
{
	private Dictionary<string, dance_info> mPlayerDanceInfoDic;

	private string mCurDanceId;

	private DanceData mCurDanceData;

	private DanceData mCurNormalDanceData;

	private DanceData mCurSpecialDanceData;

	public Dictionary<string, dance_info> PlayerDanceInfoDic => mPlayerDanceInfoDic;

	public string CurDanceId
	{
		get
		{
			return mCurDanceId;
		}
		set
		{
			mCurDanceId = value;
		}
	}

	public DanceData CurDanceData
	{
		get
		{
			return mCurDanceData;
		}
		set
		{
			mCurDanceData = value;
		}
	}

	public DanceData CurNormalDanceData => mCurNormalDanceData;

	public DanceData CurSpecialDanceData => mCurSpecialDanceData;

	public bool IsHaveDanceData()
	{
		return mPlayerDanceInfoDic != null;
	}

	public bool IsSpecialDanceDataEnable()
	{
		return mPlayerDanceInfoDic[mCurSpecialDanceData.ID].enable && mPlayerDanceInfoDic[mCurSpecialDanceData.ID].endTime > SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.GetCurServerTime();
	}

	public void SyncPlayerDanceInfo(ret_request_dance_info.request request)
	{
		if (request.HasCurUse)
		{
			mCurDanceId = request.curUse;
			mCurDanceData = DataManager.GetDanceDataById(mCurDanceId);
		}
		if (!request.HasDance_info)
		{
			return;
		}
		mPlayerDanceInfoDic = request.dance_info;
		List<dance_info> list = new List<dance_info>(mPlayerDanceInfoDic.Values);
		for (int i = 0; i < list.Count; i++)
		{
			DanceData danceDataById = DataManager.GetDanceDataById(list[i].ID);
			if (danceDataById.Price == 0)
			{
				mCurNormalDanceData = danceDataById;
			}
			else
			{
				mCurSpecialDanceData = danceDataById;
			}
		}
	}

	public void SyncPlayerDanceInfo(start_participate_dance.request request)
	{
		if (request.HasCurUse)
		{
			mCurDanceId = request.curUse;
		}
		if (!request.HasDance_info)
		{
			return;
		}
		mPlayerDanceInfoDic = request.dance_info;
		List<dance_info> list = new List<dance_info>(mPlayerDanceInfoDic.Values);
		for (int i = 0; i < list.Count; i++)
		{
			DanceData danceDataById = DataManager.GetDanceDataById(list[i].ID);
			if (danceDataById.Price == 0)
			{
				mCurNormalDanceData = danceDataById;
			}
			else
			{
				mCurSpecialDanceData = danceDataById;
			}
		}
	}

	public void Reset()
	{
		mCurDanceId = string.Empty;
		mCurDanceData = null;
		mPlayerDanceInfoDic = null;
	}
}
