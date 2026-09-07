using UnityEngine;

public class ChangeWayBtn : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OnClick()
	{
		foreach (ObjCharacter value in Singleton<ObjManager>.Instance.ObjDict.Values)
		{
			Vector3 vector = new Vector3(Random.Range(-30, 30), 0f, Random.Range(-30, 30));
			if (NavMesh.SamplePosition(vector, out var _, 0.1f, 15))
			{
				value.MoveTo(vector);
			}
		}
	}
}
