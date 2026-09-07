public class TowerCurrentFloorInfoLogic : SingletonUnity<TowerCurrentFloorInfoLogic>
{
	public UILabel currentFloorLabel;

	public void ShowCurrentFloor(int floor)
	{
		currentFloorLabel.text = $"Floor {floor}";
	}

	public static void ShowFloor(int floor)
	{
		if (SingletonUnity<TowerCurrentFloorInfoLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<TowerCurrentFloorInfoLogic>.Instance.gameObject))
		{
			SingletonUnity<TowerCurrentFloorInfoLogic>.Instance.ShowCurrentFloor(floor);
			return;
		}
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.TowerCurrentFloorInfo, delegate(bool bSuccess, object param)
		{
			if (bSuccess)
			{
				SingletonUnity<TowerCurrentFloorInfoLogic>.Instance.ShowCurrentFloor(floor);
			}
		});
	}
}
