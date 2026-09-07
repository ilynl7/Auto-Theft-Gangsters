using UnityEngine;

public class ResourcesManager
{
	public delegate void LoadAutoComboInfoDelegate(GameObject autoInfoObj);

	public delegate void LoadHeadInfoDelegate(GameObject headInfoObj);

	public delegate void LoadSimpleShadowDelegate(GameObject simpleShadowObj);

	public delegate void LoadDropItemDelegate(GameObject dropItemObj, ObjInitDropItemData initData);

	public static Object LoadAndInstantiate(string path)
	{
		Object @object = Load(path);
		if (@object != null)
		{
			return Object.Instantiate(@object);
		}
		return null;
	}

	public static Object Load(string path)
	{
		Object @object = null;
		return Resources.Load(path);
	}

	public static Object LoadAnimation(string path)
	{
		Object @object = null;
		return Resources.Load(path);
	}

	public static void LoadAutoComboPrefab(UIPathData uiData, string prefabName, LoadAutoComboInfoDelegate del)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager != null && sceneManager.NameBoadPool != null)
		{
			SingletonUnity<UIManager>.Instance.LoadUIItem(uiData, LoadAutoInfo, del);
		}
	}

	public static void UnLoadAutoComboPrefab(GameObject autoInfoObj)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager != null && sceneManager.NameBoadPool != null)
		{
			Object.Destroy(autoInfoObj);
		}
	}

	private static void LoadAutoInfo(GameObject Obj, object parm)
	{
		GameObject gameObject = null;
		if (Obj != null)
		{
			gameObject = Object.Instantiate(Obj) as GameObject;
			gameObject.transform.parent = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.NameBoardRoot.transform;
		}
		if (parm is LoadAutoComboInfoDelegate loadAutoComboInfoDelegate)
		{
			loadAutoComboInfoDelegate(gameObject);
		}
	}

	public static void LoadHeadInfoPrefab(UIPathData uiData, string prefabName, LoadHeadInfoDelegate del)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager != null && sceneManager.NameBoadPool != null)
		{
			sceneManager.NameBoadPool.LoadUIItem(uiData, prefabName, LoadHeadInfo, del);
		}
	}

	public static void UnLoadHeadInfoPrefab(GameObject headInfoObj)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager != null && sceneManager.NameBoadPool != null)
		{
			BillBoard component = headInfoObj.GetComponent<BillBoard>();
			if (component != null)
			{
				component.enabled = false;
				component.BindObj = null;
			}
			sceneManager.NameBoadPool.Remove(headInfoObj);
		}
	}

	private static void LoadHeadInfo(GameObject newObj, object parm)
	{
		if (newObj != null)
		{
			newObj.transform.parent = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.NameBoardRoot.transform;
		}
		if (parm is LoadHeadInfoDelegate loadHeadInfoDelegate)
		{
			loadHeadInfoDelegate(newObj);
		}
	}

	public static void LoadSimpleShadowPrefab(UIPathData uiData, string prefabName, LoadSimpleShadowDelegate del)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager != null && sceneManager.SimpleShadowPool != null)
		{
			sceneManager.SimpleShadowPool.LoadUIItem(uiData, prefabName, LoadSimpleShadow, del);
		}
	}

	public static void UnLoadSimpleShadowPrefab(GameObject simpleShadowObj)
	{
		if (!(simpleShadowObj == null))
		{
			UnityVersionUtil.SetActiveRecursive(simpleShadowObj.gameObject, state: false);
			SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
			if (sceneManager != null && sceneManager.SimpleShadowPool != null)
			{
				SimpleShadowFollow component = simpleShadowObj.GetComponent<SimpleShadowFollow>();
				component.BindObj = null;
				simpleShadowObj.transform.position = Vector3.up * -20f;
				sceneManager.SimpleShadowPool.Remove(simpleShadowObj);
			}
		}
	}

	private static void LoadSimpleShadow(GameObject newObj, object parm)
	{
		if (newObj != null)
		{
			newObj.transform.parent = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.NameBoardRoot.transform;
		}
		if (parm is LoadSimpleShadowDelegate loadSimpleShadowDelegate)
		{
			loadSimpleShadowDelegate(newObj);
		}
	}

	public static void LoadDropItemPrefab(UIPathData uiData, string prefabName, LoadDropItemDelegate del, ObjInitDropItemData initData)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager != null && sceneManager.DropItemPool != null)
		{
			object[] parm = new object[2] { del, initData };
			sceneManager.DropItemPool.LoadUIItem(uiData, prefabName, LoadDropItem, parm);
		}
	}

	public static void UnLoadDropItemPrefab(GameObject dropItemObj)
	{
		SceneManager sceneManager = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager;
		if (sceneManager != null && sceneManager.DropItemPool != null)
		{
			sceneManager.DropItemPool.Remove(dropItemObj);
		}
	}

	private static void LoadDropItem(GameObject newObj, object parm)
	{
		if (newObj != null)
		{
			newObj.transform.parent = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.DropItemRoot.transform;
			newObj.transform.localPosition = Vector3.zero;
			newObj.transform.localEulerAngles = Vector3.zero;
		}
		object[] array = parm as object[];
		if (array[0] is LoadDropItemDelegate loadDropItemDelegate)
		{
			loadDropItemDelegate(newObj, array[1] as ObjInitDropItemData);
		}
	}
}
