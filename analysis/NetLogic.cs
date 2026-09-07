using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Sproto;
using SprotoType;
using UnityEngine;

public class NetLogic
{
	public enum CONNECT_STATUS
	{
		INVALID,
		CONNECTING,
		CONNRCTED,
		DISCONNECTED
	}

	public delegate void ConnectDelegate(bool success);

	public delegate void ConnectLostDelegate();

	public delegate void PingComplete(long time);

	public const uint SOCKET_ERROR = uint.MaxValue;

	public const int MAX_ONE_PACK_BYTE_SIZE = 16384;

	private const int MAX_PROCESS_PACK_COUNT_FRAME = 10;

	private static SprotoPack sendPack = new SprotoPack();

	private static SprotoPack recvPack = new SprotoPack();

	private static SprotoStream sendStream = new SprotoStream();

	private SocketReciveStream mSocketReciveStream;

	private SocketSendStream mSocketSendStream;

	private static ProtocolFunctionDictionary protocol = Protocol.Instance.Protocol;

	private static Dictionary<long, ProtocolFunctionDictionary.typeFunc> sessionDict;

	private byte[] headDataBuff = new byte[2];

	private Package pkg = new Package();

	private Package sendPkg = new Package();

	private SocketInstance mSocket;

	private Thread connectThread;

	private string serverIp;

	private int serverPort;

	private int connectSleepTime;

	private CONNECT_STATUS mConnectStatus;

	private bool mConnectFinish;

	private bool isCanProcessPack = true;

	private int nProcesspackCount = 10;

	private static ConnectDelegate mConnectDel = null;

	private static ConnectLostDelegate mConnectLostDel = null;

	private byte[] mMaxRevOnePackbytes;

	private byte[] mMaxSendOnePackbytes;

	private int mMaxRevOnePackCount;

	private static NetLogic mNetLogic = null;

	public static int nReceiveCount = 0;

	public static int nSendCount = 0;

	private static PingComplete mPingComplete = null;

	private long pingTime = -2L;

	private float MAX_CONNECT_TIME = 15f;

	private float curConnectTimeOut = 15f;

	private bool mReConnectingFlag;

	private static Dictionary<Type, SprotoTypeBase> cacheDict = new Dictionary<Type, SprotoTypeBase>();

	public CONNECT_STATUS ConnectStatus => mConnectStatus;

	public bool CanProcessPack
	{
		get
		{
			return isCanProcessPack;
		}
		set
		{
			isCanProcessPack = value;
		}
	}

	public bool ReConnectingFlag => mReConnectingFlag;

	private NetLogic()
	{
		mSocket = new SocketInstance();
		mMaxRevOnePackCount = 16384;
		mMaxRevOnePackbytes = new byte[16384];
		mMaxSendOnePackbytes = new byte[16384];
		NetReceiver.Init();
		NetSender.Init();
		sessionDict = new Dictionary<long, ProtocolFunctionDictionary.typeFunc>();
		connectThread = null;
	}

	public static NetLogic GetInstance()
	{
		if (mNetLogic == null)
		{
			mNetLogic = new NetLogic();
		}
		return mNetLogic;
	}

	public void StartReconnecting()
	{
		mReConnectingFlag = true;
		if (Singleton<ObjManager>.Exists && Singleton<ObjManager>.Instance.MainPlayer != null)
		{
			Singleton<ObjManager>.Instance.MainPlayer.DisactiveTargetArriveFinish();
			Singleton<ObjManager>.Instance.MainPlayer.StopMove();
		}
	}

	public void FinishReconnecting()
	{
		mReConnectingFlag = false;
	}

	public static void SetConnectDelegate(ConnectDelegate func)
	{
		mConnectDel = func;
	}

	public static void SetConnectLostDelegate(ConnectLostDelegate func)
	{
		mConnectLostDel = func;
	}

	public void Ping(PingComplete complete)
	{
		if (!mSocket.IsValid || !mSocket.IsConnected)
		{
			complete?.Invoke(-1L);
			return;
		}
		mPingComplete = complete;
		connectThread = new Thread(_Ping);
		connectThread.Start(this);
	}

	public void IntenralPing()
	{
		pingTime = -2L;
		pingTime = mSocket.Ping();
	}

	protected static void _Ping(object obj)
	{
		NetLogic netLogic = obj as NetLogic;
		netLogic.IntenralPing();
	}

