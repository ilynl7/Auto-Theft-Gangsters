using UnityEngine;

public class TestNet : MonoBehaviour
{
	private string ip = "54.174.215.170";

	private int port = 8888;

	public void ConnectServerOnClick()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(ip, port);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}
}
