using UnityEngine;

public class SingletonDontDestoryUnity<T> : MonoBehaviour where T : SingletonDontDestoryUnity<T>
{
	private static SingletonDontDestoryUnity<T> mInstance;

	private bool mIsInit;

	public static T Instance => (T)mInstance;

	public static bool Exists { get; private set; }

	public bool IsInit => mIsInit;

	protected virtual void Awake()
	{
		if (mInstance != null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		Exists = false;
		mInstance = this;
		Exists = true;
		Object.DontDestroyOnLoad(this);
		mIsInit = true;
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
