using System;
using System.Net;
using System.Net.Sockets;

public class SocketAPI
{
	public static Socket Connect(string IP, int nPort)
	{
		try
		{
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			if (PlayerData.CurLoginServerData.IsUseDns)
			{
				IPAddress[] addressList = Dns.GetHostEntry(IP).AddressList;
				Random random = new Random();
				IPEndPoint remoteEP = new IPEndPoint(addressList[random.Next(0, addressList.Length)], nPort);
				socket.Connect(remoteEP);
			}
			else
			{
				IPEndPoint remoteEP2 = new IPEndPoint(IPAddress.Parse(IP), nPort);
				socket.Connect(remoteEP2);
			}
			return socket;
		}
		catch (Exception ex)
		{
			Log.WARING_MSG("Socket connect erro!ip= " + IP.ToString() + " port=" + nPort + "=" + ex.ToString());
		}
		return null;
	}

	public static uint Send(Socket client, byte[] buff, uint len, SocketFlags flags = SocketFlags.None)
	{
		try
		{
			return (uint)client.Send(buff, (int)len, flags);
		}
		catch (Exception ex)
		{
			Log.WARING_MSG("Socket Send erro!=" + ex.ToString());
		}
		return uint.MaxValue;
	}

	public static uint Recv(Socket client, byte[] buff, uint len, uint flags = 0)
	{
		try
		{
			uint num = 0u;
			return (uint)client.Receive(buff, (int)len, (SocketFlags)flags);
		}
		catch (Exception ex)
		{
			Log.WARING_MSG("Socket Recv erro!=" + ex.ToString());
		}
		return uint.MaxValue;
	}

	public static void Close(Socket client)
	{
		try
		{
			if (client.Connected)
			{
				client.Shutdown(SocketShutdown.Both);
			}
		}
		catch (Exception ex)
		{
			Log.WARING_MSG("Shutdown Socket erro! =" + ex.ToString());
		}
		try
		{
			client.Close();
		}
		catch (Exception ex2)
		{
			Log.WARING_MSG("Close Socket erro! =" + ex2.ToString());
		}
	}

	public static uint Available(Socket client)
	{
		return (uint)client.Available;
	}
}
