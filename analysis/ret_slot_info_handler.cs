using Sproto;
using SprotoType;

public class ret_slot_info_handler
{
	public static SprotoTypeBase ret_slot_info_request(SprotoTypeBase req)
	{
		if (req is ret_slot_info.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
			instance.PlayerData.playerSlotData.UpdateSlotData(request);
			if (SingletonUnity<SlotUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SlotUIRootLogic>.Instance.gameObject))
			{
				SingletonUnity<SlotUIRootLogic>.Instance.Reset(request);
			}
		}
		return null;
	}
}
