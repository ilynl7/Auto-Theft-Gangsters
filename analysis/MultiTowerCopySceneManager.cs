public class MultiTowerCopySceneManager : SceneManager
{
	private int CurrentFloor;

	public override void Init(string id)
	{
		base.Init(id);
	}

	public void ShowFloor()
	{
		TowerCurrentFloorInfoLogic.ShowFloor(CurrentFloor);
	}
}
