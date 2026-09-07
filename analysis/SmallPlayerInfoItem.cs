using UnityEngine;

public class SmallPlayerInfoItem : MonoBehaviour
{
	public UISprite PlayerIcon;

	public UILabel NameLabel;

	public UILabel LevelLabel;

	public UILabel ComboValLabel;

	public UILabel BtnLabel;

	public UIButtonColor BtnColor;

	public BoxCollider BtnCollider;

	private long mKey;

	private DelegateDefine.OneLongParamDelegate onClickBtn;

	private string mDisableBtnName;

	private PROFESSION_TYPE mProfession;

	private string mName;

	private int mComboVal;

	private int mLevel;

	private long mGuildId;

	private string mGuildName;

	public void Reset(PROFESSION_TYPE profession, string name, int comboVal, int level, string btnName, string disableBtnName, bool isBtnEnable, long key, long guildId, string guildName, DelegateDefine.OneLongParamDelegate clickFunc = null)
	{
		Reset((int)profession, name, comboVal, level, btnName, disableBtnName, isBtnEnable, key, guildId, guildName, clickFunc);
	}

	public void Reset(int profession, string name, int comboVal, int level, string btnName, string disableBtnName, bool isBtnEnable, long key, long guildId, string guildName, DelegateDefine.OneLongParamDelegate clickFunc = null)
	{
		PlayerIcon.spriteName = GameDefine.Player_Icon_Small_Pic[profession];
		NameLabel.text = name;
		LevelLabel.text = $"Lv.{level}";
		ComboValLabel.text = $"{comboVal}";
		mKey = key;
		mProfession = (PROFESSION_TYPE)profession;
		mName = name;
		mComboVal = comboVal;
		mLevel = level;
		mGuildId = guildId;
		mGuildName = guildName;
		BtnCollider.enabled = isBtnEnable;
		if (!isBtnEnable)
		{
			BtnColor.isEnabled = false;
			BtnColor.SetState(UIButtonColor.State.Disabled, instant: true);
			BtnLabel.text = disableBtnName;
		}
		else
		{
			BtnLabel.text = btnName;
			BtnColor.isEnabled = true;
			BtnColor.SetState(UIButtonColor.State.Normal, instant: true);
		}
		mDisableBtnName = disableBtnName;
		onClickBtn = clickFunc;
	}

	public void OnClickBtn()
	{
		if (onClickBtn != null)
		{
			onClickBtn(mKey);
		}
		BtnCollider.enabled = false;
		BtnColor.isEnabled = false;
		BtnColor.SetState(UIButtonColor.State.Disabled, instant: true);
		BtnLabel.text = mDisableBtnName;
	}

	public void OnClickIcon()
	{
		TargetBasicInfo selectTargetBasicInfo = SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.SelectTargetBasicInfo;
		selectTargetBasicInfo.ResetInfo(mKey, mLevel, mComboVal, mName, mProfession, 1, mGuildId, mGuildName, UICamera.currentTouch.pos);
		HitOtherPLayerLogic.ShowMenu(HitType.HitOtherPlayer, selectTargetBasicInfo);
	}
}
