using Sproto;
using SprotoType;

public class visitor_handler
{
	public static void visitor_response(SprotoTypeBase rep)
	{
		visitor.response response = rep as visitor.response;
		if (rep == null)
		{
		}
	}
}
