using System;

public class SocketSendStream
{
	public const uint DEFAULT_SOCKET_SEND_BUFFER_SIZE = 8192u;

	public const uint DEFAULT_SOCKET_SEND_BUFFER_MAX_SIZE = 20480u;

	private const uint SOCKET_ERROR = uint.MaxValue;

	private SocketInstance mSocket;

	private uint mBufferLen;

	private uint mBufferMaxLen;

	private byte[] mBuffer;

	private byte[] mTempBuffer;

	private uint mHead;

	private uint mTail;

	public SocketSendStream(SocketInstance socket, uint bufferLen = 8192u, uint buffMaxLen = 20480u)
	{
		mSocket = socket;
		mBufferLen = bufferLen;
		mBufferMaxLen = buffMaxLen;
		mHead = 0u;
		mTail = 0u;
		mBuffer = new byte[mBufferLen];
		mTempBuffer = new byte[mBufferLen];
	}

	public void Clean()
	{
		mBuffer = null;
		mTempBuffer = null;
		mHead = 0u;
		mTail = 0u;
	}

	public uint Write(byte[] buff, uint len)
	{
		uint num = ((mHead > mTail) ? (mHead - mTail - 1) : (mBufferLen - mTail + mHead - 1));
		if (len >= num && !ExtendSize(len - num + 1))
		{
			mHead = 0u;
			mTail = 0u;
			Log.WARING_MSG("Send pack is Max!!!!!");
			return 0u;
		}
		for (int i = 0; i < len; i++)
		{
			mBuffer[mTail++] = buff[i];
			if (mTail >= mBufferLen)
			{
				mTail -= mBufferLen;
			}
		}
		return len;
	}

	public uint Send()
	{
		if (mSocket == null)
		{
			return 0u;
		}
		uint num = 0u;
		uint num2 = 0u;
		try
		{
			for (uint buffLen = GetBuffLen(); buffLen != 0; buffLen = GetBuffLen())
			{
				uint num3 = mHead;
				for (int i = 0; i < buffLen; i++)
				{
					mTempBuffer[i] = mBuffer[num3++];
					if (num3 >= mBufferLen)
					{
						num3 -= mBufferLen;
					}
				}
				num2 = mSocket.Send(mTempBuffer, (int)buffLen);
				if (num2 == uint.MaxValue)
				{
					return uint.MaxValue;
				}
				num += num2;
				mHead += num2;
				if (mHead >= mBufferLen)
				{
					mHead -= mBufferLen;
				}
			}
		}
		catch (Exception)
		{
		}
		mHead = (mTail = 0u);
		return num;
	}

	public uint GetBuffLen()
	{
		if (mHead < mTail)
		{
			return mTail - mHead;
		}
		if (mHead > mTail)
		{
			return mBufferLen - mHead + mTail;
		}
		return 0u;
	}

	private bool ExtendSize(uint size)
	{
		if (size >= mBufferLen)
		{
			return false;
		}
		uint num = mBufferLen >> 1;
		byte[] array = new byte[num];
		mTempBuffer = new byte[num];
		uint buffLen = GetBuffLen();
		if (array == null || mTempBuffer == null)
		{
			return false;
		}
		if (mHead < mTail)
		{
			for (int i = 0; i < mTail - mHead - 1; i++)
			{
				array[i] = mBuffer[mHead + i];
			}
		}
		else
		{
			for (int j = 0; j < mBufferLen - mHead; j++)
			{
				array[j] = mBuffer[mHead + j];
			}
			for (int k = 0; k < mTail; k++)
			{
				array[mBufferLen - mHead + k] = mBuffer[k];
			}
		}
		mBuffer = array;
		mBufferLen = num;
		mHead = 0u;
		mTail = buffLen;
		return true;
	}
}
