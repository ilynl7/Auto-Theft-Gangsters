using SprotoType;

public class PlayerSlotData
{
	private int mFreeTimes;

	private int mSumTimes;

	private int mNotGetItemsNum;

	public void UpdateSlotData(ret_slot_info.request request)
	{
		mFreeTimes = (int)request.slot_info.curNum;
		mSumTimes = (int)request.slot_info.sumNum;
		if (request.HasSlot_items)
		{
			mNotGetItemsNum = request.slot_items.Count;
		}
		else
		{
			mNotGetItemsNum = 0;
		}
		UpdateTips();
	}

	public void UpdateTips()
	{
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.CheckTips(CheckTips(), GameDefine.TIPS_TYPE.SLOT);
		}
	}

	public void SetInfo(int freetimes, int sum, int notget)
	{
		mFreeTimes = freetimes;
		mSumTimes = sum;
		mNotGetItemsNum = notget;
		UpdateTips();
	}

	public bool CheckTips()
	{
		if (mFreeTimes > 0 || mNotGetItemsNum > 0)
		{
			return true;
		}
		return false;
	}
}
