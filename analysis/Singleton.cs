public class Singleton<T> where T : class, new()
{
	private static T mInstance;

	public static T Instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = new T();
			}
			return mInstance;
		}
	}

	public static bool Exists => mInstance != null;

	public static void ClearInstace()
	{
		mInstance = (T)null;
	}
}
