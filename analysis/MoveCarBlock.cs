using UnityEngine;

public class MoveCarBlock : MonoBehaviour
{
	public CarPath Path;

	public int PoliceFlag;

	public static string[] NormalCarName = new string[1] { "Chevrolet" };

	private bool EnableFlag;

	private void OnTriggerEnter(Collider other)
	{
		GameObject gameObject = other.gameObject;
		if (gameObject.transform.parent != null && gameObject.transform.parent.parent != null)
		{
			Obj component = gameObject.transform.parent.parent.gameObject.GetComponent<Obj>();
			if (component != null && component.ObjType == GameDefine.OBJ_TYPE.OBJ_PLAYER_CAR)
			{
				StartBlocking();
			}
		}
	}

	private void StartBlocking()
	{
		if (!EnableFlag)
		{
			EnableFlag = true;
			ObjCarInitData initData = null;
			if (PoliceFlag == 0)
			{
				initData = new ObjCarInitData(Path.PathPointList[0].transform.position, Path.PathPointList[0].transform.eulerAngles, -1L, "PoliceCar", policeFlag: true);
			}
			else if (PoliceFlag == 3)
			{
				initData = new ObjCarInitData(Path.PathPointList[0].transform.position, Path.PathPointList[0].transform.eulerAngles, -1L, NormalCarName[Random.Range(0, NormalCarName.Length)], policeFlag: false);
			}
			else if (PoliceFlag == 4)
			{
				initData = new ObjCarInitData(Path.PathPointList[0].transform.position, Path.PathPointList[0].transform.eulerAngles, -1L, "PoliceCar", policeFlag: false);
			}
			ObjSimpleAICar simpleAICar = Singleton<ObjManager>.Instance.GetSimpleAICar(initData);
			if (simpleAICar != null)
			{
				simpleAICar.Reset(initData);
				simpleAICar.SetPath(Path, 0);
				simpleAICar.EnableCar(null);
				simpleAICar.rigidbody.velocity = simpleAICar.transform.forward * 30f;
			}
		}
	}
}
