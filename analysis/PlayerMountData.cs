using System.Collections.Generic;
using SprotoType;

public class PlayerMountData
{
	private List<mount> mCurMountInfoList = new List<mount>();

	public void UpdateMountInfo(ret_mount_info.request request)
	{
		mCurMountInfoList = new List<mount>(request.mount_info.Values);
		UpdateTips();
	}

	public void UpdateTips()
	{
		if (SingletonUnity<FunctionBtnRootLogic>.Exists)
		{
			SingletonUnity<FunctionBtnRootLogic>.Instance.CheckTips(CheckTips(), GameDefine.TIPS_TYPE.VEHICLE);
		}
	}

	public void SetMountState(string id)
	{
		for (int i = 0; i < mCurMountInfoList.Count; i++)
		{
			if (mCurMountInfoList[i].ID.Equals(id))
			{
				mCurMountInfoList[i].state = 1L;
				break;
			}
		}
		UpdateTips();
	}

	public bool CheckTips()
	{
		ItemContainer itemBackPack = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.ItemBackPack;
		List<GameItem> targetTypeItem = ItemContainerTool.GetTargetTypeItem(itemBackPack, IsAll: false, GameDefine.ITEM_TYPE.EXCHANGE);
		if (targetTypeItem == null || targetTypeItem.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < mCurMountInfoList.Count; i++)
		{
			if (mCurMountInfoList[i].state != 0L)
			{
				continue;
			}
			for (int j = 0; j < targetTypeItem.Count; j++)
			{
				if (targetTypeItem[j].ItemData.Function.ToString().Equals(mCurMountInfoList[i].ID))
				{
					return true;
				}
			}
		}
		return false;
	}
}
