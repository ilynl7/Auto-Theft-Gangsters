public class RealPVPSceneManager : SceneManager
{
	public override void Init(string id)
	{
		base.Init(id);
		LockControl();
	}

	public void LockControl()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.JueseJiNengQuUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.YiDongKongZhiUI);
	}

	private void OpenControl()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.JueseJiNengQuUI);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.YiDongKongZhiUI);
	}

	public override void StartGame()
	{
		OpenControl();
	}

	public override void SuccessMission()
	{
	}
}
