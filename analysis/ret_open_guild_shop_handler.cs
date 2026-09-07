using Sproto;
using SprotoType;

public class ret_open_guild_shop_handler
{
	public static SprotoTypeBase ret_open_guild_shop_request(SprotoTypeBase req)
	{
		if (req is ret_open_guild_shop.request { HasShop_list: not false })
		{
		}
		return null;
	}
}
