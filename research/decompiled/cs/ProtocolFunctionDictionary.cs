using System;
using System.Collections.Generic;

namespace Sproto;

public class ProtocolFunctionDictionary
{
	public class MetaInfo
	{
		public Type ProtocolType;

		public KeyValuePair<Type, typeFunc> Request;

		public KeyValuePair<Type, typeFunc> Response;
	}

	public delegate SprotoTypeBase typeFunc(byte[] buffer, int offset, int len);

	private Dictionary<int, MetaInfo> MetaDictionary;

	private Dictionary<Type, int> ProtocolDictionary;

	public MetaInfo this[int tag] => MetaDictionary[tag];

	public int this[Type protocolType] => ProtocolDictionary[protocolType];

	public ProtocolFunctionDictionary()
	{
		MetaDictionary = new Dictionary<int, MetaInfo>();
		ProtocolDictionary = new Dictionary<Type, int>();
	}

	private MetaInfo _getMeta(int tag)
	{
		if (!MetaDictionary.TryGetValue(tag, out var value))
		{
			value = new MetaInfo();
			MetaDictionary.Add(tag, value);
		}
		return value;
	}

	public void SetProtocol<ProtocolType>(int tag)
	{
		MetaInfo metaInfo = _getMeta(tag);
		metaInfo.ProtocolType = typeof(ProtocolType);
		ProtocolDictionary.Add(metaInfo.ProtocolType, tag);
	}

	public void SetRequest<T>(int tag) where T : SprotoTypeBase, new()
	{
		MetaInfo metaInfo = _getMeta(tag);
		_set<T>(tag, out metaInfo.Request);
	}

	public void SetResponse<T>(int tag) where T : SprotoTypeBase, new()
	{
		MetaInfo metaInfo = _getMeta(tag);
		_set<T>(tag, out metaInfo.Response);
	}

	private void _set<T>(int tag, out KeyValuePair<Type, typeFunc> field) where T : SprotoTypeBase, new()
	{
		typeFunc value = delegate(byte[] buffer, int offset, int len)
		{
			T result = new T();
			result.init(buffer, offset, len);
			return result;
		};
		field = new KeyValuePair<Type, typeFunc>(typeof(T), value);
	}

	private SprotoTypeBase _gen(KeyValuePair<Type, typeFunc> field, int tag, byte[] buffer, int offset = 0, int len = 0)
	{
		if (field.Value != null)
		{
			return field.Value(buffer, offset, len);
		}
		return null;
	}

	public SprotoTypeBase GenResponse(int tag, byte[] buffer, int offset = 0, int len = 0)
	{
		MetaInfo metaInfo = MetaDictionary[tag];
		return _gen(metaInfo.Response, tag, buffer, offset, len);
	}

	public SprotoTypeBase GenRequest(int tag, byte[] buffer, int offset = 0, int len = 0)
	{
		MetaInfo metaInfo = MetaDictionary[tag];
		return _gen(metaInfo.Request, tag, buffer, offset, len);
	}
}
