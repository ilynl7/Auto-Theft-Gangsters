using System.Collections.Generic;
using UnityEngine;

public class DamageBoardManager : MonoBehaviour
{
	private class DamageBoardLoadInfo
	{
		public int mType;

		public string valueStr;

		public Vector3 mPos;

		public DamageBoardLoadInfo(int type, string str, Vector3 pos)
		{
			mType = type;
			valueStr = str;
			mPos = pos;
		}
	}

	private const int mMaxDamgeBoardType = 10;

	private const int mMaxDamgeBoard = 16;

	private static Dictionary<int, List<DamageBoard>> mEnableDict = new Dictionary<int, List<DamageBoard>>();

	private static Dictionary<int, List<DamageBoard>> mDisableDict = new Dictionary<int, List<DamageBoard>>();

	private void Awake()
	{
		ClearDamgeDict();
	}

	public static void ClearDamgeDict()
	{
		mEnableDict.Clear();
		mDisableDict.Clear();
	}

	public static void PreLoadDamageBoard()
	{
		ClearDamgeDict();
	}

	private static void AddDict(Dictionary<int, List<DamageBoard>> dict, int type, DamageBoard board)
	{
		List<DamageBoard> value = null;
		if (dict.TryGetValue(type, out value))
		{
			value.Add(board);
			return;
		}
		value = new List<DamageBoard>();
		value.Add(board);
		dict.Add(type, value);
	}

	public void ShowDamgaeBoard(int type, string strValue, Vector3 pos)
	{
		List<DamageBoard> value = null;
		DamageBoard damageBoard = null;
		if (mDisableDict.TryGetValue(type, out value) && value.Count > 0)
		{
			damageBoard = value[0];
			damageBoard.Reuse();
			value.RemoveAt(0);
			AddDict(mEnableDict, type, damageBoard);
		}
		else if ((mEnableDict.ContainsKey(type) && mEnableDict[type].Count < 16) || !mEnableDict.ContainsKey(type))
		{
			DamageBoardLoadInfo param = new DamageBoardLoadInfo(type, strValue, pos);
			switch (type)
			{
			case 1:
				SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.DamgaeBoardUI, LoadDamgeBoard, param);
				break;
			case 3:
				SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.DamgaeBoardUI2, LoadDamgeBoard, param);
				break;
			case 4:
				SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.DamgaeBoardUI3, LoadDamgeBoard, param);
				break;
			case 5:
				SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.DamgaeBoardUI4, LoadDamgeBoard, param);
				break;
			case 7:
				SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.DamgaeBoardUI5, LoadDamgeBoard, param);
				break;
			case 8:
				SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.DamgaeBoardUI6, LoadDamgeBoard, param);
				break;
			case 10:
				SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.DamgaeBoardUI7, LoadDamgeBoard, param);
				break;
			default:
				SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.DamgaeBoardUI, LoadDamgeBoard, param);
				break;
			}
		}
		else
		{
			value = null;
			if (mEnableDict.TryGetValue(type, out value))
			{
				damageBoard = value[0];
				damageBoard.Reuse();
				value.RemoveAt(0);
				AddDict(mEnableDict, type, damageBoard);
			}
		}
		if (damageBoard != null)
		{
			damageBoard.ShowDamgeBoard(type, strValue, pos);
		}
	}

	private static void LoadDamgeBoard(GameObject obj, object param)
	{
		GameObject gameObject = Object.Instantiate(obj) as GameObject;
		gameObject.transform.parent = SingletonDontDestoryUnity<GameManager>.Instance.SceneManager.DamageBoadRoot.transform;
		DamageBoard component = gameObject.GetComponent<DamageBoard>();
		if (component != null)
		{
			DamageBoardLoadInfo damageBoardLoadInfo = param as DamageBoardLoadInfo;
			AddDict(mEnableDict, damageBoardLoadInfo.mType, component);
			component.ShowDamgeBoard(damageBoardLoadInfo.mType, damageBoardLoadInfo.valueStr, damageBoardLoadInfo.mPos);
		}
	}

	private void UpdateDamageBoard()
	{
		float time = Time.time;
		for (int i = 1; i < 10; i++)
		{
			List<DamageBoard> value = null;
			if (!mEnableDict.TryGetValue(i, out value))
			{
				continue;
			}
			for (int num = value.Count - 1; num > -1; num--)
			{
				DamageBoard damageBoard = value[num];
				if (time - damageBoard.ShowTime > 0.5f)
				{
					damageBoard.Reuse();
					AddDict(mDisableDict, i, damageBoard);
					value.RemoveAt(num);
				}
			}
		}
	}

	private void FixedUpdate()
	{
		UpdateDamageBoard();
	}
}
