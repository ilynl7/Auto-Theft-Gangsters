public class AnimationEditorManager : SceneManager
{
	public override void Init(string id)
	{
		base.Init(id);
		SingletonUnity<MyEvent>.Instance.Register("OnMainPlayerCreate", this, "OnMainPlayerCreate");
	}

	public void OnMainPlayerCreate()
	{
		UIManager instance = SingletonUnity<UIManager>.Instance;
		instance.ShowUI(UIInfo.YiDongKongZhiUI);
		instance.ShowUI(UIInfo.AnimationEditor, delegate
		{
			SingletonUnity<AnimaEditorUIRootLogic>.Instance.Init();
		});
	}

	public override void Update()
	{
	}
}
