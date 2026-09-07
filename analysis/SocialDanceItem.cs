using System;
using UnityEngine;

public class SocialDanceItem : MonoBehaviour
{
	private SocialDanceData mData;

	public UISprite iconSprite;

	public UISprite cdSprite;

	public UILabel NameLabel;

	private bool UnlockState;

	private void OnEnable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Combine(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(Reset));
	}

	private void OnDisable()
	{
		UIUpdateEvent.LevelUpEvent = (UIUpdateEvent.UpdateNoParamEvent)Delegate.Remove(UIUpdateEvent.LevelUpEvent, new UIUpdateEvent.UpdateNoParamEvent(Reset));
	}

	public void Init(SocialDanceData data)
	{
		mData = data;
		Reset();
	}

	private void Reset()
	{
		iconSprite.spriteName = mData.ICON;
		NameLabel.text = StrDictionary.GetDictionaryString(mData.Name);
		UnlockState = false;
		int level = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Level;
		if (level < mData.Level)
		{
			UnlockState = true;
			cdSprite.fillAmount = 1f;
			NameLabel.text = $"Lv.{mData.Level}";
		}
		else
		{
			UpdateCd();
		}
	}

	private void UpdateCd()
	{
		if (!UnlockState && cdSprite.fillAmount != SocialDanceUIRoot.progress)
		{
			cdSprite.fillAmount = SocialDanceUIRoot.progress;
		}
	}

	private void Update()
	{
		UpdateCd();
	}

	public void OnClickSocial()
	{
		if (UnlockState)
		{
			NoticeLogic.AddNotifyData2Client(false, "#{100154}", false, mData.Level);
		}
		else
		{
			if (!(SocialDanceUIRoot.progress <= 0f))
			{
				return;
			}
			ObjMainPlayer mainPlayer = Singleton<ObjManager>.Instance.MainPlayer;
			SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.SocialDanceRoot);
			if (mainPlayer != null)
			{
				if (mainPlayer.PlayeSocialDance(mData.ID))
				{
					SocialDanceUIRoot.CDTime = 5f + Time.realtimeSinceStartup;
				}
				else
				{
					NoticeLogic.AddNotifyData("#{200061}");
				}
			}
		}
	}
}
