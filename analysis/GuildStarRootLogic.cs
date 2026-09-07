using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class GuildStarRootLogic : SingletonUnity<GuildStarRootLogic>
{
	public UILabel MapNameLabel;

	public UILabel CoinLabel;

	public UITexture MapBgTexture;

	public UITexture MapXingpanDi;

	public UILabel NeedLevelLabel;

	public UISprite AttSp1;

	public UILabel Attname1;

	public UILabel AttInfo1;

	public UISprite AttSp2;

	public UILabel Attname2;

	public UILabel AttInfo2;

	public UISprite Cost1quaSp;

	public UISprite Cost1Sp;

	public UILabel Cost1Label;

	public UISprite Cost2quaSp;

	public UISprite Cost2Sp;

	public UILabel Cost2Label;

	public UILabel CostNameLabel;

	public UISprite CompleteFlag;

	public UISprite upgradeBtn;

	private List<guild_star> GuildstarsInfo = new List<guild_star>();

	private Dictionary<string, guild_star> GuildstarsDic = new Dictionary<string, guild_star>();

	private guild_star CurStarInfo;

	private GuildStarData CurStarData;

	private bool CurMapIsComplete;

	private List<string> textureList = new List<string>();

	public TweenPosition MainAnima;

	public TweenPosition TempAnima;

	public GuildStarMapLogic MainPage;

	public GuildStarMapLogic TempPage;

	public UISprite LeftSp;

	public UISprite RightSp;

	private int CurShowIndex;

	private int MaxStarMapNum = 12;

	private Vector3 LeftPos = new Vector3(-400f, 0f, 0f);

	private Vector3 rightPos = new Vector3(400f, 0f, 0f);

	private float MoveTime = 0.5f;

	private int NeedMapIndex;

	public void EnableReset()
	{
		CurShowIndex = 1;
		CoinLabel.text = GameMoneyHelper.GetGuildContribute().ToString();
		InitTexture();
		CompleteFlag.enabled = false;
		NGUITools.SetActive(MainPage.gameObject, state: false);
		NGUITools.SetActive(TempPage.gameObject, state: false);
		NGUITools.SetActive(Cost1quaSp.gameObject, state: false);
		NGUITools.SetActive(Cost2quaSp.gameObject, state: false);
	}

	public void InitTexture()
	{
		List<string> list = new List<string>();
		if (MapBgTexture.mainTexture == null)
		{
			list.Add(GameDefine.GuildStarBG);
		}
		if (MapXingpanDi.mainTexture == null)
		{
			list.Add(GameDefine.GuildXingpanDi);
		}
		if (list.Count != 0 && UnityVersionUtil.IsactiveInHierarchy(base.gameObject))
		{
			StartCoroutine(BundleManager.LoadTexture(list, TextureLoadFinish));
		}
	}

	private void TextureLoadFinish(Dictionary<string, Texture> retdic)
	{
		if (retdic != null && retdic.Count != 0)
		{
			if (retdic.ContainsKey(GameDefine.GuildStarBG))
			{
				MapBgTexture.mainTexture = retdic[GameDefine.GuildStarBG];
			}
			if (retdic.ContainsKey(GameDefine.GuildXingpanDi))
			{
				MapXingpanDi.mainTexture = retdic[GameDefine.GuildXingpanDi];
			}
		}
	}

	public void Reset(ret_guild_star.request request)
	{
		GuildstarsInfo.Clear();
		GuildstarsDic.Clear();
		if (request.HasGuild_stars)
		{
			GuildstarsDic = request.guild_stars;
			GuildstarsInfo = new List<guild_star>(request.guild_stars.Values);
			GuildstarsInfo.Sort((guild_star x, guild_star y) => (x.ID.Length != y.ID.Length) ? (x.ID.Length - y.ID.Length) : x.ID.CompareTo(y.ID));
		}
		for (int i = 0; i < GuildstarsInfo.Count; i++)
		{
			GuildStarData guildStarDataById = DataManager.GetGuildStarDataById(GuildstarsInfo[i].ID);
			CurShowIndex = guildStarDataById.MapID;
			if (GuildstarsInfo[i].HasState && GuildstarsInfo[i].state == 0L)
			{
				break;
			}
		}
		NeedMapIndex = CurShowIndex;
		GetCurShowData(CurShowIndex);
		NGUITools.SetActive(MainPage.gameObject, state: true);
		NGUITools.SetActive(TempPage.gameObject, state: false);
		MainPage.UpdateInfo(GuildstarsDic, CurShowIndex);
		SetDirBtn();
		refershRightInfo();
	}

	public void UpdateInfo(ret_update_guild_star.request request)
	{
		GuildstarsDic.Clear();
		GuildstarsInfo.Clear();
		if (request.HasGuild_stars)
		{
			GuildstarsDic = request.guild_stars;
			GuildstarsInfo = new List<guild_star>(request.guild_stars.Values);
			GuildstarsInfo.Sort((guild_star x, guild_star y) => (x.ID.Length != y.ID.Length) ? (x.ID.Length - y.ID.Length) : x.ID.CompareTo(y.ID));
		}
		for (int i = 0; i < GuildstarsInfo.Count; i++)
		{
			GuildStarData guildStarDataById = DataManager.GetGuildStarDataById(GuildstarsInfo[i].ID);
			CurShowIndex = guildStarDataById.MapID;
			if (GuildstarsInfo[i].HasState && GuildstarsInfo[i].state == 0L)
			{
				break;
			}
		}
		NeedMapIndex = CurShowIndex;
		GetCurShowData(CurShowIndex);
		NGUITools.SetActive(MainPage.gameObject, state: true);
		NGUITools.SetActive(TempPage.gameObject, state: false);
		MainPage.UpdateInfo(GuildstarsDic, CurShowIndex);
		SetDirBtn();
		refershRightInfo();
	}

	public void refershRightInfo()
	{
		if (CurStarData != null)
		{
			AttSp1.spriteName = GameDefine.GetAttributeIcon(CurStarData.Status1);
			Attname1.text = GameDefine.GetAttributeName_S(CurStarData.Status1);
			AttInfo1.text = $"+{CurStarData.Value1}";
			AttSp2.spriteName = GameDefine.GetAttributeIcon(CurStarData.Status2);
			Attname2.text = GameDefine.GetAttributeName_S(CurStarData.Status2);
			AttInfo2.text = $"+{CurStarData.Value2}";
			NeedLevelLabel.text = StrDictionary.GetDictionaryString("#{106026}", CurStarData.UnlockLevel);
			if (CurMapIsComplete)
			{
				MapNameLabel.text = string.Format("{0} Lv.{1}", StrDictionary.GetDictionaryString(CurStarData.MapName), StrDictionary.GetDictionaryString("#{106002}"));
				NGUITools.SetActive(Cost1quaSp.gameObject, state: false);
				NGUITools.SetActive(Cost2quaSp.gameObject, state: false);
				CostNameLabel.enabled = false;
				CompleteFlag.enabled = true;
				upgradeBtn.spriteName = GameDefine.BtnIcon[2];
				return;
			}
			MapNameLabel.text = $"{StrDictionary.GetDictionaryString(CurStarData.MapName)} Lv.{CurStarData.Lv - 1}";
			CostNameLabel.enabled = true;
			CompleteFlag.enabled = false;
			if (!string.IsNullOrEmpty(CurStarData.CostID1))
			{
				ItemData itemDataByID = DataManager.GetItemDataByID(CurStarData.CostID1);
				Cost1quaSp.spriteName = itemDataByID.QualityType.ToString();
				Cost1Sp.spriteName = itemDataByID.BackPackIcon;
				Cost1Label.text = CurStarData.Cost1.ToString();
				if (CurStarData.Cost1 <= GameMoneyHelper.GetMoneyNum((int)GameDefine.ITEM_ID_MONEYTYPR[CurStarData.CostID1]))
				{
					Cost1Label.color = Color.white;
				}
				else
				{
					Cost1Label.color = Color.red;
				}
				NGUITools.SetActive(Cost1quaSp.gameObject, state: true);
			}
			else
			{
				NGUITools.SetActive(Cost1quaSp.gameObject, state: false);
			}
			if (!string.IsNullOrEmpty(CurStarData.CostID2))
			{
				ItemData itemDataByID2 = DataManager.GetItemDataByID(CurStarData.CostID2);
				Cost2quaSp.spriteName = itemDataByID2.QualityType.ToString();
				Cost2Sp.spriteName = itemDataByID2.BackPackIcon;
				Cost2Label.text = CurStarData.Cost2.ToString();
				if (CurStarData.Cost2 <= GameMoneyHelper.GetMoneyNum((int)GameDefine.ITEM_ID_MONEYTYPR[CurStarData.CostID2]))
				{
					Cost2Label.color = Color.white;
				}
				else
				{
					Cost2Label.color = Color.red;
				}
				NGUITools.SetActive(Cost2quaSp.gameObject, state: true);
			}
			else
			{
				NGUITools.SetActive(Cost2quaSp.gameObject, state: false);
			}
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			if (playerData.PlayerGuild.GuilLevel >= CurStarData.UnlockLevel && NeedMapIndex == CurStarData.MapID)
			{
				upgradeBtn.spriteName = GameDefine.BtnIcon[1];
			}
			else
			{
				upgradeBtn.spriteName = GameDefine.BtnIcon[2];
			}
		}
		else
		{
			CostNameLabel.enabled = false;
			NGUITools.SetActive(Cost1quaSp.gameObject, state: false);
			NGUITools.SetActive(Cost2quaSp.gameObject, state: false);
			CompleteFlag.enabled = false;
		}
	}

	public void OnClickLeftBtn()
	{
		if (CurShowIndex > 1)
		{
			CurShowIndex--;
			MainAnima.from = MainAnima.transform.localPosition;
			MainAnima.to = rightPos;
			MainAnima.duration = MoveTime;
			MainAnima.ResetToBeginning();
			TempAnima.from = LeftPos;
			TempAnima.to = Vector3.zero;
			TempAnima.duration = MoveTime;
			TempAnima.ResetToBeginning();
			MainAnima.PlayForward();
			TempAnima.PlayForward();
			NGUITools.SetActive(TempPage.gameObject, state: true);
			TempPage.UpdateInfo(GuildstarsDic, CurShowIndex);
			GetCurShowData(CurShowIndex);
			SetDirBtn();
			SwapPage();
			refershRightInfo();
		}
	}

	public void OnClickRightBtn()
	{
		if (CurShowIndex < MaxStarMapNum)
		{
			CurShowIndex++;
			MainAnima.from = MainAnima.transform.localPosition;
			MainAnima.to = LeftPos;
			MainAnima.duration = MoveTime;
			MainAnima.ResetToBeginning();
			TempAnima.from = rightPos;
			TempAnima.to = Vector3.zero;
			TempAnima.duration = MoveTime;
			TempAnima.ResetToBeginning();
			MainAnima.PlayForward();
			TempAnima.PlayForward();
			NGUITools.SetActive(TempPage.gameObject, state: true);
			TempPage.UpdateInfo(GuildstarsDic, CurShowIndex);
			GetCurShowData(CurShowIndex);
			SetDirBtn();
			SwapPage();
			refershRightInfo();
		}
	}

	public void OnClickUpgrade()
	{
		if (CurStarData == null)
		{
			return;
		}
		if (CurMapIsComplete)
		{
			MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{106018}"), "#{100127}");
			return;
		}
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.PlayerGuild.GuilLevel < CurStarData.UnlockLevel)
		{
			MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{106016}", CurStarData.UnlockLevel), "#{100127}");
		}
		else if (NeedMapIndex != CurStarData.MapID)
		{
			MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{106017}"), "#{100127}");
		}
		else if ((string.IsNullOrEmpty(CurStarData.CostID1) || GameMoneyHelper.BeforeCheckBuy(GameDefine.ITEM_ID_MONEYTYPR[CurStarData.CostID1], CurStarData.Cost1)) && (string.IsNullOrEmpty(CurStarData.CostID2) || GameMoneyHelper.BeforeCheckBuy(GameDefine.ITEM_ID_MONEYTYPR[CurStarData.CostID2], CurStarData.Cost2)))
		{
			WaitResponseUIRootLogic.OpenWaitBox(294, 10f, 0f);
			update_guild_star.request request = new update_guild_star.request();
			request.ID = CurStarData.ID;
			NetLogic.GetInstance().Send<Protocol.update_guild_star>(request);
		}
	}

	public void OnClickCost1Btn()
	{
		if (CurStarData != null && !string.IsNullOrEmpty(CurStarData.CostID1))
		{
			ItemData itemDataByID = DataManager.GetItemDataByID(CurStarData.CostID1);
			if (itemDataByID != null)
			{
				ItemInfoRootLogicNew.ShowItemTips(itemDataByID);
			}
		}
	}

	public void OnClickCost2Btn()
	{
		if (CurStarData != null && !string.IsNullOrEmpty(CurStarData.CostID2))
		{
			ItemData itemDataByID = DataManager.GetItemDataByID(CurStarData.CostID2);
			if (itemDataByID != null)
			{
				ItemInfoRootLogicNew.ShowItemTips(itemDataByID);
			}
		}
	}

	public void UpdateMoneyLabel()
	{
		CoinLabel.text = GameMoneyHelper.GetGuildContribute().ToString();
	}

	public void OnClickTishi()
	{
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.ExcInfoRoot, delegate
		{
			SingletonUnity<ExcInfoRootLogic>.Instance.Reset("#{101533}", "#{106027}", null);
		});
	}

	public void SwapPage()
	{
		TweenPosition mainAnima = MainAnima;
		MainAnima = TempAnima;
		TempAnima = mainAnima;
		GuildStarMapLogic mainPage = MainPage;
		MainPage = TempPage;
		TempPage = mainPage;
	}

	public void SetDirBtn()
	{
		if (CurShowIndex == 1)
		{
			NGUITools.SetActive(LeftSp.gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(LeftSp.gameObject, state: true);
		}
		if (CurShowIndex == MaxStarMapNum)
		{
			NGUITools.SetActive(RightSp.gameObject, state: false);
		}
		else
		{
			NGUITools.SetActive(RightSp.gameObject, state: true);
		}
	}

	public void GetCurShowData(int curMapID)
	{
		CurStarInfo = null;
		CurStarData = null;
		bool flag = false;
		for (int num = GuildstarsInfo.Count - 1; num >= 0; num--)
		{
			GuildStarData guildStarDataById = DataManager.GetGuildStarDataById(GuildstarsInfo[num].ID);
			if (guildStarDataById != null && guildStarDataById.MapID == curMapID)
			{
				if (!flag)
				{
					flag = true;
					CurMapIsComplete = true;
					CurStarInfo = GuildstarsInfo[num];
					CurStarData = guildStarDataById;
				}
				if (GuildstarsInfo[num].HasState && GuildstarsInfo[num].state == 0L)
				{
					CurMapIsComplete = false;
					CurStarInfo = GuildstarsInfo[num];
					CurStarData = guildStarDataById;
				}
			}
		}
	}
}
