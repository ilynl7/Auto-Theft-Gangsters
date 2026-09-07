using Sproto;
using SprotoType;

public class equip_inhert_handler
{
	public static void equip_inhert_response(SprotoTypeBase rep)
	{
		equip_inhert.response response = rep as equip_inhert.response;
		if (rep == null)
		{
		}
	}
}
