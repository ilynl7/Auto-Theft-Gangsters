using Sproto;
using SprotoType;

public class login_max_count_handler
{
	public static SprotoTypeBase login_max_count_request(SprotoTypeBase req)
	{
		if (req is login_max_count.request)
		{
			NoticeLogic.AddNotifyData("#{200063}");
		}
		return null;
	}
}
