using UnityEngine;

public class DelegateDefine
{
	public delegate void OneIntParamDelegate(int val);

	public delegate void TwoIntParamDelegate(int val, int val2);

	public delegate void ThreeParamDelegate(int val, int val2, string val3, bool val4);

	public delegate void ThirdIntParamDelegate(int val, int val2, int val3);

	public delegate void OneLongParamDelegate(long val);

	public delegate void NoParamDelegate();

	public delegate bool NoParamReturnDelegate();

	public delegate void TwoParamDelegate(int val, bool istrue);

	public delegate void OneStringParamDelegate(string val);

	public delegate void StringGameObjectDelegate(string val, GameObject obj);

	public delegate void OneGameItemParamDelegate(GameItem item);

	public delegate void GameObjectDelegate(GameObject obj);
}