	public void ConnectToServer(string serIP, int serPort, int sleepTime)
	{
		if (mConnectStatus != CONNECT_STATUS.CONNECTING)
		{
			GameManager.OnLineState = false;
			serverIp = serIP;
			serverPort = serPort;
			connectSleepTime = sleepTime;
			connectThread = new Thread(_ConnectThread);
			connectThread.Start(this);
			curConnectTimeOut = MAX_CONNECT_TIME;
		}
	}

	public void ReConnectToServer()
	{
		if (mConnectStatus != CONNECT_STATUS.CONNECTING)
		{
			GameManager.OnLineState = false;
			if (mSocketReciveStream != null)
			{
				mSocketReciveStream.Clean();
			}
			if (mSocketSendStream != null)
			{
				mSocketSendStream.Clean();
			}
			connectThread = new Thread(_ConnectThread);
			connectThread.Start(this);
			curConnectTimeOut = MAX_CONNECT_TIME;
		}
	}

	~NetLogic()
	{
	}

	public void ConnectThread()
	{
		mConnectStatus = CONNECT_STATUS.CONNECTING;
		mSocket.Close();
		mSocket.Connect(serverIp, serverPort);
		if (mSocket.IsValid)
		{
			mSocketReciveStream = new SocketReciveStream(mSocket);
			mSocketSendStream = new SocketSendStream(mSocket);
			mConnectStatus = CONNECT_STATUS.CONNRCTED;
		}
		else
		{
			Debug.LogWarning("!!!Connect erro!!!");
			mSocket.Close();
			mConnectStatus = CONNECT_STATUS.DISCONNECTED;
		}
		mConnectFinish = true;
	}

	protected static void _ConnectThread(object obj)
	{
		NetLogic netLogic = obj as NetLogic;
		netLogic.ConnectThread();
	}

	public void ConnectLost()
	{
		mConnectStatus = CONNECT_STATUS.DISCONNECTED;
		GameManager.OnLineState = false;
		if (mConnectLostDel != null)
		{
			mConnectLostDel();
		}
	}

	public void DisconnectServer()
	{
		mSocket.Close();
		mConnectStatus = CONNECT_STATUS.DISCONNECTED;
	}

	public void TestDisconnectServer()
	{
		SendLast();
		mSocket.Close();
		mConnectStatus = CONNECT_STATUS.DISCONNECTED;
		ConnectLost();
	}

	public void Send<T>(SprotoTypeBase rpcReq = null, RpcRspHandler rpcRspHandler = null)
	{
		if (!mReConnectingFlag)
		{
			NetSender.SendInternal<T>(rpcReq, rpcRspHandler);
			return;
		}
		int num = protocol[typeof(T)];
		if (num == 218 || num == 234)
		{
			NetSender.SendInternal<T>(rpcReq, rpcRspHandler);
		}
	}

	public void SendInternal<T>(SprotoTypeBase rpc = null, long? session = null)
	{
		SendInternal(rpc, session, protocol[typeof(T)]);
	}

	public void Update()
	{
		if (mConnectStatus == CONNECT_STATUS.CONNECTING)
		{
			curConnectTimeOut -= Time.deltaTime;
			if (curConnectTimeOut <= 0f)
			{
				connectThread.Abort();
				mConnectStatus = CONNECT_STATUS.DISCONNECTED;
				mConnectFinish = true;
			}
		}
		if (mConnectFinish)
		{
			if (mConnectDel != null)
			{
				mConnectDel(mConnectStatus == CONNECT_STATUS.CONNRCTED);
			}
			mConnectFinish = false;
		}
		if (pingTime != -2)
		{
			if (mPingComplete != null)
			{
				mPingComplete(pingTime);
			}
			pingTime = -2L;
		}
		ProcessNet();
	}

	public void SendLast()
	{
		if (mSocket.IsValid && mSocket.IsConnected)
		{
			ProcessSend();
		}
	}

	public void ProcessNet()
	{
		if (mSocket.IsValid && mSocket.IsConnected && ProcessSend() && ProcessRecive())
		{
			ProcessPack();
		}
	}

