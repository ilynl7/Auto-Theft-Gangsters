using Sproto;
using SprotoType;

public class update_game_server_handler
{
	public static void update_game_server_response(SprotoTypeBase rep)
	{
		update_game_server.response response = rep as update_game_server.response;
		if (rep == null)
		{
		}
	}
}
