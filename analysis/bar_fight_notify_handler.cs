using Sproto;
using SprotoType;

public class bar_fight_notify_handler
{
	public static SprotoTypeBase bar_fight_notify_request(SprotoTypeBase req)
	{
		if (req is bar_fight_notify.request request)
		{
			PopTipsRoot.ShowPop(0, request.id);
		}
		return null;
	}
}
