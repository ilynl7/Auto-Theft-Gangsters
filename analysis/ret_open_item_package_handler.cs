using Sproto;
using SprotoType;

public class ret_open_item_package_handler
{
	public static SprotoTypeBase ret_open_item_package_request(SprotoTypeBase req)
	{
		if (req is ret_open_item_package.request request && SingletonUnity<OpenBoxRootLogic>.Exists)
		{
			SingletonUnity<OpenBoxRootLogic>.Instance.ShowTipPage(request);
		}
		return null;
	}
}
