using SprotoType;

public class TowerInfoData
{
	private tower_info mPlayerTowerInfo;

	private bool mIsHaveSpecialReward;

	public tower_info PlayerTowerInfo => mPlayerTowerInfo;

	public bool IsHaveSpecialReward => mIsHaveSpecialReward;

	public void Reset()
	{
		if (mPlayerTowerInfo != null)
		{
			mPlayerTowerInfo = null;
		}
		mIsHaveSpecialReward = false;
	}

	public void ResetTowerData()
	{
		if (mPlayerTowerInfo != null)
		{
			mPlayerTowerInfo.times = 0L;
			mPlayerTowerInfo.cur_floor = 0L;
			UpdateTips();
		}
	}

	public void UpdateTowerData(tower_info info)
	{
		mPlayerTowerInfo = info;
		UpdateTips();
	}

	public void UpdateTowerbest(long bestfloor)
	{
		if (mPlayerTowerInfo != null)
		{
			mPlayerTowerInfo.floor = bestfloor;
		}
	}

	public void UpdateTowerData(ret_grant_tower_reward.request request)
	{
		mPlayerTowerInfo = request.tower_info;
		mIsHaveSpecialReward = false;
		if (request.HasTower_special_reward)
		{
			for (int i = 0; i < request.tower_special_reward.Count; i++)
			{
				if (request.tower_special_reward[i].state == 0L && request.tower_special_reward[i].floor <= mPlayerTowerInfo.floor)
				{
					mIsHaveSpecialReward = true;
					break;
				}
			}
		}
		UpdateTips();
	}

	public void UpdateTowerData(ret_request_tower_copy_info.request request)
	{
		mPlayerTowerInfo = request.tower_info;
		mIsHaveSpecialReward = false;
		if (request.HasTower_special_reward)
		{
			for (int i = 0; i < request.tower_special_reward.Count; i++)
			{
				if (request.tower_special_reward[i].state == 0L && request.tower_special_reward[i].floor <= mPlayerTowerInfo.floor)
				{
					mIsHaveSpecialReward = true;
					break;
				}
			}
		}
		UpdateTips();
	}

	public bool IsHaveTowerTips()
	{
		if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.ACTIVITY_CHALLENGE))
		{
			return false;
		}
		if (mPlayerTowerInfo != null)
		{
			if (IsHaveSpecialReward)
			{
				return true;
			}
			if (mPlayerTowerInfo.cur_floor == 0L && mPlayerTowerInfo.wipe_out_state != 1)
			{
				return true;
			}
			if (mPlayerTowerInfo.cur_floor != 0L && mPlayerTowerInfo.times > 0 && mPlayerTowerInfo.wipe_out_state != 1)
			{
				return true;
			}
			if (mPlayerTowerInfo.wipe_out_state == 2)
			{
				return true;
			}
		}
		return false;
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
