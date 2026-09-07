using Sproto;

namespace SprotoType;

public class facebook_link
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 6;

		private string _facebook_id;

		private string _facebook_token;

		private string _id;

		private string _key;

		private long _confirm;

		private long _bindType;

		public string facebook_id
		{
			get
			{
				return _facebook_id;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_facebook_id = value;
			}
		}

		public bool HasFacebook_id => has_field.has_field(0);

		public string facebook_token
		{
			get
			{
				return _facebook_token;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_facebook_token = value;
			}
		}

		public bool HasFacebook_token => has_field.has_field(1);

		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(2);

		public string key
		{
			get
			{
				return _key;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_key = value;
			}
		}

		public bool HasKey => has_field.has_field(3);

		public long confirm
		{
			get
			{
				return _confirm;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_confirm = value;
			}
		}

		public bool HasConfirm => has_field.has_field(4);

		public long bindType
		{
			get
			{
				return _bindType;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_bindType = value;
			}
		}

		public bool HasBindType => has_field.has_field(5);

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
					facebook_id = deserialize.read_string();
					break;
				case 1:
					facebook_token = deserialize.read_string();
					break;
				case 2:
					id = deserialize.read_string();
					break;
				case 3:
					key = deserialize.read_string();
					break;
				case 4:
					confirm = deserialize.read_integer();
					break;
				case 5:
					bindType = deserialize.read_integer();
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
				serialize.write_string(facebook_id, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(facebook_token, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(id, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_string(key, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(confirm, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_integer(bindType, 5);
			}
			return serialize.close();
		}
	}

	public class response : SprotoTypeBase
	{
		private static int max_field_count = 4;

		private long _state;

		private string _id;

		private string _key;

		private long _bindType;

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

		public string key
		{
			get
			{
				return _key;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_key = value;
			}
		}

		public bool HasKey => has_field.has_field(2);

		public long bindType
		{
			get
			{
				return _bindType;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_bindType = value;
			}
		}

		public bool HasBindType => has_field.has_field(3);

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
					id = deserialize.read_string();
					break;
				case 2:
					key = deserialize.read_string();
					break;
				case 3:
					bindType = deserialize.read_integer();
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
				serialize.write_string(id, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_string(key, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(bindType, 3);
			}
			return serialize.close();
		}
	}
}
