using Sproto;
using SprotoType;

public class notice_handler
{
	public static SprotoTypeBase notice_request(SprotoTypeBase req)
	{
		if (req is notice.request request)
		{
			NoticeLogic.AddNotifyData(request.notice, request.repeate);
		}
		return null;
	}
}
