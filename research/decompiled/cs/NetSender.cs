using System.Collections.Generic;
using Sproto;

public class NetSender
{
	private static long session;

	private static Dictionary<long, RpcRspHandler> rpcRspHandlerDict;

	public static void Init()
	{
		rpcRspHandlerDict = new Dictionary<long, RpcRspHandler>();
		session = 0L;
	}

	public static void SendInternal<T>(SprotoTypeBase rpcReq = null, RpcRspHandler rpcRspHandler = null)
	{
		if (rpcRspHandler != null)
		{
			session++;
			AddHandler(session, rpcRspHandler);
			NetLogic.GetInstance().SendInternal<T>(rpcReq, session);
		}
		else
		{
			NetLogic.GetInstance().SendInternal<T>(rpcReq);
		}
	}

	private static void AddHandler(long session, RpcRspHandler rpcRspHandler)
	{
		rpcRspHandlerDict.Add(session, rpcRspHandler);
	}

	private static void RemoveHandler(long session)
	{
		if (rpcRspHandlerDict.ContainsKey(session))
		{
			rpcRspHandlerDict.Remove(session);
		}
	}

	public static RpcRspHandler GetHandler(long session)
	{
		rpcRspHandlerDict.TryGetValue(session, out var value);
		RemoveHandler(session);
		return value;
	}
}
