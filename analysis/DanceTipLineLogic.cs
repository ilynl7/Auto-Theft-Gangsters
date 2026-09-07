using SprotoType;
using UnityEngine;

public class DanceTipLineLogic : MonoBehaviour
{
	private dance_state_info CurInfo;

	private CityDanceData CurData;

	public UISprite Icon;

	public UILabel nameLabel;

	public UISprite SliderSp;

	public UISlider SliderPro;

	public UILabel infolabel;

	public TweenScale AnimaScale;

	private Color finishcolor = new Color(1f, 59f / 85f, 0f);

	private Color procolor = new Color(58f / 85f, 1f, 0f);

	private long reamaintime;

	private long allTime;

	private float temptime;

	private ObjMainPlayer mMainPlayer;

	private bool singleDanceFlag;

	public void Reset(dance_state_info curinfo)
	{
		mMainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
		CurInfo = curinfo;
		CurData = DataManager.GetCityDanceDataById(curinfo.ID);
		Icon.spriteName = StrDictionary.GetDictionaryString(CurData.MissionIcon);
		nameLabel.text = StrDictionary.GetDictionaryString(CurData.Name);
		allTime = CurData.DurationTime;
		PlayerCommonData playerCommonData = SingletonDontDestoryUnity<GameManager>.Instance.PlayerCommonData;
		singleDanceFlag = false;
		switch (CurData.Type)
		{
		case 0:
			reamaintime = CurInfo.duration;
			singleDanceFlag = true;
			break;
		case 1:
		case 2:
		case 3:
			reamaintime = CurInfo.end_time - playerCommonData.GetCurServerTime();
			break;
		}
		temptime = 0f;
		if (reamaintime > 0)
		{
			SliderPro.value = 1f - (float)reamaintime / (float)allTime;
			SliderSp.color = procolor;
			infolabel.color = procolor;
			infolabel.text = TimeTools.GetMinuteSecondStr(reamaintime);
			AnimaScale.enabled = true;
		}
		else
		{
			SliderPro.value = 1f;
			SliderSp.color = finishcolor;
			infolabel.color = finishcolor;
			infolabel.text = StrDictionary.GetDictionaryString("#{105078}");
			AnimaScale.ResetToBeginning();
			AnimaScale.enabled = false;
		}
	}

	private void Update()
	{
		if (!(mMainPlayer == null) && (!singleDanceFlag || mMainPlayer.CurPlayerState == PLAYER_STATE.DANCE) && reamaintime > 0)
		{
			temptime += Time.deltaTime;
			if (temptime >= 1f)
			{
				reamaintime--;
				temptime -= 1f;
				infolabel.text = TimeTools.GetMinuteSecondStr(reamaintime);
				SliderPro.value = 1f - (float)reamaintime / (float)allTime;
			}
			if (reamaintime <= 0)
			{
				SliderPro.value = 1f;
				SliderSp.color = finishcolor;
				infolabel.color = finishcolor;
				infolabel.text = StrDictionary.GetDictionaryString("#{105078}");
				AnimaScale.ResetToBeginning();
				AnimaScale.enabled = false;
			}
		}
	}
}
