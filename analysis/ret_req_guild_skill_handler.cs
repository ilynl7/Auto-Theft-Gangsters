using Sproto;
using SprotoType;
using UnityEngine;

public class ret_req_guild_skill_handler
{
	public static SprotoTypeBase ret_req_guild_skill_request(SprotoTypeBase req)
	{
		if (req is ret_req_guild_skill.request request)
		{
			WaitResponseUIRootLogic.CloseBox();
			if (request.HasGuild_skill)
			{
				PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
				playerData.PlayerGuild.UpdateSKill(request.guild_skill);
				if (SingletonUnity<GuildSkillRootLogic>.Exists && UnityVersionUtil.IsActive(SingletonUnity<GuildSkillRootLogic>.Instance.gameObject))
				{
					SingletonUnity<GuildSkillRootLogic>.Instance.UpdateGuildSKill();
				}
			}
			else
			{
				Debug.LogError("ret_req_guild_skill guild skill null!!!");
			}
		}
		return null;
	}
}
