using Sproto;
using SprotoType;

public class comb_value_up_tip_handler
{
	public static SprotoTypeBase comb_value_up_tip_request(SprotoTypeBase req)
	{
		if (req is comb_value_up_tip.request request)
		{
			FightingValUpgradeRootLogic.ShowFinghtingValUpgradeRoot(request.current, request.next);
		}
		return null;
	}
}
