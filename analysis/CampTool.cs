using System.Collections.Generic;

public class CampTool
{
	public static bool CanAttack(GameDefine.CAMP_TYPE selfCamp, GameDefine.CAMP_TYPE targetCamp)
	{
		switch (selfCamp)
		{
		case GameDefine.CAMP_TYPE.PLAYER_1:
			switch (targetCamp)
			{
			case GameDefine.CAMP_TYPE.PLAYER_1:
				return false;
			case GameDefine.CAMP_TYPE.PLAYER_2:
				return true;
			case GameDefine.CAMP_TYPE.NORMAL_NPC:
				return true;
			case GameDefine.CAMP_TYPE.NPC_ATTACK_NPC:
				return true;
			case GameDefine.CAMP_TYPE.STATIC_NPC:
				return true;
			case GameDefine.CAMP_TYPE.FUNCTION_NPC:
				return false;
			case GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC:
				return false;
			}
			break;
		case GameDefine.CAMP_TYPE.PLAYER_2:
			switch (targetCamp)
			{
			case GameDefine.CAMP_TYPE.PLAYER_1:
				return true;
			case GameDefine.CAMP_TYPE.PLAYER_2:
				return false;
			case GameDefine.CAMP_TYPE.NORMAL_NPC:
				return true;
			case GameDefine.CAMP_TYPE.NPC_ATTACK_NPC:
				return true;
			case GameDefine.CAMP_TYPE.STATIC_NPC:
				return true;
			case GameDefine.CAMP_TYPE.FUNCTION_NPC:
				return false;
			case GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC:
				return false;
			}
			break;
		case GameDefine.CAMP_TYPE.NORMAL_NPC:
			switch (targetCamp)
			{
			case GameDefine.CAMP_TYPE.PLAYER_1:
				return true;
			case GameDefine.CAMP_TYPE.PLAYER_2:
				return true;
			case GameDefine.CAMP_TYPE.NORMAL_NPC:
				return false;
			case GameDefine.CAMP_TYPE.NPC_ATTACK_NPC:
				return false;
			case GameDefine.CAMP_TYPE.STATIC_NPC:
				return false;
			case GameDefine.CAMP_TYPE.FUNCTION_NPC:
				return false;
			case GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC:
				return true;
			}
			break;
		case GameDefine.CAMP_TYPE.NPC_ATTACK_NPC:
			switch (targetCamp)
			{
			case GameDefine.CAMP_TYPE.PLAYER_1:
				return true;
			case GameDefine.CAMP_TYPE.PLAYER_2:
				return true;
			case GameDefine.CAMP_TYPE.NORMAL_NPC:
				return false;
			case GameDefine.CAMP_TYPE.NPC_ATTACK_NPC:
				return false;
			case GameDefine.CAMP_TYPE.STATIC_NPC:
				return false;
			case GameDefine.CAMP_TYPE.FUNCTION_NPC:
				return false;
			case GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC:
				return true;
			}
			break;
		case GameDefine.CAMP_TYPE.STATIC_NPC:
			switch (targetCamp)
			{
			case GameDefine.CAMP_TYPE.PLAYER_1:
				return false;
			case GameDefine.CAMP_TYPE.PLAYER_2:
				return false;
			case GameDefine.CAMP_TYPE.NORMAL_NPC:
				return false;
			case GameDefine.CAMP_TYPE.NPC_ATTACK_NPC:
				return false;
			case GameDefine.CAMP_TYPE.STATIC_NPC:
				return false;
			case GameDefine.CAMP_TYPE.FUNCTION_NPC:
				return false;
			case GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC:
				return false;
			}
			break;
		case GameDefine.CAMP_TYPE.FUNCTION_NPC:
			switch (targetCamp)
			{
			case GameDefine.CAMP_TYPE.PLAYER_1:
				return false;
			case GameDefine.CAMP_TYPE.PLAYER_2:
				return false;
			case GameDefine.CAMP_TYPE.NORMAL_NPC:
				return false;
			case GameDefine.CAMP_TYPE.NPC_ATTACK_NPC:
				return false;
			case GameDefine.CAMP_TYPE.STATIC_NPC:
				return false;
			case GameDefine.CAMP_TYPE.FUNCTION_NPC:
				return false;
			case GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC:
				return false;
			}
			break;
		case GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC:
			switch (targetCamp)
			{
			case GameDefine.CAMP_TYPE.PLAYER_1:
				return false;
			case GameDefine.CAMP_TYPE.PLAYER_2:
				return false;
			case GameDefine.CAMP_TYPE.NORMAL_NPC:
				return false;
			case GameDefine.CAMP_TYPE.NPC_ATTACK_NPC:
				return false;
			case GameDefine.CAMP_TYPE.STATIC_NPC:
				return false;
			case GameDefine.CAMP_TYPE.FUNCTION_NPC:
				return false;
			case GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC:
				return false;
			}
			break;
		}
		return false;
	}

