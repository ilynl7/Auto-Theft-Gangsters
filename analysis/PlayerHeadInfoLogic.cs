using UnityEngine;

public class PlayerHeadInfoLogic : HeadInfoLogic
{
	public UILabel GuildLabel;

	public UISprite TitlePic;

	public UISprite TitlePic2;

	public UISprite ChampionGuildPic;

	public UISprite CampPic;

	private string mGuildName = string.Empty;

	private int mTitleLevel = -1;

	private bool mShowHpLine;

	private float curValue = -1f;

	private bool mIsChampionGuild;

	private float noGuildHeight = 25f;

	private float hasGuildHeight = 50f;

	private float championGuildHeight = 70f;

	private GameDefine.CAMP_TYPE mCamp;

	private float lastChangeTime;

	public override void Init()
	{
		if (mHpLineLogic != null)
		{
			NGUITools.SetActive(mHpLineLogic.gameObject, state: false);
		}
	}

	public void Refresh(int titleLevel, string name, string guildName, bool isChampion)
	{
		mIsChampionGuild = isChampion;
		if (guildName != mGuildName)
		{
			if (string.IsNullOrEmpty(guildName))
			{
				GuildLabel.text = string.Empty;
				mIsChampionGuild = false;
			}
			else
			{
				GuildLabel.text = $"<{guildName}>";
			}
			mGuildName = guildName;
		}
		if (mTitleLevel != titleLevel)
		{
			ChangePic(titleLevel);
			mTitleLevel = titleLevel;
		}
		UpdateChampionGuildPic(mIsChampionGuild);
		UpdateCampPic();
	}

	public void Reset(bool isMainPlayer, int titleLevel, string name, string guildName, GameDefine.CAMP_TYPE camp, bool ShowHpLine = false, bool isChampionGuild = false)
	{
		mCamp = camp;
		curValue = -1f;
		NameLabel.text = name;
		mIsChampionGuild = isChampionGuild;
		if (string.IsNullOrEmpty(guildName))
		{
			GuildLabel.text = string.Empty;
			mIsChampionGuild = false;
		}
		else
		{
			GuildLabel.text = $"<{guildName}>";
		}
		mGuildName = guildName;
		if (isMainPlayer)
		{
			NameLabel.color = Color.white;
		}
		else
		{
			NameLabel.color = Color.green;
		}
		ChangePic(titleLevel);
		mTitleLevel = titleLevel;
		mShowHpLine = ShowHpLine;
		if (mHpLineLogic != null)
		{
			NGUITools.SetActive(mHpLineLogic.gameObject, state: false);
		}
		UpdateChampionGuildPic(mIsChampionGuild);
		UpdateCampPic();
	}

	public void UpdateName(string name)
	{
		NameLabel.text = name;
	}

	public void UpdateChampionGuildPic(bool isChampion)
	{
		if (UnityVersionUtil.IsActive(base.gameObject))
		{
			if (isChampion)
			{
				NGUITools.SetActive(ChampionGuildPic.gameObject, state: true);
				ChampionGuildPic.width = GuildLabel.width + 110;
			}
			else
			{
				NGUITools.SetActive(ChampionGuildPic.gameObject, state: false);
			}
		}
		else
		{
			NGUITools.SetActive(ChampionGuildPic.gameObject, state: false);
		}
	}

	private void UpdateCampPic()
	{
		MapInfoData currentMapInofData = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.CurrentMapInofData;
		if (currentMapInofData.MapType == MAPTYPE.SURVIVE_BATTLE2 || currentMapInofData.MapType == MAPTYPE.GUILD_BATTLE)
		{
			NGUITools.SetActive(CampPic.gameObject, state: true);
			if (string.IsNullOrEmpty(mGuildName))
			{
				CampPic.transform.localPosition = Vector3.up * noGuildHeight;
			}
			else if (mIsChampionGuild)
			{
				CampPic.transform.localPosition = Vector3.up * championGuildHeight;
			}
			else
			{
				CampPic.transform.localPosition = Vector3.up * hasGuildHeight;
			}
			if (currentMapInofData.MapType == MAPTYPE.GUILD_BATTLE)
			{
				PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
				if (playerData.PlayerGuild == null)
				{
					return;
				}
				if (mGuildName.Equals(playerData.PlayerGuild.GuilName))
				{
					if (playerData.IsGuildBattleRedTeam)
					{
						CampPic.color = Color.red;
					}
					else
					{
						CampPic.color = Color.blue;
					}
				}
				else if (playerData.IsGuildBattleRedTeam)
				{
					CampPic.color = Color.blue;
				}
				else
				{
					CampPic.color = Color.red;
				}
			}
			else if (mCamp == GameDefine.CAMP_TYPE.PLAYER_1)
			{
				CampPic.color = Color.green;
			}
			else
			{
				CampPic.color = Color.red;
			}
		}
		else
		{
			NGUITools.SetActive(CampPic.gameObject, state: false);
		}
	}

	public void ChangePic(int titleLevel)
	{
		if (titleLevel > 0)
		{
			TitlePic.spriteName = GameDefine.TianTi_TuBiao[titleLevel - 1];
			TitlePic.enabled = true;
			TitlePic2.enabled = true;
			UISpriteData atlasSprite = TitlePic.GetAtlasSprite();
			TitlePic.width = atlasSprite.width / 2;
			TitlePic.height = atlasSprite.height / 2;
			TitlePic.transform.localPosition = -Vector3.right * (NameLabel.width / 2 + 20);
			TitlePic2.spriteName = TitlePic.spriteName;
			TitlePic2.width = TitlePic.width;
			TitlePic2.height = TitlePic.height;
			TitlePic2.transform.localPosition = Vector3.right * (NameLabel.width / 2 + 20);
		}
		else
		{
			TitlePic.enabled = false;
			TitlePic2.enabled = false;
		}
	}

	public override void SetHpVal(float val)
	{
		if (!mShowHpLine || !(Mathf.Abs(val - curValue) > float.Epsilon))
		{
			return;
		}
		if (Mathf.Abs(curValue + 1f) > float.Epsilon)
		{
			if (mShowHpLine)
			{
				if (mHpLineLogic != null)
				{
					ShowHpLine();
					if (mHpLineLogic != null)
					{
						mHpLineLogic.ChangeVal(val);
					}
				}
			}
			else if (mHpLineLogic != null)
			{
				NGUITools.SetActive(mHpLineLogic.gameObject, state: false);
			}
		}
		curValue = val;
	}

	public void ShowHpLine()
	{
		NGUITools.SetActive(mHpLineLogic.gameObject, state: true);
		lastChangeTime = Time.time;
	}

	public void HideHpLine()
	{
		NGUITools.SetActive(mHpLineLogic.gameObject, state: false);
	}

	private void Update()
	{
		if (mShowHpLine && UnityVersionUtil.IsActive(mHpLineLogic.gameObject) && Time.time - lastChangeTime > GameDefine.NPC_HP_LINE_SHOW_TIME)
		{
			HideHpLine();
		}
	}
}
