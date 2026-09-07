using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DominInfoRootLogic : SingletonUnity<DominInfoRootLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UILabel ZoneNameLabel;

	public UILabel BuildingNameLabel;

	public GameObject ChallengesRoot;

	public UILabel PlayerNameLabel;

	public UILabel PlayerLevelLabel;

	public UILabel PlayerGuildLabel;

	public UILabel PlayerPowerLabel;

	public UILabel PlayerProfessionLabel;

	public UITexture PlayerModelPic;

	public UISprite Reward1Btn;

	public UISprite Reward2Btn;

	public GameObject Reward1GetNowRoot;

	public GameObject Reward1ClaimRoot;

	public GameObject Reward2GetNowRoot;

	public GameObject Reward2ClaimRoot;

	public UILabel Reward1TimeLabel;

	public UILabel Reward2TimeLabel;

	public UILabel GetNowTimeLabel;

	public UILabel Reward1Label;

	public UILabel Reward2Label;

	public UILabel Reward1PercentLabel;

	public UILabel Reward2PercentLabel;

	public UISprite Reward1PercentLine;

	public UISprite Reward1PercentBottomLine;

	public UISprite Reward2PercentLine;

	public UISprite Reward2PercentBottomLine;

	public ShowRewardItems RewardItems1;

	public ShowRewardItems RewardItems2;

	public UISprite PlayerIconPic;

	public UILabel PowerPercentLabel;

	public UISprite PowerPercentLine;

	public UISprite PowerPercentBottomLine;

	public TeamFakeObjPicRootLogic CurFakeObjRoot;

	public FakeObjLogic CurFakeObj;

	public GameObject WinPicRoot;

	private domin_info curDominInfo;

	private character_look curLookInfo;

	private DominData curDominData;

	private float timeCount;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			TutorialManager.OnClickTutorialBtn onClickTutorialBtn = mOnClickTutorialBtn;
			mOnClickTutorialBtn = null;
			onClickTutorialBtn();
		}
	}

	public void FlashPage()
	{
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (playerData.Domin_InfoDic.ContainsKey(curDominInfo.id))
		{
			domin_info domin_info = playerData.Domin_InfoDic[curDominInfo.id];
			if (playerData.Domin_CharacterDic.ContainsKey(domin_info.serverId))
			{
				Reset(domin_info, playerData.Domin_CharacterDic[domin_info.serverId]);
			}
			else
			{
				Reset(domin_info, null);
			}
		}
	}

	public void Reset(domin_info info, character_look chaLook)
	{
		curDominInfo = info;
		curLookInfo = chaLook;
		curDominData = DataManager.GetDominDataByID(info.id);
		CurFakeObjRoot.CreateModelPic();
		CurFakeObjRoot.EnableFakeObjRoot();
		PlayerModelPic.mainTexture = CurFakeObjRoot.ModelPic;
		UnityVersionUtil.SetActiveRecursive(PlayerModelPic.gameObject, state: true);
		ZoneNameLabel.text = StrDictionary.GetDictionaryString(curDominData.ZoneName);
		BuildingNameLabel.text = StrDictionary.GetDictionaryString(curDominData.BuildingName);
		if (curLookInfo != null)
		{
			PlayerNameLabel.text = curLookInfo.general.name;
			PlayerLevelLabel.text = "Lv." + curLookInfo.attribute_other.level;
			if (curLookInfo.attribute_other.HasGuildId)
			{
				PlayerGuildLabel.text = curLookInfo.attribute_other.guildName;
			}
			else
			{
				PlayerGuildLabel.text = "----";
			}
			PlayerPowerLabel.text = curLookInfo.attribute_other.combValue.ToString();
			PlayerProfessionLabel.text = StrDictionary.GetDictionaryString(GameDefine.ProfessionName[curLookInfo.general.profession]);
			if (!SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
			{
				if ((int)curLookInfo.general.profession == 0)
				{
					curLookInfo.visual.HeadId = GameDefine.XD_NormalModel[1];
					curLookInfo.visual.LegId = GameDefine.XD_NormalModel[3];
					curLookInfo.visual.BodyId = GameDefine.XD_NormalModel[2];
					curLookInfo.visual.WeaponId = GameDefine.XD_NormalModel[0];
				}
				else if ((int)curLookInfo.general.profession == 1)
				{
					curLookInfo.visual.HeadId = GameDefine.QJ_NormalModel[1];
					curLookInfo.visual.LegId = GameDefine.QJ_NormalModel[3];
					curLookInfo.visual.BodyId = GameDefine.QJ_NormalModel[2];
					curLookInfo.visual.WeaponId = GameDefine.QJ_NormalModel[0];
				}
				else
				{
					curLookInfo.visual.HeadId = GameDefine.NQS_NormalModel[1];
					curLookInfo.visual.LegId = GameDefine.NQS_NormalModel[3];
					curLookInfo.visual.BodyId = GameDefine.NQS_NormalModel[2];
					curLookInfo.visual.WeaponId = GameDefine.NQS_NormalModel[0];
				}
			}
			CurFakeObj.InitFakeObject(curLookInfo.visual, (int)curLookInfo.general.profession, CurFakeObjRoot.MeshRoot);
			PlayerIconPic.spriteName = GameDefine.Player_Icon_Small_Pic[curLookInfo.general.profession];
		}
		else
		{
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			PlayerNameLabel.text = playerData.MainPlayerAttrData.Name;
			PlayerLevelLabel.text = "Lv." + playerData.Level;
			PlayerGuildLabel.text = ((!playerData.IsHaveGuild()) ? "----" : playerData.PlayerGuild.GuilName);
			PlayerPowerLabel.text = playerData.MainPlayerAttrData.ComboValue.ToString();
			PlayerProfessionLabel.text = StrDictionary.GetDictionaryString(GameDefine.ProfessionName[(int)playerData.Profession]);
			CurFakeObj.InitFakeObject(playerData.PartWeaponId, playerData.PartHeadId, playerData.PartBodyId, playerData.PartLegId, playerData.Profession, CurFakeObjRoot.MeshRoot);
			PlayerIconPic.spriteName = GameDefine.Player_Icon_Small_Pic[(int)playerData.Profession];
		}
		if (curDominInfo.state == 0L)
		{
			NGUITools.SetActive(ChallengesRoot, state: true);
			Reward1Label.text = StrDictionary.GetDictionaryString("#{103008}");
			Reward1Label.color = Color.red;
			Reward2Label.text = StrDictionary.GetDictionaryString("#{103008}");
			Reward2Label.color = Color.red;
			Reward1Btn.spriteName = GameDefine.BtnIcon[2];
			Reward2Btn.spriteName = GameDefine.BtnIcon[2];
			NGUITools.SetActive(WinPicRoot, state: false);
		}
		else
		{
			NGUITools.SetActive(ChallengesRoot, state: false);
			Reward1Label.text = string.Empty;
			Reward2Label.text = string.Empty;
			Reward1Btn.spriteName = GameDefine.BtnIcon[1];
			Reward2Btn.spriteName = GameDefine.BtnIcon[1];
			NGUITools.SetActive(WinPicRoot, state: true);
		}
		List<string> list = new List<string>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		list.Add(curDominData.Resources1);
		ItemData itemDataByID = DataManager.GetItemDataByID(curDominData.Resources1);
		list2.Add(itemDataByID.Quality);
		list3.Add(curDominData.Total1);
		RewardItems1.ShowRewards(list, list2, list3);
		list.Clear();
		list.Add(curDominData.Resources2);
		itemDataByID = DataManager.GetItemDataByID(curDominData.Resources2);
		list2.Clear();
		list2.Add(itemDataByID.Quality);
		list3.Clear();
		list3.Add(curDominData.Total2);
		RewardItems2.ShowRewards(list, list2, list3);
		UpdatePercentLabel();
		SingletonUnity<NewMapUIRootLogic>.Instance.Reset("11");
		SingletonUnity<NewMapUIRootLogic>.Instance.ChooseActivityObj(curDominInfo.id, isMission: false);
	}

	private void UpdatePercentLabel()
	{
		long num = curDominInfo.res_count1;
		if (curDominInfo.state == 1)
		{
			num += Mathf.CeilToInt((float)(PlayerCommonData.GetServerTime() - curDominInfo.res_time1) / curDominData.Speed1_Second);
		}
		if (num > curDominData.Total1)
		{
			num = curDominData.Total1;
		}
		float percent = (float)num / (float)curDominData.Total1;
		SetPercentLabel(percent, Reward1PercentLabel, Reward1PercentLine, Reward1PercentBottomLine);
		if (num < curDominData.Total1)
		{
			if (UnityVersionUtil.IsActive(Reward1ClaimRoot.gameObject))
			{
				NGUITools.SetActive(Reward1ClaimRoot, state: false);
			}
			if (!UnityVersionUtil.IsActive(Reward1GetNowRoot.gameObject))
			{
				NGUITools.SetActive(Reward1GetNowRoot, state: true);
			}
			if (curDominInfo.state == 1)
			{
				float num2 = curDominInfo.res_end_time1 - PlayerCommonData.GetServerTime();
				float percent2 = 1f - Mathf.Clamp01(num2 / (float)(curDominInfo.res_end_time1 - curDominInfo.res_time1));
				SetPercentLabel(percent2, Reward1PercentLabel, Reward1PercentLine, Reward1PercentBottomLine);
				Reward1TimeLabel.text = TimeTools.GetHourMinSecStr((int)num2);
			}
			else
			{
				float num3 = (float)(curDominData.Total1 - num) * curDominData.Speed1_Second;
				Reward1TimeLabel.text = TimeTools.GetHourMinSecStr((int)num3);
			}
		}
		else
		{
			if (!UnityVersionUtil.IsActive(Reward1ClaimRoot.gameObject))
			{
				NGUITools.SetActive(Reward1ClaimRoot, state: true);
			}
			if (UnityVersionUtil.IsActive(Reward1GetNowRoot.gameObject))
			{
				NGUITools.SetActive(Reward1GetNowRoot, state: false);
			}
		}
		long num4 = curDominInfo.res_count2;
		if (curDominInfo.state == 1)
		{
			num4 += Mathf.CeilToInt((float)(PlayerCommonData.GetServerTime() - curDominInfo.res_time2) / curDominData.Speed2_Second);
		}
		if (num4 > curDominData.Total2)
		{
			num4 = curDominData.Total2;
		}
		float percent3 = (float)num4 / (float)curDominData.Total2;
		SetPercentLabel(percent3, Reward2PercentLabel, Reward2PercentLine, Reward2PercentBottomLine);
		if (num4 < curDominData.Total2)
		{
			if (UnityVersionUtil.IsActive(Reward2ClaimRoot.gameObject))
			{
				NGUITools.SetActive(Reward2ClaimRoot, state: false);
			}
			if (!UnityVersionUtil.IsActive(Reward2GetNowRoot.gameObject))
			{
				NGUITools.SetActive(Reward2GetNowRoot, state: true);
			}
			if (curDominInfo.state == 1)
			{
				float num5 = curDominInfo.res_end_time2 - PlayerCommonData.GetServerTime();
				float percent4 = 1f - Mathf.Clamp01(num5 / (float)(curDominInfo.res_end_time2 - curDominInfo.res_time2));
				SetPercentLabel(percent4, Reward2PercentLabel, Reward2PercentLine, Reward2PercentBottomLine);
				Reward2TimeLabel.text = TimeTools.GetHourMinSecStr((int)num5);
			}
			else
			{
				float num6 = (float)(curDominData.Total2 - num4) * curDominData.Speed2_Second;
				Reward2TimeLabel.text = TimeTools.GetHourMinSecStr((int)num6);
			}
		}
		else
		{
			if (!UnityVersionUtil.IsActive(Reward2ClaimRoot.gameObject))
			{
				NGUITools.SetActive(Reward2ClaimRoot, state: true);
			}
			if (UnityVersionUtil.IsActive(Reward2GetNowRoot.gameObject))
			{
				NGUITools.SetActive(Reward2GetNowRoot, state: false);
			}
		}
		if (curDominInfo.state == 0L)
		{
			SetPercentLabel(0f, PowerPercentLabel, PowerPercentLine, PowerPercentBottomLine);
			return;
		}
		float num7 = curDominInfo.end_time - PlayerCommonData.GetServerTime();
		if (num7 < 0f)
		{
			num7 = 0f;
		}
		float num8 = 1f - (float)(PlayerCommonData.GetServerTime() - curDominInfo.donmin_time) / (float)(curDominInfo.end_time - curDominInfo.donmin_time);
		SetPercentLabel(num8, PowerPercentLabel, PowerPercentLine, PowerPercentBottomLine);
		PowerPercentLabel.text = $"{(int)(num8 * 100f)}%({TimeTools.GetHourMinSecStr((int)num7)})";
	}

	private void SetPercentLabel(float percent, UILabel label, UISprite topPic, UISprite bottomPic)
	{
		if (percent > 1f)
		{
			percent = 1f;
		}
		if (percent > float.Epsilon)
		{
			if (!topPic.enabled)
			{
				topPic.enabled = true;
			}
			topPic.width = (int)((float)bottomPic.width * percent);
		}
		else
		{
			topPic.enabled = false;
		}
		label.text = $"{(int)(percent * 100f)}%";
	}

	private void Update()
	{
		if (curDominInfo != null && curDominInfo.state != 0L)
		{
			timeCount += Time.deltaTime;
			if (timeCount > 1f)
			{
				timeCount = 0f;
				UpdatePercentLabel();
			}
		}
	}

	private void OnDisable()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.CAPTURE_CLICK_START)
		{
			CheckTutorialEvent();
		}
		UnLoadFakeObj();
	}

	public void UnLoadFakeObj()
	{
		if (CurFakeObj != null)
		{
			CurFakeObj.DestroyFakeObj();
			CurFakeObj = null;
		}
		else
		{
			Debug.Log("mPlayerModelVisual == null");
		}
		if (CurFakeObjRoot != null)
		{
			CurFakeObjRoot.DisableFakeObjRoot();
		}
	}

	public void OnClickChallengeBtn()
	{
		if (TutorialManager.CurStep == TUTORIAL_STEP.CAPTURE_CLICK_START)
		{
			CheckTutorialEvent();
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.IsFinishDownload)
		{
			if (curDominInfo != null && curDominInfo.state != 1)
			{
				enter_domin_pk_scene.request request = new enter_domin_pk_scene.request();
				request.id = curDominInfo.id;
				NetLogic.GetInstance().Send<Protocol.enter_domin_pk_scene>(request);
			}
		}
		else
		{
			SingletonUnity<UIManager>.Instance.CloseAllPOPUI();
			SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DownLoadResRoot);
		}
	}

	public void OnClickReward1Btn()
	{
		if (curDominInfo.state != 1)
		{
			return;
		}
		if (UnityVersionUtil.IsActive(Reward1ClaimRoot))
		{
			require_domin_rewards.request request = new require_domin_rewards.request();
			request.id = curDominInfo.id;
			request.index = 0L;
			request.cost = false;
			NetLogic.GetInstance().Send<Protocol.require_domin_rewards>(request);
			curDominInfo.res_time1 = PlayerCommonData.GetServerTime();
			curDominInfo.res_count1 = 0L;
			return;
		}
		long num = curDominData.Total1 - Mathf.FloorToInt((float)(PlayerCommonData.GetServerTime() - curDominInfo.res_time1) / curDominData.Speed1_Second) - curDominInfo.res_count1;
		long costVal = Mathf.CeilToInt((float)(num * curDominData.Price1) / 10000f);
		ShowItemsRootLogic.ShowYestOrNoBtn(DataManager.GetItemDataByID(curDominData.Resources1), curDominData.Total1, "#{100127}", StrDictionary.GetDictionaryString("#{103018}", GameMoneyHelper.GetMoneyValStr((int)costVal, curDominData.PriceType1)), delegate
		{
			if (GameMoneyHelper.BeforeCheckBuyTop(curDominData.PriceType1, (int)costVal))
			{
				require_domin_rewards.request rpcReq = new require_domin_rewards.request
				{
					id = curDominInfo.id,
					index = 0L,
					cost = true
				};
				NetLogic.GetInstance().Send<Protocol.require_domin_rewards>(rpcReq);
				curDominInfo.res_time1 = PlayerCommonData.GetServerTime();
				curDominInfo.res_count1 = 0L;
			}
		}, null);
	}

	public void OnClickReward2Btn()
	{
		if (curDominInfo.state != 1)
		{
			return;
		}
		if (UnityVersionUtil.IsActive(Reward2ClaimRoot))
		{
			require_domin_rewards.request request = new require_domin_rewards.request();
			request.id = curDominInfo.id;
			request.index = 1L;
			request.cost = false;
			NetLogic.GetInstance().Send<Protocol.require_domin_rewards>(request);
			curDominInfo.res_time2 = PlayerCommonData.GetServerTime();
			curDominInfo.res_count2 = 0L;
			return;
		}
		long num = curDominData.Total2 - Mathf.FloorToInt((float)(PlayerCommonData.GetServerTime() - curDominInfo.res_time2) / curDominData.Speed2_Second) - curDominInfo.res_count2;
		long costVal = Mathf.CeilToInt((float)(num * curDominData.Price2) / 10000f);
		ShowItemsRootLogic.ShowYestOrNoBtn(DataManager.GetItemDataByID(curDominData.Resources2), curDominData.Total2, "#{100127}", StrDictionary.GetDictionaryString("#{103018}", GameMoneyHelper.GetMoneyValStr((int)costVal, curDominData.PriceType2)), delegate
		{
			if (GameMoneyHelper.BeforeCheckBuyTop(curDominData.PriceType2, (int)costVal))
			{
				require_domin_rewards.request rpcReq = new require_domin_rewards.request
				{
					id = curDominInfo.id,
					index = 1L,
					cost = true
				};
				NetLogic.GetInstance().Send<Protocol.require_domin_rewards>(rpcReq);
				curDominInfo.res_time2 = PlayerCommonData.GetServerTime();
				curDominInfo.res_count2 = 0L;
			}
		}, null);
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.DominInfoRoot);
		SingletonUnity<UIManager>.Instance.ShowUI(UIInfo.DominPageRoot, delegate
		{
			SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.FlashDominRootPage();
		});
	}
}
