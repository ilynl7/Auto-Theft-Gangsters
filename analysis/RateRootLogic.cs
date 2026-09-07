public class RateRootLogic : SingletonUnity<RateRootLogic>
{
	public UILabel InfoLabel;

	public void OnEnable()
	{
		InfoLabel.text = StrDictionary.GetDictionaryString("#{100160}");
		LocalDataSaveManager.SetRateFlag(0);
	}

	public void OnClickYesBtn()
	{
		CloseRate();
		SingletonDontDestoryUnity<GameManager>.Instance.Rating();
	}

	public void CloseRate()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RateRoot);
		if (SingletonUnity<AutoPopUIRoot>.Exists && UnityVersionUtil.IsActive(SingletonUnity<AutoPopUIRoot>.Instance.gameObject))
		{
			SingletonUnity<AutoPopUIRoot>.Instance.NextPop();
		}
	}
}
