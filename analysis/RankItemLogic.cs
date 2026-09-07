using System.Collections.Generic;
using SprotoType;
using UnityEngine;

public class RankItemLogic : MonoBehaviour
{
	public delegate void onClickBtn(int ranknum, int itemindex);

	public int curRankNum;

	public int curItemIndex;

	public UILabel RankLabel;

	public UISprite RankPic;

	public onClickBtn OnClick;

	public UILabel nameLabel;

	public UILabel infoLabel;

	public UISprite VacationFlag;

	public UILabel chairmanlabel;

	public UILabel lvlabel;

	public UILabel carnameLabel;

	public UISprite guildIcon;

	public UILabel MemberLabel;

	public UIGrid CityGrid;

	public List<UISprite> cityList = new List<UISprite>();

	public void Init(onClickBtn Btnfun, int itemindex)
	{
		OnClick = Btnfun;
		curItemIndex = itemindex;
	}

	public void Reset(int rankNum, sort_item curinfo, RANK_TYPE type)
	{
		if (RankPic != null)
		{
			if (rankNum < 3)
			{
				RankPic.enabled = true;
				RankPic.spriteName = GameDefine.RANK_PICNAME[rankNum];
				RankPic.MakePixelPerfect();
				RankLabel.enabled = false;
			}
			else
			{
				RankPic.enabled = false;
				RankLabel.enabled = true;
			}
		}
		RankLabel.text = $"NO.{rankNum + 1}";
		curRankNum = rankNum;
		switch (type)
		{
		case RANK_TYPE.FIGHT:
		case RANK_TYPE.LEVEL:
			nameLabel.text = curinfo.name;
			infoLabel.text = $"{curinfo.score >> 32}";
			VacationFlag.spriteName = GameDefine.Profession_PicName[curinfo.profession];
			carnameLabel.enabled = false;
			VacationFlag.enabled = true;
			break;
		case RANK_TYPE.LADDER:
		{
			string[] array3 = curinfo.name.Split('#');
			nameLabel.text = array3[0];
			if (array3.Length > 1)
			{
				infoLabel.text = $"{array3[1]}";
			}
			else
			{
				infoLabel.text = string.Format("{0}", "error");
			}
			VacationFlag.spriteName = GameDefine.Profession_PicName[curinfo.profession];
			carnameLabel.enabled = false;
			VacationFlag.enabled = true;
			break;
		}
		case RANK_TYPE.TOWER:
			nameLabel.text = curinfo.name;
			infoLabel.text = $"{(curinfo.score >> 32) + 1}";
			VacationFlag.spriteName = GameDefine.Profession_PicName[curinfo.profession];
			carnameLabel.enabled = false;
			VacationFlag.enabled = true;
			break;
		case RANK_TYPE.GUILD:
		{
			string[] array3 = curinfo.name.Split('#');
			nameLabel.text = array3[0];
			chairmanlabel.text = array3[1];
			lvlabel.text = string.Empty + curinfo.profession;
			infoLabel.text = $"{curinfo.score >> 32}";
			if (curinfo.HasParm3 && curinfo.HasParm4)
			{
				MemberLabel.text = $"{curinfo.parm3}/{curinfo.parm4}";
			}
			else
			{
				MemberLabel.text = string.Empty;
			}
			if (guildIcon != null)
			{
				if (curinfo.HasParm1)
				{
					guildIcon.spriteName = GameDefine.GuildIcon[(int)curinfo.parm1];
				}
				else
				{
					guildIcon.spriteName = GameDefine.GuildIcon[0];
				}
			}
			if (curinfo.HasParm2)
			{
				List<dict_hash> list = new List<dict_hash>(curinfo.parm2.Values);
				int num = list.Count - cityList.Count;
				if (num > 0)
				{
					for (int i = 0; i < num; i++)
					{
						GameObject gameObject = Object.Instantiate(cityList[0].gameObject) as GameObject;
						UISprite component = gameObject.GetComponent<UISprite>();
						gameObject.name = $"city{cityList.Count:D2}";
						gameObject.transform.parent = CityGrid.transform;
						gameObject.transform.localScale = Vector3.one;
						gameObject.transform.localPosition = Vector3.zero;
						cityList.Add(component);
					}
				}
				for (int j = 0; j < cityList.Count; j++)
				{
					if (j < list.Count)
					{
						MapInfoData mapInfoDataByID = DataManager.GetMapInfoDataByID(list[j].id);
						if (mapInfoDataByID != null)
						{
							cityList[j].spriteName = mapInfoDataByID.MapIcon;
							NGUITools.SetActive(cityList[j].gameObject, state: true);
						}
						else
						{
							NGUITools.SetActive(cityList[j].gameObject, state: false);
						}
					}
					else
					{
						NGUITools.SetActive(cityList[j].gameObject, state: false);
					}
				}
				if (list.Count > 5)
				{
					CityGrid.transform.localScale = Vector3.one * 0.7f;
				}
				else
				{
					CityGrid.transform.localScale = Vector3.one;
				}
				CityGrid.Reposition();
			}
			else
			{
				for (int k = 0; k < cityList.Count; k++)
				{
					NGUITools.SetActive(cityList[k].gameObject, state: false);
				}
			}
			break;
		}
		case RANK_TYPE.CAR:
		{
			string[] array2 = curinfo.name.Split('#');
			nameLabel.text = array2[0];
			MountData mountDataById = DataManager.GetMountDataById(array2[1]);
			carnameLabel.enabled = true;
			VacationFlag.enabled = false;
			carnameLabel.text = StrDictionary.GetDictionaryString(mountDataById.CarName);
			infoLabel.text = $"{TimeTools.GetCentiSecondStr((int)(GameDefine.DayCentiSecond - (curinfo.score >> 32)))}";
			break;
		}
		case RANK_TYPE.SEX:
		{
			string[] array = curinfo.name.Split('#');
			nameLabel.text = array[0];
			infoLabel.text = $"{curinfo.score >> 32}";
			VacationFlag.spriteName = GameDefine.Profession_PicName[curinfo.profession];
			carnameLabel.enabled = false;
			VacationFlag.enabled = true;
			break;
		}
		case RANK_TYPE.CASH:
			break;
		}
	}

	public void OnClickBtn()
	{
		if (OnClick != null)
		{
			OnClick(curRankNum, curItemIndex);
		}
	}
}
