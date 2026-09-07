using Sproto;
using SprotoType;

public class facebook_link_handler
{
	public static void facebook_link_response(SprotoTypeBase rep)
	{
		facebook_link.response response = rep as facebook_link.response;
		if (rep == null)
		{
		}
	}
}
