using Sproto;
using SprotoType;

public class heart_beat_handler
{
	public static void heart_beat_response(SprotoTypeBase rep)
	{
		heart_beat.response response = rep as heart_beat.response;
		if (rep == null)
		{
		}
	}
}
