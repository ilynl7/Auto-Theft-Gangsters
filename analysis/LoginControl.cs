using Sproto;
using UnityEngine;

public class LoginControl : MonoBehaviour
{
	public string ip = "127.0.0.1";

	public int port = 9777;

	public void Connect(bool isSuccess)
	{
		if (!isSuccess)
		{
		}
	}

	private void HandShakeRequest(string name, string key)
	{
	}

	private void HandShakeResponse(SprotoTypeBase rpcRsp)
	{
	}

	private void SendAuthRequest(string res)
	{
	}

	public void ConnectServer()
	{
		SingletonDontDestoryUnity<NetManager>.Instance.ConnectToServer(ip, port, Connect);
	}
}