	private void SendInternal(SprotoTypeBase rpc, long? session, int? tag = null)
	{
		if (mSocketSendStream == null || mConnectStatus != CONNECT_STATUS.CONNRCTED)
		{
			return;
		}
		sendPkg.clear();
		if (tag.HasValue)
		{
			sendPkg.type = tag.Value;
		}
		if (session.HasValue)
		{
			sendPkg.session = session.Value;
			if (tag.HasValue)
			{
				sessionDict.Add(session.Value, protocol[tag.Value].Response.Value);
			}
		}
		sendStream.Seek(0, SeekOrigin.Begin);
		int len = sendPkg.encode(sendStream);
		if (rpc != null)
		{
			len += rpc.encode(sendStream);
		}
		byte[] buff = sendPack.pack(sendStream.Buffer, mMaxSendOnePackbytes, ref len);
		headDataBuff[0] = (byte)(len >> 8);
		headDataBuff[1] = (byte)len;
		uint buffLen = mSocketSendStream.GetBuffLen();
		mSocketSendStream.Write(headDataBuff, 2u);
		mSocketSendStream.Write(buff, (uint)len);
		uint buffLen2 = mSocketSendStream.GetBuffLen();
		if (buffLen < buffLen2)
		{
			if (nSendCount < 0)
			{
				nSendCount = 0;
			}
			nSendCount += (int)(buffLen2 - buffLen);
		}
	}

	private bool ProcessSend()
	{
		if (mSocketSendStream == null)
		{
			return false;
		}
		if (!mSocket.IsCanSend())
		{
			return true;
		}
		uint num = mSocketSendStream.Send();
		if (num == uint.MaxValue)
		{
			mSocket.Close();
			ConnectLost();
			return false;
		}
		return true;
	}

	private bool ProcessRecive()
	{
		if (mSocketReciveStream == null)
		{
			return false;
		}
		if (!mSocket.IsCanReceive())
		{
			return true;
		}
		uint buffLen = mSocketReciveStream.GetBuffLen();
		uint num = mSocketReciveStream.Recive();
		uint buffLen2 = mSocketReciveStream.GetBuffLen();
		if (num == uint.MaxValue)
		{
			mSocket.Close();
			ConnectLost();
			return false;
		}
		if (buffLen < buffLen2)
		{
			if (nReceiveCount < 0)
			{
				nReceiveCount = 0;
			}
			nReceiveCount += (int)(buffLen2 - buffLen);
		}
		return true;
	}

	private void ProcessPack()
	{
		if (mSocketReciveStream == null)
		{
			return;
		}
		nProcesspackCount = 10;
		while (isCanProcessPack && nProcesspackCount-- > 0 && mSocketReciveStream.Peek(headDataBuff, 2u))
		{
			int len = (headDataBuff[0] << 8) | headDataBuff[1];
			if (len <= 0 || mSocketReciveStream.GetBuffLen() < len + 2)
			{
				break;
			}
			mSocketReciveStream.Skip(2u);
			if ((float)len > (float)mMaxRevOnePackCount * 0.5f)
			{
				mMaxRevOnePackCount = len * 2;
				mMaxRevOnePackbytes = new byte[mMaxRevOnePackCount];
				Log.DEBUG_MSG("Pack is Max=" + len);
			}
			mSocketReciveStream.Read(mMaxRevOnePackbytes, (uint)len);
			pkg.clear();
			byte[] buffer = recvPack.unpack(mMaxRevOnePackbytes, ref len);
			int offset = pkg.init(buffer, 0, len);
			if (pkg.HasType)
			{
				int tag = (int)pkg.type;
				RpcReqHandler handler = NetReceiver.GetHandler(tag);
				if (handler != null)
				{
					SprotoTypeBase rpc = handler(protocol.GenRequest(tag, buffer, offset, len));
					if (pkg.HasSession)
					{
						int num = (int)pkg.session;
						SendInternal(rpc, num);
					}
				}
			}
			else if (pkg.HasSession)
			{
				int num2 = (int)pkg.session;
				RpcRspHandler handler2 = NetSender.GetHandler(num2);
				if (handler2 != null)
				{
					sessionDict.TryGetValue(num2, out var value);
					handler2(value(buffer, offset, len));
				}
			}
		}
	}

	public static T GetSprotoInstance<T>() where T : SprotoTypeBase
	{
		Type typeFromHandle = typeof(T);
		if (!cacheDict.TryGetValue(typeFromHandle, out var value))
		{
			value = Activator.CreateInstance<T>();
			cacheDict.Add(typeFromHandle, value);
		}
		value.clear();
		return (T)value;
	}
}
