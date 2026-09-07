using Sproto;
using SprotoType;

public class character_list_handler
{
	public static void character_list_response(SprotoTypeBase rep)
	{
		character_list.response response = rep as character_list.response;
		if (rep == null)
		{
		}
	}
}
