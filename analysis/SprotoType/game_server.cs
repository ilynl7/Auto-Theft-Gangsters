using Sproto;

namespace SprotoType;

public class game_server : SprotoTypeBase
{
	private static int max_field_count = 11;

	private long _serverId;

	private string _serverName;

	private string _serverIP;

	private long _serverPort;

	private long _serverState;

	private long _serverPlayerState;

	private long _serverArea;

	private long _serverRank;

	private long _serverTimeZone;

	private long _serverWeight;

	private long _newServer;

	public long serverId
	{
		get
		{
			return _serverId;
		}
		set
		{
			has_field.set_field(0, is_has: true);
			_serverId = value;
		}
	}

	public bool HasServerId => has_field.has_field(0);

	public string serverName
	{
		get
		{
			return _serverName;
		}
		set
		{
			has_field.set_field(1, is_has: true);
			_serverName = value;
		}
	}

	public bool HasServerName => has_field.has_field(1);

	public string serverIP
	{
		get
		{
			return _serverIP;
		}
		set
		{
			has_field.set_field(2, is_has: true);
			_serverIP = value;
		}
	}

	public bool HasServerIP => has_field.has_field(2);

	public long serverPort
	{
		get
		{
			return _serverPort;
		}
		set
		{
			has_field.set_field(3, is_has: true);
			_serverPort = value;
		}
	}

	public bool HasServerPort => has_field.has_field(3);

	public long serverState
	{
		get
		{
			return _serverState;
		}
		set
		{
			has_field.set_field(4, is_has: true);
			_serverState = value;
		}
	}

	public bool HasServerState => has_field.has_field(4);

	public long serverPlayerState
	{
		get
		{
			return _serverPlayerState;
		}
		set
		{
			has_field.set_field(5, is_has: true);
			_serverPlayerState = value;
		}
	}

	public bool HasServerPlayerState => has_field.has_field(5);

	public long serverArea
	{
		get
		{
			return _serverArea;
		}
		set
		{
			has_field.set_field(6, is_has: true);
			_serverArea = value;
		}
	}

	public bool HasServerArea => has_field.has_field(6);

	public long serverRank
	{
		get
		{
			return _serverRank;
		}
		set
		{
			has_field.set_field(7, is_has: true);
			_serverRank = value;
		}
	}

	public bool HasServerRank => has_field.has_field(7);

	public long serverTimeZone
	{
		get
		{
			return _serverTimeZone;
		}
		set
		{
			has_field.set_field(8, is_has: true);
			_serverTimeZone = value;
		}
	}

	public bool HasServerTimeZone => has_field.has_field(8);

	public long serverWeight
	{
		get
		{
			return _serverWeight;
		}
		set
		{
			has_field.set_field(9, is_has: true);
			_serverWeight = value;
		}
	}

	public bool HasServerWeight => has_field.has_field(9);

	public long newServer
	{
		get
		{
			return _newServer;
		}
		set
		{
			has_field.set_field(10, is_has: true);
			_newServer = value;
		}
	}

	public bool HasNewServer => has_field.has_field(10);

	public game_server()
		: base(max_field_count)
	{
	}

	public game_server(byte[] buffer)
		: base(max_field_count, buffer)
	{
		decode();
	}

	protected override void decode()
	{
		int num = -1;
		while ((num = deserialize.read_tag()) != -1)
		{
			switch (num)
			{
			case 0:
				serverId = deserialize.read_integer();
				break;
			case 1:
				serverName = deserialize.read_string();
				break;
			case 2:
				serverIP = deserialize.read_string();
				break;
			case 3:
				serverPort = deserialize.read_integer();
				break;
			case 4:
				serverState = deserialize.read_integer();
				break;
			case 5:
				serverPlayerState = deserialize.read_integer();
				break;
			case 6:
				serverArea = deserialize.read_integer();
				break;
			case 7:
				serverRank = deserialize.read_integer();
				break;
			case 8:
				serverTimeZone = deserialize.read_integer();
				break;
			case 9:
				serverWeight = deserialize.read_integer();
				break;
			case 10:
				newServer = deserialize.read_integer();
				break;
			default:
				deserialize.read_unknow_data();
				break;
			}
		}
	}

	public override int encode(SprotoStream stream)
	{
		serialize.open(stream);
		if (has_field.has_field(0))
		{
			serialize.write_integer(serverId, 0);
		}
		if (has_field.has_field(1))
		{
			serialize.write_string(serverName, 1);
		}
		if (has_field.has_field(2))
		{
			serialize.write_string(serverIP, 2);
		}
		if (has_field.has_field(3))
		{
			serialize.write_integer(serverPort, 3);
		}
		if (has_field.has_field(4))
		{
			serialize.write_integer(serverState, 4);
		}
		if (has_field.has_field(5))
		{
			serialize.write_integer(serverPlayerState, 5);
		}
		if (has_field.has_field(6))
		{
			serialize.write_integer(serverArea, 6);
		}
		if (has_field.has_field(7))
		{
			serialize.write_integer(serverRank, 7);
		}
		if (has_field.has_field(8))
		{
			serialize.write_integer(serverTimeZone, 8);
		}
		if (has_field.has_field(9))
		{
			serialize.write_integer(serverWeight, 9);
		}
		if (has_field.has_field(10))
		{
			serialize.write_integer(newServer, 10);
		}
		return serialize.close();
	}
}
