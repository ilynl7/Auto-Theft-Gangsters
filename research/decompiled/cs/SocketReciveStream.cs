using System;

public class SocketReciveStream
{
	public const uint DEFAULT_SOCKET_RECIVE_BUFFER_SIZE = 262144u;

	public const uint DEFAULT_SOCKET_RECIVE_BUFFER_MAX_SIZE = 524288u;

	private const uint SOCKET_ERROR = uint.MaxValue;

	private SocketInstance mSocket;

	private uint mBufferLen;

	private uint mBufferMaxLen;

	private byte[] mBuffer;

	private byte[] mTempBuffer;

	private uint mHead;

	private uint mTail;

	public SocketReciveStream(SocketInstance socket, uint bufferLen = 262144u, uint buffMaxLen = 524288u)
	{
		mSocket = socket;
		mBufferLen = bufferLen;
		mBufferMaxLen = buffMaxLen;
		mHead = 0u;
		mTail = 0u;
		mBuffer = new byte[mBufferLen];
		mTempBuffer = new byte[mBufferLen];
	}

	public void InitStream(uint bufferLen = 262144u)
	{
		mHead = 0u;
		mTail = 0u;
		mBufferLen = bufferLen;
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

	public uint Read(byte[] buff, uint len)
	{
		if (len > GetBuffLen())
		{
			return 0u;
		}
		for (int i = 0; i < len; i++)
		{
			buff[i] = mBuffer[mHead++];
			if (mHead >= mBufferLen)
			{
				mHead -= mBufferLen;
			}
		}
		return len;
	}

	public bool Peek(byte[] buff, uint len)
	{
		if (len == 0)
		{
			return false;
		}
		if (len > GetBuffLen())
		{
			return false;
		}
		uint num = mHead;
		for (int i = 0; i < len; i++)
		{
			buff[i] = mBuffer[num++];
			if (num >= mBufferLen)
			{
				num -= mBufferLen;
			}
		}
		return true;
	}

	public bool Skip(uint len)
	{
		if (len == 0)
		{
			return false;
		}
		if (len > GetBuffLen())
		{
			return false;
		}
		mHead += len;
		if (mHead >= mBufferLen)
		{
			mHead -= mBufferLen;
		}
		return true;
	}

	public uint Recive()
	{
		if (mSocket == null)
		{
			return 0u;
		}
		uint num = 0u;
		uint num2 = 0u;
		try
		{
			uint buffFree = GetBuffFree();
			if (buffFree != 0)
			{
				num2 = mSocket.Recv(mTempBuffer, (int)buffFree, 0u);
				if (num2 == uint.MaxValue)
				{
					return uint.MaxValue;
				}
				for (int i = 0; i < num2; i++)
				{
					mBuffer[mTail++] = mTempBuffer[i];
					if (mTail >= mBufferLen)
					{
						mTail -= mBufferLen;
					}
				}
				num += num2;
			}
			if (num2 == buffFree)
			{
				uint num3 = mSocket.Avaiable();
				if (num3 != 0)
				{
					if (!ExtendSize(num3 + 1))
					{
						InitStream();
						return uint.MaxValue;
					}
					num2 = mSocket.Recv(mTempBuffer, (int)num3, 0u);
					if (num2 == uint.MaxValue)
					{
						return uint.MaxValue;
					}
					for (int j = 0; j < num2; j++)
					{
						mBuffer[mTail++] = mTempBuffer[j];
						if (mTail >= mBufferLen)
						{
							mTail -= mBufferLen;
						}
					}
					num += num2;
				}
			}
		}
		catch (Exception)
		{
		}
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

	public uint GetBuffFree()
	{
		if (mHead <= mTail)
		{
			return mBufferLen - mTail + mHead - 1;
		}
		if (mHead > mTail)
		{
			return mHead - mTail - 1;
		}
		return 0u;
	}

	private bool ExtendSize(uint size)
	{
		uint num = Math.Max(mBufferLen, size) + mBufferLen;
		if (num >= mBufferMaxLen)
		{
			return false;
		}
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
