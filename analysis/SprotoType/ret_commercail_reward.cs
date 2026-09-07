using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class ret_commercail_reward
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 8;

		private List<item> _items;

		private long _type;

		private long _parm1;

		private string _parm2;

		private string _productId;

		private string _token;

		private string _payload;

		private long _state;

		public List<item> items
		{
			get
			{
				return _items;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_items = value;
			}
		}

		public bool HasItems => has_field.has_field(0);

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(1);

		public long parm1
		{
			get
			{
				return _parm1;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_parm1 = value;
			}
		}

		public bool HasParm1 => has_field.has_field(2);

		public string parm2
		{
			get
			{
				return _parm2;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_parm2 = value;
			}
		}

		public bool HasParm2 => has_field.has_field(3);

		public string productId
		{
			get
			{
				return _productId;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_productId = value;
			}
		}

		public bool HasProductId => has_field.has_field(4);

		public string token
		{
			get
			{
				return _token;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_token = value;
			}
		}

		public bool HasToken => has_field.has_field(5);

		public string payload
		{
			get
			{
				return _payload;
			}
			set
			{
				has_field.set_field(6, is_has: true);
				_payload = value;
			}
		}

		public bool HasPayload => has_field.has_field(6);

		public long state
		{
			get
			{
				return _state;
			}
			set
			{
				has_field.set_field(7, is_has: true);
				_state = value;
			}
		}

		public bool HasState => has_field.has_field(7);

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
					items = deserialize.read_obj_list<item>();
					break;
				case 1:
					type = deserialize.read_integer();
					break;
				case 2:
					parm1 = deserialize.read_integer();
					break;
				case 3:
					parm2 = deserialize.read_string();
					break;
				case 4:
					productId = deserialize.read_string();
					break;
				case 5:
					token = deserialize.read_string();
					break;
				case 6:
					payload = deserialize.read_string();
					break;
				case 7:
					state = deserialize.read_integer();
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
				serialize.write_obj(items, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_integer(type, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_integer(parm1, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_string(parm2, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_string(productId, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_string(token, 5);
			}
			if (has_field.has_field(6))
			{
				serialize.write_string(payload, 6);
			}
			if (has_field.has_field(7))
			{
				serialize.write_integer(state, 7);
			}
			return serialize.close();
		}
	}
}
