using UnityEngine;

public class SingletonUnity<T> : MonoBehaviour where T : SingletonUnity<T>
{
	private static SingletonUnity<T> mInstance;

	public static T Instance => (T)mInstance;

	public static bool Exists { get; private set; }

	protected virtual void Awake()
	{
		if (mInstance == null)
		{
			mInstance = this;
			Exists = true;
		}
		else if (mInstance != this)
		{
			Debug.LogWarning("Two Instance" + typeof(T).ToString());
			Object.Destroy(mInstance);
			mInstance = this;
		}
	}

	protected virtual void OnDestroy()
	{
		if (mInstance == this)
		{
			Exists = false;
			mInstance = null;
		}
	}
}
