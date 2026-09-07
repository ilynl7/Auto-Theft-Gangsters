using SprotoType;

public class PlayerRankPVPData
{
	public const int MaxRankPos = 1000;

	public const int MaxRankTimes = 10;

	private int mRankPVPRankNum = -1;

	private int mRankTimes;

	private int mBestRankPVPRankNum = -1;

	public int RankPVPRankNum
	{
		get
		{
			return mRankPVPRankNum;
		}
		set
		{
			mRankPVPRankNum = value;
		}
	}

	public int RankTimes
	{
		get
		{
			return mRankTimes;
		}
		set
		{
			mRankTimes = value;
		}
	}

	public int BestRankPVPRankNum
	{
		get
		{
			return mBestRankPVPRankNum;
		}
		set
		{
			mBestRankPVPRankNum = value;
		}
	}

	public string GetRankPosStr()
	{
		if (mRankPVPRankNum > 0 && mRankPVPRankNum < 1001)
		{
			return mRankPVPRankNum.ToString();
		}
		return "--";
	}

	public string GetBestRankPosStr()
	{
		if (mBestRankPVPRankNum > 0 && mBestRankPVPRankNum < 1001)
		{
			return mBestRankPVPRankNum.ToString();
		}
		return "--";
	}

	public void Reset()
	{
		mRankTimes = 0;
		mBestRankPVPRankNum = -1;
	}

	public void UpdateRankPvPData(syn_rank_pvp_data.request request)
	{
		if (request.HasRankPos)
		{
			mRankPVPRankNum = (int)request.rankPos;
		}
		else
		{
			mRankPVPRankNum = -1;
		}
		if (request.HasBestRankPos)
		{
			mBestRankPVPRankNum = (int)request.bestRankPos;
		}
		else
		{
			mBestRankPVPRankNum = -1;
		}
		mRankTimes = (int)request.times;
		UpdateTips();
	}

	public void UpdateRankPvpInfo(tiantti_result.request request)
	{
		if (request.HasRankPos2)
		{
			mRankPVPRankNum = (int)request.rankPos2;
		}
		if (request.HasBestRankPos)
		{
			mBestRankPVPRankNum = (int)request.bestRankPos;
		}
		else
		{
			mBestRankPVPRankNum = -1;
		}
	}

	public void SyncTimesPVPlocal()
	{
		if (mRankTimes > 0)
		{
			mRankTimes--;
		}
		UpdateTips();
	}

	public bool IsHavePVPTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.RANK_PVP))
		{
			return false;
		}
		return mRankTimes > 0;
	}

	public void UpdateTips()
	{
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.UpdateMessageTips();
		}
		if (SingletonUnity<MenuBaseRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<MenuBaseRootLogic>.Instance.gameObject))
		{
			SingletonUnity<MenuBaseRootLogic>.Instance.RefershTips();
		}
		if (SingletonUnity<ActivityTipsRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<ActivityTipsRootLogic>.Instance.gameObject))
		{
			SingletonUnity<ActivityTipsRootLogic>.Instance.UpdateInfo();
		}
	}
}
