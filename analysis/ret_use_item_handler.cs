using Sproto;
using SprotoType;

public class ret_use_item_handler
{
	public static SprotoTypeBase ret_use_item_request(SprotoTypeBase req)
	{
		if (req is ret_use_item.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			long success = request.success;
			if (success == 1)
			{
				ItemContainer itemContainer = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.GetItemContainer(ITEM_CONTAINER_TYPE.ITEM_BACKPACK);
				if (itemContainer != null)
				{
					GameItem item = itemContainer.GetItemByIndexId(request.indexId);
					item.StackNum--;
					if (item.StackNum < 1)
					{
						itemContainer.RemoveItem(item);
					}
					if (UIUpdateEvent.UpdateBackPackEvent != null)
					{
						UIUpdateEvent.UpdateBackPackEvent();
					}
					if (item.ItemData.Type == GameDefine.ITEM_TYPE.POTION)
					{
						if (SingletonUnity<PotionLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PotionLogic>.Instance.gameObject))
						{
							SingletonUnity<PotionLogic>.Instance.Reset();
						}
					}
					else if (item.ItemData.Type == GameDefine.ITEM_TYPE.EXCHANGE)
					{
						if (SingletonUnity<PlayerCarRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<PlayerCarRootLogic>.Instance.gameObject))
						{
							string text = item.ItemData.Function.ToString();
							GameManager instance = SingletonDontDestoryUnity<GameManager>.Instance;
							instance.PlayerData.playerMountData.SetMountState(text);
							SingletonUnity<PlayerCarRootLogic>.Instance.CurMountInfoDic[text].state = 1L;
							SingletonUnity<PlayerCarRootLogic>.Instance.UpdateCarPage(item.ItemData.Function.ToString());
						}
						SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GetCarUIRoot, delegate
						{
							SingletonUnity<GetCarUIRootLogic>.Instance.Reset(item.ItemData.Function.ToString());
							if (SingletonUnity<PlayerCarRootLogic>.Exists && TutorialManager.CurStep == TUTORIAL_STEP.CAR_CLICK_GET)
							{
								SingletonUnity<PlayerCarRootLogic>.Instance.CheckTutorialEvent();
							}
						});
					}
				}
			}
			else if (UIManager.IsUnlockTutorialEnable())
			{
				TutorialManager.CloseTutorial();
			}
		}
		return null;
	}
}
