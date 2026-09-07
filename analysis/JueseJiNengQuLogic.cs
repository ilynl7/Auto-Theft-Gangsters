using System;
using UnityEngine;

public class JueseJiNengQuLogic : SingletonUnity<JueseJiNengQuLogic>
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UISprite[] skillIconSprites;

	public UISprite[] skillCDTimeMaskSprites;

	public UIWidget[] SkillBtnRoot;

	public SkillInfoBtnLogic[] BtnLabelPicList;

	public GameObject SwitchBtnRoot;

	public UISprite SwitchCDPic;

	public UILabel SkillGroupLabel;

	private ObjMainPlayer mainPlayer;

	private bool ResetFlag;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	private void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mainPlayer.CameraController.smoothOrbitSpeed = 10f;
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	private void UpdateSKill()
	{
		if (mainPlayer == null)
		{
			mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (!ResetFlag)
		{
			ResetIcon();
		}
		if (!(mainPlayer != null))
		{
			return;
		}
		skillCDTimeMaskSprites[0].fillAmount = mainPlayer.GetComboSKillTimePercent();
		for (int i = 0; i < mainPlayer.PlayerSkillIDList.Count; i++)
		{
			if (!string.IsNullOrEmpty(mainPlayer.PlayerSkillIDList[i]))
			{
				skillCDTimeMaskSprites[i + 1].fillAmount = mainPlayer.GetSkillTimePercent(mainPlayer.PlayerSkillIDList[i]);
			}
			else
			{
				skillCDTimeMaskSprites[i + 1].fillAmount = 0f;
			}
		}
		SwitchCDPic.fillAmount = mainPlayer.GetSwitchSkillCDPercent();
	}

	public void UseSkill_1_Onclick()
	{
		if (mainPlayer == null)
		{
			mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (mainPlayer.IsHaveWeapon())
		{
			if (mOnClickTutorialBtn != null && TutorialManager.CurStep == TUTORIAL_STEP.NORMAL_ATTACK_BUTTON)
			{
				CheckTutorialEvent();
			}
			mainPlayer.ClearAutoSelectCharacter();
			mainPlayer.UseComboSkill();
		}
		else
		{
			NoticeLogic.AddNotifyData("#{200062}");
		}
	}

	public void UseSkill_2_Onclick()
	{
		if (mainPlayer == null)
		{
			mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (mainPlayer.IsHaveWeapon())
		{
			if (mOnClickTutorialBtn != null && TutorialManager.CurStep == TUTORIAL_STEP.SKILL1_BUTTON)
			{
				CheckTutorialEvent();
			}
			mainPlayer.ClearAutoSelectCharacter();
			if (!string.IsNullOrEmpty(mainPlayer.PlayerSkillIDList[1]))
			{
				mainPlayer.UseSkill(mainPlayer.PlayerSkillIDList[1]);
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100681}");
			}
		}
		else
		{
			NoticeLogic.AddNotifyData("#{200062}");
		}
	}

	public void UseSkill_3_Onclick()
	{
		if (mainPlayer == null)
		{
			mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (mainPlayer.IsHaveWeapon())
		{
			mainPlayer.ClearAutoSelectCharacter();
			if (mOnClickTutorialBtn != null && TutorialManager.CurStep == TUTORIAL_STEP.SKILL2_BUTTON)
			{
				CheckTutorialEvent();
			}
			if (!string.IsNullOrEmpty(mainPlayer.PlayerSkillIDList[2]))
			{
				mainPlayer.UseSkill(mainPlayer.PlayerSkillIDList[2]);
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100681}");
			}
		}
		else
		{
			NoticeLogic.AddNotifyData("#{200062}");
		}
	}

	public void UseSkill_4_Onclick()
	{
		if (mainPlayer == null)
		{
			mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (mainPlayer.IsHaveWeapon())
		{
			mainPlayer.ClearAutoSelectCharacter();
			if (!string.IsNullOrEmpty(mainPlayer.PlayerSkillIDList[3]))
			{
				mainPlayer.UseSkill(mainPlayer.PlayerSkillIDList[3]);
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100681}");
			}
		}
		else
		{
			NoticeLogic.AddNotifyData("#{200062}");
		}
	}

	public void UseSkill_S_Onclick()
	{
		if (mainPlayer == null)
		{
			mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (mainPlayer.IsHaveWeapon())
		{
			mainPlayer.ClearAutoSelectCharacter();
			if (!string.IsNullOrEmpty(mainPlayer.PlayerSkillIDList[0]))
			{
				mainPlayer.UseSkill(mainPlayer.PlayerSkillIDList[0]);
			}
			else
			{
				NoticeLogic.AddNotifyData("#{100681}");
			}
		}
		else
		{
			NoticeLogic.AddNotifyData("#{200062}");
		}
	}

	private new void Awake()
	{
		base.Awake();
		ResetFlag = false;
	}

	private void ResetIcon()
	{
		if (mainPlayer == null)
		{
			mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		}
		if (mainPlayer != null)
		{
			UpdateIcon();
			ResetFlag = true;
		}
	}

	public void UpdateIcon()
	{
		if (!UnityVersionUtil.IsActive(base.gameObject))
		{
			return;
		}
		for (int i = 0; i < SkillBtnRoot.Length; i++)
		{
			skillIconSprites[i].spriteName = "CZ_jiNengSuoDing";
			skillCDTimeMaskSprites[i].fillAmount = 1f;
		}
		for (int j = 0; j < BtnLabelPicList.Length; j++)
		{
			for (int k = 0; k < BtnLabelPicList[j].SkillLabelPic.Count; k++)
			{
				BtnLabelPicList[j].SkillLabelPic[k].color = Color.white;
			}
		}
		for (int l = 0; l < mainPlayer.CharacterSkillData.Count; l++)
		{
			if (mainPlayer.CharacterSkillData[l].IsDisable)
			{
				continue;
			}
			int index = mainPlayer.CharacterSkillData[l].Index;
			if (index == 0)
			{
				SkillData skillDataById = DataManager.GetSkillDataById(mainPlayer.CharacterSkillData[l].ID);
				skillIconSprites[index].spriteName = skillDataById.Icon;
			}
			if (mainPlayer.SkillIndex == 0)
			{
				if (index <= 2 || index >= 7)
				{
					continue;
				}
				if (mainPlayer.CharacterSkillData[l].UnlockLevel > mainPlayer.AttributeData.Level)
				{
					skillIconSprites[index - 2].spriteName = "CZ_jiNengSuoDing";
					continue;
				}
				SkillData skillDataById2 = DataManager.GetSkillDataById(mainPlayer.CharacterSkillData[l].ID);
				if (skillDataById2 == null)
				{
					continue;
				}
				skillIconSprites[index - 2].spriteName = skillDataById2.Icon;
				if (index == 3)
				{
					continue;
				}
				for (int m = 0; m < BtnLabelPicList[index - 4].SkillLabelPic.Count; m++)
				{
					if (m < skillDataById2.LabelIdList.Count)
					{
						SkillLabelData skillLabelDataByID = DataManager.GetSkillLabelDataByID(skillDataById2.LabelIdList[m]);
						BtnLabelPicList[index - 4].SkillLabelPic[m].color = skillLabelDataByID.LabelColor;
					}
					else
					{
						BtnLabelPicList[index - 4].SkillLabelPic[m].color = Color.white;
					}
				}
				continue;
			}
			switch (index)
			{
			case 3:
			{
				if (mainPlayer.CharacterSkillData[l].UnlockLevel > mainPlayer.AttributeData.Level)
				{
					skillIconSprites[index - 2].spriteName = "CZ_jiNengSuoDing";
					break;
				}
				SkillData skillDataById4 = DataManager.GetSkillDataById(mainPlayer.CharacterSkillData[l].ID);
				skillIconSprites[index - 2].spriteName = skillDataById4.Icon;
				break;
			}
			case 7:
			case 8:
			case 9:
			{
				if (mainPlayer.CharacterSkillData[l].UnlockLevel > mainPlayer.AttributeData.Level)
				{
					skillIconSprites[index - 5].spriteName = "CZ_jiNengSuoDing";
					break;
				}
				SkillData skillDataById3 = DataManager.GetSkillDataById(mainPlayer.CharacterSkillData[l].ID);
				skillIconSprites[index - 5].spriteName = skillDataById3.Icon;
				for (int n = 0; n < BtnLabelPicList[index - 7].SkillLabelPic.Count; n++)
				{
					if (n < skillDataById3.LabelIdList.Count)
					{
						SkillLabelData skillLabelDataByID2 = DataManager.GetSkillLabelDataByID(skillDataById3.LabelIdList[n]);
						BtnLabelPicList[index - 7].SkillLabelPic[n].color = skillLabelDataByID2.LabelColor;
					}
					else
					{
						BtnLabelPicList[index - 7].SkillLabelPic[n].color = Color.white;
					}
				}
				break;
			}
			}
		}
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SKILL))
		{
			NGUITools.SetActive(SwitchBtnRoot, state: true);
			SkillGroupLabel.text = $"{mainPlayer.SkillIndex + 1}";
		}
		else
		{
			NGUITools.SetActive(SwitchBtnRoot, state: false);
		}
	}

	public void Reset(bool hideAllBtn = false)
	{
		ResetIcon();
		if (hideAllBtn)
		{
			for (int i = 0; i < SkillBtnRoot.Length; i++)
			{
				NGUITools.SetActive(SkillBtnRoot[i].gameObject, state: false);
			}
		}
	}

	public void ShowSkillBtn(int index)
	{
		if (index >= 0 && index < SkillBtnRoot.Length)
		{
			NGUITools.SetActive(SkillBtnRoot[index].gameObject, state: true);
		}
	}

	public void ShowAllSkillBtn()
	{
		for (int i = 0; i < SkillBtnRoot.Length; i++)
		{
			NGUITools.SetActive(SkillBtnRoot[i].gameObject, state: true);
		}
		UpdateIcon();
	}

	private void Update()
	{
		UpdateSKill();
	}

	public void OnClickSwitchBtn()
	{
		if (mainPlayer.SwitchSkillGroup())
		{
			UpdateIcon();
		}
	}

	private void OnEnable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateIcon));
	}

	private void OnDisable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(UpdateIcon));
	}
}
