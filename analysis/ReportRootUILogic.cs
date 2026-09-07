using System.Text.RegularExpressions;
using SprotoType;

public class ReportRootUILogic : SingletonUnity<ReportRootUILogic>
{
	private const int MAX_CONTENT = 1500;

	private const int MAX_Title = 140;

	private const int MAX_EMAIL = 141;

	public UIInput InputEmail;

	public UIInput InputTitle;

	public UIInput InputContent;

	public void OnClickSendContent()
	{
		string value = InputEmail.value;
		string value2 = InputTitle.value;
		string value3 = InputContent.value;
		if (string.IsNullOrEmpty(value))
		{
			NoticeLogic.AddNotifyData("#{705007}");
			return;
		}
		if (value.Length < 4 || value.Length > 141)
		{
			NoticeLogic.AddNotifyData("#{705006}");
			return;
		}
		Regex regex = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
		if (!regex.IsMatch(value))
		{
			NoticeLogic.AddNotifyData("#{705006}");
			return;
		}
		if (string.IsNullOrEmpty(value2))
		{
			NoticeLogic.AddNotifyData("#{705008}");
			return;
		}
		if (value2.Length < 5 || value2.Length > 140)
		{
			NoticeLogic.AddNotifyData("#{705008}");
			return;
		}
		if (string.IsNullOrEmpty(value3))
		{
			NoticeLogic.AddNotifyData("#{705009}");
			return;
		}
		if (value3.Length < 50 || value3.Length > 1500)
		{
			NoticeLogic.AddNotifyData("#{705009}");
			return;
		}
		send_mail_box.request request = new send_mail_box.request();
		request.subject = value2;
		request.context = value3;
		request.email = value;
		NetLogic.GetInstance().Send<Protocol.send_mail_box>(request);
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ReportRoot);
		NoticeLogic.AddNotifyData("#{705010}");
	}

	public void OnClickClose()
	{
		SingletonUnity<UIManager>.Instance.CloseUI(UIInfo.ReportRoot);
	}
}
