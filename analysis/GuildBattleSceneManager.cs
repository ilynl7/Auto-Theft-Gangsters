using System.Collections.Generic;
using UnityEngine;

public class GuildBattleSceneManager : SceneManager
{
	private float CheckInterTime;

	private float curTemptime;

	private ObjMainPlayer mMainPlayer;

	private List<GuildBattlePoint> mBattlePointList = new List<GuildBattlePoint>();

	public override void Init(string id)
	{
		base.Init(id);
		CheckInterTime = 2f;
		curTemptime = 0f;
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.GuildBattleInfoRoot, delegate
		{
			SingletonUnity<GuildBattleInfoRoot>.Instance.EnableReset();
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.GuildBattleInfoRoot);
		});
		List<MapAreaInfoData> mapAreaInfoDataListById = DataManager.GetMapAreaInfoDataListById(mapInfoData.GatherAreaId);
		if (mapAreaInfoDataListById != null && mapAreaInfoDataListById.Count > 0)
		{
			GameObject gameObject = ResourcesManager.LoadAndInstantiate("Items/GuildBattlePoint") as GameObject;
			mBattlePointList.Add(gameObject.GetComponent<GuildBattlePoint>());
			for (int i = 1; i < mapAreaInfoDataListById.Count; i++)
			{
				mBattlePointList.Add((Object.Instantiate(gameObject) as GameObject).GetComponent<GuildBattlePoint>());
			}
			for (int j = 0; j < mapAreaInfoDataListById.Count; j++)
			{
				mBattlePointList[j].Reset(j, new Vector3(mapAreaInfoDataListById[j].PointList[0].x, SceneManager.GetHitHeight(mapAreaInfoDataListById[j].PointList[0].x, mapAreaInfoDataListById[j].PointList[0].y), mapAreaInfoDataListById[j].PointList[0].y), mapAreaInfoDataListById[j].CircleRange, Color.white);
			}
		}
	}

	public override void OnLoadingOver()
	{
		base.OnLoadingOver();
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		LockControl();
	}

	private void LockControl()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.JueseJiNengQuUI);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.YiDongKongZhiUI);
	}

	public void OpenControl()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.JueseJiNengQuUI);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.YiDongKongZhiUI);
	}

	~GuildBattleSceneManager()
	{
	}

	public void UpdatePointColor(long id, Color col)
	{
		for (int i = 0; i < mBattlePointList.Count; i++)
		{
			if (mBattlePointList[i].Id == id)
			{
				mBattlePointList[i].UpdateColor(col);
			}
		}
	}

	public override void Update()
	{
		base.Update();
		curTemptime += Time.deltaTime;
		if (curTemptime > CheckInterTime)
		{
			curTemptime = 0f;
			NetLogic.GetInstance().Send<Protocol.req_guild_score_info>();
		}
	}
}
