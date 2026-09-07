using SprotoType;
using UnityEngine;

public class WildBossSubLineLogic : MonoBehaviour
{
	private TutorialManager.OnClickTutorialBtn mOnClickTutorialBtn;

	public UILabel NameLabel;

	public UISprite IconSprite;

	public UILabel LevelLabel;

	public UISprite SelectBkSprite;

	public UISprite CompleteSprite;

	public DelegateDefine.TwoIntParamDelegate onClickItem;

	public GameObject SelectUpSprite;

	public int enableLineFlag;

	public int curIndex = -1;

	public void RegisterTutorialEvent(TutorialManager.OnClickTutorialBtn tutorialEvent)
	{
		mOnClickTutorialBtn = tutorialEvent;
	}

	public void CheckTutorialEvent()
	{
		if (mOnClickTutorialBtn != null)
		{
			mOnClickTutorialBtn();
			mOnClickTutorialBtn = null;
		}
	}

	public void ResetItem(int infoindex, activity_info info, DelegateDefine.TwoIntParamDelegate clickFunc)
	{
		if (info.Type == 5)
		{
			curIndex = infoindex;
			PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
			WildBossData wildBossDataByID = DataManager.GetWildBossDataByID(info.ID);
			NpcData npcDataByID = DataManager.GetNpcDataByID(wildBossDataByID.BossID);
			PlayerData playerData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData;
			enableLineFlag = 0;
			IconSprite.spriteName = wildBossDataByID.Icon;
			NameLabel.text = StrDictionary.GetDictionaryString(npcDataByID.Name);
			LevelLabel.text = string.Format("{0}: {1}-{2}", StrDictionary.GetDictionaryString("#{100771}"), wildBossDataByID.LevelMin, wildBossDataByID.LevelMax);
			int num = (int)info.CurNum;
			if (info.State == 2)
			{
				CompleteSprite.enabled = true;
				enableLineFlag = 2;
			}
			else
			{
				CompleteSprite.enabled = false;
			}
			if (info.State == 0L && playerCommonData.GetCurServerTime() < info.Parm)
			{
				enableLineFlag = 2;
			}
			if (num <= 0 && playerCommonData.GetCurServerTime() > info.time + 5400)
			{
				enableLineFlag = 3;
			}
			if (CheckLevel(wildBossDataByID.LevelMin, wildBossDataByID.LevelMax))
			{
				SetLabelWarining(LevelLabel, needWarning: false);
			}
			else
			{
				SetLabelWarining(LevelLabel, needWarning: true);
				enableLineFlag = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckIsLevelHeigh(wildBossDataByID.LevelMin, wildBossDataByID.LevelMax);
			}
			RefreshSelect(-1);
			onClickItem = clickFunc;
		}
	}

	public bool CheckLevel(int minLevel, int maxlevel)
	{
		return SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.CheckLevel(minLevel, maxlevel);
	}

	public void OnClikcItemBtn()
	{
		if (onClickItem != null)
		{
			onClickItem(curIndex, enableLineFlag);
		}
		if (TutorialManager.CurStep == TUTORIAL_STEP.WORLD_BOSS_CHOOSE_COPY)
		{
			CheckTutorialEvent();
		}
	}

	private void SetLabelWarining(UILabel label, bool needWarning, bool isLowWarning = false)
	{
		if (needWarning)
		{
			if (isLowWarning)
			{
				label.color = new Color(0.537f, 0.537f, 0.537f, 1f);
			}
			else
			{
				label.color = Color.red;
			}
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
