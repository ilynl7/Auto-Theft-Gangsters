using Sproto;
using SprotoType;

public class facebook_unlink_handler
{
	public static void facebook_unlink_response(SprotoTypeBase rep)
	{
		facebook_unlink.response response = rep as facebook_unlink.response;
		if (rep == null)
		{
		}
	}
}
