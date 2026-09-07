using UnityEngine;

public class JSSXKuangRootLogic : SingletonUnity<JSSXKuangRootLogic>
{
	public JSSXKuangUILogic JSSXKuangUIRoot;

	public Transform ScaleRoot;

	private CharacterAttributeData curAttributeData;

	public void Reset(CharacterAttributeData data)
	{
		curAttributeData = data;
		if (JSSXKuangUIRoot == null)
		{
			SingletonUnity<UIManager>.Instance.LoadUIItem(UIInfo.PlayerInfoRootItem, OnLoadPlayerInfoRootItem);
		}
		else
		{
			JSSXKuangUIRoot.Show(curAttributeData);
		}
	}

	public void OnLoadPlayerInfoRootItem(GameObject newObj, object param)
	{
		GameObject gameObject = Object.Instantiate(newObj) as GameObject;
		gameObject.transform.parent = ScaleRoot;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localScale = Vector3.one;
		JSSXKuangUIRoot = gameObject.GetComponent<JSSXKuangUILogic>();
		JSSXKuangUIRoot.Show(curAttributeData);
	}
}
