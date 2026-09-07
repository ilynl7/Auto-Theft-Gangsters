using SprotoType;
using UnityEngine;

public class WorldItemLogic : MonoBehaviour
{
	public UILabel mapNameLabel;

	public UISprite mapIconSprite;

	public GameObject curSceneFlag;

	public UISprite OpenFlag;

	public UISprite GangFlag;

	private bool IsActivityMap;

	private Vector3 TargetPos;

	private guild_map_info CurCaptureInfo;

	private MapInfoData curMapInfoData;

	private bool isLock;

	private bool isCurrScene;

	public void ResetItem(MapInfoData data, bool isactflag = false)
	{
		IsActivityMap = isactflag;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		curMapInfoData = data;
		isLock = playerData.MainPlayerAttrData.Level < curMapInfoData.OpenLv;
		isCurrScene = sceneManager.CurrentMapInofData.ID == data.ID;
		TargetPos = curMapInfoData.BirthPosVector3;
		CurCaptureInfo = null;
		CurCaptureInfo = playerData.ActivityData.GetGuildMapInfo(data.ID);
		TargetPos = playerData.ActivityData.GetCityCaptureNpcPos(data);
		UpdateItem();
	}

	private void UpdateItem()
	{
		if (curMapInfoData.OpenLv > 0)
		{
			mapNameLabel.text = $"{StrDictionary.GetDictionaryString(curMapInfoData.Name)}:Lv.{curMapInfoData.OpenLv}";
		}
		else
		{
			mapNameLabel.text = $"{StrDictionary.GetDictionaryString(curMapInfoData.Name)}";
		}
		mapIconSprite.spriteName = curMapInfoData.MapIcon;
		if (isLock || (IsActivityMap && (CurCaptureInfo == null || CurCaptureInfo.state != 1)))
		{
			mapIconSprite.color = GameDefine.GrayColor;
			OpenFlag.enabled = false;
			GangFlag.enabled = false;
		}
		else
		{
			mapIconSprite.color = Color.white;
			if (CurCaptureInfo != null && (CurCaptureInfo.state == 1 || CurCaptureInfo.HasGuildId))
			{
				if (CurCaptureInfo.state == 1)
				{
					OpenFlag.enabled = true;
					GangFlag.enabled = false;
				}
				else if (CurCaptureInfo.HasGuildId && CurCaptureInfo.HasGuildIcon)
				{
					OpenFlag.enabled = false;
					GangFlag.enabled = true;
				}
				else
				{
					OpenFlag.enabled = false;
					GangFlag.enabled = false;
				}
			}
			else
			{
				OpenFlag.enabled = false;
				GangFlag.enabled = false;
			}
		}
		base.transform.localPosition = curMapInfoData.WoldPosVector3;
		NGUITools.SetActive(curSceneFlag, isCurrScene);
	}

	public void OnClick()
	{
		if ((isCurrScene && !IsActivityMap) || (IsActivityMap && (CurCaptureInfo == null || CurCaptureInfo.state != 1)))
		{
			return;
		}
		if (isLock)
		{
			NoticeLogic.AddNotifyData2Client(false, "#{100154}", false, curMapInfoData.OpenLv);
			return;
		}
		AutoSearchPathManager autoSearchPath = SingletonDontDestoryUnity<GameManager>.Instance.AutoSearchPath;
		ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		if (mainPlayer != null)
		{
			mainPlayer.BreakAutoCombatState();
			mainPlayer.SkillLogic.BreakCurSkill();
		}
		if (IsActivityMap)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.StopAutoMoveToMission();
			if (CurCaptureInfo != null)
			{
				enter_guild_city_scene.request request = new enter_guild_city_scene.request();
				request.id = CurCaptureInfo.id;
				NetLogic.GetInstance().Send<Protocol.enter_guild_city_scene>(request);
			}
			else
			{
				SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AutoMoveDest(curMapInfoData.ID, TargetPos, AUTO_SEARCH_PARTH_FINISHEVENT.CHANGE_MAP);
			}
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.WorldMapRoot);
			SingletonUnity<UIManager>.Instance.CloseAllPOPUI();
		}
		else if (autoSearchPath.IsInTelePortCircle)
		{
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange)
			{
				if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
				{
					SingletonUnity<UIManager>.Instance.CloseAllPOPUI();
					SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DownLoadResRoot);
					return;
				}
				SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CopySceneChange = true;
				enter_new_map.request request2 = new enter_new_map.request();
				request2.mapInfoId = curMapInfoData.ID;
				NetLogic.GetInstance().Send<Protocol.enter_new_map>(request2);
				WaitResponseUIRootLogic.OpenWaitBox(106, 10f, 0f);
			}
		}
		else
		{
			SingletonDontDestoryUnity<GameManager>.Instance.MissionManager.AutoMoveDest(curMapInfoData.ID, curMapInfoData.BirthPosVector3, AUTO_SEARCH_PARTH_FINISHEVENT.CHANGE_MAP);
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.WorldMapRoot);
		}
	}
}
