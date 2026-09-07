public class UUID
{
	private static long id = 99L;

	public static long GenUUID()
	{
		return id++;
	}
}
