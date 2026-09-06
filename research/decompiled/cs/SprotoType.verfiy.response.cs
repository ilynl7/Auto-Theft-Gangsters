using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class verfiy
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 3;

		private string _id;

		private string _key;

		private string _versionCode;

		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(0);

		public string key
		{
			get
			{
				return _key;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_key = value;
			}
		}

		public bool HasKey => has_field.has_field(1);

		public string versionCode
		{
			get
			{
				return _versionCode;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_versionCode = value;
			}
		}

		public bool HasVersionCode => has_field.has_field(2);

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
					id = deserialize.read_string();
					break;
				case 1:
					key = deserialize.read_string();
					break;
				case 2:
					versionCode = deserialize.read_string();
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
				serialize.write_string(id, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(key, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(versionCode, 2);
			}
			return serialize.close();
		}
	}

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 12;

		private long _state;

		private long _session;

		private List<game_server> _game_server;

		private string _user_server;

		private long _facebook_bind;

		private string _versionCode;

		private string _dataVersionCode;

		private long _downloadFlag;

		private string _notice;

		private string _notice_version;

		private string _facebook_bind1;

		private string _google_bind;

		public long state
		{
			get
			{
				return _state;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_state = value;
			}
		}

		public bool HasState => has_field.has_field(0);

		public long session
		{
			get
			{
				return _session;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_session = value;
			}
		}

		public bool HasSession => has_field.has_field(1);

		public List<game_server> game_server
		{
			get
			{
				return _game_server;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_game_server = value;
			}
		}

		public bool HasGame_server => has_field.has_field(2);

		public string user_server
		{
			get
			{
				return _user_server;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_user_server = value;
			}
		}

		public bool HasUser_server => has_field.has_field(3);

		public long facebook_bind
		{
			get
			{
				return _facebook_bind;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_facebook_bind = value;
			}
		}

		public bool HasFacebook_bind => has_field.has_field(4);

		public string versionCode
		{
			get
			{
				return _versionCode;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_versionCode = value;
			}
		}

		public bool HasVersionCode => has_field.has_field(5);

		public string dataVersionCode
		{
			get
			{
				return _dataVersionCode;
			}
			set
			{
				has_field.set_field(6, is_has: true);
				_dataVersionCode = value;
			}
		}

		public bool HasDataVersionCode => has_field.has_field(6);

		public long downloadFlag
		{
			get
			{
				return _downloadFlag;
			}
			set
			{
				has_field.set_field(7, is_has: true);
				_downloadFlag = value;
			}
		}

		public bool HasDownloadFlag => has_field.has_field(7);

		public string notice
		{
			get
			{
				return _notice;
			}
			set
			{
				has_field.set_field(8, is_has: true);
				_notice = value;
			}
		}

		public bool HasNotice => has_field.has_field(8);

		public string notice_version
		{
			get
			{
				return _notice_version;
			}
			set
			{
				has_field.set_field(9, is_has: true);
				_notice_version = value;
			}
		}

		public bool HasNotice_version => has_field.has_field(9);

		public string facebook_bind1
		{
			get
			{
				return _facebook_bind1;
			}
			set
			{
				has_field.set_field(10, is_has: true);
				_facebook_bind1 = value;
			}
		}

		public bool HasFacebook_bind1 => has_field.has_field(10);

		public string google_bind
		{
			get
			{
				return _google_bind;
			}
			set
			{
				has_field.set_field(11, is_has: true);
				_google_bind = value;
			}
		}

		public bool HasGoogle_bind => has_field.has_field(11);

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
					state = deserialize.read_integer();
					break;
				case 1:
					session = deserialize.read_integer();
					break;
				case 2:
					game_server = deserialize.read_obj_list<game_server>();
					break;
				case 3:
					user_server = deserialize.read_string();
					break;
				case 4:
					facebook_bind = deserialize.read_integer();
					break;
				case 5:
					versionCode = deserialize.read_string();
					break;
				case 6:
					dataVersionCode = deserialize.read_string();
					break;
				case 7:
					downloadFlag = deserialize.read_integer();
					break;
				case 8:
					notice = deserialize.read_string();
					break;
				case 9:
					notice_version = deserialize.read_string();
					break;
				case 10:
					facebook_bind1 = deserialize.read_string();
					break;
				case 11:
					google_bind = deserialize.read_string();
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
				serialize.write_integer(state, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(session, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_obj(game_server, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_string(user_server, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(facebook_bind, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_string(versionCode, 5);
			}
			if (has_field.has_field(6))
			{
				serialize.write_string(dataVersionCode, 6);
			}
			if (has_field.has_field(7))
			{
				serialize.write_integer(downloadFlag, 7);
			}
			if (has_field.has_field(8))
			{
				serialize.write_string(notice, 8);
			}
			if (has_field.has_field(9))
			{
				serialize.write_string(notice_version, 9);
			}
			if (has_field.has_field(10))
			{
				serialize.write_string(facebook_bind1, 10);
			}
			if (has_field.has_field(11))
			{
				serialize.write_string(google_bind, 11);
			}
			return serialize.close();
		}
	}
}
