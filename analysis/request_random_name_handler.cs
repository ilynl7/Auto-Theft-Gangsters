using Sproto;
using SprotoType;

public class request_random_name_handler
{
	public static void request_random_name_response(SprotoTypeBase rep)
	{
		request_random_name.response response = rep as request_random_name.response;
		if (rep == null)
		{
		}
	}
}
