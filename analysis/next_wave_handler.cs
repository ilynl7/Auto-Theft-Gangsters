using Sproto;
using SprotoType;

public class next_wave_handler
{
	public static SprotoTypeBase next_wave_request(SprotoTypeBase req)
	{
		if (req is next_wave.request request)
		{
			SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			MapInfoData currentMapInofData = sceneManager.CurrentMapInofData;
			if (currentMapInofData.MapType == MAPTYPE.SINGLE_KILL_MONSTER_COPY)
			{
				if (sceneManager is KillBossLocalSceneManager killBossLocalSceneManager)
				{
					int num = (int)request.waveid;
					killBossLocalSceneManager.OpenBlock(num - 1);
				}
			}
			else if (currentMapInofData.MapType == MAPTYPE.EQUIP_COPY)
			{
				if (sceneManager is EquipCopySceneManager equipCopySceneManager)
				{
					int num2 = (int)request.waveid;
					equipCopySceneManager.OpenBlock(num2 - 1);
				}
			}
			else if (currentMapInofData.MapType == MAPTYPE.MUTIPLE_KILL_MONSTER_COPY)
			{
				if (sceneManager is MultiKillBossSceneManager multiKillBossSceneManager)
				{
					int num3 = (int)request.waveid;
					multiKillBossSceneManager.OpenBlock(num3 - 1);
				}
			}
			else if (currentMapInofData.MapType == MAPTYPE.EXP_DAILY_COPY || currentMapInofData.MapType == MAPTYPE.SINGLE_EXP_DAILY_COPY)
			{
				if (sceneManager is EXPSceneManager eXPSceneManager)
				{
					eXPSceneManager.OpenBlock();
				}
			}
			else if (currentMapInofData.MapType == MAPTYPE.SINGLE_RUN_POINT_COPY && sceneManager is SingleRunPointSceneManager singleRunPointSceneManager)
			{
				singleRunPointSceneManager.OpenBlock();
			}
		}
		return null;
	}
}
