using Sproto;

namespace SprotoType;

public class login
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 7;

		private long _session;

		private string _id;

		private long _logintype;

		private string _version;

		private string _unityVersion;

		private long _serverId;

		private long _time;

		public long session
		{
			get
			{
				return _session;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_session = value;
			}
		}

		public bool HasSession => has_field.has_field(0);

		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(1);

		public long logintype
		{
			get
			{
				return _logintype;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_logintype = value;
			}
		}

		public bool HasLogintype => has_field.has_field(2);

		public string version
		{
			get
			{
				return _version;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_version = value;
			}
		}

		public bool HasVersion => has_field.has_field(3);

		public string unityVersion
		{
			get
			{
				return _unityVersion;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_unityVersion = value;
			}
		}

		public bool HasUnityVersion => has_field.has_field(4);

		public long serverId
		{
			get
			{
				return _serverId;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_serverId = value;
			}
		}

		public bool HasServerId => has_field.has_field(5);

		public long time
		{
			get
			{
				return _time;
			}
			set
			{
				has_field.set_field(6, is_has: true);
				_time = value;
			}
		}

		public bool HasTime => has_field.has_field(6);

		public request()
			: base(max_field_count)
		{
		}

		public request(byte[] buffer)
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
					session = deserialize.read_integer();
					break;
				case 1:
					id = deserialize.read_string();
					break;
				case 2:
					logintype = deserialize.read_integer();
					break;
				case 3:
					version = deserialize.read_string();
					break;
				case 4:
					unityVersion = deserialize.read_string();
					break;
				case 5:
					serverId = deserialize.read_integer();
					break;
				case 6:
					time = deserialize.read_integer();
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
				serialize.write_integer(session, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(id, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(logintype, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_string(version, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_string(unityVersion, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(serverId, 5);
			}
			if (has_field.has_field(6))
			{
				serialize.write_integer(time, 6);
			}
			return serialize.close();
		}
	}

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _type;

		private string _versionCode;

		private string _dataVersionCode;

		private long _serverLevel;

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(0);

		public string versionCode
		{
			get
			{
				return _versionCode;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_versionCode = value;
			}
		}

		public bool HasVersionCode => has_field.has_field(1);

		public string dataVersionCode
		{
			get
			{
				return _dataVersionCode;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_dataVersionCode = value;
			}
		}

		public bool HasDataVersionCode => has_field.has_field(2);

		public long serverLevel
		{
			get
			{
				return _serverLevel;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_serverLevel = value;
			}
		}

		public bool HasServerLevel => has_field.has_field(3);

		public response()
			: base(max_field_count)
		{
		}

		public response(byte[] buffer)
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
					type = deserialize.read_integer();
					break;
				case 1:
					versionCode = deserialize.read_string();
					break;
				case 2:
					dataVersionCode = deserialize.read_string();
					break;
				case 3:
					serverLevel = deserialize.read_integer();
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
				serialize.write_integer(type, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(versionCode, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(dataVersionCode, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(serverLevel, 3);
			}
			return serialize.close();
		}
	}
}
