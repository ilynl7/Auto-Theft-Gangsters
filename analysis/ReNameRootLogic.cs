using System.Text.RegularExpressions;
using Sproto;
using SprotoType;

public class ReNameRootLogic : SingletonUnity<ReNameRootLogic>
{
	public UIInput NameInput;

	private int NameMinLength = 5;

	private int NameMaxLength = 20;

	public UILabel NameTipsLabel;

	public RewardItem curitem;

	private new void Awake()
	{
		NameTipsLabel.text = StrDictionary.GetDictionaryString("#{200056}", NameMinLength, NameMaxLength);
		curitem.UpdateItem("5024", EQUIP_QUALITY.KUANG_BLUE, 1);
	}

	public void OnClickRandomNameBtn()
	{
		CharacterGetRandomName();
	}

	public void OnClickCreateBtn()
	{
		if (!string.IsNullOrEmpty(NameInput.value))
		{
			NameInput.value = NameInput.value.Trim();
		}
		if (!string.IsNullOrEmpty(NameInput.value))
		{
			if (NameInput.value.Length < NameMinLength || NameInput.value.Length > NameMaxLength)
			{
				MessageBoxLogic.OpenOKBox(StrDictionary.GetDictionaryString("#{200060}", NameMinLength, NameMaxLength), "#{100127}");
			}
			else if (!CheckNameIsRight(NameInput.value))
			{
				MessageBoxLogic.OpenOKBox("#{200058}", "#{100127}");
			}
			else
			{
				OnClickCloseBtn();
				WaitResponseUIRootLogic.OpenWaitBox(301, 10f, 0f);
				re_name.request request = new re_name.request();
				request.name = NameInput.value;
				NetLogic.GetInstance().Send<Protocol.re_name>(request);
			}
		}
		else
		{
			MessageBoxLogic.OpenOKBox("#{200057}", "#{100127}");
		}
	}

	private bool CheckNameIsRight(string namestr)
	{
		string pattern = "^[a-zA-Z0-9]{1}([a-zA-Z0-9]|[ _]){4,19}$";
		if (Regex.IsMatch(namestr, pattern))
		{
			return true;
		}
		return false;
	}

	private void CharacterGetRandomName()
	{
		long type = 0L;
		if (SingletonDontDestoryUnity<GameManager>.Instance.PlayerData.Profession == PROFESSION_TYPE.NQS)
		{
			type = 1L;
		}
		request_random_name.request request = new request_random_name.request();
		request.type = type;
		NetLogic.GetInstance().Send<Protocol.request_random_name>(request, CharacterGetRandomNameResponse);
	}

	private void CharacterGetRandomNameResponse(SprotoTypeBase req)
	{
		if (req is request_random_name.response response)
		{
			NameInput.value = response.name;
		}
	}

	public void OnClickCloseBtn()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.RenameRoot);
	}
}
