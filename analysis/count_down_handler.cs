using Sproto;
using SprotoType;

public class count_down_handler
{
	public static SprotoTypeBase count_down_request(SprotoTypeBase req)
	{
		if (req is count_down.request { type: var type })
		{
			switch (type)
			{
			}
		}
		return null;
	}
}
