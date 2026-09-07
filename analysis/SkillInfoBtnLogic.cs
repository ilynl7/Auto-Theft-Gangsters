using System.Collections.Generic;
using UnityEngine;

public class SkillInfoBtnLogic : MonoBehaviour
{
	public UISprite SkillIconSprite;

	public UILabel SkillLevelLabel;

	public List<UISprite> SkillLabelPic;

	public UILabel SkillGroupLabel;

	public UISprite SkillGroupPic;

	public GameObject PlusPicRoot;

	public UISprite RemoveBtnPic;

	private string lockPicName = "CZ_jiNengSuoDing";

	private Vector2 lockPicSize = new Vector2(30f, 34f);

	private Vector2 unLockPicSize = new Vector2(68f, 66f);

	private Vector3 lockPicPos = new Vector3(0f, 9.445351f, 0f);

	private Color lockPicColor = new Color(0f, 0.95686275f, 1f, 1f);

	private Color unlockLabelColor = new Color(1f, 11f / 15f, 0f, 1f);

	private CharacterSkillData mChaSkillData;

	private SkillData mSkillData;

	private Color Group1Color = new Color(0f, 19f / 51f, 25f / 51f, 0.6509804f);

	private Color Group2Color = new Color(0f, 0.78039217f, 0.4f, 0.6509804f);

	public GameObject levelUpTips;

	public SkillDragItemLogic DragItemLogic;

	public SkillDragSurface DragSurface;

	private bool IsTopSlotFlag;

	public string SkillId => mChaSkillData.ID;

	public CharacterSkillData CharacterSkillData => mChaSkillData;

	public void UpdateDrageSurface(int index)
	{
		if (DragSurface != null)
		{
			DragSurface.index = index;
		}
	}

	public void Reset(CharacterSkillData skData, bool isTop = false)
	{
		IsTopSlotFlag = isTop;
		if (RemoveBtnPic != null)
		{
			NGUITools.SetActive(RemoveBtnPic.gameObject, state: false);
		}
		if (skData == null)
		{
			if (SkillLevelLabel != null)
			{
				SkillLevelLabel.text = string.Empty;
			}
			SkillIconSprite.spriteName = "CZ_jiNeng_CD1";
			mChaSkillData = skData;
			mSkillData = null;
			NGUITools.SetActive(levelUpTips, state: false);
			if (SkillGroupPic != null)
			{
				NGUITools.SetActive(SkillGroupPic.gameObject, state: false);
			}
			if (PlusPicRoot != null)
			{
				NGUITools.SetActive(PlusPicRoot, state: true);
			}
			for (int i = 0; i < SkillLabelPic.Count; i++)
			{
				SkillLabelPic[i].color = Color.white;
			}
			if (DragItemLogic != null)
			{
				DragItemLogic.enabled = false;
			}
			return;
		}
		mChaSkillData = skData;
		mSkillData = DataManager.GetSkillDataById(mChaSkillData.ID);
		PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
		if (SkillLevelLabel != null)
		{
			SkillLevelLabel.text = string.Empty;
		}
		if (SkillGroupLabel != null)
		{
			if (mSkillData == null)
			{
				SkillGroupLabel.text = string.Empty;
				NGUITools.SetActive(SkillGroupPic.gameObject, state: false);
			}
			else if (mChaSkillData.Index >= 4 && mChaSkillData.Index <= 6)
			{
				SkillGroupLabel.text = "1";
				NGUITools.SetActive(SkillGroupPic.gameObject, state: true);
				SkillGroupPic.color = Group1Color;
			}
			else if (mChaSkillData.Index >= 7 && mChaSkillData.Index <= 9)
			{
				SkillGroupLabel.text = "2";
				NGUITools.SetActive(SkillGroupPic.gameObject, state: true);
				SkillGroupPic.color = Group2Color;
			}
			else
			{
				SkillGroupLabel.text = string.Empty;
				NGUITools.SetActive(SkillGroupPic.gameObject, state: false);
			}
		}
		if (PlusPicRoot != null)
		{
			if (mSkillData == null)
			{
				NGUITools.SetActive(PlusPicRoot, state: true);
			}
			else
			{
				NGUITools.SetActive(PlusPicRoot, state: false);
			}
		}
		if (mSkillData != null && RemoveBtnPic != null)
		{
			NGUITools.SetActive(RemoveBtnPic.gameObject, state: true);
		}
		if (playerData == null)
		{
			return;
		}
		if (DragItemLogic != null)
		{
			DragItemLogic.enabled = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData.IsFunctionUnlock(FUNCTION_TYPE.SKILL_DRAG) && playerData.MainPlayerAttrData.Level >= skData.UnlockLevel;
		}
		if (playerData.MainPlayerAttrData.Level < skData.UnlockLevel)
		{
			if (SkillLevelLabel != null)
			{
				SkillLevelLabel.text = $"Lv.{skData.UnlockLevel}";
				SkillLevelLabel.color = lockPicColor;
			}
			SkillIconSprite.spriteName = lockPicName;
			NGUITools.SetActive(levelUpTips, state: false);
			if (SkillGroupLabel != null)
			{
				SkillGroupLabel.text = string.Empty;
			}
			return;
		}
		if (mSkillData != null && mSkillData.IsUpgrade == 1)
		{
			NGUITools.SetActive(levelUpTips, playerData.MainPlayerAttrData.Level > skData.Level + 1);
		}
		else
		{
			NGUITools.SetActive(levelUpTips, state: false);
		}
		if (skData.IsDisable)
		{
			SkillIconSprite.spriteName = "CZ_jiNeng_CD1";
			for (int j = 0; j < SkillLabelPic.Count; j++)
			{
				SkillLabelPic[j].color = Color.white;
			}
		}
		else
		{
			SkillIconSprite.spriteName = mSkillData.Icon;
			SkillIconSprite.color = Color.white;
			for (int k = 0; k < SkillLabelPic.Count; k++)
			{
				if (k < mSkillData.LabelIdList.Count)
				{
					SkillLabelData skillLabelDataByID = DataManager.GetSkillLabelDataByID(mSkillData.LabelIdList[k]);
					SkillLabelPic[k].color = skillLabelDataByID.LabelColor;
				}
				else
				{
					SkillLabelPic[k].color = Color.white;
				}
			}
		}
		if (SkillLevelLabel != null)
		{
			SkillLevelLabel.text = $"Lv.{skData.Level + 1}";
		}
	}

	public void OnClickBtn()
	{
		if (IsTopSlotFlag)
		{
			if (mChaSkillData != null)
			{
				SingletonUnity<SkillInfoRootLogic>.Instance.OnClickSkillBtn(mSkillData, mChaSkillData, base.gameObject);
				SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(4);
			}
		}
		else if (mSkillData == null || mChaSkillData == null)
		{
			if (TutorialManager.CurStep != TUTORIAL_STEP.SKILL_DRAG_MOVE)
			{
				TutorialManager.ShowTutorial(TUTORIAL_STEP.SKILL_DRAG_MOVE);
			}
		}
		else
		{
			SingletonUnity<SkillInfoRootLogic>.Instance.OnClickSkillBtn(mSkillData, mChaSkillData, base.gameObject);
			SingletonDontDestoryUnity<SoundManager>.Instance.PlaySoundEffect(4);
		}
	}

	public void OnClickRemoveBtn()
	{
		if (mSkillData != null)
		{
			SingletonUnity<SkillInfoRootLogic>.Instance.RemoveSkill(mSkillData.ID);
		}
	}
}