	public static void GetBeAttackedList(GameDefine.CAMP_TYPE selfCamp, List<int> targetList)
	{
		targetList.Clear();
		switch (selfCamp)
		{
		case GameDefine.CAMP_TYPE.PLAYER_1:
			targetList.Add(1);
			targetList.Add(2);
			targetList.Add(3);
			break;
		case GameDefine.CAMP_TYPE.PLAYER_2:
			targetList.Add(0);
			targetList.Add(2);
			targetList.Add(3);
			break;
		case GameDefine.CAMP_TYPE.NORMAL_NPC:
			targetList.Add(0);
			targetList.Add(1);
			break;
		case GameDefine.CAMP_TYPE.NPC_ATTACK_NPC:
			targetList.Add(0);
			targetList.Add(1);
			break;
		case GameDefine.CAMP_TYPE.STATIC_NPC:
			targetList.Add(0);
			targetList.Add(1);
			break;
		case GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC:
			targetList.Add(2);
			targetList.Add(3);
			break;
		}
	}

	public static bool IsFirstClass(GameDefine.CAMP_TYPE source, GameDefine.CAMP_TYPE target)
	{
		switch (source)
		{
		case GameDefine.CAMP_TYPE.NORMAL_NPC:
			if (target == GameDefine.CAMP_TYPE.PLAYER_1 || target == GameDefine.CAMP_TYPE.PLAYER_2)
			{
				return true;
			}
			break;
		case GameDefine.CAMP_TYPE.NPC_ATTACK_NPC:
			if (target == GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public static bool IsSameTeam(long id)
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.IsHaveTeam() && playerData.TeamInfo.GetTeamMemberByServerId(id) != null)
		{
			return true;
		}
		return false;
	}

	public static void TipsCanSelectAttackPlayer(ObjOtherPlayer self, ObjOtherPlayer target)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.IsInSafeArea(target.Position) || sceneManager.IsInSafeArea(self.Position) || !sceneManager.IsCanAttackOtherPlayerScene())
		{
			return;
		}
		if (self.AttributeData.PkMode == 1)
		{
			if (IsSameTeam(target.ServerId))
			{
				NoticeLogic.AddNotifyData("#{200160}");
			}
		}
		else if (self.AttributeData.PkMode == 2)
		{
			if (IsSameTeam(target.ServerId))
			{
				NoticeLogic.AddNotifyData("#{200160}");
			}
			else if (self.GuildId == target.GuildId && target.GuildId != -1)
			{
				NoticeLogic.AddNotifyData("#{200159}");
			}
		}
	}

	public static bool ISPlayerCanAttack(ObjOtherPlayer self, ObjOtherPlayer target)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager.IsInSafeArea(target.Position) || sceneManager.IsInSafeArea(self.Position))
		{
			return false;
		}
		if (!sceneManager.IsCanAttackOtherPlayerScene())
		{
			if (CanAttack(self.AttributeData.Camp, target.AttributeData.Camp))
			{
				return true;
			}
			return false;
		}
		if (self.AttributeData.PkMode == 1)
		{
			if (sceneManager.IsGuildBattleScene())
			{
				if (self.GuildId != target.GuildId || target.GuildId == -1 || self.GuildId == -1)
				{
					return true;
				}
			}
			else if (!IsSameTeam(target.ServerId))
			{
				return true;
			}
		}
		else if (self.AttributeData.PkMode == 2 && (self.GuildId != target.GuildId || target.GuildId == -1 || self.GuildId == -1))
		{
			if (sceneManager.IsGuildBattleScene())
			{
				return true;
			}
			if (!IsSameTeam(target.ServerId))
			{
				return true;
			}
		}
		return false;
	}
}
