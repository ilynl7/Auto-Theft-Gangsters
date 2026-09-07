using Sproto;
using SprotoType;

public class login_handler
{
	public static void login_response(SprotoTypeBase rep)
	{
		login.response response = rep as login.response;
		if (rep == null)
		{
		}
	}
}
