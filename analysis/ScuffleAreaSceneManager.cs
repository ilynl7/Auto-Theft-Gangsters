public class ScuffleAreaSceneManager : SceneManager
{
	protected ScuffleData scuffleData;

	public override void Init(string id)
	{
		base.Init(id);
		scuffleData = DataManager.GetScuffleDataById(base.CurrentMapInofData.Param1);
		int floorId = ((!scuffleData.MapID.Equals(base.CurrentMapInofData.ID)) ? 1 : 0);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ScuffleAreaInfoRoot, delegate
		{
			SingletonUnity<ScuffleAreaInfoRoot>.Instance.Reset(floorId, scuffleData);
		});
	}

	public virtual void MoveToNextFloor()
	{
	}
}
