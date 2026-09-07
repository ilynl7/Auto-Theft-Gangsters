using Sproto;
using SprotoType;

public class ask_character_info_handler
{
	public static void ask_character_info_response(SprotoTypeBase rep)
	{
		ask_character_info.response response = rep as ask_character_info.response;
		if (rep == null)
		{
		}
	}
}
