using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class DominLineLogic : MonoBehaviour
{
	public UISprite PlayerIcon;

	public UISprite LockIcon;

	public UILabel ZoneLabel;

	public UILabel PlayerNameLabel;

	public UILabel PercentLabel;

	public UISprite PercentLinePic;

	public UISprite PercentLineBottom;

	public ShowRewardItems ShowReward;

	public UIWidget AloneSP;

	public UIWidget TeamSp;

	public UIWidget GuildSp;

	public GameObject WinPicRoot;

	public UILabel LevelLabel;

	private domin_info curInfo;

	private DominData curData;

	private DelegateDefine.OneStringParamDelegate onClickItem;

	private float updateTimeCount;

	public void Reset(domin_info info, character_look cha, DelegateDefine.OneStringParamDelegate func)
	{
		curInfo = info;
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		curData = DataManager.GetDominDataByID(info.id);
		ZoneLabel.text = StrDictionary.GetDictionaryString(curData.ZoneName);
		if (curData.Entrytype == 0)
		{
			NGUITools.SetActive(AloneSP.gameObject, state: true);
			NGUITools.SetActive(TeamSp.gameObject, state: false);
			NGUITools.SetActive(GuildSp.gameObject, state: false);
		}
		else if (curData.Entrytype == 2 || curData.Entrytype == 1)
		{
			NGUITools.SetActive(AloneSP.gameObject, state: false);
			NGUITools.SetActive(TeamSp.gameObject, state: true);
			NGUITools.SetActive(GuildSp.gameObject, state: false);
		}
		else if (curData.Entrytype == 3)
		{
			NGUITools.SetActive(AloneSP.gameObject, state: false);
			NGUITools.SetActive(TeamSp.gameObject, state: false);
			NGUITools.SetActive(GuildSp.gameObject, state: true);
		}
		else
		{
			NGUITools.SetActive(AloneSP.gameObject, state: false);
			NGUITools.SetActive(TeamSp.gameObject, state: false);
			NGUITools.SetActive(GuildSp.gameObject, state: false);
		}
		if (curData.IsOpen == 0)
		{
			NGUITools.SetActive(LockIcon.gameObject, state: true);
			PlayerNameLabel.text = StrDictionary.GetDictionaryString("#{103024}");
			PlayerNameLabel.color = Color.red;
			NGUITools.SetActive(WinPicRoot, state: false);
			LevelLabel.text = string.Empty;
		}
		else if (playerData.Level < curData.LevelMin)
		{
			NGUITools.SetActive(LockIcon.gameObject, state: true);
			PlayerNameLabel.text = $"Lv.{curData.LevelMin}";
			PlayerNameLabel.color = Color.red;
			NGUITools.SetActive(WinPicRoot, state: false);
			LevelLabel.text = string.Empty;
		}
		else
		{
			NGUITools.SetActive(LockIcon.gameObject, state: false);
			PlayerNameLabel.color = Color.white;
			if (curInfo.state == 0L)
			{
				NGUITools.SetActive(WinPicRoot, state: false);
				if (cha != null)
				{
					PlayerIcon.spriteName = GameDefine.Game_Player_Icon_pic[cha.general.profession];
					PlayerNameLabel.text = cha.general.name;
					LevelLabel.text = "Lv." + cha.attribute_other.level;
				}
				else
				{
					Debug.Log("No Character Info");
				}
			}
			else
			{
				NGUITools.SetActive(WinPicRoot, state: true);
				PlayerIcon.spriteName = GameDefine.Game_Player_Icon_pic[(int)playerData.Profession];
				PlayerNameLabel.text = StrDictionary.GetDictionaryString("#{103004}");
				LevelLabel.text = "Lv." + playerData.Level;
			}
		}
		onClickItem = func;
		List<string> list = new List<string>();
		list.Add(curData.Resources1);
		list.Add(curData.Resources2);
		List<int> list2 = new List<int>();
		ItemData itemDataByID = DataManager.GetItemDataByID(curData.Resources1);
		list2.Add(itemDataByID.Quality);
		ItemData itemDataByID2 = DataManager.GetItemDataByID(curData.Resources2);
		list2.Add(itemDataByID2.Quality);
		List<int> list3 = new List<int>();
		list3.Add(0);
		list3.Add(0);
		ShowReward.ShowRewards(list, list2, list3);
		UpdatePercent();
	}

	public void UpdatePercent()
	{
		if (curInfo.state == 1)
		{
			long serverTime = PlayerCommonData.GetServerTime();
			long num = serverTime - curInfo.donmin_time;
			long num2 = curInfo.end_time - curInfo.donmin_time;
			float picLine = 1f - (float)num / (float)num2;
			SetPicLine(picLine);
		}
		else
		{
			SetPicLine(0f);
		}
	}

	public void SetPicLine(float percent)
	{
		if (percent > 1f)
		{
			percent = 1f;
		}
		if (percent > float.Epsilon)
		{
			if (!PercentLinePic.enabled)
			{
				PercentLinePic.enabled = true;
			}
			PercentLinePic.width = (int)((float)PercentLineBottom.width * percent);
		}
		else
		{
			PercentLinePic.enabled = false;
		}
		PercentLabel.text = $"{(int)(percent * 100f)}%";
	}

	private void Update()
	{
		if (curInfo != null && curInfo.state == 1)
		{
			updateTimeCount += Time.deltaTime;
			if (updateTimeCount > 1f)
			{
				updateTimeCount = 0f;
				UpdatePercent();
			}
		}
	}

	public void OnClickItem()
	{
		if (curData.IsOpen == 0)
		{
			NoticeLogic.AddNotifyData("#{103019}");
		}
		else if (!UnityVersionUtil.IsActive(LockIcon.gameObject))
		{
			if (onClickItem != null)
			{
				onClickItem(curInfo.id);
			}
			else
			{
				Debug.Log("onClickItem == null");
			}
		}
		else
		{
			NoticeLogic.AddNotifyData("#{103009}");
		}
	}
}
