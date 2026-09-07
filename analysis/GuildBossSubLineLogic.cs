using SprotoType;
using UnityEngine;

public class GuildBossSubLineLogic : MonoBehaviour
{
	public UILabel NameLabel;

	public UISprite IconSprite;

	public UILabel LevelLabel;

	public UISprite SelectBkSprite;

	public UISprite CompleteSprite;

	public DelegateDefine.ThirdIntParamDelegate onClickItem;

	public GameObject SelectUpSprite;

	public int enableLineFlag;

	public int curIndex = -1;

	public int parentIndex = -1;

	public void ResetItem(int pIndex, int infoindex, guild_boss info, DelegateDefine.ThirdIntParamDelegate clickFunc)
	{
		curIndex = infoindex;
		parentIndex = pIndex;
		GuildBossData guildBossDataByID = DataManager.GetGuildBossDataByID(info.id);
		NpcData npcDataByID = DataManager.GetNpcDataByID(guildBossDataByID.BossID);
		Guild playerGuild = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild;
		enableLineFlag = 0;
		IconSprite.spriteName = guildBossDataByID.Icon;
		NameLabel.text = StrDictionary.GetDictionaryString(npcDataByID.Name);
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		int key = (infoindex + TimeTools.GetOffsetDay(guildBossDataByID.StartTime, playerCommonData.TimeOffset) + GameDefine.WEEK_NAME.Count) % GameDefine.WEEK_NAME.Count;
		LevelLabel.text = StrDictionary.GetDictionaryString(GameDefine.WEEK_NAME[key]);
		if (info.state == 2)
		{
			CompleteSprite.enabled = true;
		}
		else
		{
			CompleteSprite.enabled = false;
		}
		if (info.state != 1)
		{
			enableLineFlag = 2;
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.GuilLevel < guildBossDataByID.LevelMin)
		{
			enableLineFlag = 1;
		}
		RefreshSelect(-1);
		onClickItem = clickFunc;
	}

	public bool CheckGuildLevel(int minLevel, int maxlevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.PlayerGuild.CheckGuildLevel(minLevel, maxlevel);
	}

	public void OnClikcItemBtn()
	{
		if (onClickItem != null)
		{
			onClickItem(parentIndex, curIndex, enableLineFlag);
		}
	}

	private void SetLabelWarining(UILabel label, bool needWarning)
	{
		if (needWarning)
		{
			label.color = Color.red;
		}
		else
		{
			label.color = Color.white;
		}
	}

	public void RefreshSelect(int index)
	{
		if (enableLineFlag == 0)
		{
			SelectBkSprite.spriteName = "CZ_wuPinYanSe_3";
		}
		else
		{
			SelectBkSprite.spriteName = "CZ_tongYongDi_zhuYao_4_1";
		}
		if (index != curIndex)
		{
			UnityVersionUtil.SetActiveRecursive(SelectUpSprite, state: false);
		}
		else
		{
			UnityVersionUtil.SetActiveRecursive(SelectUpSprite, state: true);
		}
	}
}
