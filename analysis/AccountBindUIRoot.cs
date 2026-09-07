public class AccountBindUIRoot : SingletonUnity<AccountBindUIRoot>
{
	public UILabel FbLabel;

	public UILabel Glabel;

	public void Init()
	{
	}

	private void OnEnable()
	{
		Init();
	}

	public void OnClickClose()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.AccountBindUIRoot);
	}

	public void OnClickFb()
	{
		if (PlayerData.FaceBookBind == 200 || PlayerData.FaceBookBind < 0)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.SignInFacebook();
		}
		else
		{
			SingletonDontDestoryUnity<GameManager>.Instance.SignOutFacebook();
		}
		OnClickClose();
	}

	public void OnClickG()
	{
		if (PlayerData.FaceBookBind == 100 || PlayerData.FaceBookBind < 0)
		{
			SingletonDontDestoryUnity<GameManager>.Instance.SignInGoogle();
		}
		else
		{
			SingletonDontDestoryUnity<GameManager>.Instance.SignOutGoogle();
		}
		OnClickClose();
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
