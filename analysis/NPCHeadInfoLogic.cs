using UnityEngine;

public class NPCHeadInfoLogic : HeadInfoLogic
{
	public UISprite HeadPic;

	public TweenColor UITweenCol;

	private bool NeedShowName;

	private float lastChangeTime;

	public void SetNameLabel(string name, GameDefine.CAMP_TYPE npcCamp, bool needShowHPLine = true, HEAD_PIC_TYPE headType = HEAD_PIC_TYPE.INVALID, bool needShowName = false)
	{
		NeedShowName = needShowName;
		NameLabel.text = name;
		switch (npcCamp)
		{
		case GameDefine.CAMP_TYPE.FUNCTION_NPC:
			NameLabel.color = Color.yellow;
			NGUITools.SetActive(NameLabel.gameObject, state: true);
			break;
		case GameDefine.CAMP_TYPE.PLAYER_FRIEND_NPC:
			NameLabel.color = Color.yellow;
			NGUITools.SetActive(NameLabel.gameObject, state: true);
			break;
		case GameDefine.CAMP_TYPE.NORMAL_NPC:
			NameLabel.color = Color.red;
			if (!NeedShowName)
			{
				NGUITools.SetActive(NameLabel.gameObject, state: false);
			}
			break;
		case GameDefine.CAMP_TYPE.NPC_ATTACK_NPC:
			NameLabel.color = Color.red;
			if (!NeedShowName)
			{
				NGUITools.SetActive(NameLabel.gameObject, state: false);
			}
			break;
		default:
			NameLabel.color = Color.red;
			if (!NeedShowName)
			{
				NGUITools.SetActive(NameLabel.gameObject, state: false);
			}
			break;
		}
		switch (headType)
		{
		case HEAD_PIC_TYPE.INVALID:
			NGUITools.SetActive(HeadPic.gameObject, state: false);
			break;
		case HEAD_PIC_TYPE.SELF_ESCORT_NPC:
			NGUITools.SetActive(HeadPic.gameObject, state: true);
			HeadPic.spriteName = "CZ_renWuBiaoZhi";
			HeadPic.MakePixelPerfect();
			UITweenCol.enabled = true;
			UITweenCol.from = new Color(0f, 19f / 51f, 1f, 1f);
			UITweenCol.to = new Color(0f, 0.83137256f, 1f, 1f);
			break;
		case HEAD_PIC_TYPE.OTHER_ESCORT_NPC:
			NGUITools.SetActive(HeadPic.gameObject, state: true);
			HeadPic.spriteName = "CZ_renWuBiaoZhi";
			HeadPic.MakePixelPerfect();
			UITweenCol.enabled = true;
			UITweenCol.from = new Color(1f, 0.4392157f, 0f, 1f);
			UITweenCol.to = new Color(1f, 0f, 0f, 1f);
			break;
		case HEAD_PIC_TYPE.MISSION_COMPLETE_NPC:
			NGUITools.SetActive(HeadPic.gameObject, state: true);
			HeadPic.spriteName = "CZ_renWu_wenHao";
			HeadPic.MakePixelPerfect();
			HeadPic.color = Color.white;
			UITweenCol.enabled = false;
			break;
		case HEAD_PIC_TYPE.MISSION_TARGET_NPC:
			NGUITools.SetActive(HeadPic.gameObject, state: true);
			HeadPic.spriteName = "CZ_renWu_tanHao";
			HeadPic.MakePixelPerfect();
			HeadPic.color = Color.white;
			UITweenCol.enabled = false;
			break;
		case HEAD_PIC_TYPE.MISSION_ACCEPT_NPC:
			NGUITools.SetActive(HeadPic.gameObject, state: true);
			HeadPic.spriteName = "CZ_renWu_wenHao_hui";
			HeadPic.MakePixelPerfect();
			HeadPic.color = Color.white;
			UITweenCol.enabled = false;
			break;
		}
		if (!needShowHPLine)
		{
			UnityVersionUtil.SetActiveRecursive(mHpLineLogic.gameObject, state: false);
		}
	}

	public override void SetHpVal(float val)
	{
		ShowHpLine();
		if (mHpLineLogic != null)
		{
			mHpLineLogic.ChangeVal(val);
		}
	}

	public void ShowHpLine()
	{
		if (!NeedShowName)
		{
			NGUITools.SetActive(NameLabel.gameObject, state: true);
		}
		NGUITools.SetActive(mHpLineLogic.gameObject, state: true);
		lastChangeTime = Time.time;
	}

	public void HideHpLine()
	{
		if (!NeedShowName)
		{
			NGUITools.SetActive(NameLabel.gameObject, state: false);
		}
		NGUITools.SetActive(mHpLineLogic.gameObject, state: false);
	}

	private void Update()
	{
		if (UnityVersionUtil.IsActive(mHpLineLogic.gameObject) && Time.time - lastChangeTime > GameDefine.NPC_HP_LINE_SHOW_TIME)
		{
			HideHpLine();
		}
	}
}
