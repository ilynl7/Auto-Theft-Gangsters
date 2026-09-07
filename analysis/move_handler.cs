using Sproto;
using SprotoType;

public class move_handler
{
	public static void move_response(SprotoTypeBase rep)
	{
		move.response response = rep as move.response;
		if (rep == null)
		{
		}
	}
}
