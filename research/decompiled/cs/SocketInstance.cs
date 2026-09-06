using System.Net.NetworkInformation;
using System.Net.Sockets;

public class SocketInstance
{
	private Socket mSocket;

	private string mIP;

	private int mPort;

	public bool IsValid => mSocket != null;

	public bool IsConnected => mSocket != null && mSocket.Connected;

	public long Ping()
	{
		Ping ping = new Ping();
		int timeout = 5000;
		PingReply pingReply = ping.Send(mIP, timeout);
		if (pingReply.Status == IPStatus.Success)
		{
			return pingReply.RoundtripTime;
		}
		return -1L;
	}

	public void Connect(string ip, int port)
	{
		mIP = ip;
		mPort = port;
		mSocket = SocketAPI.Connect(mIP, mPort);
	}

	public uint Send(byte[] buff, int len, SocketFlags flags = SocketFlags.None)
	{
		return SocketAPI.Send(mSocket, buff, (uint)len, flags);
	}

	public uint Recv(byte[] buff, int len, uint flags = 0)
	{
		return SocketAPI.Recv(mSocket, buff, (uint)len, flags);
	}

	public uint Avaiable()
	{
		return SocketAPI.Available(mSocket);
	}

	public bool IsCanSend()
	{
		if (mSocket != null)
		{
			return mSocket.Poll(0, SelectMode.SelectWrite);
		}
		return false;
	}

	public bool IsCanReceive()
	{
		if (mSocket != null)
		{
			return mSocket.Poll(0, SelectMode.SelectRead);
		}
		return false;
	}

	public void Close()
	{
		if (mSocket != null)
		{
			mSocket.Close();
			mSocket = null;
		}
	}
}
