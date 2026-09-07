using Sproto;
using SprotoType;

public class retrieve_account_handler
{
	public static SprotoTypeBase retrieve_account_request(SprotoTypeBase req)
	{
		if (req is retrieve_account.request { id: var id })
		{
			PlayerData.SavePlayerAccountId(id.ToString());
			PlayerData.SavePlayerAccountKey(id.ToString());
		}
		return null;
	}
}
