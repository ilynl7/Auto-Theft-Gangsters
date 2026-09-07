using UnityEngine;

public class SurviveBattleFloorInfoRootLogic : SingletonUnity<SurviveBattleFloorInfoRootLogic>
{
	public UILabel TypeLabel;

	public UILabel ScoreLabel;

	public UISprite BtnPic;

	public TweenPosition TwPosition;

	public UISprite BtnSprite;

	public UITexture EffectPic;

	public GameObject HandTipRoot;

	public void Reset(int floorid, int scores)
	{
		EffectPic.enabled = false;
		if (floorid == 0)
		{
			TypeLabel.text = StrDictionary.GetDictionaryString("#{101583}");
			BtnPic.spriteName = "CZ_tuBiao_Up";
			BtnSprite.color = new Color(1f, 73f / 85f, 0f, 1f);
		}
		else
		{
			TypeLabel.text = StrDictionary.GetDictionaryString("#{101584}");
			BtnPic.spriteName = "CZ_tuBiao_Down";
			BtnSprite.color = Color.white;
		}
		ScoreLabel.text = scores.ToString();
		DisableHandTip();
	}

	public void UpdateBtnEnable(bool isEnable)
	{
		TwPosition.enabled = isEnable;
		if (isEnable)
		{
			BtnSprite.color = new Color(1f, 73f / 85f, 0f, 1f);
			EffectPic.enabled = true;
			EnableHandTip();
		}
		else
		{
			BtnSprite.color = Color.white;
			EffectPic.enabled = false;
			DisableHandTip();
		}
	}

	public void EnableHandTip()
	{
		if (!UnityVersionUtil.IsActive(HandTipRoot))
		{
			NGUITools.SetActive(HandTipRoot, state: true);
		}
	}

	public void DisableHandTip()
	{
		if (UnityVersionUtil.IsActive(HandTipRoot))
		{
			NGUITools.SetActive(HandTipRoot, state: false);
		}
	}

	public void OnClickBtn()
	{
		(SingletonDontDestoryUnity<GameManager>.Instance.SceneManager as SurvivalBattleSceneManager).MoveToNextFloor();
	}
}
