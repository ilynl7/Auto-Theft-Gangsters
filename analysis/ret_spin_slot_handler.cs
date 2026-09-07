using Sproto;
using SprotoType;

public class ret_spin_slot_handler
{
	public static SprotoTypeBase ret_spin_slot_request(SprotoTypeBase req)
	{
		if (req is ret_spin_slot.request request && SingletonUnity<SlotUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<SlotUIRootLogic>.Instance.gameObject))
		{
			SingletonUnity<SlotUIRootLogic>.Instance.UpdateResult(request);
		}
		return null;
	}
}
