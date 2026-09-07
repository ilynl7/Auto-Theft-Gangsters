using Sproto;
using SprotoType;

public class ret_guild_req_info_handler
{
	public static SprotoTypeBase ret_guild_req_info_request(SprotoTypeBase req)
	{
		if (req is ret_guild_req_info.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			if (request.HasGuild_info)
			{
				playerData.PlayerGuild.Init(request.guild_info, request.donate_records);
				playerData.PlayerGuild.UpdateAllContribute((int)(request.HasAll_contribute ? request.all_contribute : 0));
				if (request.HasContribute)
				{
					GameMoneyHelper.SetGuildContribute((int)request.contribute);
				}
				else
				{
					GameMoneyHelper.SetGuildContribute(0L);
				}
				if (string.IsNullOrEmpty(playerData.MainPlayerAttrData.GuildName) && mainPlayer != null)
				{
					PlayerHeadInfoLogic playerHeadInfoLogic = mainPlayer.HeadInfoLogic as PlayerHeadInfoLogic;
					if (SingletonDontDestoryUnity<GameManager>.Instance.SceneManager != null && SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.IsSurviveBattleScene())
					{
						playerHeadInfoLogic.Reset(isMainPlayer: true, playerData.MainPlayerAttrData.CurTitleLevel, playerData.MainPlayerAttrData.Name, request.guild_info.guildName, playerData.MainPlayerAttrData.Camp, ShowHpLine: false, SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsChampionGuild(request.guild_info.guildId));
					}
					else
					{
						playerHeadInfoLogic.Reset(isMainPlayer: true, playerData.MainPlayerAttrData.CurTitleLevel, playerData.MainPlayerAttrData.Name, request.guild_info.guildName, GameDefine.CAMP_TYPE.PLAYER_1, ShowHpLine: false, SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsChampionGuild(request.guild_info.guildId));
					}
				}
				if (SingletonUnity<NewGuildUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewGuildUIRootLogic>.Instance.gameObject))
				{
					SingletonUnity<NewGuildUIRootLogic>.Instance.ShowHasGuildInfo();
				}
			}
			else
			{
				if (!request.HasExist || request.exist)
				{
					NoticeLogic.AddNotifyData("#{100153}");
					if (SingletonUnity<NewGuildUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewGuildUIRootLogic>.Instance.gameObject))
					{
						SingletonUnity<NewGuildUIRootLogic>.Instance.OnClickCloseBtn();
					}
					return null;
				}
				playerData.PlayerGuild.ResetGuild();
				if (SingletonUnity<NewGuildUIRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<NewGuildUIRootLogic>.Instance.gameObject))
				{
					SingletonUnity<NewGuildUIRootLogic>.Instance.RequestGuildList();
				}
			}
		}
		return null;
	}
}
