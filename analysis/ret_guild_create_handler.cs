using Sproto;
using SprotoType;

public class ret_guild_create_handler
{
	public static SprotoTypeBase ret_guild_create_request(SprotoTypeBase req)
	{
		if (req is ret_guild_create.request { state: not 0L })
		{
		}
		return null;
	}
}
