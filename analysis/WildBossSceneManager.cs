using UnityEngine;

public class WildBossSceneManager : SceneManager
{
	private float CheckInterTime;

	private float curTemptime;

	public override void Init(string id)
	{
		base.Init(id);
		CheckInterTime = 2f;
		curTemptime = 0f;
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.MultiRankSmallRootUI, delegate
		{
			SingletonUnity<MultiRankSmallRootLogic>.Instance.EnableReset();
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.MultiRankSmallRootUI);
		});
	}

	~WildBossSceneManager()
	{
	}

	public override void Update()
	{
		base.Update();
		curTemptime += Time.deltaTime;
		if (curTemptime > CheckInterTime)
		{
			curTemptime = 0f;
			NetLogic.GetInstance().Send<Protocol.request_battle_info>();
		}
	}
}
