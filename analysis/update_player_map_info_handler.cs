using Sproto;
using SprotoType;

public class update_player_map_info_handler
{
	public static void update_player_map_info_response(SprotoTypeBase rep)
	{
		update_player_map_info.response response = rep as update_player_map_info.response;
		if (rep == null)
		{
		}
	}
}
